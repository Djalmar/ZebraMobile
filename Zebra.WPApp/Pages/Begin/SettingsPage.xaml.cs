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
        private bool categoriesDownloaded, isLoggedOnFacebook;
        private GeoCoordinateWatcher watcher;
        private List<ZebrasLib.Classes.Category> categories;
        private List<ZebrasLib.Classes.Category> selectedCategories;
        private double latitude;
        private double longitude;
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
            watcher.Start();
            categories = await ZebrasLib.Places.PlacesMethods.getCategories();
            if (categories != null)
            {
                categoriesDownloaded = true;
                lstCategories.ItemsSource = categories;
            }
            else {
                txtInternetError.Visibility = Visibility.Visible;
                txtInternetError.Text = AppResources.TxtInternetConnectionProblem;
                txtbLoggedIn.Text = AppResources.TxtInternetConnectionProblem;
                lstCategories.Visibility = Visibility.Collapsed;
            }

            if (App.FirstTimeLaunch)
            {
                borderNextButton.Visibility = Visibility.Collapsed;
                if (categories != null)
                {
                    txtbLoggedIn.Text = AppResources.TxbFacebookLoggedIn;
                    isLoggedOnFacebook = true;
                }
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
                txtbLoggedIn.Text = AppResources.TxtInternetConnectionProblem;
        }

        private void borderNextButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (pivotMain.SelectedIndex == 2)
            {
                stackWait.Visibility = System.Windows.Visibility.Visible;
                pivotMain.Visibility = System.Windows.Visibility.Collapsed;

                SaveSettings();
                SaveCategories();
            }
            else pivotMain.SelectedIndex++;
        }

        private void SaveSettings()
        {
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

            App.Popularity = (int)sldPopularity.Value * 2;
            if (sldNearDistance.Value == 0)
                App.Popularity++;

            App.nearDistance = sldNearDistance.Value;
            if (sldNearDistance.Value == 0)
                App.nearDistance++;
        }
        private void SaveCategories()
        {
            if (lstCategories.SelectedItems.Count > 0)
            {
                selectedCategories = new List<ZebrasLib.Classes.Category>();
                foreach (Object SelectedItem in lstCategories.SelectedItems)
                    selectedCategories.Add(SelectedItem as ZebrasLib.Classes.Category);

                DBPhone.Context.DisposeDataBase();
                DBPhone.Context.RemoveDatabase();
                DBPhone.CategoriesMethods.AddItems(categories);
   
                SavePlaces();   
            }
            else FinishSettings();
        }
        private async void SavePlaces()
        {
            List<ZebrasLib.Classes.Place> lstDownloadedPlaces;
            lstDownloadedPlaces = await ZebrasLib.Places.PlacesMethods.getAllPlacesFromThisCategories(selectedCategories,
                latitude, longitude,App.usesKilometers);
            if (lstDownloadedPlaces != null)
            {
                DBPhone.PlacesMethods.AddItems(lstDownloadedPlaces);
                FinishSettings();
            }
            else txtbLoggedIn.Text = AppResources.TxtInternetConnectionProblem;
        }
        private void FinishSettings()
        {
            if (!App.FirstTimeLaunch)
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
            else stackWait.Visibility = System.Windows.Visibility.Collapsed;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            stackWait.Visibility = System.Windows.Visibility.Visible;
            SaveSettings();
            SaveCategories();
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //latitude = e.Position.Location.Latitude;
            //longitude = e.Position.Location.Longitude;
            latitude = -16.482936;
            longitude = -68.121576;
            watcher.Stop();
        }
    }
}