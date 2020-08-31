
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FinalProject.Model;
namespace FinalProject
{
    public class AdminController
    {
        private MovieService movieService = new MovieService();
        private ScheduleService scheduleService = new ScheduleService();
        private OrderService orderService = new OrderService();

        /// 电影
        //获取所有电影
        public string LoadAllMovies(out List<Dictionary<string, string>> keyValuePairs)
        {
            return movieService.GetAllMovies(out keyValuePairs);
        }
        //获取某日上映电影列表
        public string LoadMoviesByDate(DateTime date,out List<Dictionary<string, string>> keyValuePairs)
        {
            return movieService.GetMoviesByDate(date,out keyValuePairs);
        }
        //添加电影
        public string AddMovie(Dictionary<string,string> dic)
        {
            return movieService.AddMovie(dic);
        }
        public string AddMovie(List<Dictionary<string,string>> movies)
        {
            return movieService.AddMovie(movies);
        }
        //更新电影
        public string UpdateMovie(Dictionary<string, string> dic)
        {
            return movieService.UpdateMovie(dic);
        }
        //更新电影状态
        public string UpdateMovieState(Dictionary<string, string> dic)
        {
            return movieService.UpdateMovieState(dic);
        }

        /// 档期
        //获取所有档期
        public string LoadAllSchedules(out List<Dictionary<string, string>> keyValuePairs)
        {
            return scheduleService.GetAllSchedule(out keyValuePairs);
        }
        //获取某日档期
        internal string LoadSchedulesByDate(DateTime date, out List<Dictionary<string, string>> keyValuePairs)
        {
            return scheduleService.GetSchedulesByDate(date,out keyValuePairs);
        }
        //获取自动排制的档期
        public string AutoInsertSchedule(int[] rate, DateTime StartDate, out List<Dictionary<string, string>> AutoSchedules)
        {
            return scheduleService.AutoSpawnSchedule(rate, StartDate, out AutoSchedules);
        }
        //添加档期
        public string AddSchedules(List<Dictionary<string, string>> keyValuePairs)
        {
            return scheduleService.AddSchedules(keyValuePairs);
        }

        /// 订单
        //获取所有订单
        public string LoadAllOrder(out List<Dictionary<string, string>> keyValuePairs)
        {
            return orderService.GetAllOrders(out keyValuePairs);
        }
        //加载某日订单
        internal string LoadOrdersByDate(DateTime date, out List<Dictionary<string, string>> keyValuePairs)
        {
            return orderService.GetOrdersByDate(date,out keyValuePairs);
        }

        /// <summary>
        /// 根据日期获取销售统计数据
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="scheduleRate"></param>
        /// <param name="priceDic"></param>
        /// <param name="movies"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public string GetStatisticData(DateTime dateTime, out List<Dictionary<string, string>> scheduleRate, out List<Dictionary<string, string>> priceDic, out Dictionary<string, int> movies, out Dictionary<string, string> total)
        {
            return orderService.StatisticProcess(dateTime, out scheduleRate, out priceDic, out movies, out total);
        }


    }
}