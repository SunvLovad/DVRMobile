using Android.App;
using Android.Views;
using Android.Widget;
using DVRMobile.Services;
using Xamarin.Forms;

namespace DVRMobile.Droid.Services
{
    public class MyDependencyServiceManager : IMyDependencyService
    {
        public void EnterFullScreen()
        {
            Android.Views.Window window = (Forms.Context as Activity).Window;
            window.AddFlags(WindowManagerFlags.Fullscreen);
        }

        public void ExitFullScreen()
        {
            Android.Views.Window window = (Forms.Context as Activity).Window;
            window.ClearFlags(WindowManagerFlags.Fullscreen);
        }

        public void LongShowToast(string msg)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, msg, ToastLength.Long).Show();
        }

        public void ShortShowToast(string msg)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, msg, ToastLength.Short).Show();
        }
    }
}