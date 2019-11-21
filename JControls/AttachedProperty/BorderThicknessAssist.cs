using System.Windows;

namespace JControls.AttachedProperty
{
    public static class BorderThicknessAssist
    {
        public static Thickness GetBorderThickness(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(BorderThicknessProperty);
        }

        public static void SetBorderThickness(DependencyObject obj, Thickness value)
        {
            obj.SetValue(BorderThicknessProperty, value);
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.RegisterAttached("BorderThickness", typeof(Thickness), typeof(BorderThicknessAssist), new PropertyMetadata(default(Thickness)));
    }
}