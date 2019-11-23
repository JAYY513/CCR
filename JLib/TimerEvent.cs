using System;
using System.Windows;
using System.Windows.Threading;

namespace JLib
{
    public class TimerEvent
    {
        private static StartEventHandler OnStartParent;

        private TimeSpan _interval;

        private UIElement _uIElement;

        private DispatcherTimer timer_toast = new DispatcherTimer();

        /// <summary>
        ///
        /// </summary>
        /// <param name="uIElement">Dispatcher UIElement</param>
        public TimerEvent(UIElement uIElement)
        {
            _uIElement = uIElement;
            timer_toast.Interval = TimeSpan.FromMilliseconds(3000);
            timer_toast.Tick += (sender1, e1) =>
            {
                _uIElement.Dispatcher?.Invoke(() => { OnStop?.Invoke(); });
                timer_toast.Stop();
            };

            OnStartParent += (time) =>
            {
                timer_toast.Stop();
                OnStart?.Invoke(time);
                timer_toast.Interval = TimeSpan.FromMilliseconds(time);
                timer_toast.Start();
            };
        }

        public delegate void StartEventHandler(int time);

        public delegate void StopEventHandler();

        public event StartEventHandler OnStart;

        public event StopEventHandler OnStop;

        public TimeSpan Interval
        {
            get => _interval;
            set
            {
                _interval = value;
                if (_uIElement != null)
                    _uIElement.Dispatcher?.Invoke(() => { OnStop?.Invoke(); });
                timer_toast.Stop();
                timer_toast.Interval = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="time"></param>
        public void Run(int time)
        {
            OnStartParent?.Invoke(time);
        }
    }
}