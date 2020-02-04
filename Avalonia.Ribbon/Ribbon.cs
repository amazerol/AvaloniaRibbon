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
        public static readonly StyledProperty<RibbonMenu> MenuProperty = AvaloniaProperty.Register<Ribbon, RibbonMenu>(nameof(Menu));
        public static readonly StyledProperty<bool> IsMenuOpenProperty;
        public static readonly DirectProperty<Ribbon, IEnumerable> SelectedGroupsProperty = AvaloniaProperty.RegisterDirect<Ribbon, IEnumerable>(nameof(SelectedGroups), o => o.SelectedGroups, (o, v) => o.SelectedGroups = v);

        static Ribbon()
        {
            OrientationProperty = StackLayout.OrientationProperty.AddOwner<Ribbon>();
            OrientationProperty.OverrideDefaultValue<Ribbon>(Orientation.Horizontal);
            HeaderBackgroundProperty = AvaloniaProperty.Register<Ribbon, IBrush>(nameof(HeaderBackground));
            HeaderForegroundProperty = AvaloniaProperty.Register<Ribbon, IBrush>(nameof(HeaderForeground));
            IsCollapsedProperty = AvaloniaProperty.Register<Ribbon, bool>(nameof(IsCollapsed));
            IsMenuOpenProperty = AvaloniaProperty.Register<Ribbon, bool>(nameof(IsMenuOpen));

            SelectedIndexProperty.Changed.AddClassHandler<Ribbon>((x, e) =>
            {
                if (((int)e.NewValue >= 0) && (x.SelectedItem != null) && (x.SelectedItem is RibbonTab tab))
                    x.SelectedGroups = tab.Groups;
                else
                    x.SelectedGroups = new AvaloniaList<object>();
            });

            AccessKeyHandler.AccessKeyPressedEvent.AddClassHandler<Ribbon>((sender, e) =>
            {
                if (e.Source is Control ctrl)
                    (sender as Ribbon).HandleKeyTip(ctrl);
            });

            KeyTip.ShowKeyTipKeysProperty.Changed.AddClassHandler<Ribbon>(new Action<Ribbon, AvaloniaPropertyChangedEventArgs>((sender, args) =>
            {
                if ((bool)args.NewValue)
                {
                    foreach (RibbonTab t in sender.Items)
                    {
                        KeyTip.GetKeyTip(t).IsOpen = true;
                    }
                }
                else
                {
                    foreach (RibbonTab t in sender.Items)
                    {
                        KeyTip.GetKeyTip(t).IsOpen = false;
                    }
                }
            }));
        }

        public Ribbon()
        {
            LostFocus += (sneder, args) => KeyTip.SetShowKeyTipKeys(this, false);
        }

        protected IMenuInteractionHandler InteractionHandler { get; }

        private IEnumerable _selectedGroups = new AvaloniaList<object>();

        public event EventHandler<RoutedEventArgs> MenuClosed;

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

        public RibbonMenu Menu
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
            {
                inputRoot.AccessKeyHandler.MainMenu = this;
            }
        }

        public void Close()
        {
            if (!IsOpen)
                return;

            KeyTip.SetShowKeyTipKeys(this, false);
            IsOpen = false;
            FocusManager.Instance.Focus(_prevFocusedElement);
        }

        IInputElement _prevFocusedElement = null;
        public void Open()
        {
            if (IsOpen)
                return;

            IsOpen = true;
            _prevFocusedElement = FocusManager.Instance.Current;
            Focus();
            KeyTip.SetShowKeyTipKeys(this, true);

            RaiseEvent(new RoutedEventArgs
            {
                RoutedEvent = RibbonKeyTipsOpenedEvent,
                Source = this,
            });
        }

        public static readonly RoutedEvent<RoutedEventArgs> RibbonKeyTipsOpenedEvent = RoutedEvent.Register<MenuBase, RoutedEventArgs>("RibbonKeyTipsOpened", RoutingStrategies.Bubble);

        public static readonly RoutedEvent<RoutedEventArgs> RibbonKeyTipsClosedEvent = RoutedEvent.Register<MenuBase, RoutedEventArgs>("RibbonKeyTipsClosed", RoutingStrategies.Bubble);

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (IsFocused)
            {
                if ((e.Key != Key.LeftAlt) && (e.Key != Key.RightAlt) && (e.Key != Key.F10))
                    HandleKeyTip(e.Key);
                else
                    Close();
            }
        }

        void HandleKeyTip(Control item)
        {
            item.RaiseEvent(new RoutedEventArgs(PointerPressedEvent));
            item.RaiseEvent(new RoutedEventArgs(PointerReleasedEvent));
            Debug.WriteLine("AccessKey handled for " + item.ToString());
        }

        public void ActivateKeyTips()
        {
            foreach (RibbonTab t in Items)
                Debug.WriteLine("TAB KEYS: " + KeyTip.GetKeyTipKeys(t));

            if (Menu != null)
                Debug.WriteLine("MENU KEYS: " + KeyTip.GetKeyTipKeys(Menu));
        }

        public bool HandleKeyTip(Key key)
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
                        t.ActivateKeyTips();
                        break;
                    }
                }
                if ((!tabKeyMatched) && (Menu != null))
                {
                    string menuKeys = KeyTip.GetKeyTipKeys(Menu);

                    if (KeyTip.HasKeyTipKey(Menu, key))
                    {
                        IsMenuOpen = true;
                        retVal = true;
                    }
                }
            }
            return retVal;
        }
    }
}
