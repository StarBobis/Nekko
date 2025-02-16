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

            //AddTag(StackPanel_BasicTags,"ƣ�ͼ�ʻ", Color.FromArgb(255, 255, 0, 0)); // ��ɫ
            //AddTag(StackPanel_BasicTags, "������ͨ", Color.FromArgb(255, 0, 0, 255)); // ��ɫ
        }


        private void AddTag(StackPanel stackPanel, string text, Color textColor)
        {
            // ����һ���µ�TextBlock
            TextBlock tag = new TextBlock
            {
                Text = text,
                Margin = new Thickness(5), // ���ü��
                Foreground = new SolidColorBrush(textColor) // ����������ɫ
            };

            // ��ѡ�����ñ���ɫ��Բ�ǵ���ʽ
            var border = new Border
            {
                Child = tag,
                Background = new SolidColorBrush(Color.FromArgb(127, 200, 200, 200)), // ��͸������ɫ
                CornerRadius = new CornerRadius(10), // Բ�ǰ뾶
                Margin = new Thickness(2),
                Padding = new Thickness(2)
            };

            // ��border��ӵ�StackPanel
            stackPanel.Children.Add(border);
        }

        public async void ReadCurrentSummonerInfo()
        {
            //8835280e-6795-52ee-9e2a-4dd9d8bf0a6b
            //SummonerInfo CurrentSummoner = await LeagueClientUtils.GetSummonerGameRecordByPuuid(TestPuuid);

            SummonerInfo CurrentSummoner = await LeagueClientUtils.GetCurrentSummonerInfo();
            TextBlock_SummonerInfo.Text = CurrentSummoner.GameName + " #" + CurrentSummoner.TagLine;

            int TotalXP = CurrentSummoner.XpSinceLastLevel + CurrentSummoner.XpUntilNextLevel;
            TextBlock_SummonerLevel.Text = "�ٻ�ʦ�ȼ�: " + CurrentSummoner.SummonerLevel.ToString() + " ����ֵ: " + CurrentSummoner.XpSinceLastLevel.ToString() + " / " + TotalXP.ToString();
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

            //����KDA
            AddTag(StackPanel_KDATags, "KDA: " + kda.ToString("F1") , Color.FromArgb(255, 0, 0, 255)); // ��ɫ
            AddTag(StackPanel_KDATags, "�Լ۱�: " + realkda.ToString("F1") , Color.FromArgb(255, 0, 0, 255)); // ��ɫ
            AddTag(StackPanel_KDATags, "�Ŷ���: " + teamkda.ToString("F1") , Color.FromArgb(255, 0, 0, 255)); // ��ɫ


            //�ж���ʲô��
            if (kda <= 1.5)
            {
                AddTag(StackPanel_KDATags, "�ӱ�", Color.FromArgb(255, 255, 0, 0)); // ��ɫ
            }
            else if (kda > 1.5 && kda <= 2)
            {
                AddTag(StackPanel_KDATags, "�µ���", Color.FromArgb(255, 255, 0, 0)); // ��ɫ
            }
            else if (kda > 2 && kda <= 3)
            {
                AddTag(StackPanel_KDATags, "�е���", Color.FromArgb(255, 0, 0, 255)); // ��ɫ
            }
            else if (kda > 3 && kda <= 4)
            {
                AddTag(StackPanel_KDATags, "�ϵ���", Color.FromArgb(255, 0, 255, 0)); // ��ɫ
            }
            else
            {
                AddTag(StackPanel_KDATags, "����Ⱥ��", Color.FromArgb(255, 0, 255, 0)); // ��ɫ
            }

            Double AverageKill = (double)(totalKill) / gameObjectList.Count;
            Double AverageAssistant = (double)(totalAssist) / gameObjectList.Count;
            Double AverageDeath = (double)(totalDeath) / gameObjectList.Count;

            AddTag(StackPanel_KDATags, "������ɱ: " + AverageKill.ToString(), Color.FromArgb(255, 0, 255, 0)); // ��ɫ
            AddTag(StackPanel_KDATags, "��������: " + AverageAssistant.ToString(), Color.FromArgb(255, 0, 255, 0)); // ��ɫ
            AddTag(StackPanel_KDATags, "��������: " + AverageDeath.ToString(), Color.FromArgb(255, 0, 255, 0)); // ��ɫ

        }


        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            SummonerInfo task = await LeagueClientUtils.GetSummonerInfoByPuuid(TestPuuid);


            //GameRecord record = await LeagueClientUtils.GetSummonerGameRecord();
            //Debug.WriteLine(record.PlatformId);
            //Debug.WriteLine(record.AccountId);
            // �滻Ϊ��� LCU API �˿ں�����

            LeagueClientAuthInfo authInfo = LeagueClientUtils.GetClientInfoByWMIC();


            string port = authInfo.Port; // �˿ں�
            string token = authInfo.Token; // ����
            string baseUrl = $"https://127.0.0.1:{port}";

            // ���� SSL ֤����֤
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            // ���� HttpClient
            using (HttpClient client = new HttpClient(handler))
            {
                // ���� Basic Auth
                var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"riot:{token}")));
                client.DefaultRequestHeaders.Authorization = authValue;

                // ��ȡ��ǰѡ�˽׶���Ϣ
                string sessionUrl = $"{baseUrl}/lol-champ-select/v1/session";
                HttpResponseMessage response = await client.GetAsync(sessionUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    Debug.WriteLine(jsonResponse);
                    // ����Ƿ���ѡ�˽׶�
                    if (doc.RootElement.TryGetProperty("myTeam", out JsonElement myTeam))
                    {
                        Debug.WriteLine("��ǰ����ѡ�˽׶�");
                        foreach (JsonElement teammate in myTeam.EnumerateArray())
                        {
                            // ��ȡ���ѵ��ٻ�ʦ���ƺ� ID
                            if (teammate.TryGetProperty("summonerName", out JsonElement summonerName) &&
                                teammate.TryGetProperty("summonerId", out JsonElement summonerId))
                            {
                                Debug.WriteLine($"����: {summonerName.GetString()} (ID: {summonerId.GetInt64()})");

                                // ��ȡ������ϸ��Ϣ
                                string summonerUrl = $"{baseUrl}/lol-summoner/v1/summoners/{summonerId.GetInt64()}";
                                HttpResponseMessage summonerResponse = await client.GetAsync(summonerUrl);

                                if (summonerResponse.IsSuccessStatusCode)
                                {
                                    string summonerJson = await summonerResponse.Content.ReadAsStringAsync();
                                    JsonDocument summonerDoc = JsonDocument.Parse(summonerJson);
                                    Debug.WriteLine($"��ϸ��Ϣ: {summonerDoc.RootElement}");
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("��ǰ������ѡ�˽׶�");
                    }
                }
                else
                {
                    Debug.WriteLine($"����ʧ�ܣ�״̬��: {response.StatusCode}");
                }
            }

        }

    }
}
