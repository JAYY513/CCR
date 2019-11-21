using System;
using System.Collections.Generic;

namespace TTSHelper
{
    public class HttpBase
    {
        public static string BaseUrl = "http://192.168.0.142:9090/";
        public static System.Net.CookieContainer CookieContainer = new System.Net.CookieContainer();

        private static string _authorization;
        private static Dictionary<string, string> _header;
        private static Random _random = new Random();

        public static string Authorization
        {
            get => _authorization;
            set
            {
                _authorization = "Bearer " + value;
                Header["Authorization"] = "Bearer " + value;
            }
        }

        public static Dictionary<string, string> Header
        {
            get
            {
                if (_header == null)
                {
                    _header = new Dictionary<string, string>()
                    {
                             {"origin",@"http://www.baidu.com" },
                             {"user-agent",@"Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) voicedesktop/2.0.4 Chrome/66.0.3359.181 Electron/3.0.11 Safari/537.36" },
                             {"content-type","application/json" },
                             {"DEVICECODE",MacCodeHelper.getMacAddress8()},
                             {"REQUESTUUID",""},
                             {"Authorization","" },
                    };
                }
                _header["REQUESTUUID"] = _random.Next(0, 999999).ToString();
                return _header;
            }
            set => _header = value;
        }
    }
}