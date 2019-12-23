using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace Avalonia.Ribbon.Samples.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public void OnClickCommand(object parameter)
        {
            string paramString = parameter.ToString();
            if (parameter is string str)
                paramString = str;
            Console.WriteLine("OnClickCommand invoked: " + paramString);
        }
    }
}
