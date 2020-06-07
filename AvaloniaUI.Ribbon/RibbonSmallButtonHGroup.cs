using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using System;

namespace AvaloniaUI.Ribbon
{
    public class RibbonSmallButtonHGroup : ItemsControl, IStyleable
    {
        Type IStyleable.StyleKey => typeof(RibbonSmallButtonHGroup);
    }
}
