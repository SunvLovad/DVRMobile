using DVRMobile.Internationalization;
using DVRMobile.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace DVRMobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem
                {
                    Id = MenuItemType.LiveView,
                    Title = StringResources.Find("LIVE_VIEW"),
                    Icon = ImageSource.FromFile("liveview_icon.png")
                },
                new HomeMenuItem
                {
                    Id = MenuItemType.Setting,
                    Title = StringResources.Find("SETTING"),
                    Icon = ImageSource.FromFile("setting_icon.png")
                },
                new HomeMenuItem
                {
                    Id = MenuItemType.About,
                    Title = StringResources.Find("INFORMATION"),
                    Icon = ImageSource.FromFile("info_icon.png")
                }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}