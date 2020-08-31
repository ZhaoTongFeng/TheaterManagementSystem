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
    /// StatisticPage.xaml 的交互逻辑
    /// </summary>
    public partial class StatisticPage : Page
    {
        public StatisticPage()
        {
            InitializeComponent();
            //StatisticDataGrid
        }
        private void StatisticCalendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dynamic calendar = sender;
            //设置日期
            selectTime.Content = calendar.SelectedDate.ToString();

            AdminMainWindow.adminController.GetStatisticData(calendar.SelectedDate, out List<Dictionary<string, string>> scheduleRate, out List<Dictionary<string, string>> priceDic, out Dictionary<string, int> movies, out Dictionary<string, string> total);
            var q1 = from x in scheduleRate
                     select new
                     {
                         档期Id = x["档期Id"],
                         电影名称 = x["电影名称"],
                         就座率 = x["就座率"],
                         开场时间 = x["开场时间"],
                     };
            ScheduleDataGrid.ItemsSource = q1.ToList();

            MovieDataGrid.ItemsSource = movies;

            var q2 = from x in priceDic
                     select new
                     {
                         订单编号 = x["订单编号"],
                         电影名称 = x["电影名称"],
                         下单时间 = x["下单时间"],
                         单价 = x["单价"],
                         票数 = x["票数"],
                         销售额 = x["销售额"],
                     };
            OrderDataGrid.ItemsSource = q2.ToList();

            TotalPrice.Content = String.Format("总销售额: {0} 元",total["总营业额"]);
            AvgRate.Content = String.Format("上座率: {0} %",total["平均就坐率"]);
            AvgNum.Content = String.Format("平均上座人数: {0} 人",total["平均上座人数"]);
        }
    }
}
