using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Controls.Ribbon
{
    public class Gallery : ListBox, IStyleable, IRibbonControl
    {
        public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;

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
            _itemsPresenter = e.NameScope.Find<ItemsPresenter>("PART_ItemsPresenter");
            _mainPresenter = e.NameScope.Find<ContentControl>("PART_ItemsPresenterHolder");
            _flyoutPresenter = e.NameScope.Find<ContentControl>("PART_FlyoutItemsPresenterHolder");
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
}
