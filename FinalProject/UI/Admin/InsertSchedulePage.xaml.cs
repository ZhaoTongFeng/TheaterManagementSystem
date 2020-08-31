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
using FinalProject.Model;
namespace FinalProject.UI
{
    /// <summary>
    /// 双击日历选择日期，
    /// 系统在列表中显示当天上映电影，
    /// 管理员设置分配权重，点击一键排制，
    /// 系统自动排制电影档期并显示，
    /// 管理员修改，
    /// 点击确定添加，
    /// 系统将档期添加到数据库
    /// </summary>
    public partial class InsertSchedulePage : Page
    {
        
        private List<Dictionary<string, string>> AutoScheduleDics;//暂存自动排制的档期数据
        private List<List<object>> objs;//ListboxItems
        private DateTime date = new DateTime();//SelectedDate

        public InsertSchedulePage()
        {
            InitializeComponent();
            Test();
        }

        //！！！测试用例，如果不用在构造函数中添加注释
        static bool isadd = false;
        private void Test()
        {
            if (InsertSchedulePage.isadd == true)
            {
                return;
            }
            //设置日期
            date = DateTime.Now;
            AdminMainWindow.adminController.LoadMoviesByDate(date, out List<Dictionary<string, string>> movies);
            CreateMovieListRow(movies);

            int[] rate = new int[objs.Count];
            int count = 0;
            foreach (List<object> list in objs)
            {
                rate[count] = int.Parse((list[1] as TextBox).Text);
                count++;
            }
            GetAutoSchedules(date, rate);
            InsertToDb();
            InsertSchedulePage.isadd = true;
        }

        //双击日历，显示当天上映的电影列表
        private void MCalendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dynamic calendar = sender;
            //设置日期
            date = calendar.SelectedDate;
            AdminMainWindow.adminController.LoadMoviesByDate(date,out List <Dictionary<string, string>> movies);
            CreateMovieListRow(movies);
        }

        //列表中显示当天上映的电影
        private void CreateMovieListRow(List<Dictionary<string, string>> movies)
        {
            PlayingMovieListbox.Items.Clear();
            //设置UI控件
            objs = new List<List<object>>();
            for (int i = 0; i < movies.Count; i++)
            {
                List<object> list = new List<object>();
                Grid grid = new Grid()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());

                Label titlelab = new Label()
                {
                    Content = movies[i]["Name"]
                };
                grid.Children.Add(titlelab);
                titlelab.SetValue(Grid.RowProperty, 0);
                titlelab.SetValue(Grid.ColumnProperty, 0);
                list.Add(titlelab);

                TextBox rateTextbox = new TextBox()
                {
                    Width = 25,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = "1"
                };
                grid.Children.Add(rateTextbox);
                rateTextbox.SetValue(Grid.RowProperty, 0);
                rateTextbox.SetValue(Grid.ColumnProperty, 1);
                list.Add(rateTextbox);

                Label numlab = new Label();
                grid.Children.Add(numlab);
                titlelab.SetValue(Grid.RowProperty, 0);
                titlelab.SetValue(Grid.ColumnProperty, 2);
                list.Add(numlab);

                PlayingMovieListbox.Items.Add(grid);
                objs.Add(list);
            }
        }


        //点击一键排制，获取自动排制的档期数据
        private void AutoSpawnScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            string err = "";
            if (date == new DateTime())
            {
                err += "请在日历上双击日期以选择";
                MessageBox.Show("添加失败:" + err);
                return;
            }
            int[] rate = new int[objs.Count];
            int count = 0;
            foreach (List<object> list in objs)
            {
                rate[count] = int.Parse((list[1] as TextBox).Text);
                count++;
            }
            GetAutoSchedules(date, rate);
            ShowAutoSchedules(AutoScheduleDics);
            SetMovieNum(AutoScheduleDics);
        }

        //获取自动排制的档期数据
        private void GetAutoSchedules(DateTime d,int[] r)
        {
            string err = AdminMainWindow.adminController.AutoInsertSchedule(r, d, out AutoScheduleDics);
            if (err != "")
            {
                MessageBox.Show(err);
            }
        }
        //TempGrid显示自动排制的档期数据
        private void ShowAutoSchedules(List<Dictionary<string, string>> AutoScheduleDics)
        {
            TempGrid.ItemsSource = null;
            var q = from t in AutoScheduleDics
                    select new
                    {
                        电影名称 = t["MovieName"],
                        房间 = t["RoomName"],
                        票价 = t["Price"],
                        开场日期 = t["StartDate"],
                        开场时间 = t["StartTime"],
                        结束时间 = t["EndTime"],
                    };
            TempGrid.ItemsSource = q.ToList();
        }
        //显示自动排制的电影数量统计
        private void SetMovieNum(List<Dictionary<string, string>> AutoScheduleDics)
        {
            Dictionary<string, int> scheduleNum = new Dictionary<string, int>();

            foreach (var item in AutoScheduleDics)
            {
                if (scheduleNum.ContainsKey(item["MovieName"]) == true)
                {
                    scheduleNum[item["MovieName"]] += 1;
                }
                else
                {
                    scheduleNum[item["MovieName"]] = 1;
                }
            }
            foreach (List<object> list in objs)
            {
                (list[2] as Label).Content = scheduleNum[(list[0] as Label).Content.ToString()];
            }
        }

        //插入数据库
        private string InsertToDb()
        {
            string err = "";
            if (date == new DateTime())
            {
                err += "请在日历上双击日期以选择";
                return err;
            }

            if (AutoScheduleDics == null)
            {
                err += "请先点击一键排制";
                return err;
            }

            err += AdminMainWindow.adminController.AddSchedules(AutoScheduleDics);
            if (err == "")
            {
                TempGrid.ItemsSource = null;
                AutoScheduleDics = null;
                PlayingMovieListbox.Items.Clear();
                objs = null;

            }
            return err;
        }
        //点击完成按钮，将经过修改的自动排制档期（暂时没有实现手动修改功能）插入到数据库
        private void CompleteSpawn_Click(object sender, RoutedEventArgs e)
        {
            string err = InsertToDb();
            if (err == "")
            {
                MessageBox.Show("添加成功");
            }
            else
            {
                MessageBox.Show("添加失败:"+err);
            }
            

        }

    }
}
