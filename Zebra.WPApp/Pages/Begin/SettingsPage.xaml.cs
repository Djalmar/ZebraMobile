using Microsoft.Phone.Controls;
using OurFacebook;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Windows.Controls;
using System.Windows.Navigation;
using Zebra.WPApp.Resources;
using ZebrasLib;

namespace Zebra.WPApp.Pages.Begin
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        private GeoCoordinateWatcher watcher;
        private List<ZebrasLib.Classes.Category> categories;
        private List<ZebrasLib.Classes.Category> selectedCategories;

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

        private async void SettingsPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            categories = await ZebrasLib.Places.PlacesMethods.getCategories();
            DBPhone.CategoriesMethods.AddItems(categories);
            lstCategories.ItemsSource = categories;
            lstCategories.DisplayMemberPath = "name";
        }

        #region Toggle
        private void tglSwitchDownloadSetting_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            tglSwitchDownloadSetting.Content = "Manual";
            txtDownloadPlacesDetail.Text = AppResources.TxtbDownloadPlacesManual;
        }

        private void tglSwitchDownloadSetting_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            tglSwitchDownloadSetting.Content = "Auto";
            txtDownloadPlacesDetail.Text = AppResources.TxtbDownloadPlacesAuto;
        }

        private void tglSwitchDistanceUnit_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            tglSwitchDistanceUnit.Content = AppResources.TxtbMiles;
        }

        private void tglSwitchDistanceUnit_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            tglSwitchDistanceUnit.Content = AppResources.TxtbKilometers;
        }

        #endregion Toggle

        private void borderNextButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (pivotMain.SelectedIndex == 2)
            {
                stackWait.Visibility = System.Windows.Visibility.Visible;
                pivotMain.Visibility = System.Windows.Visibility.Collapsed;

                #region SaveSettings

                if (tglSwitchDistanceUnit.IsChecked == true)
                    App.usesKilometers = true;
                else App.usesKilometers = false;

                if (tglSwitchDownloadSetting.IsChecked == true)
                    App.AutoDownloadsPlaces = true;

                else
                {
                    App.AutoDownloadsPlaces = false;
                    App.FirstTimeDataBase = true;
                }

                App.nearDistance = sldNearDistance.Value;
                if (sldNearDistance.Value == 0)
                    App.nearDistance++;

                App.FirstTimeLaunch = true;

                #endregion SaveSettings

                #region Download Places and Categories

                if (lstCategories.SelectedItems.Count > 0)
                {
                    selectedCategories = new List<ZebrasLib.Classes.Category>();
                    foreach (Object SelectedItem in lstCategories.SelectedItems)
                        selectedCategories.Add(SelectedItem as ZebrasLib.Classes.Category);
                    if (selectedCategories.Count > 0)
                        watcher.Start();
                    
                }

                #endregion Download Places

                else
                    NavigationService.Navigate(new Uri("/Pages/Begin/MenuPage.xaml", UriKind.Relative));
            }
            else pivotMain.SelectedIndex++;
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

        private async void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            List<ZebrasLib.Classes.Place> lstDownloadedPlaces;
            lstDownloadedPlaces = await ZebrasLib.Places.PlacesMethods.getAllPlacesFromThisCategories(categories,
                -16.5013, -68.1207);
            DBPhone.PlacesMethods.AddItems(lstDownloadedPlaces);
            NavigationService.Navigate(new Uri("/Pages/Begin/MenuPage.xaml", UriKind.Relative));
        }

    }
}