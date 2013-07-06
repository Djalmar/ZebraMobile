using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Zebra.WPApp.Resources;
using Zebra.WPApp.UserControls;
using ZebrasLib.Classes;

namespace Zebra.WPApp.Pages.Places
{
    public partial class SelectedPlacePage : PhoneApplicationPage
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public GeoCoordinateWatcher watcher { get; set; }

        public int Cantidad { get; set; }

        public List<Place> lstByPrice { get; set; }

        public List<Place> lstByFeatures { get; set; }

        public SelectedPlacePage()
        {
            InitializeComponent();
            this.Loaded += SelectedPlacePage_Loaded;
            watcher = new GeoCoordinateWatcher();
            watcher.PositionChanged += watcher_PositionChanged;
            watcher.StatusChanged += watcher_StatusChanged;
            mapPlace.Tap += mapPlace_Tap;
        }

        private void mapPlace_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MapsDirectionsTask task = new MapsDirectionsTask();
            task.Start = new LabeledMapLocation(AppResources.TxtPlacesYoureHere, new GeoCoordinate(Latitude, Longitude));
            task.End = new LabeledMapLocation(staticClasses.selectedPlace.name,
                new GeoCoordinate(
                    staticClasses.selectedPlace.latitude,
                    staticClasses.selectedPlace.longitude));

            task.Show();
        }

        private void SelectedPlacePage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAppBar();
            watcher.Start();
        }

        private void LoadAppBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton btnShare = new ApplicationBarIconButton();
            btnShare.IconUri = new Uri("/Assets/AppBar/Share.png", UriKind.Relative);
            btnShare.Text = AppResources.TxtShare;
            btnShare.Click += btnShare_Click;
            ApplicationBar.Buttons.Add(btnShare);

            ApplicationBarIconButton btnAR = new ApplicationBarIconButton();
            btnAR.IconUri = new Uri("/Assets/AppBar/AR.png", UriKind.Relative);
            btnAR.Text = AppResources.AR;
            btnAR.Click += btnAR_Click;
            ApplicationBar.Buttons.Add(btnAR);
        }

        private void btnAR_Click(object sender, EventArgs e)
        {
            List<Place> lstAR = new List<Place>();
            IEnumerable<Place> lstDistincts;
            lstAR.AddRange(lstByFeatures);
            lstAR.AddRange(lstByPrice);
            lstDistincts = lstAR.Distinct();
            lstAR = lstDistincts.ToList();
            if (lstAR.Count > 0)
            {
                staticClasses.lstGartItems = new System.Collections.ObjectModel.ObservableCollection<GART.Data.ARItem>();
                foreach (Place P in lstAR)
                {
                    staticClasses.lstGartItems.Add(
                        new GARTItem
                        {
                            Name = P.name,
                            Icon = staticClasses.selectedCategory.icon,
                            Content = P.address,
                            GeoLocation = new GeoCoordinate(P.latitude
                                , P.longitude),
                        }
                    );
                }
                NavigationService.Navigate(new Uri("/Pages/AR.xaml", UriKind.Relative));
            }
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            ShareContent.title = AppResources.TxtShareImAt + " " + staticClasses.selectedPlace.name;
            ShareContent.message = AppResources.TxtShareSomebody + "\n" +
                AppResources.TxtSharePlaceIts + " " + staticClasses.selectedPlace.address;
            ShareContent.link = new Uri("http://bing.com/maps/default.aspx" +
                "?cp=" + staticClasses.selectedPlace.latitude + "~" + staticClasses.selectedPlace.longitude +
                "&lvl=18" +
                "&style=r", UriKind.Absolute);

            NavigationService.Navigate(new Uri("/Pages/Share.xaml", UriKind.Relative));
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
            #region Features

            lstRelatedByFeatures.ItemsSource = null;
            lstRelatedByFeatures.Visibility = System.Windows.Visibility.Visible;
            txtNoPlacesRelatedByFeature.Visibility = System.Windows.Visibility.Collapsed;
            lstByFeatures = ZebrasLib.Places.PlacesMethods.getDistancesForEachPlace(Latitude,
                Longitude,
                (
                   DBPhone.PlacesMethods.getRelatedPlacesBasedOn(
                    staticClasses.selectedPlace.kidsArea,
                    staticClasses.selectedPlace.smokingArea,
                    staticClasses.selectedPlace.parentCategoryCode,
                    staticClasses.selectedPlace.code)
                    ),App.usesKilometers);
            lstRelatedByFeatures.ItemsSource = lstByFeatures;
            if (lstRelatedByFeatures.Items.Count == 0)
            {
                lstRelatedByFeatures.Visibility = Visibility.Collapsed;
                txtNoPlacesRelatedByFeature.Text = AppResources.TxtNoPlacesRelated;
                txtNoPlacesRelatedByFeature.Visibility = Visibility.Visible;
            }

            #endregion Features

            lstRelatedByPrices.ItemsSource = null;
            lstRelatedByPrices.Visibility = System.Windows.Visibility.Visible;
            txtNoPlacesRelatedByPrice.Visibility = System.Windows.Visibility.Collapsed;
            lstByPrice = ZebrasLib.Places.PlacesMethods.getDistancesForEachPlace(Latitude,
                Longitude,
                (
                 DBPhone.PlacesMethods.getRelatedPlacesBasedOn(
                    staticClasses.selectedPlace.minPrice,
                    staticClasses.selectedPlace.maxPrice,
                    staticClasses.selectedPlace.parentCategoryCode,
                    staticClasses.selectedPlace.code)

                    ),App.usesKilometers);
            lstRelatedByPrices.ItemsSource = lstByPrice;
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
            if (lista.Count != 0)
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
            //Latitude = e.Position.Location.Latitude;
            //Longitude = e.Position.Location.Longitude;
            Latitude = -16.482936;
            Longitude = -68.121576;
            LoadMap();
            LoadPlace();
            LoadRelatedPlaces();
            watcher.Stop();
        }

        #endregion GPS

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
            string data = "code=" + placeCode +
                "&rating=" + rating;
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