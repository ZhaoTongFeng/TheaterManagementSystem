using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Model;
namespace FinalProject
{
    public class OrderDAO
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string Select(int id,out List<Order> orders)
        {
            string messsage_err = "";
            orders = null;
            try
            {
                if (id == -1)
                {
                    var q = from t in dbContext.db.Order
                            select t;
                    orders = q.ToList();

                }
                else
                {
                    var q = from t in dbContext.db.Order
                            where t.Id == id
                            select t;
                    orders = q.ToList();
                }
            }
            catch(Exception ex)
            {
                messsage_err += "插入失败：" + ex.Source;
                
            }
            return messsage_err;

        }
        protected string SelectByDate(DateTime date, out List<Order> orders)
        {
            string messsage_err = "";
            orders = new List<Order>();

            try
            {
                foreach (Order item in dbContext.db.Order)
                {
                    if (item.DateTime.Date.ToString() == date.Date.ToString())
                    {
                        orders.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                messsage_err += "失败：" + ex.Source;

            }
            return messsage_err;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected string Insert(Order order)
        {
            string messsage_err = "";

            var db = dbContext.db.Database;
            db.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Order] ON");
            dbContext.db.Order.Add(order);
            dbContext.db.SaveChanges();
            db.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Order] OFF");


            return messsage_err;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected string Update(Order order)
        {
            string messsage_err = "";
            try
            {
                var q = from t in dbContext.db.Order
                        where t.Id == order.Id
                        select t;
                foreach (Order o in q)
                {

                    o.States = order.States;
                }
                dbContext.db.SaveChanges();
            }
            catch (Exception ex)
            {
                messsage_err += "插入失败：" + ex.Source;

            }
            return messsage_err;
        }
    }
}
