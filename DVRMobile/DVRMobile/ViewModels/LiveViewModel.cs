using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

using Xamarin.Forms;

using DVRMobile.Views;
using DVRMobile.Internationalization;
using DVRMobile.Base;
using System.Windows.Input;

namespace DVRMobile.ViewModels
{
    public class LiveViewModel : BaseViewModel
    {
        #region properties
        private ObservableCollection<ChannelPanelContentViewModel> channelPanelList = null;
        public ObservableCollection<ChannelPanelContentViewModel> ChannelPanelList
        {
            get { return channelPanelList; }
            set { SetProperty(ref channelPanelList, value); }
        }

        public MyICommand<string> DivisionClicked { get; set; }

        public bool IsSelectedDivision1
        {
            get { return Global.curDivision == ENUM_DIVISIONS.DIVISION_1; }
        }

        public bool IsSelectedDivision4
        {
            get { return Global.curDivision == ENUM_DIVISIONS.DIVISION_4; }
        }

        public bool IsSelectedDivision9
        {
            get { return Global.curDivision == ENUM_DIVISIONS.DIVISION_9; }
        }

        public bool IsSelectedDivision16
        {
            get { return Global.curDivision == ENUM_DIVISIONS.DIVISION_16; }
        }

        public bool IsSelectedDivision36
        {
            get { return Global.curDivision == ENUM_DIVISIONS.DIVISION_36; }
        }

        public bool IsSelectedDivision64
        {
            get { return Global.curDivision == ENUM_DIVISIONS.DIVISION_64; }
        }

        private int currentPanelIndex = -1;
        public int CurrentPanelIndex
        {
            get { return currentPanelIndex; }
            set
            {
                if (currentPanelIndex != value)
                {
                    int oldPageIndex = currentPanelIndex;
                    SetProperty(ref currentPanelIndex, value);
                    OnPropertyChanged("CurrentPanelIndexLabel");
                    OnChannelPanelIndexChanged(oldPageIndex, value);
                }
            }
        }

        private double footerControlHeight = 30.0;
        public double FooterControlHeight
        {
            get { return footerControlHeight; }
            set { SetProperty(ref footerControlHeight, value); }
        }

        private double channelControlHeight = 30.0;
        public double ChannelControlHeight
        {
            get { return channelControlHeight; }
            set { SetProperty(ref channelControlHeight, value); }
        }

        private double footerControlFontSize = 13.0;
        public double FooterControlFontSize
        {
            get { return footerControlFontSize; }
            set { SetProperty(ref footerControlFontSize, value); }
        }

        public String CurrentPanelIndexLabel
        {
            get
            {
                if (ChannelPanelList == null || CurrentPanelIndex == -1)
                    return "1/1";

                return (CurrentPanelIndex + 1) + "/" + ChannelPanelList.Count;
            }
        }

        private bool isHorizontalDevice = false;
        public bool IsHorizontalDevice
        {
            get { return isHorizontalDevice; }
            set { SetProperty(ref isHorizontalDevice, value); }
        }

        private bool isVerticalDevice = true;
        public bool IsVerticalDevice
        {
            get { return isVerticalDevice; }
            set { SetProperty(ref isVerticalDevice, value); }
        }

        private bool hasNavigationBar = true;
        public bool HasNavigationBar
        {
            get { return hasNavigationBar; }
            set { SetProperty(ref hasNavigationBar, value); }
        }

        private bool isActiveFullScreen = false;
        public bool IsActiveFullScreen
        {
            get { return isActiveFullScreen; }
            set { SetProperty(ref isActiveFullScreen, value, "IsActiveFullScreen", OnFullScreenClicked); }
        }

        private int carouselLayoutRowSpan = 1;
        public int CarouselLayoutRowSpan
        {
            get { return carouselLayoutRowSpan; }
            set { SetProperty(ref carouselLayoutRowSpan, value); }
        }

        private bool isEnableChannelControl = false;
        public bool IsEnableChannelControl
        {
            get { return isEnableChannelControl; }
            set { SetProperty(ref isEnableChannelControl, value); }
        }

        public bool IsActiveSwapChannel
        {
            get { return Global.isActiveSwapChannel; }
            set { SetProperty(ref Global.isActiveSwapChannel, value); }
        }

        private bool isActivePlaybackChannel = false;
        public bool IsActivePlaybackChannel
        {
            get { return isActivePlaybackChannel; }
            set { SetProperty(ref isActivePlaybackChannel, value, "IsActivePlaybackChannel", OnPlaybackChannelChanged); }
        }

        private bool isFullScreenChannel = false;
        public bool IsFullScreenChannel
        {
            get { return isFullScreenChannel; }
            set { SetProperty(ref isFullScreenChannel, value, "IsFullScreenChannel", OnFullScreenChannelChanged); }
        }

        public ICommand RemoveChannelCommand { get; set; }

        #endregion

        #region fields
        private object lockCS = new object();
        #endregion

        public LiveViewModel()
        {
            Title = StringResources.Find("LIVE_VIEW");
            ChannelPanelList = new ObservableCollection<ChannelPanelContentViewModel>();
            DivisionClicked = new MyICommand<string>((division) => OnDivisionClicked(division));
            RemoveChannelCommand = new Command(() => OnRemoveChannelClicked());
            MessagingCenter.Subscribe<SearchPage, string>(this, "ReloadLiveView", async (sender, searchKey) =>
            {
                LoadChannelPanelList(searchKey);
            });
            Global.OnChangeDivision = (division, channelIndex) =>
            {
                OnDivisionClicked(((int)division).ToString(), channelIndex);
            };
            Global.OnUpdateStatusChannelControl = (isEnable) =>
            {
                IsEnableChannelControl = isEnable;
                IsActiveSwapChannel = false;
            };
            AssignEventHandler();
            LoadChannelPanelList();
        }

        private void AssignEventHandler()
        {
            Global.serverSiteConnectingManager.ServerConnected += ServerSiteConnecting_ServerConnected;
            Global.serverSiteConnectingManager.ServerDisconnected += ServerSiteConnecting_ServerDisconnected;
        }

        private void ServerSiteConnecting_ServerConnected(int serverIndex, bool connectByUser)
        {
            var serverSiteConnecting = Global.serverSiteConnectingManager.GetServerSiteConnecting(serverIndex);
            if(serverSiteConnecting != null && serverSiteConnecting.isConnected)
                LoadChannelPanelList();
        }

        private void ServerSiteConnecting_ServerDisconnected(int serverId, bool disconnectByUser)
        {
            if(disconnectByUser == true)
                LoadChannelPanelList();
        }

        public void OnDestroy()
        {
            Global.serverSiteConnectingManager.ServerConnected -= ServerSiteConnecting_ServerConnected;
            Global.serverSiteConnectingManager.ServerDisconnected -= ServerSiteConnecting_ServerDisconnected;
        }

        private void OnPlaybackChannelChanged()
        {
            if (IsActivePlaybackChannel == false)
                Services.Toast.Show("UnPlayback", TOAST_LENGTH.SHORT);
            else
                Services.Toast.Show("Playback", TOAST_LENGTH.SHORT);
        }

        private void OnFullScreenChannelChanged()
        {
            if(IsFullScreenChannel == true)
            {
                if (Global.curDivision != ENUM_DIVISIONS.DIVISION_1)
                {
                    Global.oldDivision = Global.curDivision;
                    Global.OnChangeDivision(ENUM_DIVISIONS.DIVISION_1, Global.curChannelActive);
                }
            }
            else
            {
                if (Global.curDivision == ENUM_DIVISIONS.DIVISION_1)
                {
                    Global.OnChangeDivision(Global.oldDivision != ENUM_DIVISIONS.NONE ? Global.oldDivision : ENUM_DIVISIONS.DIVISION_4, Global.curChannelActive);
                    Global.oldDivision = ENUM_DIVISIONS.NONE;
                }
            }
        }

        private void OnRemoveChannelClicked()
        {
            
        }

        private void OnFullScreenClicked()
        {
            if (IsActiveFullScreen == true)
                Services.FullScreen.EnterFullScreen();
            else
                Services.FullScreen.ExitFullScreen();
            HasNavigationBar = !IsActiveFullScreen;
        }

        private void OnDivisionClicked(string division, int channelIndex = -1)
        {
            int iDivision;
            if(int.TryParse(division, out iDivision))
            {
                ENUM_DIVISIONS newDivision = ENUM_DIVISIONS.NONE;
                switch (iDivision)
                {
                    case (int)ENUM_DIVISIONS.DIVISION_1:
                    case (int)ENUM_DIVISIONS.DIVISION_4:
                    case (int)ENUM_DIVISIONS.DIVISION_9:
                    case (int)ENUM_DIVISIONS.DIVISION_16:
                    case (int)ENUM_DIVISIONS.DIVISION_36:
                    case (int)ENUM_DIVISIONS.DIVISION_64:
                        newDivision = (ENUM_DIVISIONS)iDivision;
                        break;
                }

                if(newDivision != ENUM_DIVISIONS.NONE && newDivision != Global.curDivision)
                {
                    ENUM_DIVISIONS oldDivision = Global.curDivision;
                    Global.curDivision = newDivision;
                    OnPropertyChanged("IsSelectedDivision1");
                    OnPropertyChanged("IsSelectedDivision4");
                    OnPropertyChanged("IsSelectedDivision9");
                    OnPropertyChanged("IsSelectedDivision16");
                    OnPropertyChanged("IsSelectedDivision36");
                    OnPropertyChanged("IsSelectedDivision64");
                    OnDivisionChanged(oldDivision, newDivision, channelIndex);
                }
            }
        }

        private void OnDivisionChanged(ENUM_DIVISIONS oldDivision, ENUM_DIVISIONS newDivision, int channelIndex = -1)
        {
            int index = 0;
            foreach (var channelContentView in ChannelPanelList)
            {
                channelContentView.startChannelIndex = index * (int)Global.curDivision;
                channelContentView.ReloadGUI();
                index++;
            }

            if (channelIndex != -1 && channelIndex < ChannelPanelList.Count)
                Global.curChannelActive = channelIndex;

            if (Global.curChannelActive != -1)
                CurrentPanelIndex = Global.curChannelActive / (int)newDivision;

            IsFullScreenChannel = (newDivision == ENUM_DIVISIONS.DIVISION_1);
        }

        private void OnChannelPanelIndexChanged(int oldPageIndex, int newPageIndex)
        {
            if (oldPageIndex != -1)
            {
                //if(oldPageIndex < ChannelPanelList.Count)
                //    ChannelPanelList[oldPageIndex]. = "Stop";
                //stop live view
            }

            if(newPageIndex != -1)
            {
                if (newPageIndex < ChannelPanelList.Count)
                {
                    //ChannelPanelList[newPageIndex].MyText = $"trang {newPageIndex}";
                    //start live view
                }
            }
            else
            {
                foreach(var channelContentView in ChannelPanelList)
                {
                    //channelContentView.MyText = "Stop";
                }
            }
        }

        public void LoadChannelPanelList(string searchKey = null)
        {
            lock (lockCS)
            {
                try
                {
                    CurrentPanelIndex = -1;
                    DisableChannelControl();

                    for (int i = 0; i < ChannelPanelList.Count; i++)
                    {
                        ChannelPanelList[i].Stop();
                    }

                    ChannelPanelList = null;
                    ChannelPanelList = new ObservableCollection<ChannelPanelContentViewModel>();

                    int index = 0;
                    for (int i = 0; i < Global.MAX_SERVER_CONNECTING_SUPPORT; i++)
                    {
                        var serverSiteConnecting = Global.serverSiteConnectingManager.GetServerSiteConnecting(i);
                        if (serverSiteConnecting != null)
                        {
                            int numOfChannel = serverSiteConnecting.GetListChannel().Count;
                            int numOfPage = numOfChannel / (int)Global.curDivision;
                            if(numOfPage * (int)Global.curDivision != numOfChannel)
                                numOfPage++;

                            for (int page = 0; page < numOfPage; page++)
                            {
                                var channelPanelContentViewModel = new ChannelPanelContentViewModel();
                                channelPanelContentViewModel.startChannelIndex = index * (int)Global.curDivision;
                                channelPanelContentViewModel.serverIndex = i;
                                ChannelPanelList.Add(channelPanelContentViewModel);
                                index++;
                            }
                        }
                    }

                    if (ChannelPanelList.Count == 0)
                    {
                        var channelPanelContentViewModel = new ChannelPanelContentViewModel();
                        channelPanelContentViewModel.startChannelIndex = 0;
                        ChannelPanelList.Add(channelPanelContentViewModel);
                    }

                    UpdateChannelPanelHeight();
                    CurrentPanelIndex = 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        private void UpdateChannelPanelHeight()
        {
            foreach(var channelPanel in ChannelPanelList)
            {
                channelPanel.ContentHeight = Base.Global.deviceHeight;
            }
        }

        public void CaculateControlSize(double width, double height)
        {
            Base.Global.deviceWidth = width;
            Base.Global.deviceHeight = height;
            if (height < width) //horizontal
            {
                IsHorizontalDevice = true;
                IsVerticalDevice = false;
                CarouselLayoutRowSpan = 3;
                Base.Global.isHorizontalDevice = true;
                FooterControlHeight = 32;
            }
            else //vertical
            {
                IsHorizontalDevice = false;
                IsVerticalDevice = true;
                CarouselLayoutRowSpan = 1;
                Base.Global.isHorizontalDevice = false;
                if (IsActiveFullScreen == true)
                    IsActiveFullScreen = false;
                FooterControlHeight = 38;
            }

            ChannelControlHeight = 28;
            FooterControlFontSize = FooterControlHeight * 0.45;
            UpdateChannelPanelHeight();
        }

        private void DisableChannelControl()
        {
            IsActiveSwapChannel = false;
            Global.curChannelActive = -1;
            IsEnableChannelControl = false;
        }
    }
}