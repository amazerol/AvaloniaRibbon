using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    public class RibbonGroupsStackPanel : StackPanel //Panel
    {
        static RibbonGroupsStackPanel()
        {
            ParentProperty.Changed.AddClassHandler<RibbonGroupsStackPanel>((sender, e) =>
            {
                Dispatcher.UIThread.Post(() => sender.SizeControls());
            });

            BoundsProperty.Changed.AddClassHandler<RibbonGroupsStackPanel>((sender, e) =>
            {
                if ((e.NewValue != null) && (e.NewValue is Rect newRect))
                    sender.SizeControls(newRect.Size);
            });
        }

        /*public RibbonGroupsStackPanel()
        {
            LayoutUpdated += (sneder, args) => UpdateLayoutState();
        }*/

        /*protected override void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.ChildrenChanged(sender, e);
            SizeControls();
        }*/

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnAttachedToLogicalTree(e);
            /*Measure(Bounds.Size);
            Arrange(Bounds);*/
            
            SizeControls();
        }

        protected override void LogicalChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.LogicalChildrenCollectionChanged(sender, e);
            
            /*foreach (RibbonGroupBox box in Children.OfType<RibbonGroupBox>())
                box.DisplayMode = GroupDisplayMode.Small;*/
            
            Size size = new Size(double.PositiveInfinity, double.PositiveInfinity);
            //Measure(size);
            SizeControls(size);
            SizeControls();
        }
        
        private void SizeControls()
        {
            SizeControls(Bounds.Size);
        }

        private void SizeControls(Size newSize)
        {
            var children = Children.Reverse().OfType<RibbonGroupBox>();

            if (Orientation == Orientation.Vertical)
            {
                int count = 0;
                while (GetChildrenTotalHeight() >= newSize.Height)
                {
                    var largeChildren = children.Where(x => x.DisplayMode == GroupDisplayMode.Large);
                    
                    if (largeChildren.Count() > 0)
                    {
                        var firstLargeChild = largeChildren.First();
                        firstLargeChild.DisplayMode = GroupDisplayMode.Small;
                        firstLargeChild.InvalidateArrange();
                        firstLargeChild.InvalidateMeasure();
                        firstLargeChild.Measure(newSize);
                    }
                    else
                        break;

                    count++;
                    if (count >= children.Count())
                        break;
                }
                count = 0;
                while (GetChildrenTotalHeight() < newSize.Height)
                {
                    var nonLargeChildren = children.Where(x => x.DisplayMode != GroupDisplayMode.Large);
                    
                    if (nonLargeChildren.Count() > 0)
                    {
                        var lastNonLargeChild = nonLargeChildren.Last();
                        lastNonLargeChild.DisplayMode = GroupDisplayMode.Large;
                        lastNonLargeChild.InvalidateArrange();
                        lastNonLargeChild.InvalidateMeasure();
                        lastNonLargeChild.Measure(newSize);

                        if (GetChildrenTotalHeight() > newSize.Height)
                            lastNonLargeChild.DisplayMode = GroupDisplayMode.Small;
                    }
                    else
                        break;

                    count++;
                    if (count >= children.Count())
                        break;
                }
            }
            else
            {
                int count = 0;
                while (GetChildrenTotalWidth() >= newSize.Width)
                {
                    var largeChildren = children.Where(x => x.DisplayMode == GroupDisplayMode.Large);
                    
                    if (largeChildren.Count() > 0)
                    {
                        var firstLargeChild = largeChildren.First();
                        firstLargeChild.DisplayMode = GroupDisplayMode.Small;
                        firstLargeChild.InvalidateArrange();
                        firstLargeChild.InvalidateMeasure();
                        firstLargeChild.Measure(newSize);
                    }
                    else
                        break;

                    count++;
                    if (count >= children.Count())
                        break;
                }
                count = 0;
                while (GetChildrenTotalWidth() < newSize.Width)
                {
                    var nonLargeChildren = children.Where(x => x.DisplayMode != GroupDisplayMode.Large);
                    
                    if (nonLargeChildren.Count() > 0)
                    {
                        var lastNonLargeChild = nonLargeChildren.Last();
                        lastNonLargeChild.DisplayMode = GroupDisplayMode.Large;
                        lastNonLargeChild.InvalidateArrange();
                        lastNonLargeChild.InvalidateMeasure();
                        lastNonLargeChild.Measure(newSize);

                        if (GetChildrenTotalWidth() > newSize.Width)
                            lastNonLargeChild.DisplayMode = GroupDisplayMode.Small;
                    }
                    else
                        break;

                    count++;
                    if (count >= children.Count())
                        break;
                }
            }
        }

        double GetChildrenTotalWidth()
        {
            var children = Children.OfType<RibbonGroupBox>(); //.Reverse().Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>();
            double totalWidth = 0;

            Size desiredSize = DesiredSize;
            if (Orientation == Orientation.Vertical)
                desiredSize = desiredSize.WithWidth(Bounds.Width);
            else
                desiredSize = desiredSize.WithHeight(Bounds.Height);

            Arrange(new Rect(desiredSize));
            Measure(desiredSize);
            for (int i = 0; i < children.Count(); i++)
            {
                /*double newSize = children.ElementAt(i).Bounds.X - totalWidth;
                if (newSize <= 0)
                    totalWidth += children.ElementAt(i).Bounds.Width + newSize;*/
                totalWidth += Math.Max(0, children.ElementAt(i).Bounds.Width);
            }

            return totalWidth;
        }

        double GetChildrenTotalHeight()
        {
            var children = Children.OfType<RibbonGroupBox>(); //.Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>();
            double totalHeight = 0;

            Arrange(new Rect(DesiredSize));
            Measure(DesiredSize);
            for (int i = 0; i < children.Count(); i++)
            {
                double newSize = children.ElementAt(i).Bounds.Y - totalHeight;
                if (newSize <= 0)
                    totalHeight += children.ElementAt(i).Bounds.Height + newSize;
            }
            return totalHeight;
        }
    }
}
