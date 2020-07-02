using Avalonia.Media.Imaging;
using Avalonia.Styling;
using System;
using System.Windows.Input;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonSplitButton : ComboBox, IStyleable, IRibbonControl
    {
        public static readonly StyledProperty<object> ContentProperty;
        public static readonly StyledProperty<object> IconProperty;
        public static readonly StyledProperty<object> LargeIconProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
        public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;
        //public static readonly StyledProperty<bool> CanAddToQuickAccessToolbarProperty;

        public static readonly DirectProperty<RibbonSplitButton, ICommand> CommandProperty;
        public static readonly StyledProperty<object> CommandParameterProperty;

        static RibbonSplitButton()
        {
            ContentProperty = RibbonButton.ContentProperty.AddOwner<RibbonSplitButton>();
            IconProperty = RibbonButton.IconProperty.AddOwner<RibbonSplitButton>();
            LargeIconProperty = AvaloniaProperty.Register<RibbonButton, object>(nameof(LargeIcon), null);
            //SizeProperty = RibbonButton.SizeProperty.AddOwner<RibbonComboButton>();
            //MinSizeProperty = RibbonButton.MinSizeProperty.AddOwner<RibbonComboButton>();
            //MaxSizeProperty = RibbonButton.MaxSizeProperty.AddOwner<RibbonComboButton>();
            //CanAddToQuickAccessToolbarProperty = RibbonButton.CanAddToQuickAccessToolbarProperty.AddOwner<RibbonComboButton>();
            CommandProperty = Button.CommandProperty.AddOwner<RibbonSplitButton>(button => button.Command, (button, command) => button.Command = command);
            CommandParameterProperty = Button.CommandParameterProperty.AddOwner<RibbonSplitButton>();
            //AffectsRender<RibbonComboButton>(SizeProperty, MinSizeProperty, MaxSizeProperty);
            //AffectsMeasure<RibbonComboButton>(SizeProperty, MinSizeProperty, MaxSizeProperty);
            //AffectsArrange<RibbonComboButton>(SizeProperty, MinSizeProperty, MaxSizeProperty);
            //RibbonControLHelper<RibbonComboButton>.AddHandlers(MinSizeProperty, MaxSizeProperty);
            RibbonControlHelper<RibbonSplitButton>.SetProperties(out SizeProperty, out MinSizeProperty, out MaxSizeProperty);
            Button.FocusableProperty.OverrideDefaultValue<RibbonSplitButton>(false);
        }

        Type IStyleable.StyleKey => typeof(RibbonSplitButton);

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public object LargeIcon
        {
            get => GetValue(LargeIconProperty);
            set => SetValue(LargeIconProperty, value);
        }


        public RibbonControlSize Size
        {
            get => GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public RibbonControlSize MinSize
        {
            get => GetValue(MinSizeProperty);
            set => SetValue(MinSizeProperty, value);
        }

        public RibbonControlSize MaxSize
        {
            get => GetValue(MaxSizeProperty);
            set => SetValue(MaxSizeProperty, value);
        }

        /*public bool CanAddToQuickAccessToolbar
        {
            get => GetValue(CanAddToQuickAccessToolbarProperty);
            set => SetValue(CanAddToQuickAccessToolbarProperty, value);
        }*/

        private ICommand _command = null;
        public ICommand Command
        {
            get => _command;
            set => SetAndRaise(CommandProperty, ref _command, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
    }
}
