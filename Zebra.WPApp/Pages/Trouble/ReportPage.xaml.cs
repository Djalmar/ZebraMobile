using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Zebra.WPApp.Resources;
using ZebrasLib;
using ZebrasLib.Classes;
using ZebrasLib.Events;
using Newtonsoft.Json;
namespace Zebra.WPApp.Pages.Trouble
{
    public partial class ReportPage : PhoneApplicationPage
    {
        private List<string> troubleCategoryList;
        public GeoCoordinateWatcher watcher { get; set; }
        public ProblemsResult result { get; set; }
        double latitude;
        double longitude;

        public ReportPage()
        {
            InitializeComponent();
            this.Loaded += ReportPage_Loaded;
            watcher = new GeoCoordinateWatcher();
            watcher.MovementThreshold = 200;
            watcher.PositionChanged += watcher_PositionChanged;
        }

        private void ReportPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAppBar();
            watcher.Start();
            LoadProblemsList();
        }

        #region AppBar
        private void LoadAppBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton btnShare = new ApplicationBarIconButton();
            btnShare.IconUri = new Uri("/Assets/AppBar/share.png",UriKind.Relative);
            btnShare.Text = AppResources.TxtShare + " n " + AppResources.TxtReport;
            btnShare.Click += btnShare_Click;
            ApplicationBar.Buttons.Add(btnShare);

            ApplicationBarIconButton btnReport = new ApplicationBarIconButton();
            btnReport.IconUri = new Uri("/Assets/AppBar/upload.png",UriKind.Relative);
            btnReport.Text = AppResources.TxtReport;
            btnReport.Click += btnReport_Click;
            ApplicationBar.Buttons.Add(btnReport);
        }

        void btnReport_Click(object sender, EventArgs e)
        {
            Report();
            NavigationService.GoBack();
        }

        private void Report()
        {
            ProblemsResult result = new ProblemsResult();
            string description = txtDescription.Text;
            int reportType = lspTroubleCategory.SelectedIndex + 1;
            if (description.Length > 0 && reportType > 0)
                ReportEvent(App.facebookId,
                    latitude,
                    longitude,
                    txtDescription.Text,
                    reportType);
        }
        async void ReportEvent(string facebookCode,
                    double latitude,
                    double longitude,
                    string description,
                    int type)
        { 
            string data =
                    "facebookcode=" + App.facebookId +
                    "&latitude=" + latitude +
                    "&longitude=" + longitude +
                    "&description=" + description +
                    "&type=" + type +
                    "&timezone=" + Main.GetValueFromTimeZone();
            string resultFromServer = await Zebra.Utilities.Internet.UploadStringAsyncUsingPUT(new Uri(Main.urlReportProblem,UriKind.Absolute), data);
            ProblemsResult result = JsonConvert.DeserializeObject<ProblemsResult>(resultFromServer);
            if (result != null)
            {
                if (result.status == "200")
                    MessageBox.Show(AppResources.TxtReportSucceded);
                else {
                    if (result.status == "400")
                        MessageBox.Show(AppResources.TxtReportRepeated);
                    else MessageBox.Show(AppResources.TxtReportFailed);
                }
            }
            else MessageBox.Show(AppResources.TxtInternetConnectionProblem);
        }

        void btnShare_Click(object sender, EventArgs e)
        {
            #region Report
            ProblemsResult result = new ProblemsResult();
            string description = txtDescription.Text;
            int reportType = lspTroubleCategory.SelectedIndex + 1;
            if (description.Length > 0 && reportType > 0)
                ReportEvent(App.facebookId,
                    latitude,
                    longitude,
                    txtDescription.Text,
                    reportType);
            #endregion
            #region Share
            ShareContent.title = AppResources.TxtShareImAt +" "+ lspTroubleCategory.SelectedItem.ToString();
            ShareContent.message = txtDescription.Text;
            ShareContent.link= new Uri("http://bing.com/maps/default.aspx" +
                "?cp=" + latitude + "~" + longitude+
                "&lvl=18" +
                "&style=r",UriKind.Absolute);

            NavigationService.Navigate(new Uri("/Pages/Share.xaml", UriKind.Relative));
            #endregion
            NavigationService.GoBack();
        }
        #endregion

        private void LoadProblemsList()
        {
            troubleCategoryList = new List<string>();
            troubleCategoryList.Add("a Traffic jam");
            troubleCategoryList.Add("a Manifestation");
            troubleCategoryList.Add("a Parade");
            troubleCategoryList.Add("a Blockade");
            troubleCategoryList.Add("an Accident");
            troubleCategoryList.Add("something");
            lspTroubleCategory.ItemsSource = troubleCategoryList;
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            latitude = e.Position.Location.Latitude;
            longitude = e.Position.Location.Longitude;
            watcher.Stop();
        }
    }
}