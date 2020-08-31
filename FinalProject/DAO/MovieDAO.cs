using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Model;
namespace FinalProject
{
    public class MovieDAO
    {
        /// <summary>
        /// 插入电影
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected string Insert(Movie m)
        {
            string message_error = "";
            try
            {
                dbContext.db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Movie] ON");
                dbContext.db.Movie.Add(m);
                dbContext.db.SaveChanges();
                dbContext.db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Movie] OFF");
            }
            catch(Exception ex)
            {
                message_error += "插入异常" + ex.Source;
            }             
            return message_error;
        }
        protected string Insert(List<Movie> movies)
        {
            string message_error = "";
            try
            {
                dbContext.db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Movie] ON");
                foreach (Movie movie in movies)
                {
                    dbContext.db.Movie.Add(movie);
                    dbContext.db.SaveChanges();
                }
                dbContext.db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Movie] OFF");
            }
            catch (Exception ex)
            {
                message_error += "插入异常" + ex.Source;
            }
            return message_error;
        }

        /// <summary>
        /// 获取电影
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal string Select(string name,out List<Movie> movies)
        {
            string message_error = "";
            movies = null;
            try
            {
                if (name == "")
                {
                    var q = from t in dbContext.db.Movie
                            select t;
                    movies = q.ToList();
                }
                else
                {
                    var q = from t in dbContext.db.Movie
                            where t.Name == name
                            select t;
                    movies = q.ToList();
                }
            }
            catch (Exception ex)
            {
                message_error += "获取异常" + ex.Source;
            }
            return message_error;

        }
        internal string Select(int MOVIE_STATE,out List<Movie> movies)
        {
            string message_error = "";
            movies = null;
            try
            {
                var q = from t in dbContext.db.Movie
                        where t.State == MOVIE_STATE
                        select t;
                movies = q.ToList();
            }
            catch (Exception ex)
            {
                message_error += "Select异常" + ex.Source;
            }
            return message_error;
        }
        protected string SelectById(int id,out Movie movie)
        {
            string message_error = "";
            movie = null;
            try
            {

                var q = from t in dbContext.db.Movie
                        where t.Id == id
                        select t;
                movie = q.ToList()[0];

            }
            catch (Exception ex)
            {
                message_error += "获取异常" + ex.Source;
            }
            return message_error;
        }
        protected string SelectByDate(int mOVIE_STATE_PLAYING, DateTime date,out List<Movie> movies)
        {
            string message_error = "";
            movies = null;
            try
            {
                var q = from t in dbContext.db.Movie
                        where t.StartPlayingTime <= date && t.EndPlayingTime >= date && t.State== mOVIE_STATE_PLAYING
                        select t;
                movies = q.ToList();
            }
            catch (Exception ex)
            {
                message_error += "获取异常" + ex.Source;
            }
            return message_error;
        }
        /// <summary>
        /// 更新电影所有数据(除Id)
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected string Update(Movie m)
        {
            string message_error = "";
            try
            {
                var q = from t in dbContext.db.Movie
                        where t.Id == m.Id
                        select t;
                foreach (Movie movie in q)
                {
                    movie.Type = m.Type;
                    movie.Name = m.Name;
                    movie.Length = m.Length;
                    movie.Price = m.Price;
                    movie.State = m.State;

                    movie.StartPlayingTime = m.StartPlayingTime;
                    movie.EndPlayingTime = m.EndPlayingTime;

                    movie.Country = m.Country;
                    movie.Language = m.Language;
                    movie.img_path = m.img_path;

                    movie.Director = m.Director;
                    movie.Scriptwriter = m.Scriptwriter;
                    movie.Actor = m.Actor;
                    movie.Introduce = m.Introduce;
                    
                }
                dbContext.db.SaveChanges();
            }
            catch (Exception ex)
            {
                message_error += "更新异常" + ex.Source;
            }
            return message_error;

        }

        /// <summary>
        /// 只更新状态
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected string UpdateState(Movie m)
        {
            string message_error = "";
            try
            {
                var q = from t in dbContext.db.Movie
                        where t.Id == m.Id
                        select t;
                foreach (Movie movie in q)
                {
                    movie.State = m.State;
                }
                dbContext.db.SaveChanges();
            }
            catch (Exception ex)
            {
                message_error += "更新异常" + ex.Source;
            }
            return message_error;

        }
    }
}
