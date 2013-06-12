using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ZebrasLib.Events;
using System.Device.Location;
using Zebra.WPApp.Resources;
using ZebrasLib.Classes;
using ZebrasLib;

namespace Zebra.WPApp.Pages.Trouble
{
    public partial class ReportPage : PhoneApplicationPage
    {

        List<string> troubleCategoryList;
        public GeoCoordinateWatcher watcher { get; set; }
        public EventResult result { get; set; }
        public ReportPage()
        {
            InitializeComponent();
            this.Loaded += ReportPage_Loaded;
        }

        void ReportPage_Loaded(object sender, RoutedEventArgs e)
        {
            //aqui el metodo que solo me retorna las categorias

            troubleCategoryList=new List<string>();
            troubleCategoryList.Add("Parade");
            troubleCategoryList.Add("Bloqueo");
            troubleCategoryList.Add("Parade");
            troubleCategoryList.Add("Bloqueo");
            lspTroubleCategory.ItemsSource = troubleCategoryList;
             
        }

        private void btnReportClick(object sender, EventArgs e)
        {
            StartGps();            
        }

        private void StartGps()
        {
            watcher = new GeoCoordinateWatcher();
            watcher.PositionChanged += watcher_PositionChanged;
            watcher.StatusChanged += watcher_StatusChanged;
            watcher.MovementThreshold = 300;
            watcher.Start();
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            //if (e.Status == GeoPositionStatus.Disabled)
            //    MessageBox.Show(AppResources.messageGpsDisabled);
            //if (e.Status == GeoPositionStatus.NoData)
            //    MessageBox.Show(AppResources.messageGpsNoData);
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            double latitude = e.Position.Location.Latitude;
            double longitude = e.Position.Location.Longitude;
            int type = lspTroubleCategory.SelectedIndex;
            string description = txtDescription.Text;
            //result = await EventsMethods.ReportEvent(App.facebookId, latitude, longitude, description, type);
            //if (Main.thereIsNoProblemo(result.status,result.message))
            //    MessageBox.Show("Se ha hecho el reporte");
            watcher.Stop();
        }

        private void btnShareClick(object sender, EventArgs e)
        {
            StartGps();
            #region share
            NavigationService.Navigate(new Uri("/Pages/OtherPages/Share.xaml?message="+txtDescription.Text+"&&title="+lspTroubleCategory.SelectedIndex, UriKind.Relative));
            #endregion
        }
    }
}