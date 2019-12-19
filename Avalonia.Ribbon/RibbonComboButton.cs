using Avalonia.Media.Imaging;
using Avalonia.Styling;
using System;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonComboButton : ComboBox, IStyleable, IRibbonControl
    {
        public static readonly StyledProperty<object> ContentProperty;
        public static readonly StyledProperty<object> IconProperty;
        public static readonly StyledProperty<object> LargeIconProperty;
        public static readonly StyledProperty<RibbonControlSize> SizeProperty;
        public static readonly StyledProperty<bool> CanAddToQuickAccessToolbarProperty;

        static RibbonComboButton()
        {
            ContentProperty = RibbonButton.ContentProperty.AddOwner<RibbonComboButton>();
            IconProperty = RibbonButton.IconProperty.AddOwner<RibbonComboButton>();
            LargeIconProperty = AvaloniaProperty.Register<RibbonButton, object>(nameof(LargeIcon), null);
            SizeProperty = RibbonButton.SizeProperty.AddOwner<RibbonComboButton>();
            CanAddToQuickAccessToolbarProperty = RibbonButton.CanAddToQuickAccessToolbarProperty.AddOwner<RibbonComboButton>();
        }

        Type IStyleable.StyleKey => typeof(RibbonComboButton);

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

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
