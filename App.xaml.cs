using Avalonia;
using Avalonia.Markup.Xaml;

namespace AvaloniaRibbon
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
