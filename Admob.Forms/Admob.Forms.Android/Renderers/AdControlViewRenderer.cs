using System.ComponentModel;
using Admob.Forms.Controls;
using Android.Content;
using Android.Gms.Ads;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdControlView), typeof(Admob.Forms.Droid.Renderers.AdControlViewRenderer))]
namespace Admob.Forms.Droid.Renderers
{
    public class AdControlViewRenderer : ViewRenderer<AdControlView, AdView>
    {
		public AdControlViewRenderer(Context context) : base(context) { }

		private int GetSmartBannerDpHeight()
		{
			var dpHeight = Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density;

			if (dpHeight <= 400) return 32;
			if (dpHeight > 400 && dpHeight <= 720) return 50;
			return 90;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<AdControlView> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null && Control == null)
			{
				var adView = CreateAdView();
				e.NewElement.HeightRequest = GetSmartBannerDpHeight();
				SetNativeControl(adView);
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == nameof(AdView.AdUnitId))
			{
				if (string.IsNullOrWhiteSpace(Control.AdUnitId) && !string.IsNullOrWhiteSpace(Element.AdUnitId))
				{
					Control.AdUnitId = Element.AdUnitId;
					Control.LoadAd(new AdRequest.Builder().Build());
				}
			}
		}

		private AdView CreateAdView()
		{
			var adView = new AdView(Context)
			{
				AdSize = AdSize.SmartBanner,
			};

			if (!string.IsNullOrWhiteSpace(Element.AdUnitId))
				adView.AdUnitId = Element.AdUnitId;

			adView.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
			

			if (adView.AdUnitId != null)
				adView.LoadAd(new AdRequest.Builder().Build());

			return adView;
		}
	}
}
