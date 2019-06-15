using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;

namespace AvaloniaRibbon.Views
{
    public class RibbonControl : TabControl
    {

        public static readonly StyledProperty<IBrush> RemainingTabControlHeaderColorProperty =
            AvaloniaProperty.Register<Border, IBrush>(nameof(RemainingTabControlHeaderColor));

        public IBrush RemainingTabControlHeaderColor
        {
            get { return GetValue(RemainingTabControlHeaderColorProperty); }
            set { SetValue(RemainingTabControlHeaderColorProperty, value); }
        }

       
    }
}
