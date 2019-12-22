using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Ribbon;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;

namespace Avalonia.Ribbon.Samples.Views
{
    public class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            //this.FindName<CheckBox>("VerticalRibbonCheckBox").
            Controls.Ribbon.Ribbon ribbon = this.Find<Controls.Ribbon.Ribbon>("RibbonControl");
            Button verticalRibbonButton = this.Find<Button>("VerticalRibbonButton");
            verticalRibbonButton.Click += (sneder, args) =>
            {
                ribbon.Orientation = Orientation.Vertical;
                DockPanel.SetDock(ribbon, Dock.Left);
            };
            Button horizontalRibbonButton = this.Find<Button>("HorizontalRibbonButton");
            horizontalRibbonButton.Click += (sneder, args) =>
            {
                ribbon.Orientation = Orientation.Horizontal;
                DockPanel.SetDock(ribbon, Dock.Top);
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
