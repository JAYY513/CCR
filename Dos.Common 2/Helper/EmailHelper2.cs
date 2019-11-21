using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Dos.Common.Helper
{
    public class EmailHelper2
    {
        public static void Send(string username,string password,string host,int port ,bool usessl,string sendto,string title,string content,List<string> cc,string attach)
        {
            SmtpClient client = null;
            Attachment attachment = null;
            try
            {
                //string username = "tts_18025251937@163.com";
                //string password = "NBTX22177577";
                //string host = "smtp.163.com";
                //int port = 25;
                //bool usessl = false;
                //string sendto = "tts_18025251937@163.com";
                //string title = "日志文件";
                //string content = "日志文件测试";


                client = new SmtpClient();
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential(username, password);//用户名、密码
                client.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式    
                client.Host = host;//邮件服务器
                client.Port = port;//端口号 非SSL方式，默认端口号为：25 
                client.EnableSsl = usessl; //经过ssl加密    

                MailMessage msg = new MailMessage();
                //加发件人 
                msg.To.Add(sendto);
                msg.From = new MailAddress(username);//发件人地址
                msg.Subject = title;//邮件标题   
                msg.Body = content;//邮件内容   
                msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码   
                msg.IsBodyHtml = true;//是否是HTML邮件   
                msg.Priority = MailPriority.High;//邮件优先级    

                if (string.IsNullOrWhiteSpace(attach) == false)
                {
                    attachment = new Attachment(attach);
                    msg.Attachments.Add(attachment);
                }
                //cc = new List<string>() { "2212642@qq.com" };                
                if (cc != null && cc.Count > 0)
                {
                    foreach (var item in cc)
                    {
                        msg.CC.Add(item);
                        break;
                    }
                }
                client.Send(msg);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (client!=null)
                {
                    client.Dispose();
                }
                if (attachment != null)
                { 
                    attachment.Dispose();
                }
            }
        }
    }
}
