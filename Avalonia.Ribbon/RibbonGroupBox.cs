using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Avalonia.Controls.Ribbon
{
    public enum GroupDisplayMode
    {
        Large,
        Small,
        Flyout
    }

    public class RibbonGroupBox : HeaderedItemsControl, IStyleable, IKeyTipHandler
    {
        public static readonly DirectProperty<RibbonGroupBox, ICommand> CommandProperty;
        public static readonly StyledProperty<object> CommandParameterProperty = AvaloniaProperty.Register<RibbonGroupBox, object>(nameof(CommandParameter));
        public static readonly StyledProperty<GroupDisplayMode> DisplayModeProperty = StyledProperty<RibbonGroupBox>.Register<RibbonGroupBox, GroupDisplayMode>(nameof(DisplayMode), defaultValue: GroupDisplayMode.Large);
        
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

        public void ActivateKeyTips()
        {
            foreach (Control child in Items)
                Debug.WriteLine("CONTROL KEYS: " + IRibbonControl.GetKeyTipKeys(child));
            
            Focus();
            KeyDown += RibbonGroupBox_KeyDown;
        }

        private void RibbonGroupBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = HandleKeyTip(e.Key);
            KeyDown -= RibbonGroupBox_KeyDown;
        }

        public bool HandleKeyTip(Key key)
        {
            bool retVal = false;
            foreach (Control child in Items)
            {
                if (IRibbonControl.HasKeyTipKey(child, key))
                {
                    if (child is IKeyTipHandler hdlr)
                    {
                        hdlr.ActivateKeyTips();
                        Debug.WriteLine("Group handled " + key.ToString() + " for IKeyTipHandler");
                    }
                    else
                    {
                        if ((child is Button btn) && (btn.Command != null))
                            btn.Command.Execute(btn.CommandParameter);
                        else
                            child.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                        ((Parent as ItemsControl).TemplatedParent as Ribbon).Close();
                    }
                    retVal = true;
                    break;
                }
            }
            return retVal;
        }

        public event EventHandler Rearranged;
        public event EventHandler Remeasured;
    }
}