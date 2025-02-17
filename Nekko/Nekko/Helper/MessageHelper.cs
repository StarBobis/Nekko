﻿using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nekko
{
    public class MessageHelper
    {
        public static async Task<bool> ShowConfirm(string ContentChinese, string ContentEnglish = "")
        {
            try
            {
                string TipContent = ContentChinese;
                //if (GlobalConfig.GameCfg.Value.Language && ContentEnglish != "")
                //{
                //    TipContent = ContentEnglish;
                //}

                ContentDialog subscribeDialog = new ContentDialog
                {
                    Title = "Tips",
                    Content = TipContent,
                    PrimaryButtonText = "OK", // 更改为确认
                    CloseButtonText = "Cancel", // 添加取消按钮
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = App.m_window.Content.XamlRoot // 确保设置 XamlRoot
                };

                ContentDialogResult result = await subscribeDialog.ShowAsync();

                // 根据用户点击的按钮返回相应的结果
                return result == ContentDialogResult.Primary; // 如果点击的是确认按钮，则返回true；否则返回false
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }


        public static async Task<bool> Show(string ContentChinese, string ContentEnglish = "")
        {
            try
            {
                string TipContent = ContentChinese;
                //if (GlobalConfig.GameCfg.Value.Language && ContentEnglish != "")
                //{
                //    TipContent = ContentEnglish;
                //}

                ContentDialog subscribeDialog = new ContentDialog
                {
                    Title = "Tips",
                    Content = TipContent,
                    PrimaryButtonText = "OK",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = App.m_window.Content.XamlRoot // 确保设置 XamlRoot
                };

                ContentDialogResult result = await subscribeDialog.ShowAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }

        }

    }
}
