using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    public abstract class RibbonMenuBase : ToggleButton
    {
        public abstract bool IsMenuOpen
        {
            get;
            set;
        }
    }
}
