using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Controls.Ribbon
{
    public class KeyTip
    {
        public static readonly AttachedProperty<string> KeyTipKeysProperty = AvaloniaProperty.RegisterAttached<KeyTip, Control, string>("KeyTipKeys");
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


        public static readonly AttachedProperty<bool> ShowKeyTipKeysProperty = AvaloniaProperty.RegisterAttached<KeyTip, Control, bool>("ShowKeyTipKeys");
        public static bool GetShowKeyTipKeys(Control element) => element.GetValue(ShowKeyTipKeysProperty);
        public static void SetShowKeyTipKeys(Control element, bool value) => element.SetValue(ShowKeyTipKeysProperty, value);

        static KeyTip()
        {       
                /*.AddClassHandler<Ribbon>(new Action<Ribbon, AvaloniaPropertyChangedEventArgs>((sender, args) =>
            {
            
            }));*/
        }

        private KeyTip() { }


        static Dictionary<Control, Popup> _keyTips = new Dictionary<Control, Popup>();
        public static Popup GetKeyTip(Control element)
        {
            if (_keyTips.TryGetValue(element, out Popup val))
                return val;
            else
            {
                var tip = new Popup()
                {
                    PlacementTarget = element,
                    PlacementMode = PlacementMode.Bottom,
                    Width = 25,
                    Height = 20,
                    VerticalOffset = -20,
                    Child = new TextBlock()
                    {
                        Text = KeyTip.GetKeyTipKeys(element),
                        Background = new SolidColorBrush(Colors.White),
                        Foreground = new SolidColorBrush(Colors.Black),
                        Width = 25,
                        Height = 20
                    }
                };
                _keyTips.Add(element, tip);
                return _keyTips[element];
            }
        }
    }
}
