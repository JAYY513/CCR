using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CCRMain
{
    public class Execute
    {
        public static void OnExecute(Action action)
        {
            App.Current.Dispatcher.Invoke(action);
        }

        //public static void OnTask(Action action)
        //{
        //    Task.Factory.StartNew(() => OnExecute(action));
        //}
    }
}
