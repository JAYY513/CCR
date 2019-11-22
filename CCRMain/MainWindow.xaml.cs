using CCRMain.Models;
using CCRMain.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TTSHelper.TTSAPI;

namespace CCRMain
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = this.user.Text;
            var employeeAccount = this.employeeAccount.Text;
            var pwd = this.pwd.Text;
            var code = this.code.Text;
            if(LoginApi.Login(user, employeeAccount, pwd, code))
            {
                Thread.Sleep(100);
                ViewModels.ViewModelLoctor.CallNumberViewModel.AddTableModels(SendData.SendData.TableQuery());
                Thread.Sleep(100);
                ViewModels.ViewModelLoctor.CallNumberViewModel.RefreshLineUpGroup(TableStatusApi.QueryLineUp<LineUpGroup>("1-4"));
            }
         }

        private void CallNumber_Click(object sender, RoutedEventArgs e)
        {
            CallNumber callNumber = new CallNumber
            {
                DataContext = ViewModels.ViewModelLoctor.CallNumberViewModel
            };
            callNumber.Show();
        }

        private void RefreshCodeClick(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                var image = LoginApi.GetCodeUrlStream();

                this.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = image;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        codeImg.Source = bitmapImage;
                    }
                    catch (Exception ex)
                    {
                    }
                });
            });
        }
    }
}