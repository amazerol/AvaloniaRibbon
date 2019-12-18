using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Ribbon;
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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
