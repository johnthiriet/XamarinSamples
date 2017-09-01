using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace FromCoreToRenderer.Controls
{
    public class MyCustomLabel : Label
    {
        public const string ColorChangedMessageName = "ColorChanged";

        public event EventHandler<Color> ColorChanged; 

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Text))
            {
                var fields = typeof(Color).GetTypeInfo().DeclaredFields;
                FieldInfo colorField = fields.FirstOrDefault(x => string.Equals(x.Name, Text, StringComparison.OrdinalIgnoreCase));
                if (colorField != null)
                {
                    var color = (Color) colorField.GetValue(null);

                    // Here we use a message.
                    MessagingCenter.Send(this, ColorChangedMessageName, color);

                    // The same can be achieved by raising a custom event subscribed into the custom renderer
                    ColorChanged?.Invoke(this, color);
                }
            }
        }
    }
}
