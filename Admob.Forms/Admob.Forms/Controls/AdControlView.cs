using System;
using Xamarin.Forms;

namespace Admob.Forms.Controls
{
    public class AdControlView : View
    {
        public AdControlView()
        {
        }

        public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create(
               nameof(AdUnitId),
               typeof(string),
               typeof(AdControlView),
               string.Empty);

        public string AdUnitId
        {
            get => (string)GetValue(AdUnitIdProperty);
            set => SetValue(AdUnitIdProperty, value);
        }
    }
}

