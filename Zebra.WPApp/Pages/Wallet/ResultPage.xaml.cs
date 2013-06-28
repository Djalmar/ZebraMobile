using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ZebrasLib.Classes;
using ZebrasLib.Wallet;
using System.Device.Location;
namespace Zebra.WPApp.Pages.Wallet
{
    public partial class ResultPage : PhoneApplicationPage
    {
        int maxmoney, minmoney, people;
        string content, categorie;
        List<Place> lstPlace;
        double latitude;
        double longitude;
        GeoCoordinateWatcher watcher;
        public ResultPage()
        {
            InitializeComponent();
            lstPlace = new List<Place>();
            watcher = new GeoCoordinateWatcher();
            watcher.MovementThreshold = 200;
            watcher.PositionChanged+=watcher_PositionChanged;
        }

        async void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            latitude = e.Position.Location.Latitude;
            longitude = e.Position.Location.Longitude;

            if (NavigationContext.QueryString.TryGetValue("content", out content))
                lstPlace = await WalletMethods.getPlacesBetweenAndQuery(minmoney, maxmoney, latitude, longitude, content);
            else
                if (NavigationContext.QueryString.TryGetValue("categorie", out categorie))
                    lstPlace = await WalletMethods.getPlacesBetween(maxmoney, minmoney,
                        //-16.5001360633125,-68.1174392219843
                        latitude, longitude
                        ,categorie);
                        
            if (lstPlace != null)
                lstResults.ItemsSource = lstPlace;
            //else There was a big problemo jefe
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            minmoney = int.Parse(NavigationContext.QueryString["minmoney"]);
            maxmoney = int.Parse(NavigationContext.QueryString["maxmoney"]);
            people = int.Parse(NavigationContext.QueryString["people"]);
            minmoney = minmoney / people;
            maxmoney = maxmoney / people;

            watcher.Start();
        }
    }
}