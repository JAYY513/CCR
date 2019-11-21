using Newtonsoft.Json.Linq;

namespace TTSHelper.TTSAPI
{
    public class TableStatusApi
    {
        //public static void Login(string user, string employeeAccount, string pwd, string code)
        //{
        //    try
        //    {
        //        var result = Dos.Common.HttpHelper.Post(new Dos.Common.HttpParam()
        //        {
        //            Url = $"{HttpBase.BaseUrl}authentication/form",
        //            Headers = new Dictionary<string, string>()
        //                {
        //                     {"origin",@"http://www.baidu.com" },
        //                     {"user-agent",@"Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) voicedesktop/2.0.4 Chrome/66.0.3359.181 Electron/3.0.11 Safari/537.36" },
        //                     {"content-type","application/json" },
        //                     //{"DEVICECODE",MacCodeHelper.getMacAddress8() },
        //                     //{"Authorization",MacCodeHelper.getMacAddress8() },
        //                },
        //            PostParam = new
        //            {
        //                username = user,
        //                password = pwd,
        //                code = code,
        //                deviceCode = MacCodeHelper.getMacAddress8(),
        //                merchantType = 1
        //            }
        //           ,
        //            CookieContainer = HttpBase.CookieContainer
        //        });
        //        if (result != null)
        //        {
        //            JObject json = result.ToJObject();
        //            if (json["status"].ToString() == "1")
        //            {
        //                HttpBase.Authorization = json["data"].ToString();
        //                MessageBox.Show("登录成功");
        //            }
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        MessageBox.Show(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
        //    }
        //}

        public static JObject CheckTablestatus()
        {
            var json = HttpHelper.Get("tablestatus");
            return json;
        }

        /// <summary>
        /// 获取商家桌号列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="status">状态 0所有 1空闲 11占用</param>
        /// <returns></returns>
        public static T TableQuery<T>(string status = "0")
        {
            ///pageSize 最大显示Table数 页码自动根据 pageSize 设置最大页码数
            return HttpHelper.Post<T>(@"table/query", new
            {
                pageNum = "1",
                pageSize = "1000",
                status = status
            });;
        }


        public static T TableLineUp<T>(string number)
        {
            return HttpHelper.Put<T>(@"table/lineUp", new
            {
                number = number
            });
        }

        public static void ClearTable()
        {
            var a = HttpHelper.Put(@"table/clearTable");
        }

        /// <summary>
        /// 根据桌号获取排号列表
        /// </summary>
        public static void QueryLineUp(string id,string status)
        {
            var a = HttpHelper.Post(@"table/queryLineUp",new 
            {
                pageNum = "1",
                pageSize = "1000",
                status = status,
                tid = id
            });
        }
    }
}