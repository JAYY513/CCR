using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JLib.MVVM;
using System.Collections.ObjectModel;

namespace CCRMain.Models
{
    public class LineUpGroup : BindableBase
    {
        private ObservableCollection<LineUpItem> _lineUpItems = new ObservableCollection<LineUpItem>();
        public ObservableCollection<LineUpItem> data
        {
            get => _lineUpItems;
            set => SetProperty(ref _lineUpItems, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        //private string _tableSize;
        //public string TableSize
        //{
        //    get => _tableSize;
        //    set => SetProperty(ref _tableSize, value);
        //}

        private string _total;
        public string total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }
    }
}
