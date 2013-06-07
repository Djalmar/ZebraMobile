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
        List<Category> lstSubCategpries;
        List<Place> lstPlaces;
        public PlacesPage()
        {
            InitializeComponent();
            this.Loaded += PlacesPage_Loaded;
            result = new PlacesResult();
            lstSubCategpries = new List<Category>();
            lstPlaces = new List<Place>();
        }

        void PlacesPage_Loaded(object sender, RoutedEventArgs e)
        {
            result = PlacesMethods.MockDataGetPlaces();
            lstSubCategpries = PlacesMethods.MockDataGetSubCategories();
            if (Main.thereIsNoProblemo(result.status, result.message))
            {
                lstPlaces = result.placesList;
                foreach (var subCategory in lstSubCategpries)
                {
                    ExpanderView expander = new ExpanderView();
                    expander.Header = subCategory.name;
                    var query = from variable in lstPlaces where variable.categoryCode.Equals(subCategory.code) select variable;
                    foreach (var place in query)
                    {
                        expander.Items.Add(place);
                    }
                }
                //lstbAllPlaces.ItemsSource = lstPlaces;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txbCategory.Title = NavigationContext.QueryString["category"];
        }
    }
}