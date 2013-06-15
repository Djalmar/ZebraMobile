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
using ZebrasLib.Events;
using ZebrasLib;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using Zebra.WPApp.UserControls;
namespace Zebra.WPApp.Pages.Trouble
{
    public partial class TroublesPage : PhoneApplicationPage
    {
        private List<Event> troublesList;
        public List<Event> TroublesList
        {
            get { return troublesList; }
            set { troublesList = value; }
        }
        private EventResult result;

        public EventResult Result
        {
            get { return result; }
            set { result = value; }
        }
        
        public TroublesPage()
        {
            InitializeComponent();
            this.Loaded += TroublesPage_Loaded;
        }

        async void TroublesPage_Loaded(object sender, RoutedEventArgs e)
        {
            result = await MockData.MockDataGetEvents();
            if (Main.thereIsNoProblemo(result.status))
            {
                troublesList = result.eventsList;
                lstTroubles.ItemsSource = troublesList;
            }
            LoadPushPins();
        }

        private void LoadPushPins()
        {
            MapLayer layers = new MapLayer();
            MapOverlay overlay;

            foreach (var item in result.eventsList)
            {
                
                overlay = new MapOverlay();
                uscPushPin pushPin = new uscPushPin(item);
                pushPin.txbCategory.Text = item.type + "";
                overlay.Content = pushPin;
                overlay.GeoCoordinate = new GeoCoordinate(item.latitude, item.longitude);
                layers.Add(overlay);
            }
            mapTroubles.Center = new GeoCoordinate(-16.482208, -68.123117);
            mapTroubles.Layers.Add(layers);
        }

        //REPORT
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Trouble/ReportPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {

        }
    }
}