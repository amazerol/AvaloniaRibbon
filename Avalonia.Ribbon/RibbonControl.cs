using Avalonia.Media;
using Avalonia.Styling;
using System;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonControl : TabControl, IStyleable
    {

        public static readonly StyledProperty<IBrush> RemainingTabControlHeaderColorProperty;

        static RibbonControl()
        {
            RemainingTabControlHeaderColorProperty = AvaloniaProperty.Register<RibbonControl, IBrush>(nameof(RemainingTabControlHeaderColor));
        }

        Type IStyleable.StyleKey => typeof(RibbonControl);

        public IBrush RemainingTabControlHeaderColor
        {
            get { return GetValue(RemainingTabControlHeaderColorProperty); }
            set { SetValue(RemainingTabControlHeaderColorProperty, value); }
        }

       
    }
}
