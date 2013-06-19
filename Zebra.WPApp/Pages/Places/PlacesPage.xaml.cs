using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Zebra.WPApp.Resources;
using ZebrasLib;
using ZebrasLib.Classes;
using ZebrasLib.Places;

namespace Zebra.WPApp.Pages.Places
{
    public partial class PlacesPage : PhoneApplicationPage
    {
        private PlacesResult result;
        private GeoCoordinateWatcher watcher;
        private bool noProblemo;

        public PlacesPage()
        {
            InitializeComponent();
            this.Loaded += PlacesPage_Loaded;
            result = new PlacesResult();
            watcher = new GeoCoordinateWatcher();
            watcher.StatusChanged += watcher_StatusChanged;
            watcher.PositionChanged += watcher_PositionChanged;
        }

        private void PlacesPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAppBar();

            watcher.Start();
            watcher.MovementThreshold = 200;

            DownloadOrGetPlacesFromDataBase();
        }

        private void LoadAppBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton btnUpdatePlaces = new ApplicationBarIconButton();
            btnUpdatePlaces.IconUri = new Uri("/Assets/download.png", UriKind.Relative);
            btnUpdatePlaces.Text = AppResources.AppBarDownload;
            btnUpdatePlaces.Click += btnUpdatePlaces_Click;
            ApplicationBar.Buttons.Add(btnUpdatePlaces);
        }

        async void btnUpdatePlaces_Click(object sender, EventArgs e)
        {
            result = await MockData.MockDataGetPlaces();
            lstbAllPlaces.ItemsSource = await getDajaCategories(result.placesList);

            DBPhone.Methods.RemovePlaces();
            DBPhone.Methods.AddPlaces(result.placesList);
        }

        private async void DownloadOrGetPlacesFromDataBase()
        {
            if (App.ManuallyDownloadPlaces == false)
                result = await MockData.MockDataGetPlaces();
            else result.placesList = DBPhone.Methods.GetPlaces();

            lstbAllPlaces.ItemsSource = await getDajaCategories(result.placesList);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txbCategory.Title = NavigationContext.QueryString["category"];
        }

        private async void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            result = await MockData.MockDataGetPlaces();
            noProblemo = Main.thereIsNoProblemo(result.status);
            if (noProblemo)
            {
                result.placesList = PlacesMethods.getDistancesForEachPlace(
                    e.Position.Location.Latitude,
                    e.Position.Location.Longitude,
                    result.placesList);

                lstbAllPlaces.ItemsSource = await getDajaCategories(result.placesList);
                lstbPopularPlaces.ItemsSource = await getDajaCategories(PlacesMethods.getPlacesOrderedByPopularity(result.placesList));
                lstbNearPlaces.ItemsSource =
                    await getDajaCategories(PlacesMethods.getPlacesOrderedByDistance(
                        e.Position.Location.Latitude,
                        e.Position.Location.Longitude,
                        result.placesList));
            }
                
        }

        private void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    MessageBox.Show("GPS Disabled");
                    break;

                case GeoPositionStatus.NoData:
                    MessageBox.Show("No GPS Data");
                    break;

                default:
                    break;
            }
        }

        private void placeSelected(object sender, System.Windows.Input.GestureEventArgs e)
        {
            staticClasses.selectedPlace = (((sender as StackPanel).Tag) as Place);
            NavigationService.Navigate(new Uri("/Pages/Places/SelectedPlacePage.xaml",UriKind.Relative));
        }

        private async Task<List<bindingCategory>> getDajaCategories(List<Place> lstPlaces)
        {
            List<bindingCategory> lstCategoriesDaja = new List<bindingCategory>();
            List<Category> lstSubCategories = await MockData.MockDataGetSubCategories();
            bindingCategory categoryDaja;

            foreach (Category subCategory in lstSubCategories)
            {
                categoryDaja = new bindingCategory();
                categoryDaja.category = subCategory;
                var query = from variable
                            in lstPlaces
                            where variable.categoryCode.Equals(subCategory.code)
                            select variable;

                categoryDaja.lstPlaces = query.ToList();

                lstCategoriesDaja.Add(categoryDaja);
            }
            return lstCategoriesDaja;
        }
    }
}