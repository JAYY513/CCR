using CCRMain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TTSHelper;
using TTSHelper.TTSAPI;

namespace CCRMain.SendData
{
    public class SendData
    {
        public static ObservableCollection<TableModel> TableQuery()
        {
            var status = ViewModels.ViewModelLoctor.CallNumberViewModel.IsSelectedAllTables ? "0" : ViewModels.ViewModelLoctor.CallNumberViewModel.IsSelectedEmptyAllTables ? "1" : "11";
            var type = ViewModels.ViewModelLoctor.CallNumberViewModel.IsSelectedAllTablesType ? "" : ViewModels.ViewModelLoctor.CallNumberViewModel.IsSelectedBoxTablesType ? "包厢" : "大厅";
            return TableStatusApi2.TryTableQuery(out _, out ObservableCollection<TableModel> tableModels, status, type) == ResponseStatus.SUCCESS ? tableModels : null;
        }
    }
}
