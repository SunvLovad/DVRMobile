using DVRMobile.Internationalization;
using System;


namespace DVRMobile.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        public SettingViewModel()
        {
            Title = StringResources.Find("SETTING");
        }
    }
}
