using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Model;
using FinalProject.Tools;
namespace FinalProject
{
    //一个客户端同一时刻只对应一把锁
    public class SiteStateTimer: ScheduleDAO
    {
        public delegate bool MyEventHandler(int Timer_index);
        private event MyEventHandler Delete;
        public delegate void MyEventHandler1();
        private event MyEventHandler1 OverTimer;
        private System.Timers.Timer timer = new System.Timers.Timer();
        private Dictionary<string, string> Schedule { set; get; }
        private List<string> SiteIDs { set; get; }
        private int Timer_index { set; get; }//客户端唯一标识
        //构造函数保存档期和座位并锁定座位，开始倒计时
        public SiteStateTimer(MyEventHandler fun, Dictionary<string, string> Schedule, List<string> SiteIDs, int Timer_index, MyEventHandler1 OverTimer)
        {
            this.Schedule = Schedule;
            this.SiteIDs = SiteIDs;
            this.Timer_index = Timer_index;
            Delete += fun;
            this.OverTimer += OverTimer;

            LockSites();//锁定座位
            timer.Interval = 10000;//开始倒计时 
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
        //2.超时自动解锁并通知客户端
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            UnLockSite();
            StopTimer();
            OverTimer();//回调通知客户端
        }
        //销毁并从外部移除（回调）
        public bool StopTimer()
        {
            timer.Stop();
            timer.Dispose();
            Delete(Timer_index);
            return true;
        }
        //锁定座位
        private bool LockSites()
        {
            char[] SiteState = Schedule["SiteState"].ToCharArray();
            foreach (string Timer_index in SiteIDs)
            {
                SiteState[int.Parse(Timer_index)] = Config.SITE_STATE_SELECTED.ToString()[0];
            }
            string s = new string(SiteState);
            UpdateSiteState(int.Parse(Schedule["Id"]), s);
            return true;
        }
        //解锁座位
        public bool UnLockSite()
        {
            char[] SiteState = Schedule["SiteState"].ToCharArray();
            foreach (string Timer_index in SiteIDs)
            {
                SiteState[int.Parse(Timer_index)] = Config.SITE_STATE_NULL.ToString()[0];
            }
            string s = new string(SiteState);
            UpdateSiteState(int.Parse(Schedule["Id"]), s);
            return true;
        }
    }

    public class ScheduleService: ScheduleDAO
    {
        private MovieDAO movieDAO = new MovieDAO();

        //计算可选座位总数
        private int GetAllowSiteCount(string siteState)
        {
            int allow_site_count = 0;
            for (int i = 0; i < siteState.Length; i++)
            {
                if (siteState[i] == '0')
                {
                    allow_site_count++;
                }
            }
            return allow_site_count;
        }
        //计算结束时间
        private TimeSpan GetEndTime(int movieLength, TimeSpan StartTime)
        {
            
            TimeSpan length = new TimeSpan(movieLength / 60, movieLength % 60, 0);
            return TimeTools.TimeAdd(StartTime,length);
        }
        //档期类转字典
        protected Dictionary<string, string> ScheduleClassToDic(Schedule item)
        {
            int allow_site_count = GetAllowSiteCount(item.SiteState);//剩余座位
            TimeSpan endtime = GetEndTime(item.Movie.Length, item.StartTime);//结束时间
            Dictionary<string, string> kv = new Dictionary<string, string>()
            {    
                {"Id",item.Id.ToString() },
                {"MovieName",item.Movie.Name },
                {"MovieId",item.MovieId.ToString() },
                {"Price",item.Movie.Price.ToString() },
                {"RoomName",item.RoomName },
                {"StartDate",item.StartDate.ToString("d") },
                {"StartTime",item.StartTime.ToString() },
                {"EndTime",endtime.ToString() },
                {"SiteState",item.SiteState },
                {"State",Config.SCHEDULE_STATE[item.State] },
                {"AllowSiteNum",allow_site_count.ToString() },
            };
            return kv;
        }
        //档期字典转类
        protected string ScheduleDicToClass(Dictionary<string, string> keyValuePairs,out Schedule schedule)
        {
            schedule = null;
            string message_err = "";

            int movie_id = -1;
            if (int.TryParse(keyValuePairs["MovieId"], out movie_id) == false)
            {
                message_err += "movie_id类型错误";
                return message_err;
            }
            int State = -1;
            for (int i = 0; i < Config.SCHEDULE_STATE.Length; i++)
            {
                if(Config.SCHEDULE_STATE[i]== keyValuePairs["State"])
                {
                    State = i;
                }
            }
            if (State == -1)
            {
                message_err += "State类型错误";
                return message_err;
            }



            DateTime StartDate = new DateTime();
            TimeSpan StartTime = new TimeSpan();
            
            if(DateTime.TryParse(keyValuePairs["StartDate"], out StartDate)==false || TimeSpan.TryParse(keyValuePairs["StartTime"], out StartTime) == false)
            {
                message_err += "开场日期或时间类型错误";
                return message_err;
            }
            schedule = new Schedule()
            {
                MovieId = movie_id,
                RoomName = keyValuePairs["RoomName"],
                StartDate = StartDate,
                StartTime = StartTime,
                SiteState = keyValuePairs["SiteState"],
                State = State
            };
            return message_err;

        }


        //////////////////////////////////////////////////////////////////////////////////////用户控制器调用
        /// <summary>
        /// 获取正在上映的电影档期字典列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetPlayingSchedules(string name,out List<Dictionary<string, string>> keyValuePairs)
        {
            keyValuePairs = new List<Dictionary<string, string>>();
            List<Schedule> schedules = null;
            string message_err = Select(name,out schedules);
            foreach (Schedule item in schedules)
            {
                if (item.State == Config.SCHEDULE_STATE_COMING)
                {
                    keyValuePairs.Add(ScheduleClassToDic(item));
                }
            }
            return message_err;
        }
        /// <summary>
        /// 获取正在上映的电影最近档期
        /// </summary>
        /// <returns></returns>
        public string GetNearPlayingSchedules(out List<Dictionary<string, string>> keyValuePairs)
        {
            keyValuePairs = new List<Dictionary<string, string>>();
            string message_error = movieDAO.Select(Config.MOVIE_STATE_PLAYING, out List<Movie> movies);
            if (message_error == "")
            {
                foreach (Movie movie in movies)
                {
                    message_error += Select(movie.Name, out List<Schedule> schedules);
                    if (message_error == ""&& schedules.Count > 0)
                    {
                        Schedule schedule = schedules[0];
                        if (schedule.State == Config.SCHEDULE_STATE_COMING)
                        {
                            keyValuePairs.Add(ScheduleClassToDic(schedule));
                        }
                    }
                }
            }
            return message_error;
        }


        //倒计时类列表
        private List<SiteStateTimer> siteStateTimers = new List<SiteStateTimer>();
        //锁定座位，当跳转支付页面时调用
        public int LockSites(Dictionary<string, string> schedule, List<string> siteIDs, SiteStateTimer.MyEventHandler1 myEventHandler)
        {
            
            Console.WriteLine(schedule["SiteState"]);
            SiteStateTimer siteStateTimer = new SiteStateTimer(DeleteTimer,schedule,siteIDs,siteStateTimers.Count, myEventHandler);
            siteStateTimers.Add(siteStateTimer);
            //返回计时器下标
            return siteStateTimers.Count - 1;
        }
        //1&4.客户端停止倒计时并解锁座位
        public bool UnLookSites(int timer_index)
        {
            //首先执行解锁操作，在从列表中释放自己
            siteStateTimers[timer_index].UnLockSite();
            siteStateTimers[timer_index].StopTimer();
            return true;
        }
        //3.仅停止倒计时
        public bool StopLookTimer(int timer_index)
        {
            return siteStateTimers[timer_index].StopTimer();
        }
        //移除计时器
        private bool DeleteTimer(int timer_index)
        {
            siteStateTimers.RemoveAt(timer_index);
            return true;
        }

        //////////////////////////////////////////////////////////////////////////////////////管理员控制器调用
        /// <summary>
        /// 获取所有档期列表
        /// </summary>
        /// <returns></returns>
        public string GetAllSchedule(out List<Dictionary<string, string>> keyValuePairs)
        {
            keyValuePairs = new List<Dictionary<string, string>>();
            string message_err = Select(-1,out List<Schedule> schedules);
            if (message_err == "")
            {
                foreach (Schedule item in schedules)
                {
                    keyValuePairs.Add(ScheduleClassToDic(item));
                }
            }
            return message_err;
        }
        /// <summary>
        /// 获取指定日期档期
        /// </summary>
        /// <param name="date"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public string GetSchedulesByDate(DateTime date, out List<Dictionary<string, string>> keyValuePairs)
        {
            keyValuePairs = new List<Dictionary<string, string>>();
            string message_err = SelectByDate(date, out List<Schedule> schedules);
            if (message_err == "")
            {
                foreach (Schedule item in schedules)
                {
                    keyValuePairs.Add(ScheduleClassToDic(item));
                }
            }
            return message_err;
        }

        /// <summary>
        /// 新增档期
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public string AddSchedules(List<Dictionary<string, string>> keyValues)
        {
            string err = "";
            foreach (Dictionary<string, string> item in keyValues)
            {
                err+=ScheduleDicToClass(item, out Schedule s);
                err += Insert(s);
            }
            
            return err;
        }

        /// <summary>
        /// 排片模型
        /// 用于存放电影时长和场数
        /// </summary>
        private class MovieTimes
        {
            public int Id { get; set; }
            public int Length { get; set; }
            public int Times { get; set; }
        }

        /// <summary>
        /// 自动排制一天档期
        /// </summary>
        /// <param name="rate">电影权重</param>
        /// <param name="StartDate">开场日期</param>
        /// <param name="AutoSchedules">返回排制结果</param>
        /// <returns></returns>
        public string AutoSpawnSchedule(int[] rate, DateTime StartDate, out List<Dictionary<string, string>> AutoSchedules)
        {
            AutoSchedules = new List<Dictionary<string, string>>();
            int num_room = 5;//放映室数量
            string[] RoomNames = { "梅花厅", "兰花厅", "竹叶厅", "菊花厅", "玫瑰厅" };
            int breakTime = 20;//中场休息时间
            int businessTime = 840;//一天的放映时长：早上10点-晚上12点，14个小时*60分钟=840分钟
            TimeSpan StartPlayingTime = new TimeSpan(10, 0, 0);//第一部电影开始放映时间

            ///计算一天中整个电影院能播放的总(平均)场次（放映室数量*一天的放映时长/上映电影平均时长）
            //按道理这里应该是选择的那天上映的电影列表，这里为了简单直接用所有电影进行排制
            string message_error = movieDAO.Select(Config.MOVIE_STATE_PLAYING, out List<Movie> movies);
            if (message_error != "")
            {
                return message_error;
            }
            int sum_length = 0;//总电影时长
            foreach (Movie movie in movies)
            {
                sum_length += movie.Length;
            }
            int avg_length = sum_length / movies.Count;//电影平均时长
            int avg_num = num_room * businessTime / avg_length;//总(平均)场次

            ///计算每部电影分配到的电影场次（从大到小排序，尽量确保场次少的电影不会被安排在较早的时段）
            int sum_rate = 0;//总权重
            for (int i = 0; i < rate.Length; i++)
            {
                sum_rate += rate[i];
            }
            List<MovieTimes> num_movieTimes = new List<MovieTimes>();
            for (int i = 0; i < movies.Count; i++)
            {
                float n1 = rate[i];
                float n2 = sum_rate;
                float n3 = n1 / n2;
                float n4 = n3 * avg_num;
                int times = (int)(n4) + 1;
                MovieTimes id_times = new MovieTimes() { Id = movies[i].Id, Length = movies[i].Length, Times = times };
                num_movieTimes.Add(id_times);
            }
            List<MovieTimes> movieTimes = num_movieTimes.AsEnumerable().OrderBy(s => s.Times).ToList();

            ///开始排制
            int ptr = 0; //循环链表指针
            List<Schedule> schedules = new List<Schedule>();//档期列表
            for (int i = 0; i < num_room; i++)
            {
                ///生成档期顺序
                TimeSpan startTime = StartPlayingTime;
                List<MovieTimes> room_movieId = new List<MovieTimes>();
                ///从档期次数队列中循环添加到房间的放映列表中，直到当前的可用时长为0 或 没有电影可添加
                while (true)
                {
                    int empty = 0;
                    bool gate = true;
                    //获取即将添加的电影下标（计算循环队列的下标）
                    int index_movieTimes = ptr % movieTimes.Count;
                    ptr++;
                    if (gate == true)
                    {
                        //如果这个下标对应的电影场次已经为0了则查找下一个
                        while (movieTimes[index_movieTimes].Times <= 0)
                        {
                            empty++;
                            index_movieTimes = ptr % movieTimes.Count;
                            ptr++;
                            //如果都添加完了时间还没凑满，就取消这个限制,再添加一场之后结束本厅的添加
                            if (empty == movieTimes.Count)
                            {
                                gate = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                    //将电影添加到房间的播放序列中
                    MovieTimes m = new MovieTimes()
                    {
                        Id = movieTimes[index_movieTimes].Id,
                        Length = movieTimes[index_movieTimes].Length
                    };

                    //场数减1
                    movieTimes[index_movieTimes].Times -= 1;

                    ///生成档期
                    //实例化档期
                    string roomName = RoomNames[i];
                    string siteState = "";
                    for (int y = 0; y < 50; y++)
                    {
                        siteState += "0";
                    }
                    var q = from t in movies
                            where t.Id == m.Id
                            select t;
                    Schedule schedule = new Schedule()
                    {
                        MovieId = m.Id,
                        Movie = q.ToList()[0],
                        RoomName = roomName,
                        StartDate = StartDate,
                        StartTime = startTime,
                        SiteState = siteState,
                        State = Config.SCHEDULE_STATE_COMING
                    };
                    schedules.Add(schedule);

                    ///计算下一部电影的开始时间
                    //加上电影时长
                    TimeSpan movieLength = new TimeSpan(0, m.Length, 0);
                    startTime = TimeTools.TimeAdd(startTime, movieLength);
                    //加上休息时间
                    TimeSpan breakTimeSpan = new TimeSpan(0, breakTime, 0);
                    startTime = TimeTools.TimeAdd(startTime, breakTimeSpan);
                    if (startTime.Minutes % 10 != 0)
                    {
                        ///填充成整点
                        TimeSpan lerpTime = new TimeSpan(0, 10 - startTime.Minutes % 10, 0);
                        startTime = TimeTools.TimeAdd(startTime, lerpTime);
                    }

                    if(startTime.Hours>=0 && startTime.Hours < 10)
                    {
                        break;
                    }

                }
                ptr++;//往后偏移一个电影使同一部电影的时间差最大化
            }
            foreach (Schedule schedule in schedules)
            {
                AutoSchedules.Add(ScheduleClassToDic(schedule));
            } 
            return message_error;
        }


    }
}
