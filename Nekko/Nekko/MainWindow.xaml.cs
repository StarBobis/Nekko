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
using Nekko_LCU;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Http;
using System.Text.RegularExpressions;
using Windows.Graphics;
using Microsoft.UI.Windowing;
using Nekko_Core;



// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Nekko
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static MainWindow CurrentWindow;
        public NavigationView navigationView => nvSample;

        public MainWindow()
        {
            this.InitializeComponent();
            CurrentWindow = this;

            MoveWindowToCenterScreen();
            this.ExtendsContentIntoTitleBar = true;

            //���ñ���
            this.Title = "Nekko V1.0.0.4";

            //���ô��ڴ�С
            //1111 814   
            this.AppWindow.Resize(new SizeInt32(1132 + 16, 826 + 9));
            
            //����ͼ��
            this.AppWindow.SetIcon("Assets/Nekko.ico");

            //Ĭ�Ͻ�����ҳ���� 8
            if (nvSample.MenuItems.Count > 0)
            {
                nvSample.SelectedItem = nvSample.MenuItems[0];
                contentFrame.Navigate(typeof(HomePage));
            }
        }

        private void nvSample_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {

            // �������������ð�ť���򵼺�������ҳ��
            if (args.IsSettingsInvoked)
            {
                contentFrame.Navigate(typeof(SettingsPage));
            }
            else if (args.InvokedItemContainer is NavigationViewItem item)
            {
                var pageTag = item.Tag.ToString();
                Type pageType = null;

                switch (pageTag)
                {
                    case "HomePage":
                        pageType = typeof(HomePage);
                        break;
                    case "HistoryPage":
                        pageType = typeof(HistoryPage);
                        break;

                    //GameInfoPage
                    case "GameInfoPage":
                        pageType = typeof(GameInfoPage);
                        break;

                    case "RealTimeTeamPage":
                        pageType = typeof(RealTimeTeamPage);
                        break;
                    
                }

                if (pageType != null && contentFrame.Content?.GetType() != pageType)
                {
                    contentFrame.Navigate(pageType);
                }
            }
        }

        private void MoveWindowToCenterScreen()
        {

            // ��ȡ�봰�ڹ�����DisplayArea
            var displayArea = DisplayArea.GetFromWindowId(this.AppWindow.Id, DisplayAreaFallback.Nearest);
            // ��ȡ���ڵ�ǰ�ĳߴ�
            var windowSize = this.AppWindow.Size;

            // ȷ�����ǻ�ȡ������ȷ����ʾ����Ϣ
            if (displayArea != null)
            {
                // ���㴰�ھ�����������Ͻ����꣬������ʾ����ʵ�ʹ��������ų��������ȣ�
                int x = (int)(displayArea.WorkArea.X + (displayArea.WorkArea.Width - windowSize.Width) / 2);
                int y = (int)(displayArea.WorkArea.Y + (displayArea.WorkArea.Height - windowSize.Height) / 2);

                // ���ô���λ��
                this.AppWindow.Move(new PointInt32 { X = x, Y = y });
            }

            int window_pos_x = 0;
            int window_pos_y = 0;

            window_pos_x = (int)(displayArea.WorkArea.X + (displayArea.WorkArea.Width - windowSize.Width) / 2);
            window_pos_y = (int)(displayArea.WorkArea.Y + (displayArea.WorkArea.Height - windowSize.Height) / 2);

            if (window_pos_x != -1 && window_pos_y != -1)
            {
                this.AppWindow.Move(new PointInt32(window_pos_x, window_pos_y));
            }
        }
    }
}
