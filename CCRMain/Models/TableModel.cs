using JLib.MVVM;

namespace CCRMain.Models
{
    public class TableModel : BindableBase
    {/// <summary>
     /// 
     /// </summary>
     /// 
        
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        
        public int sid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        
        public int sUid { get; set; }
        /// <summary>
        /// 桌号1
        /// </summary>
        
        public string name { get; set; }
        /// <summary>
        /// 大厅
        /// </summary>
        
        public string areaName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        
        public string suitNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        
        public string useTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        
        public string amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        
        public string appointmentTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        
        public string addTime { get; set; }
        //private string _name;
        //public string Name { get => _name; set => SetProperty(ref _name, value); }

        //private string _money;
        //public string Money { get => _money; set => SetProperty(ref _money, value); }

        //private string _time;
        //public string Time { get => _time; set => SetProperty(ref _time, value); }

        //private string _headcount;
        //public string Headcount { get => _headcount; set => SetProperty(ref _headcount, value); }
    }
}