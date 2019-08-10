using DVRMobile.CustomRenderers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DVRMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(500);
            searchTextBox.Focus();
        }

        private void SearchTextBox_SearchButtonPressed(object sender, EventArgs e)
        {
            var mySearchBy = sender as MySearchBar;
            if(mySearchBy != null)
            {
                if (mySearchBy.isBackButtonClicked == true)
                    Navigation.PopAsync();
            }
        }

        private string[] colors = new string[] { "Blue", "Green", "Red", "Yellow", "Gray" };
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchKey = searchTextBox.Text.Trim().ToLower();
            if (searchKey == string.Empty)
            {
                suggestionsListView.ItemsSource = new List<string>();
                return;
            }

            List<string> searchData = new List<string>();
            foreach(string color in colors)
            {
                if(color.ToLower().Contains(searchKey))
                    searchData.Add(color);
            }

            suggestionsListView.ItemsSource = searchData;
        }

        private void SuggestionsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string searchKey = e.Item as string;
            MessagingCenter.Send(this, "ReloadLiveView", searchKey);
            Navigation.PopAsync();
        }
    }
}