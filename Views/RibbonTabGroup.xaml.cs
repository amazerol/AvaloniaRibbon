using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaRibbon.Views
{
    public class RibbonTabGroup : ContentControl
    {
        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<RibbonButton, string>(nameof(Text));

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
