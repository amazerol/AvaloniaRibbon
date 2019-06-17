using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace AvaloniaRibbon.Views
{
    public class RibbonLinearButton : Button
    {
        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<RibbonLinearButton, string>(nameof(Text));

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public static readonly StyledProperty<IBitmap> IconPathProperty =
            AvaloniaProperty.Register<RibbonLinearButton, IBitmap>(nameof(IconPath));

        public IBitmap IconPath
        {
            get { return GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

    }
}
