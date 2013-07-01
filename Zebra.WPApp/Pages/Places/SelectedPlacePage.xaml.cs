﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Zebra.WPApp;
using Zebra.WPApp.UserControls;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using ZebrasLib.Classes;
using Zebra.WPApp.Resources;
using System.Globalization;
using ZebrasLib;
using ZebrasLib.Classes;
using Newtonsoft.Json;
using System.Threading.Tasks;
namespace Zebra.WPApp.Pages.Places
{
    public partial class SelectedPlacePage : PhoneApplicationPage
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public GeoCoordinateWatcher watcher { get; set; }
        public int Cantidad { get; set; }
        public SelectedPlacePage()
        {
            InitializeComponent();
            this.Loaded += SelectedPlacePage_Loaded;
            watcher = new GeoCoordinateWatcher();
            watcher.PositionChanged += watcher_PositionChanged;
            watcher.StatusChanged += watcher_StatusChanged;
        }

        void SelectedPlacePage_Loaded(object sender, RoutedEventArgs e)
        {
            watcher.Start();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string comingFrom = NavigationContext.QueryString["comingFrom"];
            if (comingFrom == "Search")
            {
                panItemFeatures.Visibility = Visibility.Collapsed;
                panItemPrices.Visibility = Visibility.Collapsed;
            }
        }
        private void LoadRelatedPlaces()
        { 
            lstRelatedByFeatures.ItemsSource = null;
            lstRelatedByFeatures.Visibility = System.Windows.Visibility.Visible;
            txtNoPlacesRelatedByFeature.Visibility = System.Windows.Visibility.Collapsed;
            lstRelatedByFeatures.ItemsSource = ZebrasLib.Places.PlacesMethods.getDistancesForEachPlace(Latitude,
                Longitude, 
                (
                    DBPhone.PlacesMethods.getRelatedPlacesBasedOn(staticClasses.selectedPlace.minPrice,
                    staticClasses.selectedPlace.maxPrice,
                    staticClasses.selectedPlace.parentCategoryCode,
                    staticClasses.selectedPlace.code)
                    ));

            if (lstRelatedByFeatures.Items.Count == 0)
            {
                lstRelatedByFeatures.Visibility = Visibility.Collapsed;
                txtNoPlacesRelatedByFeature.Text = AppResources.TxtNoPlacesRelated;
                txtNoPlacesRelatedByFeature.Visibility = Visibility.Visible;
            }
            lstRelatedByPrices.ItemsSource = null;
            lstRelatedByPrices.Visibility = System.Windows.Visibility.Visible;
            txtNoPlacesRelatedByPrice.Visibility = System.Windows.Visibility.Collapsed;
            lstRelatedByPrices.ItemsSource = ZebrasLib.Places.PlacesMethods.getDistancesForEachPlace(Latitude,
                Longitude,
                ( 
                    DBPhone.PlacesMethods.getRelatedPlacesBasedOn(staticClasses.selectedPlace.kidsArea,
                    staticClasses.selectedPlace.smokingArea,
                    staticClasses.selectedPlace.parentCategoryCode,
                    staticClasses.selectedPlace.code)
                    ));
            if (lstRelatedByPrices.Items.Count == 0)
            {
                lstRelatedByPrices.Visibility = Visibility.Collapsed;
                txtNoPlacesRelatedByPrice.Text = AppResources.TxtNoPlacesRelated;
                txtNoPlacesRelatedByPrice.Visibility = Visibility.Visible;
            }
        }

        private void LoadPlace()
        {
            panorama.DataContext = staticClasses.selectedPlace;
            List<Service> listaServicios = CrearListadeServicios();
            txbFeatures.Visibility = System.Windows.Visibility.Visible;
            var query = from variable in listaServicios where variable.Exist.Equals("Visible") select variable;
            var lista = query.ToList();
            if (lista.Count!=0)
                lstFeatures.ItemsSource = listaServicios;
            else
                txbFeatures.Visibility = System.Windows.Visibility.Collapsed;
            run.Text = System.Globalization.RegionInfo.CurrentRegion.CurrencySymbol;
            run2.Text = System.Globalization.RegionInfo.CurrentRegion.CurrencySymbol;
        }

        private void LoadMap()
        {
            uscPushPinPlace pushPin = new uscPushPinPlace();
            MapLayer layer = new MapLayer();
            MapOverlay overlay = new MapOverlay();
            MapOverlay myPosition = new MapOverlay();
            uscUserPosition myPush = new uscUserPosition();
            myPosition.Content = myPush;
            myPosition.GeoCoordinate = new GeoCoordinate(Latitude, Longitude);
            overlay.Content = pushPin;
            overlay.GeoCoordinate = new GeoCoordinate(staticClasses.selectedPlace.latitude, staticClasses.selectedPlace.longitude);
            layer.Add(overlay);
            layer.Add(myPosition);
            mapPlace.Layers.Add(layer);
            mapPlace.Center = new GeoCoordinate(staticClasses.selectedPlace.latitude, staticClasses.selectedPlace.longitude);
            mapPlace.ZoomLevel = 14;
        }
       
        #region GPS
        private void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
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

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Latitude = e.Position.Location.Latitude;
            Longitude = e.Position.Location.Longitude;
            LoadMap();
            LoadPlace();
            LoadRelatedPlaces();
            watcher.Stop();
        }
        #endregion


        private List<Service> CrearListadeServicios()
        {
            return new List<Service>()
            {
                new Service(){Name=AppResources.TxtParking,Exist=staticClasses.selectedPlace.parking.ToString()},
                new Service(){Name=AppResources.TxtDelivery,Exist=staticClasses.selectedPlace.delivery.ToString()},
                new Service(){Name=AppResources.TxtHolidays,Exist=staticClasses.selectedPlace.holidays.ToString()},
                new Service(){Name=AppResources.TxtSmoking,Exist=staticClasses.selectedPlace.smokingArea.ToString()},
                new Service(){Name=AppResources.TxtKids,Exist=staticClasses.selectedPlace.kidsArea.ToString()}
            };
        }
        private void lstRelatedByPrices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Place place = lstRelatedByPrices.SelectedItem as Place;
            if (place != null)
            {
                staticClasses.selectedPlace = place;
                mapPlace.Layers.Clear();
                this.SelectedPlacePage_Loaded(sender, e);
            }
        }

        private void lstRelatedByFeatures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Place place = lstRelatedByFeatures.SelectedItem as Place;
            if (place != null)
            {
                staticClasses.selectedPlace = place;
                mapPlace.Layers.Clear();
                this.SelectedPlacePage_Loaded(sender, e);
            }
        } 

        /// <summary>
        /// Method you can use to rate places
        /// </summary>
        /// <param name="placeCode">Just the place code, </param>
        /// <param name="rating">Remember that now rating is an INTEGER, not a float, not a double</param>
        /// <returns>Returns a bool to say if the rating was succesfull, so you should display an mbox for the user saying 
        /// what happened, if it was succesful or not.</returns>
        public static async Task<bool> tryRatePlace(string placeCode, int rating)
        {
            string data = "code="+placeCode+
                "&rating="+rating;
            string downloadedString = await Zebra.Utilities.Internet.UploadStringAsyncUsingPUT(new Uri(ZebrasLib.Main.urlPlacesRate, UriKind.Absolute), data);
            PlacesResult result = JsonConvert.DeserializeObject<PlacesResult>(downloadedString);
            if (result != null)
            {
                if (result.status == "200")
                    return true;
                else return false;
            }
            return false;
        }
        
        public class Service
        {
            public string Name { get; set; }
            private string exists;

            public string Exist
            {
                get { return exists; }
                set 
                {
                    if (value.Equals("True"))
                        exists = "Visible";
                    else
                        exists = "Collapsed";
                }
            }
        }
    }
}