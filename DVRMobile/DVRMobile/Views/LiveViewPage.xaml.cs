using System;
using System.ComponentModel;
using Xamarin.Forms;

using DVRMobile.ViewModels;
using DVRMobile.Internationalization;
using DVRMobile.Models;
using DVRMobile.Base;

namespace DVRMobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class LiveViewPage : ContentPage
    {
        private LiveViewModel viewModel = null;
        public LiveViewPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new LiveViewModel();
        }

        //on page close
        public void OnDestroy()
        {
            Global.OnChangeDivision = null;
            Global.curChannelActive = -1;
            viewModel.OnDestroy();
        }

        //open server page
        async void ServerPage_Clicked(object sender, EventArgs e)
        {
            var serverPage = new NavigationPage(new ServerPage());
            serverPage.Title = StringResources.Find("SERVER_LIST");
            await Navigation.PushAsync(serverPage);
        }

        async void Search_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }

        //on size change
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            viewModel.CaculateControlSize(width, height);
        }
    }
}