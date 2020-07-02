using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    public enum RibbonControlSize
    {
        Small,
        Medium,
        Large
    }

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
    }
}
