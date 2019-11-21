using System.Windows;
using System.Windows.Controls;

namespace JControls
{
    /// <summary>
    /// JCalendar.xaml 的交互逻辑
    /// </summary>
    public partial class JCalendar : UserControl
    {
        public JCalendar()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cld.SelectedDate.HasValue)
            {
                _ = cld.SelectedDate.Value;
            }
        }
    }
}