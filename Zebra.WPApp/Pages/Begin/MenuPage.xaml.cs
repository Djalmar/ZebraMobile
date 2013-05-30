using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using ZebrasLib.Classes;
using ZebrasLib.Events;
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
                #region Uso de mock data y descarga de informacion de facebook de los reporteros de un evento dado

                List<Event> lstEvents = EventsMethods.MockDataGetEvents();
                foreach (Event E in lstEvents)
                {
                    MessageBox.Show(E.description);
                    E.reporters = await FacebookMethods.GetFbInfoForTheseReporters(E.reporters, App.facebookAccessToken);
                    string message = "Reportado por: ";
                    foreach (Reporter R in E.reporters)
                    {
                        message += R.name + " ";
                    }
                    MessageBox.Show(message);
                }

                #endregion Uso de mock data y descarga de informacion de facebook de los reporteros de un evento dado

                #region Get a list of facebook friends that are using the same app

                //List<facebookUser> users = await FacebookMethods.downloadFriendsList(App.facebookAccessToken);
                //MessageBox.Show(users.Count().ToString());

                #endregion Get a list of facebook friends that are using the same app
            }
            else MessageBox.Show("You're not logged in");
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