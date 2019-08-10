using Xamarin.Forms;

namespace DVRMobile.Models
{
    public enum MenuItemType
    {
        LiveView,
        Setting,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        public ImageSource Icon { get; set; }
    }
}
