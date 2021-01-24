using System.ComponentModel;
using Admob.Forms.Controls;
using Google.MobileAds;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AdControlView), typeof(Admob.Forms.iOS.Renderers.AdControlViewRenderer))]
namespace Admob.Forms.iOS.Renderers
{
    public class AdControlViewRenderer : ViewRenderer<AdControlView, BannerView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<AdControlView> e)
        {
            base.OnElementChanged(e);

			if (Control == null)
			{
				SetNativeControl(CreateBannerView());
			}
		}

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == nameof(BannerView.AdUnitId))
			{
				if (string.IsNullOrWhiteSpace(Control.AdUnitId) && !string.IsNullOrWhiteSpace(Element.AdUnitId))
				{
					Control.AdUnitId = Element.AdUnitId;
					Control.LoadRequest(GetRequest());
				}
			}
		}

		private Request GetRequest()
		{
			var request = Request.GetDefaultRequest();
			return request;
		}

		private BannerView CreateBannerView()
		{
			var bannerView = new BannerView(AdSizeCons.SmartBannerPortrait)
			{
				RootViewController = GetVisibleViewController()
			};

			if (!string.IsNullOrWhiteSpace(Element.AdUnitId))
				bannerView.AdUnitId = Element.AdUnitId;
			
			if (!string.IsNullOrWhiteSpace(bannerView.AdUnitId))
				bannerView.LoadRequest(GetRequest());

			return bannerView;
		}

		private UIViewController GetVisibleViewController()
		{
			var windows = UIApplication.SharedApplication.Windows;
			foreach (var window in windows)
			{
				if (window.RootViewController != null)
				{
					return window.RootViewController;
				}
			}
			return null;
		}
	}
}
