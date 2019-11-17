using Avalonia.Styling;
using System;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonTab : TabItem, IStyleable
    {
        Type IStyleable.StyleKey => typeof(RibbonTab);
    }
}
