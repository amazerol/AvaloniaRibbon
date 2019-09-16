using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;

namespace AvaloniaRibbon.Views
{
    public class RibbonWindow : ItemsControl
    {
        public static readonly StyledProperty<IBrush> TitleBarColorProperty =
            AvaloniaProperty.Register<RibbonWindow, IBrush>(nameof(TitleBarColor));

        public IBrush TitleBarColor
        {
            get { return GetValue(TitleBarColorProperty); }
            set { SetValue(TitleBarColorProperty, value); }
        }

        void SetupSide(string name, StandardCursorType cursor, WindowEdge edge, ref TemplateAppliedEventArgs e)
        {
            var ctl = e.NameScope.Get<Control>(name);
            ctl.Cursor = new Cursor(cursor);
            ctl.PointerPressed += delegate
            {
                ((Window)this.GetVisualRoot()).PlatformImpl?.BeginResizeDrag(edge);
            };
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                e.NameScope.Get<Control>("TitleBar").DoubleTapped += delegate
                {
                    ((Window)this.GetVisualRoot()).WindowState = ((Window)this.GetVisualRoot()).WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                };
                ((Window)this.GetVisualRoot()).PropertyChanged += (s, ev) =>
                {
                    if (ev.Property.Name == "WindowState")
                    {
                        if (((Window)this.GetVisualRoot()).WindowState.HasFlag(WindowState.Maximized))
                        {
                            e.NameScope.Get<Image>("ImageMaximizeButton").Source = new Avalonia.Media.Imaging.Bitmap("./Assets/Window/already_maximized.png");
                        }
                        else
                        {
                            e.NameScope.Get<Image>("ImageMaximizeButton").Source = new Avalonia.Media.Imaging.Bitmap("./Assets/Window/maximize.png");
                        }
                    }
                };
            }
            e.NameScope.Get<Control>("TitleBar").PointerPressed += delegate
            {
                ((Window)this.GetVisualRoot()).PlatformImpl?.BeginMoveDrag();
            };
            SetupSide("Left_top", StandardCursorType.LeftSide, WindowEdge.West, ref e);
            SetupSide("Left_mid", StandardCursorType.LeftSide, WindowEdge.West, ref e);
            SetupSide("Left_bottom", StandardCursorType.LeftSide, WindowEdge.West, ref e);
            SetupSide("Right_top", StandardCursorType.RightSide, WindowEdge.East, ref e);
            SetupSide("Right_mid", StandardCursorType.RightSide, WindowEdge.East, ref e);
            SetupSide("Right_bottom", StandardCursorType.RightSide, WindowEdge.East, ref e);
            SetupSide("Top", StandardCursorType.TopSide, WindowEdge.North, ref e);
            SetupSide("Bottom", StandardCursorType.BottomSize, WindowEdge.South, ref e);
            SetupSide("TopLeft", StandardCursorType.TopLeftCorner, WindowEdge.NorthWest, ref e);
            SetupSide("TopRight", StandardCursorType.TopRightCorner, WindowEdge.NorthEast, ref e);
            SetupSide("BottomLeft", StandardCursorType.BottomLeftCorner, WindowEdge.SouthWest, ref e);
            SetupSide("BottomRight", StandardCursorType.BottomRightCorner, WindowEdge.SouthEast, ref e);
            e.NameScope.Get<Button>("MinimizeButton").Click += delegate { ((Window)this.GetVisualRoot()).WindowState = WindowState.Minimized; };
            e.NameScope.Get<Button>("MaximizeButton").Click += delegate
            {
                ((Window)this.GetVisualRoot()).WindowState = ((Window)this.GetVisualRoot()).WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            };
            e.NameScope.Get<Button>("CloseButton").Click += delegate
            {
                ((Window)this.GetVisualRoot()).Close();
            };

        }
    }
}
