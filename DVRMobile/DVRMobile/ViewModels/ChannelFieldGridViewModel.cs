using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace DVRMobile.ViewModels
{
    public class ChannelFieldGridViewModel : BaseViewModel
    {
        private bool isActiveChannel = false;
        public bool IsActiveChannel
        {
            get { return isActiveChannel; }
            set { SetProperty(ref isActiveChannel, value); }
        }

        public int ChannelIndex { get; set; }
        public ChannelFieldGridViewModel(int channelIndex)
        {
            this.ChannelIndex = channelIndex;
        }

        public void DoubleClickProc()
        {
            if (Base.Global.OnChangeDivision != null)
            {
                if (Base.Global.oldDivision != Base.ENUM_DIVISIONS.NONE)
                {
                    Base.Global.OnChangeDivision(Base.Global.oldDivision, ChannelIndex);
                    Base.Global.oldDivision = Base.ENUM_DIVISIONS.NONE;
                }
                else
                {
                    Base.Global.oldDivision = Base.Global.curDivision;
                    Base.Global.OnChangeDivision(Base.ENUM_DIVISIONS.DIVISION_1, ChannelIndex);
                }
            }
        }

        private void OnUnActiveChannel()
        {
            IsActiveChannel = false;
        }

        public void ClickProc()
        {
            if (Base.Global.curChannelActive == ChannelIndex)
                return;

            Base.Global.curChannelActive = -1;
            Base.Global.OnUnActiveChannel = OnUnActiveChannel;

            Base.Global.curChannelActive = ChannelIndex;
            IsActiveChannel = true;
        }

        public void UpdateIsActive()
        {
            IsActiveChannel = Base.Global.curChannelActive == ChannelIndex;
            if(IsActiveChannel == true)
                Base.Global.OnUnActiveChannel = OnUnActiveChannel;
        }
    }
}