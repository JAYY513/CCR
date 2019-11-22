using CCRMain.Models;
using JLib.Commands;
using JLib.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        public ObservableCollection<TableModel> TableModels
        {
            get => tableModels;
            set => SetProperty(ref tableModels, value);
        }

        public void AddTableModels(ObservableCollection<TableModel> tableModels)
        {           
            lock (lockObj)
            {
                TableModels.Clear();
                if (tableModels != null)
                {
                    foreach (var item in tableModels)
                    {
                        //AddTableByTableType(item);
                        TableModels.Add(item);
                    }

                    if (IsSelectedAllTables)
                    {
                        EmptyCount = tableModels.Where(i => i.status == 1).Count().ToString();
                        UsingCount = tableModels.Where(i => i.status == 11 || i.status == 31 || i.status == 21).Count().ToString();
                    }
                    else if (IsSelectedEmptyAllTables)
                    {
                        EmptyCount = tableModels.Count().ToString();
                    }
                    else
                    {
                        UsingCount = tableModels.Count().ToString();
                    }
                }
            }
        }

        private ObservableCollection<LineUpGroup> lineUpGroups = new ObservableCollection<LineUpGroup>()
        {
            new LineUpGroup() { Name = "1-4人"/*,   TableSize="1-4"*/ },
            new LineUpGroup() { Name = "5-7人"/*,   TableSize="5-8"*/ },
            new LineUpGroup() { Name = "8-20人"/*, TableSize="9"*/},
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

        private string _emptyCount;
        public string EmptyCount
        {
            get => _emptyCount;
            set => SetProperty(ref _emptyCount, value);
        }
        private string _usingCount;
        public string UsingCount
        {
            get => _usingCount;
            set => SetProperty(ref _usingCount, value);
        }

        public void RefreshLineUpGroup(LineUpGroup lineUpGroup,int selectedIndex)
        {
            int tempIndex = TabControlSelectedIndex;
            if (lineUpGroup == null) return;
            //SelectedLineUpGroups = null;
            if (lineUpGroup.data.Count == 0) return;
            if (selectedIndex == 0)
            {
                lineUpGroup.Name = "1-4人";
                LineUpGroups[0] = lineUpGroup;
                UpdateAllCount();
            }
            else if (selectedIndex == 1)
            {
                lineUpGroup.Name = "5-7人";
                LineUpGroups[1] = lineUpGroup;
                UpdateAllCount();
            }
            else if (selectedIndex == 2)
            {
                lineUpGroup.Name = "8-20人";
                LineUpGroups[2] = lineUpGroup;
                UpdateAllCount();
            }
            else if (selectedIndex == 3)
            {
                lineUpGroup.Name = "全部";
                LineUpGroups[3] = lineUpGroup;
                LineUpGroups[0].total = lineUpGroup.data.Where(A => A.tableSize == "1-4").Count().ToString();
                LineUpGroups[0].data = new ObservableCollection<LineUpItem>(lineUpGroup.data.Where(A => A.tableSize == "1-4"));
                LineUpGroups[1].total = lineUpGroup.data.Where(A => A.tableSize == "5-7").Count().ToString();
                LineUpGroups[1].data = new ObservableCollection<LineUpItem>(lineUpGroup.data.Where(A => A.tableSize == "5-7"));
                LineUpGroups[2].total = lineUpGroup.data.Where(A => A.tableSize == "8-20").Count().ToString();
                LineUpGroups[2].data = new ObservableCollection<LineUpItem>(lineUpGroup.data.Where(A => A.tableSize == "8-20"));
            }
            SelectedLineUpGroups = LineUpGroups[tempIndex];
        }

        private void UpdateAllCount()
        {
            LineUpGroups[3].total = (Convert.ToInt32(LineUpGroups[0].total) + Convert.ToInt32(LineUpGroups[1].total) + Convert.ToInt32(LineUpGroups[2].total)).ToString();
        }

        public DelegateCommand<LineUpItem> LineUpItemCommand => new DelegateCommand<LineUpItem>(LineUpItemExecute);
        
        private void LineUpItemExecute(LineUpItem lineUpItem)
        {            
            Tools.SpeechAPI.SFSpeechProxy.CallTableNumeric(lineUpItem.lineUpNumber.ToString());
            TableStatusApi.ChangeLineUp(lineUpItem.id.ToString(), "1");
        }

        public DelegateCommand<LineUpItem> RepastItemCommand => new DelegateCommand<LineUpItem>(RepastItemExecute);

        private void RepastItemExecute(LineUpItem lineUpItem)
        {
            Tools.SpeechAPI.SFSpeechProxy.CallTableNumeric(lineUpItem.lineUpNumber.ToString());
            TableStatusApi.ChangeLineUp(lineUpItem.id.ToString(), "2");
        }
        
        public DelegateCommand<LineUpItem> DequeueItemCommand => new DelegateCommand<LineUpItem>(DequeueItemExecute);

        private void DequeueItemExecute(LineUpItem lineUpItem)
        {
            TableStatusApi.ChangeLineUp(lineUpItem.id.ToString(), "3");            
            var item = TableStatusApi.QueryLineUp<LineUpGroup>(TabControlSelectedIndex == 0 ? "1-4" : TabControlSelectedIndex == 1 ? "5-7" : TabControlSelectedIndex == 2 ? "8-20" : "");
            RefreshLineUpGroup(item, TabControlSelectedIndex);

        }

        private int _tabControlSelectedIndex = 3;
        public int TabControlSelectedIndex
        {
            get => _tabControlSelectedIndex;
            set => SetProperty(ref _tabControlSelectedIndex, value);
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
        }
    }
}