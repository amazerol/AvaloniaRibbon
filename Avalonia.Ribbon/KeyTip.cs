using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                var tipContent = new ContentControl()
                {
                    Background = new SolidColorBrush(Colors.White),
                    /*Content = new TextBlock()
                    {
                        [!TextBlock.TextProperty] = element[!KeyTip.KeyTipKeysProperty],
                        Foreground = new SolidColorBrush(Colors.Black),
                        HorizontalAlignment = Layout.HorizontalAlignment.Center,
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = Layout.VerticalAlignment.Center
                    }*/
                    MinWidth = 20,
                    //MinHeight = 20,
                    [!ContentControl.ContentProperty] = element[!KeyTip.KeyTipKeysProperty]
                };
                if (tipContent.Content != null)
                    Debug.WriteLine("TEXT: " + tipContent.Content.ToString()/*(tipContent.Child as TextBlock).Text*/);
                //tipContent.Arrange(new Rect(0, 0, double.PositiveInfinity, double.PositiveInfinity));
                
                var tip = new Popup()
                {
                    PlacementTarget = element,
                    PlacementMode = PlacementMode.Right,
                    [!Popup.WidthProperty] = tipContent.GetObservable(Control.BoundsProperty).Select(x => x.Width).ToBinding(),
                    [!Popup.HeightProperty] = tipContent.GetObservable(Control.BoundsProperty).Select(x => x.Height).ToBinding(), //tipContent[!Control.HeightProperty],
                    VerticalAlignment = Layout.VerticalAlignment.Bottom,
                    Child = tipContent
                };
                tip.Classes.Add("KeyTip");
                ////tipContent.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                //tip[!Popup.VerticalOffsetProperty] = element.GetObservable(Control.BoundsProperty).Select(x => x.Height - tipContent.Bounds.Height).ToBinding();
                tip[!Popup.VerticalOffsetProperty] = element.GetObservable(Control.BoundsProperty).Select(x => x.Height).CombineLatest  (tipContent.GetObservable(Control.BoundsProperty), (x, y) => x - y.Height).ToBinding();
                //.CombineLatest(tipContent.GetObservable(Control.BoundsProperty), x => x).ToBinding(); //.CombineLatest().Select(y => y.
                //.CombineLatest<double>(tipContent.GetObservable(Control.BoundsProperty), (x => x.Height) ).ToBinding();
                //tip[!Popup.HorizontalOffsetProperty] = tip.GetObservable(Popup.WidthProperty).Select(x => x * -1).ToBinding();
                //tip[!Popup.HorizontalOffsetProperty] = tipContent.GetObservable(Control.WidthProperty).Select(x => x * -1).ToBinding();
                tip[!Popup.HorizontalOffsetProperty] = tipContent.GetObservable(Control.BoundsProperty).Select(x => x.Width * -1).ToBinding();
                ((ISetLogicalParent)tip).SetParent(element);
                ////tip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                tip.IsOpen = true;
                tip.IsOpen = false;
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
