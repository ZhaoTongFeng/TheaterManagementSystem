using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Model;
namespace FinalProject
{
    public class ScheduleDAO
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string Select(int id,out List<Schedule> schedules)
        {
            string mess_err = "";
            schedules = null;
            try
            {
                if (id == -1)
                {
                    var q = from t in dbContext.db.Schedule
                            select t;
                    schedules = q.ToList();

                }
                else
                {
                    var q = from t in dbContext.db.Schedule
                            where t.Id == id
                            select t;
                    schedules = q.ToList();
                }
            }catch(Exception ex)
            {
                mess_err += "档期获取异常：" + ex.Source;
            }
            return mess_err;

        }
        protected string Select(string MovieName, out List<Schedule> schedules)
        {
            string mess_err = "";
            schedules = null;
            try
            {
                var q = from t in dbContext.db.Schedule
                        orderby t.StartTime
                        where t.Movie.Name == MovieName
                        select t;
                schedules = q.ToList();
            }catch(Exception ex)
            {
                mess_err += "档期获取异常：" + ex.Source;
            }
            return mess_err;

        }
        protected string SelectByDate(DateTime date, out List<Schedule> schedules)
        {
            string mess_err = "";
            schedules = null;
            try
            {
                var q = from t in dbContext.db.Schedule
                        orderby t.StartTime
                        where t.StartDate == date
                        select t;
                schedules = q.ToList();
            }
            catch (Exception ex)
            {
                mess_err += "档期获取异常：" + ex.Source;
            }
            return mess_err;
        }

        /// <summary>
        /// 插入档期
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        protected string Insert(Schedule schedule)
        {
            string mess_err = "";
            try
            {
                var db = dbContext.db.Database;
                db.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Schedule] ON");
                dbContext.db.Schedule.Add(schedule);
                dbContext.db.SaveChanges();
                db.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Schedule] OFF");
            }
            catch (Exception ex)
            {
                mess_err += "档期插入异常：" + ex.Source;
            }
            return mess_err;
        }
        protected string Insert(List<Schedule> schedules)
        {
            string mess_err = "";
            try
            {
                var db = dbContext.db.Database;
                db.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Schedule] ON");
                foreach (Schedule item in schedules)
                {
                    dbContext.db.Schedule.Add(item);
                }
                dbContext.db.SaveChanges();
                db.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Schedule] OFF");
            }
            catch (Exception ex)
            {
                mess_err += "档期插入异常：" + ex.Source;
            }
            return mess_err;
        }

        /// <summary>
        /// 更新档期基本信息
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        protected string Update(Schedule schedule)
        {
            string mess_err = "";
            try
            {
                var q = from t in dbContext.db.Schedule
                        where t.Id == schedule.Id
                        select t;
                foreach (Schedule s in q)
                {
                    s.MovieId = schedule.MovieId;
                    s.RoomName = schedule.RoomName;
                    s.StartTime = schedule.StartTime;

                }
                dbContext.db.SaveChanges();
            }
            catch (Exception ex)
            {
                mess_err += "档期插入异常：" + ex.Source;
            }
            return mess_err;
        }

        /// <summary>
        /// 更新档期状态
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        protected string UpdateSiteState(int scheduleID,string newSiteState)
        {
            string mess_err = "";
            try
            {
                var q = from t in dbContext.db.Schedule
                        where t.Id == scheduleID
                        select t;
                foreach (Schedule s in q)
                {
                    s.SiteState = newSiteState;
                }
                dbContext.db.SaveChanges();
            }
            catch (Exception ex)
            {
                mess_err += "档期插入异常：" + ex.Source;
            }
            return mess_err;
        }

    }
}
