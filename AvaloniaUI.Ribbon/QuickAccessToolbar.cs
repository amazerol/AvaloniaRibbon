using Avalonia.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Linq;
using System.Collections;
using Avalonia.Interactivity;
using Avalonia.Controls.Platform;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls.Presenters;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Avalonia.Controls.Generators;
using Avalonia.Data.Converters;
using System.Globalization;
using System.Collections.Specialized;

namespace AvaloniaUI.Ribbon
{
    public class QuickAccessToolbar : ItemsControl, IStyleable//, IKeyTipHandler
    {
        

        static QuickAccessToolbar()
        {
            /*RibbonProperty.Changed.AddClassHandler<QuickAccessToolbar>((sender, e) => {
                
            });*/
        }

        /*public void TestItems()
        {
            if (Ribbon != null)
            {
                foreach (object obj in ((Ribbon.Tabs[0] as RibbonTab).Groups[0]).Items.OfType<ICanAddToQuickAccess>().Where(x => x.CanAddToQuickAccess))
                    ((AvaloniaList<object>)Items).Add(obj);
            }
        }*/

        Type IStyleable.StyleKey => typeof(QuickAccessToolbar);

        /*Panel panel = null;
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            panel = e.NameScope.Find<Panel>("PART_ControlsPanel");

            RefreshItems();
        }

        protected override void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.ItemsChanged(e);
            RefreshItems();
        }

        protected override void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.ItemsCollectionChanged(sender, e);
            RefreshItems();
        }

        void RefreshItems()
        {
            panel.Children.Clear();
            
            foreach (Control itm in ((AvaloniaList<object>)Items).OfType<Control>())
                panel.Children.Add(itm);
        }*/

        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new ItemContainerGenerator<QuickAccessItem>(this, QuickAccessItem.ItemProperty, QuickAccessItem.ContentTemplateProperty);
        }

        public bool ContainsItem(ICanAddToQuickAccess item) => Items.OfType<ICanAddToQuickAccess>().Contains(item);

        public bool AddItem(ICanAddToQuickAccess item)
        {
            if ((item == null) || ContainsItem(item))
                return false;
            else if (item.CanAddToQuickAccess)
            {
                Items = Items.OfType<ICanAddToQuickAccess>().Append(item);
                return true;
            }
            else
                return false;
        }

        public bool RemoveItem(ICanAddToQuickAccess item)
        {
            if ((item == null) || (!ContainsItem(item)))
                return false;
            else
            {
                var items = Items.OfType<ICanAddToQuickAccess>().ToList();
                items.Remove(item);
                Items = items;
                return true;
            }
        }
    }

    
    public class QuickAccessItem : ContentControl, IStyleable
    {
        public static readonly StyledProperty<ICanAddToQuickAccess> ItemProperty = AvaloniaProperty.Register<QuickAccessItem, ICanAddToQuickAccess>(nameof(Item));
        public ICanAddToQuickAccess Item
        {
            get => GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        Type IStyleable.StyleKey => typeof(QuickAccessItem);

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            e.NameScope.Find<MenuItem>("PART_RemoveFromQuickAccessToolbar").Click += (sneder, args) => Avalonia.VisualTree.VisualExtensions.FindAncestorOfType<QuickAccessToolbar>(this)?.RemoveItem(Item);
        }
    }


    public class ItemsToControlsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Controls retVal = new Controls();
            foreach (Control itm in ((AvaloniaList<object>)value).OfType<Control>())
                retVal.Add(itm);
            
            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}