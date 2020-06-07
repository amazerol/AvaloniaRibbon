using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaUI.Ribbon
{
    public interface IKeyTipHandler
    {
        void ActivateKeyTips(Ribbon ribbon, IKeyTipHandler prev);

        bool HandleKeyTipKeyPress(Key key);
    }
}
