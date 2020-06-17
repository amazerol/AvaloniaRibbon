using Avalonia;
using Avalonia.Controls;
using Avalonia.Collections;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.Controls.Templates;

namespace AvaloniaUI.Ribbon
{
    public class RibbonMenu : ItemsControl, IRibbonMenu, IStyleable
    {
        private IEnumerable _rightColumnItems = new AvaloniaList<object>();
        RibbonMenuItem _previousSelectedItem = null;


        public static readonly StyledProperty<object> ContentProperty = ContentControl.ContentProperty.AddOwner<RibbonMenu>();

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly StyledProperty<bool> IsMenuOpenProperty = AvaloniaProperty.Register<RibbonMenu, bool>(nameof(IsMenuOpen), false);

        public bool IsMenuOpen
        {
            get => GetValue(IsMenuOpenProperty);
            set => SetValue(IsMenuOpenProperty, value);
        }

        public static readonly StyledProperty<object> SelectedSubItemsProperty = AvaloniaProperty.Register<RibbonMenu, object>(nameof(SelectedSubItems));

        public object SelectedSubItems
        {
            get => GetValue(SelectedSubItemsProperty);
            set => SetValue(SelectedSubItemsProperty, value);
        }

        public static readonly StyledProperty<bool> HasSelectedItemProperty = AvaloniaProperty.Register<RibbonMenu, bool>(nameof(HasSelectedItem), false);

        public bool HasSelectedItem
        {
            get => GetValue(HasSelectedItemProperty);
            set => SetValue(HasSelectedItemProperty, value);
        }

        public static readonly StyledProperty<string> RightColumnHeaderProperty = AvaloniaProperty.Register<RibbonMenu, string>(nameof(RightColumnHeader));

        public string RightColumnHeader
        {
            get => GetValue(RightColumnHeaderProperty);
            set => SetValue(RightColumnHeaderProperty, value);
        }


        public static readonly DirectProperty<RibbonMenu, IEnumerable> RightColumnItemsProperty = AvaloniaProperty.RegisterDirect<RibbonMenu, IEnumerable>(nameof(RightColumnItems), o => o.RightColumnItems, (o, v) => o.RightColumnItems = v);

        public IEnumerable RightColumnItems
        {
            get => _rightColumnItems;
            set => SetAndRaise(RightColumnItemsProperty, ref _rightColumnItems, value);
        }



        private static readonly FuncTemplate<IPanel> DefaultPanel = new FuncTemplate<IPanel>(() => new StackPanel());

        public static readonly StyledProperty<ITemplate<IPanel>> RightColumnItemsPanelProperty = AvaloniaProperty.Register<RibbonMenu, ITemplate<IPanel>>(nameof(RightColumnItemsPanel), DefaultPanel);

        public ITemplate<IPanel> RightColumnItemsPanel
        {
            get => GetValue(RightColumnItemsPanelProperty);
            set => SetValue(RightColumnItemsPanelProperty, value);
        }


        public static readonly StyledProperty<IDataTemplate> RightColumnItemTemplateProperty = AvaloniaProperty.Register<RibbonMenu, IDataTemplate>(nameof(RightColumnItemTemplate));

        public IDataTemplate RightColumnItemTemplate
        {
            get => GetValue(RightColumnItemTemplateProperty);
            set => SetValue(RightColumnItemTemplateProperty, value);
        }




        static RibbonMenu()
        {
            IsMenuOpenProperty.Changed.AddClassHandler<RibbonMenu>(new Action<RibbonMenu, AvaloniaPropertyChangedEventArgs>((sender, e) =>
            {
                if (!(bool)e.NewValue)
                {
                    sender.SelectedSubItems = null;
                    sender.HasSelectedItem = false;

                    if (sender._previousSelectedItem != null)
                        sender._previousSelectedItem.IsSelected = false;
                }
            }));
        }

        protected override void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.ItemsChanged(e);
            ResetItemHoverEvents();
        }

        protected override void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.ItemsCollectionChanged(sender, e);
            ResetItemHoverEvents();
        }

        void ResetItemHoverEvents()
        {
            foreach (RibbonMenuItem item in Items.OfType<RibbonMenuItem>())
            {
                item.PointerEnter -= Item_PointerEnter;
                item.PointerEnter += Item_PointerEnter;
            }
        }

        private void Item_PointerEnter(object sender, Avalonia.Input.PointerEventArgs e)
        {
            if ((sender is RibbonMenuItem item))
            {
                int counter = 0;
                Timer timer = new Timer(1);
                timer.Elapsed += (sneder, args) =>
                {
                    if (counter < 25)
                        counter++;
                    else
                    {
                        Dispatcher.UIThread.Post(() =>
                        {
                            if (item.IsPointerOver)
                            {
                                if (item.HasItems)
                                {
                                    SelectedSubItems = item.Items;
                                    HasSelectedItem = true;

                                    item.IsSelected = true;

                                    if (_previousSelectedItem != null)
                                        _previousSelectedItem.IsSelected = false;

                                    _previousSelectedItem = item;
                                }
                                else
                                {
                                    SelectedSubItems = null;
                                    HasSelectedItem = false;

                                    if (_previousSelectedItem != null)
                                        _previousSelectedItem.IsSelected = false;
                                }
                            }
                        });

                        timer.Stop();
                    }
                };
                timer.Start();
            }
        }
    }

    public class RibbonMenuItem : HeaderedItemsControl, IStyleable
    {
        private ICommand _command;


        public static readonly StyledProperty<object> IconProperty = AvaloniaProperty.Register<RibbonMenuItem, object>(nameof(Icon));

        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly StyledProperty<bool> IsSubmenuOpenProperty = AvaloniaProperty.Register<RibbonMenuItem, bool>(nameof(IsSubmenuOpen));

        public bool IsSubmenuOpen
        {
            get => GetValue(IsSubmenuOpenProperty);
            set => SetValue(IsSubmenuOpenProperty, value);
        }

        public static readonly StyledProperty<bool> HasItemsProperty = AvaloniaProperty.Register<RibbonMenuItem, bool>(nameof(HasItems));

        public bool HasItems
        {
            get => GetValue(HasItemsProperty);
            set => SetValue(HasItemsProperty, value);
        }

        public static readonly StyledProperty<bool> IsSelectedProperty = AvaloniaProperty.Register<RibbonMenuItem, bool>(nameof(IsSelected));

        public bool IsSelected
        {
            get => GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        protected override void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.ItemsChanged(e);
            HasItems = Items.OfType<object>().Count() > 0;
        }

        protected override void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.ItemsCollectionChanged(sender, e);
            HasItems = Items.OfType<object>().Count() > 0;
        }

        public static readonly DirectProperty<RibbonMenuItem, ICommand> CommandProperty = Button.CommandProperty.AddOwner<RibbonMenuItem>(button => button.Command, (button, command) => button.Command = command);


        public ICommand Command
        {
            get => _command;
            set => SetAndRaise(CommandProperty, ref _command, value);
        }


        public static readonly StyledProperty<object> CommandParameterProperty = Button.CommandParameterProperty.AddOwner<RibbonMenuItem>();

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly RoutedEvent<RoutedEventArgs> ClickEvent = RoutedEvent.Register<Button, RoutedEventArgs>(nameof(Click), RoutingStrategies.Bubble);
        
        public event EventHandler<RoutedEventArgs> Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            e.NameScope.Get<Button>("PART_ContentButton").Click += (sneder, args) =>
            {
                var f = new RoutedEventArgs(ClickEvent);
                RaiseEvent(f);
            };
        }
    }

    public class zRibbonMenu : ToggleButton, IStyleable
    {
        /*Type IStyleable.StyleKey => typeof(RibbonMenu);


        private IEnumerable _menuItems = new AvaloniaList<object>();
        private IEnumerable _menuPlacesItems = new AvaloniaList<object>();

        public IEnumerable MenuItems
        {
            get { return _menuItems; }
            set { SetAndRaise(MenuItemsProperty, ref _menuItems, value); }
        }

        public IEnumerable MenuPlacesItems
        {
            get { return _menuPlacesItems; }
            set { SetAndRaise(MenuPlacesItemsProperty, ref _menuPlacesItems, value); }
        }

        public override   IsMenuOpen
        {
            get
            {
                if (IsChecked != null)
                    return IsChecked.Value;
                else
                    return false;
            }
            set => IsChecked = value;
        }

        public static readonly DirectProperty<RibbonMenu, IEnumerable> MenuItemsProperty;
        public static readonly DirectProperty<RibbonMenu, IEnumerable> MenuPlacesItemsProperty;

        static RibbonMenu()
        {
            MenuItemsProperty = MenuBase.ItemsProperty.AddOwner<RibbonMenu>(x => x.MenuItems, (x, v) => x.MenuItems = v);
            MenuPlacesItemsProperty = ItemsControl.ItemsProperty.AddOwner<RibbonMenu>(x => x.MenuPlacesItems, (x, v) => x.MenuPlacesItems = v);
        }*/
    }
}
