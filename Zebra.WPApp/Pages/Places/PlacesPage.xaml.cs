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
using ZebrasLib.Places;
using ZebrasLib;
namespace Zebra.WPApp.Pages.Places
{
    public partial class PlacesPage : PhoneApplicationPage
    {
        PlacesResult result;
        List<Place> lstPlaces;
        public PlacesPage()
        {
            InitializeComponent();
            this.Loaded += PlacesPage_Loaded;
            result = new PlacesResult();
            lstPlaces = new List<Place>();
        }

        void PlacesPage_Loaded(object sender, RoutedEventArgs e)
        {
            result = PlacesMethods.MockDataGetPlaces();
            if (Main.thereIsNoProblemo(result.status, result.message))
            {
                lstPlaces = result.placesList;
                lstbAllPlaces.ItemsSource = lstPlaces;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txbCategory.Title = NavigationContext.QueryString["category"];
        }
    }
}