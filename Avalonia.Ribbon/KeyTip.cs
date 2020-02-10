using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
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
                //var tipText = 
                /*tipText.Bind(TextBlock.TextProperty, new Binding()
                {
                    Source = element,
                    Path = 
                });*/

                var tipContent = new Border()
                {
                    Background = new SolidColorBrush(Colors.White),
                    Child = new TextBlock()
                    {
                        [!TextBlock.TextProperty] = element[!KeyTip.KeyTipKeysProperty]
                    }
                };
                System.Diagnostics.Debug.WriteLine("TEXT: " + (tipContent.Child as TextBlock).Text);
                tipContent.Classes.Add("KeyTip");

                var tip = new Popup()
                {
                    PlacementTarget = element,
                    PlacementMode = PlacementMode.Right,
                    Width = 25,
                    Height = 20,
                    Child = tipContent
                };
                tip[!Popup.VerticalOffsetProperty] = element.GetObservable(Control.BoundsProperty).Select(x => x.Height - 20).ToBinding();
                tip[!Popup.HorizontalOffsetProperty] = tip.GetObservable(Popup.WidthProperty).Select(x => x * -1).ToBinding();
                    /*new Binding("Bounds.Height")
                {
                    Source = element
                }*/
                _keyTips.Add(element, tip);
                return _keyTips[element];
            }
        }
    }
}
