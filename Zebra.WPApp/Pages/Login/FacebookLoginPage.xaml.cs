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
            if (!Main.isAuthenticated)
            {
                Main.isAuthenticated = true;

                await LogIn.Authenticate();
                NavigationService.Navigate(new Uri("/Pages/Login/LandingPage.xaml", UriKind.Relative));
            }
        }
    }
}