
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FinalProject.UI
{
    /// <summary>
    /// AdminMainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public static AdminController adminController = new AdminController();

        private DatabasePage dbpage;
        private InsertMoviePage insertMoviePage;
        private InsertSchedulePage insertSchedulePage;
        private StatisticPage statisticPage;

        public AdminMainWindow()
        {
            InitializeComponent();
            dbpage = new DatabasePage(GoToMovieInsertPage,GoToScheduleInsertPage,GoToStatisticsPage);
            insertMoviePage = new InsertMoviePage();
            insertSchedulePage = new InsertSchedulePage();
            statisticPage = new StatisticPage();

            mFrame.Resources.Add("UUID_1", dbpage);
            mFrame.Resources.Add("UUID_2", insertMoviePage);
            mFrame.Resources.Add("UUID_3", insertSchedulePage);
            mFrame.Resources.Add("UUID_4", statisticPage);

            dbpage.ShowDB();
            mFrame.Navigate(dbpage);
        }

        //切换数据表显示内容
        private void SelectDbDisplayType(object sender, RoutedEventArgs e)
        {
            string name = (e.Source as Button).Name.ToString();
            dbpage.ShowDB(name);
            mFrame.Navigate(dbpage);
        }

        //跳转插入电影页面
        private void GoToMovieInsertPage(Dictionary<string, string> movieDic)
        {
            mFrame.Navigate(insertMoviePage);
            if (movieDic != null)
            {
                insertMoviePage.Edit(movieDic);
            }
        }
        //跳转插入档期页面
        private void GoToScheduleInsertPage()
        {
            mFrame.Navigate(insertSchedulePage);
        }
        //跳转统计页面
        private void GoToStatisticsPage()
        {
            mFrame.Navigate(statisticPage);
        }
    }
}
