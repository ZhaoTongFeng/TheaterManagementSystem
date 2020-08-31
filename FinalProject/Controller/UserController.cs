
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FinalProject.Model;
namespace FinalProject
{
    public class UserController
    {
        private MovieService movieService = new MovieService();
        private ScheduleService scheduleService = new ScheduleService();
        private OrderService orderService = new OrderService();

        //电影
        /// <summary>
        /// 获取当前上映电影列表
        /// </summary>
        /// <returns></returns>
        public string LoadMovies(out List<Dictionary<string, string>> movies)
        {
            return movieService.GetPlayingMovies(out movies);
        }

        //档期
        /// <summary>
        /// 获取电影对应的档期列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string LoadSchedules(string name,out List<Dictionary<string, string>> keyValuePairs)
        {
            return scheduleService.GetPlayingSchedules(name,out keyValuePairs);
        }

        /// <summary>
        /// 加载上映电影最近的一个档期
        /// </summary>
        public string LoadNearSchedules(out List<Dictionary<string, string>> keyValuePairs)
        {
            return scheduleService.GetNearPlayingSchedules(out keyValuePairs);
        }

        /// <summary>
        /// 开始锁定座位
        /// </summary>
        /// <param name="scheduleID"></param>
        /// <param name="siteIDs"></param>
        /// <returns></returns>
        public int LockSite(Dictionary<string,string> schedule,List<string> siteIDs,SiteStateTimer.MyEventHandler1 myEventHandler)
        {
            return scheduleService.LockSites(schedule, siteIDs, myEventHandler);
        }

        /// <summary>
        /// 停止倒计时
        /// </summary>
        /// <param name="timer_index"></param>
        /// <returns></returns>
        public bool StopLookTimer(int timer_index)
        {
            return scheduleService.StopLookTimer(timer_index);
        }

        /// <summary>
        /// 取消锁定座位
        /// </summary>
        /// <param name="timer_index"></param>
        /// <returns></returns>
        public bool UnLookSite(int timer_index)
        {
            return scheduleService.UnLookSites(timer_index);
        }


        //订单
        //新增订单
        public string AddOrder(Dictionary<string, string> OrderDic)
        {
            return orderService.AddOrder(OrderDic);
        }
        //开始付款
        internal void StartPayment(int timer_index, string name, Dictionary<string, string> scheduleDic, List<string> siteIDs, OrderService.EventHandler completePay)
        {
            orderService.StartPayment(timer_index,name, scheduleDic, siteIDs, completePay);
        }
    }
}