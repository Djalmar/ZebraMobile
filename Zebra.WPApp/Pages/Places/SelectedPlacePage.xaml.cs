using System;
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

namespace Zebra.WPApp.Pages.Places
{
    public partial class SelectedPlacePage : PhoneApplicationPage
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public GeoCoordinateWatcher watcher { get; set; }
        public SelectedPlacePage()
        {
            InitializeComponent();
            this.Loaded += SelectedPlacePage_Loaded;
            watcher = new GeoCoordinateWatcher();
        }

        void SelectedPlacePage_Loaded(object sender, RoutedEventArgs e)
        {
            watcher.Start();
            LoadPlace();
            LoadRelatedPlaces();
            watcher.PositionChanged += watcher_PositionChanged;
            watcher.StatusChanged += watcher_StatusChanged;
        }

        private void LoadRelatedPlaces()
        { 
            lstRelatedByFeatures.ItemsSource = null;
            lstRelatedByFeatures.Visibility = System.Windows.Visibility.Visible;
            txtNoPlacesRelatedByFeature.Visibility = System.Windows.Visibility.Collapsed;
            lstRelatedByFeatures.ItemsSource = DBPhone.PlacesMethods.getRelatedPlacesBasedOn(staticClasses.selectedPlace.minPrice,
                staticClasses.selectedPlace.maxPrice,
                staticClasses.selectedPlace.categoryCode,
                staticClasses.selectedPlace.code);

            if (lstRelatedByFeatures.Items.Count == 0)
            {
                lstRelatedByFeatures.Visibility = Visibility.Collapsed;
                txtNoPlacesRelatedByFeature.Text = AppResources.TxtNoPlacesRelated;
                txtNoPlacesRelatedByFeature.Visibility = Visibility.Visible;
            }
            lstRelatedByPrices.ItemsSource = null;
            lstRelatedByPrices.Visibility = System.Windows.Visibility.Visible;
            txtNoPlacesRelatedByPrice.Visibility = System.Windows.Visibility.Collapsed;
            lstRelatedByPrices.ItemsSource = DBPhone.PlacesMethods.getRelatedPlacesBasedOn(staticClasses.selectedPlace.kidsArea,
                staticClasses.selectedPlace.smokingArea,
                staticClasses.selectedPlace.categoryCode,
                staticClasses.selectedPlace.code);
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
            lstFeatures.ItemsSource = listaServicios;
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
            mapPlace.Center = new GeoCoordinate(staticClasses.selectedPlace.latitude,staticClasses.selectedPlace.longitude);
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
            watcher.Stop();
        }
        #endregion


        private List<Service> CrearListadeServicios()
        {
            return new List<Service>()
            {
                new Service(){Name="delivery",Exist=staticClasses.selectedPlace.delivery},
                new Service(){Name="holidays",Exist=staticClasses.selectedPlace.holidays},
                new Service(){Name="kids area",Exist=staticClasses.selectedPlace.kidsArea},
                new Service(){Name="delivery",Exist=staticClasses.selectedPlace.delivery}
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
        
        public class Service
        {
            public string Name { get; set; }
            public bool Exist { get; set; }
        }
    }
}