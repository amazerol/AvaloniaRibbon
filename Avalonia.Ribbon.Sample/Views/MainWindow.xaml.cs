using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Ribbon;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.Styling;
using System;

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
            Button horizontalRibbonButton = this.Find<Button>("HorizontalRibbonButton");
            verticalRibbonButton.Click += (sneder, args) =>
            {
                ribbon.Orientation = Orientation.Vertical;
                DockPanel.SetDock(ribbon, Dock.Left);
                verticalRibbonButton.IsVisible = false;
                horizontalRibbonButton.IsVisible = true;
            };
            horizontalRibbonButton.Click += (sneder, args) =>
            {
                ribbon.Orientation = Orientation.Horizontal;
                DockPanel.SetDock(ribbon, Dock.Top);
                horizontalRibbonButton.IsVisible = false;
                verticalRibbonButton.IsVisible = true;
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            //this.Find<CheckBox>("LightsOnCheckBox").Click += LightsOnCheckBox_Click;
        }

        private void LightsOnCheckBox_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string includeString = "avares://Avalonia.Themes.Default/Accents/BaseLight.xaml";
            if (!((sender as CheckBox).IsChecked.Value))
                includeString = "avares://Avalonia.Themes.Default/Accents/BaseDark.xaml";

            App.Current.Styles[1] = new StyleInclude(new Uri("resm:Styles?assembly=Avalonia.Ribbon.Sample"))
            {
                Source = new Uri(includeString)
            };
        }
    }
}
