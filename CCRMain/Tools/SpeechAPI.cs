using JLib.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTSVoice.Proxy;

namespace CCRMain.Tools
{
    public class SpeechAPI
    {
        public static SFSpeechProxy SFSpeechProxy = Singleton<SFSpeechProxy>.Instance;
    }
}
