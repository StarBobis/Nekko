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
        
        public static void SetStackPanelTags(List<GameObject> gameObjectList, StackPanel In_StackPanel_KDATags, StackPanel In_StackPanel_BattleTags, StackPanel In_StackPanel_NiuMaTags)
        {
            In_StackPanel_KDATags.Children.Clear();
            In_StackPanel_BattleTags.Children.Clear();
            In_StackPanel_NiuMaTags.Children.Clear();


            TagInfo tagInfo = new TagInfo(gameObjectList);

            foreach (Tag tag in tagInfo.KDATagList)
            {
                AddTag(In_StackPanel_KDATags, tag.TagName, tag.TagColor);
            }

            foreach (Tag tag in tagInfo.BattleTagList)
            {
                AddTag(In_StackPanel_BattleTags, tag.TagName, tag.TagColor);
            }
           
            foreach (Tag tag in tagInfo.NiuMaTagList)
            {
                AddTag(In_StackPanel_NiuMaTags, tag.TagName, tag.TagColor);
            }

        }
    }
}
