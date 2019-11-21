using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLib.Singleton
{
    public abstract class Singleton<T> where T : new()
    {
        private static T instance;
        private static readonly object loca = new object();

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (loca)
                    {
                        if (instance == null)
                        {
                            instance = new T();
                        }
                    }
                }
                return instance;
            }
        }
    }
}
