using CCRMain.Models;
using System.Diagnostics;
using System.Windows;
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
           var A = TableStatusApi.TableLineUp<TableLineUp>(number);
        }
    }
}