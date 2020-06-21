using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    public class RibbonGroupsStackPanel : StackPanel
    {
        double _lastArrangeSizeWidth = -1;
        double _lastTotalChildrenWidth = -1;
        double _lastArrangeSizeHeight = -1;
        double _lastTotalChildrenHeight = -1;
        bool _cycle2 = false;

        static RibbonGroupsStackPanel()
        {
            ParentProperty.Changed.AddClassHandler<RibbonGroupsStackPanel>((sender, e) =>
            {
                Dispatcher.UIThread.Post(() => sender.UpdateLayoutState());
            });
        }

        public RibbonGroupsStackPanel()
        {
            LayoutUpdated += (sneder, args) => UpdateLayoutState();

            AttachedToVisualTree += (sneder, args) =>
            {
                    AdjustForChangedChildren();
                    UpdateLayoutState();
            };
        }

        protected override void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.ChildrenChanged(sender, e);
            AdjustForChangedChildren();
        }

        protected override void LogicalChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.LogicalChildrenCollectionChanged(sender, e);
            AdjustForChangedChildren();
        }

        void AdjustForChangedChildren()
        {
            foreach (RibbonGroupBox box in Children.Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>())
            {
                box.DisplayMode = GroupDisplayMode.Large;
                box.InvalidateArrange();
                box.InvalidateMeasure();
                box.Measure(Bounds.Size);
            }

            Dispatcher.UIThread.Post(() =>
            {
                if (IsInitialized && (
                    ((Orientation == Orientation.Horizontal) && (_lastTotalChildrenWidth > 0))
                    || ((Orientation == Orientation.Vertical) && (_lastTotalChildrenHeight > 0))
                    ) && (Children.Count() > 0))
                {
                    _cycle2 = false;
                    UpdateLayoutState();
                }
            });
        }

        protected void UpdateLayoutState()
        {
            if (_cycle2 && (_lastTotalChildrenWidth >= 0))
            {
                SizeControls(Bounds.Size, _lastTotalChildrenWidth, _lastTotalChildrenHeight);
                _cycle2 = false;
            }
            else
            {
                _lastTotalChildrenWidth = GetChildrenTotalWidth();
                _lastTotalChildrenHeight = GetChildrenTotalHeight();
                _cycle2 = true;
                Measure(Bounds.Size);
            }
        }

        private void SizeControls(Size arrangeSize, double lastTotalChildrenWidth, double lastTotalChildrenHeight)
        {
            var children = Children.Reverse().Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>();

            if (Orientation == Orientation.Vertical)
            {
                if (_lastArrangeSizeHeight >= 0)
                {
                    if (lastTotalChildrenHeight > arrangeSize.Height)
                    {
                        int count = 0;
                        while (GetChildrenTotalHeight() > arrangeSize.Height)
                        {
                            var largeChildren = children.Where(x => x.DisplayMode == GroupDisplayMode.Large);
                            if (largeChildren.Count() > 0)
                            {
                                var firstLargeChild = largeChildren.First();
                                firstLargeChild.DisplayMode = GroupDisplayMode.Small;
                                firstLargeChild.InvalidateArrange();
                                firstLargeChild.InvalidateMeasure();
                                firstLargeChild.Measure(arrangeSize);
                            }
                            else
                                break;

                            count++;
                            if (count >= children.Count())
                                break;
                        }
                    }
                    else if (lastTotalChildrenHeight <= arrangeSize.Height)
                    {
                        int count = 0;
                        while (GetChildrenTotalHeight() <= arrangeSize.Height)
                        {
                            var nonLargeChildren = children.Where(x => x.DisplayMode != GroupDisplayMode.Large);
                            if (nonLargeChildren.Count() > 0)
                            {
                                var lastNonLargeChild = nonLargeChildren.Last();
                                lastNonLargeChild.DisplayMode = GroupDisplayMode.Large;
                                lastNonLargeChild.InvalidateArrange();
                                lastNonLargeChild.InvalidateMeasure();
                                lastNonLargeChild.Measure(arrangeSize);

                                if (GetChildrenTotalHeight() > arrangeSize.Height)
                                {
                                    lastNonLargeChild.DisplayMode = GroupDisplayMode.Small;
                                    break;
                                }
                            }
                            else
                                break;

                            count++;
                            if (count >= children.Count())
                                break;
                        }
                    }
                }
                _lastArrangeSizeHeight = arrangeSize.Height;
            }
            else
            {
                if (_lastArrangeSizeWidth >= 0)
                {
                    if (lastTotalChildrenWidth > arrangeSize.Width)
                    {
                        int count = 0;
                        while (GetChildrenTotalWidth() > arrangeSize.Width)
                        {
                            var largeChildren = children.Where(x => x.DisplayMode == GroupDisplayMode.Large);
                            if (largeChildren.Count() > 0)
                            {
                                var firstLargeChild = largeChildren.First();
                                firstLargeChild.DisplayMode = GroupDisplayMode.Small;
                                firstLargeChild.InvalidateArrange();
                                firstLargeChild.InvalidateMeasure();
                                firstLargeChild.Measure(arrangeSize);
                            }
                            else
                                break;

                            count++;
                            if (count >= children.Count())
                                break;
                        }
                    }
                    else if (lastTotalChildrenWidth <= arrangeSize.Width)
                    {
                        int count = 0;
                        while (GetChildrenTotalWidth() <= arrangeSize.Width)
                        {
                            var nonLargeChildren = children.Where(x => x.DisplayMode != GroupDisplayMode.Large);
                            if (nonLargeChildren.Count() > 0)
                            {
                                var lastNonLargeChild = nonLargeChildren.Last();
                                lastNonLargeChild.DisplayMode = GroupDisplayMode.Large;
                                lastNonLargeChild.InvalidateArrange();
                                lastNonLargeChild.InvalidateMeasure();
                                lastNonLargeChild.Measure(arrangeSize);

                                if (GetChildrenTotalWidth() > arrangeSize.Width)
                                {
                                    lastNonLargeChild.DisplayMode = GroupDisplayMode.Small;
                                    break;
                                }
                            }
                            else
                                break;

                            count++;
                            if (count >= children.Count())
                                break;
                        }
                    }
                }
                _lastArrangeSizeWidth = arrangeSize.Width;
            }
        }

        double GetChildrenTotalWidth()
        {
            var children = Children.Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>();
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
                double newSize = children.ElementAt(i).Bounds.X - totalWidth;
                if (newSize <= 0)
                    totalWidth += children.ElementAt(i).Bounds.Width + newSize;
            }

            return totalWidth;
        }

        double GetChildrenTotalHeight()
        {
            var children = Children.Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>();
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
