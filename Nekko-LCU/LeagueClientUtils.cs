using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nekko_Core;
using Nekko_LCU;
using System.Xml.Linq;
using System.Management;
using System.Text.Json;


namespace Nekko_LCU
{
    public static class LeagueClientUtils
    {

        public static LeagueClientAuthInfo leagueClientAuthInfo {get; set;} = new LeagueClientAuthInfo();
        public static HttpClient LeagueHttpClient { get; set; } = null;

        public static LeagueClientAuthInfo GetClientInfoByWMI(bool forceRefresh = false)
        {
            if (!forceRefresh && leagueClientAuthInfo != null && !string.IsNullOrEmpty(leagueClientAuthInfo.Port))
            {
                Debug.WriteLine("读取缓存的LeagueClientAutoInfo");
                return leagueClientAuthInfo;
            }

            Debug.WriteLine("读取新的LeagueClientAutoInfo");

            LeagueClientAuthInfo authInfo = new LeagueClientAuthInfo();
            var query = new SelectQuery(@"SELECT CommandLine FROM Win32_Process WHERE Name = 'LeagueClientUx.exe'");

            using (var searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    string commandLine = mo["CommandLine"]?.ToString();
                    if (string.IsNullOrEmpty(commandLine)) continue;

                    // 使用更健壮的正则表达式匹配参数
                    var portMatch = Regex.Match(commandLine, @"--app-port=([\""]?)(\d+)\1");
                    var tokenMatch = Regex.Match(commandLine, @"--remoting-auth-token=([\""]?)([\w-]+)\1");
                    var serverMatch = Regex.Match(commandLine, @"--rso_platform_id=([\""]?)(\w+)\1");

                    if (portMatch.Success) authInfo.Port = portMatch.Groups[2].Value;
                    if (tokenMatch.Success) authInfo.Token = tokenMatch.Groups[2].Value;
                    if (serverMatch.Success) authInfo.Server = serverMatch.Groups[2].Value;

                    // 找到第一个有效进程后立即退出
                    if (!string.IsNullOrEmpty(authInfo.Port)) break;
                }
            }

            leagueClientAuthInfo = authInfo;
            return authInfo;
        }

        /// <summary>
        /// 获取当前英雄联盟客户端的Port,Token,Server
        /// </summary>
        /// <returns></returns>
        [Obsolete("此方法已过时，因为部分电脑无法使用WMIC，且微软已停止支持WMIC，而是支持WMI，但是留作参考")]
        public static LeagueClientAuthInfo GetClientInfoByWMIC(bool forceRefresh = false)
        {
            if (!forceRefresh)
            {
                if (leagueClientAuthInfo.Port != null && leagueClientAuthInfo.Port != "")
                {
                    Debug.WriteLine("读取缓存的LeagueClientAutoInfo");
                    return leagueClientAuthInfo;
                }
            }


            Debug.WriteLine("读取新的LeagueClientAutoInfo");

            string command = "wmic process WHERE name='LeagueClientUx.exe' GET commandline";
            string output = CommandUtils.ExecuteCmdCommand(command);


            // 使用正则表达式提取所需信息
            string port = Regex.Match(output, @"--app-port=([^""]+)").Groups[1].Value;
            string token = Regex.Match(output, @"--remoting-auth-token=([^""]+)").Groups[1].Value;
            string server = Regex.Match(output, @"--rso_platform_id=([^""]+)").Groups[1].Value;

            //Debug.WriteLine($"Port: {port}"); //Port: 59746 
            //Debug.WriteLine($"Token: {token}"); //Token: oW58i5VQ_LH_yoNTJrb - eA
            //Debug.WriteLine($"Server: {server}"); //Server: TJ101
            LeagueClientAuthInfo authInfo = new LeagueClientAuthInfo();
            authInfo.Port = port;
            authInfo.Token = token;
            authInfo.Server = server;

            leagueClientAuthInfo = authInfo;
            return authInfo;
        }

        public static HttpClient GetNewRiotAuthClient(bool forceRefresh = false)
        {
            //获取英雄联盟校验信息
            LeagueClientAuthInfo authInfo = LeagueClientUtils.GetClientInfoByWMI(forceRefresh);

            //忽略所有证书错误
            HttpClientHandler errorHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    // 忽略所有证书验证错误
                    return true;
                }
            };

            //创建新的HttpClient
            HttpClient client = new HttpClient(errorHandler);
            client.BaseAddress = new Uri($"https://127.0.0.1:{authInfo.Port}/");
            var authTokenBytes = Encoding.ASCII.GetBytes($"riot:{authInfo.Token}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authTokenBytes));


            return client;
        }


        public static HttpClient GetRiotAuthClient(bool forceRefresh = false)
        {
            if (!forceRefresh)
            {
                if (LeagueHttpClient == null)
                {
                    LeagueHttpClient = GetNewRiotAuthClient();
                }
            }
            else
            {
                LeagueHttpClient = GetNewRiotAuthClient();
            }

            return LeagueHttpClient;
        }

        


        /// <summary>
        /// 获取当前英雄联盟客户端的进程pid
        /// </summary>
        /// <returns></returns>
        public static string GetLeagueClientPid()
        {
            
            string processName = "LeagueClientUx"; // 进程名称，不包含.exe扩展名
            int[] pids = ProcessUtils.GetProcessIdsByName(processName);
           
            if (pids.Length > 0)
            {
                Debug.WriteLine($"Found {pids.Length} process(es) named '{processName}'");
                foreach (int pid in pids)
                {
                    Debug.WriteLine($"Process ID: {pid}");
                }
                return pids[0].ToString();
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 获取当前客户端的召唤师信息
        /// SummonerInfo summonerInfo = await LeagueClientUtils.GetCurrentSummonerInfo();
        /// </summary>
        /// <returns></returns>
        public static async Task<SummonerInfo> GetCurrentSummonerInfo()
        {
            HttpClient client = GetRiotAuthClient();
            HttpResponseMessage response = await client.GetAsync($"/lol-summoner/v1/current-summoner");
            string responseBody = await response.Content.ReadAsStringAsync();

            SummonerInfo summonerInfo = new SummonerInfo(responseBody);
            return summonerInfo;
        }

        /// <summary>
        /// 根据召唤师名字和#编号 来查找召唤师信息
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public static async Task<SummonerInfo> GetSummonerInfoByNameTag(string Name, string Tag)
        {
            HttpClient client = GetRiotAuthClient();
            HttpResponseMessage response = await client.GetAsync($"/lol-summoner/v1/summoners?name={Name}%23{Tag}");
            string responseBody = await response.Content.ReadAsStringAsync();

            SummonerInfo summonerInfo = new SummonerInfo(responseBody);
            return summonerInfo;
        }

        /// <summary>
        /// 通过Puuid查询同个大区的召唤师信息
        /// </summary>
        /// <param name="Puuid"></param>
        /// <returns></returns>
        public static async Task<SummonerInfo> GetSummonerInfoByPuuid(string Puuid)
        {
            HttpClient client = GetRiotAuthClient();
            HttpResponseMessage response = await client.GetAsync($"/lol-summoner/v2/summoners/puuid/{Puuid}");
            string responseBody = await response.Content.ReadAsStringAsync();

            SummonerInfo summonerInfo = new SummonerInfo(responseBody);
            return summonerInfo;
        }


        /// <summary>
        /// 根据召唤师的Puuid查询游戏记录
        /// </summary>
        /// <param name="Puuid"></param>
        /// <returns></returns>
        public static async Task<GameRecord> GetSummonerGameRecordByPuuid(string Puuid,int beginIndex, int endIndex)
        {
            HttpClient client = GetRiotAuthClient();

            // 使用PUUID查询历史战绩
            HttpResponseMessage historyResponse = await client.GetAsync($"/lol-match-history/v1/products/lol/{Puuid}/matches?begIndex={beginIndex}&endIndex={endIndex}" );
            var historyJson = await historyResponse.Content.ReadAsStringAsync();
            TimerUtils.Start("CreateGameRecord");
            GameRecord gameRecord = new GameRecord(historyJson);
            TimerUtils.End("CreateGameRecord");

            return gameRecord;
        }

        public static async Task<GameObject> GetGameInfoByGameId(string gameId)
        {
            HttpClient client = GetRiotAuthClient();

            // 使用PUUID查询历史战绩
            HttpResponseMessage historyResponse = await client.GetAsync($"/lol-match-history/v1/games/{gameId}");

            if (historyResponse.IsSuccessStatusCode)
            {
                var resultJson = await historyResponse.Content.ReadAsStringAsync();

                GameObject gameRecord = new GameObject(resultJson);
                return gameRecord;
            }
            else
            {
                GameObject gameRecord = new GameObject();
                
                Debug.WriteLine("Failed to get match history.");
                return gameRecord;
            }

        }

        /// <summary>
        /// 获取当前选人阶段信息，也只有在英雄选择阶段这个API有用，进游戏之后也查不到了
        /// </summary>
        /// <returns></returns>
        public static async Task<ChampionSelect> GetChampionSelectInfo()
        {
            HttpClient client = GetRiotAuthClient();

            HttpResponseMessage response = await client.GetAsync($"/lol-champ-select/v1/session");
            string jsonResponse = await response.Content.ReadAsStringAsync();

            ChampionSelect championSelect = new ChampionSelect(jsonResponse);
            return championSelect;
        }


    }
}
