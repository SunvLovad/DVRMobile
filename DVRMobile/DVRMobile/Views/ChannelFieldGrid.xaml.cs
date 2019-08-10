using DVRMobile.Base;
using DVRMobile.ViewModels;
using LibVLCSharp.Shared;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DVRMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChannelFieldGrid : ContentView
    {
        private ChannelFieldGridViewModel viewModel = null;
        public ChannelFieldGrid(int channelIndex, int serverIndex)
        {
            InitializeComponent();
            viewModel = new ChannelFieldGridViewModel(channelIndex);
            viewModel.UpdateIsActive();
            BindingContext = viewModel;
            txt.Text = $"Channel {channelIndex}";

            var serverSiteConnecting = Global.serverSiteConnectingManager.GetServerSiteConnecting(serverIndex);
            if(serverSiteConnecting != null)
            {
                var channel = serverSiteConnecting.SetChannelClientIndex(channelIndex);
                if(channel != null)
                {
                    this.videoView.MediaPlayer = new MediaPlayer(Global.libVlc);
                    channel.mainRtspUri = String.Format("rtsp://192.168.1.107:5554/dvms/streaming/ch{0}/sub", channelIndex + 1);
                    this.videoView.MediaPlayer.Play(new Media(Global.libVlc, channel.mainRtspUri, FromType.FromLocation));                
                }
            }
        }

        private int clickCount = 0;
        private void ChannelFieldButton_Clicked(object sender, EventArgs e)
        {
            viewModel.ClickProc();
            if (clickCount == 0)
                Device.StartTimer(new TimeSpan(0, 0, 0, 0, 200), OnClickProc);
            clickCount++;
        }

        private bool OnClickProc()
        {
            if (clickCount > 1)
                viewModel.DoubleClickProc();
            clickCount = 0;
            return false;
        }
    }
}