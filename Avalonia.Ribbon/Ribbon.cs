using Avalonia.Collections;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Linq;
using System.Collections;
using Avalonia.Interactivity;
using Avalonia.Controls.Platform;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls.Presenters;

namespace Avalonia.Controls.Ribbon
{
    public class Ribbon : TabControl, IStyleable, IMainMenu, IKeyTipHandler
    {
        public Orientation Orientation
        {
            get { return GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly StyledProperty<Orientation> OrientationProperty;
        public static readonly StyledProperty<IBrush> HeaderBackgroundProperty;
        public static readonly StyledProperty<IBrush> HeaderForegroundProperty;
        public static readonly StyledProperty<bool> IsCollapsedProperty;
        public static readonly StyledProperty<bool> IsCollapsedPopupOpenProperty;
        public static readonly StyledProperty<RibbonMenuBase> MenuProperty = AvaloniaProperty.Register<Ribbon, RibbonMenuBase>(nameof(Menu));
        public static readonly StyledProperty<bool> IsMenuOpenProperty;
        public static readonly DirectProperty<Ribbon, IEnumerable> SelectedGroupsProperty = AvaloniaProperty.RegisterDirect<Ribbon, IEnumerable>(nameof(SelectedGroups), o => o.SelectedGroups, (o, v) => o.SelectedGroups = v);

        static Ribbon()
        {
            OrientationProperty = StackLayout.OrientationProperty.AddOwner<Ribbon>();
            OrientationProperty.OverrideDefaultValue<Ribbon>(Orientation.Horizontal);
            HeaderBackgroundProperty = AvaloniaProperty.Register<Ribbon, IBrush>(nameof(HeaderBackground));
            HeaderForegroundProperty = AvaloniaProperty.Register<Ribbon, IBrush>(nameof(HeaderForeground));
            IsCollapsedProperty = AvaloniaProperty.Register<Ribbon, bool>(nameof(IsCollapsed));
            IsCollapsedPopupOpenProperty = AvaloniaProperty.Register<Ribbon, bool>(nameof(IsCollapsedPopupOpen));
            IsMenuOpenProperty = AvaloniaProperty.Register<Ribbon, bool>(nameof(IsMenuOpen));

            SelectedIndexProperty.Changed.AddClassHandler<Ribbon>((x, e) =>
            {
                if (((int)e.NewValue >= 0) && (x.SelectedItem != null) && (x.SelectedItem is RibbonTab tab))
                    x.SelectedGroups = tab.Groups;
                else
                    x.SelectedGroups = new AvaloniaList<object>();


            });

            /*SelectedItemProperty.Changed.AddClassHandler<Ribbon>((x, e) =>
            {
                
            });*/

            IsCollapsedProperty.Changed.AddClassHandler(new Action<Ribbon, AvaloniaPropertyChangedEventArgs>((sneder, args) =>
            {
                sneder.UpdatePresenterLocation((bool)args.NewValue);
            }));

            AccessKeyHandler.AccessKeyPressedEvent.AddClassHandler<Ribbon>((sender, e) =>
            {
                if (e.Source is Control ctrl)
                    (sender as Ribbon).HandleKeyTipControl(ctrl);
            });

            KeyTip.ShowChildKeyTipKeysProperty.Changed.AddClassHandler<Ribbon>(new Action<Ribbon, AvaloniaPropertyChangedEventArgs>((sender, args) =>
            {
                bool isOpen = (bool)args.NewValue;
                if (isOpen)
                    sender.Focus();
                sender.SetChildKeyTipsVisibility(isOpen);
            }));
        }

        void SetChildKeyTipsVisibility(bool open)
        {
            foreach (RibbonTab t in Items)
            {
                KeyTip.GetKeyTip(t).IsOpen = open;
            }
            if (Menu != null)
                KeyTip.GetKeyTip(Menu).IsOpen = open;
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            KeyTip.SetShowChildKeyTipKeys(this, false);
        }

        /*bool _popupAlreadyOpen = false;
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (IsCollapsed && _popup.IsOpen)
                _popupAlreadyOpen = true;
            base.OnPointerPressed(e);
        }*/

        /*protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            if (IsCollapsed && _itemHeadersPresenter.IsPointerOver)
            {
                //if (!_popupAlreadyOpen)
                    _popup.IsOpen = true;
                /*else
                    _popupAlreadyOpen = false;*/
                /*foreach (RibbonTab tab in Items)
                {
                    if (tab.HeaderPresenter.IsPointerOver)
                }*
            }
        }*/

        protected IMenuInteractionHandler InteractionHandler { get; }

        private IEnumerable _selectedGroups = new AvaloniaList<object>();

        public static readonly RoutedEvent<RoutedEventArgs> MenuClosedEvent = RoutedEvent.Register<Ribbon, RoutedEventArgs>(nameof(MenuClosed), RoutingStrategies.Bubble);
        public event EventHandler<RoutedEventArgs> MenuClosed
        {
            add { AddHandler(MenuClosedEvent, value); }
            remove { RemoveHandler(MenuClosedEvent, value); }
        }

        public IEnumerable SelectedGroups
        {
            get { return _selectedGroups; }
            set { SetAndRaise(SelectedGroupsProperty, ref _selectedGroups, value); }
        }

        Type IStyleable.StyleKey => typeof(Ribbon);

        public bool IsCollapsed
        {
            get => GetValue(IsCollapsedProperty);
            set => SetValue(IsCollapsedProperty, value);
        }

        public bool IsCollapsedPopupOpen
        {
            get => GetValue(IsCollapsedPopupOpenProperty);
            set => SetValue(IsCollapsedPopupOpenProperty, value);
        }

        public RibbonMenuBase Menu
        {
            get => GetValue(MenuProperty);
            set => SetValue(MenuProperty, value);
        }

        public bool IsMenuOpen
        {
            get => GetValue(IsMenuOpenProperty);
            set => SetValue(IsMenuOpenProperty, value);
        }

        public IBrush HeaderBackground
        {
            get { return GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public IBrush HeaderForeground
        {
            get { return GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        public static readonly DirectProperty<Ribbon, bool> IsOpenProperty = MenuBase.IsOpenProperty.AddOwner<Ribbon>(o => o.IsOpen); //, (o, v) => o.IsOpen = v
        
        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            protected set { SetAndRaise(IsOpenProperty, ref _isOpen, value); }
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            int oldIndex = SelectedIndex;
            int newIndex = SelectedIndex;
            if (ItemCount > 1)
            {
                if (((Orientation == Orientation.Horizontal) && (e.Delta.Y > 0)) || ((Orientation == Orientation.Vertical) && (e.Delta.Y < 0)))
                {
                    if (newIndex > 0)
                        newIndex--;
                }
                else if (((Orientation == Orientation.Horizontal) && (e.Delta.Y < 0)) || ((Orientation == Orientation.Vertical) && (e.Delta.Y > 0)))
                {
                    if (newIndex < (ItemCount - 1))
                        newIndex++;
                }
            }
            SelectedIndex = newIndex;
            if ((SelectedItem is RibbonTab tab) && (!tab.IsEnabled))
                SelectedIndex = oldIndex;
            base.OnPointerWheelChanged(e);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            var inputRoot = e.Root as IInputRoot;

            if (inputRoot?.AccessKeyHandler != null)
                inputRoot.AccessKeyHandler.MainMenu = this;

            if ((inputRoot != null) && (inputRoot is WindowBase wnd))
                wnd.Deactivated += InputRoot_Deactivated;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            var inputRoot = e.Root as IInputRoot;
            if ((inputRoot != null) && (inputRoot is WindowBase wnd))
                wnd.Deactivated -= InputRoot_Deactivated;
        }

        private void InputRoot_Deactivated(object sender, EventArgs e)
        {
            Close();
        }

        public void Close()
        {
            if (!IsOpen)
                return;

            KeyTip.SetShowChildKeyTipKeys(this, false);
            IsOpen = false;
            FocusManager.Instance.Focus(_prevFocusedElement);

            RaiseEvent(new RoutedEventArgs
            {
                RoutedEvent = MenuClosedEvent,
                Source = this,
            });
        }

        IInputElement _prevFocusedElement = null;
        public void Open()
        {
            if (IsOpen)
                return;

            IsOpen = true;
            _prevFocusedElement = FocusManager.Instance.Current;
            Focus();
            KeyTip.SetShowChildKeyTipKeys(this, true);

            RaiseEvent(new RoutedEventArgs
            {
                RoutedEvent = RibbonKeyTipsOpenedEvent,
                Source = this,
            });
        }

        public static readonly RoutedEvent<RoutedEventArgs> RibbonKeyTipsOpenedEvent = RoutedEvent.Register<MenuBase, RoutedEventArgs>("RibbonKeyTipsOpened", RoutingStrategies.Bubble);

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (IsFocused)
            {
                if ((e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.F10) || (e.Key == Key.Escape))
                    Close();
                else //if ((e.Key != Key.LeftAlt) && (e.Key != Key.RightAlt) && (e.Key != Key.F10))
                    HandleKeyTipKeyPress(e.Key);
            }
        }

        void HandleKeyTipControl(Control item)
        {
            item.RaiseEvent(new RoutedEventArgs(PointerPressedEvent));
            item.RaiseEvent(new RoutedEventArgs(PointerReleasedEvent));
            Debug.WriteLine("AccessKey handled for " + item.ToString());
        }

        public void ActivateKeyTips(Ribbon ribbon, IKeyTipHandler prev)
        {
            foreach (RibbonTab t in Items)
                Debug.WriteLine("TAB KEYS: " + KeyTip.GetKeyTipKeys(t));

            if (Menu != null)
                Debug.WriteLine("MENU KEYS: " + KeyTip.GetKeyTipKeys(Menu));
        }

        public bool HandleKeyTipKeyPress(Key key)
        {
            bool retVal = false;
            if (IsOpen)
            {
                Debug.WriteLine("Key pressed: " + key);
                bool tabKeyMatched = false;
                foreach (RibbonTab t in Items)
                {
                    if (KeyTip.HasKeyTipKey(t, key))
                    {
                        SelectedItem = t;
                        tabKeyMatched = true;
                        retVal = true;
                        if (IsCollapsed)
                            IsCollapsedPopupOpen = true;
                        t.ActivateKeyTips(this, this);
                        break;
                    }
                }
                if ((!tabKeyMatched) && (Menu != null))
                {
                    //string menuKeys = KeyTip.GetKeyTipKeys(Menu);

                    if (KeyTip.HasKeyTipKey(Menu, key))
                    {
                        IsMenuOpen = true;
                        if (Menu is IKeyTipHandler handler)
                        {
                            handler.ActivateKeyTips(this, this);
                        }
                        retVal = true;
                    }
                }
            }
            return retVal;
        }

        Popup _popup;
        ItemsControl _groupsHost;
        ContentControl _mainPresenter;
        ContentControl _flyoutPresenter;
        ItemsPresenter _itemHeadersPresenter;

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);
            _popup = e.NameScope.Find<Popup>("PART_CollapsedContentPopup");
            
            _groupsHost = e.NameScope.Find<ItemsControl>("PART_SelectedGroupsHost");
            _mainPresenter = e.NameScope.Find<ContentControl>("PART_GroupsPresenterHolder");
            _flyoutPresenter = e.NameScope.Find<ContentControl>("PART_PopupGroupsPresenterHolder");
            UpdatePresenterLocation(IsCollapsed);
            _itemHeadersPresenter = e.NameScope.Find<ItemsPresenter>("PART_ItemsPresenter");


            bool secondClick = false;
            /*_popup.Opened += (sneder, args) =>
            {
                Debug.WriteLine("Content is null: " + (((_popup.Child as Border).Child as ContentControl).Content == null).ToString());
            };*/
            _itemHeadersPresenter.PointerReleased += (sneder, args) =>
            {
                if (IsCollapsed)
                {
                    RibbonTab mouseOverItem = null;
                    foreach (RibbonTab tab in _itemHeadersPresenter.Items)
                    {
                        if (tab.IsPointerOver)
                        {
                            mouseOverItem = tab;
                            break;
                        }
                    }
                    if (mouseOverItem != null)
                    {
                        if (SelectedItem != mouseOverItem)
                            SelectedItem = mouseOverItem;
                        if (!secondClick)
                            IsCollapsedPopupOpen = true; //_popup.IsOpen = true;
                        else
                            secondClick = false;
                    }
                }
            };
            _itemHeadersPresenter.DoubleTapped += (sneder, args) =>
            {
                if (IsCollapsed)
                {
                    if (IsCollapsedPopupOpen)
                        IsCollapsedPopupOpen = false;
                    IsCollapsed = false;
                }
                else
                {
                    IsCollapsed = true;
                    secondClick = true;
                }
            };

            /*_popup.LostFocus += (sneder, args) =>
            {
                _popup.StaysOpen = _itemHeadersPresenter.IsPointerOver;
            };*/
        }

        private void UpdatePresenterLocation(bool intoFlyout)
        {
            if (_groupsHost.Parent is IContentPresenter presenter)
                presenter.Content = null;
            else if (_groupsHost.Parent is ContentControl control)
                control.Content = null;
            else if (_groupsHost.Parent is Panel panel)
                panel.Children.Remove(_groupsHost);

            if (intoFlyout)
                _flyoutPresenter.Content = _groupsHost;
            else
                _mainPresenter.Content = _groupsHost;
        }
    }
}
