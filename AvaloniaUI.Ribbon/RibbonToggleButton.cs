using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    public class RibbonToggleButton : ToggleButton, IStyleable, IRibbonControl, ICanAddToQuickAccess
    {
        public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;
        public static readonly StyledProperty<IControlTemplate> IconProperty = RibbonButton.IconProperty.AddOwner<RibbonToggleButton>();
        public static readonly StyledProperty<IControlTemplate> LargeIconProperty = RibbonButton.LargeIconProperty.AddOwner<RibbonToggleButton>();
        public static readonly StyledProperty<IControlTemplate> QuickAccessIconProperty = RibbonButton.QuickAccessIconProperty.AddOwner<RibbonToggleButton>();


        public static readonly StyledProperty<bool> CanAddToQuickAccessProperty = RibbonButton.CanAddToQuickAccessProperty.AddOwner<RibbonToggleButton>();
        public bool CanAddToQuickAccess
        {
            get => GetValue(CanAddToQuickAccessProperty);
            set => SetValue(CanAddToQuickAccessProperty, value);
        }

        static RibbonToggleButton()
        {
            RibbonControlHelper<RibbonToggleButton>.SetProperties(out SizeProperty, out MinSizeProperty, out MaxSizeProperty);
            ToggleButton.FocusableProperty.OverrideDefaultValue<RibbonToggleButton>(false);
        }

        Type IStyleable.StyleKey => typeof(RibbonToggleButton);

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