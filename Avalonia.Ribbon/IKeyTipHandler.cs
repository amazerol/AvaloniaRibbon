using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Controls.Ribbon
{
    public interface IKeyTipHandler
    {
        void ActivateKeyTips();

        bool HandleKeyTip(Key key);
    }
}
