
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FinalProject.Tools
{
    class TimeTools
    {
        /// <summary>
        /// 计算两个时间段之和，如果发生溢出则返回溢出后的时间
        /// 如果使用加号使两个时间相加，如果溢出会导致插入数据库失败
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static TimeSpan TimeAdd(TimeSpan t1, TimeSpan t2)
        {
            int minute = t1.Minutes + t2.Minutes;
            int hour = t1.Hours + t2.Hours + minute / 60;

            if (hour >= 24)
            {
                return new TimeSpan(0, minute % 60, 0);
            }
            else
            {
                return t1 + t2;
            }
        }
    }
}