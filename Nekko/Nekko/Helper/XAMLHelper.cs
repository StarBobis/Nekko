using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Nekko_LCU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Nekko
{
    public class XAMLHelper
    {
        private static void AddTag(StackPanel stackPanel, string text, Color textColor)
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
        public static Color GetNumberLimitColor(double InputNumber, double limit1, double limit2, double limit3)
        {
            if (InputNumber < limit1)
            {
                return Color.FromArgb(255, 255, 0, 0); //红色
            }
            else if (InputNumber >= limit1 && InputNumber < limit2)
            {
                return Color.FromArgb(255, 255, 255, 0); //黄色
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
        public static void SetStackPanelTags(List<GameObject> gameObjectList, StackPanel In_StackPanel_KDATags, StackPanel In_StackPanel_BattleTags, StackPanel In_StackPanel_NiuMaTags)
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
            AddTag(In_StackPanel_NiuMaTags, "近20局胜率: " + totalShenglv.ToString("F1") + "%", GetNumberLimitColor(totalShenglv / 100, 0.5, 0.55, 0.6));


            //计算KDA
            AddTag(In_StackPanel_KDATags, "KDA: " + kdaInfo.kda.ToString("F1"), GetNumberLimitColor(kdaInfo.kda, 1.0, 2.0, 3.0));
            AddTag(In_StackPanel_KDATags, "性价比: " + kdaInfo.realkda.ToString("F1"), GetNumberLimitColor(kdaInfo.realkda, 0.5, 1.0, 2.0));
            AddTag(In_StackPanel_KDATags, "团队性: " + kdaInfo.teamkda.ToString("F1"), GetNumberLimitColor(kdaInfo.teamkda, 2, 5, 10));
            AddTag(In_StackPanel_KDATags, kdaInfo.MaName, GetNumberLimitColor(kdaInfo.kda, 2, 3, 4));


            AddTag(In_StackPanel_KDATags, "场均击杀: " + kdaInfo.AverageKill.ToString("F1"), GetNumberLimitColor(kdaInfo.AverageKill, 1, 8, 15));
            AddTag(In_StackPanel_KDATags, "场均助攻: " + kdaInfo.AverageAssistant.ToString("F1"), GetNumberLimitColor(kdaInfo.AverageAssistant, 5, 10, 15));
            AddTag(In_StackPanel_KDATags, "场均死亡: " + kdaInfo.AverageDeath.ToString("F1"), GetNumberLimitColor(1.0 / kdaInfo.AverageDeath, 0.1, 0.2, 0.3));
        }
    }
}
