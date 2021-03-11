using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Icon = System.Drawing.Icon;
using System.Timers;

namespace AvaloniaUI.Ribbon
{
    public class RibbonWindow : Window, IStyleable
    {
        public static readonly StyledProperty<IBrush> TitleBarBackgroundProperty = AvaloniaProperty.Register<RibbonWindow, IBrush>(nameof(TitleBarBackground));
        public IBrush TitleBarBackground
        {
            get => GetValue(TitleBarBackgroundProperty);
            set => SetValue(TitleBarBackgroundProperty, value);
        }

        public static readonly StyledProperty<IBrush> TitleBarForegroundProperty = AvaloniaProperty.Register<RibbonWindow, IBrush>(nameof(TitleBarForeground));
        public IBrush TitleBarForeground
        {
            get => GetValue(TitleBarForegroundProperty);
            set => SetValue(TitleBarForegroundProperty, value);
        }

        public static readonly StyledProperty<Orientation> OrientationProperty = StackLayout.OrientationProperty.AddOwner<RibbonWindow>();
        public Orientation Orientation
        {
            get => GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        static bool UseLeftSideCaptionButtons()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return true;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                //TODO: See if there's any sane way of getting  the user's Window manager/decorator/etc and its configuration, and deciding or guessing based on that
                return false;
            }
            else //on Windows
                return false;
        }
        public static readonly StyledProperty<bool> LeftSideCaptionButtonsProperty = AvaloniaProperty.Register<RibbonWindow, bool>(nameof(LeftSideCaptionButtons), UseLeftSideCaptionButtons());
        public bool LeftSideCaptionButtons
        {
            get => GetValue(LeftSideCaptionButtonsProperty);
            set => SetValue(LeftSideCaptionButtonsProperty, value);
        }

        public static readonly StyledProperty<Ribbon> RibbonProperty = AvaloniaProperty.Register<RibbonWindow, Ribbon>(nameof(Ribbon), null);
        public Ribbon Ribbon
        {
            get => GetValue(RibbonProperty);
            set => SetValue(RibbonProperty, value);
        }

        public static readonly StyledProperty<QuickAccessToolbar> QuickAccessToolbarProperty = Ribbon.QuickAccessToolbarProperty.AddOwner<RibbonWindow>();
        public QuickAccessToolbar QuickAccessToolbar
        {
            get => GetValue(QuickAccessToolbarProperty);
            set => SetValue(QuickAccessToolbarProperty, value);
        }


        static RibbonWindow()
        {
            OrientationProperty.OverrideDefaultValue<RibbonWindow>(Orientation.Horizontal);

            RibbonProperty.Changed.AddClassHandler<RibbonWindow>((sender, e) => sender.RefreshRibbon(e.OldValue, e.NewValue));
            QuickAccessToolbarProperty.Changed.AddClassHandler<RibbonWindow>((sender, e) => sender.RefreshQat(e.OldValue, e.NewValue));
        }

        public RibbonWindow() : base()
        {
            RefreshRibbon(null, Ribbon);
            RefreshQat(null, QuickAccessToolbar);
        }

        void RefreshRibbon(object oldValue, object newValue)
        {
            if ((oldValue != null) && (oldValue is Ribbon oldRibbon))
            {
                oldRibbon.QuickAccessToolbar = null;
                oldRibbon.ClearValue(Ribbon.OrientationProperty); 
            }


            if ((newValue != null) && (newValue is Ribbon newRibbon))
            {
                newRibbon.QuickAccessToolbar = QuickAccessToolbar;
                newRibbon[!Ribbon.OrientationProperty] = this[!OrientationProperty];

                if (QuickAccessToolbar != null)
                    QuickAccessToolbar.Ribbon = newRibbon;
            }
            else if (QuickAccessToolbar != null)
                QuickAccessToolbar.Ribbon = null;
        }

        void RefreshQat(object oldValue, object newValue)
        {
            if ((oldValue != null) && (oldValue is QuickAccessToolbar oldQat))
                oldQat.Ribbon = null;


            if ((newValue != null) && (newValue is QuickAccessToolbar newQat))
            {
                newQat.Ribbon = Ribbon;

                if (Ribbon != null)
                    Ribbon.QuickAccessToolbar = newQat;
            }
            else if (Ribbon != null)
                Ribbon.QuickAccessToolbar = null;
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

        
        bool _titlebarSecondClick = false;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            var window = this;
            try
            {
                var titleBar = GetControl<Control>(e, "PART_TitleBar");

                titleBar.PointerPressed += (object sender, PointerPressedEventArgs ep) =>
                {
                    if (_titlebarSecondClick)
                        window.WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                    else
                        window.PlatformImpl?.BeginMoveDrag(ep);
                    

                    if (!_titlebarSecondClick)
                    {
                        _titlebarSecondClick = true;

                        Timer secondClickTimer = new Timer(250);
                        secondClickTimer.Elapsed += (sneder, e) =>
                        {
                            _titlebarSecondClick = false;
                            secondClickTimer.Stop();
                        };
                        secondClickTimer.Start();
                    }
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

                GetControl<Button>(e, "PART_MinimizeButton").Click += delegate
                {
                    window.WindowState = WindowState.Minimized;
                };
                GetControl<Button>(e, "PART_MaximizeButton").Click += delegate
                {
                    window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                };
                GetControl<Button>(e, "PART_CloseButton").Click += delegate
                {
                    window.Close();
                };
            }
            catch (KeyNotFoundException) { }
        }

        Type IStyleable.StyleKey => typeof(RibbonWindow);
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
                catch (ArgumentNullException)
                {
                    try
                    {

                        Icon icon = new Icon(stream);
                        System.Drawing.Bitmap bmp = icon.ToBitmap();
                        bmp.Save(stream, ImageFormat.Png);
                        return new Bitmap(stream);
                    }
                    catch (ArgumentException)
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
