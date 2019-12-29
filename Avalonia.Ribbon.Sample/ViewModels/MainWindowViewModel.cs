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
            string paramString = "[NO CONTENT]";
            
            if (parameter != null)
            {
                if (parameter is string str)
                    paramString = str;
                else
                    paramString = parameter.ToString();
            }

            Console.WriteLine("OnClickCommand invoked: " + paramString);
        }
    }
}
