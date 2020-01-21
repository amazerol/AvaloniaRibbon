using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonToggleButton : ToggleButton, IStyleable, IRibbonControl
    {
        public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;
        public static readonly StyledProperty<object> IconProperty = RibbonButton.IconProperty.AddOwner<RibbonToggleButton>();
        public static readonly StyledProperty<object> LargeIconProperty = RibbonButton.LargeIconProperty.AddOwner<RibbonToggleButton>();
        //public static readonly StyledProperty<bool> CanAddToQuickAccessToolbarProperty;

        static RibbonToggleButton()
        {
            //CanAddToQuickAccessToolbarProperty = AvaloniaProperty.Register<RibbonButton, bool>(nameof(CanAddToQuickAccessToolbar), true);
            RibbonControlHelper<RibbonToggleButton>.SetProperties(out SizeProperty, out MinSizeProperty, out MaxSizeProperty);
        }

        Type IStyleable.StyleKey => typeof(RibbonToggleButton);

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

        /*public bool CanAddToQuickAccessToolbar
        {
            get => GetValue(CanAddToQuickAccessToolbarProperty);
            set => SetValue(CanAddToQuickAccessToolbarProperty, value);
        }*/
    }
}