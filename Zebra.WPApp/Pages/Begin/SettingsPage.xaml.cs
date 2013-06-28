using Microsoft.Phone.Controls;
using OurFacebook;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Windows.Controls;
using System.Windows.Navigation;
using Zebra.WPApp.Resources;
using System.Windows;
using ZebrasLib;

namespace Zebra.WPApp.Pages.Begin
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        private bool categoriesDownloaded, isLoggedOnFacebook, firstTime;
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
            firstTime = true;
            watcher = new GeoCoordinateWatcher();
            watcher.MovementThreshold = 200;
            watcher.PositionChanged += watcher_PositionChanged;
            this.Loaded += SettingsPage_Loaded;
        }

        private async void SettingsPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            categories = await ZebrasLib.Places.PlacesMethods.getCategories();
            if (categories != null)
            {
                if (firstTime)
                {
                    DBPhone.CategoriesMethods.AddItems(categories);
                    firstTime = false;
                }
                categoriesDownloaded = true;
                lstCategories.ItemsSource = categories;
            }
            else {
                txtInternetError.Visibility = Visibility.Visible;
                txtInternetError.Text = AppResources.TxtInternetConnectionProblem;
                lstCategories.Visibility = Visibility.Collapsed;
            }
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

                #endregion SaveSettings
                if (lstCategories.SelectedItems.Count > 0)
                {
                    selectedCategories = new List<ZebrasLib.Classes.Category>();
                    foreach (Object SelectedItem in lstCategories.SelectedItems)
                        selectedCategories.Add(SelectedItem as ZebrasLib.Classes.Category);
                    if (selectedCategories.Count > 0)
                        watcher.Start();
                }
                else FinishSettings();
            }
            else pivotMain.SelectedIndex++;
        }

        private void FinishSettings()
        {
            if (categoriesDownloaded)
            {
                if (isLoggedOnFacebook)
                {
                    App.FirstTimeLaunch = true;
                    NavigationService.Navigate(new Uri("/Pages/Begin/MenuPage.xaml", UriKind.Relative));
                    txtbLoggedIn.Text = AppResources.TxbFacebookLogin;
                }
                else
                {
                    stackWait.Visibility = System.Windows.Visibility.Collapsed;
                    pivotMain.Visibility = System.Windows.Visibility.Visible;
                    txtbLoggedIn.Text = AppResources.TxtFacebookNeeded;
                }
            }
            else
            {
                txtbLoggedIn.Text = AppResources.TxtInternetConnectionProblem;
                stackWait.Visibility = System.Windows.Visibility.Collapsed;
                pivotMain.Visibility = System.Windows.Visibility.Visible;
            }
                
            
        }

        private async void borderFacebook_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            txtbLoggedIn.Visibility = System.Windows.Visibility.Visible;
            prgLoginFacebook.Visibility = System.Windows.Visibility.Collapsed;
            bool isAuthenticated = await FacebookMethods.canAuthenticate();
            if (isAuthenticated)
            {
                App.isAuthenticated = true;
                App.facebookAccessToken = Main.AccessToken;
                App.facebookId = Main.FacebookId;
                isLoggedOnFacebook = true;
                txtbLoggedIn.Text = AppResources.TxbFacebookLoggedIn;
            }
            else
            {
                txtbLoggedIn.Text = AppResources.TxtInternetConnectionProblem;
            }
        }

        private async void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            List<ZebrasLib.Classes.Place> lstDownloadedPlaces;
            lstDownloadedPlaces = await ZebrasLib.Places.PlacesMethods.getAllPlacesFromThisCategories(selectedCategories,
                e.Position.Location.Latitude,e.Position.Location.Longitude);
            if (lstDownloadedPlaces != null)
            {
                DBPhone.PlacesMethods.AddItems(lstDownloadedPlaces);
                FinishSettings();
            }
            else txtbLoggedIn.Text = AppResources.TxtInternetConnectionProblem;
            
            watcher.Stop();
        }

    }
}