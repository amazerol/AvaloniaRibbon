using System;
using System.Collections.Generic;
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

        private void RibbonGroupsStackPanel_LayoutUpdated(object sender, EventArgs e)
        {
            if ((_cycle2) && (_lastTotalChildrenWidth >= 0))
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

        private void SizeControls(Size arrangeSize, double totalChildrenWidth)
        {
            var children = Children.Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>();

            if (_lastArrangeSizeWidth >= 0)
            {
                if ((totalChildrenWidth > arrangeSize.Width) && (arrangeSize.Width < _lastArrangeSizeWidth))
                {
                    //for (int i = children.Count() - 1; i >= 0; i--)
                    foreach (RibbonGroupBox box in children.Where(x => x.DisplayMode == GroupDisplayMode.Large).Reverse())
                    {
                        //RibbonGroupBox box = children.ElementAt(i);
                        double newTotalChildrenWidth = 0;
                        //base.MeasureCore(arrangeSize);
                        for (int j = 0; j < children.Count(); j++)
                        {
                            RibbonGroupBox box2 = children.ElementAt(j);
                            box2.InvalidateArrange();
                            box2.InvalidateMeasure();
                            box2.Measure(arrangeSize);
                            //box2.DisplayMode = box2.DisplayMode;
                            //box.Arrange();
                            //box2.InvalidateArrange();
                            //box2.InvalidateMeasure();
                            //box2.Arrange(tempRect);
                            //box2.Measure(tempSize);
                            newTotalChildrenWidth += box2.Bounds.Width;
                        }
                        
                        if (GetChildrenTotalWidth() > arrangeSize.Width)
                            box.DisplayMode = GroupDisplayMode.Small;

                        break;
                    }
                }
                else if ((totalChildrenWidth <= arrangeSize.Width) && (arrangeSize.Width > _lastArrangeSizeWidth))
                {
                    //for (int i = 0; i < children.Count; i++)

                    //for (int i = 0; i < children.Count(); i++)
                    foreach (RibbonGroupBox box in children.Where(x => x.DisplayMode != GroupDisplayMode.Large))
                    {
                        //RibbonGroupBox box = children.ElementAt(i);
                        box.DisplayMode = GroupDisplayMode.Large;
                        double newTotalChildrenWidth = 0;
                        //base.MeasureCore(arrangeSize);
                        for (int j = 0; j < children.Count(); j++)
                        {
                            RibbonGroupBox box2 = children.ElementAt(j);
                            box2.InvalidateArrange();
                            box2.InvalidateMeasure();
                            box2.Measure(arrangeSize);
                            //box2.DisplayMode = box2.DisplayMode;
                            //box2.Measure(tempSize);
                            /*box2.InvalidateArrange();
                            box2.InvalidateMeasure();*/
                            //box2.Arrange(tempRect);
                            //box2.Measure(tempSize);
                            newTotalChildrenWidth += box2.Bounds.Width;
                        }

                        if (GetChildrenTotalWidth() > arrangeSize.Width)
                            box.DisplayMode = GroupDisplayMode.Small;

                    }
                }
            }
            //_lastTotalChildrenWidth = totalChildrenWidth;
            _lastArrangeSizeWidth = arrangeSize.Width;
        }

        double GetChildrenTotalWidth()
        {
            var children = Children.Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>();
            double totalWidth = 0;
            GroupDisplayMode[] oldModes = new GroupDisplayMode[children.Count()];

            for (int i = 0; i < children.Count(); i++)
            {
                oldModes[i] = children.ElementAt(i).DisplayMode;
                /*if ((int)mode >= 0)
                    children.ElementAt(i).DisplayMode = mode;*/
            }
            Arrange(new Rect(DesiredSize));
            Measure(DesiredSize);
            for (int i = 0; i < children.Count(); i++)
            {
                totalWidth += children.ElementAt(i).Bounds.Width;
                children.ElementAt(i).DisplayMode = oldModes[i];
            }

            return totalWidth;
        }
    }
}
