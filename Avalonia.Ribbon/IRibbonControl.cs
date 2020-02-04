using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Controls.Ribbon
{
    public interface IRibbonControl : IAvaloniaObject
    {
        RibbonControlSize Size
        {
            get;
            set;
        }

        RibbonControlSize MinSize
        {
            get;
            set;
        }

        RibbonControlSize MaxSize
        {
            get;
            set;
        }

        /*bool CanAddToQuickAccessToolbar
        {
            get;
            set;
        }*/
    }

    public enum RibbonControlSize
    {
        Small,
        Medium,
        Large
    }
}
