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

namespace Nekko_LCU
{
    public class LeagueClientUtils
    {
        
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
    }
}
