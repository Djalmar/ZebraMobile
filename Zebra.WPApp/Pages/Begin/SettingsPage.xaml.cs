using Microsoft.Phone.Controls;
using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using ZebrasLib;
using ZebrasLib.Places;
using ZebrasLib.Classes;
using System.Collections.Generic;
using OurFacebook;
using Zebra.WPApp.Resources;
using DBPhone;
using System.Device.Location;
namespace Zebra.WPApp.Pages.Begin
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        double latitude;
        double longitude;
        GeoCoordinateWatcher watcher;
        List<Category> categories;
        public SettingsPage()
        {
            InitializeComponent();

            lstCategories.SelectionMode = SelectionMode.Multiple;
            borderFacebook.Tap += borderFacebook_Tap;
            borderNextButton.Tap += borderNextButton_Tap;
            tglSwitchDistanceUnit.Checked += tglSwitchDistanceUnit_Checked;
            tglSwitchDistanceUnit.Unchecked += tglSwitchDistanceUnit_Unchecked;
            tglSwitchDownloadSetting.Checked += tglSwitchDownloadSetting_Checked;
            tglSwitchDownloadSetting.Unchecked += tglSwitchDownloadSetting_Unchecked;
            watcher = new GeoCoordinateWatcher();
            watcher.MovementThreshold = 200;
            watcher.PositionChanged += watcher_PositionChanged;
            this.Loaded += SettingsPage_Loaded;
        }

        async void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            List<ZebrasLib.Classes.Place> lstDownloadedPlaces;
            lstDownloadedPlaces = await PlacesMethods.getAllPlacesFromThisCategories(categories, 
                e.Position.Location.Latitude, 
                e.Position.Location.Longitude);
            DBPhone.Methods.AddPlaces(lstDownloadedPlaces);
        }

        void tglSwitchDownloadSetting_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            tglSwitchDownloadSetting.Content = "Manual";
            txtDownloadPlacesDetail.Text = AppResources.TxtbDownloadPlacesManual;
        }

        void tglSwitchDownloadSetting_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

            tglSwitchDownloadSetting.Content = "Auto";
            txtDownloadPlacesDetail.Text = AppResources.TxtbDownloadPlacesAuto;
        }

        #region Toggle
        void tglSwitchDistanceUnit_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            tglSwitchDistanceUnit.Content = AppResources.TxtbMiles;
        }

        void tglSwitchDistanceUnit_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            tglSwitchDistanceUnit.Content = AppResources.TxtbKilometers;
        }
        #endregion

        async void borderNextButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (pivotMain.SelectedIndex == 2)
            {
                stackWait.Visibility = System.Windows.Visibility.Visible;
                pivotMain.Visibility = System.Windows.Visibility.Collapsed;
                #region Download Places
                if (lstCategories.SelectedItems.Count > 0)
                {
                    categories = lstCategories.SelectedItems as List<Category>;
                    if (categories.Count > 0)
                        watcher.Start();
                }
                #endregion

                #region SaveSettings
                if (tglSwitchDistanceUnit.IsChecked == true)
                    App.usesKilometers = true;
                else App.usesKilometers = false;

                if (tglSwitchDownloadSetting.IsChecked == true)
                    App.AutoDownloadsPlaces = true;

                else {
                    App.AutoDownloadsPlaces = false;
                    App.FirstTimeDataBase = true;
                }

                App.nearDistance = sldNearDistance.Value;
                if (sldNearDistance.Value == 0)
                    App.nearDistance++;

                App.FirstTimeLaunch = true;
                #endregion
                NavigationService.Navigate(new Uri("/Pages/Begin/MenuPage.xaml", UriKind.Relative));

            }
            else pivotMain.SelectedIndex++;
        }

        private async void SettingsPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lstCategories.ItemsSource = await MockData.MockDataGetCategories();
            lstCategories.DisplayMemberPath = "name";
        }

        private async void borderFacebook_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            prgLoginFacebook.Visibility = System.Windows.Visibility.Collapsed;
            bool isAuthenticated = await FacebookMethods.canAuthenticate();
            if (isAuthenticated)
            {
                App.isAuthenticated = true;
                App.facebookAccessToken = Main.AccessToken;
                App.facebookId = Main.FacebookId;
            }
            txtbLoggedIn.Visibility = System.Windows.Visibility.Visible;
        }
    }
}