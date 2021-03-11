using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using Avalonia.Controls.Templates;

namespace AvaloniaUI.Ribbon
{

    public class RibbonDropDownItemPresenter : Button, IStyleable
    {
        /*public static readonly StyledProperty<IControlTemplate> IconProperty = RibbonControlItem.IconProperty.AddOwner<RibbonControlItemPresenter>();
        public IControlTemplate Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }*/

        Type IStyleable.StyleKey => typeof(RibbonDropDownItemPresenter);
    }
}