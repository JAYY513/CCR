using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TTSVoice.Entitys;

namespace TTSVoice.Speech
{
    public class TTSSpeech
    {
        /// <summary>
        /// 语音队列
        /// </summary>
        private Queue<IEnumerable<SoundBlock>> soundBlocks = new Queue<IEnumerable<SoundBlock>>(10);
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private Task _backgroundSpackTask = null;
        private object _locakObject = new object();
        /// <summary>
        /// 下个语音等待秒数
        /// </summary>
        protected virtual int SpaceNextSleep { get; set; } = 1;
        public TTSSpeech()
        {

        }
        /// <summary>
        /// 开始监听语音队列
        /// </summary>
        public void Start()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            if (_backgroundSpackTask != null)
                _backgroundSpackTask.Wait();

            var token = cancellationTokenSource.Token;

            _backgroundSpackTask = Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {

                    IEnumerable<SoundBlock> bks = null;
                    lock (_locakObject)
                    {
                        if (soundBlocks.Count > 0)
                        {
                            bks = this.soundBlocks.Dequeue();
                        }
                    }
                    if (bks != null)
                    {
                        try
                        {
                            this.Spack(bks);
                        }
                        catch(Exception ex)
                        {

                        }
                        
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(this.SpaceNextSleep));
                }
            }, TaskCreationOptions.LongRunning);
        }
        /// <summary>
        /// 结束语音监听
        /// </summary>
        public void Stop()
        {
            this.cancellationTokenSource.Cancel();
            if (this._backgroundSpackTask != null)
                this._backgroundSpackTask.Wait();
        }
        /// <summary>
        /// 添加语句块
        /// </summary>
        /// <param name="soundBlocks"></param>
        public void AddBlocks(IEnumerable<SoundBlock> soundBlocks)
        {
            lock (this._locakObject)
            {
                this.soundBlocks.Enqueue(soundBlocks);
            }
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="soundBlocks"></param>
        public virtual void Spack(IEnumerable<SoundBlock> soundBlocks)
        {
            
                using (var s = new SpeechSynthesizer())
                {
                    foreach (var r in soundBlocks)
                    {
                        s.Volume = r.Volume;
                        s.Rate = r.Rate;
                        s.Speak(r.Content);
                    }

                }
          

        }
    }
}
