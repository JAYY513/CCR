using System.Windows;

namespace JControls.AttachedProperty
{
    public class HintTextBoxAP
    {
        public static string GetHintText(DependencyObject obj)
        {
            return (string)obj.GetValue(HintTextProperty);
        }

        public static void SetHintText(DependencyObject obj, string value)
        {
            obj.SetValue(HintTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for HintText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.RegisterAttached("HintText", typeof(string), typeof(HintTextBoxAP), new PropertyMetadata(default(string)));
    }
}