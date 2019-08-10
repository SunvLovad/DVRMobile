using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DVRMobile.Models;

namespace DVRMobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.LiveView, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id, bool isRemoveLiveView = false)
        {
            if (isRemoveLiveView || id != (int)MenuItemType.LiveView)
            {
                if (MenuPages.ContainsKey((int)MenuItemType.LiveView))
                {
                    ((LiveViewPage)MenuPages[(int)MenuItemType.LiveView].RootPage).OnDestroy();
                    MenuPages[(int)MenuItemType.LiveView] = null;
                    MenuPages.Remove((int)MenuItemType.LiveView);
                }
            }

            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.LiveView:
                        MenuPages.Add(id, new NavigationPage(new LiveViewPage()));
                        break;
                    case (int)MenuItemType.Setting:
                        MenuPages.Add(id, new NavigationPage(new SettingPage()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}