using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTSVoice.Proxy
{
    public interface ICallProxy
    {
        /// <summary>
        /// 清理台号（客人用餐结束）
        /// </summary>
        /// <param name="tableNum"></param>
        void LeaveTableNumeric(int tableNum);

        /// <summary>
        /// 欢迎使用
        /// </summary>
        /// <param name="tableNum"></param>
        void CallWelcome(string systemName="本");

        /// <summary>
        /// 叫号
        /// </summary>
        /// <param name="tableNum"></param>
        void CallTableNumeric(int tableNum);


        /// <summary>
        /// 呼叫服务员
        /// </summary>
        /// <param name="tableNum"></param>
        void CallWaiter(int tableNum);

    }
}
