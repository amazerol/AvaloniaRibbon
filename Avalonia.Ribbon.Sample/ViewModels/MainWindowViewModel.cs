using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace Avalonia.Ribbon.Samples.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
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
            LastActionText = paramString;
        }

        string _lastActionText = "none";

        public event PropertyChangedEventHandler PropertyChanged;

        public string LastActionText
        {
            get => _lastActionText;
            set
            {
                _lastActionText = value;
                NotifyPropertyChanged();
            }
        }

        public void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
