using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using Icon = System.Drawing.Icon;

namespace AvaloniaUI.Ribbon
{
    public class RibbonWindow : Window, IStyleable
    {
        public static readonly StyledProperty<IBrush> TitleBarBackgroundProperty;
        public static readonly StyledProperty<IBrush> TitleBarForegroundProperty;
        
        static RibbonWindow()
        {
            TitleBarBackgroundProperty = AvaloniaProperty.Register<RibbonWindow, IBrush>(nameof(TitleBarBackground));
            TitleBarForegroundProperty = AvaloniaProperty.Register<RibbonWindow, IBrush>(nameof(TitleBarForeground));
        }

        Type IStyleable.StyleKey => typeof(RibbonWindow);

        public IBrush TitleBarBackground
        {
            get { return GetValue(TitleBarBackgroundProperty); }
            set { SetValue(TitleBarBackgroundProperty, value); }
        }

        public IBrush TitleBarForeground
        {
            get { return GetValue(TitleBarForegroundProperty); }
            set { SetValue(TitleBarForegroundProperty, value); }
        }

        void SetupSide(string name, StandardCursorType cursor, WindowEdge edge, ref TemplateAppliedEventArgs e)
        {
            var control = e.NameScope.Get<Control>(name);
            control.Cursor = new Cursor(cursor);
            control.PointerPressed += (object sender, PointerPressedEventArgs ep) =>
            {
                ((Window)this.GetVisualRoot()).PlatformImpl?.BeginResizeDrag(edge, ep);
            };
        }

        T GetControl<T>(TemplateAppliedEventArgs e, string name) where T : class
        {
            return e.NameScope.Get<T>(name);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            var window = this;
            try
            {
                var titleBar = GetControl<Control>(e, "TitleBar");

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {

                    titleBar.DoubleTapped += delegate
                    {
                        window.WindowState = ((Window)this.GetVisualRoot()).WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                    };
                }

                titleBar.PointerPressed += (object sender, PointerPressedEventArgs ep) =>
                {
                    window.PlatformImpl?.BeginMoveDrag(ep);
                };

                try
                {
                    SetupSide("Left_top", StandardCursorType.LeftSide, WindowEdge.West, ref e);
                    SetupSide("Left_mid", StandardCursorType.LeftSide, WindowEdge.West, ref e);
                    SetupSide("Left_bottom", StandardCursorType.LeftSide, WindowEdge.West, ref e);
                    SetupSide("Right_top", StandardCursorType.RightSide, WindowEdge.East, ref e);
                    SetupSide("Right_mid", StandardCursorType.RightSide, WindowEdge.East, ref e);
                    SetupSide("Right_bottom", StandardCursorType.RightSide, WindowEdge.East, ref e);
                    SetupSide("Top", StandardCursorType.TopSide, WindowEdge.North, ref e);
                    SetupSide("Bottom", StandardCursorType.BottomSide, WindowEdge.South, ref e);
                    SetupSide("TopLeft", StandardCursorType.TopLeftCorner, WindowEdge.NorthWest, ref e);
                    SetupSide("TopRight", StandardCursorType.TopRightCorner, WindowEdge.NorthEast, ref e);
                    SetupSide("BottomLeft", StandardCursorType.BottomLeftCorner, WindowEdge.SouthWest, ref e);
                    SetupSide("BottomRight", StandardCursorType.BottomRightCorner, WindowEdge.SouthEast, ref e);
                }
                catch { }

                GetControl<Button>(e, "MinimizeButton").Click += delegate
                {
                    window.WindowState = WindowState.Minimized;
                };
                GetControl<Button>(e, "MaximizeButton").Click += delegate
                {
                    window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                };
                GetControl<Button>(e, "CloseButton").Click += delegate
                {
                    window.Close();
                };
            }
            catch (KeyNotFoundException) { }
        }
    }

    public class WindowIconToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var wIcon = value as WindowIcon;
                MemoryStream stream = new MemoryStream();
                wIcon.Save(stream);
                stream.Position = 0;
                try
                {
                    return new Bitmap(stream);
                }
                catch (ArgumentNullException ex)
                {
                    try
                    {

                        Icon icon = new Icon(stream);
                        System.Drawing.Bitmap bmp = icon.ToBitmap();
                        bmp.Save(stream, ImageFormat.Png);
                        return new Bitmap(stream);
                    }
                    catch (ArgumentException e)
                    {
                        Icon icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetEntryAssembly().Location);
                        System.Drawing.Bitmap bmp = icon.ToBitmap();
                        Stream stream3 = new MemoryStream();
                        bmp.Save(stream3, ImageFormat.Png);
                        return new Bitmap(stream3);
                    }
                }
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
