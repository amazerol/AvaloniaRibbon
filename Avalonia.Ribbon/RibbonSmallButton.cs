using Avalonia.Styling;
using System;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonSmallButton : RibbonButton, IStyleable
    {
        Type IStyleable.StyleKey => typeof(RibbonSmallButton);
    }
}
