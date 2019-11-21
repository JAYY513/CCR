using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JLib.MVVM;

namespace CCRMain.Models
{
    public class TableLineUp : BindableBase
    {/// <summary>
     /// 
     /// </summary>
        public string lineUpNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
    }
}
