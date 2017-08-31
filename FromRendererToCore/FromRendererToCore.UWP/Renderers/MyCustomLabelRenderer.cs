using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using FromRendererToCore.Controls;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MyCustomLabel), typeof(FromRendererToCore.UWP.Renderers.MyCustomLabelRenderer))]

namespace FromRendererToCore.UWP.Renderers
{
    public class MyCustomLabelRenderer : LabelRenderer
    {
        private TextBlock _control;

        private static readonly Random Random = new Random();

        private void OnLabelTapped(object sender, TappedRoutedEventArgs e)
        {
            var label = (MyCustomLabel)Element;
            label?.TappedCommand?.Execute("RandomString ! " + Random.Next());
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if (_control == null)
                {
                    _control = Control;
                    _control.Tapped += OnLabelTapped;
                }

                Control.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else
            {
                if (_control != null)
                {
                    _control.Tapped -= OnLabelTapped;
                    _control = null;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _control.Tapped -= OnLabelTapped;
                _control = null;
            }

            base.Dispose(disposing);
        }
    }
}
