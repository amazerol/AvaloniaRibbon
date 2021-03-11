using Avalonia;
using Avalonia.Controls;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Avalonia.Layout;

namespace AvaloniaUI.Ribbon
{
    public class RibbonGroupWrapPanel : WrapPanel
    {
        public static readonly StyledProperty<GroupDisplayMode> DisplayModeProperty = RibbonGroupBox.DisplayModeProperty.AddOwner<RibbonGroupWrapPanel>(); //AvaloniaProperty.Register<RibbonGroupWrapPanel, GroupDisplayMode>(nameof(DisplayMode), defaultValue: GroupDisplayMode.Large);
        public GroupDisplayMode DisplayMode
        {
            get => GetValue(DisplayModeProperty);
            set => SetValue(DisplayModeProperty, value);
        }

        static RibbonGroupWrapPanel()
        {
            AffectsArrange<RibbonGroupWrapPanel>(DisplayModeProperty);
            AffectsMeasure<RibbonGroupWrapPanel>(DisplayModeProperty);
            AffectsRender<RibbonGroupWrapPanel>(DisplayModeProperty);
            
            DisplayModeProperty.Changed.AddClassHandler<RibbonGroupWrapPanel>((sneder, args) =>
            {
                var children2 = sneder.Children.Where(x => x is IRibbonControl);
                if (((GroupDisplayMode)args.NewValue) == GroupDisplayMode.Large)
                {
                    foreach (IRibbonControl ctrl in children2)
                        ctrl.Size = ctrl.MaxSize;
                }
                else if (((GroupDisplayMode)args.NewValue) == GroupDisplayMode.Small)
                {
                    foreach (IRibbonControl ctrl in children2)
                        ctrl.Size = ctrl.MinSize;
                }
            });
        }

        public RibbonGroupWrapPanel()
        {
            if (TemplatedParent is RibbonGroupBox parentBox)
            {
                parentBox.Rearranged += (sneder, args) => ArrangeOverride(Bounds.Size);
                parentBox.Remeasured += (sneder, args) => MeasureOverride(Bounds.Size);
            }
        }
    }
}
