using Avalonia.Collections;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using Avalonia.Styling;
using System;
using System.Collections;
using System.Diagnostics;

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
            IRibbonControl.SetShowKeyTipKeys(this, true);
            KeyDown += RibbonTab_KeyDown;
        }

        private void RibbonTab_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = HandleKeyTip(e.Key);
            KeyDown -= RibbonTab_KeyDown;
            IRibbonControl.SetShowKeyTipKeys(this, false);
        }

        public bool HandleKeyTip(Key key)
        {
            bool retVal = false;
            foreach (RibbonGroupBox g in Groups)
            {
                if (IRibbonControl.HasKeyTipKey(g, key))
                {
                    g.Command?.Execute(g.CommandParameter);
                    (Parent as Ribbon).Close();
                    retVal = true;
                    break;
                }
                else
                {
                    foreach (Control c in g.Items)
                    {
                        if (IRibbonControl.HasKeyTipKey(c, key))
                        {
                            if (c is IKeyTipHandler hdlr)
                            {
                                hdlr.ActivateKeyTips();
                                Debug.WriteLine("Group handled " + key.ToString() + " for IKeyTipHandler");
                            }
                            else
                            {
                                if ((c is Button btn) && (btn.Command != null))
                                    btn.Command.Execute(btn.CommandParameter);
                                else
                                    c.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                                (Parent as Ribbon).Close();
                            }
                            retVal = true;
                            break;
                        }
                    }
                    if (retVal)
                        break;
                }
            }
            return retVal;
        }
    }
}
