using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TTSHelper.TTSAPI
{
    public class LoginApi
    {
        public static void Login(string user, string employeeAccount, string pwd, string code)
        {
            try
            {
                var result = Dos.Common.HttpHelper.Post(new Dos.Common.HttpParam()
                {
                    Url = $"{HttpBase.BaseUrl}authentication/form",
                    Headers = new Dictionary<string, string>()
                        {
                             {"origin",@"http://www.baidu.com" },
                             {"user-agent",@"Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) voicedesktop/2.0.4 Chrome/66.0.3359.181 Electron/3.0.11 Safari/537.36" },
                             {"content-type","application/json" },
                             //{"DEVICECODE",MacCodeHelper.getMacAddress8() },
                             //{"Authorization",MacCodeHelper.getMacAddress8() },
                        },
                    PostParam = new
                    {
                        username = user,
                        password = pwd,
                        code = code,
                        deviceCode = MacCodeHelper.getMacAddress8(),
                        merchantType = 1
                    }
                   ,
                    CookieContainer = HttpBase.CookieContainer
                });
                if (result != null)
                {
                    JObject json = result.ToJObject();
                    if (json["status"].ToString() == "1")
                    {
                        HttpBase.Authorization = json["data"].ToString();
                        MessageBox.Show("登录成功");
                    }
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
                if(ex.Response!= null)
                MessageBox.Show(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }
        }

        public static BitmapImage GetCodeUrl()
        {
            try
            {
                var url = $"{HttpBase.BaseUrl}loginValidateCode/{MacCodeHelper.getMacAddress8()}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = HttpBase.CookieContainer;
                WebResponse response = request.GetResponse();

                // 转换为byte类型
                System.IO.Stream stream = response.GetResponseStream();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
            catch
            {
                return null;
            }
        }
        public static System.IO.Stream GetCodeUrlStream()
        {
            try
            {
                var url = $"{HttpBase.BaseUrl}loginValidateCode/{MacCodeHelper.getMacAddress8()}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = HttpBase.CookieContainer;
                WebResponse response = request.GetResponse();

                //MemoryStream result = new MemoryStream();
                //response.GetResponseStream().CopyTo(result);
                //// 转换为byte类型
                //System.IO.Stream stream = response.GetResponseStream();
                //BitmapImage bitmapImage = new BitmapImage();
                //bitmapImage.BeginInit();
                //bitmapImage.StreamSource = stream;
                //bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                //bitmapImage.EndInit();

               // string sss = new StreamReader(result).ReadToEnd();

                return response.GetResponseStream(); ;
            }
            catch
            {
                return null;
            }
        }

    }
}