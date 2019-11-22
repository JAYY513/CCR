using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JControls
{
    /// <summary>
    /// NewNavigationBar.xaml 的交互逻辑
    /// </summary>
    public partial class NewNavigationBar : UserControl
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(NewNavigationBar), new PropertyMetadata(null, OnItemSourceChanged));

        // Using a DependencyProperty as the backing store for MaxPageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxPageSizeProperty =
            DependencyProperty.Register("MaxPageSize", typeof(int), typeof(NewNavigationBar), new PropertyMetadata(6, OnMaxPageSizeChanged));

        // Using a DependencyProperty as the backing store for PageIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(NewNavigationBar), new PropertyMetadata(OnPageIndexChanged));

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(NewNavigationBar), new PropertyMetadata(null));

        public List<object> tempItemsSourceList = new List<object>();

        public NewNavigationBar()
        {
            InitializeComponent();
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public int MaxPageSize
        {
            get { return (int)GetValue(MaxPageSizeProperty); }
            set { SetValue(MaxPageSizeProperty, value); }
        }

        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public void SetSelectedItem(int pageIndex = 0)
        {
            if (tempItemsSourceList.Count == 0)
            {
                SelectedItem = null;
                return; 
            }
            List<List<object>> lists = new List<List<object>>();
            for (int i = 0; i < tempItemsSourceList.Count; i++)
            {
                if (i % MaxPageSize == 0)
                {
                    lists.Add(new List<object>());
                }
                lists.Last().Add(tempItemsSourceList[i]);
            }
            if (pageIndex > lists.Count - 1) pageIndex = lists.Count - 1;
            else if (pageIndex < 0) pageIndex = 0;
            SelectedItem = lists[pageIndex];
        }

        private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newNavigationBar = (NewNavigationBar)d;
            newNavigationBar.tempItemsSourceList.Clear();
            if (e.NewValue == null)
            {
                newNavigationBar.SelectedItem = null;
                return;
            }
            var itemsSource = ((IEnumerable)e.NewValue).GetEnumerator();
            for (int i = 0; itemsSource.MoveNext(); i++)
            {
                newNavigationBar.tempItemsSourceList.Add(itemsSource.Current);
            }
            newNavigationBar.SetSelectedItem(newNavigationBar.PageIndex);
        }
        private static void OnMaxPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newNavigationBar = (NewNavigationBar)d;
            newNavigationBar.SetSelectedItem(newNavigationBar.PageIndex);
        }
        private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newNavigationBar = (NewNavigationBar)d;
            newNavigationBar.SetSelectedItem((int)e.NewValue);
        }

        private void LeftClick(object sender, RoutedEventArgs e)
        {
            if (PageIndex > 0)
                PageIndex -= 1;

        }

        private void RightClick(object sender, RoutedEventArgs e)
        {
            if (PageIndex < tempItemsSourceList.Count - 1)
                PageIndex += 1;
        }
    }
}