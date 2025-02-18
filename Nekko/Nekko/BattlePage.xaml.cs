using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Nekko_LCU;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Nekko
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BattlePage : Page
    {
        public BattlePage()
        {
            this.InitializeComponent();
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
                List<GameObject> gameObjectList = await LeagueClientUtils.ReadSummonerRankRecordByPuuid(puuid);
                SummonerInfo summonerInfo = await LeagueClientUtils.GetSummonerInfoByPuuid(puuid);
                string SummonerName = summonerInfo.GameName + "#" + summonerInfo.TagLine;

                if (index == 1)
                {
                    Summoner1_Info.Text = SummonerName;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner1_KDATags, StackPanel_Summoner1_BattleTags, StackPanel_Summoner1_NiuMaTags);
                }
                else if (index == 2)
                {
                    Summoner2_Info.Text = SummonerName;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner2_KDATags, StackPanel_Summoner2_BattleTags, StackPanel_Summoner2_NiuMaTags);
                }
                else if (index == 3)
                {
                    Summoner3_Info.Text = SummonerName;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner3_KDATags, StackPanel_Summoner3_BattleTags, StackPanel_Summoner3_NiuMaTags);
                }
                else if (index == 4)
                {
                    Summoner4_Info.Text = SummonerName;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner4_KDATags, StackPanel_Summoner4_BattleTags, StackPanel_Summoner4_NiuMaTags);
                }
                else if (index == 5)
                {
                    Summoner5_Info.Text = SummonerName;
                    XAMLHelper.SetStackPanelTags(gameObjectList, StackPanel_Summoner5_KDATags, StackPanel_Summoner5_BattleTags, StackPanel_Summoner5_NiuMaTags);
                }

                index++;
            }

        }
    }
}
