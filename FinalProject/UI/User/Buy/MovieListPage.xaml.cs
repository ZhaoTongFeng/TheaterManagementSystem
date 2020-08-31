using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace FinalProject.UI
{
    /// <summary>
    /// 显示正在上映的电影列表
    /// 如果数据库更新，必须重启程序
    /// </summary>
    public partial class MovieListPage : Page
    {
        public delegate void MyEventHandler(List<Dictionary<string, string>> shcedules);
        public event MyEventHandler GoToSchedulePage;//跳转档期页面事件
        List<string> movieNames = new List<string>();//当前电影名称的列表

        /// <summary>
        /// 构造函数绑定事件并显示正在上映的电影
        /// </summary>
        /// <param name="fun"></param>
        public MovieListPage(MyEventHandler fun)
        {
            InitializeComponent();
            GoToSchedulePage += fun;
            List<Dictionary<string, string>> keyValuePairs = null;
            string message_error = UserMainWindow.userController.LoadMovies(out keyValuePairs);
            if (message_error != "")
            {
                MessageBox.Show(message_error);
                return;
            }
            if (keyValuePairs.Count == 0)
            {
                MessageBox.Show("没有正在上映的电影");
                return;
            }

            int count = 0;
            string message_err = "";
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    //添加图片按钮
                    movieNames.Add(keyValuePairs[count]["Name"]);
                    string n = "btn_" + count;
                    Button btn_Click = new Button()
                    {
                        Name = n
                    };
                    btn_Click.Margin = new Thickness(8, 8, 8, 8);
                    MovieGrid.Children.Add(btn_Click);
                    btn_Click.SetValue(Grid.RowProperty, i * 2);
                    btn_Click.SetValue(Grid.ColumnProperty, j);
                    btn_Click.Click += new RoutedEventHandler(this.ClickMovieImage);
                    //设置图片
                    try
                    {
                        ImageBrush brush = new ImageBrush();
                        brush.ImageSource = new BitmapImage(new Uri("img/" + keyValuePairs[count]["img_path"], UriKind.Relative));
                        btn_Click.Background = brush;
                    }
                    catch (Exception esad)
                    {
                        message_err += esad.Source+"\n";
                        MessageBox.Show("图片加载错误" + message_err);
                        return;
                    }
                    //Label显示电影名称
                    Label label = new Label()
                    {
                        Content = keyValuePairs[count]["Name"],
                        FontSize = 18
                    };
                    MovieGrid.Children.Add(label);
                    label.SetValue(Grid.RowProperty, i * 2 + 1);
                    label.SetValue(Grid.ColumnProperty, j);

                    count++;
                    if (count == keyValuePairs.Count)
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 点击电影图片跳转电影档期页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickMovieImage(object sender, RoutedEventArgs e)
        {
            string btnName = (e.Source as Button).Name.ToString();
            string MovieName = movieNames[int.Parse(btnName.Replace("btn_", ""))];
            string err = UserMainWindow.userController.LoadSchedules(MovieName,out List<Dictionary<string,string>> schedulesDics);
            if (err != "")
            {
                MessageBox.Show(err);
                return;
            }
            GoToSchedulePage(schedulesDics);
        }


    }
}
