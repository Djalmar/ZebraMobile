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
        List<Place> lstAllPlaces;
        double latitude;
        double longitude;
        bool comingBack;
        public PlacesPage()
        {
            InitializeComponent();
            result = new PlacesResult();
            watcher = new GeoCoordinateWatcher();
            watcher.MovementThreshold = 200;
            noProblemo = true;
            latitude = 150;
            longitude = 150;
            watcher.StatusChanged += watcher_StatusChanged;
            watcher.PositionChanged += watcher_PositionChanged;
            comingBack = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txbCategory.Title = NavigationContext.QueryString["category"];
            if(!comingBack)
            {
                LoadAppBar();
                DownloadOrGetPlacesFromDataBase();
                watcher.Start();
                comingBack = true;
            }
        }

        private void LoadAppBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton btnUpdatePlaces = new ApplicationBarIconButton();
            btnUpdatePlaces.IconUri = new Uri("/Assets/AppBar/download.png", UriKind.Relative);
            btnUpdatePlaces.Text = AppResources.AppBarDownload;
            btnUpdatePlaces.Click += btnUpdatePlaces_Click;
            ApplicationBar.Buttons.Add(btnUpdatePlaces);
        }

        private async void btnUpdatePlaces_Click(object sender, EventArgs e)
        {
            lstAllPlaces = await DownloadPlacesFromTheInternet();
            watcher.Start();
        }

        private async void DownloadOrGetPlacesFromDataBase()
        {
            if (App.AutoDownloadsPlaces)
                lstAllPlaces = await DownloadPlacesFromTheInternet();
            else
                lstAllPlaces = DBPhone.Methods.GetPlaces();
        }

        private async Task<List<Place>> DownloadPlacesFromTheInternet()
        {
            result = await MockData.MockDataGetPlaces();
            noProblemo = Main.thereIsNoProblemo(result.status);
            if (noProblemo)
            {
                UpdateDataBase(result.placesList);
                return result.placesList;
            }
            return null;
        }

        private void UpdateDataBase(List<Place> list)
        {
            DBPhone.Methods.RemovePlaces();
            DBPhone.Methods.AddPlaces(list);
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            latitude = e.Position.Location.Latitude;
            longitude = e.Position.Location.Longitude;
            if (lstAllPlaces.Count > 0)
            {
                lstAllPlaces = PlacesMethods.getDistancesForEachPlace(latitude, longitude, lstAllPlaces);
                PopulateLists(lstAllPlaces);
            }
            watcher.Stop();
        }

        private async void PopulateLists(List<Place> lstAllPlaces)
        {
            lstbAllPlaces.ItemsSource = await getDajaCategories(lstAllPlaces);
            lstbPopularPlaces.ItemsSource = await getDajaCategories(PlacesMethods.getPlacesOrderedByPopularity(lstAllPlaces));
            if (latitude != 150)
            {
                lstbNearPlaces.ItemsSource =
                    await getDajaCategories(PlacesMethods.getPlacesOrderedByDistance(latitude,longitude,lstAllPlaces));
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
            NavigationService.Navigate(new Uri("/Pages/Places/SelectedPlacePage.xaml", UriKind.Relative));
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