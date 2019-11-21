using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TTSVoice.Entitys; 

namespace TTSVoice.Speech
{
    /// <summary>
    /// 迅捷语音
    /// </summary>
    class SFSpeech:TTSSpeech
    {
        private string fileDir = System.AppDomain.CurrentDomain.BaseDirectory;
        public override void Spack(IEnumerable<SoundBlock> soundBlocks)
        {
            foreach (var sb in soundBlocks)
            {
                string filename = System.IO.Path.Combine(this.fileDir, sb.ToString() + ".mp3");
                if (!System.IO.File.Exists(filename))
                {
                    _downFile(filename, sb);
                }
                if (System.IO.File.Exists(filename))
                {
                    _play(filename);
                }

            }
        }
        [DllImport("winmm.dll")]
        public static extern bool PlaySound(string pszSound, int hmod, int fdwSound); 
        private void _play(string filename)
        {
            //自动接单
            //using (SoundPlayer player = new SoundPlayer(filename))
            //{
            //    player.Play();

            //} 
            //Mp3Player mp3Play = new Mp3Player()
            //{
            //    FileName = filename,
            //};
            //mp3Play.play(); 

            //WavPlayer.mciPlay(filename);
            Play(filename);
        }
        public static void Play(string filename)
        {
            using (var ms = File.OpenRead(filename))
            using (var rdr = new Mp3FileReader(ms))
            using (var wavStream = WaveFormatConversionStream.CreatePcmStream(rdr))
            using (var baStream = new BlockAlignReductionStream(wavStream))
            using (var waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
            {
                waveOut.Init(baStream);
                waveOut.Play();
                while (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void _downFile(string filename, SoundBlock sb)
        {

            var result = Dos.Common.HttpHelper.Post<SFResult>(new Dos.Common.HttpParam()
            {
                Url = @"https://user.api.hudunsoft.com/v1/alivoice/texttoaudio",
                Headers = new Dictionary<string, string>()
                        {
                             {"origin",@"http://voice-pc.xunjiepdf.com" },
                             {"user-agent",@"Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) voicedesktop/2.0.4 Chrome/66.0.3359.181 Electron/3.0.11 Safari/537.36" },
                             {"content-type","application/x-www-form-urlencoded " }
                        },
                PostParam = new
                {
                    client = "pc",
                    source = "335",
                    soft_version = "V2.0.4.0",
                    text = sb.Content,
                    bgid = 0,
                    bg_volume = 1,
                    format = "mp3",
                    voice = "aimei",
                    volume = sb.Volume,
                    speech_rate = sb.Rate,
                    title = sb.Content
                }

            });
            if (result != null && !string.IsNullOrWhiteSpace(result.data.file_link))
            {
                var r = Dos.Common.HttpHelper.GetStream(result.data.file_link);
                var s = System.IO.File.Create(filename);
                r.CopyTo(s);

                
                r.Close();
                s.Flush();
                s.Close();
            }
        }
    }
}
