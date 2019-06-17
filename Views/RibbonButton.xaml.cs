// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Linq;
using ICommand = System.Windows.Input.ICommand;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using System.Collections;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using System.Globalization;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace AvaloniaRibbon.Views
{
    public class RibbonButton : Button
    {

        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<RibbonButton, string>(nameof(Text));

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public static readonly StyledProperty<IBitmap> IconPathProperty =
            AvaloniaProperty.Register<RibbonButton, IBitmap>(nameof(IconPath));

        public IBitmap IconPath
        {
            get { return GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

    }

}
