using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace AvaloniaUI.Ribbon
{
    public enum GroupDisplayMode
    {
        Large,
        Small/*,
        Flyout*/
    }

    public class RibbonGroupBox : HeaderedItemsControl, IStyleable
    {
        public static readonly DirectProperty<RibbonGroupBox, ICommand> CommandProperty;
        public static readonly StyledProperty<object> CommandParameterProperty = AvaloniaProperty.Register<RibbonGroupBox, object>(nameof(CommandParameter));
        public static readonly StyledProperty<GroupDisplayMode> DisplayModeProperty = StyledProperty<RibbonGroupBox>.Register<RibbonGroupBox, GroupDisplayMode>(nameof(DisplayMode), GroupDisplayMode.Small);
        
        public GroupDisplayMode DisplayMode
        {
            get => GetValue(DisplayModeProperty);
            set => SetValue(DisplayModeProperty, value);
        }

        ICommand _command;

        static RibbonGroupBox()
        {
            AffectsArrange<RibbonGroupBox>(DisplayModeProperty);
            AffectsMeasure<RibbonGroupBox>(DisplayModeProperty);
            AffectsRender<RibbonGroupBox>(DisplayModeProperty);

            CommandProperty = AvaloniaProperty.RegisterDirect<RibbonGroupBox, ICommand>(nameof(Command), button => button.Command, (button, command) => button.Command = command, enableDataValidation: true);
        }

        Type IStyleable.StyleKey => typeof(RibbonGroupBox);

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public ICommand Command
        {
            get { return _command; }
            set { SetAndRaise(CommandProperty, ref _command, value); }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Rearranged?.Invoke(this, null);
            return base.ArrangeOverride(finalSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Remeasured?.Invoke(this, null);
            return base.MeasureOverride(availableSize);
        }

        public event EventHandler Rearranged;
        public event EventHandler Remeasured;
    }
}