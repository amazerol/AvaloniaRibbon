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

        bool CanAddToQuickAccessToolbar
        {
            get;
            set;
        }
    }

    public static class RibbonControLHelper<T> where T : AvaloniaObject
    {
        public static void AddHandlers(StyledProperty<RibbonControlSize> minSize, StyledProperty<RibbonControlSize> maxSize)
        {
            minSize.Changed.AddClassHandler<T>((sender, args) =>
            {
                if (((int)args.NewValue) > (int)((sender as IRibbonControl).Size))
                    (sender as IRibbonControl).Size = (RibbonControlSize)(args.NewValue);
            });

            maxSize.Changed.AddClassHandler<T>((sender, args) =>
            {
                if (((int)args.NewValue) < (int)((sender as IRibbonControl).Size))
                    (sender as IRibbonControl).Size = (RibbonControlSize)(args.NewValue);
            });
        }
    }

    public enum RibbonControlSize
    {
        Small,
        Medium,
        Large
    }
}
