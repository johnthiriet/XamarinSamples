using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Gms.Ads;
using Android.Gms.Ads.Initialization;

namespace Admob.Forms.Droid
{
    [Activity(Label = "Admob.Forms", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private class OnInitializationCompleteListener : Java.Lang.Object, IOnInitializationCompleteListener
        {
            private Action<IInitializationStatus> _onInitializationCompleted;

            public OnInitializationCompleteListener(Action<IInitializationStatus> onInitializationCompleted)
            {
                _onInitializationCompleted = onInitializationCompleted;
            }

            public void OnInitializationComplete(IInitializationStatus initializationStatus)
            {
                _onInitializationCompleted?.Invoke(initializationStatus);
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing)
                {
                    _onInitializationCompleted = null;
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            MobileAds.Initialize(this, new OnInitializationCompleteListener(OnInitializationComplete));

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        private void OnInitializationComplete(IInitializationStatus initializationStatus)
        {
    
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}