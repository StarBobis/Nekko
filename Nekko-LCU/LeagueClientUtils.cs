using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            //Port: 59746
            //Token: oW58i5VQ_LH_yoNTJrb - eA
            //Server: TJ101

            LeagueClientAuthInfo authInfo = new LeagueClientAuthInfo();
            authInfo.Port = port;
            authInfo.Token = token;
            authInfo.Server = server;
            return authInfo;
        }



    }
}
