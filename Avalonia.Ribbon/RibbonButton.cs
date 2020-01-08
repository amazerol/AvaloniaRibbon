using Avalonia.Media.Imaging;
using Avalonia.Styling;
using System;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonButton : Button, IStyleable, IRibbonControl
    {

        public static readonly StyledProperty<object> IconProperty = AvaloniaProperty.Register<RibbonButton, object>(nameof(Icon));
        public static readonly StyledProperty<object> LargeIconProperty = AvaloniaProperty.Register<RibbonButton, object>(nameof(LargeIcon));
        public static readonly StyledProperty<RibbonControlSize> SizeProperty;
        public static readonly StyledProperty<RibbonControlSize> MinSizeProperty;
        public static readonly StyledProperty<RibbonControlSize> MaxSizeProperty;
        public static readonly StyledProperty<bool> CanAddToQuickAccessToolbarProperty;

        static RibbonButton()
        {
            //Func<IAvaloniaObject, RibbonControlSize, RibbonControlSize> validat = ValidateRibbonControlSize;
            SizeProperty = AvaloniaProperty.Register<IAvaloniaObject, RibbonControlSize>(nameof(Size), RibbonControlSize.Large, validate: ValidateSize);
            /*((/*a, b, c*) => 
            {
                //ValidateRibbonControlSize
            }
            */
            MinSizeProperty = AvaloniaProperty.Register<RibbonButton, RibbonControlSize>(nameof(MinSize), RibbonControlSize.Small);
            MaxSizeProperty = AvaloniaProperty.Register<RibbonButton, RibbonControlSize >(nameof(MaxSize), RibbonControlSize.Large);
            CanAddToQuickAccessToolbarProperty = AvaloniaProperty.Register<RibbonButton, bool>(nameof(CanAddToQuickAccessToolbar), true);
            //AffectsRender<RibbonButton>(SizeProperty, MinSizeProperty, MaxSizeProperty);
            AffectsMeasure<RibbonButton>(SizeProperty, MinSizeProperty, MaxSizeProperty);
            AffectsArrange<RibbonButton>(SizeProperty, MinSizeProperty, MaxSizeProperty);
            RibbonControLHelper<RibbonButton>.AddHandlers(MinSizeProperty, MaxSizeProperty);
        }

        /*private static RibbonControlSize ValidateRibbonControlSize(IAvaloniaObject obj, RibbonControlSize val)
        {
            if (obj is IRibbonControl ctrl)
            {
                if (((int)val) == -1)
                    return ctrl.MaxSize;
                else
                    return val;
            }
            else
                throw new Exception("obj must be an IRibbonControl!");
        }*/
        private static RibbonControlSize ValidateSize(IAvaloniaObject obj, RibbonControlSize val)
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

        Type IStyleable.StyleKey => typeof(RibbonButton);

        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public object LargeIcon
        {
            get => GetValue(LargeIconProperty);
            set => SetValue(LargeIconProperty, value);
        }


        public RibbonControlSize Size
        {
            get => GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public RibbonControlSize MinSize
        {
            get => GetValue(MinSizeProperty);
            set => SetValue(MinSizeProperty, value);
        }

        public RibbonControlSize MaxSize
        {
            get => GetValue(MaxSizeProperty);
            set => SetValue(MaxSizeProperty, value);
        }

        public bool CanAddToQuickAccessToolbar
        {
            get => GetValue(CanAddToQuickAccessToolbarProperty);
            set => SetValue(CanAddToQuickAccessToolbarProperty, value);
        }
    }

}
