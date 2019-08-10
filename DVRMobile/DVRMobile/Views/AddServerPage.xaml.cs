using DVRMobile.Internationalization;
using DVRMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DVRMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddServerPage : ContentPage
    {
        private Server newServer = null;
        public AddServerPage()
        {
            InitializeComponent();
            backButton.OnClick = () =>
            {
                Navigation.PopModalAsync();
            };
            BindingContext = newServer = new Server();
            Base.Setting.Initances.isChangeServerList = false;
            titleLabel.Text = StringResources.Find("ADD_SERVER");
        }

        public AddServerPage(Server server)
        {
            InitializeComponent();
            backButton.OnClick = () =>
            {
                Navigation.PopModalAsync();
            };
            BindingContext = newServer = server.Clone();
            Base.Setting.Initances.isChangeServerList = false;
            titleLabel.Text = StringResources.Find("EDIT_SERVER");
        }

        private void SaveServer_Clicked(object sender, EventArgs e)
        {
            if (newServer.Id == -1) //add
                Base.Setting.Initances.AddNewServer(newServer);
            else
                Base.Setting.Initances.UpdateServer(newServer);
            Navigation.PopModalAsync();
        }
    }
}