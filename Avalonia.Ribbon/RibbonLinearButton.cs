using Avalonia.Media.Imaging;
using Avalonia.Styling;
using System;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonLinearButton : Button, IStyleable
    {
        public static readonly StyledProperty<string> TextProperty;
        public static readonly StyledProperty<IBitmap> IconPathProperty;

        static RibbonLinearButton()
        {
            TextProperty = AvaloniaProperty.Register<RibbonLinearButton, string>(nameof(Text));
            IconPathProperty = AvaloniaProperty.Register<RibbonLinearButton, IBitmap>(nameof(IconPath));
        }

        Type IStyleable.StyleKey => typeof(RibbonLinearButton);

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
