using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FinalProject.UI
{
    /// <summary>
    /// ResultPage.xaml 的交互逻辑
    /// </summary>
    public partial class ResultPage : Page
    {
        public delegate void MyEventHandler();
        private delegate void TimerDispatcherDelegate();
        public event MyEventHandler GoHomePage;
        private Timer timercount;
        public ResultPage(MyEventHandler fun)
        {
            InitializeComponent();
            GoHomePage += fun;
        }
        public void ShowResult(Dictionary<string, string> scheduleDic, List<string> siteIDs, string paymentinfo)
        {        
            //启动本地倒计时timer
            timercount = new Timer(2000);
            timercount.Elapsed += Timercount_Elapsed;
            timercount.Enabled = true;
        }

        private void Timercount_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new TimerDispatcherDelegate(GoHome));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GoHome();
        }
        private void GoHome()
        {
            timercount.Dispose();
            GoHomePage();
        }
    }
}
