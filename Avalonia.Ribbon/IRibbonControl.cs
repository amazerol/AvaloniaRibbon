using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Controls.Ribbon
{
    public interface IRibbonControl : IAvaloniaObject
    {
        RibbonControlSize Size
        {
            get;
            set;
        }

        RibbonControlSize MinSize
        {
            get;
            set;
        }

        RibbonControlSize MaxSize
        {
            get;
            set;
        }

        /*bool CanAddToQuickAccessToolbar
        {
            get;
            set;
        }*/

        public static readonly AttachedProperty<string> KeyTipKeysProperty = AvaloniaProperty.RegisterAttached<IRibbonControl, Control, string>("KeyTipKeys");
        public static string GetKeyTipKeys(Control element) => element.GetValue(KeyTipKeysProperty);
        public static void SetKeyTipKeys(Control element, string value) => element.SetValue(KeyTipKeysProperty, value);
        public static bool HasKeyTipKey(Control element, Key key)
        {
            string keys = GetKeyTipKeys(element);
            return HasKeyTipKeys(element) && keys.Contains(key.ToString(), StringComparison.OrdinalIgnoreCase);
        }
        public static bool HasKeyTipKeys(Control element)
        {
            string keys = GetKeyTipKeys(element);
            return (!string.IsNullOrEmpty(keys)) && (!string.IsNullOrWhiteSpace(keys));
        }


        public static readonly AttachedProperty<bool> ShowKeyTipKeysProperty = AvaloniaProperty.RegisterAttached<IRibbonControl, Control, bool>("ShowKeyTipKeys");
        public static bool GetShowKeyTipKeys(Control element) => element.GetValue(ShowKeyTipKeysProperty);
        public static void SetShowKeyTipKeys(Control element, bool value) => element.SetValue(ShowKeyTipKeysProperty, value);

        //public static readonly RoutedEvent<RoutedEventArgs> KeyTipControlActivatedEvent = RoutedEvent.Register<IRibbonControl, RoutedEventArgs>("KeyTipControlActivated", RoutingStrategies.Bubble);
    }

    public enum RibbonControlSize
    {
        Small,
        Medium,
        Large
    }
}
