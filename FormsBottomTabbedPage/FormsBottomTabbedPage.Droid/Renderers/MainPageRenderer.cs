﻿using System.Collections.Generic;
using System.Linq;
using Android.Views;
using BottomNavigationBar;
using BottomNavigationBar.Listeners;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(FormsBottomTabbedPage.MainPage), typeof(FormsBottomTabbedPage.Droid.Renderers.MainPageRenderer))]
namespace FormsBottomTabbedPage.Droid.Renderers
{
    public class MainPageRenderer : VisualElementRenderer<MainPage>, IOnTabClickListener
    {
        private BottomBar _bottomBar;

        private Page _currentPage;

        private int _lastSelectedTabIndex = -1;

        public MainPageRenderer()
        {
            // Required to say packager to not to add child pages automatically
            AutoPackage = false;
        }

        public void OnTabSelected(int position)
        {
            LoadPageContent(position);
        }

        public void OnTabReSelected(int position)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<MainPage> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                ClearElement(e.OldElement);
            }

            if (e.NewElement != null)
            {
                InitializeElement(e.NewElement);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearElement(Element);
            }

            base.Dispose(disposing);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (Element == null)
            {
                return;
            }

            int width = r - l;
            int height = b - t;

            _bottomBar.Measure(
                MeasureSpec.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpec.MakeMeasureSpec(height, MeasureSpecMode.AtMost));

            // We need to call measure one more time with measured sizes 
            // in order to layout the bottom bar properly
            _bottomBar.Measure(
                MeasureSpec.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpec.MakeMeasureSpec(_bottomBar.ItemContainer.MeasuredHeight, MeasureSpecMode.Exactly));

            int barHeight = _bottomBar.ItemContainer.MeasuredHeight;

            _bottomBar.Layout(0, b - barHeight, width, b);

            float density = Android.Content.Res.Resources.System.DisplayMetrics.Density;

            double contentWidthConstraint = width / density;
            double contentHeightConstraint = (height - barHeight) / density;

            if (_currentPage != null)
            {
                var renderer = Platform.GetRenderer(_currentPage);

                renderer.Element.Measure(contentWidthConstraint, contentHeightConstraint);
                renderer.Element.Layout(new Rectangle(0, 0, contentWidthConstraint, contentHeightConstraint));

                renderer.UpdateLayout();
            }
        }

        private void InitializeElement(MainPage element)
        {
            PopulateChildren(element);
        }

        private void PopulateChildren(MainPage element)
        {
            // Unfortunately bottom bar can not be reused so we have to
            // remove it and create the new instance
            _bottomBar?.RemoveFromParent();

            _bottomBar = CreateBottomBar(element.Children);
            AddView(_bottomBar);

            LoadPageContent(0);
        }

        private void ClearElement(MainPage element)
        {
            if (_currentPage != null)
            {
                IVisualElementRenderer renderer = Platform.GetRenderer(_currentPage);

                if (renderer != null)
                {
                    renderer.View.RemoveFromParent();
                    renderer.View.Dispose();
                    renderer.Dispose();

                    _currentPage = null;
                }

                if (_bottomBar != null)
                {
                    _bottomBar.RemoveFromParent();
                    _bottomBar.Dispose();
                    _bottomBar = null;
                }
            }
        }

        private BottomBar CreateBottomBar(IEnumerable<Page> pageIntents)
        {
            var bar = new BottomBar(Context);

            // TODO: Configure the bottom bar here according to your needs

            bar.SetOnTabClickListener(this);
            bar.UseFixedMode();

            PopulateBottomBarItems(bar, pageIntents);

            bar.ItemContainer.SetBackgroundColor(Color.WhiteSmoke);

            return bar;
        }

        private void PopulateBottomBarItems(BottomBar bar, IEnumerable<Page> pages)
        {
            var barItems = pages.Select(x => new BottomBarTab(Context.Resources.GetDrawable(x.Icon), x.Title));

            bar.SetItems(barItems.ToArray());
        }

        private void LoadPageContent(int position)
        {
            ShowPage(position);
        }

        private void ShowPage(int position)
        {
            if (position != _lastSelectedTabIndex)
            {
                Element.CurrentPage = Element.Children[position];

                if (Element.CurrentPage != null)
                {
                    LoadPageContent(Element.CurrentPage);
                }
            }

            _lastSelectedTabIndex = position;
        }

        private void LoadPageContent(Page page)
        {
            UnloadCurrentPage();

            _currentPage = page;

            LoadCurrentPage();

            Element.CurrentPage = _currentPage;
        }

        private void LoadCurrentPage()
        {
            var renderer = Platform.GetRenderer(_currentPage);

            if (renderer == null)
            {
                renderer = Platform.CreateRenderer(_currentPage);
                Platform.SetRenderer(_currentPage, renderer);

                AddView(renderer.View);
            }
            else
            {
                // As we show and hide pages manually OnAppearing and OnDisappearing
                // workflow methods won't be called by the framework. Calling them manually...
                var basePage = _currentPage as Page;
                basePage?.SendAppearing();
            }

            renderer.View.Visibility = ViewStates.Visible;
        }

        private void UnloadCurrentPage()
        {
            if (_currentPage != null)
            {
                var basePage = _currentPage as Page;
                basePage?.SendDisappearing();

                var renderer = Platform.GetRenderer(_currentPage);

                if (renderer != null)
                {
                    renderer.View.Visibility = ViewStates.Invisible;
                }
            }
        }
    }
}
