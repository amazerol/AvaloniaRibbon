using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonGroupsGrid : Grid
    {
        /*public RibbonGroupGrid()
        {
            if (TemplatedParent is RibbonGroupBox parentBox)
            {
                
                parentBox.Rearranged += (sneder, args) => ArrangeOverride(Bounds.Size);
                parentBox.Remeasured += (sneder, args) => MeasureOverride(Bounds.Size);
            }
        }*/

        int _lastChildrenCount = 0;
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var children = Children.Where(x => x is RibbonGroupBox).Cast<RibbonGroupBox>();
            double totalChildrenWidth = 0;
            bool childrenCountChanged = children.Count() != _lastChildrenCount;

            Size tempSize = new Size(double.PositiveInfinity, Bounds.Height);
            //string templateArea = string.Empty;
            string columnDefinitions = string.Empty;
            //////GroupDisplayMode[] displayModes = new GroupDisplayMode[children.Count()];
            for (int i = 0; i < children.Count(); i++)
            {
                RibbonGroupBox box = children.ElementAt(i);
                GroupDisplayMode oldMode = box.DisplayMode;
                //GridExtra.Avalonia.GridEx.SetAreaName(box, i.ToString());
                if (childrenCountChanged)
                    Grid.SetColumn(box, i);
                //////displayModes[i] = box.DisplayMode;
                box.DisplayMode = GroupDisplayMode.Large;
                totalChildrenWidth += box.DesiredSize.Width;
                //box.Measure(tempSize);
                //box.Arrange(arrangeSize);
                //templateArea += i.ToString() + " ";
                columnDefinitions += "Auto,";
                box.DisplayMode = oldMode;
            }
            if ((children.Count() > 0) && childrenCountChanged)
            {
                columnDefinitions = columnDefinitions.Substring(0, columnDefinitions.LastIndexOf(','));
                ColumnDefinitions = new ColumnDefinitions(columnDefinitions);
            }

            /*for (int i = 0; i < children.Count(); i++)
            {
                //if (children.ElementAt(i).DisplayMode != GroupDisplayMode.Large)
                children.ElementAt(i).DisplayMode = displayModes[i];
            }*/

            //templateArea += "/";
            //GridExtra.Avalonia.GridEx.SetTemplateArea(this, templateArea);
            //Size bigSize = base.ArrangeOverride(arrangeSize);
            Debug.WriteLine(totalChildrenWidth + "; " + arrangeSize.Width);
            if (totalChildrenWidth > arrangeSize.Width)
            {
                for (int i = children.Count() - 1; i >= 0; i--)
                {
                    children.ElementAt(i).DisplayMode = GroupDisplayMode.Small;
                    //children.ElementAt(i).Arrange(new Rect());
                    ////children.ElementAt(i).Measure(tempSize);
                    //children.ElementAt(i).Measure(arrangeSize);
                    //children.ElementAt(i).Arrange(arrangeSize);
                    /*double newTotalChildrenWidth = 0;
                    for (int j = 0; j < children.Count(); j++)
                        newTotalChildrenWidth += children.ElementAt(i).DesiredSize.Width;

                    if (newTotalChildrenWidth <= arrangeSize.Width)
                        break;*/
                }
            }
            /*if (DisplayMode == GroupDisplayMode.Large)
            {
                
            }*/

            _lastChildrenCount = children.Count();
            try
            {
                return base.ArrangeOverride(arrangeSize);
            }
            catch (NullReferenceException nex)
            {
                return arrangeSize;
            }
        }
    }
}
