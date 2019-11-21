using System.Windows;
using System.Windows.Controls;

namespace JControls
{
    /// <summary>
    /// HeaderTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class HeaderTextBox : UserControl
    {
        public HeaderTextBox()
        {
            InitializeComponent();
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(HeaderTextBox), new PropertyMetadata(default(string), OnHeaderChnaged));

        private static void OnHeaderChnaged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HeaderTextBox headerTextBox)
            {
                if (e.NewValue is string str)
                {
                    if (string.IsNullOrEmpty(str))
                        headerTextBox.lb.Visibility = Visibility.Collapsed;
                    else
                        headerTextBox.lb.Visibility = Visibility.Visible;
                    headerTextBox.lb.Content = str;
                }
            }
        }

        public double HeaderWidth
        {
            get { return (double)GetValue(HeaderWidthProperty); }
            set { SetValue(HeaderWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderWidthProperty =
            DependencyProperty.Register("HeaderWidth", typeof(double), typeof(HeaderTextBox), new PropertyMetadata(0d, OnHeaderWidthChanged));

        private static void OnHeaderWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HeaderTextBox headerTextBox)
            {
                if (headerTextBox.Header.Length > 0)
                {
                    headerTextBox.lb.Width = (double)e.NewValue;
                }
            }
        }

        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HintText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.Register("HintText", typeof(string), typeof(HeaderTextBox), new PropertyMetadata(string.Empty, OnHintTextChanged));

        private static void OnHintTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HeaderTextBox headerTextBox)
            {
                AttachedProperty.HintTextBoxAP.SetHintText(headerTextBox.tb2, (string)e.NewValue);
            }
        }

        public HorizontalAlignment HorizontalHeaderAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalHeaderAlignmentProperty); }
            set { SetValue(HorizontalHeaderAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalHeaderAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HorizontalHeaderAlignmentProperty =
            DependencyProperty.Register("HorizontalHeaderAlignment", typeof(HorizontalAlignment), typeof(HeaderTextBox), new PropertyMetadata(default(HorizontalAlignment), OnHorizontalHeaderAlignmentChanged));

        private static void OnHorizontalHeaderAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HeaderTextBox headerTextBox)
            {
                headerTextBox.lb.HorizontalContentAlignment = (HorizontalAlignment)e.NewValue;
            }
        }
    }
}