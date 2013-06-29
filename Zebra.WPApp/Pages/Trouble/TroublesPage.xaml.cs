using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Windows;
using System.Windows.Navigation;
using Zebra.WPApp.Pages.Places;
using Zebra.WPApp.Resources;
using Zebra.WPApp.UserControls;
using ZebrasLib.Classes;
using ZebrasLib.Events;

namespace Zebra.WPApp.Pages.Trouble
{
    public partial class TroublesPage : PhoneApplicationPage
    {
        public List<Problem> lstEvents { get; set; }

        private GeoCoordinateWatcher watcher;
        private double latitude;
        private double longitude;

        public TroublesPage()
        {
            InitializeComponent();
            this.Loaded += TroublesPage_Loaded;
            watcher = new GeoCoordinateWatcher();
            watcher.MovementThreshold = 200;
            watcher.PositionChanged += watcher_PositionChanged;
        }

        private void TroublesPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAppBar();
            watcher.Start(); 
        }

        #region AppBar
        private void LoadAppBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton btnReport = new ApplicationBarIconButton();
            btnReport.IconUri = new Uri("/Assets/AppBar/check.png",UriKind.Relative);
            btnReport.Text = AppResources.TxtReport;
            btnReport.Click += btnReport_Click;
            ApplicationBar.Buttons.Add(btnReport);
            
        }

        void btnReport_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Trouble/ReportPage.xaml", UriKind.Relative));
        }
        #endregion

        private async void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            latitude = e.Position.Location.Latitude;
            longitude = e.Position.Location.Longitude;
            prgEvents.Visibility = System.Windows.Visibility.Visible;
            lstEvents = await ProblemsMethods.GetProblems(latitude,longitude);
            lstEvents = await OurFacebook.FacebookMethods.GetFbInfoForTheseReporters(lstEvents, App.facebookAccessToken);
            prgEvents.Visibility = System.Windows.Visibility.Collapsed;
            if (lstEvents != null)
                LoadPushPins();
            else MessageBox.Show(AppResources.TxtInternetConnectionProblem);
        }

        private void LoadPushPins()
        {
            MapLayer layers = new MapLayer();
            MapOverlay overlay;

            foreach (var item in lstEvents)
            {
                overlay = new MapOverlay();
                uscPushPin pushPin = new uscPushPin(item);
                pushPin.txbCategory.Text = item.type + "";
                overlay.Content = pushPin;
                overlay.GeoCoordinate = new GeoCoordinate(item.latitude, item.longitude);
                layers.Add(overlay);
            }
            mapTroubles.CartographicMode = MapCartographicMode.Hybrid;
            mapTroubles.Center = new GeoCoordinate(-16.482208, -68.123117);
            mapTroubles.Layers.Add(layers);
            lstTroubles.ItemsSource = lstEvents;
        }

        private void lstTroubles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Problem problem = lstTroubles.SelectedItem as Problem;
            if (problem != null)
            {
                staticClasses.selectedEvent = problem;
                NavigationService.Navigate(new Uri("/Pages/Trouble/EventReportersPage.xaml", UriKind.Relative));
                lstTroubles.SelectedItem = null;
                lstTroubles.SelectedIndex = -1;
            }
        }
    }
}