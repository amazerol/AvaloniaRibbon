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

        static RibbonTab()
        {
            KeyTip.ShowKeyTipKeysProperty.Changed.AddClassHandler<RibbonTab>(new Action<RibbonTab, AvaloniaPropertyChangedEventArgs>((sender, args) =>
            {
                if ((bool)args.NewValue)
                {
                    foreach (RibbonGroupBox g in sender.Groups)
                    {
                        if ((g.Command != null) && KeyTip.HasKeyTipKeys(g))
                            KeyTip.GetKeyTip(g).IsOpen = true;

                        foreach (Control c in g.Items)
                        {
                            if (KeyTip.HasKeyTipKeys(c))
                                KeyTip.GetKeyTip(c).IsOpen = true;
                        }
                    }
                }
                else
                {
                    foreach (RibbonGroupBox g in sender.Groups)
                    {
                        KeyTip.GetKeyTip(g).IsOpen = false;

                        foreach (Control c in g.Items)
                            KeyTip.GetKeyTip(c).IsOpen = false;
                    }
                }
            }));
        }

        public void ActivateKeyTips()
        {
            foreach (RibbonGroupBox g in Groups)
                System.Diagnostics.Debug.WriteLine("GROUP KEYS: " + KeyTip.GetKeyTipKeys(g));
            
            Focus();
            KeyTip.SetShowKeyTipKeys(this, true);
            KeyDown += RibbonTab_KeyDown;
        }

        private void RibbonTab_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = HandleKeyTip(e.Key);
            KeyDown -= RibbonTab_KeyDown;
            KeyTip.SetShowKeyTipKeys(this, false);
        }

        public bool HandleKeyTip(Key key)
        {
            bool retVal = false;
            foreach (RibbonGroupBox g in Groups)
            {
                if (KeyTip.HasKeyTipKey(g, key))
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
                        if (KeyTip.HasKeyTipKey(c, key))
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

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            var inputRoot = e.Root as IInputRoot;
            if ((inputRoot != null) && (inputRoot is WindowBase wnd))
                wnd.Deactivated += InputRoot_Deactivated;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            var inputRoot = e.Root as IInputRoot;
            if ((inputRoot != null) && (inputRoot is WindowBase wnd))
                wnd.Deactivated -= InputRoot_Deactivated;
        }

        private void InputRoot_Deactivated(object sender, EventArgs e)
        {
            KeyTip.SetShowKeyTipKeys(this, false);
            (Parent as Ribbon).Close();
        }
    }
}
