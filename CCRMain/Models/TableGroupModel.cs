using System.Collections.ObjectModel;

namespace CCRMain.Models
{
    public class TableGroupModel : JLib.MVVM.BindableBase
    {
        private ObservableCollection<TableModel> _tableModels = new ObservableCollection<TableModel>();

        public ObservableCollection<TableModel> TableModels
        {
            get => _tableModels;
            set => SetProperty(ref _tableModels, value);
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _headcount;
        public string Headcount { get => _headcount; set => SetProperty(ref _headcount, value); }
    }
}