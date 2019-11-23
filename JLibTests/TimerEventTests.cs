using Microsoft.VisualStudio.TestTools.UnitTesting;
using JLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace JLib.Tests
{
    [TestClass()]
    public class TimerEventTests
    {
        TimerEvent TimerEvent = new TimerEvent(new UIElement());
        public TimerEventTests()
        {
            TimerEvent.OnStart += TimerEvent_OnStart;
            TimerEvent.OnStop += TimerEvent_OnStop;
        }
        private void TimerEvent_OnStop()
        {

        }

        private void TimerEvent_OnStart(int time)
        {

        }
        [TestMethod()]
        public void TimerEventTest()
        {
            Task task = new Task(()=> { TimerEvent.Run(3000); });
            task.ContinueWith(_=>Thread.Sleep(6000)).Wait();
            task.Start();
        }

        [TestMethod()]
        public void RunTest()
        {
            TimerEvent.Run(3000);
        }
    }

}