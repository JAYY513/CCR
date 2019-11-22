using JLib.MVVM;
using System.Windows.Input;

namespace CCRMain.Models
{
    public class LineUpItem : BindableBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tableNumber { get; set; }
        /// <summary>
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
        public string tableSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string addTime { get; set; }

        public ICommand LineUpItemCommand { get; set; }

    }
}
