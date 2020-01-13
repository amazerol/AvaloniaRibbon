using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            double width = Bounds.Width;
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
            }
        }

        private void RibbonGroupsStackPanel_LayoutUpdated(object sender, EventArgs e)
        {
            if (_cycle2 && (_lastTotalChildrenWidth >= 0))
            {
                SizeControls(Bounds.Size, (Bounds.Size.Width < _lastArrangeSizeWidth), (Bounds.Size.Width > _lastArrangeSizeWidth));
                _cycle2 = false;
            }
            else
            {
                _lastTotalChildrenWidth = GetChildrenTotalWidth();
                _cycle2 = true;
                Measure(Bounds.Size);
            }
        }

        /*private double Arrange1(Size arrangeSize)
        {
            var children = Children.Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>();
            double totalChildrenWidth = 0;
            
            foreach (RibbonGroupBox box in children)
            {
                totalChildrenWidth += box.Bounds.Width;
            }

            return totalChildrenWidth;
        }*/

        private void SizeControls(Size arrangeSize, bool enlarge, bool shrink)
        {
            var children = Children.Reverse().Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>();

            if (_lastArrangeSizeWidth >= 0)
            {
                if ((_lastTotalChildrenWidth > arrangeSize.Width) && enlarge)
                {
                    /*var largeChildren = children.Where(x => x.DisplayMode == GroupDisplayMode.Large);
                    foreach (RibbonGroupBox box in largeChildren)
                    {
                        double newTotalChildrenWidth = 0;
                        for (int j = 0; j < children.Count(); j++)
                        {
                            RibbonGroupBox box2 = children.ElementAt(j);
                            box2.InvalidateArrange();
                            box2.InvalidateMeasure();
                            box2.Measure(arrangeSize);
                            newTotalChildrenWidth += box2.Bounds.Width;
                        }
                        
                        if (GetChildrenTotalWidth() > arrangeSize.Width)
                            box.DisplayMode = GroupDisplayMode.Small;

                        break;
                    }*/
                    var largeChildren = children.Where(x => x.DisplayMode == GroupDisplayMode.Large);
                    if (largeChildren.Count() > 0)
                    {
                        var firstLargeChild = largeChildren.First();
                        firstLargeChild.DisplayMode = GroupDisplayMode.Small;
                        firstLargeChild.InvalidateArrange();
                        firstLargeChild.InvalidateMeasure();
                        firstLargeChild.Measure(arrangeSize);
                    }
                }
                else if ((_lastTotalChildrenWidth <= arrangeSize.Width) && shrink)
                {
                    /*var nonLargeChildren = children.Where(x => x.DisplayMode != GroupDisplayMode.Large);
                    foreach (RibbonGroupBox box in nonLargeChildren)
                    {
                        box.DisplayMode = GroupDisplayMode.Large;
                        double newTotalChildrenWidth = 0;
                        for (int j = 0; j < children.Count(); j++)
                        {
                            RibbonGroupBox box2 = children.ElementAt(j);
                            box2.InvalidateArrange();
                            box2.InvalidateMeasure();
                            box2.Measure(arrangeSize);
                            newTotalChildrenWidth += box2.Bounds.Width;
                        }

                        if (GetChildrenTotalWidth() > arrangeSize.Width)
                        {
                            box.DisplayMode = GroupDisplayMode.Small;
                        }

                    }*/
                    var nonLargeChildren = children.Where(x => x.DisplayMode != GroupDisplayMode.Large);
                    if (nonLargeChildren.Count() > 0)
                    {
                        var lastNonLargeChild = nonLargeChildren.Last();
                        lastNonLargeChild.DisplayMode = GroupDisplayMode.Large;
                        lastNonLargeChild.InvalidateArrange();
                        lastNonLargeChild.InvalidateMeasure();
                        lastNonLargeChild.Measure(arrangeSize);

                        if (GetChildrenTotalWidth() > arrangeSize.Width)
                            lastNonLargeChild.DisplayMode = GroupDisplayMode.Small;
                    }
                }
            }
            _lastArrangeSizeWidth = arrangeSize.Width;
        }

        double GetChildrenTotalWidth()
        {
            var children = Children.Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>();
            double totalWidth = 0;
            //GroupDisplayMode[] oldModes = new GroupDisplayMode[children.Count()];

            /*for (int i = 0; i < children.Count(); i++)
            {
                oldModes[i] = children.ElementAt(i).DisplayMode;
                if ((int)mode >= 0)
                    children.ElementAt(i).DisplayMode = mode;
            }*/
            Arrange(new Rect(DesiredSize));
            Measure(DesiredSize);
            for (int i = 0; i < children.Count(); i++)
            {
                totalWidth += children.ElementAt(i).Bounds.Width;
                //children.ElementAt(i).DisplayMode = oldModes[i];
            }

            return totalWidth;
        }
    }
}
