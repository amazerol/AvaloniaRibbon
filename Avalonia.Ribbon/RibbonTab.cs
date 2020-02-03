using Avalonia.Collections;
using Avalonia.Input;
using Avalonia.Metadata;
using Avalonia.Styling;
using System;
using System.Collections;

namespace Avalonia.Controls.Ribbon
{
    public class RibbonTab : TabItem, IStyleable, IKeyTipHandler
    {
        Type IStyleable.StyleKey => typeof(RibbonTab);

        public static readonly DirectProperty<RibbonTab, IEnumerable> GroupsProperty = AvaloniaProperty.RegisterDirect<RibbonTab, IEnumerable>(nameof(Groups), o => o.Groups, (o, v) => o.Groups = v);

        private IEnumerable _groups = new AvaloniaList<object>();
        [Content]
        public IEnumerable Groups
        {
            get { return _groups; }
            set { SetAndRaise(GroupsProperty, ref _groups, value); }
        }

        public void ActivateKeyTips()
        {
            foreach (RibbonGroupBox g in Groups)
                System.Diagnostics.Debug.WriteLine("GROUP KEYS: " + IRibbonControl.GetKeyTipKeys(g));
            
            Focus();
            KeyDown += RibbonTab_KeyDown;
        }

        private void RibbonTab_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = HandleKeyTip(e.Key);
            KeyDown -= RibbonTab_KeyDown;
        }

        public bool HandleKeyTip(Key key)
        {
            bool retVal = false;
            foreach (RibbonGroupBox g in Groups)
            {
                if (IRibbonControl.HasKeyTipKey(g, key))
                {
                    g.ActivateKeyTips();
                    retVal = true;
                    break;
                }
                else
                {
                    //evaluate Group controls' keys
                }
            }
            return retVal;
        }
    }
}
