using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Windows;

namespace TTSHelper
{
    public class HttpHelper
    {
        public static string Put(string targetUrl, object postParam = null)
        {
            var h = HttpBase.Header;
            try
            {
                var result = Dos.Common.HttpHelper.Put(new Dos.Common.HttpParam()
                {
                    Url = $"{HttpBase.BaseUrl}{targetUrl}",
                    Headers = HttpBase.Header,
                    PostParam = postParam,
                    CookieContainer = HttpBase.CookieContainer
                });
                if (result == null) return "";
                return result;
                //JObject json = result.ToJObject();
                //if (json["status"].ToString() == "1")
                //{
                //    return result;
                //}
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                return "";
            }
        }
        public static T PutData<T>(string targetUrl, object postParam = null)
        {
            var h = HttpBase.Header;
            try
            {
                var result = Dos.Common.HttpHelper.Put(new Dos.Common.HttpParam()
                {
                    Url = $"{HttpBase.BaseUrl}{targetUrl}",
                    Headers = HttpBase.Header,
                    PostParam = postParam,
                    CookieContainer = HttpBase.CookieContainer
                });
                if (result == null) return default;

                JObject json = result.ToJObject();

                if (json != null && json["status"].ToString() == "1")
                {
                    return JsonConvert.DeserializeObject<T>(json["data"].ToString());
                }
                return default;
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
                if (ex.Response != null)
                    MessageBox.Show(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                return default;
            }
        }
        public static T Put<T>(string targetUrl, object postParam = null)
        {
            var h = HttpBase.Header;
            try
            {
                var result = Dos.Common.HttpHelper.Put(new Dos.Common.HttpParam()
                {
                    Url = $"{HttpBase.BaseUrl}{targetUrl}",
                    Headers = HttpBase.Header,
                    PostParam = postParam,
                    CookieContainer = HttpBase.CookieContainer
                });
                if (result == null) return default;
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
                if (ex.Response != null)
                    MessageBox.Show(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                return default;
            }
        }

        public static string Post(string targetUrl, object postParam = null)
        {
            var h = HttpBase.Header;
            try
            {
                var result = Dos.Common.HttpHelper.Post(new Dos.Common.HttpParam()
                {
                    Url = $"{HttpBase.BaseUrl}{targetUrl}",
                    Headers = HttpBase.Header,
                    PostParam = postParam,
                    CookieContainer = HttpBase.CookieContainer
                });
                if (result == null) return "";
                return result;
                //JObject json = result.ToJObject();
                //if (json["status"].ToString() == "1")
                //{
                //    return result;
                //}
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                return "";
            }
        }
        public static T Post<T>(string targetUrl, object postParam)
        {
            var h = HttpBase.Header;
            try
            {
                var result = Dos.Common.HttpHelper.Post(new Dos.Common.HttpParam()
                {
                    Url = $"{HttpBase.BaseUrl}{targetUrl}",
                    Headers = HttpBase.Header,
                    PostParam = postParam,
                    CookieContainer = HttpBase.CookieContainer
                });
                if (result == null) return default;              
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
                if(ex.Response!=null)
                MessageBox.Show(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                return default;
            }
        }
        public static T PostT<T>(string targetUrl, object postParam)
        {
            var h = HttpBase.Header;
            try
            {
                var result = Dos.Common.HttpHelper.Post(new Dos.Common.HttpParam()
                {
                    Url = $"{HttpBase.BaseUrl}{targetUrl}",
                    Headers = HttpBase.Header,
                    PostParam = postParam,
                    CookieContainer = HttpBase.CookieContainer
                });
                if (result == null) return default;

                var json = result.ToJObject<T>();

                if (json != null)
                {
                    return json;
                }
                return default;
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
                if (ex.Response != null)
                    MessageBox.Show(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                return default;
            }
        }
        public static string GetBase(string targetUrl)
        {
            var url = $"{HttpBase.BaseUrl}{targetUrl}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Set("Authorization", HttpBase.Authorization);
            request.Method = "GET";
            request.CookieContainer = HttpBase.CookieContainer;
            WebResponse response = request.GetResponse();
            var result = "";
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        public static T Get<T>(string targetUrl)
        {
            var result = GetBase(targetUrl);
            var json = JsonConvert.DeserializeObject<T>(result);
            return json;
        }

        public static JObject Get(string targetUrl)
        {
            var result = GetBase(targetUrl);
            var json = (JObject)JsonConvert.DeserializeObject(result);
            return json;
        }
    }
}