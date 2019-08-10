using DVRMobile.CustomRenderers;
using DVRMobile.Models;
using DVRMobile.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DVRMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServerChannelPage : ContentPage
    {
        private ServerChannelViewModel viewModel = null;
        public ServerChannelPage(Server server)
        {
            InitializeComponent();
            backButton.OnClick = () =>
            {
                Navigation.PopModalAsync();
            };

            viewModel = new ServerChannelViewModel(server.Id);
            viewModel.Title = server.ServerName;
            BindingContext = viewModel;
        }

        private void CheckAllChannel_Clicked(object sender, System.EventArgs e)
        {
            viewModel.AllChannelCheckedProc();
        }

        private void CheckChannel_Clicked(object sender, System.EventArgs e)
        {
            viewModel.UpdateNumOfChannelChecked();
            viewModel.UpdateCheckAllChannel();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTextBox = sender as SearchBar;
            if (searchTextBox != null)
            {
                string searchKey = searchTextBox.Text.Trim().ToLower();
                viewModel.SearchProc(searchKey);
            }
        }

        private void Save_Clicked(object sender, System.EventArgs e)
        {
            viewModel.OnClosing();
            Navigation.PopModalAsync();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Task.Delay(100);
            var listView = sender as ListView;
            listView.SelectedItem = null;
        }
    }
}