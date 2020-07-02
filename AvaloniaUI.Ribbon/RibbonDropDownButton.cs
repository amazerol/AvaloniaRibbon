using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    public class RibbonDropDownButton : ItemsControl, IRibbonControl
    {
        private bool _isDropDownOpen;


        public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;
        public static readonly StyledProperty<object> IconProperty = AvaloniaProperty.Register<RibbonDropDownButton, object>(nameof(Icon));
        public static readonly StyledProperty<object> LargeIconProperty = AvaloniaProperty.Register<RibbonDropDownButton, object>(nameof(LargeIcon));

        public static readonly StyledProperty<object> ContentProperty = ContentControl.ContentProperty.AddOwner<RibbonDropDownButton>();
        public static readonly DirectProperty<RibbonDropDownButton, bool> IsDropDownOpenProperty = ComboBox.IsDropDownOpenProperty.AddOwner<RibbonDropDownButton>(o => o.IsDropDownOpen, (o, v) => o.IsDropDownOpen = v);

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

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public bool IsDropDownOpen
        {
            get => _isDropDownOpen;
            set => SetAndRaise(IsDropDownOpenProperty, ref _isDropDownOpen, value);
        }

        static RibbonDropDownButton()
        {
            RibbonControlHelper<RibbonDropDownButton>.SetProperties(out SizeProperty, out MinSizeProperty, out MaxSizeProperty);
        }

        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new ItemContainerGenerator<RibbonDropDownItem>(this, RibbonDropDownItem.ContentProperty, RibbonDropDownItem.ContentTemplateProperty);
        }
    }

    public class RibbonDropDownItem : GalleryItem { }
}
