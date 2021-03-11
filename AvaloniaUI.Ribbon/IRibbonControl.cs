using Avalonia;
using Avalonia.Controls;
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

    public static class RibbonControlExtensions
    {
        public static Ribbon GetParentRibbon(IControl control)
        {
            return Avalonia.VisualTree.VisualExtensions.FindAncestorOfType<Ribbon>(control, true);
            /*IControl parentRbn = control.Parent;
            while ((!(parentRbn is Ribbon)) && (parentRbn != null))
            {
                parentRbn = parentRbn.Parent;
                /*if (parentRbn == null)
                    break;*
            }
            
            if (parentRbn is Ribbon ribbon)
                return ribbon;
            else
                return null;*/
        }
    }
}
