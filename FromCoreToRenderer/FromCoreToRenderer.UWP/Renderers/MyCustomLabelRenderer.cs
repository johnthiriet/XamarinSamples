using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using FromCoreToRenderer.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MyCustomLabel), typeof(FromCoreToRenderer.UWP.Renderers.MyCustomLabelRenderer))]

namespace FromCoreToRenderer.UWP.Renderers
{
    public class MyCustomLabelRenderer : LabelRenderer
    {
        private TextBlock _control;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if (_control == null)

                 SubscribeToColorChanged();
                _control = Control;
            }
            else
            {
                if (_control != null)
                {
                    UnsubscribeFromColorChanged();
                    _control = null;
                }
            }
        }

        private void SubscribeToColorChanged()
        {
            // With event
            ((MyCustomLabel)Element).ColorChanged += OnColorChanged;

            // With message
            MessagingCenter.Subscribe<MyCustomLabel, Color>(this, MyCustomLabel.ColorChangedMessageName, OnColorChangedMessage);
        }

        private void UnsubscribeFromColorChanged()
        {
            // With event
            ((MyCustomLabel)Element).ColorChanged -= OnColorChanged;

            // With message
            MessagingCenter.Unsubscribe<MyCustomLabel, Color>(this, MyCustomLabel.ColorChangedMessageName);
        }

        private void OnColorChanged(object sender, Color args)
        {
            ChangeColor(args);
        }

        private void OnColorChangedMessage(MyCustomLabel sender, Color color)
        {
            ChangeColor(color);
        }

        private void ChangeColor(Color color)
        {
            var nativeColor =
                Windows.UI.ColorHelper.FromArgb(
                    (byte) (color.A * 0xff),
                    (byte) (color.R * 0xff),
                    (byte) (color.G * 0xff),
                    (byte) (color.B * 0xff));
            if (_control != null)
                _control.Foreground = new SolidColorBrush(nativeColor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnsubscribeFromColorChanged();
                _control = null;
            }

            base.Dispose(disposing);
        }
    }
}