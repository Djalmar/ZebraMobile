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
            LoadMap();
            LoadPlace();
            LoadRelatedPlaces();
        }

        private void LoadRelatedPlaces()
        {
            lstRelatedByFeatures.ItemsSource = DBPhone.PlacesMethods.getRelatedPlacesBasedOn(staticClasses.selectedPlace.minPrice,
                staticClasses.selectedPlace.maxPrice,
                staticClasses.selectedPlace.categoryCode,
                staticClasses.selectedPlace.code);

            if (lstRelatedByFeatures.Items.Count == 0)
            {
                //lstRelatedByFeatures.Visibility = Visibility.Collapsed;
                //txtNoPlacesRelatedByFeature.Text = AppResources.TxtNoPlacesRelated;
                //txtNoPlacesRelatedByFeature.Visibility = Visibility.Visible;
            }
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
            uscPushPin pushPin = new uscPushPin();
            MapLayer layer = new MapLayer();
            MapOverlay overlay = new MapOverlay();
            overlay.Content = pushPin;
            overlay.GeoCoordinate = new GeoCoordinate(staticClasses.selectedPlace.latitude, staticClasses.selectedPlace.longitude);
            layer.Add(overlay);
            mapPlace.Layers.Add(layer);
            mapPlace.Center = new GeoCoordinate(-16.5013, -68.1207);
            mapPlace.ZoomLevel = 14;
        }

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
                NavigationService.Navigate(new Uri("/Pages/Places/SelectedPage.xaml", UriKind.Relative));
            }
        }

        private void lstRelatedByFeatures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Place place = lstRelatedByFeatures.SelectedItem as Place;
            if (place != null)
            {
                staticClasses.selectedPlace = place;
                NavigationService.Navigate(new Uri("/Pages/Places/SelectedPage.xaml", UriKind.Relative));
            }
        } 
        
        public class Service
        {
            public string Name { get; set; }
            public bool Exist { get; set; }
        }
    }
}