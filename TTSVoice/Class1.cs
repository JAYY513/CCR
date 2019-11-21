using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTSVoice
{
    public class Class1
    {
        public static void test()
        {
            Proxy.ICallProxy callProxy = new Proxy.SFSpeechProxy();
            //callProxy.CallTableNumeric(5);
            callProxy.CallWelcome();


        }
    }
}
