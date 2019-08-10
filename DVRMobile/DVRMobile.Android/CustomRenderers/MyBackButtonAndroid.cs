using System;
using Android.Content;
using Android.Widget;
using DVRMobile.CustomRenderers;
using DVRMobile.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyBackButton), typeof(MyBackButtonAndroid))]
namespace DVRMobile.Droid.CustomRenderers
{
    [Obsolete]
    public class MyBackButtonAndroid : ImageRenderer
    {
        private MyBackButton myBackButton = null;
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                myBackButton = e.NewElement as MyBackButton;
                Control.SetImageResource(Resource.Drawable.abc_ic_ab_back_material);
                Control.SetColorFilter(Android.Graphics.Color.White);
                Control.Click += Control_Click;
            }
        }

        private void Control_Click(object sender, EventArgs e)
        {
            if (myBackButton != null && myBackButton.OnClick != null)
                myBackButton.OnClick();
        }
    }
}