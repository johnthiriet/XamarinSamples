using Xamarin.Forms;

namespace Admob.Forms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Device.RuntimePlatform == Device.iOS)
            {
                adControl.AdUnitId = "ca-app-pub-3940256099942544/2934735716";
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                adControl.AdUnitId = "ca-app-pub-3940256099942544/6300978111";
            }
        }
    }
}
