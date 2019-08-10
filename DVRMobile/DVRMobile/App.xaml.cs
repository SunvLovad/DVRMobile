using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DVRMobile.Services;
using DVRMobile.Views;
using LibVLCSharp.Shared;
using DVRMobile.Base;

namespace DVRMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            Base.Setting.Initances.LoadSettings();
            Base.Global.ConnectServerBeforeLoadSetting();
            Core.Initialize();
            Global.libVlc = new LibVLC();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
