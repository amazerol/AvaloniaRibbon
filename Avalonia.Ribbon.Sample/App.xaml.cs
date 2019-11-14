using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Ribbon.Samples.Views;

namespace Avalonia.Ribbon.Samples
{
    public class App : Application
    {
        internal static IClassicDesktopStyleApplicationLifetime Lifetime => Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime)
            {
                Lifetime.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
