using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FinalProject.Model;
namespace FinalProject.Interface
{
    public interface PaymentInterface
    {
        /// <summary>
        /// 当用户到付款页面的时候
        /// 根据金额向第三方支付平台请求一个二维码
        /// 监听端口，等待用户扫码付款
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        Payment RequestQR(int price);

        /// <summary>
        /// 用户付款之后，第三方返回结果之后调用这个处理结果
        /// </summary>
        /// <param name="paymentInfo"></param>
        /// <returns></returns>
        bool OnExecutedQR(Payment payment);
    }

}
