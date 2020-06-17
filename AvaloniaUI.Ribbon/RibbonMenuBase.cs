using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    public interface IRibbonMenu
    {
        public bool IsMenuOpen
        {
            get;
            set;
        }
    }
}
