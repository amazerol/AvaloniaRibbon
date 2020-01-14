using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonGroupsStackPanel : StackPanel
    {
        double _lastArrangeSizeWidth = -1;
        double _lastTotalChildrenWidth = -1;
        bool _cycle2 = false;

        public RibbonGroupsStackPanel()
        {
            LayoutUpdated += RibbonGroupsStackPanel_LayoutUpdated;
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
            if (IsInitialized && (_lastTotalChildrenWidth > 0) && (Children.Count() > 0))
            {
                _cycle2 = false;
                UpdateLayoutState();
            }
            /*foreach (RibbonGroupBox box in Children.Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>())
            {
                box.InvalidateArrange();
                box.InvalidateMeasure();
                box.Measure(Bounds.Size);
            }*/
            /*if (IsInitialized && (_lastTotalChildrenWidth > 0) && (Children.Count() > 0) && (GetChildrenTotalWidth() == _lastTotalChildrenWidth))
            {
                SizeControls(Bounds.Size, _lastTotalChildrenWidth);
            }*/

            //
            /*double width = Bounds.Width;
            if (GetChildrenTotalWidth() > width)
            {
                while (GetChildrenTotalWidth() > width)
                {
                    if (width == Bounds.Width)
                        break;

                    SizeControls(Bounds.Size, false, true);
                    width = Bounds.Width;
                }
            }
            else if (GetChildrenTotalWidth() < width)
            {
                while (GetChildrenTotalWidth() < width)
                {
                    SizeControls(Bounds.Size, true, false);
                    width = Bounds.Width;
                    
                    if (width == Bounds.Width)
                        break;
                    else if (width > Bounds.Width)
                    {
                        SizeControls(Bounds.Size, false, true);
                        break;
                    }
                }
            }*/
        }

        private void RibbonGroupsStackPanel_LayoutUpdated(object sender, EventArgs e)
        {
            UpdateLayoutState();
        }

        protected void UpdateLayoutState()
        {
            if (_cycle2 && (_lastTotalChildrenWidth >= 0))
            {
                SizeControls(Bounds.Size, _lastTotalChildrenWidth);
                _cycle2 = false;
            }
            else
            {
                _lastTotalChildrenWidth = GetChildrenTotalWidth();
                _cycle2 = true;
                Measure(Bounds.Size);
            }
        }

        private void SizeControls(Size arrangeSize, double lastTotalChildrenWidth)
        {
            var children = Children.Reverse().Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>();

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

        double GetChildrenTotalWidth()
        {
            var children = Children.Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>();
            double totalWidth = 0;

            Arrange(new Rect(DesiredSize));
            Measure(DesiredSize);
            for (int i = 0; i < children.Count(); i++)
                totalWidth += children.ElementAt(i).Bounds.Width;

            return totalWidth;
        }
    }
}
