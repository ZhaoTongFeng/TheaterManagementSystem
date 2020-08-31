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
using FinalProject.Tools;
namespace FinalProject.UI
{
    /// <summary>
    /// 进入付款页面，就通知服务器锁定座位并开始倒计时，等待用户下一部的操作
    /// 1.返回，用户想返回，或者下一个顾客点了主页（支付失败，解锁座位，关闭计时器，动作由客户端发出）
    /// 2.超时，顾客突然不想付款了，直接走了（支付失败，解锁座位，关闭计时器，动作由服务端发出）
    /// 3.付款成功，顾客付款成功，跳转出票页面，（支付成功，关闭计时器，动作由Orderservice发出）
    /// 4.付款失败，顾客付款失败，提示第三方错误信息（支付失败，解锁座位，关闭计时器，动作由Orderservice发出）
    /// 支付成功 对应是否解锁座位
    /// 一次操作结束关闭计时器
    /// </summary>
    public partial class PaymentPage : Page
    {
        public delegate void BackEventHandler();
        public delegate void BackEventHandler2(int i);
        public delegate void MyEventHandler(Dictionary<string, string> dic, List<string> siteIDs, string paymentinfo);
        private delegate void TimerDispatcherDelegate();
        private event MyEventHandler GoToResultPage;
        private event BackEventHandler2 GoBack;
        
        private Dictionary<string, string> scheduleDic;//由选座页面传入的档期信息
        private List<string> siteIDs;//已选座位下标
        private string paymentinfo;//第三方付款信息
        private Timer timercount;//计时器仅用于显示
        private int timer_index=-1;//服务端计时器下标（用于客户端调用停止倒计时并解锁座位）


        public PaymentPage(MyEventHandler fun, BackEventHandler2 fun2)
        {
            InitializeComponent();
            GoToResultPage += fun;
            GoBack += fun2;
        }


        /// <summary>
        /// 显示订单信息
        /// </summary>
        /// <param name="scheduleDic">档期信息</param>
        /// <param name="siteIDs">已选座位</param>
        public void ShowPrepareInfo(Dictionary<string, string> scheduleDic, List<string> siteIDs)
        {

            //重置和覆盖参数
            this.scheduleDic = scheduleDic;
            this.siteIDs = siteIDs;
            paymentinfo = "";
            timer_index = -1;

            //显示支付信息
            MovieName.Content = scheduleDic["MovieName"];
            DateTime.Content = scheduleDic["StartDate"].ToString()+ scheduleDic["StartTime"].ToString();
            RoomName.Content = scheduleDic["RoomName"];
            string s_sites = "";
            for(int i = 0; i < siteIDs.Count; i++)
            {
                s_sites+= int.Parse(siteIDs[i]) / 10+ "排" + int.Parse(siteIDs[i]) % 10 + "座";
                if (i != siteIDs.Count - 1)
                {
                    s_sites += ",";
                }
            }
            Sites.Content = s_sites;
            scheduleDic["TotalPrice"] = (siteIDs.Count * int.Parse(scheduleDic["Price"])).ToString();
            TotalPrice.Content = scheduleDic["TotalPrice"] + "￥";

            //通知服务器开始倒计时并锁定座位，服务器超时会自动解锁座位
            timer_index = UserMainWindow.userController.LockSite(scheduleDic, siteIDs, OnOverTime);

            //启动本地倒计时timer
            timercount = new Timer(1000);
            timercount.Elapsed += new ElapsedEventHandler(SetString);
            timercount.Enabled = true;
        }

        //倒计时效果,真正的倒计时还是在服务端
        private int number = 9;
        private void SetString(object sender, EventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new TimerDispatcherDelegate(UpdateUI));
        }
        private void UpdateUI() {
            btn_AutoBack.Content = string.Format("取消({0})", number);
            number--;
        }
        private void StopLocalTimer()
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                timercount.Dispose();
                number = 9;
                btn_AutoBack.Content = string.Format("取消({0})", number);
            }));
        }





        //1.返回或主页按钮被点击时
        internal void UnLockSites()
        {
            UserMainWindow.userController.UnLookSite(timer_index);
            StopLocalTimer();
        }
        //1.取消按钮点击时
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UnLockSites();
            GoBack(1);
        }

        //超时,直接返回上一级，由服务端调用
        public void OnOverTime()
        {
            timercount.Stop();
            MessageBox.Show("订单超时，请重新选择座位");
            StopLocalTimer();
            GoBack(1);
            return;
        }

        /// <summary>
        /// 模拟扫码付款成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PayChose_Click(object sender, RoutedEventArgs e)
        {
            StopLocalTimer();
            string name = (e.Source as Button).Name.ToString();//WeixinPay\AliPay
            UserMainWindow.userController.StartPayment(timer_index,name, scheduleDic, siteIDs, CompletePay);
        }

        /// <summary>
        /// 完成支付,服务端回调
        /// </summary>
        /// <param name="isSuccess"></param>
        public void CompletePay(bool isSuccess)
        {
            if (isSuccess)
            {
                GoToResultPage(scheduleDic,siteIDs,paymentinfo);//跳转结果页面
            }
            else
            {
                MessageBox.Show("付款失败，请重试");
                GoBack(0);
            }
        }



    }
}
