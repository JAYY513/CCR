using CCRMain.Models;
using JLib.Commands;
using JLib.MVVM;
using System;
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
                    IsSelectedBoxTablesType = false;
                    IsSelectedHallTablesType = false;
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
                    IsSelectedAllTablesType = false;
                    IsSelectedHallTablesType = false;
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
                    IsSelectedAllTablesType = false;
                    IsSelectedBoxTablesType = false;
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
                    //AddTableByTableType(item);
                    TableModels.Add(item);
                }
            }
        }

        private ObservableCollection<LineUpGroup> lineUpGroups = new ObservableCollection<LineUpGroup>()
        {
            new LineUpGroup() { Name = "1-4人"/*,   TableSize="1-4"*/ },
            new LineUpGroup() { Name = "5-8人"/*,   TableSize="5-8"*/ },
            new LineUpGroup() { Name = "9人以上"/*, TableSize="9"*/},
            new LineUpGroup() { Name = "全部"/*,    TableSize="全部"*/}
        };
        public ObservableCollection< LineUpGroup> LineUpGroups
        {
            get => lineUpGroups;
            set => SetProperty(ref lineUpGroups, value);
        }

        private LineUpGroup selectedLineUpGroups;
        public LineUpGroup SelectedLineUpGroups
        {
            get => selectedLineUpGroups;
            set { SetProperty(ref selectedLineUpGroups, value);
            
            }
        }

        public void RefreshLineUpGroup(LineUpGroup lineUpGroup)
        {
            if (lineUpGroup == null) return;
            SelectedLineUpGroups = null;
            if (lineUpGroup.data.Count == 0) return;
            if (lineUpGroup.data[0].tableSize == "1-4")
            {
                lineUpGroup.Name = "1-4人";
                LineUpGroups[0] = lineUpGroup;
            }
            else if (lineUpGroup.data[0].tableSize == "5-8")
            {
                lineUpGroup.Name = "5-8人";
                LineUpGroups[1] = lineUpGroup;
            }
            else if (lineUpGroup.data[0].tableSize == "9")
            {
                lineUpGroup.Name = "9人";
                LineUpGroups[2] = lineUpGroup;
            }
            SelectedLineUpGroups = lineUpGroup;
        }

        public DelegateCommand<LineUpItem> LineUpItemCommand => new DelegateCommand<LineUpItem>(LineUpItemExecute);

        private void LineUpItemExecute(LineUpItem lineUpItem)
        {
            TableStatusApi.ChangeLineUp(lineUpItem.id.ToString(), lineUpItem.lineUpNumber.ToString(), "1");
        }

        public DelegateCommand<LineUpItem> DequeueItemCommand => new DelegateCommand<LineUpItem>(DequeueItemExecute);

        private void DequeueItemExecute(LineUpItem lineUpItem)
        {
            TableStatusApi.ChangeLineUp(lineUpItem.id.ToString(), lineUpItem.lineUpNumber.ToString(), "3");
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