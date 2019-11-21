using System.Windows;
using System.Windows.Controls;

namespace JControls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:JControls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:JControls;assembly=JControls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:DialogContent/>
    ///
    /// </summary>
    public class DialogContent : ContentControl
    {
        static DialogContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogContent), new FrameworkPropertyMetadata(typeof(DialogContent)));
        }

        public delegate void CloseButtonEventHandler(object sender, RoutedEventArgs e);

        public event CloseButtonEventHandler OnCloseButtonEventHandler;

        private Button closeButton;

        public DialogContent()
        {
            Loaded += DialogContent_Loaded;
        }

        private void DialogContent_Loaded(object sender, RoutedEventArgs e)
        {
            closeButton = (Button)this.Template.FindName("PART_Close", this);
            closeButton.Click += CloseButton_Click;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            OnCloseButtonEventHandler?.Invoke(sender, e);
        }
    }
}