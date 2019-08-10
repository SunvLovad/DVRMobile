using DVRMobile.ViewModels;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace DVRMobile.Models
{
    public class ServerItem : Server
    {
        private bool isConnected = false;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                SetProperty(ref isConnected, value);
                OnPropertyChanged("IsNotConnected");
                if(value == true)
                    IsConnecting = false;
            }
        }

        private bool isConnecting = false;
        public bool IsConnecting
        {
            get { return isConnecting; }
            set
            {
                SetProperty(ref isConnecting, value);
                OnPropertyChanged("IsEnableControl");
                if (value == true)
                    IsConnected = false;
                else
                    OnPropertyChanged("IsNotConnected");
            }
        }

        public bool IsNotConnected
        {
            get { return !isConnected && !isConnecting; }
        }

        public bool IsEnableControl
        {
            get { return !isConnecting; }
        }

        public ServerItem(Server server, bool isConnected)
        {
            this.CopyFrom(server);
            this.IsConnected = isConnected;
        }
    }
}
