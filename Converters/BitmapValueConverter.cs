// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia;
using Avalonia.Data.Converters;
using System.Globalization;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace AvaloniaRibbon.Converters
{

    public class BitmapValueConverter : IValueConverter
    {
        public static BitmapValueConverter Instance = new BitmapValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if ((string)value != String.Empty)
                {
                    string val = "." + (string)value;
                    return new Avalonia.Media.Imaging.Bitmap(val);
                }
                return null;
            } catch
            {
                return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
