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
    /// 将数据库中的数据显示到DataGrid中
    /// </summary>
    public partial class DatabasePage : Page
    {
        public delegate void MyEventHandler();
        public delegate void MyEventHandler2(Dictionary<string, string> movieDic);
        //跳转档期页面事件
        private event MyEventHandler2 GoToMovieInsertPage;
        private event MyEventHandler GoToScheduleInsertPage;
        private event MyEventHandler GoToStatisticsPage;

        //当前页面的类型
        private string currentName = "MovieInfo";
        //当前数据网格存放的数据
        private List<Dictionary<string, string>> keyValuePairs;

        public DatabasePage(MyEventHandler2 GoToMovieInsertPage, MyEventHandler GoToScheduleInsertPage, MyEventHandler GoToStatisticsPage)
        {
            InitializeComponent();
            this.GoToMovieInsertPage = GoToMovieInsertPage;
            this.GoToScheduleInsertPage = GoToScheduleInsertPage;
            this.GoToStatisticsPage = GoToStatisticsPage;
        }

        //刷新当前DB
        public void ShowDB()
        {
            ShowDB(currentName);
        }
        //点击上方的三个按钮，调用Show...方法显示数据到网格
        public void ShowDB(string name)
        {
            currentName = name;
            string message_error = "";
            if (currentName == "MovieInfo")
            {
                message_error += AdminMainWindow.adminController.LoadAllMovies(out keyValuePairs);
            }
            else if (currentName == "ScheduleInfo")
            {
                message_error += AdminMainWindow.adminController.LoadAllSchedules(out keyValuePairs);
            }
            else if (currentName == "OrderInfo")
            {
                message_error += AdminMainWindow.adminController.LoadAllOrder(out keyValuePairs);
            }

            if (message_error != "")
            {
                MessageBox.Show(message_error);
                return;
            }

            if (currentName == "MovieInfo")
            {
                ShowMovies(keyValuePairs);
                ShowMovieUI();
            }
            else if (currentName == "ScheduleInfo")
            {
                ShowSchedule(keyValuePairs);
                ShowScheduleUI();
            }
            else if (currentName == "OrderInfo")
            {
                ShowOrder(keyValuePairs);
                ShowOrderUI();
            }
        }

        //显示数据到网格
        private void ShowMovies(List<Dictionary<string, string>> keyValuePairs)
        {
            var q = from t in keyValuePairs
                    select new
                    {
                        ID=t["Id"],
                        名称 = t["Name"],
                        上映时间 = t["StartPlayingTime"],
                        下映时间 = t["EndPlayingTime"],
                        状态 = t["State"],
                        价格 = t["Price"],
                        图片 = t["img_path"],
                        时长 = t["Length"],
                        类型 = t["Type"],
                        语言 = t["Language"],
                        国家 = t["Country"],

                        导演 = t["Director"],
                        编剧 = t["Scriptwriter"],
                        演员 = t["Actor"],
                        简介 = t["Introduce"]
                    };
            DBDataGrid.ItemsSource = q.ToList();
        }
        private void ShowSchedule(List<Dictionary<string, string>> keyValuePairs)
        {
            var q = from t in keyValuePairs
                    select new
                    {
                        ID = t["Id"],
                        电影名称=t["MovieName"],
                        当前状态 = t["State"],
                        房间 = t["RoomName"],
                        票价 = t["Price"],
                        开场日期 = t["StartDate"],
                        开场时间 = t["StartTime"],
                        结束时间 = t["EndTime"],
                        座位剩余 = t["AllowSiteNum"],

                    };
            DBDataGrid.ItemsSource = q.ToList();
        }
        private void ShowOrder(List<Dictionary<string, string>> keyValuePairs)
        {

            var q = from t in keyValuePairs
                    select new
                    {
                        ID = t["Id"],
                        电影名称 = t["MovieName"],
                        档期ID = t["ScheduleID"],
                        座位 = t["SiteIDs"],
                        下单时间 = t["DateTime"],
                        付款ID = t["PaymentID"],
                        状态 = t["States"],

                    };
            DBDataGrid.ItemsSource = q.ToList();
        }

        ///显示和隐藏UI
        private void ShowMovieUI()
        {
            ImportBtn.Visibility = Visibility.Visible;
            EndPlayingBtn.Visibility = Visibility.Visible;
            ContinuePlayingBtn.Visibility = Visibility.Visible;
            StatisticsBtn.Visibility = Visibility.Hidden;
        }
        private void ShowScheduleUI()
        {
            ImportBtn.Visibility = Visibility.Visible;
            EndPlayingBtn.Visibility = Visibility.Hidden;
            ContinuePlayingBtn.Visibility = Visibility.Hidden;
            StatisticsBtn.Visibility = Visibility.Hidden;
        }
        private void ShowOrderUI()
        {
            ImportBtn.Visibility = Visibility.Hidden;
            EndPlayingBtn.Visibility = Visibility.Hidden;
            ContinuePlayingBtn.Visibility = Visibility.Hidden;
            StatisticsBtn.Visibility = Visibility.Visible;
        }

        //切换导入页面
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            if (currentName == "MovieInfo")
            {
                GoToMovieInsertPage(null);
            }
            else if (currentName == "ScheduleInfo")
            {
                GoToScheduleInsertPage();
            }
            else if (currentName == "OrderInfo")
            {
                GoToStatisticsPage();
            }
        }

        //改变电影状态
        private void ContinuePlayingBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DBDataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选中至少一条电影！");
                return;
            }
            string Name = (e.Source as Button).Name.ToString();
            string message = "";
            if(Name == "EndPlayingBtn")
            {
                message = "系统会根据时间自动下架电影，强制下架电影会导致该电影档期失效，是否下架电影";
            }
            else
            {
                message = "恢复上架将会恢复该电影档期，是否恢复电影";
            }
            if (MessageBox.Show(message, Name, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                foreach (dynamic item in DBDataGrid.SelectedItems)
                {
                    foreach (Dictionary<string, string> kv in keyValuePairs)
                    {
                        if (kv["Id"] == item.ID)
                        {

                            if (Name == "EndPlayingBtn")
                            {
                                kv["State"] = Config.MOVIE_STATE[Config.MOVIE_STATE_PASS];
                            }
                            else if (Name == "ContinuePlayingBtn")
                            {
                                kv["State"] = Config.MOVIE_STATE[Config.MOVIE_STATE_PLAYING];
                            }
                            AdminMainWindow.adminController.UpdateMovieState(kv);
                            break;
                        }
                    }
                }
                ShowDB();
            }
            

        }

        //双击切换编辑页面
        private void DBDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dynamic s = sender;
            if (currentName == "MovieInfo")
            {
                GoToMovieInsertPage(keyValuePairs[s.SelectedIndex]);
            }
            

        }

        //双击日历
        private void ScheduleCalendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dynamic calendar = sender;
            DateTime date = calendar.SelectedDate;

            string message_error = "";
            if (currentName == "MovieInfo")
            {
                message_error += AdminMainWindow.adminController.LoadMoviesByDate(date,out keyValuePairs);
            }
            else if (currentName == "ScheduleInfo")
            {
                message_error += AdminMainWindow.adminController.LoadSchedulesByDate(date,out keyValuePairs);
            }
            else if (currentName == "OrderInfo")
            {
                message_error += AdminMainWindow.adminController.LoadOrdersByDate(date,out keyValuePairs);
            }

            if (message_error != "")
            {
                MessageBox.Show(message_error);
                return;
            }

            if (currentName == "MovieInfo")
            {
                ShowMovies(keyValuePairs);
            }
            else if (currentName == "ScheduleInfo")
            {
                ShowSchedule(keyValuePairs);
            }
            else if (currentName == "OrderInfo")
            {
                ShowOrder(keyValuePairs);
            }
        }

        //点击显示所有按钮
        private void ShowAll_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowDB();
        }
    }
}
