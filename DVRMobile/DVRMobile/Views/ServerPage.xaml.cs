using System;
using System.ComponentModel;
using Xamarin.Forms;
using DVRMobile.Internationalization;
using DVRMobile.ViewModels;
using Plugin.Settings;
using System.Threading;
using DVRMobile.Models;
using DVRMobile.Services;
using System.Threading.Tasks;

namespace DVRMobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ServerPage : ContentPage
    {
        private ServerPageViewModel viewModel = null;
        public ServerPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ServerPageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadServerList();
            Base.Global.serverSiteConnectingManager.ServerConnected += ServerSiteConnectingManager_ServerConnected;
            Base.Global.serverSiteConnectingManager.ServerDisconnecting += ServerSiteConnectingManager_ServerDisconnecting;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Base.Global.serverSiteConnectingManager.ServerConnected -= ServerSiteConnectingManager_ServerConnected;
            Base.Global.serverSiteConnectingManager.ServerDisconnecting -= ServerSiteConnectingManager_ServerDisconnecting;
        }

        private void ServerSiteConnectingManager_ServerDisconnecting(int serverIndex, bool arg2)
        {
            viewModel.UpdateWhenDisconect(serverIndex);
        }

        private void ServerSiteConnectingManager_ServerConnected(int serverIndex, bool isConnectByUser)
        {
            viewModel.UpdateWhenConnected(serverIndex);
        }

        private async void AddServer_Clicked(object sender, EventArgs e)
        {
            if (viewModel.isBusy)
                return;
            viewModel.isBusy = true;
            var addServerPage = new NavigationPage(new AddServerPage());
            addServerPage.Disappearing += AddServerPage_Disappearing;
            await Navigation.PushModalAsync(addServerPage);
            viewModel.isBusy = false;
        }

        private void AddServerPage_Disappearing(object sender, EventArgs e)
        {
            if(Base.Setting.Initances.isChangeServerList == true)
            {
                //RELOAD
                if (serverIdEdit == -1)
                    viewModel.LoadServerList();
                else
                    viewModel.EditServer(serverIdEdit);
            }
            serverIdEdit = -1;
        }

        private int GetServerId(object sender)
        {
            var imageButton = sender as ImageButton;
            if (imageButton == null || imageButton.Parent == null)
                return -1;

            var stackLayout = imageButton.Parent as StackLayout;
            if (stackLayout == null)
                return -1;

            return stackLayout.TabIndex;
        }

        private async void EffectImageButton(object sender)
        {
            var imageButton = sender as ImageButton;
            await imageButton.FadeTo(0.7, 50);
            await imageButton.FadeTo(1, 50);
        }

        private int serverIdEdit = -1;
        private async void EditServer_Clicked(object sender, EventArgs e)
        {
            if (viewModel.isBusy)
                return;
            viewModel.isBusy = true;
            int serverId = GetServerId(sender);
            var server = Base.Setting.Initances.FindServerById(serverId);
            if (server != null)
            {
                EffectImageButton(sender);
                serverIdEdit = serverId;
                var editServerPage = new NavigationPage(new AddServerPage(server));
                editServerPage.Disappearing += AddServerPage_Disappearing;
                await Navigation.PushModalAsync(editServerPage);
            }
            viewModel.isBusy = false;
        }

        private async void DeleteServer_Clicked(object sender, EventArgs e)
        {
            int serverId = GetServerId(sender);
            var server = Base.Setting.Initances.FindServerById(serverId);
            if (server != null)
            {
                EffectImageButton(sender);
                var result = await this.DisplayAlert(StringResources.Find("ALERT"), StringResources.Find("DELETE_SERVER_CONFIRM") + " \"" + server.ServerName + "\"?", "Yes", "No");
                if (result)
                {
                    Base.Setting.Initances.RemoveServer(serverId);
                    viewModel.RemoveServer(serverId);
                }
            }
        }

        private async void ConnectServer_Clicked(object sender, EventArgs e)
        {
            int serverId = GetServerId(sender);
            EffectImageButton(sender);

            if (Base.Global.serverSiteConnectingManager.IsServerConnected(serverId) == false)
                viewModel.ConnectServer(serverId);
            else
            {
                var server = Base.Setting.Initances.FindServerById(serverId);
                if (server != null)
                {
                    var result = await this.DisplayAlert(StringResources.Find("ALERT"), StringResources.Find("DELETE_SERVER_CONFIRM") + " \"" + server.ServerName + "\"?", "Yes", "No");
                    if (result == true)
                        viewModel.DisconnectServer(serverId);
                }
            }
        }

        private async void ServerListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (viewModel.isBusy)
                return;

            ServerItem serverSelected = e.Item as ServerItem;
            if(serverSelected != null)
            {
                if(Base.Global.serverSiteConnectingManager.IsServerConnected(serverSelected.Id) == false)
                {
                    Toast.Show(StringResources.Find("NOT_CONNECT_SERVER"));
                    await Task.Delay(100);
                    var listView = sender as ListView;
                    listView.SelectedItem = null;
                    return;
                }

                viewModel.isBusy = true;
                var serverChannelPage = new NavigationPage(new ServerChannelPage(serverSelected));
                await Navigation.PushModalAsync(serverChannelPage);
                viewModel.isBusy = false;
            }
        }
    }
}