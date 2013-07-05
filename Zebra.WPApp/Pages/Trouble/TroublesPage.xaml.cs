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
using ZebrasLib;
using ZebrasLib.Classes;
using ZebrasLib.Events;
using System.Linq;
namespace Zebra.WPApp.Pages.Trouble
{
    public partial class TroublesPage : PhoneApplicationPage
    {
        public List<Problem> lstEvents { get; set; }
        public List<Problem> lstEventsByFriends { get; set; }
        ApplicationBarIconButton btnReport;
        ApplicationBarIconButton btnAR;
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
        }

        private async void GetFriendsProblems()
        {
            List<facebookUser> facebookFriends = await OurFacebook.FacebookMethods.downloadFriendsList(App.facebookAccessToken);
            List<string> friendsCodes = new List<string>();
            foreach (facebookUser user in facebookFriends)
                friendsCodes.Add(user.id);
            
            lstEventsByFriends = await ProblemsMethods.GetProblems(friendsCodes, Main.GetValueFromTimeZone());
            if (lstEventsByFriends.Count > 0)
            {
                lstEventsByFriends = await OurFacebook.FacebookMethods.GetFbInfoForTheseReporters(lstEventsByFriends, App.facebookAccessToken);
                lstTroublesByFriends.ItemsSource = lstEventsByFriends;
            }
                
            else {
                lstTroublesByFriends.Visibility = Visibility.Collapsed;
                txtNoReportsTodayByFriends.Visibility = Visibility.Visible;
                txtNoReportsTodayByFriends.Text = AppResources.TxtTroublesNoReportsByFriends;
            }
            
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

            btnAR = new ApplicationBarIconButton();
            btnAR.IconUri = new Uri("/Assets/AppBar/AR.png", UriKind.Relative);
            btnAR.Text = AppResources.TxtReport;
            btnAR.Click += btnAR_Click;
            ApplicationBar.Buttons.Add(btnAR);
        }

        void btnAR_Click(object sender, EventArgs e)
        {
            if (lstEvents.Count > 0)
            {
                staticClasses.lstGartItems = new System.Collections.ObjectModel.ObservableCollection<GART.Data.ARItem>();
                foreach (Problem P in lstEvents)
                {
                    staticClasses.lstGartItems.Add(
                        new GARTItem
                        {
                            Name = P.reporters.First().name,
                            Icon = P.reporters.First().picture,
                            Content = P.description,
                            GeoLocation = new GeoCoordinate(P.latitude, P.longitude),
                        }
                    );
                }
                NavigationService.Navigate(new Uri("/Pages/AR.xaml", UriKind.Relative));
            }
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
            lstEvents = await ProblemsMethods.GetProblems(latitude,longitude, Main.GetValueFromTimeZone());
            if (lstEvents != null)
            {
                LoadPushPins();
                lstEvents = await OurFacebook.FacebookMethods.GetFbInfoForTheseReporters(lstEvents, App.facebookAccessToken);
                GetFriendsProblems(); 
                if (lstEvents.Count == 0)
                {
                    txtNoReportsToday.Text = AppResources.TxtTroublesNoReports;
                    lstTroubles.Visibility = Visibility.Collapsed;
                    txtNoReportsToday.Visibility = Visibility.Visible;

                    lstTroublesByFriends.Visibility = Visibility.Collapsed;
                    txtNoReportsTodayByFriends.Visibility = Visibility.Visible;
                    txtNoReportsTodayByFriends.Text = AppResources.TxtTroublesNoReports;   
                }
            }
            else
                SetTheWorldOnFire(AppResources.TxtInternetConnectionProblem);
            prgEvents.Visibility = System.Windows.Visibility.Collapsed;
            watcher.Stop();
            
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

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            if (lstTroubles != null)
            {
                int troublesCount = lstTroubles.Items.Count;
                if (troublesCount > 0)
                {
                    string name = (lstTroubles.Items.Last() as Problem).reporters.First().name;
                    TileContent.trafficMessage = troublesCount.ToString() + " " + AppResources.TxtTroublesOcurrence + "\n" +
                        AppResources.TxtTroublesReportedBy + "\n" + name;
                }
            }
                
        }
    }
}