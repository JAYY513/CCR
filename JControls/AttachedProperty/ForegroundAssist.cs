using System.Windows;
using System.Windows.Media;

namespace JControls.AttachedProperty
{
    public static class ForegroundAssist
    {
        public static Brush GetForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ForegroundProperty);
        }

        public static void SetForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(ForegroundProperty, value);
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached("Foreground", typeof(Brush), typeof(ForegroundAssist), new PropertyMetadata(default(Brush)));
    }
}