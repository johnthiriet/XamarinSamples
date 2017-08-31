using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace FromRendererToCore.Controls
{
    public class MyCustomLabel : Label
    {
        public static readonly BindableProperty TappedCommandProperty =
            BindableProperty.Create("TappedCommand", typeof(ICommand), typeof(MyCustomLabel), null);

        public ICommand TappedCommand
        {
            get => (ICommand)GetValue(TappedCommandProperty);
            set => SetValue(TappedCommandProperty, value);
        }

        public MyCustomLabel()
        {
        }
    }
}
