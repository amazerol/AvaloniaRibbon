using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using System;

namespace AvaloniaUI.Ribbon
{
    public class RibbonButton : Button, IStyleable, IRibbonControl, ICanAddToQuickAccess
    {

        public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;
        public static readonly StyledProperty<object> IconProperty = AvaloniaProperty.Register<RibbonButton, object>(nameof(Icon));
        public static readonly StyledProperty<object> LargeIconProperty = AvaloniaProperty.Register<RibbonButton, object>(nameof(LargeIcon));
        public static readonly StyledProperty<object> QuickAccessIconProperty = AvaloniaProperty.Register<RibbonButton, object>(nameof(QuickAccessIcon));

        public static readonly StyledProperty<bool> CanAddToQuickAccessProperty = AvaloniaProperty.Register<RibbonButton, bool>(nameof(QuickAccessIcon), true);

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
        
        public object QuickAccessIcon
        {
            get => GetValue(QuickAccessIconProperty);
            set => SetValue(QuickAccessIconProperty, value);
        }

        public bool CanAddToQuickAccess
        {
            get => GetValue(CanAddToQuickAccessProperty);
            set => SetValue(CanAddToQuickAccessProperty, value);
        }


        public RibbonControlSize Size
        {
            get => (RibbonControlSize)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public RibbonControlSize MinSize
        {
            get => (RibbonControlSize)GetValue(MinSizeProperty);
            set => SetValue(MinSizeProperty, value);
        }

        public RibbonControlSize MaxSize
        {
            get => (RibbonControlSize)GetValue(MaxSizeProperty);
            set => SetValue(MaxSizeProperty, value);
        }

        public static readonly StyledProperty<IControlTemplate> QuickAccessTemplateProperty = AvaloniaProperty.Register<RibbonButton, IControlTemplate>(nameof(Template));

        public IControlTemplate QuickAccessTemplate
        {
            get => GetValue(QuickAccessTemplateProperty);
            set => SetValue(QuickAccessTemplateProperty, value);
        }
    }

}
