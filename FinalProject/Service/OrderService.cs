using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Model;
using FinalProject.UI;
namespace FinalProject
{
    class OrderService:OrderDAO
    {
        /// <summary>
        /// 类转字典
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected Dictionary<string, string> OrderClassToDic(Order order)
        {
            string states = "";
            for (int i = 0; i < order.States.Length; i++)
            {
                if (order.States[i] != ',')
                {
                    int index = int.Parse(order.States[i].ToString());
                    states += Config.ORDER_STATE[index];
                    if (i != order.States.Length - 1)
                    {
                        states += ",";
                    }
                }
            }
            Dictionary<string, string> keyValues = new Dictionary<string, string>()
            {
                {"Id",order.Id.ToString() },
                {"MovieName",order.Schedule.Movie.Name },
                {"ScheduleID",order.ScheduleID.ToString() },
                {"SiteIDs",order.SiteIDs },
                {"DateTime",order.DateTime.ToString() },
                {"PaymentID",order.PaymentId.ToString() },
                {"States",states }
            };
            return keyValues;
        }
        /// <summary>
        /// 字典转类
        /// </summary>
        /// <param name="OrderDic"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected string OrderDicToClass(Dictionary<string, string> OrderDic, out Order order)
        {
            order = null;
            string message_err = "";
            int ScheduleID = -1;
            if (int.TryParse(OrderDic["ScheduleId"],out ScheduleID) == false)
            {
                message_err += "档期Id类型错误";
                return message_err;
            }
            int PaymentId = -1;
            if (int.TryParse(OrderDic["PaymentId"],out PaymentId) == false)
            {
                message_err += "PaymentId类型错误";
                return message_err;
            }

            order = new Order()
            {
                ScheduleID = ScheduleID,
                SiteIDs = OrderDic["SiteIDs"],
                PaymentId = PaymentId,
                States = OrderDic["States"],
                DateTime = DateTime.Now
            };
            Insert(order);
            return message_err;
        }

        //////////////////////////////////////////////////////////////////////////////////////用户控制器调用


        /// <summary>
        /// 创建新订单
        /// </summary>
        /// <param name="OrderDic"></param>
        /// <returns></returns>
        public string AddOrder(Dictionary<string, string> OrderDic)
        {
            string message_err = "";
            message_err += OrderDicToClass(OrderDic, out Order order);
            if (message_err == "")
            {
                Insert(order);
            }
            return message_err;
        }

        public delegate void EventHandler(bool issucc);
        private event EventHandler CompletedPayment;
        public void StartPayment(int timer_index, string name, Dictionary<string, string> scheduleDic, List<string> siteIDs, EventHandler completePay)
        {
            CompletedPayment = completePay;
            //调用第三方支付平台接口获取二维码，等用户扫完码，第三方返回支付信息的时候再回调PaymentCompleted
            //这里没有办法实现，只能直接调用
            PaymentCompleted(timer_index,name, scheduleDic, siteIDs, completePay);
        }

        /// <summary>
        /// 完成订单，call前端页面跳转结果页面
        /// </summary>
        /// <returns></returns>
        public void PaymentCompleted(int timer_index,string name, Dictionary<string, string> scheduleDic, List<string> siteIDs, EventHandler completePay)
        {
            //暂停计时器不解锁座位只会在付款成功后调用
            UserMainWindow.userController.StopLookTimer(timer_index);
            string ids = "";
            string states = "";
            for (int i = 0; i < siteIDs.Count; i++)
            {
                ids += siteIDs[i];
                states += "1";
                if (i != siteIDs.Count - 1)
                {
                    ids += ",";
                    states += ",";
                }
            }
            Order order = new Order
            {
                ScheduleID = int.Parse(scheduleDic["Id"]),
                SiteIDs = ids,
                PaymentId = new Random().Next(1, 1000),
                States = states,
                DateTime = DateTime.Now
            };
            //插入订单信息
            Insert(order);

            bool isSuccess = true;
            CompletedPayment(isSuccess);
            
        }

        //////////////////////////////////////////////////////////////////////////////////////管理员控制器调用
        /// <summary>
        /// 获取所有订单
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public string GetAllOrders(out List<Dictionary<string, string>> keyValuePairs)
        {
            keyValuePairs = new List<Dictionary<string, string>>();
            string message_err = Select(-1, out List<Order> orders);
            if (message_err == "")
            {
                foreach (Order order in orders)
                {
                    keyValuePairs.Add(OrderClassToDic(order));
                }
            }
            return message_err;
        }
        /// <summary>
        /// 获取指定日期订单
        /// </summary>
        /// <param name="date"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public string GetOrdersByDate(DateTime date, out List<Dictionary<string, string>> keyValuePairs)
        {
            keyValuePairs = new List<Dictionary<string, string>>();
            string message_err = SelectByDate(date, out List<Order> orders);
            if (message_err == "")
            {
                foreach (Order order in orders)
                {
                    keyValuePairs.Add(OrderClassToDic(order));
                }
            }
            return message_err;
        }

        /// <summary>
        /// 统计
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="scheduleRate">档期就座率</param>
        /// <param name="priceDic">订单销售统计</param>
        /// <param name="movies">电影上座人数</param>
        /// <param name="total">总销售统计</param>
        /// <returns></returns>
        public string StatisticProcess(DateTime dateTime, out List<Dictionary<string, string>> scheduleRate, out List<Dictionary<string, string>> priceDic, out Dictionary<string, int> movies, out Dictionary<string,string> total)
        {
            total = new Dictionary<string, string>();
            priceDic = new List<Dictionary<string, string>>();
            scheduleRate = new List<Dictionary<string, string>>();//就座率
            List<Dictionary<string, string>> movieRate = new List<Dictionary<string, string>>();
            Dictionary<int, double> schedules = new Dictionary<int, double>();
            movies = new Dictionary<string, int>();//上座人数
            string message_err = SelectByDate(dateTime, out List<Order> orders);

            if (message_err == "")
            {
                //按照档期来统计
                double totalRate = 0;
                int movieCount = 0;
                int totalNum = 0;
                int totalPrice = 0;
                foreach (Order order in orders)
                {
                    Schedule schedule = order.Schedule;
                    Movie movie = schedule.Movie;

                    int sum = 0;
                    for (int i = 0; i < schedule.SiteState.Length; i++)
                    {
                        if (schedule.SiteState[i] == '1')
                        {
                            sum++;
                        }
                    }
                    double rate = (double)sum / (double)(schedule.SiteState.Length);

                    //档期就座率
                    if (schedules.ContainsKey(schedule.Id) == false)
                    {
                        schedules.Add(schedule.Id, rate);
                        totalRate += rate;
                        scheduleRate.Add(new Dictionary<string, string>()
                        {
                            {"档期Id",schedule.Id.ToString() },
                            {"电影名称",movie.Name },
                            {"就座率",rate.ToString() },
                            {"开场时间",schedule.StartTime.ToString() }
                        });
                    }
                    //电影上座人数
                    if (movies.ContainsKey(movie.Name) == false)
                    {
                        movies.Add(movie.Name, sum);
                        movieCount++;
                    }
                    else
                    {
                        movies[movie.Name] += sum;
                        totalNum += sum;
                    }
                    //销售额
                    int num = (order.SiteIDs.Length + 1) / 2;
                    int singleprice = num * movie.Price;
                    totalPrice += singleprice;
                    priceDic.Add(new Dictionary<string, string>() {
                        {"订单编号",order.Id.ToString() },
                        {"电影名称",movie.Name },
                        {"下单时间",order.DateTime.ToString() },
                        {"单价",movie.Price.ToString() },
                        {"票数",num.ToString() },
                        {"销售额",singleprice.ToString() }
                    });

                }
                if (schedules.Count == 0)
                {
                    total["平均就坐率"] = "0";
                }
                else
                {
                    total["平均就坐率"] = (totalRate / schedules.Count).ToString();
                }
                if (movieCount == 0)
                {
                    total["平均上座人数"] = "0";
                }
                else
                {
                    total["平均上座人数"] = (totalNum / movieCount).ToString();
                }
                total["总营业额"] = totalPrice.ToString();
            }
            return message_err;
        }
    }
}
