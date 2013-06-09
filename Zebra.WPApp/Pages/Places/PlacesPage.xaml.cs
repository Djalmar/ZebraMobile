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
        List<myCategory> bindingList;
        public PlacesPage()
        {
            InitializeComponent();
            this.Loaded += PlacesPage_Loaded;
            result = new PlacesResult();
            lstSubCategpries = new List<Category>();
            lstPlaces = new List<Place>();
            bindingList = new List<myCategory>();
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
                    myCategory categoria = new myCategory();
                    categoria.Categoryy = subCategory;
                    var query = from variable in lstPlaces where variable.categoryCode.Equals(subCategory.code) select variable;
                    categoria.PlacesList = new List<Place>();
                    foreach (var item in query)
                    {
                        categoria.PlacesList.Add(item);
                    }
                    bindingList.Add(categoria);
                }
                lstPopular.ItemsSource = bindingList;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txbCategory.Title = NavigationContext.QueryString["category"];
        }
    }
    class myCategory
    {
        private Category category;

        public Category Categoryy
        {
            get { return category; }
            set { category = value; }
        }
        
        private List<Place> placesList;

        public List<Place> PlacesList
        {
            get { return placesList; }
            set { placesList = value; }
        }
        public myCategory()
        {

        }
    }
}