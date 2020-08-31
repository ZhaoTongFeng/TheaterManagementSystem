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
using System.Windows.Shapes;

namespace FinalProject.UI
{
    /// <summary>
    /// UserMainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserMainWindow : Window
    {
        public static UserController userController = new UserController();

        UserHomePage homePage;
        MovieListPage movieListPage;
        SchedulePage schedulePage;
        SitePage sitePage;
        PaymentPage paymentPage;
        ResultPage resultPage;
        RefundMainPage refundMainPage;

        Stack<string> currentPage = new Stack<string>();

        public UserMainWindow()
        {
            InitializeComponent();
            
            movieListPage = new MovieListPage(GoSchedulePage);
            schedulePage = new SchedulePage(GoSitePage);
            sitePage = new SitePage(GoPaymentPage);
            paymentPage = new PaymentPage(GoResultPage,GoBack);
            resultPage = new ResultPage(GoHomePage);
            homePage = new UserHomePage(GoMovieList, GoRefundPage, GoSitePage);
            refundMainPage = new RefundMainPage(GoHomePage);

            //预加载页面
            UserFrame.Resources.Add("UUID_homePage", homePage);
            UserFrame.Resources.Add("UUID_movieListPage", movieListPage);
            UserFrame.Resources.Add("UUID_schedulePage", schedulePage);
            UserFrame.Resources.Add("UUID_sitePage", sitePage);
            UserFrame.Resources.Add("UUID_paymentPage", paymentPage);
            UserFrame.Resources.Add("UUID_resultPage", resultPage);
            UserFrame.Resources.Add("UUID_refundMainPage", refundMainPage);
            //导航到主页
            GoHomePage();
            SetCurrentTitle("主页");
        }

        /// <summary>
        /// 设置当前页面名称
        /// </summary>
        /// <param name="CurrentPageName"></param>
        private void SetTitle(string CurrentPageName)
        {
            CurrentPageNameLabel.Content = CurrentPageName;
        }
        private void SetCurrentTitle(string Title)
        {
            CurrentPageNameLabel.Content = Title;
        }

        private void GoHome()
        {
            btn_home.Visibility = Visibility.Hidden;
            btn_back.Visibility = Visibility.Hidden;
            UserFrame.Navigate(homePage);
        }

        //主页
        public void GoHomePage()
        {
            if (currentPage.Count != 0)
            {
                string fromPage = currentPage.Pop();
                if (fromPage == "UUID_paymentPage")
                {
                    paymentPage.UnLockSites();
                }
                currentPage.Clear();
            }
            btn_back.Visibility = Visibility.Hidden;
            btn_home.Visibility = Visibility.Hidden;
            UserFrame.Navigate(homePage);
            homePage.GoBackPage();
            SetCurrentTitle("主页");
        }


        //电影列表页
        public void GoMovieList()
        {
            btn_home.Visibility = Visibility.Visible;
            currentPage.Push("UUID_movieListPage");
            btn_back.Visibility = Visibility.Hidden;

            UserFrame.Navigate(movieListPage);
            SetCurrentTitle("选择电影");
        }


        //档期列表页
        public void GoSchedulePage(List<Dictionary<string, string>> shcedules)
        {
            btn_home.Visibility = Visibility.Visible;
            currentPage.Push("UUID_schedulePage");
            btn_back.Visibility = Visibility.Visible;
            schedulePage.ShowSchedules(shcedules);
            UserFrame.Navigate(schedulePage);
            SetCurrentTitle("选择档期");
        }


        //选座页
        public void GoSitePage(Dictionary<string,string> dic)
        {
            btn_home.Visibility = Visibility.Visible;
            btn_back.Visibility = Visibility.Visible;

            currentPage.Push("UUID_sitePage");
            sitePage.ShowSiteGrid(dic);
            UserFrame.Navigate(sitePage);
            SetCurrentTitle("选择座位");
        }


        //付款页面
        public void GoPaymentPage(Dictionary<string, string> ScheduleDic, List<string> siteIDs)
        {
            btn_home.Visibility = Visibility.Visible;
            btn_back.Visibility = Visibility.Visible;

            currentPage.Push("UUID_paymentPage");
            
            paymentPage.ShowPrepareInfo(ScheduleDic, siteIDs);
            UserFrame.Navigate(paymentPage);
            SetCurrentTitle("付款");
        }
        //结果页面
        public void GoResultPage(Dictionary<string, string> scheduleDic, List<string> siteIDs, string paymentinfo)
        {
            btn_home.Visibility = Visibility.Visible;
            btn_back.Visibility = Visibility.Hidden;
            currentPage.Push("UUID_resultPage");
            resultPage.ShowResult(scheduleDic, siteIDs, paymentinfo);
            UserFrame.Navigate(resultPage);
            SetCurrentTitle("");
        }


        //退票页面
        public void GoRefundPage()
        {
            btn_home.Visibility = Visibility.Visible;
            currentPage.Push("UUID_refundMainPage");
            UserFrame.Navigate(refundMainPage);
            SetCurrentTitle("退票");
        }

        //回退
        public void GoBack(int i)
        {
            string fromPage = currentPage.Pop();
            if (fromPage == "UUID_movieListPage")
            {
                GoHomePage();
            }
            else if (fromPage == "UUID_schedulePage")
            {
                GoMovieList();
            }
            else if (fromPage == "UUID_sitePage")
            {
                UserFrame.Navigate(schedulePage);
            }
            else if (fromPage == "UUID_paymentPage")
            {
                App.Current.Dispatcher.Invoke((Action)(() =>
                {
                    if (i == 0)
                    {
                        paymentPage.UnLockSites();
                    }
                    sitePage.ClearSite();
                    UserFrame.Navigate(sitePage);
                }));

            }
            else if (fromPage == "UUID_resultPage")
            {

            }

        }
        private void Btn_home_Click(object sender, RoutedEventArgs e)
        {
            string name = (e.Source as Button).Name.ToString();
            if (name == "btn_home")
            {
                GoHomePage();
            }
            else if (name == "btn_back")
            {
                GoBack(0);
            }
        }
    }
}
