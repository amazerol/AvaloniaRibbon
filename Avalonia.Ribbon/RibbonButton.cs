using Avalonia.Media.Imaging;
using Avalonia.Styling;
using System;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonButton : Button, IStyleable, IRibbonControl
    {

        public static readonly StyledProperty<object> IconProperty = AvaloniaProperty.Register<RibbonButton, object>(nameof(Icon));
        public static readonly StyledProperty<object> LargeIconProperty = AvaloniaProperty.Register<RibbonButton, object>(nameof(LargeIcon));
        public static readonly StyledProperty<RibbonControlSize> SizeProperty;
        public static readonly StyledProperty<bool> CanAddToQuickAccessToolbarProperty;

        static RibbonButton()
        {
            SizeProperty = AvaloniaProperty.Register<RibbonButton, RibbonControlSize>(nameof(RibbonControlSize), RibbonControlSize.Large);
            CanAddToQuickAccessToolbarProperty = AvaloniaProperty.Register<RibbonButton, bool>(nameof(CanAddToQuickAccessToolbar), true);
        }

        Type IStyleable.StyleKey => typeof(RibbonButton);

        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public object LargeIcon
        {
            get => GetValue(LargeIconProperty);
            set => SetValue(LargeIconProperty, value);
        }


        public RibbonControlSize Size
        {
            get => GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public bool CanAddToQuickAccessToolbar
        {
            get => GetValue(CanAddToQuickAccessToolbarProperty);
            set => SetValue(CanAddToQuickAccessToolbarProperty, value);
        }
    }

}
