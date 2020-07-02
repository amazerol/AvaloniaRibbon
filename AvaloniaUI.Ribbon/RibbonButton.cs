using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using System;

namespace AvaloniaUI.Ribbon
{
    public class RibbonButton : Button, IStyleable, IRibbonControl
    {

        public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;
        public static readonly StyledProperty<object> IconProperty = AvaloniaProperty.Register<RibbonButton, object>(nameof(Icon));
        public static readonly StyledProperty<object> LargeIconProperty = AvaloniaProperty.Register<RibbonButton, object>(nameof(LargeIcon));

        static RibbonButton()
        {
            RibbonControlHelper<RibbonButton>.SetProperties(out SizeProperty, out MinSizeProperty, out MaxSizeProperty);
            Button.FocusableProperty.OverrideDefaultValue<RibbonButton>(false);
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

        public RibbonControlSize MinSize
        {
            get => GetValue(MinSizeProperty);
            set => SetValue(MinSizeProperty, value);
        }

        public RibbonControlSize MaxSize
        {
            get => GetValue(MaxSizeProperty);
            set => SetValue(MaxSizeProperty, value);
        }
    }

}
