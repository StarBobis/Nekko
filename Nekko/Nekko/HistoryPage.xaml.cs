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
using System.Reflection.Metadata;

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


        public HistoryPage()
        {
            this.InitializeComponent();

            Referesh();

            BattleInfoListView.ItemsSource = BattleInfoCollection;

            //AddTag(StackPanel_BasicTags,"疲劳驾驶", Color.FromArgb(255, 255, 0, 0)); // 红色
            //AddTag(StackPanel_BasicTags, "辅助精通", Color.FromArgb(255, 0, 0, 255)); // 蓝色
        }

        public async void Referesh()
        {
            LeagueClientUtils.GetClientInfoByWMI(true);

            if (TextBox_SummonerName.Text == "")
            {
                ReadCurrentSummonerInfo();
                ReadCurrentSummonerGameRecord();
            }
            else
            {
                await SearchSummonerInfo();
            }
           
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
                Background = new SolidColorBrush(Color.FromArgb(188, 0, 0, 0)), // 半透明背景色
                CornerRadius = new CornerRadius(10), // 圆角半径
                Margin = new Thickness(2),
                Padding = new Thickness(2)
            };

            // 将border添加到StackPanel
            stackPanel.Children.Add(border);
        }

        public void SetCurrentSummonerInfo(SummonerInfo CurrentSummoner)
        {
            TextBlock_SummonerInfo.Text = CurrentSummoner.GameName + " #" + CurrentSummoner.TagLine;

            int TotalXP = CurrentSummoner.XpSinceLastLevel + CurrentSummoner.XpUntilNextLevel;
            TextBlock_SummonerLevel.Text = "召唤师等级: " + CurrentSummoner.SummonerLevel.ToString() + " 经验值: " + CurrentSummoner.XpSinceLastLevel.ToString() + " / " + TotalXP.ToString();
        }

        public async void ReadCurrentSummonerInfo()
        {
            SummonerInfo CurrentSummoner = await LeagueClientUtils.GetCurrentSummonerInfo();
            SetCurrentSummonerInfo(CurrentSummoner);
        }



        public static Color GetNumberLimitColor(double InputNumber, double limit1, double limit2, double limit3)
        {
            if (InputNumber < limit1)
            {
                return Color.FromArgb(255, 255, 0, 0); //红色
            }
            else if (InputNumber >= limit1 && InputNumber < limit2)
            {
                return Color.FromArgb(255, 255,255, 0); //黄色
            }
            else if (InputNumber >= limit2 && InputNumber < limit3)
            {
                return Color.FromArgb(255, 0, 255, 0); // 绿色
            }
            else
            {
                return Color.FromArgb(255, 128, 0, 128); // 紫色
            }
        }

        public void SetStackPanelTags(List<GameObject> gameObjectList, StackPanel In_StackPanel_KDATags, StackPanel In_StackPanel_BattleTags, StackPanel In_StackPanel_NiuMaTags)
        {
            In_StackPanel_KDATags.Children.Clear();
            In_StackPanel_BattleTags.Children.Clear();
            In_StackPanel_NiuMaTags.Children.Clear();

            KDAInfo kdaInfo = new KDAInfo(gameObjectList);


            Dictionary<string, int> KuaKuaTagMap = new Dictionary<string, int>();
            Dictionary<string, int> NiuMaTagMap = new Dictionary<string, int>();


            int totalNiumaTag = 0;
            int totalKuaKuaTag = 0;

            int totalWin = 0;
            int totalLose = 0;

            foreach (GameObject gameobj in gameObjectList)
            {

                BattleInfo battleInfo = new BattleInfo(gameobj);

                if (battleInfo.Win == "胜利")
                {
                    totalWin++;
                }
                else
                {
                    totalLose++;
                }

                foreach (string NiumaTag in battleInfo.NiuMaTagList)
                {
                    if (NiuMaTagMap.ContainsKey(NiumaTag))
                    {
                        NiuMaTagMap[NiumaTag] = NiuMaTagMap[NiumaTag] + 1;
                    }
                    else
                    {
                        NiuMaTagMap[NiumaTag] = 1;
                    }
                    totalNiumaTag++;
                }

                foreach (string KuaKuaTag in battleInfo.KuaKuaTagList)
                {
                    if (KuaKuaTagMap.ContainsKey(KuaKuaTag))
                    {
                        KuaKuaTagMap[KuaKuaTag] = KuaKuaTagMap[KuaKuaTag] + 1;
                    }
                    else
                    {
                        KuaKuaTagMap[KuaKuaTag] = 1;
                    }
                    totalKuaKuaTag++;

                }
            }

            foreach (var pair in KuaKuaTagMap)
            {
                string KuaKuaTag = "";
                KuaKuaTag = pair.Key + "*" + pair.Value;
                AddTag(In_StackPanel_BattleTags, KuaKuaTag, Color.FromArgb(255, 0, 255, 0));
            }

            foreach (var pair in NiuMaTagMap)
            {
                string KuaKuaTag = "";
                KuaKuaTag = pair.Key + "*" + pair.Value;
                AddTag(In_StackPanel_BattleTags, KuaKuaTag, Color.FromArgb(255, 255, 0, 0));
            }

            double NiumaNumber = (double)totalNiumaTag / (totalKuaKuaTag + totalNiumaTag) * 100;

            if (NiumaNumber <= 10)
            {
                AddTag(In_StackPanel_NiuMaTags, "金牌大腿", Color.FromArgb(255, 0, 255, 0));
            }
            else if (NiumaNumber > 10 && NiumaNumber <= 20)
            {
                AddTag(In_StackPanel_NiuMaTags, "银牌大腿", Color.FromArgb(255, 0, 255, 0));
            }
            else if (NiumaNumber > 20 && NiumaNumber <= 30)
            {
                AddTag(In_StackPanel_NiuMaTags, "偶尔能C", Color.FromArgb(255, 0, 255, 0));
            }
            else if (NiumaNumber > 30 && NiumaNumber <= 40)
            {
                AddTag(In_StackPanel_NiuMaTags, "混子", Color.FromArgb(255, 0, 255, 0));
            }
            else if (NiumaNumber > 40)
            {
                AddTag(In_StackPanel_NiuMaTags, "峡谷鬼见愁", Color.FromArgb(255, 255, 0, 0));
            }

            string NiumaZhiShu = "牛马含量: " + NiumaNumber.ToString("F1") + "%";
            AddTag(In_StackPanel_NiuMaTags, NiumaZhiShu, GetNumberLimitColor(100 / NiumaNumber, 2, 2.5, 5));

            //近期胜率
            double totalShenglv = (double)totalWin / (totalWin + totalLose) * 100;
            AddTag(In_StackPanel_NiuMaTags, "近20局胜率: " + totalShenglv.ToString("F1") + "%", GetNumberLimitColor(totalShenglv/100, 0.5, 0.55, 0.6));


            //计算KDA
            AddTag(In_StackPanel_KDATags, "KDA: " + kdaInfo.kda.ToString("F1"), GetNumberLimitColor(kdaInfo.kda, 1.0, 2.0, 3.0));
            AddTag(In_StackPanel_KDATags, "性价比: " + kdaInfo.realkda.ToString("F1"), GetNumberLimitColor(kdaInfo.realkda, 0.5, 1.0, 2.0));
            AddTag(In_StackPanel_KDATags, "团队性: " + kdaInfo.teamkda.ToString("F1"), GetNumberLimitColor(kdaInfo.teamkda, 2, 5, 10));
            AddTag(In_StackPanel_KDATags, kdaInfo.MaName, GetNumberLimitColor(kdaInfo.kda, 2, 3, 4));


            AddTag(In_StackPanel_KDATags, "场均击杀: " + kdaInfo.AverageKill.ToString("F1"), GetNumberLimitColor(kdaInfo.AverageKill, 1, 8, 15));
            AddTag(In_StackPanel_KDATags, "场均助攻: " + kdaInfo.AverageAssistant.ToString("F1"), GetNumberLimitColor(kdaInfo.AverageAssistant, 5, 10, 15));
            AddTag(In_StackPanel_KDATags, "场均死亡: " + kdaInfo.AverageDeath.ToString("F1"), GetNumberLimitColor(1.0 / kdaInfo.AverageDeath, 0.1, 0.2, 0.3));
        }


        public void SetGameRecord(List<GameObject> gameObjectList,StackPanel In_StackPanel_KDATags,StackPanel In_StackPanel_BattleTags, StackPanel In_StackPanel_NiuMaTags)
        {
            BattleInfoCollection.Clear();
            foreach (GameObject gameobj in gameObjectList)
            {
                BattleInfo battleInfo = new BattleInfo(gameobj);
                BattleInfoCollection.Add(battleInfo);
            }

            SetStackPanelTags(gameObjectList, In_StackPanel_KDATags, In_StackPanel_BattleTags, In_StackPanel_NiuMaTags);
        }

        public async Task<List<GameObject>> ReadSummonerRankRecordByPuuid(string Puuid)
        {

            List<GameObject> gameObjectsList = new List<GameObject>();

            for (int i = 0; i < 10; i++)
            {
                TimerUtils.Start("GetSummonerGameRecordByPuuid");
                GameRecord gameRecord = await LeagueClientUtils.GetSummonerGameRecordByPuuid(Puuid, 0 + i * 20, i * 20 + 20);
                TimerUtils.End("GetSummonerGameRecordByPuuid");

                List<GameObject> tmpList = gameRecord.GamesObjects.GameObjectList;
                bool shouldExists = false;
                foreach (GameObject gameobj in tmpList)
                {
                    if (gameobj.QueueId == 420)
                    {
                        gameObjectsList.Add(gameobj);
                    }

                    if (gameObjectsList.Count == 20)
                    {
                        shouldExists = true;
                        break;
                    }
                }

                if (shouldExists)
                {
                    break;
                }
            }
            return gameObjectsList;
        }

        public async void ReadSummonerGameRecordByPuuid(string Puuid)
        {
            bool onlyDanShuang = (bool)CheckBox_OnlyDanShuangPai.IsChecked;
            if (onlyDanShuang)
            {
                List<GameObject> gameObjectsList = new List<GameObject>();

                for (int i = 0; i < 10; i++)
                {
                    GameRecord gameRecord = await LeagueClientUtils.GetSummonerGameRecordByPuuid(Puuid, 0, 20);

                    List<GameObject> tmpList = gameRecord.GamesObjects.GameObjectList;
                    bool shouldExists = false;
                    foreach (GameObject gameobj in tmpList)
                    {
                        if (gameobj.QueueId == 420)
                        {
                            gameObjectsList.Add(gameobj);
                        }

                        if (gameObjectsList.Count == 20)
                        {
                            shouldExists = true;
                            break;
                        }
                    }

                    if (shouldExists)
                    {
                        break;
                    }
                }

                SetGameRecord(gameObjectsList,StackPanel_KDATags,StackPanel_BattleTags,StackPanel_NiuMaTags);

            }
            else
            {
                GameRecord gameRecord = await LeagueClientUtils.GetSummonerGameRecordByPuuid(Puuid, 0, 20);
                SetGameRecord(gameRecord.GamesObjects.GameObjectList, StackPanel_KDATags, StackPanel_BattleTags, StackPanel_NiuMaTags);
            }
        }


        public async void ReadCurrentSummonerGameRecord()
        {
            SummonerInfo summonerInfo = await LeagueClientUtils.GetCurrentSummonerInfo();
            ReadSummonerGameRecordByPuuid(summonerInfo.Puuid);

        }


        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            //SummonerInfo task = await LeagueClientUtils.GetSummonerInfoByPuuid(TestPuuid);
            SummonerInfo task1 = await LeagueClientUtils.GetSummonerInfoByNameTag("ELO连胜队列","54063");
        }


        

        public async Task SearchSummonerInfo()
        {
            try
            {
                string SummonerName = TextBox_SummonerName.Text;
                string[] splits = SummonerName.Split("#");

                if (splits.Length < 2)
                {
                    await MessageHelper.Show("输入的召唤师名称应为 名称#编号， 例如 隔壁老王#11451");
                    return;
                }

                string Name = splits[0];
                string Tag = splits[1];

                SummonerInfo targetSummonerInfo = await LeagueClientUtils.GetSummonerInfoByNameTag(Name, Tag);

                SetCurrentSummonerInfo(targetSummonerInfo);

                ReadSummonerGameRecordByPuuid(targetSummonerInfo.Puuid);
            }
            catch (Exception ex)
            {
                await MessageHelper.Show("查询失败\n\n" + ex.ToString());
            }
        }


        private async void Button_SearchSummonerByName_Click(object sender, RoutedEventArgs e)
        {
            await SearchSummonerInfo();


        }

        private async void Menu_BattleDetailInfo_Click(object sender, RoutedEventArgs e)
        {
            BattleInfo battleInfo = new BattleInfo();

            try
            {
                battleInfo = BattleInfoCollection[BattleInfoListView.SelectedIndex];

            }catch (Exception ex)
            {
                ex.ToString();
                _ = MessageHelper.Show("请先左键选中一个对局记录");
                return;
            }

            GameObject gameObject = battleInfo.gameObject;

            TimerUtils.Start("GetGameInfoByGameId");
            GameObject gameRecord = await LeagueClientUtils.GetGameInfoByGameId(gameObject.GameId.ToString());
            TimerUtils.End("GetGameInfoByGameId");

            int Index = 0;

            foreach (ParticipantIdentity participantIdentity in gameRecord.ParticipantIdentities)
            {
                TimerUtils.Start("ReadSummonerRankRecordByPuuid");

                List<GameObject> gameObjectList = await ReadSummonerRankRecordByPuuid(participantIdentity.ParticipantPlayer.Puuid);
                string SummonerInfoString = "";
                SummonerInfoString = SummonerInfoString + participantIdentity.ParticipantPlayer.GameName + "#" + participantIdentity.ParticipantPlayer.TagLine;

                ParticipantStats participantStats = gameRecord.Participants[Index].ParticipantStats;
                SummonerInfoString = SummonerInfoString + "  " + participantStats.Kills.ToString() + "/" + participantStats.Deaths.ToString() + "/" + participantStats.Assists.ToString();
                TimerUtils.End("ReadSummonerRankRecordByPuuid");

                if (participantIdentity.ParticipantId == 1)
                {
                    Summoner1_Info.Text = SummonerInfoString;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner1_KDATags, StackPanel_Summoner1_BattleTags, StackPanel_Summoner1_NiuMaTags);
                }
                else if (participantIdentity.ParticipantId == 2)
                {
                    Summoner2_Info.Text = SummonerInfoString;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner2_KDATags, StackPanel_Summoner2_BattleTags, StackPanel_Summoner2_NiuMaTags);
                }
                else if (participantIdentity.ParticipantId == 3)
                {
                    Summoner3_Info.Text = SummonerInfoString;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner3_KDATags, StackPanel_Summoner3_BattleTags, StackPanel_Summoner3_NiuMaTags);
                }
                else if (participantIdentity.ParticipantId == 4)
                {
                    Summoner4_Info.Text = SummonerInfoString;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner4_KDATags, StackPanel_Summoner4_BattleTags, StackPanel_Summoner4_NiuMaTags);
                }
                else if (participantIdentity.ParticipantId == 5)
                {
                    Summoner5_Info.Text = SummonerInfoString;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner5_KDATags, StackPanel_Summoner5_BattleTags, StackPanel_Summoner5_NiuMaTags);
                }

                else if (participantIdentity.ParticipantId == 6)
                {
                    Summoner6_Info.Text = SummonerInfoString;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner6_KDATags, StackPanel_Summoner6_BattleTags, StackPanel_Summoner6_NiuMaTags);
                }

                else if (participantIdentity.ParticipantId == 7)
                {
                    Summoner7_Info.Text = SummonerInfoString;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner7_KDATags, StackPanel_Summoner7_BattleTags, StackPanel_Summoner7_NiuMaTags);
                }

                else if (participantIdentity.ParticipantId == 8)
                {
                    Summoner8_Info.Text = SummonerInfoString;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner8_KDATags, StackPanel_Summoner8_BattleTags, StackPanel_Summoner8_NiuMaTags);
                }
                else if (participantIdentity.ParticipantId == 9)
                {
                    Summoner9_Info.Text = SummonerInfoString;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner9_KDATags, StackPanel_Summoner9_BattleTags, StackPanel_Summoner9_NiuMaTags);
                }
                else if (participantIdentity.ParticipantId == 10)
                {
                    Summoner10_Info.Text = SummonerInfoString;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner10_KDATags, StackPanel_Summoner10_BattleTags, StackPanel_Summoner10_NiuMaTags);
                }
                Index++;
            }
            

            //ContentFrame.Navigate(typeof(TargetPage), parameter);
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            Referesh();
        }

        private void CheckBox_OnlyDanShuangPai_Checked(object sender, RoutedEventArgs e)
        {
            Referesh();
        }

        private void CheckBox_OnlyDanShuangPai_Unchecked(object sender, RoutedEventArgs e)
        {
            Referesh();
        }

        

        private async void Button_CurrentGameInfo_Click(object sender, RoutedEventArgs e)
        {
            ChampionSelect championSelect = await LeagueClientUtils.GetChampionSelectInfo();
            if (championSelect.teamMemberList == null)
            {
                _ = MessageHelper.Show("对局尚未开始");
                return;
            }

            int index = 1;
            foreach (TeamMemberInfo teamMemberInfo in championSelect.teamMemberList)
            {
                string puuid = teamMemberInfo.Puuid;
                List<GameObject> gameObjectList = await ReadSummonerRankRecordByPuuid(puuid);
                SummonerInfo summonerInfo = await LeagueClientUtils.GetSummonerInfoByPuuid(puuid);
                string SummonerName = summonerInfo.GameName +"#" + summonerInfo.TagLine;

                if (index == 1)
                {
                    Summoner1_Info.Text = SummonerName;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner1_KDATags, StackPanel_Summoner1_BattleTags, StackPanel_Summoner1_NiuMaTags);
                }
                else if (index == 2)
                {
                    Summoner2_Info.Text = SummonerName;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner2_KDATags, StackPanel_Summoner2_BattleTags, StackPanel_Summoner2_NiuMaTags);
                }
                else if (index == 3)
                {
                    Summoner3_Info.Text = SummonerName;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner3_KDATags, StackPanel_Summoner3_BattleTags, StackPanel_Summoner3_NiuMaTags);
                }
                else if (index == 4)
                {
                    Summoner4_Info.Text = SummonerName;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner4_KDATags, StackPanel_Summoner4_BattleTags, StackPanel_Summoner4_NiuMaTags);
                }
                else if (index == 5)
                {
                    Summoner5_Info.Text = SummonerName;
                    SetStackPanelTags(gameObjectList, StackPanel_Summoner5_KDATags, StackPanel_Summoner5_BattleTags, StackPanel_Summoner5_NiuMaTags);
                }

                index++;
            }
            
        }
    }
}
