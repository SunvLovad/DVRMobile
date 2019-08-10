using System;
using Android.Content;
using Android.Widget;
using DVRMobile.CustomRenderers;
using DVRMobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MySearchBar), typeof(MySearchBarAndroid))]
namespace DVRMobile.Droid.CustomRenderers
{
    [Obsolete]
    public class MySearchBarAndroid : SearchBarRenderer
    {
        private MySearchBar mySearchBar = null;
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                mySearchBar = e.NewElement as MySearchBar;
                var searchView = Control;
                searchView.Iconified = true;
                searchView.SetIconifiedByDefault(false);
                // (Resource.Id.search_mag_icon); is wrong / Xammie bug
                int searchIconId = Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                var icon = searchView.FindViewById(searchIconId);
                (icon as ImageView).SetImageResource(Resource.Drawable.abc_ic_ab_back_material);
                (icon as ImageView).SetColorFilter(Android.Graphics.Color.White);
                icon.Click += Icon_Click;
            }
        }

        private void Icon_Click(object sender, EventArgs e)
        {
            if(mySearchBar != null)
            {
                mySearchBar.isBackButtonClicked = true;
                mySearchBar.OnSearchButtonPressed();
            }
        }
    }
}