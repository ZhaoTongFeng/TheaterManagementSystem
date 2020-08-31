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
    /// 在这个页面停留很久，数据过期问题
    /// </summary>
    public partial class SitePage : Page
    {
        public delegate void MyEventHandler(Dictionary<string, string> scheduleDic, List<string> siteIDs);
        private event MyEventHandler GoToPaymentPage;
        private Dictionary<string, string> scheduleDic;//从档期页面传入的档期信息
        private List<string> siteIDs = new List<string>();//当前选择的座位
        public SitePage(MyEventHandler fun)
        {
            InitializeComponent();
            GoToPaymentPage += fun;
        }


        /// <summary>
        /// 使用之前的档期信息显示座位情况
        /// 主要用在返回
        /// </summary>
        internal void ClearSite()
        {
            ShowSiteGrid(scheduleDic);
        }

        /// <summary>
        /// 根据Schedule.SiteState显示座位情况
        /// 1表示已选，0表示未选
        /// </summary>
        /// <param name="dic"></param>
        public void ShowSiteGrid(Dictionary<string,string> dic)
        {
            siteIDs.Clear();
            scheduleDic = dic;
            List<char> l = dic["SiteState"].ToList();
            int count = 0;
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {

                        Button btn_Click = new Button()
                        {
                            Name = "btn_" + count.ToString(),

                            Content = count.ToString()
                        };
                        btn_Click.Margin = new Thickness(4, 4, 4, 4);
                        SiteGrid.Children.Add(btn_Click); //添加到Grid控件
                        btn_Click.SetValue(Grid.RowProperty, i + 1);
                        btn_Click.SetValue(Grid.ColumnProperty, j + 1);
                        btn_Click.Click += new RoutedEventHandler(this.ClickSiteButton);



                        if (int.Parse(l[count].ToString()) == 0)
                        {
                            btn_Click.Background = Brushes.Black;
                        }
                        else if (int.Parse(l[count].ToString()) == 1)
                        {
                            btn_Click.Background = Brushes.Blue;
                        }


                        count++;
                        if (count == l.Count)
                        {
                            return;
                        }
                    }
                }
            }));




        }

        /// <summary>
        /// 点击座位
        /// 根据背景图片判断是选择还是取消选择
        /// 如果是选择就把该座位的ID存到siteIDs列表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickSiteButton(object sender, RoutedEventArgs e)
        {
            
            Button button = (e.Source as Button);
            string count = button.Content.ToString();
            if (button.Background == Brushes.Red)
            {
                //取消
                button.Background = System.Windows.Media.Brushes.Black;
                siteIDs.Remove(count);
            }
            else if (button.Background == Brushes.Black)
            {
                //选择
                button.Background = System.Windows.Media.Brushes.Red;
                siteIDs.Add(count);
            }
            else
            {
                //不可选
            }
            messageLab.Content = String.Format("已选{0}个座位，总计：{1}￥",siteIDs.Count,siteIDs.Count*int.Parse(scheduleDic["Price"]));

            //这里还差动态更新当前选择的信息
        }

        /// <summary>
        /// 完成选座，点击下单跳转付款页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompleteChoseSite_Click(object sender, RoutedEventArgs e)
        {
            if (siteIDs.Count == 0)
            {
                MessageBox.Show("请至少选择一个座位");
                return;
            }
            else
            {
                GoToPaymentPage(scheduleDic, siteIDs);
            }
        }


    }
}
