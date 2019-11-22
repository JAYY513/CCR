using CCRMain.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using TTSHelper.TTSAPI;

namespace CCRMain.Views
{
 
    /// <summary>
    /// CallNumber.xaml 的交互逻辑
    /// </summary>
    public partial class CallNumber : Window
    {
        public CallNumber()
        {
            InitializeComponent();
            DataContextChanged += CallNumber_DataContextChanged;
            SizeChanged += CallNumber_SizeChanged;
        }

        private void CallNumber_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (Height * Width > 737838)
            //    FontSize = 15;
            //else
            //    FontSize = 12;
            Debug.WriteLine((this.Height * Width).ToString());
        }

        private void Lb_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void CallNumber_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private void nkb_OnLineUp(string number)
        {
            TableStatusApi2.TryTableLineUp(number, out TableLineUp tableLineUp, out _);
        }

        private void TabControl_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
        
        }

        int tempSelectedIndex = -1;
        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            var selectedIndex = tabControl.SelectedIndex;
            if (selectedIndex == -1) return;
            if (tempSelectedIndex == selectedIndex) return;
            tempSelectedIndex = selectedIndex;
            ViewModels.ViewModelLoctor.CallNumberViewModel.RefreshLineUpGroup(TableStatusApi.QueryLineUp<LineUpGroup>(selectedIndex == 0 ? "1-4" : selectedIndex == 1 ? "5-8" : selectedIndex == 2 ? "8-20" : "全部"));
        }
    }
}