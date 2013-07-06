using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Zebra.WPApp.Resources;
using ZebrasLib.Classes;
using ZebrasLib.Places;

namespace Zebra.WPApp.Pages.Places
{
    public partial class CategoriesPage : PhoneApplicationPage
    {
        private bool comingBack;
        private GeoCoordinateWatcher watcher;

        public CategoriesPage()
        {
            InitializeComponent();
            this.Loaded += CategoriesPage_Loaded;
            comingBack = false;
            lstCategoryList.SelectionChanged += lstCategoryList_SelectionChanged;
            lstSearchResults.SelectionChanged += lstSearchResults_SelectionChanged;
            txtSearch.ActionIconTapped+=txtSearch_ActionIconTapped;
            watcher = new GeoCoordinateWatcher();
            watcher.MovementThreshold = 200;
            watcher.PositionChanged+=watcher_PositionChanged;
            watcher.StatusChanged += watcher_StatusChanged;
        }

        void lstSearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Place place = lstSearchResults.SelectedItem as Place;
            if (place != null)
            {
                staticClasses.selectedPlace = place;
                NavigationService.Navigate(new Uri("/Pages/Places/SelectedPlacePage.xaml?comingFrom=Search", UriKind.Relative));
            }
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    MessageBox.Show(AppResources.TxtGPSDisabled);
                    NavigationService.GoBack();
                    break;
                case GeoPositionStatus.NoData:
                    MessageBox.Show(AppResources.TxtGPSNoData);
                    NavigationService.GoBack();
                    break;
                default:
                    break;
            }
        }

        private void CategoriesPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!comingBack)
            {
                lstCategoryList.ItemsSource = DBPhone.CategoriesMethods.GetItems();
                comingBack = true;
            }
        }

        private void lstCategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            Category selectedcategory = lstCategoryList.SelectedItem as Category;
            if (selectedcategory != null)
            {
                if (selectedcategory.name != "")
                {
                    staticClasses.selectedCategory = selectedcategory;
                    NavigationService.Navigate(new Uri("/Pages/Places/PlacesPage.xaml", UriKind.Relative));
                    lstCategoryList.SelectedIndex = -1;
                }
            }
        }

        #region Search
        private void txtSearch_ActionIconTapped(object sender, EventArgs e)
        {
            lstSearchResults.ItemsSource = new List<Place>(); 
            lstSearchResults.Focus();
            if (txtSearch.Text.Length > 0)
            {
                //Don't touch. SERIOUSLY don't touch.
                //It's the only GeoCoordinateWatcher that won't work on the whole App.
                try
                {
                    watcher.Start();
                }
                catch (Exception)
                { /*Beg so this don't crash again (don't even know why)*/}
            }
            
            else MessageBox.Show(AppResources.TxtSearchFailed);
        }

        private async void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //double latitude = e.Position.Location.Latitude;
            //double longitude = e.Position.Location.Longitude;
            double latitude = -16.482936;
            double longitude = -68.121576;
            prgSearchProgress.Visibility = System.Windows.Visibility.Visible;
            try
            {
                List<Place> lstReturned = await PlacesMethods.getPlacesByQuery(txtSearch.Text, latitude, longitude);
                if (lstReturned != null)
                        lstSearchResults.ItemsSource = lstReturned;
                else MessageBox.Show(AppResources.TxtInternetConnectionProblem);
            }
            catch (Exception)
            { /*Don't worry, be happy.*/}
            finally
            {
                prgSearchProgress.Visibility = System.Windows.Visibility.Collapsed;
                watcher.Stop();
            }
        }
        #endregion
    }
}