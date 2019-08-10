
using DVRMobile.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using Xamarin.Forms;

namespace DVRMobile.ViewModels
{
    public class ServerPageViewModel : BaseViewModel
    {
        private ObservableCollection<ServerItem> serverList = null;
        public ObservableCollection<ServerItem> ServerList
        {
            get { return serverList; }
            set { SetProperty(ref serverList, value); }
        }

        public bool isBusy = false;

        public ServerPageViewModel()
        {
        }

        public void LoadServerList()
        {
            ServerList = null;
            ServerList = Base.Setting.Initances.LoadServerList();
        }

        public ServerItem FindServerById(int serverId)
        {
            foreach(var server in serverList)
            {
                if (server.Id == serverId)
                    return server;
            }

            return null;
        }

        public void RemoveServer(int serverId)
        {
            var server = FindServerById(serverId);
            if (server != null)
                serverList.Remove(server);
        }

        public void EditServer(int serverId)
        {
            var server = FindServerById(serverId);
            var editServer = Base.Setting.Initances.FindServerById(serverId);
            if (server != null && editServer != null)
            {
                bool isChangeAccountOrIp = false;
                if (server.ServerAddress != editServer.ServerAddress ||
                    server.HttpPort != editServer.HttpPort ||
                    server.UserName != editServer.UserName ||
                    server.Password != editServer.Password)
                    isChangeAccountOrIp = true;

                server.CopyFrom(editServer);

                if (isChangeAccountOrIp == true)
                    Base.Global.serverSiteConnectingManager.ReconnectServer(serverId, true);
            }
        }

        public void ConnectServer(int serverId)
        {
            isBusy = true;
            SetServerIsInConnecting(serverId, true);

            new Thread(() =>
            {
                Base.Global.serverSiteConnectingManager.ConnectServer(serverId);
                Device.BeginInvokeOnMainThread(new Action(() =>
                {
                    SetServerIsInConnecting(serverId, false);
                    isBusy = false;
                }));
            }).Start();
        }

        public void DisconnectServer(int serverId)
        {
            isBusy = true;
            SetServerIsConnected(serverId, false);
            new Thread(() =>
            {
                Base.Global.serverSiteConnectingManager.DisconnectServer(serverId, true);
                isBusy = false;
            }).Start();
        }

        public void SetServerIsInConnecting(int serverId, bool value)
        {
            var server = FindServerById(serverId);
            if (server != null)
                server.IsConnecting = value;
        }

        public void SetServerIsConnected(int serverId, bool value)
        {
            var server = FindServerById(serverId);
            if (server != null)
                server.IsConnected = value;
        }

        public void UpdateWhenConnected(int serverIndex)
        {
            var serverSiteConnecting = Base.Global.serverSiteConnectingManager.GetServerSiteConnecting(serverIndex);
            if(serverSiteConnecting != null)
            {
                var server = FindServerById(serverSiteConnecting.serverConfig.Id);
                if (server != null)
                    server.IsConnected = true;
            }
        }

        public void UpdateWhenDisconect(int serverIndex)
        {
            var serverSiteConnecting = Base.Global.serverSiteConnectingManager.GetServerSiteConnecting(serverIndex);
            if (serverSiteConnecting != null)
            {
                var server = FindServerById(serverSiteConnecting.serverConfig.Id);
                if (server != null)
                    server.IsConnected = false;
            }
        }
    }
}
