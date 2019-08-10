using System;
using System.Collections.Generic;
using System.Text;

namespace DVRMobile.Models
{
    public class Channel
    {
        public int channelId { get; set; }
        public string channelName { get; set; }
        public int cameraIndex { get; set; }
        public string cameraName { get; set; }
        public string cameraDesc { get; set; }
        public string mainRtspUri { get; set; }
        public string subRtspUri { get; set; }
        public int clientChannelIndex { get; set; } = -1;
        public bool isMappingToClient { get; set; } = false;
    }
}