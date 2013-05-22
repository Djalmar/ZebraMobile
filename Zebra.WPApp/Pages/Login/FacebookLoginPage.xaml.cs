using Microsoft.Phone.Controls;
using System.Windows;
using System.Windows.Navigation;
using ZebrasLib;
using ZebrasLib.Facebook;

namespace Zebra.WPApp.Pages
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {
        public FacebookLoginPage()
        {
            InitializeComponent();
            this.Loaded += FacebookLoginPage_Loaded;
        }

        private async void FacebookLoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            bool isAuthenticated = await FacebookMethods.canAuthenticate();
            if (isAuthenticated)
            {
                App.isAuthenticated = true;
                App.facebookAccessToken = Main.AccessToken;
                App.facebookId = Main.FacebookId;
                NavigationService.GoBack();
            }
            else NavigationService.GoBack();
        }
    }
}