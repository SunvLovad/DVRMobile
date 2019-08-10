using System;

namespace DVRMobile.Models
{
    public class Server : ViewModels.BaseViewModel
    {
        public int Id { get; set; } = -1;

        private string serverName = string.Empty;
        public string ServerName
        {
            get { return serverName; }
            set { SetProperty(ref serverName, value); OnPropertyChanged("IsValid"); }
        }

        private string serverAddress = string.Empty;
        public string ServerAddress
        {
            get { return serverAddress; }
            set { SetProperty(ref serverAddress, value); OnPropertyChanged("IsValid"); }
        }

        private int httpPort = 10080;
        public int HttpPort
        {
            get { return httpPort; }
            set { SetProperty(ref httpPort, value); OnPropertyChanged("IsValid"); }
        }

        private string userName = string.Empty;
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); OnPropertyChanged("IsValid"); }
        }

        private string passWord = string.Empty;
        public string Password
        {
            get { return passWord; }
            set { SetProperty(ref passWord, value); }
        }

        private bool autoConnectWhenOpen = false;
        public bool AutoConnectWhenOpen
        {
            get { return autoConnectWhenOpen; }
            set { SetProperty(ref autoConnectWhenOpen, value); }
        }

        public bool IsValid
        {
            get
            {
                return serverName.Trim() != "" &&
                    ServerAddress.Trim() != "" &&
                    UserName.Trim() != "" &&
                    HttpPort > 0 && HttpPort < 65536;
            }
        }

        public string Token { get; set; } = null;

        public void CopyFrom(Server server)
        {
            this.Id = server.Id;
            this.ServerName = server.ServerName;
            this.ServerAddress = server.ServerAddress;
            this.HttpPort = server.HttpPort;
            this.UserName = server.UserName;
            this.Password = server.Password;
            this.AutoConnectWhenOpen = server.AutoConnectWhenOpen;
        }

        public bool IsChanged(Server server)
        {
            return (this.ServerName != server.ServerName ||
            this.ServerAddress != server.ServerAddress ||
            this.HttpPort != server.HttpPort ||
            this.UserName != server.UserName ||
            this.Password != server.Password ||
            this.AutoConnectWhenOpen != server.AutoConnectWhenOpen) ;
        }

        public Server Clone()
        {
            Server server = new Server();
            server.CopyFrom(this);
            return server;
        }

        public override string ToString()
        {
            serverName = serverName.Trim().Replace("|", "-");
            serverAddress = serverAddress.Trim().Replace("|", "-");
            userName = userName.Trim().Replace("|", "-");
            passWord = passWord.Trim().Replace("|", "-");
            return $"id={Id}|name={ServerName}|ip={ServerAddress}|port={HttpPort}|user={UserName}|pass={Password}|auto_connect={(AutoConnectWhenOpen ? 1 : 0)}";
        }
    }
}