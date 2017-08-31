using System;
using FromRendererToCore.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyCustomLabel), typeof(FromRendererToCore.Droid.Renderers.MyCustomLabelRenderer))]

namespace FromRendererToCore.Droid.Renderers
{
    public class MyCustomLabelRenderer : LabelRenderer
    {
        private Android.Widget.TextView _control;
        private static readonly Random Random = new Random();

        private void OnLabelTapped(object sender, EventArgs args)
        {
            var label = (MyCustomLabel)Element;
            label?.TappedCommand?.Execute("RandomString ! " + Random.Next());
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if (_control == null)
                {
                    _control = Control;
                    _control.Clickable = true;
                    _control.Click += OnLabelTapped;
                }

                Control.SetTextColor(Android.Graphics.Color.Red);
            }
            else
            {
                if (_control != null)
                {
                    _control.Click -= OnLabelTapped;
                    _control = null;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_control != null)
                    _control.Click -= OnLabelTapped;
                _control = null;
            }

            base.Dispose(disposing);
        }
    }
}
