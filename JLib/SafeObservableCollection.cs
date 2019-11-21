using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLib
{
    public class SafeObservableCollection<T>
    {
        private readonly object _Padlock = new object();
        private readonly ObservableCollection<T> _ObservableCollection;

        public SafeObservableCollection()
        {
            _ObservableCollection = new ObservableCollection<T>();
        }      
        public int Count { get { lock (_Padlock) return _ObservableCollection.Count; } }

        public T this[int index]
        {
            get
            {
                lock (_Padlock)
                    return _ObservableCollection[index];
            }
            set
            {
                lock (_Padlock)
                    _ObservableCollection[index] = value;
            }
        }

        public void Add(T value)
        {
            lock (_Padlock)
            {
                    _ObservableCollection.Add(value);
            }
        }
    }
}
