using Avalonia.Media.Imaging;
using Avalonia.Styling;
using System;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonButton : Button, IStyleable
    {

        public static readonly StyledProperty<string> TextProperty;
        public static readonly StyledProperty<IBitmap> IconPathProperty;

        static RibbonButton()
        {
            TextProperty = AvaloniaProperty.Register<RibbonButton, string>(nameof(Text));
            IconPathProperty = AvaloniaProperty.Register<RibbonButton, IBitmap>(nameof(IconPath));
        }

        Type IStyleable.StyleKey => typeof(RibbonButton);

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
