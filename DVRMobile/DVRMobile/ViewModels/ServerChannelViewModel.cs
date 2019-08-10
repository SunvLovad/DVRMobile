using DVRMobile.Internationalization;
using DVRMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DVRMobile.ViewModels
{
    public class ServerChannelViewModel : BaseViewModel
    {
        private int numOfChannelChecked = 0;
        public int numOfChannel = 0;

        public string ChannelCheckedText
        {
            get
            {
                return string.Format(StringResources.Find("NUM_OF_CHANNEL_CHECKED"), numOfChannelChecked, numOfChannel);
            }
        }

        public bool IsCheckAllChannel
        {
            get
            {
                return numOfChannelChecked > 0;
            }
        }

        private string ChannelIdCheckedStringFormat = string.Empty;
        private int serverId = 0;

        public List<ChannelItem> ListChannel { get; set; }

        private ObservableCollection<ChannelItem> listChannelDisplay = null;
        public ObservableCollection<ChannelItem> ListChannelDisplay
        {
            get { return listChannelDisplay; }
            set { SetProperty(ref listChannelDisplay, value); }
        }

        public ServerChannelViewModel(int serverId)
        {
            ListChannel = new List<ChannelItem>();
            ListChannelDisplay = new ObservableCollection<ChannelItem>();

            this.serverId = serverId;
            int serverIndex = Base.Global.serverSiteConnectingManager.FindServerIndexFromId(serverId);
            var serverSiteConnecting = Base.Global.serverSiteConnectingManager.GetServerSiteConnecting(serverIndex);
            if (serverSiteConnecting != null)
            {
                foreach (var channel in serverSiteConnecting.GetListChannel())
                {
                    var channelItem = new ChannelItem();
                    channelItem.CopyFrom(channel);
                    ListChannel.Add(channelItem);
                    ListChannelDisplay.Add(channelItem);
                    if (channelItem.IsChecked)
                        ChannelIdCheckedStringFormat += channelItem.ChannelId + ",";
                }
            }

            numOfChannel = ListChannel.Count;
            UpdateNumOfChannelChecked();
        }

        public void UpdateCheckAllChannel()
        {
            OnPropertyChanged("IsCheckAllChannel");
        }

        public void AllChannelCheckedProc()
        {
            bool isChecked = !IsCheckAllChannel;
            foreach (var channel in ListChannel)
            {
                channel.IsChecked = isChecked;
            }
            UpdateNumOfChannelChecked();
        }

        public void UpdateNumOfChannelChecked()
        {
            int numChecked = 0;
            foreach(var channel in ListChannel)
            {
                if (channel.IsChecked == true)
                    numChecked++;
            }

            numOfChannelChecked = numChecked;
            OnPropertyChanged("ChannelCheckedText");
        }

        public void SearchProc(string searchKey)
        {
            ListChannelDisplay.Clear();

            if (searchKey == string.Empty)
            {
                foreach (var channel in ListChannel)
                {
                    ListChannelDisplay.Add(channel);
                }
                return;
            }

            foreach (var channel in ListChannel)
            {
                if(channel.ChannelName.ToLower().Contains(searchKey) || channel.ChannelDesc.Contains(searchKey))
                    ListChannelDisplay.Add(channel);
            }
        }

        public void OnClosing()
        {
            string curChannelIdChecked = string.Empty;
            foreach (var channel in ListChannel)
            {
                if (channel.IsChecked)
                    curChannelIdChecked += channel.ChannelId + ",";
            }

            if(curChannelIdChecked.Equals(ChannelIdCheckedStringFormat) == false) //is change
            {
                int serverIndex = Base.Global.serverSiteConnectingManager.FindServerIndexFromId(serverId);
                var serverSiteConnecting = Base.Global.serverSiteConnectingManager.GetServerSiteConnecting(serverIndex);
                if (serverSiteConnecting != null)
                {
                    foreach (var channel in serverSiteConnecting.GetListChannel())
                    {
                        foreach (var channelItem in ListChannel)
                        {
                            if (channel.channelId == channelItem.ChannelId)
                            {
                                channel.isMappingToClient = channelItem.IsChecked;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
