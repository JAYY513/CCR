using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Windows;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TTSHelper.TTSAPI
{
   
    public static class TableStatusApi2
    {
        public static T TryTableQuery<T>(string status = "0", string type = "")
        {
            var data = default(T);
            ///pageSize 最大显示Table数 页码自动根据 pageSize 设置最大页码数
            var myResponse = HttpHelper.Post<MyResponse>(@"table/query", new
            {
                pageNum = "1",
                pageSize = "1000",
                type = type,
                status = status
            });
            if (myResponse == null)
            {
                return data;
            }
            if (myResponse.TryObject<T>(out data))
            {

            }
            else
            {

            }
            return data;
        }

        public static T TryTableLineUp<T>(string number)
        {
            var data = default(T);
            var myResponse = HttpHelper.Put<MyResponse>(@"table/lineUp", new
            {
                number = number
            });
            if (myResponse == null)
            {
                return data;
            }
            if (myResponse.TryObject<T>(out data))
            {

            }
            else
            {

            }
            return data;
        }

        public static ResponseStatus TryTableQuery<T>(out MyResponse myResponse,out T data, string status = "0", string type = "")
        {
            data = default(T);
            ///pageSize 最大显示Table数 页码自动根据 pageSize 设置最大页码数
            myResponse = HttpHelper.Post<MyResponse>(@"table/query", new
            {
                pageNum = "1",
                pageSize = "1000",
                type = type,
                status = status
            });
            if (myResponse == null)
            {                
                return ResponseStatus.NULL;
            }
            return myResponse.TryObject<T>(out data) ? ResponseStatus.SUCCESS : ResponseStatus.WARNING;
        }

        public static ResponseStatus TryTableLineUp<T>(string number, out T data,out MyResponse myResponse)
        {
            data = default(T);
            myResponse = HttpHelper.Put<MyResponse>(@"table/lineUp", new
            {
                number = number
            });
            if (myResponse == null)
            {
                return ResponseStatus.NULL;
            }
            return myResponse.TryObject<T>(out data) ? ResponseStatus.SUCCESS : ResponseStatus.WARNING;
        }
    }
    public class TableStatusApi
    {            
        public static T TableLineUp<T>(string number)
        {
            return HttpHelper.PutData<T>(@"table/lineUp", new
            {
                number = number
            });
        }

        public static void ClearTable()
        {
            var a = HttpHelper.Put(@"table/clearTable");
        }

        /// <summary>
        /// QueryLineUp
        /// </summary>
        /// <param name="tableSize">1-4</param>
        public static T QueryLineUp<T>(string tableSize)
        {
            return HttpHelper.PostT<T>(@"table/queryLineUp",new 
            {
                pageNum = "1",
                pageSize = "1000",
                tableSize = tableSize
            });
        }

        /// <summary>
        /// 根据桌号获取排号列表
        /// </summary>
        public static void QueryQueryNum(string id, string status)
        {
            var a = HttpHelper.Post(@"table/queryNum");
        }

        public static void ChangeLineUp(string lid, string tid, string type)
        {
            var a = HttpHelper.Post(@"table/changeLineUp", new
            {
                lid = lid,
                tid = tid,
                type = type
            });
        }
    }
}