using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    public class Gallery : ListBox, IStyleable, IRibbonControl
    {
        public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;
        
        public static readonly StyledProperty<double> ItemHeightProperty = AvaloniaProperty.Register<Gallery, double>(nameof(ItemHeight));

        public double ItemHeight
        {
            get => GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }

        public static readonly DirectProperty<Gallery, bool> IsDropDownOpenProperty;

        static Gallery()
        {
            IsDropDownOpenProperty = ComboBox.IsDropDownOpenProperty.AddOwner<Gallery>(element => element.IsDropDownOpen, (element, value) => element.IsDropDownOpen = value);
            IsDropDownOpenProperty.Changed.AddClassHandler(new Action<Gallery, AvaloniaPropertyChangedEventArgs>((sneder, args) =>
            {
                sneder.UpdatePresenterLocation((bool)args.NewValue);
            }));

            RibbonControlHelper<Gallery>.SetProperties(out SizeProperty, out MinSizeProperty, out MaxSizeProperty);
        }

        Type IStyleable.StyleKey => typeof(Gallery);

        private bool _isDropDownOpen;
        public bool IsDropDownOpen
        {
            get => _isDropDownOpen;
            set => SetAndRaise(IsDropDownOpenProperty, ref _isDropDownOpen, value);
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

        ItemsPresenter _itemsPresenter;
        ContentControl _mainPresenter;
        ContentControl _flyoutPresenter;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            
            _itemsPresenter = e.NameScope.Find<ItemsPresenter>("PART_ItemsPresenter");
            _mainPresenter = e.NameScope.Find<ContentControl>("PART_ItemsPresenterHolder");
            

            GalleryScrollContentPresenter pres = e.NameScope.Find<GalleryScrollContentPresenter>("PART_ScrollContentPresenter");
            e.NameScope.Find<RepeatButton>("PART_UpButton").Click += (sneder, args) => pres.Offset = pres.Offset.WithY(Math.Max(0, pres.Offset.Y - ItemHeight));
            e.NameScope.Find<RepeatButton>("PART_DownButton").Click += (sneder, args) => pres.Offset = pres.Offset.WithY(Math.Min(pres.Offset.Y + ItemHeight, _mainPresenter.Bounds.Height - pres.Bounds.Height));

            _flyoutPresenter = e.NameScope.Find<ContentControl>("PART_FlyoutItemsPresenterHolder");
            /*_flyoutPresenter.PointerWheelChanged += (s, a) => 
            {
                a.Handled = true;
            };*/
            e.NameScope.Find<Control>("PART_FlyoutRoot").PointerLeave += (sneder, e) => IsDropDownOpen = false;

            UpdatePresenterLocation(IsDropDownOpen);
        }

        private void UpdatePresenterLocation(bool intoFlyout)
        {
            if (_itemsPresenter.Parent is IContentPresenter presenter)
                presenter.Content = null;
            else if (_itemsPresenter.Parent is ContentControl control)
                control.Content = null;
            else if (_itemsPresenter.Parent is Panel panel)
                panel.Children.Remove(_itemsPresenter);

            if (intoFlyout)
                _flyoutPresenter.Content = _itemsPresenter;
            else
                _mainPresenter.Content = _itemsPresenter;
        }
    }

    public class GalleryScrollContentPresenter : ScrollContentPresenter
    {
        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            //base.OnPointerWheelChanged(e);
        }
    }
}
