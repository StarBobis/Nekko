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

namespace Nekko_LCU
{
    public class LeagueClientUtils
    {
        
        /// <summary>
        /// 获取当前英雄联盟客户端的Port,Token,Server
        /// </summary>
        /// <returns></returns>
        public static LeagueClientAuthInfo GetClientInfoByWMIC()
        {
            string command = "wmic process WHERE name='LeagueClientUx.exe' GET commandline";
            string output = CommandUtils.ExecuteCmdCommand(command);

            // 使用正则表达式提取所需信息
            string port = Regex.Match(output, @"--app-port=([^""]+)").Groups[1].Value;
            string token = Regex.Match(output, @"--remoting-auth-token=([^""]+)").Groups[1].Value;
            string server = Regex.Match(output, @"--rso_platform_id=([^""]+)").Groups[1].Value;

            //Debug.WriteLine($"Port: {port}");
            //Debug.WriteLine($"Token: {token}");
            //Debug.WriteLine($"Server: {server}");

            //输出案例数据：
            //Port: 59746
            //Token: oW58i5VQ_LH_yoNTJrb - eA
            //Server: TJ101

            LeagueClientAuthInfo authInfo = new LeagueClientAuthInfo();
            authInfo.Port = port;
            authInfo.Token = token;
            authInfo.Server = server;
            return authInfo;
        }

        /// <summary>
        /// 获取当前英雄联盟客户端的进程pid
        /// </summary>
        /// <returns></returns>
        public static string GetClientPid()
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
            LeagueClientAuthInfo authInfo = LeagueClientUtils.GetClientInfoByWMIC();

            using (var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    // 忽略所有证书验证错误
                    return true;
                }
            })

            using (HttpClient client = new HttpClient(handler))
            {
                // 设置基本认证
                string credentials = $"riot:{authInfo.Token}";
                string base64Credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

                // 构建URL
                string url = $"https://127.0.0.1:{authInfo.Port}/lol-summoner/v1/current-summoner";
                HttpResponseMessage response = await client.GetAsync(url);

                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 读取并解析响应内容
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseBody);

                SummonerInfo summonerInfo = new SummonerInfo(responseBody);
                return summonerInfo;
            }

        }

        /// <summary>
        /// 通过Puuid查询同个大区的召唤师信息
        /// </summary>
        /// <param name="Puuid"></param>
        /// <returns></returns>
        public static async Task<SummonerInfo> GetSummonerInfoByPuuid(string Puuid)
        {
            LeagueClientAuthInfo authInfo = LeagueClientUtils.GetClientInfoByWMIC();

            using (var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    // 忽略所有证书验证错误
                    return true;
                }
            })

            using (HttpClient client = new HttpClient(handler))
            {
                // 设置基本认证
                string credentials = $"riot:{authInfo.Token}";
                string base64Credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

                // 构建URL
                string url = $"https://127.0.0.1:{authInfo.Port}/lol-summoner/v2/summoners/puuid/{Puuid}";
                HttpResponseMessage response = await client.GetAsync(url);

                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 读取并解析响应内容
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseBody);

                SummonerInfo summonerInfo = new SummonerInfo(responseBody);
                return summonerInfo;
            }

        }


        /// <summary>
        /// 根据召唤师的Puuid查询游戏记录
        /// </summary>
        /// <param name="Puuid"></param>
        /// <returns></returns>
        public static async Task<GameRecord> GetSummonerGameRecordByPuuid(string Puuid)
        {
            using (var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    // 忽略所有证书验证错误
                    return true;
                }
            })

            using (HttpClient client = new HttpClient(handler))
            {
                LeagueClientAuthInfo authInfo = LeagueClientUtils.GetClientInfoByWMIC();

                // 假设你已经通过其他方式获得了Token和Port
                string token = authInfo.Token;
                string port = authInfo.Port; // 替换为实际的端口号

                // 设置基本地址和认证信息
                client.BaseAddress = new Uri($"https://127.0.0.1:{port}/");
                var authTokenBytes = Encoding.ASCII.GetBytes($"riot:{token}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authTokenBytes));

                // 禁用SSL证书验证（仅用于开发环境）
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                // 获取当前召唤师的信息以获得PUUID

                // 假设我们从召唤师信息中提取了PUUID
                // 使用PUUID查询历史战绩
                HttpResponseMessage historyResponse = await client.GetAsync($"/lol-match-history/v1/products/lol/{Puuid}/matches");
                if (historyResponse.IsSuccessStatusCode)
                {
                    var historyJson = await historyResponse.Content.ReadAsStringAsync();
                    GameRecord gameRecord = new GameRecord(historyJson);
                    Debug.WriteLine(historyJson); // 输出历史战绩JSON字符串

                    return gameRecord;
                }
                else
                {
                    GameRecord gameRecord = new GameRecord();
                    Debug.WriteLine("Failed to get match history.");
                    return gameRecord;

                }
            }

        }


        public static async Task<GameRecord> GetCurrentSummonerGameRecord()
        {
            SummonerInfo summonerInfo = await LeagueClientUtils.GetCurrentSummonerInfo();
            return await GetSummonerGameRecordByPuuid(summonerInfo.Puuid);
        }

    }
}
