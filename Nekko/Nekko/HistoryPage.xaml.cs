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

            BattleInfoListView.ItemsSource = BattleInfoCollection;

            Referesh();

        }

        public async void Referesh()
        {
            LeagueClientUtils.GetRiotAuthClient(true);

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



       

        


        public void SetGameRecord(List<GameObject> gameObjectList,StackPanel In_StackPanel_KDATags,StackPanel In_StackPanel_BattleTags, StackPanel In_StackPanel_NiuMaTags)
        {
            BattleInfoCollection.Clear();
            foreach (GameObject gameobj in gameObjectList)
            {
                BattleInfo battleInfo = new BattleInfo(gameobj);
                BattleInfoCollection.Add(battleInfo);
            }

            XAMLHelper.SetStackPanelTags(gameObjectList, In_StackPanel_KDATags, In_StackPanel_BattleTags, In_StackPanel_NiuMaTags);
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

                List<GameObject> gameObjectList = await LeagueClientUtils.ReadSummonerRankRecordByPuuid(participantIdentity.ParticipantPlayer.Puuid);
                string SummonerInfoString = "";
                SummonerInfoString = SummonerInfoString + participantIdentity.ParticipantPlayer.GameName + "#" + participantIdentity.ParticipantPlayer.TagLine;

                ParticipantStats participantStats = gameRecord.Participants[Index].ParticipantStats;
                SummonerInfoString = SummonerInfoString + "  " + participantStats.Kills.ToString() + "/" + participantStats.Deaths.ToString() + "/" + participantStats.Assists.ToString();
                TimerUtils.End("ReadSummonerRankRecordByPuuid");

                if (participantIdentity.ParticipantId == 1)
                {
                    Summoner1_Info.Text = SummonerInfoString;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner1_KDATags, StackPanel_Summoner1_BattleTags, StackPanel_Summoner1_NiuMaTags);
                }
                else if (participantIdentity.ParticipantId == 2)
                {
                    Summoner2_Info.Text = SummonerInfoString;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner2_KDATags, StackPanel_Summoner2_BattleTags, StackPanel_Summoner2_NiuMaTags);
                }
                else if (participantIdentity.ParticipantId == 3)
                {
                    Summoner3_Info.Text = SummonerInfoString;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner3_KDATags, StackPanel_Summoner3_BattleTags, StackPanel_Summoner3_NiuMaTags);
                }
                else if (participantIdentity.ParticipantId == 4)
                {
                    Summoner4_Info.Text = SummonerInfoString;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner4_KDATags, StackPanel_Summoner4_BattleTags, StackPanel_Summoner4_NiuMaTags);
                }
                else if (participantIdentity.ParticipantId == 5)
                {
                    Summoner5_Info.Text = SummonerInfoString;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner5_KDATags, StackPanel_Summoner5_BattleTags, StackPanel_Summoner5_NiuMaTags);
                }

                else if (participantIdentity.ParticipantId == 6)
                {
                    Summoner6_Info.Text = SummonerInfoString;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner6_KDATags, StackPanel_Summoner6_BattleTags, StackPanel_Summoner6_NiuMaTags);
                }

                else if (participantIdentity.ParticipantId == 7)
                {
                    Summoner7_Info.Text = SummonerInfoString;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner7_KDATags, StackPanel_Summoner7_BattleTags, StackPanel_Summoner7_NiuMaTags);
                }

                else if (participantIdentity.ParticipantId == 8)
                {
                    Summoner8_Info.Text = SummonerInfoString;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner8_KDATags, StackPanel_Summoner8_BattleTags, StackPanel_Summoner8_NiuMaTags);
                }
                else if (participantIdentity.ParticipantId == 9)
                {
                    Summoner9_Info.Text = SummonerInfoString;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner9_KDATags, StackPanel_Summoner9_BattleTags, StackPanel_Summoner9_NiuMaTags);
                }
                else if (participantIdentity.ParticipantId == 10)
                {
                    Summoner10_Info.Text = SummonerInfoString;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner10_KDATags, StackPanel_Summoner10_BattleTags, StackPanel_Summoner10_NiuMaTags);
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

        

        
    }
}
