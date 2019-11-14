using Avalonia;
using Avalonia.Controls;
using System.Windows.Input;

namespace AvaloniaRibbon.Views
{
    public class RibbonTabGroup : ContentControl
    {
        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<RibbonTabGroup, string>(nameof(Text));

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DirectProperty<RibbonTabGroup, ICommand> CommandProperty =
            AvaloniaProperty.RegisterDirect<RibbonTabGroup, ICommand>(nameof(Command),
                button => button.Command, (button, command) => button.Command = command, enableDataValidation: true);
        private ICommand _command;
        public ICommand Command
        {
            get { return _command; }
            set { SetAndRaise(CommandProperty, ref _command, value); }
        }

    }
}


