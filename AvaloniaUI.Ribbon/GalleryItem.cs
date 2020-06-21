using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    public class GalleryItem : ListBoxItem
    {
        public static readonly StyledProperty<object> IconProperty = AvaloniaProperty.Register<RibbonMenuItem, object>(nameof(Icon));

        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
}
