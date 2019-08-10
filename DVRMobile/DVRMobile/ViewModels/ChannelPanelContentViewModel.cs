using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DVRMobile.ViewModels
{
    public class ChannelPanelContentViewModel : BaseViewModel
    {
        private double contentHeight = 200.0;
        public double ContentHeight
        {
            get { return contentHeight; }
            set
            {
                double newValue = Base.Global.isHorizontalDevice ? value * 0.8 : value * 0.7;
                SetProperty(ref contentHeight, newValue);
            }
        }

        public int startChannelIndex = 0;
        public int serverIndex = 0;

        public Action OnReloadGUI = null;
        public Action OnStop = null;
        public void ReloadGUI()
        {
            if (OnReloadGUI != null)
                OnReloadGUI();
        }

        public void Stop()
        {
            if (OnStop != null)
                OnStop();
        }

        public ChannelPanelContentViewModel()
        {
        }
    }
}
