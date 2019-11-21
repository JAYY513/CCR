using System.Windows;
using System.Windows.Media;

namespace JControls.AttachedProperty
{
    public static class BackgroundAP
    {
        public static Brush GetBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(BackgroundProperty);
        }

        public static void SetBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(BackgroundProperty, value);
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(BackgroundAP), new PropertyMetadata(default(Brush)));
    }
}