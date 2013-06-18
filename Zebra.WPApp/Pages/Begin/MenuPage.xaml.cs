﻿using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using ZebrasLib;
using ZebrasLib.Classes;
using ZebrasLib.Events;
using OurFacebook;
namespace Zebra.WPApp.Pages.Begin
{
    public partial class MenuPage : PhoneApplicationPage
    {
   
        public MenuPage()
        {
            InitializeComponent();
        }

        private void btnWallet_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Wallet/GetDataPage.xaml", UriKind.Relative));
        }

        private void btnZebra_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!App.isAuthenticated)
                NavigationService.Navigate(new Uri("/Pages/Login/FacebookLoginPage.xaml", UriKind.Relative));
            else MessageBox.Show("You're already Logged in");
        }

        void btnPlaces_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Places/CategoriesPage.xaml", UriKind.Relative));
        }

        void btnTraffic_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Trouble/TroublesPage.xaml", UriKind.Relative));
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Places/CategoriesPage.xaml", UriKind.Relative));
        }

        private async void btnSettings_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.isAuthenticated)
            {
                #region Uso de mock data y descarga de informacion de facebook de los reporteros de un evento dado

                EventResult result = await MockData.MockDataGetEvents();
                if (Main.thereIsNoProblemo(result.status))
                    result.eventsList = EventsMethods.formatedList(result.eventsList);
                foreach (Event E in result.eventsList)
                {
                    MessageBox.Show(E.description);
                    E.reporters = await FacebookMethods.GetFbInfoForTheseReporters(E.reporters, App.facebookAccessToken);
                    string message = "Reportado por: ";
                    foreach (Reporter R in E.reporters)
                    {
                        message += R.name + " a las " + R.reportedAt + ". ";
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
    }
}