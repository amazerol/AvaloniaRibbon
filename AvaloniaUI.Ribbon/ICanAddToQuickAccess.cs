using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    public interface ICanAddToQuickAccess : IAvaloniaObject
    {
        IControlTemplate QuickAccessTemplate
        {
            get;
            set;
        }

        bool CanAddToQuickAccess
        {
            get;
            set;
        }
    }
}