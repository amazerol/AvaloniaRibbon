using Avalonia;
using Avalonia.Collections;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using Avalonia.Styling;
using System;
using System.Linq;
using System.Collections;
using System.Diagnostics;
using Avalonia.Controls.Primitives;
using System.Collections.Specialized;

namespace AvaloniaUI.Ribbon
{
    public class RibbonContextualTabGroup : HeaderedItemsControl, IStyleable
    {
        static RibbonContextualTabGroup()
        {
            IsVisibleProperty.Changed.AddClassHandler<RibbonContextualTabGroup>((sender, e) =>
            {
                if ((e.NewValue is bool visible) && (!visible))
                    sender.SwitchToNextVisibleTab();
            });
        }

        void SwitchToNextVisibleTab()
        {
            Ribbon rbn = IRibbonControl.GetParentRibbon(this);
            if ((rbn != null) && ((IAvaloniaList<object>)Items).Contains(rbn.SelectedItem))
            {
                int selIndex = rbn.SelectedIndex;
                
                rbn.CycleTabs(false);
                
                if (selIndex == rbn.SelectedIndex)
                    rbn.CycleTabs(true);
            }
                    /*var selectableItems = ((IAvaloniaList<object>)rbn.Items).OfType<RibbonTab>().Where(x => x.IsVisible && x.IsEnabled);
                    RibbonTab targetTab = null;
                    foreach (RibbonTab tab in selectableItems)
                    {
                        if (((IAvaloniaList<object>)Items).Contains(tab))
                            break;
                        
                        targetTab = tab;
                    }

                    if (targetTab == null)
                    {
                        selectableItems = selectableItems.Reverse();

                        foreach (RibbonTab tab in selectableItems)
                        {
                            if (((IAvaloniaList<object>)Items).Contains(tab))
                                break;
                            
                            targetTab = tab;
                        }
                    }
                    int index = ((IAvaloniaList<object>)rbn.Items).IndexOf(targetTab);
                    rbn.SelectedIndex = index;
                    //if (index > 0)
                    */
        }

        protected override void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.ItemsCollectionChanged(sender, e);
            if (e.OldItems != null)
            {
                foreach (RibbonTab tab in e.OldItems.OfType<RibbonTab>())
                    tab.IsContextual = false;
            }

            if (e.NewItems != null)
            {
                foreach (RibbonTab tab in e.NewItems.OfType<RibbonTab>())
                    tab.IsContextual = true;
            }
        }

        Type IStyleable.StyleKey => typeof(RibbonContextualTabGroup);
    }
}