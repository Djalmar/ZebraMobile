using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using ZebrasLib.Places;
using ZebrasLib.Classes;
using System.Windows.Input;
using System.Device.Location;
namespace Zebra.WPApp.Pages.Places
{
    public partial class CategoriesPage : PhoneApplicationPage
    {
        bool comingBack;
        GeoCoordinateWatcher watcher;
        public CategoriesPage()
        {
            InitializeComponent();
            this.Loaded += CategoriesPage_Loaded;
            comingBack = false;
            lstCategoryList.SelectionChanged += lstCategoryList_SelectionChanged;
            txtSearch.ActionIconTapped+=txtSearch_ActionIconTapped;
            watcher = new GeoCoordinateWatcher();
            watcher.MovementThreshold = 200;
            watcher.PositionChanged += watcher_PositionChanged;
        }

        async void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            double latitude =e.Position.Location.Latitude;
            double longitude = e.Position.Location.Longitude;
            lstSearchResults.ItemsSource = await PlacesMethods.getPlacesByQuery(txtSearch.Text, -16.5013, -68.1207);
            prgSearchProgress.Visibility = System.Windows.Visibility.Collapsed;
            //Cuando ya hayan mas datos se podra hacer la prueba con datos del GPS, por ahora solo con valores por Default
            //lstSearchResults.ItemsSource = await PlacesMethods.getPlacesByQuery(txtSearch.Text, latitude, longitude);
        }

        void txtSearch_ActionIconTapped(object sender, EventArgs e)
        {
            lstSearchResults.Focus();
            if (txtSearch.Text.Length > 0)
            {
                prgSearchProgress.Visibility = System.Windows.Visibility.Visible;
                watcher.Start();
            }
        }

        void CategoriesPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!comingBack)
            {
                lstCategoryList.ItemsSource = DBPhone.CategoriesMethods.GetItems();
                comingBack = true;
            }   
        }

        void lstCategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category selectedcategory = lstCategoryList.SelectedItem as Category;
            if (selectedcategory != null)
                if (selectedcategory.name != "")
                {
                    staticClasses.selectedCategory = selectedcategory;
                    NavigationService.Navigate(new Uri("/Pages/Places/PlacesPage.xaml", UriKind.Relative));
                }
        }

        private void lstSearchResults_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Place place = lstSearchResults.SelectedItem as Place;
            if (place != null)
            {
                staticClasses.selectedPlace = place;
                NavigationService.Navigate(new Uri("/Pages/Places/SelectedPlacePage.xaml", UriKind.Relative));
            }
        }
    }
}