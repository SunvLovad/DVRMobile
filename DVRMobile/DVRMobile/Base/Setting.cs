
using DVRMobile.Models;
using Plugin.Settings;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DVRMobile.Base
{
    public class Setting
    {
        public bool isChangeServerList = false;
        public int maxServerId = 0;
        private object lockServerList = new object();
        private List<Server> serverList = null;
        public Setting()
        {
            serverList = new List<Server>();
        }

        public void AddNewServer(Server newServer)
        {
            int serverId = GetNewServerId();
            lock (lockServerList)
            {
                newServer.Id = serverId;
                CrossSettings.Current.AddOrUpdateValue("server_" + newServer.Id, newServer.ToString());
                serverList.Add(newServer);

                //sort - Begin
                for (int i = 0; i < serverList.Count; i++)
                {
                    for (int j = i + 1; j < serverList.Count; j++)
                    {
                        if(serverList[i].Id > serverList[j].Id)
                        {
                            var temp = serverList[i];
                            serverList[i] = serverList[j];
                            serverList[j] = temp;
                        }
                    }
                }
                //sort - End

                isChangeServerList = true;
            }
        }

        public void UpdateServer(Server newServer)
        {
            var server = FindServerById(newServer.Id);
            if (server != null && server.IsChanged(newServer))
            {
                lock (lockServerList)
                {
                    CrossSettings.Current.AddOrUpdateValue("server_" + newServer.Id, newServer.ToString());
                    server.CopyFrom(newServer);
                }
                isChangeServerList = true;
            }
        }

        public void RemoveServer(int serverId)
        {
            var server = FindServerById(serverId);
            if(server != null)
            {
                Global.serverSiteConnectingManager.DisconnectServer(serverId, true);
                lock(lockServerList)
                    serverList.Remove(server);

                CrossSettings.Current.Remove("server_" + serverId);
            }
        }

        public ObservableCollection<ServerItem> LoadServerList()
        {
            var ServerList = new ObservableCollection<ServerItem>();
            lock (lockServerList)
            {
                foreach (var server in serverList)
                {
                    bool isConnected = Global.serverSiteConnectingManager.IsServerConnected(server.Id);
                    ServerList.Add(new ServerItem(server, isConnected));
                }
            }
            return ServerList;
        }

        public Server FindServerById(int id)
        {
            lock (lockServerList)
            {
                foreach (var server in serverList)
                {
                    if (server.Id == id)
                        return server;
                }
            }

            return null;
        }

        public void LoadSettings()
        {
            serverList.Clear();
            maxServerId = CrossSettings.Current.GetValueOrDefault("maxServerId", 0);
            for (int i = 0; i < maxServerId; i++)
            {
                string serverInfo = CrossSettings.Current.GetValueOrDefault("server_" + i, string.Empty);
                if (string.IsNullOrEmpty(serverInfo) == false)
                {
                    var server = ParseServerInfo(serverInfo);
                    if (server.IsValid)
                        serverList.Add(server);
                    else
                        CrossSettings.Current.Remove("server_" + i);
                }
            }
        }

        private Server ParseServerInfo(string serverInfo)
        {
            Server server = new Server();
            string[] serverInfoArr = serverInfo.Split('|');
            foreach (string info in serverInfoArr)
            {
                int startIndexOf = info.IndexOf('=');
                if (startIndexOf != -1)
                {
                    string key = info.Substring(0, startIndexOf);
                    string value = info.Substring(startIndexOf + 1);
                    switch (key)
                    {
                        case "id":
                            int id;
                            if (int.TryParse(value, out id))
                                server.Id = id;
                            break;
                        case "name":
                            server.ServerName = value;
                            break;
                        case "ip":
                            server.ServerAddress = value;
                            break;
                        case "port":
                            int httpPort;
                            if (int.TryParse(value, out httpPort))
                                server.HttpPort = httpPort;
                            break;
                        case "user":
                            server.UserName = value;
                            break;
                        case "pass":
                            server.Password = value;
                            break;
                        case "auto_connect":
                            int autoConnect;
                            if (int.TryParse(value, out autoConnect))
                                server.AutoConnectWhenOpen = autoConnect == 1;
                            break;
                    }
                }
            }

            return server;
        }

        /// <summary>
        /// tìm id chưa được sử dụng
        /// </summary>
        /// <returns></returns>
        private int GetNewServerId()
        {
            int serverId = 0;
            lock (lockServerList)
            {
                while (true)
                {
                    bool isExist = false;
                    foreach (var server in serverList)
                    {
                        if (server.Id == serverId)
                        {
                            isExist = true;
                            break;
                        }
                    }

                    if (isExist == false)
                        break;

                    serverId++;
                }
            }

            if (serverId >= maxServerId)
            {
                maxServerId++;
                CrossSettings.Current.AddOrUpdateValue("maxServerId", maxServerId);
            }

            return serverId;
        }

        private static Setting initances = null;
        public static Setting Initances
        {
            get
            {
                if (initances == null)
                    initances = new Setting();

                return initances;
            }
        }
    }
}
