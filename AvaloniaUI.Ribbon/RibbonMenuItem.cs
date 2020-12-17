using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using Avalonia.Interactivity;

namespace AvaloniaUI.Ribbon
{

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

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            e.NameScope.Get<Button>("PART_ContentButton").Click += (sneder, args) =>
            {
                var f = new RoutedEventArgs(ClickEvent);
                RaiseEvent(f);
            };
        }
    }
}
