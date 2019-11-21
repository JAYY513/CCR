using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JControls
{
    /// <summary>
    /// NavigationBar.xaml 的交互逻辑
    /// </summary>
    public partial class NavigationBar : UserControl
    {
        public NavigationBar()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(NavigationBar), (PropertyMetadata)new FrameworkPropertyMetadata((object)null, new PropertyChangedCallback(OnItemsSourceChanged)));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationBar navigationBar)
            {
                //Table Group
                var a = ((IEnumerable)e.NewValue).GetEnumerator();
                
                for (int i = 0; a.MoveNext(); i++)
                {
                    if (i % 3 == 0)
                    {
                        ObservableCollection<object> observableCollection = new ObservableCollection<object>();
                        navigationBar.GroupList.Add(observableCollection);
                    }
                    navigationBar.maxPage++;
                    navigationBar.GroupList.Last().Add(a.Current);
                }
                if (navigationBar.maxPage == 0) return;
                navigationBar.SelectedGroupList = navigationBar.GroupList.First();// listBox.ItemsSource = navigationBar.GroupList.First();
                navigationBar.UpdateStatus();
            }
        }

        public ObservableCollection<object> SelectedGroupList
        {
            get { return (ObservableCollection<object>)GetValue(SelectedGroupListProperty); }
            set { SetValue(SelectedGroupListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedGroupList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedGroupListProperty =
            DependencyProperty.Register("SelectedGroupList", typeof(ObservableCollection<object>), typeof(NavigationBar), new PropertyMetadata(null, OnSelectedGroupListChanged));

        private static void OnSelectedGroupListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var navigationBar = (NavigationBar)d;
            navigationBar.listBox.ItemsSource = (IEnumerable)e.NewValue;
        }

        public class PageInfo
        {
            private string myVar;

            public string Name
            {
                get { return myVar; }
                set { myVar = value; }
            }

            private ObservableCollection<object> _itemSource = new ObservableCollection<object>();

            public ObservableCollection<object> ItemSource
            {
                get { return _itemSource; }
                set { _itemSource = value; }
            }
        }

        private ObservableCollection<ObservableCollection<object>> GroupList = new ObservableCollection<ObservableCollection<object>>();

        private ObservableCollection<PageInfo> tempList = new ObservableCollection<PageInfo>();

        /// <summary>
        /// 最多4个Table
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static T ElementAt<T>(IEnumerable<T> items, int index)
        {
            using (IEnumerator<T> iter = items.GetEnumerator())
            {
                for (int i = 0; i <= index; i++, iter.MoveNext()) ;
                return iter.Current;
            }
        }

        public static readonly DependencyProperty ItemsSourceSourceProperty = DependencyProperty.Register("ItemsSourceSource", typeof(IEnumerable), typeof(NavigationBar), (PropertyMetadata)new FrameworkPropertyMetadata((object)null, new PropertyChangedCallback(OnItemsSourceSourceChanged)));
        private int maxPage;

        private static void OnItemsSourceSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationBar navigationBar && e.NewValue!=null)
            {
                var a = ((IEnumerable)e.NewValue).GetEnumerator();

                for (int i = 0; a.MoveNext(); i++)
                {
                    if (i % 4 == 0)
                    {
                        PageInfo observableCollection = new PageInfo();
                        observableCollection.Name = $"{ navigationBar.tempList.Count + 1}";
                        navigationBar.tempList.Add(observableCollection);
                    }
                    navigationBar.tempList.Last().ItemSource.Add(a.Current);
                }
                navigationBar.ItemsSource = navigationBar.tempList;
            }
        }

        public IEnumerable ItemsSourceSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceSourceProperty); }
            set { SetValue(ItemsSourceSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(NavigationBar), new PropertyMetadata(null, OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public IEnumerable ShowItemSource
        {
            get { return (IEnumerable)GetValue(ShowItemSourceProperty); }
            set { SetValue(ShowItemSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowItemSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowItemSourceProperty =
            DependencyProperty.Register("ShowItemSource", typeof(IEnumerable), typeof(NavigationBar), new PropertyMetadata(null));

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            int index = SelectedGroupList.IndexOf(SelectedItem);
            if (index == SelectedGroupList.Count - 1)
            {
                var groupIndex = GroupList.IndexOf(SelectedGroupList);
                if (groupIndex == GroupList.Count - 1) return;

                SelectedGroupList = GroupList[groupIndex + 1];
                SelectedItem = SelectedGroupList.First();
            }
            else
                SelectedItem = SelectedGroupList[index + 1];
        }

        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            int index = SelectedGroupList.IndexOf(SelectedItem);
            if (index == 0)
            {
                var groupIndex = GroupList.IndexOf(SelectedGroupList);
                if (groupIndex == 0) return;
                SelectedGroupList = GroupList[groupIndex - 1];
                SelectedItem = SelectedGroupList.Last();
            }
            else
                SelectedItem = SelectedGroupList[index - 1];
        }

        public void UpdateStatus()
        {
            LastPageButton.Content = maxPage.ToString();
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}