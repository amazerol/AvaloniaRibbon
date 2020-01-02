using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Avalonia.Layout;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonGroupsStackPanel : StackPanel
    {
        public static readonly StyledProperty<Orientation> ChildrenOrientationProperty = StackLayout.OrientationProperty.AddOwner<RibbonGroupsStackPanel>();
        
        public Orientation Orientation
        {
            get { return GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /*protected override Size ArrangeOverride(Size finalSize)
        {
            var children = Children.Where(x => x is RibbonGroupBox);
            double left = 0;
            Rect[] childRects = new Rect[children.Count()];
            
            for (int i = 0; i < children.Count(); i++)
            {
                RibbonGroupBox box = children.ElementAt(i) as RibbonGroupBox;
                double width = box.DesiredSize.Width;
                childRects[i] = new Rect(left, 0, width, box.DesiredSize.Height);
                left += (width + Spacing);
            }
            left -= Spacing;

            if (left > finalSize.Width)
            {
                double difference = 0;
                for (int i = (children.Count() - 1); i >= 0; i--)
                {
                    //RibbonGroupBox box2 = children.ElementAt(i) as RibbonGroupBox;
                    double oldWidth = childRects[i].Width;
                    double diff = Math.Abs(left - finalSize.Width);
                    childRects[i] = childRects[i].WithWidth(Math.Max(childRects[i].Width - diff, children.ElementAt(i).MinWidth));
                    if (difference > 0)
                        childRects[i] = childRects[i].WithX(childRects[i].X - difference);

                    difference = Math.Max(childRects[i].Width - oldWidth, 0);
                }
            }

            for (int i = 0; i < children.Count(); i++)
                (children.ElementAt(i) as RibbonGroupBox).Arrange(childRects[i]);

            return finalSize;
        }*/

        /*private double GetChildrenDesiredWidth()
        {
            double width = -1;
            foreach (Control ctrl in Children)
            {
                if (ctrl is RibbonGroupBox box)
                {
                    if (width == -1)
                        width = Spacing;
                    else
                        Width += Spacing;

                    width += box.DesiredSize.Width;
                }
            }

            return width;
        }*/

        protected override Size ArrangeOverride(Size finalSize)
        {
            var children = Children;
            bool fHorizontal = (Orientation == Orientation.Horizontal);
            Rect rcChild = new Rect(finalSize);
            double previousChildSize = 0.0;
            var spacing = Spacing;
            //return finalSize;
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];

                if (child == null || !child.IsVisible)
                { continue; }

                if (fHorizontal)
                {
                    rcChild = rcChild.WithX(rcChild.X + previousChildSize);
                    previousChildSize = child.DesiredSize.Width;
                    rcChild = rcChild.WithWidth(previousChildSize);
                    rcChild = rcChild.WithHeight(Math.Max(finalSize.Height, child.DesiredSize.Height));
                    previousChildSize += spacing;
                    Rect rect = ArrangeChild(child, rcChild, finalSize, Orientation);
                    previousChildSize -= child.DesiredSize.Width;
                    previousChildSize += rect.Width;
                }
                else
                {
                    rcChild = rcChild.WithY(rcChild.Y + previousChildSize);
                    previousChildSize = child.DesiredSize.Height;
                    rcChild = rcChild.WithHeight(previousChildSize);
                    rcChild = rcChild.WithWidth(Math.Max(finalSize.Width, child.DesiredSize.Width));
                    previousChildSize += spacing;
                    Rect rect = ArrangeChild(child, rcChild, finalSize, Orientation);
                    previousChildSize -= child.DesiredSize.Height;
                    previousChildSize += rect.Height;
                }
            }

            return finalSize;


            /*double totalChildrenWidth = 0;
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                totalChildrenWidth += child.DesiredSize.Width;
                if (i < (children.Count - 1))
                    totalChildrenWidth += spacing;
            }
            while (totalChildrenWidth > finalSize.Width)
            {
                double newTotalChildrenWidth = 0;
                for (int i = 0; i < children.Count; i++)
                {

                }
            }

            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];

                if (child == null || !child.IsVisible)
                { continue; }

                if (fHorizontal)
                {
                    rcChild = rcChild.WithX(rcChild.X + previousChildSize);
                    previousChildSize = child.DesiredSize.Width;
                    rcChild = rcChild.WithWidth(previousChildSize);
                    rcChild = rcChild.WithHeight(Math.Max(finalSize.Height, child.DesiredSize.Height));
                    previousChildSize += spacing;
                }
                else
                {
                    rcChild = rcChild.WithY(rcChild.Y + previousChildSize);
                    previousChildSize = child.DesiredSize.Height;
                    rcChild = rcChild.WithHeight(previousChildSize);
                    rcChild = rcChild.WithWidth(Math.Max(finalSize.Width, child.DesiredSize.Width));
                    previousChildSize += spacing;
                }

                ArrangeChild(child, rcChild, finalSize, Orientation);
            }

            return finalSize;*/
        }

        internal virtual void yArrangeChild(
            IControl child,
            Rect rect,
            Size panelSize,
            Orientation orientation)
        {
            child.Arrange(rect);
        }

        internal virtual Rect ArrangeChild(
            IControl child,
            Rect rect,
            Size panelSize,
            Orientation orientation)
        {
            double newWidth = rect.Width;
            double newHeight = rect.Height;
            if (rect.Right > panelSize.Width)
                newWidth -= rect.Right - panelSize.Width;
            /*if (rect.Bottom > panelSize.Height)
                newHeight -= rect.Bottom - panelSize.Height;*/
            Rect rect2 = rect.WithWidth(Math.Max(child.MinWidth, newWidth)); //new Rect(rect.X, rect.Y, Math.Max(0, newWidth), rect.Height);
            child.Arrange(rect2);
            child.Measure(new Size(rect2.Width, rect2.Height));
            return rect2;
        }
    }
}
