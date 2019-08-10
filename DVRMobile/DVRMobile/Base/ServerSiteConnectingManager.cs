using DVRMobile.Internationalization;
using DVRMobile.JsonModels;
using DVRMobile.Models;
using DVRMobile.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DVRMobile.Base
{
    public class ServerSiteConnectingManager
    {
        private object lockCS = new object();
        private Dictionary<int, ServerSiteConnecting> serverSiteConnectingList = null;
        private List<int> serverIdConnectingList = null;

        /// <summary>
        /// call after server connected
        /// params: 
        ///  int - Server Index
        ///  bool - connect by user
        /// </summary>
        public event Action<int, bool> ServerConnected;

        /// <summary>
        /// call after server disconnected
        /// params: 
        ///  int - Server Id
        ///  bool - disconnect by user
        /// </summary>
        public event Action<int, bool> ServerDisconnected;

        /// <summary>
        /// call in server disconnecting
        /// params: 
        ///  int - Server Index
        ///  bool - disconnect by user
        /// </summary>
        public event Action<int, bool> ServerDisconnecting;
        public ServerSiteConnectingManager()
        {
            serverSiteConnectingList = new Dictionary<int, ServerSiteConnecting>();
            serverIdConnectingList = new List<int>();
        }

        public bool IsServerConnected(int serverId)
        {
            lock (lockCS)
            {
                foreach(var serverSiteConnecting in serverSiteConnectingList)
                {
                    if (serverSiteConnecting.Value.serverConfig.Id == serverId)
                        return serverSiteConnecting.Value.isConnected;
                }

                return false;
            }
        }

        public bool GetIsServerConnecting(int serverId)
        {
            lock (lockCS)
                return serverIdConnectingList.Contains(serverId);
        }

        public void SetIsServerConnecting(int serverId, bool value)
        {
            lock (lockCS)
            {
                if (value == true)
                {
                    if (serverIdConnectingList.Contains(serverId) == false)
                        serverIdConnectingList.Add(serverId);
                }
                else
                {
                    serverIdConnectingList.Remove(serverId);
                }
            }
        }

        public int FindServerIndexFromId(int serverId)
        {
            lock (lockCS)
            {
                foreach (var serverSiteConnecting in serverSiteConnectingList)
                {
                    if (serverSiteConnecting.Value.serverConfig.Id == serverId)
                        return serverSiteConnecting.Key;
                }

                return -1;
            }
        }

        public ServerSiteConnecting GetServerSiteConnecting(int serverIndex)
        {
            lock (lockCS)
            {
                if (serverSiteConnectingList.ContainsKey(serverIndex))
                    return serverSiteConnectingList[serverIndex];
                return null;
            }
        }

        public string GetServerHttpUrl(Server server, string command)
        {
            if (command == null)
                return string.Format("http://{0}:{1}/mobi", server.ServerAddress, server.HttpPort);

            return string.Format("http://{0}:{1}/mobi/{2}", server.ServerAddress, server.HttpPort, command);
        }

        public Dictionary<string, string> GetAuthServerHttpParam(Server server)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            if (server.Token != null)
                param.Add("token", server.Token);
            else
            {
                param.Add("username", server.UserName);
                param.Add("password", server.Password);
            }

            return param;
        }

        public bool ConnectServer(int serverId, bool isConnectByUser = true)
        {
            if (IsServerConnected(serverId) || GetIsServerConnecting(serverId))
                return true;

            try
            {
                SetIsServerConnecting(serverId, true);
                var server = Setting.Initances.FindServerById(serverId);
                if (server != null)
                {
                    string serverUrl = GetServerHttpUrl(server, "connect");
                    Dictionary<string, string> param = GetAuthServerHttpParam(server);

                    string data = Global.SendHttpPostRequest(serverUrl, param, 5000);
                    return ConnectServerProc(server, data);
                }
            }
            finally
            {
                SetIsServerConnecting(serverId, false);
            }

            return false;
        }

        private bool ConnectServerProc(Server server, string data)
        {
            if (data == null)
            {
                Toast.ShowInThread(server.ServerName + ": " + StringResources.Find("CONNECT_SERVER_FAILED"), TOAST_LENGTH.LONG);
                return false;
            }

            try
            {
                data = CryptoAesAPI.Decrypt(data);
                ConnectServerJson connectServerJson = JsonConvert.DeserializeObject<ConnectServerJson>(data);

                if(connectServerJson.msgId != (int)ENUM_MSG_ID.MSG_ID_OK || connectServerJson.token == null 
                    || connectServerJson.listChannel == null)
                {
                    switch(connectServerJson.msgId)
                    {
                        case (int)ENUM_MSG_ID.LOGIN_AUTH_ERROR:
                            Toast.ShowInThread(server.ServerName + ": " + StringResources.Find("LOGIN_SERVER_FAILED"), TOAST_LENGTH.LONG);
                            break;
                        default:
                            Toast.ShowInThread(server.ServerName + ": " + StringResources.Find("LOGIN_SERVER_FAILED"), TOAST_LENGTH.LONG);
                            //ENUM_MSG_ID msgId = (ENUM_MSG_ID)connectServerJson.msgId;
                            //Toast.ShowInThread(server.ServerName + ": " + msgId.ToString(), TOAST_LENGTH.LONG);
                            break;
                    }

                    return false;
                }

                List<Channel> listChannel = new List<Channel>();
                foreach(var channel in connectServerJson.listChannel)
                {
                    channel.cameraDesc = CryptoAesAPI.ConvertAscIIToUTF8(channel.cameraDesc);
                    channel.cameraName = CryptoAesAPI.ConvertAscIIToUTF8(channel.cameraName);
                    channel.channelName = CryptoAesAPI.ConvertAscIIToUTF8(channel.channelName);
                    listChannel.Add(channel);
                }

                int serverIndex = 0;
                lock (lockCS)
                {
                    for (int i = 0; i < Global.MAX_SERVER_CONNECTING_SUPPORT; i++)
                    {
                        if (serverSiteConnectingList.ContainsKey(i) == false)
                        {
                            serverIndex = i;
                            serverSiteConnectingList.Add(serverIndex, 
                                new ServerSiteConnecting(serverIndex, server, connectServerJson.token, 
                                listChannel));
                            break;
                        }
                    }
                }

                if (ServerConnected != null)
                    ServerConnected(serverIndex, true);

                return true;
            }
            catch
            {
                Toast.ShowInThread(server.ServerName + ": " + StringResources.Find("CONNECT_SERVER_FAILED"), TOAST_LENGTH.LONG);
                return false;
            }
        }

        public void DisconnectServer(int serverId, bool isDisconnectByUser)
        {
            int serverIndex = FindServerIndexFromId(serverId);
            if (serverIndex != -1)
            {
                if (ServerDisconnecting != null)
                    ServerDisconnecting(serverIndex, isDisconnectByUser);
                lock (lockCS)
                {
                    serverSiteConnectingList.Remove(serverIndex);
                }
                if (ServerDisconnected != null)
                    ServerDisconnected(serverId, isDisconnectByUser);

                var server = Setting.Initances.FindServerById(serverId);
                if (server != null)
                {
                    string serverUrl = GetServerHttpUrl(server, "disconnect");
                    Dictionary<string, string> param = GetAuthServerHttpParam(server);

                    Global.SendHttpPostRequest(serverUrl, param, 5000);
                }
            }
        }

        public void ReconnectServer(int serverId, bool isRunInNewThread)
        {
            int serverIndex = FindServerIndexFromId(serverId);
            if (serverIndex == -1)
                return;

            if (isRunInNewThread == true)
            {
                new System.Threading.Thread(() =>
                {
                    DisconnectServer(serverId, false);
                    ConnectServer(serverId, false);
                }).Start();
            }
            else
            {
                DisconnectServer(serverId, false);
                ConnectServer(serverId, false);
            }
        }
    }
}
