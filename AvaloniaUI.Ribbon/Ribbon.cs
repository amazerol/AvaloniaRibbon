using Avalonia.Collections;
using Avalonia;
using Avalonia.Controls;
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
using System.Windows.Input;
using System.Collections.ObjectModel;
using Avalonia.Controls.Generators;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;

namespace AvaloniaUI.Ribbon
{
    public class Ribbon : TabControl, IStyleable, IMainMenu, IKeyTipHandler
    {
        public static readonly StyledProperty<Orientation> OrientationProperty = StackLayout.OrientationProperty.AddOwner<Ribbon>();
        public Orientation Orientation
        {
            get => GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public static readonly StyledProperty<IBrush> HeaderBackgroundProperty = AvaloniaProperty.Register<Ribbon, IBrush>(nameof(HeaderBackground));
        public static readonly StyledProperty<IBrush> HeaderForegroundProperty = AvaloniaProperty.Register<Ribbon, IBrush>(nameof(HeaderForeground));
        public static readonly StyledProperty<bool> IsCollapsedProperty = AvaloniaProperty.Register<Ribbon, bool>(nameof(IsCollapsed));
        public static readonly StyledProperty<bool> IsCollapsedPopupOpenProperty = AvaloniaProperty.Register<Ribbon, bool>(nameof(IsCollapsedPopupOpen));
        public static readonly StyledProperty<IRibbonMenu> MenuProperty = AvaloniaProperty.Register<Ribbon, IRibbonMenu>(nameof(Menu));
        public static readonly StyledProperty<bool> IsMenuOpenProperty = AvaloniaProperty.Register<Ribbon, bool>(nameof(IsMenuOpen));
        public static readonly DirectProperty<Ribbon, ObservableCollection<RibbonGroupBox>> SelectedGroupsProperty = AvaloniaProperty.RegisterDirect<Ribbon, ObservableCollection<RibbonGroupBox>>(nameof(SelectedGroups), o => o.SelectedGroups, (o, v) => o.SelectedGroups = v);

        public static readonly DirectProperty<Ribbon, ObservableCollection<Control>> TabsProperty = AvaloniaProperty.RegisterDirect<Ribbon, ObservableCollection<Control>>(nameof(Tabs), o => o.Tabs, (o, v) => o.Tabs = v);
        
        public static readonly DirectProperty<Ribbon, ICommand> HelpButtonCommandProperty = AvaloniaProperty.RegisterDirect<Ribbon, ICommand>(nameof(HelpButtonCommand), o => o.HelpButtonCommand, (o, v) => o.HelpButtonCommand = v);
        ICommand _helpCommand;


        public static readonly StyledProperty<QuickAccessToolbar> QuickAccessToolbarProperty = AvaloniaProperty.Register<Ribbon, QuickAccessToolbar>(nameof(QuickAccessToolbar));
        public QuickAccessToolbar QuickAccessToolbar
        {
            get => GetValue(QuickAccessToolbarProperty);
            set => SetValue(QuickAccessToolbarProperty, value);
        }

        static Ribbon()
        {
            OrientationProperty.OverrideDefaultValue<Ribbon>(Orientation.Horizontal);

            SelectedIndexProperty.Changed.AddClassHandler<Ribbon>((x, e) => x.RefreshSelectedGroups());

            IsCollapsedProperty.Changed.AddClassHandler(new Action<Ribbon, AvaloniaPropertyChangedEventArgs>((sneder, args) => sneder.UpdatePresenterLocation((bool)args.NewValue)));
            //IsCollapsedPopupOpenProperty.Changed.AddClassHandler(new Action<Ribbon, AvaloniaPropertyChangedEventArgs>((sneder, args) => sneder.UpdatePresenterLocation(sneder.IsCollapsed)));

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
            
            //BoundsProperty.Changed.AddClassHandler<RibbonGroupsStackPanel>((sender, e) => sender.InvalidateMeasure());

            TabsProperty.Changed.AddClassHandler<Ribbon>((sender, e) => sender.RefreshTabs());
        }

        RibbonTab _prevSelectedTab = null;
        void RefreshSelectedGroups()
        {
            SelectedGroups.Clear();
            if (_prevSelectedTab != null)
            {
                _prevSelectedTab.IsSelected = false;
                _prevSelectedTab = null;
            }
            
            if ((SelectedItem != null) && (SelectedItem is RibbonTab tab))
            {
                foreach (RibbonGroupBox box in tab.Groups)
                    SelectedGroups.Add(box);
                
                var last = SelectedGroups.Last();
                SelectedGroups.Remove(last);
                SelectedGroups.Add(last);
                
                if (tab.IsContextual)
                {
                    tab.IsSelected = true;
                    _prevSelectedTab = tab;
                }
            }
        }

        void RefreshTabs()
        {
            ((AvaloniaList<object>)Items).Clear();
            foreach (Control ctrl in Tabs)
            {
                if (ctrl is RibbonContextualTabGroup ctx)
                {
                    foreach (RibbonTab tb in ctx.Items)
                        ((AvaloniaList<object>)Items).Add(tb);
                }
                else if (ctrl is RibbonTab tab)
                    ((AvaloniaList<object>)Items).Add(tab);
            }
        }

        ICanAddToQuickAccess _rightClicked = null;

        public ICommand HelpButtonCommand
        {
            get => _helpCommand;
            set => SetAndRaise(HelpButtonCommandProperty, ref _helpCommand, value);
        }

        void SetChildKeyTipsVisibility(bool open)
        {
            foreach (RibbonTab t in Items)
            {
                KeyTip.GetKeyTip(t).IsOpen = open;
            }
            if (Menu != null)
                KeyTip.GetKeyTip(Menu as Control).IsOpen = open;
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            KeyTip.SetShowChildKeyTipKeys(this, false);
        }

        protected IMenuInteractionHandler InteractionHandler { get; }

        public static readonly RoutedEvent<RoutedEventArgs> MenuClosedEvent = RoutedEvent.Register<Ribbon, RoutedEventArgs>(nameof(MenuClosed), RoutingStrategies.Bubble);
        public event EventHandler<RoutedEventArgs> MenuClosed
        {
            add { AddHandler(MenuClosedEvent, value); }
            remove { RemoveHandler(MenuClosedEvent, value); }
        }

        private ObservableCollection<RibbonGroupBox> _selectedGroups = new ObservableCollection<RibbonGroupBox>();
        public ObservableCollection<RibbonGroupBox> SelectedGroups
        {
            get => _selectedGroups;
            set => SetAndRaise(SelectedGroupsProperty, ref _selectedGroups, value);
        }

        
        private ObservableCollection<Control> _tabs = new ObservableCollection<Control>();
        public ObservableCollection<Control> Tabs
        {
            get => _tabs;
            set => SetAndRaise(TabsProperty, ref _tabs, value);
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

        public IRibbonMenu Menu
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
            int newIndex = SelectedIndex;
            
            if (ItemCount > 1)
            {
                if (((Orientation == Orientation.Horizontal) && (e.Delta.Y > 0)) || ((Orientation == Orientation.Vertical) && (e.Delta.Y < 0)))
                {
                    /*while (newIndex > 0)
                    {
                        newIndex--;
                        var newTab = Items.OfType<RibbonTab>().ElementAt(newIndex);
                        if (newTab.IsEffectivelyVisible && newTab.IsEnabled)
                        {
                            switchTabs = true;
                            break;
                        }
                    }*/
                    CycleTabs(false);
                }
                else if (((Orientation == Orientation.Horizontal) && (e.Delta.Y < 0)) || ((Orientation == Orientation.Vertical) && (e.Delta.Y > 0)))
                {
                    /*while (newIndex < (ItemCount - 1))
                    {
                        newIndex++;
                        var newTab = Items.OfType<RibbonTab>().ElementAt(newIndex);
                        if (newTab.IsEffectivelyVisible && newTab.IsEnabled)
                        {
                            switchTabs = true;
                            break;
                        }
                    }*/
                    CycleTabs(true);
                }
            }
            /*if (switchTabs)
                SelectedIndex = newIndex;*/

            base.OnPointerWheelChanged(e);
        }

        public void CycleTabs(bool forward)
        {
            bool switchTabs = false; 
            //var tabs = ((AvaloniaList<object>)Items).OfType<RibbonTab>().Where(x => x.IsEffectivelyVisible && x.IsEnabled);
            int newIndex = SelectedIndex;
            Action stepIndex;
            Func<bool> verifyIndex;
            
            if (forward)
            {
                stepIndex = () => newIndex++;
                verifyIndex = () => newIndex < (ItemCount - 1);
            }
            else
            {
                stepIndex = () => newIndex--;
                verifyIndex = () => newIndex > 0;
            }
            
            
            
            /*while (newIndex < ((AvaloniaList<object>)Items).Count)
            {
                step();
                RibbonTab newSel = (RibbonTab)(((AvaloniaList<object>)Items).ElementAt(newIndex));
                bool contextualVisible = true;
                if (newSel.IsContextual)
                    contextualVisible = (newSel.Parent as RibbonContextualTabGroup).IsVisible;
                if (newSel.IsVisible && newSel.IsEnabled && contextualVisible)
                {
                    SelectedIndex = newIndex;
                    break;
                }
            }*/
            while (verifyIndex())
            {
                stepIndex();
                var newTab = Items.OfType<RibbonTab>().ElementAt(newIndex);

                bool contextualVisible = true;
                if (newTab.IsContextual)
                    contextualVisible = (newTab.Parent as RibbonContextualTabGroup).IsVisible;
                if (newTab.IsEffectivelyVisible && newTab.IsEnabled && contextualVisible)
                {
                    switchTabs = true;
                    break;
                }
            }

            if (switchTabs)
                SelectedIndex = newIndex;
        }

        public void GoToPreviousTab()
        {
            var tabs = ((AvaloniaList<object>)Items).OfType<RibbonTab>().Where(x => x.IsEffectivelyVisible && x.IsEnabled);   
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            var inputRoot = e.Root as IInputRoot;

            if (inputRoot?.AccessKeyHandler != null)
                inputRoot.AccessKeyHandler.MainMenu = this;

            if ((inputRoot != null) && (inputRoot is WindowBase wnd))
                wnd.Deactivated += InputRoot_Deactivated;
            
            inputRoot.AddHandler<PointerPressedEventArgs>(PointerPressedEvent, InputRoot_PointerPressed, handledEventsToo: true);
            
            RefreshTabs();
            RefreshSelectedGroups();
        }

        void InputRoot_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (IsCollapsedPopupOpen && (!_groupsHost.IsPointerOver))
                IsCollapsedPopupOpen = false;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            var inputRoot = e.Root as IInputRoot;
            if ((inputRoot != null) && (inputRoot is WindowBase wnd))
                wnd.Deactivated -= InputRoot_Deactivated;
            
            inputRoot.RemoveHandler<PointerPressedEventArgs>(PointerPressedEvent, InputRoot_PointerPressed);
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
                else
                    HandleKeyTipKeyPress(e.Key);
            }
        }

        void HandleKeyTipControl(Control item)
        {
            item.RaiseEvent(new RoutedEventArgs(PointerPressedEvent));
            item.RaiseEvent(new RoutedEventArgs(PointerReleasedEvent));
        }

        public void ActivateKeyTips(Ribbon ribbon, IKeyTipHandler prev)
        {
            foreach (RibbonTab t in Items)
                KeyTip.GetKeyTipKeys(t);

            if (Menu != null)
                KeyTip.GetKeyTipKeys(Menu as Control);
        }

        public bool HandleKeyTipKeyPress(Key key)
        {
            bool retVal = false;
            if (IsOpen)
            {
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
                    if (KeyTip.HasKeyTipKey(Menu as Control, key))
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
        ContextMenu _ctxMenu;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            _popup = e.NameScope.Find<Popup>("PART_CollapsedContentPopup");
            
            _groupsHost = e.NameScope.Find<ItemsControl>("PART_SelectedGroupsHost");
            _mainPresenter = e.NameScope.Find<ContentControl>("PART_GroupsPresenterHolder");
            _flyoutPresenter = e.NameScope.Find<ContentControl>("PART_PopupGroupsPresenterHolder");
            
            _itemHeadersPresenter = e.NameScope.Find<ItemsPresenter>("PART_ItemsPresenter");

            UpdatePresenterLocation(IsCollapsed);


            bool secondClick = false;
            
            _itemHeadersPresenter.PointerReleased += (sneder, args) =>
            {   
                if (IsCollapsed)
                {
                    RibbonTab mouseOverItem = null;
                    foreach (RibbonTab tab in Items)
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
                            IsCollapsedPopupOpen = true;
                        else
                            secondClick = false;
                    }
                }
                else
                {
                    foreach (RibbonTab tab in Items)
                    {
                        if (tab.IsPointerOver && tab.IsContextual)
                        {
                            SelectedItem = tab;
                            break;
                        }
                    }
                }
            };
            /*_itemHeadersPresenter.DoubleTapped += (sneder, args) =>
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
            };*/
            
            var pinToQat = e.NameScope.Find<MenuItem>("PART_PinLastHoveredControlToQuickAccess");
            pinToQat.Click += (sneder, args) =>
            {
                if (_rightClicked != null)
                    QuickAccessToolbar?.AddItem(_rightClicked);
            };
            
            _ctxMenu = e.NameScope.Find<ContextMenu>("PART_ContentAreaContextMenu");
            
            e.NameScope.Find<MenuItem>("PART_CollapseRibbon").Click += (sneder, args) =>
            {
                if (IsCollapsed)
                    IsCollapsedPopupOpen = false;
                
                IsCollapsed = !IsCollapsed;
            };
            
            _groupsHost.PointerLeave += (sneder, args) => 
            {
                if (!_ctxMenu.IsOpen)
                    _rightClicked = null;
            };

            _groupsHost.AddHandler<PointerReleasedEventArgs>(PointerReleasedEvent, 
            (sneder, args) =>
            {
                if (args.Source != null)
                {
                    var ctrl = Avalonia.VisualTree.VisualExtensions.FindAncestorOfType<ICanAddToQuickAccess>(args.Source as IVisual);
                    
                    _rightClicked = ctrl;

                    if (QuickAccessToolbar != null)
                        pinToQat.IsEnabled = (_rightClicked != null) && _rightClicked.CanAddToQuickAccess && (!QuickAccessToolbar.ContainsItem(_rightClicked));
                    else
                        pinToQat.IsEnabled = false;
                }
            }, handledEventsToo: true);
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
        
        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new ItemContainerGenerator(this);
        }
    }
}
