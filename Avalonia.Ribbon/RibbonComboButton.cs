using Avalonia.Media.Imaging;
using Avalonia.Styling;
using System;
using System.Windows.Input;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonComboButton : ComboBox, IStyleable, IRibbonControl
    {
        public static readonly StyledProperty<object> ContentProperty;
        public static readonly StyledProperty<object> IconProperty;
        public static readonly StyledProperty<object> LargeIconProperty;
        public static readonly StyledProperty<RibbonControlSize> SizeProperty;
        public static readonly StyledProperty<RibbonControlSize> MinSizeProperty;
        public static readonly StyledProperty<RibbonControlSize> MaxSizeProperty;
        public static readonly StyledProperty<bool> CanAddToQuickAccessToolbarProperty;

        public static readonly DirectProperty<RibbonComboButton, ICommand> CommandProperty;
        public static readonly StyledProperty<object> CommandParameterProperty;

        static RibbonComboButton()
        {
            ContentProperty = RibbonButton.ContentProperty.AddOwner<RibbonComboButton>();
            IconProperty = RibbonButton.IconProperty.AddOwner<RibbonComboButton>();
            LargeIconProperty = AvaloniaProperty.Register<RibbonButton, object>(nameof(LargeIcon), null);
            SizeProperty = RibbonButton.SizeProperty.AddOwner<RibbonComboButton>();
            MinSizeProperty = RibbonButton.MinSizeProperty.AddOwner<RibbonComboButton>();
            MaxSizeProperty = RibbonButton.MaxSizeProperty.AddOwner<RibbonComboButton>();
            CanAddToQuickAccessToolbarProperty = RibbonButton.CanAddToQuickAccessToolbarProperty.AddOwner<RibbonComboButton>();
            CommandProperty = Button.CommandProperty.AddOwner<RibbonComboButton>(button => button.Command, (button, command) => button.Command = command);
            CommandParameterProperty = Button.CommandParameterProperty.AddOwner<RibbonComboButton>();
            //AffectsRender<RibbonComboButton>(SizeProperty, MinSizeProperty, MaxSizeProperty);
            AffectsMeasure<RibbonComboButton>(SizeProperty, MinSizeProperty, MaxSizeProperty);
            AffectsArrange<RibbonComboButton>(SizeProperty, MinSizeProperty, MaxSizeProperty);
            RibbonControLHelper<RibbonComboButton>.AddHandlers(MinSizeProperty, MaxSizeProperty);
        }

        Type IStyleable.StyleKey => typeof(RibbonComboButton);

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

        public bool CanAddToQuickAccessToolbar
        {
            get => GetValue(CanAddToQuickAccessToolbarProperty);
            set => SetValue(CanAddToQuickAccessToolbarProperty, value);
        }

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
