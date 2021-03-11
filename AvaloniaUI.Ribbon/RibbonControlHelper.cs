using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Layout;

namespace AvaloniaUI.Ribbon
{
    public static class RibbonControlHelper<T> where T : AvaloniaObject, ILayoutable
    {
        static readonly AvaloniaProperty<RibbonControlSize> SizeProperty = AvaloniaProperty.Register<IRibbonControl, RibbonControlSize>("Size", RibbonControlSize.Large, coerce: CoerceSize);
        static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty = AvaloniaProperty.Register<IRibbonControl, RibbonControlSize>("MinSize", RibbonControlSize.Small);
        static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty = AvaloniaProperty.Register<IRibbonControl, RibbonControlSize>("MaxSize", RibbonControlSize.Large);

        private static RibbonControlSize CoerceSize(IAvaloniaObject obj, RibbonControlSize val)
        {
            if (obj is IRibbonControl ctrl)
            {
                if ((int)(ctrl.MinSize) > (int)val)
                    return ctrl.MinSize;
                else if ((int)(ctrl.MaxSize) < (int)val)
                    return ctrl.MaxSize;
                else
                    return val;
            }
            else
                throw new Exception("obj must be an IRibbonControl!");
        }


        public static void SetProperties(out AvaloniaProperty<RibbonControlSize> size, out AvaloniaProperty<RibbonControlSize> minSize, out AvaloniaProperty<RibbonControlSize> maxSize)
        {
            size = SizeProperty;
            minSize = MinSizeProperty;
            maxSize = MaxSizeProperty;
            
            

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
}
