using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ZebrasLib.Facebook;
using ZebrasLib;
namespace Zebra.WPApp.Pages
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {
        public FacebookLoginPage()
        {
            InitializeComponent();
            this.Loaded += FacebookLoginPage_Loaded;
        }

        async void FacebookLoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            bool isAuthenticated = await LogIn.canAuthenticate();
            if (isAuthenticated)
                {
                    App.isAuthenticated = true;
                    App.facebookAccessToken = Main.AccessToken;
                    App.facebookId = Main.FacebookId;
                    NavigationService.Navigate(new Uri("/Pages/Login/LandingPage.xaml", UriKind.Relative));
                }
            else NavigationService.GoBack();
        }
    }
}