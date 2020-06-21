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

        ItemsPresenter _itemsPresenter;
        ContentControl _mainPresenter;
        ContentControl _flyoutPresenter;

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);
            /*ScrollViewer vwr = e.NameScope.Find<ScrollViewer>("PART_ScrollViewer");
            vwr.PointerWheelChanged += (sneder, args) =>
            {
                if (args.Delta.X > 0)
                    vwr.Offset = vwr.Offset.WithX(vwr.Offset.X + 20);
                else if(args.Delta.X < 0)
                    vwr.Offset = vwr.Offset.WithX(vwr.Offset.X - 20);

                if (args.Delta.Y > 0)
                    vwr.Offset = vwr.Offset.WithY(vwr.Offset.Y + 20);
                else if (args.Delta.Y < 0)
                    vwr.Offset = vwr.Offset.WithY(vwr.Offset.Y - 20);

                args.Handled = false;
            };*/

            _itemsPresenter = e.NameScope.Find<ItemsPresenter>("PART_ItemsPresenter");
            _mainPresenter = e.NameScope.Find<ContentControl>("PART_ItemsPresenterHolder");
            /*this.PointerWheelChanged += (sneder, args) =>
            {
                e.Handled = true;
                if ((Parent != null) && (Parent is InputElement el))
                    el.RaiseEvent(args);
            };*/
            GalleryScrollContentPresenter pres = e.NameScope.Find<GalleryScrollContentPresenter>("PART_ScrollContentPresenter");
            /*vwr.PointerWheelChanged += (sneder, args) =>
            {
                e.Handled = true;
                this.RaiseEvent(args);
            };*/
            //e.NameScope.Find<ScrollViewer>("PART_FlyoutScrollViewer").PointerWheelChanged += (s, a) => a.Handled = true;
            //GalleryScrollContentPresenter.property


            e.NameScope.Find<RepeatButton>("PART_UpButton").Click += (sneder, args) => pres.Offset = pres.Offset.WithY(Math.Max(0, pres.Offset.Y - ItemHeight));
            e.NameScope.Find<RepeatButton>("PART_DownButton").Click += (sneder, args) => pres.Offset = pres.Offset.WithY(Math.Min(pres.Offset.Y + ItemHeight, _mainPresenter.Bounds.Height - pres.Bounds.Height));

            _flyoutPresenter = e.NameScope.Find<ContentControl>("PART_FlyoutItemsPresenterHolder");
            _flyoutPresenter.PointerWheelChanged += (s, a) => a.Handled = true;

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
