using Avalonia;
using Avalonia.Controls;
using Avalonia.Collections;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Timers;
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
}
