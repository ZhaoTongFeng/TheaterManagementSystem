using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Model;
namespace FinalProject
{
    public class MovieService:MovieDAO
    {
        /// <summary>
        /// 字典转类
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        protected string MovieDicToClass(Dictionary<string, string> dic,out Movie movie)
        {
            movie = null;
            string err_type = "";
            //类型检测
            int length = -1;
            if(int.TryParse(dic["Length"].Replace("分钟",""), out length) == false)
            {
                err_type += "电影长度转换Int类型失败";
                return err_type;
            }

            int price = -1;
            if(int.TryParse(dic["Price"], out price) == false)
            {
                err_type += "价格转换Int失败";
                return err_type;
            }

            DateTime date = new DateTime();
            if(DateTime.TryParse(dic["StartPlayingTime"], out date)==false)
            {
                err_type += "上映时间转换日期失败";
                return err_type;
            }

            DateTime enddate = new DateTime();
            if (DateTime.TryParse(dic["EndPlayingTime"], out enddate) == false)
            {
                err_type += "下映时间转换日期失败";
                return err_type;
            }

            int state = -1;
            for(int i=0;i< Config.MOVIE_STATE.Length; i++)
            {
                if (dic["State"] == Config.MOVIE_STATE[i])
                {
                    state = i;
                    break;
                }
            }
            if (state == -1)
            {
                err_type += "State转换失败";
                return err_type;
            }
            if (dic.ContainsKey("Id") == true)
            {
                movie = new Movie()
                {
                    Id = int.Parse(dic["Id"]),
                    Name = dic["Name"],
                    Length = length,
                    Type = dic["Type"],
                    Director = dic["Director"],
                    Scriptwriter = dic["Scriptwriter"],
                    Actor = dic["Actor"],
                    Country = dic["Country"],
                    Language = dic["Language"],
                    Price = price,
                    Introduce = dic["Introduce"],
                    img_path = dic["img_path"],
                    State = state,
                    StartPlayingTime = date,
                    EndPlayingTime = enddate

                };
            }
            else
            {
                movie = new Movie()
                {


                    Name = dic["Name"],
                    Length = length,
                    Type = dic["Type"],
                    Director = dic["Director"],
                    Scriptwriter = dic["Scriptwriter"],
                    Actor = dic["Actor"],
                    Country = dic["Country"],
                    Language = dic["Language"],
                    Price = price,
                    Introduce = dic["Introduce"],
                    img_path = dic["img_path"],
                    State = state,
                    StartPlayingTime = date,
                    EndPlayingTime = enddate

                };
            }

            return err_type;
        }
        /// <summary>
        /// 类转字典
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        protected Dictionary<string, string> MovieClassToDic(Movie movie)
        {
            DateTime date = (DateTime)(movie.StartPlayingTime);
            DateTime enddate = (DateTime)(movie.EndPlayingTime);
            Dictionary<string, string> keyValues = new Dictionary<string, string>()
            {
                {"Id",movie.Id.ToString() },
                {"Name",movie.Name },
                {"State",Config.MOVIE_STATE[movie.State] },
                {"Length",movie.Length.ToString()+"分钟" },
                {"Price",movie.Price.ToString() },
                {"StartPlayingTime",date.ToString("d") },
                {"EndPlayingTime",enddate.ToString("d") },
                {"Type",movie.Type },
                {"Country",movie.Country },
                {"Language",movie.Language },
                {"img_path",movie.img_path },
                {"Director",movie.Director },
                {"Scriptwriter",movie.Scriptwriter },
                {"Actor",movie.Actor },
                {"Introduce",movie.Introduce },
            };
            return keyValues;
        }

        //////////////////////////////////////////////////////////////////////////////////////用户控制器调用
        /// <summary>
        /// 获取正在上映电影信息
        /// </summary>
        /// <param name="movies"></param>
        /// <returns></returns>
        public string GetPlayingMovies(out List<Dictionary<string, string>> keyValuePairs)
        {
            List<Dictionary<string, string>> lockv = new List<Dictionary<string, string>>();
            string message_error = Select(Config.MOVIE_STATE_PLAYING,out List<Movie> movies);
            if (message_error == "")
            {
                foreach (Movie movie in movies)
                {
                    lockv.Add(MovieClassToDic(movie));
                }
            }
            keyValuePairs = lockv;
            return message_error;
        }


        //////////////////////////////////////////////////////////////////////////////////////管理员控制器调用
        /// <summary>
        /// 获取所有电影列表
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public string GetAllMovies(out List<Dictionary<string, string>> keyValuePairs)
        {
            List<Dictionary<string, string>> lockv = new List<Dictionary<string, string>>();
            string message_error = Select("", out List<Movie> movies);
            if (message_error == "")
            {
                foreach (Movie movie in movies)
                {
                    lockv.Add(MovieClassToDic(movie));
                }
            }
            keyValuePairs = lockv;
            return message_error;
        }
        /// <summary>
        /// 获取某天上映的电影
        /// </summary>
        /// <param name="date"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public string GetMoviesByDate(DateTime date, out List<Dictionary<string, string>> keyValuePairs)
        {
            List<Dictionary<string, string>> lockv = new List<Dictionary<string, string>>();
            string message_error = SelectByDate(Config.MOVIE_STATE_PLAYING, date, out List<Movie> movies);
            if (message_error == "")
            {
                foreach (Movie movie in movies)
                {
                    lockv.Add(MovieClassToDic(movie));
                }
            }
            keyValuePairs = lockv;
            return message_error;
        }

        /// <summary>
        /// 添加电影
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public string AddMovie(Dictionary<string, string> dic)
        {
            Movie movie = null;
            string err_type = "";
            err_type += MovieDicToClass(dic,out movie);
            if (err_type == "")
            {
                err_type += Insert(movie);
            }
            return err_type;
        }
        public string AddMovie(List<Dictionary<string, string>> dics)
        {
            List<Movie> movies = new List<Movie>();
            string err = "";
            foreach (Dictionary<string, string> dic in dics)
            {
                Movie movie = null;
                string err_type = MovieDicToClass(dic,out movie);
                if (err_type != "" && movie != null)
                {
                    movies.Add(movie);
                }
                else
                {
                    err += err_type;
                }
            }
            if (err == "")
            {
                err += Insert(movies);
            }
            return err;
        }

        /// <summary>
        /// 更新电影信息
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public string UpdateMovie(Dictionary<string, string> dic)
        {
            string err_type = "";
            err_type += MovieDicToClass(dic, out Movie movie);
            if (err_type == "")
            {
                err_type += Update(movie);
            }
            return err_type;
        }
        /// <summary>
        /// 更新电影状态
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public string UpdateMovieState(Dictionary<string,string> dic)
        {
            string err_type = "";
            err_type += MovieDicToClass(dic, out Movie movie);
            if (err_type == "")
            {
                err_type += UpdateState(movie);
            }
            return err_type;
        }
    }
}
