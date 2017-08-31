using System;
using FromRendererToCore.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyCustomLabel), typeof(FromRendererToCore.iOS.Renderers.MyCustomLabelRenderer))]

namespace FromRendererToCore.iOS.Renderers
{
    public class MyCustomLabelRenderer : LabelRenderer
    {
        private UILabel _control;
        private UITapGestureRecognizer _tapGestureRecognizer;
        private static readonly Random Random = new Random();

        public MyCustomLabelRenderer()
        {
            _tapGestureRecognizer = new UITapGestureRecognizer(OnLabelTapped);
        }

        private void OnLabelTapped()
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
                    _control.UserInteractionEnabled = true;
                    _control.AddGestureRecognizer(_tapGestureRecognizer);
                }

                Control.TextColor = UIColor.Red;
            }
            else
            {
                if (_control != null)
                {
                    _control.RemoveGestureRecognizer(_tapGestureRecognizer);
                    _control = null;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _control?.RemoveGestureRecognizer(_tapGestureRecognizer);
                _tapGestureRecognizer?.Dispose();
                _tapGestureRecognizer = null;
                _control = null;
            }

            base.Dispose(disposing);
        }
    }
}
