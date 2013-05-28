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
                #region Get facebook info from a list of fb codes
                //Reporter reporter = new Reporter { facebookCode = "100000308955899",};
                //Reporter reporterTwo = new Reporter { facebookCode = "500652212", };
                //Reporter reporterThree = new Reporter { facebookCode = "502315910", };
                //List<Reporter> list = new List<Reporter>();
                //list.Add(reporter);
                //list.Add(reporterTwo);
                //list.Add(reporterThree);
                //List<Reporter> reporters = await FacebookMethods.GetFbInfoForTheseReporters(list, App.facebookAccessToken);
                //foreach (Reporter R in reporters)
                //{
                //    MessageBox.Show(R.name + " " + R.picture);
                //}
                #endregion
                List<facebookUser> users = await FacebookMethods.downloadFriendsList(App.facebookAccessToken);
                MessageBox.Show(users.Count().ToString());
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