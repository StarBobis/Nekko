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



// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Nekko
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            //���ñ���
            this.Title = "Nekko V1.0.0.1";

            //���ô��ڴ�С
            this.AppWindow.Resize(new SizeInt32(1400, 888));
            
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
                    case "BattlePage":
                        pageType = typeof(BattlePage);
                        break;
                }

                if (pageType != null && contentFrame.Content?.GetType() != pageType)
                {
                    contentFrame.Navigate(pageType);
                }
            }
        }

        
    }
}
