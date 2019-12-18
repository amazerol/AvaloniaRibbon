using Avalonia.Styling;
using System;
using System.Windows.Input;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonTabGroup : ItemsControl, IStyleable
    {
        public static readonly StyledProperty<string> TextProperty;
        public static readonly DirectProperty<RibbonTabGroup, ICommand> CommandProperty;

        ICommand _command;

        static RibbonTabGroup()
        {
            TextProperty = AvaloniaProperty.Register<RibbonTabGroup, string>(nameof(Text));
            CommandProperty = AvaloniaProperty.RegisterDirect<RibbonTabGroup, ICommand>(nameof(Command), button => button.Command, (button, command) => button.Command = command, enableDataValidation: true);
        }

        Type IStyleable.StyleKey => typeof(RibbonTabGroup);

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public ICommand Command
        {
            get { return _command; }
            set { SetAndRaise(CommandProperty, ref _command, value); }
        }

    }
}