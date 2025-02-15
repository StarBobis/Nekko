using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using Windows.UI.ApplicationSettings;
using Windows.Web.AtomPub;
using Windows.Media.Protection.PlayReady;

using Nekko_Core;
using Nekko_LCU;
using System.Text;
using System.Diagnostics;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Nekko
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();


            
        }


        private async void myButton_Click(object sender, RoutedEventArgs e)
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
                SummonerInfo summonerInfo = await LeagueClientUtils.GetCurrentSummonerInfo();
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
                string puuid = summonerInfo.Puuid;

                // 使用PUUID查询历史战绩
                HttpResponseMessage historyResponse = await client.GetAsync($"/lol-match-history/v1/products/lol/{puuid}/matches");
                if (historyResponse.IsSuccessStatusCode)
                {
                    var historyJson = await historyResponse.Content.ReadAsStringAsync();
                    Debug.WriteLine(historyJson); // 输出历史战绩JSON字符串
                }
                else
                {
                    Debug.WriteLine("Failed to get match history.");
                }
            }
            
        }

    }
}
