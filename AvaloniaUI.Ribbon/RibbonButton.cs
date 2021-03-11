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
        public static readonly StyledProperty<IControlTemplate> IconProperty = AvaloniaProperty.Register<RibbonButton, IControlTemplate>(nameof(Icon));
        public static readonly StyledProperty<IControlTemplate> LargeIconProperty = AvaloniaProperty.Register<RibbonButton, IControlTemplate>(nameof(LargeIcon));
        public static readonly StyledProperty<IControlTemplate> QuickAccessIconProperty = AvaloniaProperty.Register<RibbonButton, IControlTemplate>(nameof(QuickAccessIcon));

        public static readonly StyledProperty<bool> CanAddToQuickAccessProperty = AvaloniaProperty.Register<RibbonButton, bool>(nameof(CanAddToQuickAccess), true);
        public bool CanAddToQuickAccess
        {
            get => GetValue(CanAddToQuickAccessProperty);
            set => SetValue(CanAddToQuickAccessProperty, value);
        }

        static RibbonButton()
        {
            RibbonControlHelper<RibbonButton>.SetProperties(out SizeProperty, out MinSizeProperty, out MaxSizeProperty);
            Button.FocusableProperty.OverrideDefaultValue<RibbonButton>(false);
        }

        Type IStyleable.StyleKey => typeof(RibbonButton);

        public IControlTemplate Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public IControlTemplate LargeIcon
        {
            get => GetValue(LargeIconProperty);
            set => SetValue(LargeIconProperty, value);
        }
        
        public IControlTemplate QuickAccessIcon
        {
            get => GetValue(QuickAccessIconProperty);
            set => SetValue(QuickAccessIconProperty, value);
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
