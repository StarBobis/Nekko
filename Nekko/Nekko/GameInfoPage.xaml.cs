using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using Nekko_LCU;
using Nekko_Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Nekko
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameInfoPage : Page
    {
        public GameInfoPage()
        {
            this.InitializeComponent();
        }

        // 在TargetPage中接收参数
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // 现在你可以对passedParameter进行处理
            if (e.Parameter != null)
            {
                string GameId = (string)e.Parameter; // 这里就是你传递的myParameter

                if (GameId != "")
                {
                    SetGameInfo(GameId);
                }
            }
            
        }

        public async void SetGameInfo(string GameId)
        {
            GameObject gameRecord = await LeagueClientUtils.GetGameInfoByGameId(GameId);

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

        }

    }
}
