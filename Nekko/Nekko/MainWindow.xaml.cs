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

using Nekko_Core;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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
        }

        private async void myButton_Click(object sender, RoutedEventArgs e)
        {

            SummonerInfo summonerInfo = await LeagueClientUtils.GetCurrentSummonerInfo();

            Debug.WriteLine(summonerInfo.SummonerId);
            Debug.WriteLine(summonerInfo.DisplayName);
            Debug.WriteLine(summonerInfo.GameName);
        }
    }
}
