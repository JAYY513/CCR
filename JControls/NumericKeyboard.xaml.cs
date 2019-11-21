using System.Windows.Controls;

namespace JControls
{
    /// <summary>
    /// NumericKeyboard.xaml 的交互逻辑
    /// </summary>
    public partial class NumericKeyboard : UserControl
    {
        public NumericKeyboard()
        {
            InitializeComponent();
        }

        public delegate void LineUp(string number);

        public event LineUp OnLineUp;

        public string InputString => InputTextBox.Text;

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var btnString = btn.Tag.ToString();
            if (btnString == "删除" && InputTextBox.Text.Length != 0)
                InputTextBox.Text = InputTextBox.Text.Substring(0, InputTextBox.Text.Length - 1);
            else if (btnString == "清除")
                InputTextBox.Text = "";
            else
            {
                InputTextBox.Text = InputTextBox.Text + btnString;
            }
        }

        private void LineUp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnLineUp?.Invoke(InputString);
        }
    }
}