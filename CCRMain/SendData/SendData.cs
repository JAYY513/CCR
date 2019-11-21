using CCRMain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TTSHelper.TTSAPI;

namespace CCRMain.SendData
{
    public class SendData
    {
        public static ObservableCollection<TableModel> TableQuery()
        {
            var status = ViewModels.ViewModelLoctor.CallNumberViewModel.IsSelectedAllTables ? "0" : ViewModels.ViewModelLoctor.CallNumberViewModel.IsSelectedEmptyAllTables ? "1" : "11";
            return TableStatusApi.TableQuery<ObservableCollection<TableModel>>(status);
        }
    }
}
