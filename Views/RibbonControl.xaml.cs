using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace AvaloniaRibbon.Views
{
    public class RibbonControl : TabControl
    {

        public static readonly StyledProperty<IBrush> RemainingTabControlHeaderColorProperty =
            AvaloniaProperty.Register<RibbonControl, IBrush>(nameof(RemainingTabControlHeaderColor));

        public IBrush RemainingTabControlHeaderColor
        {
            get { return GetValue(RemainingTabControlHeaderColorProperty); }
            set { SetValue(RemainingTabControlHeaderColorProperty, value); }
        }

       
    }
}
