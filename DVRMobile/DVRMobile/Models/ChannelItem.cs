using DVRMobile.ViewModels;
using System;

namespace DVRMobile.Models
{
    public class ChannelItem : BaseViewModel
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelDesc { get; set; }

        private bool isChecked = false;
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref isChecked, value); }
        }

        public void CopyFrom(Channel channel)
        {
            this.ChannelId = channel.channelId;
            this.ChannelName = channel.channelName;
            this.ChannelDesc = channel.cameraDesc;
            this.IsChecked = channel.isMappingToClient;
        }
    }
}
