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
        public List<Problem> lstEventsByFriends { get; set; }
        ApplicationBarIconButton btnReport;
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
            watcher.StatusChanged += watcher_StatusChanged;
            lstTroubles.SelectionChanged+=lstTroubles_SelectionChanged;
            lstTroublesByFriends.SelectionChanged += lstTroublesByFriends_SelectionChanged;
        }

        private void TroublesPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAppBar();
            watcher.Start();
            GetFriendsProblems();
        }

        private async void GetFriendsProblems()
        {
            List<facebookUser> facebookFriends = await OurFacebook.FacebookMethods.downloadFriendsList(App.facebookAccessToken);
            List<string> friendsCodes = new List<string>();
            foreach (facebookUser user in facebookFriends)
                friendsCodes.Add(user.id);
            
            lstEventsByFriends = await ProblemsMethods.GetProblems(friendsCodes);
            lstEventsByFriends= await OurFacebook.FacebookMethods.GetFbInfoForTheseReporters(lstEventsByFriends, App.facebookAccessToken);
            lstTroublesByFriends.ItemsSource = lstEventsByFriends;
        }

        #region AppBar
        private void LoadAppBar()
        {
            ApplicationBar = new ApplicationBar();
            btnReport = new ApplicationBarIconButton();
            btnReport.IconUri = new Uri("/Assets/AppBar/upload.png",UriKind.Relative);
            btnReport.Text = AppResources.TxtReport;
            btnReport.Click += btnReport_Click;
            ApplicationBar.Buttons.Add(btnReport);
        }

        void btnReport_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Trouble/ReportPage.xaml", UriKind.Relative));
        }
        #endregion

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    SetTheWorldOnFire(AppResources.TxtGPSDisabled);
                    break;
                case GeoPositionStatus.NoData:
                    SetTheWorldOnFire(AppResources.TxtGPSNoData);
                    break;
                default:
                    break;
            }
        }

        private async void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            latitude = e.Position.Location.Latitude;
            longitude = e.Position.Location.Longitude;
            prgEvents.Visibility = System.Windows.Visibility.Visible;
            lstEvents = await ProblemsMethods.GetProblems(latitude,longitude);
            if (lstEvents != null)
            {
                lstEvents = await OurFacebook.FacebookMethods.GetFbInfoForTheseReporters(lstEvents, App.facebookAccessToken);
                LoadPushPins();
            }
            else
                SetTheWorldOnFire(AppResources.TxtInternetConnectionProblem);
            prgEvents.Visibility = System.Windows.Visibility.Collapsed;
            
        }

        private void SetTheWorldOnFire(string status)
        {
            panProblems.Visibility = Visibility.Collapsed;
            txtNoInternet.Visibility = Visibility.Visible;
            txtNoInternet.Text = status;
            btnReport.IsEnabled = false;
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
            mapTroubles.Center = new GeoCoordinate(latitude,longitude);
            mapTroubles.Layers.Add(layers);
            lstTroubles.ItemsSource = lstEvents;
        }

        private void lstTroubles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            NavigateToReportersPage(lstTroubles);
        }

        void lstTroublesByFriends_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            NavigateToReportersPage(lstTroublesByFriends);
        }

        private void NavigateToReportersPage(System.Windows.Controls.ListBox lstTroubles)
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