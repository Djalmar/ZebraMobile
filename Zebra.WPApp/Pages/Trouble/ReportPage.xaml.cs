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
            btnShare.Text = AppResources.TxtShare;
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
                    "&type=" + type;
            string resultFromServer = await UploadStringAsyncUsingPUT(new Uri(Main.urlReportProblem,UriKind.Absolute), data);
            ProblemsResult result = JsonConvert.DeserializeObject<ProblemsResult>(resultFromServer);
            if (result != null)
            {
                if (result.status == "200")
                    MessageBox.Show(AppResources.TxtReportSucceded);
                if (result.status == "400")
                    MessageBox.Show(AppResources.TxtReportRepeated);
                else MessageBox.Show(AppResources.TxtReportFailed);
            }
            else MessageBox.Show(AppResources.TxtInternetConnectionProblem);
            NavigationService.GoBack();
        }
        void btnShare_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/OtherPages/Share.xaml" +
                "?message=" + txtDescription.Text +
                "&title=" + lspTroubleCategory.SelectedIndex, UriKind.Relative));
        }
        #endregion

        private void LoadProblemsList()
        {
            troubleCategoryList = new List<string>();
            troubleCategoryList.Add("Traffic");
            troubleCategoryList.Add("Manifestation");
            troubleCategoryList.Add("Parade");
            troubleCategoryList.Add("Blockade");
            troubleCategoryList.Add("Accident");
            troubleCategoryList.Add("Others");
            lspTroubleCategory.ItemsSource = troubleCategoryList;
        }
        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            latitude = e.Position.Location.Latitude;
            longitude = e.Position.Location.Longitude;
            watcher.Stop();
        }

        public static Task<string> UploadStringAsyncUsingPUT(Uri uri, string data)
        {
            WebClient client = new SharpGIS.GZipWebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers["Accept"] = "*/*";
            client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.116 Safari/537.36";
            client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            client.Headers["Accept-Encoding"] = "gzip,deflate,sdch";
            client.Headers["Accept-Language"] = "en-US,en;q=0.8";
            var resultFromUpload = new TaskCompletionSource<string>();

            client.UploadStringCompleted += (s, e) =>
            {
                if (e.Error != null)
                    resultFromUpload.SetException(e.Error);
                else
                    resultFromUpload.SetResult(e.Result);
            };

            client.UploadStringAsync(uri, "POST", data);

            return resultFromUpload.Task;
        }
    }
}