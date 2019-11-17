using System;

namespace Avalonia.Ribbon.Samples.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public void OnClickCommand()
        {
            Console.WriteLine("Je viens de cliquer sur le bouton test 7");
        }
    }
}
