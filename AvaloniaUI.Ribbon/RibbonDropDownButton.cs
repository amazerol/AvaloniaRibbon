using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    public class RibbonDropDownButton : ItemsControl, IRibbonControl, ICanAddToQuickAccess
    {
        private bool _isDropDownOpen;


        public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;
        public static readonly StyledProperty<bool> CanAddToQuickAccessProperty = RibbonButton.CanAddToQuickAccessProperty.AddOwner<RibbonDropDownButton>();
        public bool CanAddToQuickAccess
        {
            get => GetValue(CanAddToQuickAccessProperty);
            set => SetValue(CanAddToQuickAccessProperty, value);
        }


        public static readonly StyledProperty<IControlTemplate> IconProperty = RibbonButton.IconProperty.AddOwner<RibbonDropDownButton>();
        public IControlTemplate Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }


        public static readonly StyledProperty<IControlTemplate> LargeIconProperty = RibbonButton.LargeIconProperty.AddOwner<RibbonDropDownButton>();
        public IControlTemplate LargeIcon
        {
            get => GetValue(LargeIconProperty);
            set => SetValue(LargeIconProperty, value);
        }


        public static readonly StyledProperty<IControlTemplate> QuickAccessIconProperty = RibbonButton.QuickAccessIconProperty.AddOwner<RibbonToggleButton>();
        public IControlTemplate QuickAccessIcon
        {
            get => GetValue(QuickAccessIconProperty);
            set => SetValue(QuickAccessIconProperty, value);
        }


        public static readonly StyledProperty<IControlTemplate> QuickAccessTemplateProperty = RibbonButton.QuickAccessTemplateProperty.AddOwner<RibbonDropDownButton>();
        public IControlTemplate QuickAccessTemplate
        {
            get => GetValue(QuickAccessTemplateProperty);
            set => SetValue(QuickAccessTemplateProperty, value);
        }


        public static readonly StyledProperty<object> ContentProperty = ContentControl.ContentProperty.AddOwner<RibbonDropDownButton>();
        public static readonly DirectProperty<RibbonDropDownButton, bool> IsDropDownOpenProperty = ComboBox.IsDropDownOpenProperty.AddOwner<RibbonDropDownButton>(o => o.IsDropDownOpen, (o, v) => o.IsDropDownOpen = v);


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
            return new ItemContainerGenerator<RibbonDropDownItemPresenter>(this, RibbonDropDownItemPresenter.ContentProperty, RibbonDropDownItemPresenter.ContentTemplateProperty);
        }
    }

    //public class RibbonDropDownItem : GalleryItem { }
    public class RibbonDropDownButtonItemsPresenter : ItemsPresenter, IStyleable
    {
        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new ItemContainerGenerator<RibbonDropDownItemPresenter>(this, RibbonDropDownItemPresenter.ContentProperty, RibbonDropDownItemPresenter.ContentTemplateProperty);
        }

        Type IStyleable.StyleKey => typeof(ItemsPresenter);
    }
}
