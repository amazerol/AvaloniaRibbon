using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace AvaloniaRibbon.Views
{
    public class RibbonComboButton : ComboBox
    {
        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<RibbonComboButton, string>(nameof(Text));

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly StyledProperty<IBitmap> IconPathProperty =
            AvaloniaProperty.Register<RibbonComboButton, IBitmap>(nameof(IconPath));

        public IBitmap IconPath
        {
            get { return GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }
    }
}
