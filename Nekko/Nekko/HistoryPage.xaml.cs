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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

using System.Net.Http;
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
using System.Text.Json;
using Windows.UI;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Nekko
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HistoryPage : Page
    {

        private ObservableCollection<BattleInfo> BattleInfoCollection = new ObservableCollection<BattleInfo>();

        string TestPuuid = "8835280e-6795-52ee-9e2a-4dd9d8bf0a6b";

        public HistoryPage()
        {
            this.InitializeComponent();

            ReadCurrentSummonerInfo();
            ReadCurrentSummonerGameRecord();

            BattleInfoListView.ItemsSource = BattleInfoCollection;

            //AddTag(StackPanel_BasicTags,"疲劳驾驶", Color.FromArgb(255, 255, 0, 0)); // 红色
            //AddTag(StackPanel_BasicTags, "辅助精通", Color.FromArgb(255, 0, 0, 255)); // 蓝色
        }


        private void AddTag(StackPanel stackPanel, string text, Color textColor)
        {
            // 创建一个新的TextBlock
            TextBlock tag = new TextBlock
            {
                Text = text,
                Margin = new Thickness(5), // 设置间距
                Foreground = new SolidColorBrush(textColor) // 设置字体颜色
            };

            // 可选：设置背景色、圆角等样式
            var border = new Border
            {
                Child = tag,
                Background = new SolidColorBrush(Color.FromArgb(127, 200, 200, 200)), // 半透明背景色
                CornerRadius = new CornerRadius(10), // 圆角半径
                Margin = new Thickness(2),
                Padding = new Thickness(2)
            };

            // 将border添加到StackPanel
            stackPanel.Children.Add(border);
        }

        public async void ReadCurrentSummonerInfo()
        {
            //8835280e-6795-52ee-9e2a-4dd9d8bf0a6b
            //SummonerInfo CurrentSummoner = await LeagueClientUtils.GetSummonerGameRecordByPuuid(TestPuuid);

            SummonerInfo CurrentSummoner = await LeagueClientUtils.GetCurrentSummonerInfo();
            TextBlock_SummonerInfo.Text = CurrentSummoner.GameName + " #" + CurrentSummoner.TagLine;

            int TotalXP = CurrentSummoner.XpSinceLastLevel + CurrentSummoner.XpUntilNextLevel;
            TextBlock_SummonerLevel.Text = "召唤师等级: " + CurrentSummoner.SummonerLevel.ToString() + " 经验值: " + CurrentSummoner.XpSinceLastLevel.ToString() + " / " + TotalXP.ToString();
        }

        public async void ReadCurrentSummonerGameRecord()
        {

            GameRecord gameRecord = await LeagueClientUtils.GetCurrentSummonerGameRecord();
            List<GameObject> gameObjectList = gameRecord.GamesObjects.GameObjectList;

            ulong totalKill = 0;
            ulong totalAssist = 0;
            ulong totalDeath = 0;

            BattleInfoCollection.Clear();

            foreach (GameObject gameobj in gameObjectList)
            {

                Participant participant = gameobj.Participants[0];
                ParticipantStats participantStats = participant.ParticipantStats;

                totalKill += participantStats.Kills;
                totalAssist += participantStats.Assists;
                totalDeath += participantStats.Deaths;

                BattleInfo battleInfo = new BattleInfo(gameobj);
                BattleInfoCollection.Add(battleInfo);
            }

            double kda = (double)(totalKill + totalAssist) / totalDeath;
            double realkda = (double)(totalKill) / totalDeath;
            double teamkda = (double)(totalAssist) / totalDeath;

            //计算KDA
            AddTag(StackPanel_KDATags, "KDA: " + kda.ToString("F1") , Color.FromArgb(255, 0, 0, 255)); // 蓝色
            AddTag(StackPanel_KDATags, "性价比: " + realkda.ToString("F1") , Color.FromArgb(255, 0, 0, 255)); // 蓝色
            AddTag(StackPanel_KDATags, "团队性: " + teamkda.ToString("F1") , Color.FromArgb(255, 0, 0, 255)); // 蓝色


            //判断是什么马
            if (kda <= 1.5)
            {
                AddTag(StackPanel_KDATags, "坑比", Color.FromArgb(255, 255, 0, 0)); // 红色
            }
            else if (kda > 1.5 && kda <= 2)
            {
                AddTag(StackPanel_KDATags, "下等马", Color.FromArgb(255, 255, 0, 0)); // 红色
            }
            else if (kda > 2 && kda <= 3)
            {
                AddTag(StackPanel_KDATags, "中等马", Color.FromArgb(255, 0, 0, 255)); // 蓝色
            }
            else if (kda > 3 && kda <= 4)
            {
                AddTag(StackPanel_KDATags, "上等马", Color.FromArgb(255, 0, 255, 0)); // 蓝色
            }
            else
            {
                AddTag(StackPanel_KDATags, "傲视群雄", Color.FromArgb(255, 0, 255, 0)); // 蓝色
            }

            Double AverageKill = (double)(totalKill) / gameObjectList.Count;
            Double AverageAssistant = (double)(totalAssist) / gameObjectList.Count;
            Double AverageDeath = (double)(totalDeath) / gameObjectList.Count;

            AddTag(StackPanel_KDATags, "场均击杀: " + AverageKill.ToString(), Color.FromArgb(255, 0, 255, 0)); // 蓝色
            AddTag(StackPanel_KDATags, "场均助攻: " + AverageAssistant.ToString(), Color.FromArgb(255, 0, 255, 0)); // 蓝色
            AddTag(StackPanel_KDATags, "场均死亡: " + AverageDeath.ToString(), Color.FromArgb(255, 0, 255, 0)); // 蓝色

        }


        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            SummonerInfo task = await LeagueClientUtils.GetSummonerInfoByPuuid(TestPuuid);


            //GameRecord record = await LeagueClientUtils.GetSummonerGameRecord();
            //Debug.WriteLine(record.PlatformId);
            //Debug.WriteLine(record.AccountId);
            // 替换为你的 LCU API 端口和令牌

            LeagueClientAuthInfo authInfo = LeagueClientUtils.GetClientInfoByWMIC();


            string port = authInfo.Port; // 端口号
            string token = authInfo.Token; // 令牌
            string baseUrl = $"https://127.0.0.1:{port}";

            // 忽略 SSL 证书验证
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            // 创建 HttpClient
            using (HttpClient client = new HttpClient(handler))
            {
                // 设置 Basic Auth
                var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"riot:{token}")));
                client.DefaultRequestHeaders.Authorization = authValue;

                // 获取当前选人阶段信息
                string sessionUrl = $"{baseUrl}/lol-champ-select/v1/session";
                HttpResponseMessage response = await client.GetAsync(sessionUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    Debug.WriteLine(jsonResponse);
                    // 检查是否处于选人阶段
                    if (doc.RootElement.TryGetProperty("myTeam", out JsonElement myTeam))
                    {
                        Debug.WriteLine("当前处于选人阶段");
                        foreach (JsonElement teammate in myTeam.EnumerateArray())
                        {
                            // 获取队友的召唤师名称和 ID
                            if (teammate.TryGetProperty("summonerName", out JsonElement summonerName) &&
                                teammate.TryGetProperty("summonerId", out JsonElement summonerId))
                            {
                                Debug.WriteLine($"队友: {summonerName.GetString()} (ID: {summonerId.GetInt64()})");

                                // 获取队友详细信息
                                string summonerUrl = $"{baseUrl}/lol-summoner/v1/summoners/{summonerId.GetInt64()}";
                                HttpResponseMessage summonerResponse = await client.GetAsync(summonerUrl);

                                if (summonerResponse.IsSuccessStatusCode)
                                {
                                    string summonerJson = await summonerResponse.Content.ReadAsStringAsync();
                                    JsonDocument summonerDoc = JsonDocument.Parse(summonerJson);
                                    Debug.WriteLine($"详细信息: {summonerDoc.RootElement}");
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("当前不处于选人阶段");
                    }
                }
                else
                {
                    Debug.WriteLine($"请求失败，状态码: {response.StatusCode}");
                }
            }

        }

    }
}
