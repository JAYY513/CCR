using CCRMain.Models;
using JLib.MVVM;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TTSHelper.TTSAPI;

namespace CCRMain.ViewModels
{
    public class CallNumberViewModel : BindableBase
    {
        private bool _isSelectedAllTables = true;
        private bool _isSelectedAllTablesType = true;
        private bool _isSelectedBoxTablesType;
        private bool _isSelectedEmptyTables;
        private bool _isSelectedHallTablesType;
        private bool _isSelectedUsingTables;
        private bool _isSelectedUsingTables2;
        private ObservableCollection<TableModel> _selectedtableModels = new ObservableCollection<TableModel>();
        private ObservableCollection<TableGroupModel> _tableGroupModel = new ObservableCollection<TableGroupModel>();
        private ObservableCollection<TableModel> tableModels = new ObservableCollection<TableModel>();

        public CallNumberViewModel()
        {
            InitData();
        }

        public bool IsSelectedAllTables
        {
            get => _isSelectedAllTables;
            set
            {
                if (SetProperty(ref _isSelectedAllTables, value) && value)
                {
                    var t1 = new Task<ObservableCollection<TableModel>>(() => CCRMain.SendData.SendData.TableQuery());
                    t1.ContinueWith(preT => Execute.OnExecute(() => AddTableModels(t1.Result)));
                    t1.Start(); //IsSelectedEmptyAllTables = false;
                    //IsSelectedUsingTables = false;
                    //SelectedtableModels = GetTableModels();
                }
            }
        }

        public bool IsSelectedAllTablesType
        {
            get => _isSelectedAllTablesType;
            set
            {
                if (SetProperty(ref _isSelectedAllTablesType, value) && value)
                {
                    var t1 = new Task<ObservableCollection<TableModel>>(() => CCRMain.SendData.SendData.TableQuery());
                    t1.ContinueWith(preT => Execute.OnExecute(() => AddTableModels(t1.Result)));
                    t1.Start();
                }
            }
        }

        public bool IsSelectedBoxTablesType
        {
            get => _isSelectedBoxTablesType;
            set
            {
                if (SetProperty(ref _isSelectedBoxTablesType, value) && value)
                {
                    var t1 = new Task<ObservableCollection<TableModel>>(() => CCRMain.SendData.SendData.TableQuery());
                    t1.ContinueWith(preT => Execute.OnExecute(() => AddTableModels(t1.Result)));
                    t1.Start();
                }
            }
        }

        public bool IsSelectedEmptyAllTables
        {
            get => _isSelectedEmptyTables;
            set
            {
                if (SetProperty(ref _isSelectedEmptyTables, value) && value)
                {
                    IsSelectedAllTables = false;
                    IsSelectedUsingTables = false;
                    var t1 = new Task<ObservableCollection<TableModel>>(() => CCRMain.SendData.SendData.TableQuery());
                    t1.ContinueWith(preT => Execute.OnExecute(() => AddTableModels(t1.Result)));
                    t1.Start();
                    //SelectedtableModels = GetTableModels();
                }
            }
        }
        public bool IsSelectedHallTablesType
        {
            get => _isSelectedHallTablesType;
            set
            {
                if (SetProperty(ref _isSelectedHallTablesType, value) && value)
                {
                    var t1 = new Task<ObservableCollection<TableModel>>(() => CCRMain.SendData.SendData.TableQuery());
                    t1.ContinueWith(preT => Execute.OnExecute(()=> AddTableModels(t1.Result)));
                    t1.Start();
                    //Task<int> t3 = t1.ContinueWith(preT => DoOnThird(t1));
                    //ObservableCollection<TableModel> tableModels = default;
                    //Task.Factory.StartNew(() => { tableModels = CCRMain.SendData.SendData.TableQuery(); }).ContinueWith(_ => AddTableModels(tableModels));
                }
            }
        }

        public bool IsSelectedUsingTables
        {
            get => _isSelectedUsingTables;
            set
            {
                if (SetProperty(ref _isSelectedUsingTables, value) && value)
                {
                    IsSelectedEmptyAllTables = false;
                    IsSelectedAllTables = false;
                    var t1 = new Task<ObservableCollection<TableModel>>(() => CCRMain.SendData.SendData.TableQuery());
                    t1.ContinueWith(preT => Execute.OnExecute(() => AddTableModels(t1.Result)));
                    t1.Start();
                    //IsSelectedEmptyAllTables = false;
                    //IsSelectedAllTables = false;
                    //SelectedtableModels = GetTableModels();
                }
            }
        }

        public ObservableCollection<TableModel> SelectedtableModels
        {
            get => _selectedtableModels;
            set => SetProperty(ref _selectedtableModels, value);
        }

        public ObservableCollection<TableGroupModel> TableGroupModels
        {
            get => _tableGroupModel;
            set => SetProperty(ref _tableGroupModel, value);
        }

        public ObservableCollection<TableModel> TableModels
        {
            get => tableModels;
            set => SetProperty(ref tableModels, value);
        }

        public void AddTableModels(ObservableCollection<TableModel> tableModels)
        {
           
            lock (lockObj)
            {
                if (tableModels == null) return;
                TableGroupModels.Clear();
                TableModels.Clear();
                foreach (var item in tableModels)
                {
                    AddTableByTableType(item);
                    // TableModels.Add(item);
                }
                TableGroupModels.Add(new TableGroupModel() { Name = "1-4人", Headcount = "20", TableModels = TableModels });
                TableGroupModels.Add(new TableGroupModel() { Name = "5-8人", Headcount = "20", TableModels = TableModels });
                TableGroupModels.Add(new TableGroupModel() { Name = "1-4人", Headcount = "20", TableModels = TableModels });
                TableGroupModels.Add(new TableGroupModel() { Name = "1-4人", Headcount = "20", TableModels = TableModels });
            }
        }

        private object lockObj = new object(); 
        public ObservableCollection<TableModel> GetTableModels()
        {
            lock (lockObj)
            {
                SelectedtableModels.Clear();
                foreach (var item in tableModels)
                {
                    if ((item.status == 11 || item.status == 31) && IsSelectedUsingTables)
                    {
                        AddTableByTableType(item);
                    }
                    if ((item.status == 1 || item.status == 21) && IsSelectedEmptyAllTables)
                    {
                        AddTableByTableType(item);
                    }
                    else if (IsSelectedAllTables)
                        AddTableByTableType(item);
                }
            }
            return SelectedtableModels;
        }

        private void AddTableByTableType(TableModel tableModel)
        {
            if (IsSelectedAllTablesType)
            {
                TableModels.Add(tableModel);
            }
            else if (IsSelectedBoxTablesType)
            {
                if (tableModel.areaName.Substring(0, 2) == "包厢")
                    TableModels.Add(tableModel);
            }
            else if (IsSelectedHallTablesType)
            {
                if (tableModel.areaName.Substring(0, 2) == "大厅")
                    TableModels.Add(tableModel);
            }
        }
        private void InitData()
        {
            for (int i = 0; i < 50; i++)
            {
                TableModels.Add(new TableModel() { addTime = "18:30", amount = "3人", areaName = "￥3000", name = ("T" + i.ToString("000")) });
            }

            TableGroupModels.Add(new TableGroupModel() { Name = "1-4人", Headcount = "20", TableModels = TableModels });
            TableGroupModels.Add(new TableGroupModel() { Name = "1-4人", Headcount = "20", TableModels = TableModels });
            TableGroupModels.Add(new TableGroupModel() { Name = "1-4人", Headcount = "20", TableModels = TableModels });
            TableGroupModels.Add(new TableGroupModel() { Name = "1-4人", Headcount = "20", TableModels = TableModels });
        }
    }
}