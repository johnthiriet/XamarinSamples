using System;
using Android.App;
using Android.Gms.Ads;
using Android.Gms.Ads.Initialization;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;

namespace AdMob.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private InterstitialAd _interstitialAd;
        private AdView _adView;

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

        private class MyAdListener : AdListener
        {
            private Action _onAdLoaded;

            public MyAdListener(Action onAdLoaded)
            {
                _onAdLoaded = onAdLoaded;
            }

            public override void OnAdLoaded()
            {
                base.OnAdLoaded();

                _onAdLoaded?.Invoke();
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing)
                {
                    _onAdLoaded = null;
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            MobileAds.Initialize(this, new OnInitializationCompleteListener(OnInitializationComplete));
        }

        private void OnInitializationComplete(IInitializationStatus initializationStatus)
        {
            _adView = new AdView(this);

            _adView.AdSize = AdSize.Banner;
            _adView.AdUnitId = "ca-app-pub-3940256099942544/6300978111";

            var relativeLayout = FindViewById<RelativeLayout>(Resource.Id.content_relative_layout);
            relativeLayout?.AddView(_adView);

            AdRequest adRequest = new AdRequest.Builder().Build();
            _adView.LoadAd(adRequest);

            _interstitialAd = new InterstitialAd(this)
            {
                AdUnitId = "ca-app-pub-3940256099942544/1033173712",
                AdListener = new MyAdListener(OnAdLoaded)
            };

            _interstitialAd.LoadAd(new AdRequest.Builder().Build());
        }

        private void OnAdLoaded()
        {
            if (_interstitialAd.IsLoaded)
            {
                _interstitialAd?.Show();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _adView?.Dispose();
                _adView = null;

                _interstitialAd?.Dispose();
                _interstitialAd = null;
            }
        }
    }
}
