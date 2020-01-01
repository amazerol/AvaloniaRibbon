using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;
using System.Windows.Input;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonGroupBox : HeaderedItemsControl, IStyleable
    {
        public static readonly DirectProperty<RibbonGroupBox, ICommand> CommandProperty;
        public static readonly StyledProperty<object> CommandParameterProperty = AvaloniaProperty.Register<RibbonGroupBox, object>(nameof(CommandParameter));

        ICommand _command;

        static RibbonGroupBox()
        {
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

    }
}