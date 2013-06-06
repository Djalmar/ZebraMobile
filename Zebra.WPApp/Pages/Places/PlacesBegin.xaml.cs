using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

namespace Zebra.WPApp.Pages.Places
{
    public partial class PlacesBegin : PhoneApplicationPage
    {
        public PlacesBegin()
        {
            InitializeComponent();
            this.Loaded += PlacesBegin_Loaded;
        }
        void PlacesBegin_Loaded(object sender, RoutedEventArgs e)
        {
            addPLace("/images/Icons/restaurant.png", "Restaurants");
            addPLace("/images/Icons/internet.png", "Internet");
            addPLace("/images/Icons/coffee.png", "Coffees");
            addPLace("/images/Icons/night.png", "Night Clubs");
            addPLace("/images/Icons/cine.png", "Movies");
            addPLace("/images/Icons/sports.png", "Sports");
            addPLace("/images/Icons/build.png", "Hotels");
            addPLace("/images/Icons/hospital.png", "Hospitals");
            addPLace("/images/Icons/banc.png", "Banks");
            addPLace("/images/Icons/transport.png", "Bus Terminal");
            
        }
        public void addPLace(string icon, string category)
        {
            Place pl = new Place();
            BitmapImage bi = new BitmapImage(new Uri(icon, UriKind.Relative));
            pl.imgIcon.Source = bi;
            pl.Tap += imgIcon_Tap;
            pl.txtCategory.Text = category;
            stpItems.Children.Add(pl);
        }

        void imgIcon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Place aux = (Place)sender;
            NavigationService.Navigate(new Uri("/Pages/Places/CategoryDetaill.xaml?category=" + aux.txtCategory.Text,UriKind.Relative));
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
    }
}