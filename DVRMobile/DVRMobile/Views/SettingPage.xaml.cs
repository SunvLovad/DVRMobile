using DVRMobile.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DVRMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        private SettingViewModel viewModel = null;
        public SettingPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SettingViewModel();
        }
    }
}