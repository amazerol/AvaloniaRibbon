using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    /*public class DoubleBindingsToPointConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            string[] paramParts = parameter.ToString().Split(',');
            
            double x = 0;
            double y = 0;


            if (!double.TryParse(paramParts[0], out x))
            {
                x = (double)values[0];
            }
            
            if (!double.TryParse(paramParts[1], out y))
            {
                y = (double)values[1];
            }

            return new Point(x, y);
        }
    }*/

    public class BoundsPointToAdjustedPointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double x = 0;
            double y = 0;

            if ((value is Rect rect) && (parameter != null))
            {
                string[] paramParts = parameter.ToString().Replace(" ", string.Empty).Split(',');

                string pt = paramParts[2];
                char ptX = pt[1];
                char ptY = pt[0];

                if (ptX == 'R')
                    x = rect.Width;
                else if (ptX == 'C')
                    x = rect.Width / 2;
                
                if (ptY == 'B')
                    y = rect.Height;
                else if (ptY == 'C')
                    y = rect.Height / 2;

                /*x = rect.Width;
                y = rect.Height;*/
                
                if (double.TryParse(paramParts[0], out double xAdjust))
                    x += xAdjust;
                
                if (double.TryParse(paramParts[1], out double yAdjust))
                    y += yAdjust;
                
                
            }

            
            return new Point(x, y);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}