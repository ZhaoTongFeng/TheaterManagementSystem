using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class Config
    {
        /// <summary>
        /// 电影状态
        /// </summary>
        public static int MOVIE_STATE_COMING = 0;
        public static int MOVIE_STATE_PLAYING = 1;
        public static int MOVIE_STATE_PASS = 2;
        public static string[] MOVIE_STATE = { "即将上映", "正在上映", "已下架" };

        /// <summary>
        /// 档期状态
        /// </summary>
        public static int SCHEDULE_STATE_COMING = 0;
        public static int SCHEDULE_STATE_PLAYING = 1;
        public static int SCHEDULE_STATE_PASS = 2;
        public static string[] SCHEDULE_STATE = { "即将开场", "正在放映", "放映已结束" };

        /// <summary>
        /// 座位状态
        /// </summary>
        public static int SITE_STATE_NULL = 0;
        public static int SITE_STATE_SELECTED = 1;

        /// <summary>
        /// 订单状态
        /// </summary>
        public static int ORDER_STATE_NORMAL = 0;
        public static int ORDER_STATE_REFUNDED = 1;
        public static string[] ORDER_STATE = { "已退票", "购票成功" };

    }
}
