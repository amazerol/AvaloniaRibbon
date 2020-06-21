using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;

namespace AvaloniaUI.Ribbon
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


        public static readonly AttachedProperty<bool> ShowChildKeyTipKeysProperty = AvaloniaProperty.RegisterAttached<KeyTip, Control, bool>("ShowChildKeyTipKeys");
        public static bool GetShowChildKeyTipKeys(Control element) => element.GetValue(ShowChildKeyTipKeysProperty);
        public static void SetShowChildKeyTipKeys(Control element, bool value) => element.SetValue(ShowChildKeyTipKeysProperty, value);

        private KeyTip() { }


        static Dictionary<Control, Popup> _keyTips = new Dictionary<Control, Popup>();
        public static Popup GetKeyTip(Control element)
        {
            if (_keyTips.TryGetValue(element, out Popup val))
                return val;
            else
            {

                var tipContent = new ContentControl()
                {
                    //Background = new SolidColorBrush(Colors.White),
                    [!ContentControl.ContentProperty] = element[!KeyTip.KeyTipKeysProperty]
                };
                tipContent.Classes.Add("KeyTipContent");
                if (tipContent.Content != null)
                    Debug.WriteLine("TEXT: " + tipContent.Content.ToString());
                
                var tip = new Popup()
                {
                    PlacementTarget = element,
                    PlacementMode = PlacementMode.Right,
                    [!Popup.WidthProperty] = tipContent.GetObservable(Control.BoundsProperty).Select(x => x.Width).ToBinding(),
                    [!Popup.HeightProperty] = tipContent.GetObservable(Control.BoundsProperty).Select(x => x.Height).ToBinding(), //tipContent[!Control.HeightProperty],
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Child = tipContent
                };
                tip.Classes.Add("KeyTip");

                tipContent.InvalidateArrange();
                tipContent.InvalidateMeasure();
                tipContent.InvalidateVisual();
                
                tip.InvalidateArrange();
                tip.InvalidateMeasure();
                tip.InvalidateVisual();

                tip[!Popup.HorizontalOffsetProperty] = tipContent.GetObservable(Control.BoundsProperty).Select(x => x.Width * -1).ToBinding();
                tip[!Popup.VerticalOffsetProperty] = element.GetObservable(Control.BoundsProperty).Select(x => x.Height).CombineLatest(tipContent.GetObservable(Control.BoundsProperty), (x, y) => x - y.Height).ToBinding();

                ((ISetLogicalParent)tip).SetParent(element);

                tip.Opened += KeyTip_Opened;
                
                _keyTips.Add(element, tip);
                return _keyTips[element];
            }
        }

        private static void KeyTip_Opened(object sender, EventArgs e)
        {
            var sned = sender as Popup;
            sned.Host?.ConfigurePosition(sned.PlacementTarget, sned.PlacementMode, new Point(sned.HorizontalOffset, sned.VerticalOffset));
        }
    }
}
