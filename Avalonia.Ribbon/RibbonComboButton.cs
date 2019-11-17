using Avalonia.Media.Imaging;
using Avalonia.Styling;
using System;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonComboButton : ComboBox, IStyleable
    {
        public static readonly StyledProperty<string> TextProperty;
        public static readonly StyledProperty<IBitmap> IconPathProperty;

        static RibbonComboButton()
        {
            TextProperty = AvaloniaProperty.Register<RibbonComboButton, string>(nameof(Text));
            IconPathProperty = AvaloniaProperty.Register<RibbonComboButton, IBitmap>(nameof(IconPath));
        }

        Type IStyleable.StyleKey => typeof(RibbonComboButton);

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public IBitmap IconPath
        {
            get { return GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }
    }
}
