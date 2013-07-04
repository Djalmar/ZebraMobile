using Microsoft.Phone.Controls;
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
            btnTraffic.Tap+=btnTraffic_Tap;
            btnPlaces.Tap+=btnPlaces_Tap;
            btnWallet.Tap+=btnWallet_Tap;
            btnZebra.Tap+=btnZebra_Tap;
            btnSettings.Tap+=btnSettings_Tap;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            btnTraffic.Notification = TileContent.trafficMessage;
            btnTraffic.DisplayNotification = true;
            btnTraffic.Message = "puzzzaw";
            btnWallet.Notification = TileContent.walletMessage;
            btnWallet.DisplayNotification = true;
            btnPlaces.Notification = TileContent.placeMessage;
            btnPlaces.DisplayNotification = true;
        }

        void btnTraffic_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Trouble/TroublesPage.xaml", UriKind.Relative));
        }

        void btnPlaces_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Places/CategoriesPage.xaml", UriKind.Relative));
        }

        private void btnWallet_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Wallet/GetDataPage.xaml", UriKind.Relative));
        }

        private void btnZebra_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Begin/AboutZebritas.xaml", UriKind.Relative));
        }

        private void btnSettings_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Begin/SettingsPage.xaml", UriKind.Relative));
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            if (NavigationService.CanGoBack)
                while (NavigationService.RemoveBackEntry() != null)
                    NavigationService.RemoveBackEntry();
        }
    }
}