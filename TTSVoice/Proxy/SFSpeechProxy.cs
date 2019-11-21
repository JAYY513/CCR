using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTSVoice.Proxy
{
    /// <summary>
    ///  迅捷语音叫号代理
    /// </summary>
    public class SFSpeechProxy : ICallProxy
    {
        private  Speech.SFSpeech SFSpeech = new Speech.SFSpeech();
        public SFSpeechProxy()
        {
            SFSpeech.Start();
        }

        public void CallWelcome(string systemName)
        {
            SFSpeech.AddBlocks(new List<Entitys.SoundBlock>() {
                new Entitys.SoundBlock() { Content="欢迎使用" + systemName + "系统", Volume=10, Voice="aimei", Rate=5 },

            });
        }

        public void CallTableNumeric(int queueNum)
        {
            SFSpeech.AddBlocks(new List<Entitys.SoundBlock>() {
                new Entitys.SoundBlock() { Content="请", Volume=10, Voice="aimei", Rate=1 },
                new Entitys.SoundBlock() { Content=queueNum.ToString(), Volume=10, Voice="aimei", Rate=3 },
                new Entitys.SoundBlock() { Content=queueNum+"号顾客 ", Volume=10, Voice="aimei", Rate=5 }

            });
        }

        public void CallWaiter(int tableNum)
        {
            throw new NotImplementedException();
        }

        public void LeaveTableNumeric(int tableNum)
        {
            throw new NotImplementedException();
        }
    }
}
