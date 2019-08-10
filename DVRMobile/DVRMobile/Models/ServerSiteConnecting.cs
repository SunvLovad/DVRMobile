using System;
using System.Collections.Generic;
using System.Text;

namespace DVRMobile.Models
{
    public class ServerSiteConnecting
    {
        public bool isConnected = false;
        public string token = string.Empty;
        public Server serverConfig = null;
        public int serverIndex = 0;

        private object lockCS = new object();
        private List<Channel> listChannel = null;
        
        public Channel FindChannelByChannelId(int channelId)
        {
            lock(lockCS)
            {
                foreach (var channel in listChannel)
                {
                    if (channel.channelId == channelId)
                        return channel;
                }

                return null;
            }
        }

        public List<Channel> GetListChannel()
        {
            return listChannel;
        }

        public Channel SetChannelClientIndex(int channelIndex)
        {
            lock (lockCS)
            {
                foreach (var channel in listChannel)
                {
                    if (channel.clientChannelIndex == -1)
                    {
                        channel.clientChannelIndex = channelIndex;
                        return channel;
                    }
                }
                return null;
            }
        }

        public ServerSiteConnecting(int serverIndex, Server server, string token, List<Channel> listChannel)
        {
            serverConfig = server.Clone();
            isConnected = true;
            this.token = token;
            this.listChannel = listChannel;
        }
    }
}
