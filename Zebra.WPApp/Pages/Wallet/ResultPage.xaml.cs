using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ZebrasLib.Classes;
using ZebrasLib.Wallet;
using System.Device.Location;
using Zebra.WPApp.Pages.Places;
namespace Zebra.WPApp.Pages.Wallet
{
    public partial class ResultPage : PhoneApplicationPage
    {
        int maxmoney, minmoney, people;
        string content, categorie;
        List<Place> lstPlace;
        double latitude;
        double longitude;
        GeoCoordinateWatcher watcher;
        string comingFrom;
        public ResultPage()
        {
            InitializeComponent();
            lstPlace = new List<Place>();
            lstResults.SelectionChanged+=lstResults_SelectionChanged;
            expFilters.Collapsed += expFilters_Collapsed;
            watcher = new GeoCoordinateWatcher();
            watcher.MovementThreshold = 200;
            watcher.PositionChanged+=watcher_PositionChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            prgResults.Visibility = System.Windows.Visibility.Visible; prgResults.Visibility = System.Windows.Visibility.Visible;

            minmoney = int.Parse(NavigationContext.QueryString["minmoney"]);
            maxmoney = int.Parse(NavigationContext.QueryString["maxmoney"]);
            people = int.Parse(NavigationContext.QueryString["people"]);
            minmoney = minmoney / people;
            maxmoney = maxmoney / people;
            comingFrom = (NavigationContext.QueryString["comingFrom"]);

            watcher.Start();
        }

        async void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //latitude = e.Position.Location.Latitude;
            //longitude = e.Position.Location.Longitude;
            latitude = -16.482936;
            longitude = -68.121576;
            if (NavigationContext.QueryString.TryGetValue("content", out content))
                lstPlace = await WalletMethods.getPlacesBetweenAndQuery(minmoney, maxmoney, latitude, longitude, content);
            else
                if (NavigationContext.QueryString.TryGetValue("categorie", out categorie))
                    lstPlace = await WalletMethods.getPlacesBetween(maxmoney, minmoney,
                        latitude, longitude,categorie);
            prgResults.Visibility = System.Windows.Visibility.Collapsed;            
            if (lstPlace != null)
                lstResults.ItemsSource = lstPlace;
            //else There was a big problemo jefe
        }

        private void lstResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Place place = lstResults.SelectedItem as Place;
            if (place != null)
            {
                staticClasses.selectedPlace = place;
                NavigationService.Navigate(new Uri("/Pages/Places/SelectedPlacePage.xaml?comingFrom="+comingFrom, UriKind.Relative));
                lstResults.SelectedItem = null;
                lstResults.SelectedIndex = -1;
            }
        }

        void expFilters_Collapsed(object sender, RoutedEventArgs e)
        {
            List<Place> partialList = lstPlace;

            if (cbxKidsArea.IsChecked ==true)
                partialList = GetKids(partialList);
            if (cbxSmokingArea.IsChecked ==true)
                partialList = GetSmokers(partialList);
            if (cbxParking.IsChecked ==true)
                partialList = GetParkers(partialList);
            if (cbxHolidays.IsChecked ==true)
                partialList = GetHolidays(partialList);
            if (cbxDelivery.IsChecked ==true)
                partialList = GetDeliveres(partialList);
                
            lstResults.ItemsSource = partialList.ToList();
        }

        private List<Place> GetSmokers(List<Place> lstPlace)
        {
            IEnumerable<Place> newList = from allPlaces
                                         in lstPlace
                                         where allPlaces.smokingArea == true
                                         select allPlaces;
            return newList.ToList();
        }

        private List<Place> GetParkers(List<Place> lstPlace)
        {
            IEnumerable<Place> newList = from allPlaces
                                         in lstPlace
                                         where allPlaces.parking == true
                                         select allPlaces;
            return newList.ToList();
        }

        private List<Place> GetDeliveres(List<Place> lstPlace)
        {
            IEnumerable<Place> newList = from allPlaces
                                         in lstPlace
                                         where allPlaces.delivery == true
                                         select allPlaces;
            return newList.ToList();
        }

        private List<Place> GetHolidays(List<Place> lstPlace)
        {
            IEnumerable<Place> newList = from allPlaces
                                         in lstPlace
                                         where allPlaces.holidays == true
                                         select allPlaces;
            return newList.ToList();
        }

        private List<Place> GetKids(List<Place> lstPlace)
        {
            IEnumerable<Place> newList = from allPlaces
                                         in lstPlace
                                         where allPlaces.kidsArea == true
                                         select allPlaces;
            return newList.ToList();
        }

    }
}