using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Controls.Ribbon
{
    public interface IRibbonControl
    {
        RibbonControlSize Size
        {
            get;
            set;
        }

        bool CanAddToQuickAccessToolbar
        {
            get;
            set;
        }
    }

    public enum RibbonControlSize
    {
        Small,
        Medium,
        Large
    }
}
