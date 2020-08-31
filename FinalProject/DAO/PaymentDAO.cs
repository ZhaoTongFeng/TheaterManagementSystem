using FinalProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class PaymentDAO
    {
        /// <summary>
        /// 获取支付信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="payments"></param>
        /// <returns></returns>
        protected string Select(int id, List<Payment> payments)
        {
            string message_error = "";
            try
            {
                if (id == -1)
                {
                    var q = from t in dbContext.db.Payment
                            select t;
                    payments = q.ToList();


                }
                else
                {
                    var q = from t in dbContext.db.Payment
                            where t.Id == id
                            select t;
                    payments = q.ToList();
                }
            }catch(Exception ex)
            {
                message_error += "查询异常："+ ex.Source;

            }
            return message_error;
        }

        /// <summary>
        /// 插入支付信息
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        protected string Insert(Payment payment)
        {
            string message_error = "";
            try
            {
                dbContext.db.Payment.Add(payment);
                dbContext.db.SaveChanges();
            }catch(Exception ex)
            {
                message_error += "插入异常："+ ex.Source;
            }
            return message_error;
        }
    }
}
