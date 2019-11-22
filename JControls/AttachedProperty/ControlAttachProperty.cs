using System.Windows;
using System.Windows.Controls;

namespace JControls.AttachedProperty
{
    public class ControlAttachProperty
    {
        #region 额外头像模板

        public static ControlTemplate GetIconTemplate(DependencyObject obj)
        {
            return (ControlTemplate)obj.GetValue(IconTemplateProperty);
        }

        public static void SetIconTemplate(DependencyObject obj, ControlTemplate value)
        {
            obj.SetValue(IconTemplateProperty, value);
        }

        // Using a DependencyProperty as the backing store for IconTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.RegisterAttached("IconTemplate", typeof(ControlTemplate), typeof(ControlAttachProperty), new PropertyMetadata(null));

        #endregion 额外头像模板

        #region CornerRadius

        public static Thickness GetCornerRadius(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, Thickness value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(Thickness), typeof(ControlAttachProperty), new PropertyMetadata(default(Thickness)));

        #endregion CornerRadius

        #region string



        public static string GetString(DependencyObject obj)
        {
            return (string)obj.GetValue(StringProperty);
        }

        public static void SetString(DependencyObject obj, string value)
        {
            obj.SetValue(StringProperty, value);
        }

        // Using a DependencyProperty as the backing store for String.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StringProperty =
            DependencyProperty.RegisterAttached("String", typeof(string), typeof(ControlAttachProperty), new PropertyMetadata(""));


        #endregion string
    }
}