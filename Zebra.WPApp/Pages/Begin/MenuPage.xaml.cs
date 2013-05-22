using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using ZebrasLib.Classes;
using ZebrasLib.Facebook;
namespace Zebra.WPApp.Pages.Begin
{
    public partial class MenuPage : PhoneApplicationPage
    {
        public MenuPage()
        {
            InitializeComponent();
            btnZebra.Tap += btnZebra_Tap;
            btnWallet.Tap += btnWallet_Tap;
        }

        private async void btnWallet_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.isAuthenticated)
            {
                List<Friend> lstFbFriends = await FacebookMethods.downloadFriendsList(App.facebookAccessToken);
                MessageBox.Show(lstFbFriends.Count().ToString());
            }
        }

        private void btnZebra_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!App.isAuthenticated)
                NavigationService.Navigate(new Uri("/Pages/Login/FacebookLoginPage.xaml", UriKind.Relative));
            else MessageBox.Show("You're already Logged in");
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            if (NavigationService.CanGoBack)
            {
                while (NavigationService.RemoveBackEntry() != null)
                {
                    NavigationService.RemoveBackEntry();
                }
            }
        }
    }
}