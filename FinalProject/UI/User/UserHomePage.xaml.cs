using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinalProject.UI
{
    /// <summary>
    /// UserHomePage.xaml 的交互逻辑
    /// </summary>
    public partial class UserHomePage : Page 
    {

        private SchedulePage.MyEventHandler GoToSitePage;//跳转选座页面

        public delegate void MyEventHandler();
        public event MyEventHandler GoMovleListPage;
        public event MyEventHandler GoRefundPage;

        private SchedulePage schedulePage;
        public UserHomePage(MyEventHandler fun1,MyEventHandler fun3, SchedulePage.MyEventHandler GoToSitePage)
        {
            InitializeComponent();
            GoMovleListPage += fun1;
            GoRefundPage += fun3;
            schedulePage = new SchedulePage(GoToSitePage);
            OpenPage();
        }

        //显示最新档期
        private void ShowNearSchedule()
        {
            string err = UserMainWindow.userController.LoadNearSchedules(out List<Dictionary<string,string>> NearScheduleDics);
            schedulePage.ShowSchedules(NearScheduleDics);
        }


        //跳转购票
        private void BuyBtn_Click(object sender, RoutedEventArgs e)
        {
            GoMovleListPage();
        }


        public void OpenPage()
        {
            NearScheduleFrame.Resources.Add("UUID", schedulePage);
            NearScheduleFrame.Navigate(schedulePage);
            ShowNearSchedule();
        }

        public void GoBackPage()
        {
            ShowNearSchedule();
        }







        //跳转退票
        private void RefundBtn_Click(object sender, RoutedEventArgs e)
        {
            //GoRefundPage();
        }


    }
}
