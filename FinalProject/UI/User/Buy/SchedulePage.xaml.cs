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
    //档期页面
    public partial class SchedulePage : Page
    {
        public delegate void MyEventHandler(Dictionary<string,string> dic);
        //跳转选座页面
        private event MyEventHandler GoToSitePage;
        //当前页面显示的档期列表
        private List<Dictionary<string, string>> shcedules;
        public SchedulePage(MyEventHandler fun)
        {
            InitializeComponent();
            GoToSitePage += fun;
        }

        /// <summary>
        /// 显示档期数据
        /// </summary>
        /// <param name="MovieName"></param>
        public void ShowSchedules(List<Dictionary<string, string>> shcedules)
        {
            //清除List
            UserScheduleListBox.Items.Clear();
            //往Grid中添加Label，往List中添加Grid
            this.shcedules = shcedules;
            for (int i=0;i< shcedules.Count; i++)
            {
                Grid grid = new Grid()
                {
                    Name = "btn_" + i.ToString(),

                };
                grid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(40)
                });
                for(int y = 0; y < 6; y++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                }
                AddLabelToGrid(shcedules[i]["MovieName"], 0, grid);
                AddLabelToGrid(shcedules[i]["StartTime"], 1, grid);
                AddLabelToGrid(shcedules[i]["EndTime"], 2, grid);
                AddLabelToGrid(shcedules[i]["RoomName"], 3, grid);
                AddLabelToGrid(shcedules[i]["AllowSiteNum"], 4, grid);
                AddLabelToGrid(shcedules[i]["Price"], 5, grid);

                UserScheduleListBox.Items.Add(grid);
            }
        }

        /// <summary>
        /// Label模板
        /// </summary>
        /// <param name="content"></param>
        /// <param name="col"></param>
        /// <param name="grid"></param>
        private void AddLabelToGrid(string content,int col,Grid grid)
        {
            Label lab = new Label()
            {
                Content = content,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center

            };
            grid.Children.Add(lab);
            lab.SetValue(Grid.RowProperty, 0);
            lab.SetValue(Grid.ColumnProperty, col); 
        }

        /// <summary>
        /// 点击Item跳转选座页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserScheduleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic q = sender;
            int index = q.SelectedIndex;
            if (index != -1)
            {
                GoToSitePage(shcedules[index]);
            }
        }
    }
}
