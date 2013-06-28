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
            
            uscPushPin pushPin = new uscPushPin();
            MapLayer layer = new MapLayer();
            MapOverlay overlay = new MapOverlay();
            overlay.Content = pushPin;
            overlay.GeoCoordinate=new GeoCoordinate(staticClasses.selectedPlace.latitude, staticClasses.selectedPlace.longitude);
            layer.Add(overlay);
            mapPlace.Layers.Add(layer);
            mapPlace.Center = new GeoCoordinate(-16.5013, -68.1207);
            mapPlace.ZoomLevel = 14;
            staticClasses.selectedPlace.rating /= 2;
            panorama.DataContext = staticClasses.selectedPlace;
            List<Service> listaServicios=CrearListadeServicios();
            lstFeatures.ItemsSource = listaServicios;
            run.Text = System.Globalization.RegionInfo.CurrentRegion.CurrencySymbol;
            run2.Text = System.Globalization.RegionInfo.CurrentRegion.CurrencySymbol;
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

        
        public class Service
        {
            public string Name { get; set; }
            public bool Exist { get; set; }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            //panPlace.Title = staticClasses.selectedPlace.name;
            //txtLocationAddress.Text = staticClasses.selectedPlace.address;
            //txtLocationDistance.Text = staticClasses.selectedPlace.distance.ToString();
        }

        private void lstRelatedByPrices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Place place = lstRelatedByPrices.SelectedItem as Place;
            if (place != null)
            {
                staticClasses.selectedPlace = place;
                NavigationService.Navigate(new Uri("/Pages/Place/SelectedPage.xaml", UriKind.Relative));
            }
        }

        private void lstRelatedByFeatures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Place place = lstRelatedByFeatures.SelectedItem as Place;
            if (place != null)
            {
                staticClasses.selectedPlace = place;
                NavigationService.Navigate(new Uri("/Pages/Place/SelectedPage.xaml", UriKind.Relative));
            }
        }
    }
}