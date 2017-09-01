using Xamarin.Forms;
using FromCoreToRenderer.Controls;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyCustomLabel), typeof(FromCoreToRenderer.Droid.Renderers.MyCustomLabelRenderer))]

namespace FromCoreToRenderer.Droid.Renderers
{
    public class MyCustomLabelRenderer : LabelRenderer
    {
        private Android.Widget.TextView _control;

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
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
            var nativeColor = color.ToAndroid();
            _control?.SetTextColor(nativeColor);
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
