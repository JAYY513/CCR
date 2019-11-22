using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTSHelper
{
    public enum ResponseStatus
    {
        SUCCESS,
        WARNING,
        NULL
    }
    public class MyResponse
    {
        public int status { get; set; }
        public object data { get; set; }
        public string message { get; set; }
        /// <summary>
        /// status 为1，data不为空，并且data转换成功return true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool TryObject<T>(out T t)
        {
            if (data != null)
            {
                try
                {
                    if (status == 1)
                    {
                        t = JsonConvert.DeserializeObject<T>(this.data.ToString());
                        return true;
                    }
                    else
                    {
                        t = default(T);
                        return false;
                    }
                }
                catch
                {

                }
                finally
                {

                }
            }
            t = default(T);
            return false;
        }
        //public T DataToJson<T>()
        //{
        //}
    }

}
