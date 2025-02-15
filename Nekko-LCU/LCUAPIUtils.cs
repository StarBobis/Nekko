using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;



// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Nekko_LCU
{
    public class LCUAPIUtils
    {

        public static (int port, string token) GetLcuCredentials()
        {
            // 获取 LeagueClientUx 进程
            var process = Process.GetProcessesByName("LeagueClientUx").FirstOrDefault();
            if (process == null) throw new Exception("League client not running");

            // 通过 Windows API 获取完整命令行参数
            string commandLine = GetProcessCommandLine(process.Id);
            if (string.IsNullOrEmpty(commandLine))
                throw new Exception("Failed to retrieve command line");

            // 解析端口和令牌
            var portMatch = Regex.Match(commandLine, @"--app-port=(\d+)");
            var tokenMatch = Regex.Match(commandLine, @"--remoting-auth-token=([\w-]+)");

            if (!portMatch.Success || !tokenMatch.Success)
                throw new Exception("Invalid LCU parameters");

            return (int.Parse(portMatch.Groups[1].Value), tokenMatch.Groups[1].Value);
        }

        // Windows API 调用实现
        private static string GetProcessCommandLine(int processId)
        {
            const int ProcessCommandLineInformation = 60;
            IntPtr processHandle = IntPtr.Zero;

            try
            {
                // 打开进程并获取句柄
                processHandle = OpenProcess(
                    0x1000, // PROCESS_QUERY_LIMITED_INFORMATION
                    false,
                    processId);

                if (processHandle == IntPtr.Zero)
                    throw new Exception($"OpenProcess failed: {Marshal.GetLastWin32Error()}");

                // 查询命令行信息
                int bufferSize = 0;
                int status = NtQueryInformationProcess(
                    processHandle,
                    ProcessCommandLineInformation,
                    IntPtr.Zero,
                    0,
                    out bufferSize);

                if (status != 0 || bufferSize == 0)
                    throw new Exception("Failed to query buffer size");

                IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
                status = NtQueryInformationProcess(
                    processHandle,
                    ProcessCommandLineInformation,
                    buffer,
                    bufferSize,
                    out _);

                if (status != 0)
                    throw new Exception($"NtQueryInformationProcess failed: {status}");

                // 解析 UNICODE_STRING 结构
                var commandLineStruct = Marshal.PtrToStructure<UNICODE_STRING>(buffer);
                return Marshal.PtrToStringUni(commandLineStruct.Buffer, commandLineStruct.Length / 2);
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                    CloseHandle(processHandle);
            }
        }

        // Native 方法声明
        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(
            IntPtr processHandle,
            int processInformationClass,
            IntPtr processInformation,
            int processInformationLength,
            out int returnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(
            int dwDesiredAccess,
            bool bInheritHandle,
            int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        // 结构体定义
        [StructLayout(LayoutKind.Sequential)]
        private struct UNICODE_STRING
        {
            public ushort Length;
            public ushort MaximumLength;
            public IntPtr Buffer;
        }

        public static HttpClient CreateLcuHttpClient(int port, string token)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    (sender, cert, chain, sslPolicyErrors) => true // 忽略证书验证
            };

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://127.0.0.1:{port}/")
            };

            var authToken = Convert.ToBase64String(
                System.Text.Encoding.ASCII.GetBytes($"riot:{token}"));
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);

            return client;
        }

        public static async Task<JObject> GetCurrentSummonerInfo(HttpClient client)
        {
            var response = await client.GetAsync("/lol-summoner/v1/current-summoner");
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }


    }
}
