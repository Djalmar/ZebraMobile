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
        private GeoCoordinateWatcher watcher;
        List<Place> lstAllPlaces;
        double latitude;
        double longitude;
        bool comingBack;
        string categoryCode;
        public PlacesPage()
        {
            InitializeComponent();
            watcher = new GeoCoordinateWatcher();
            watcher.MovementThreshold = 200;
            latitude = 150;
            longitude = 150;
            watcher.StatusChanged += watcher_StatusChanged;
            watcher.PositionChanged += watcher_PositionChanged;
            comingBack = false;
            prgPlaces.Visibility = System.Windows.Visibility.Visible;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txbCategory.Title = staticClasses.selectedCategory.name;
            categoryCode = staticClasses.selectedCategory.code;
            if(!comingBack)
            {
                LoadAppBar();
                lstAllPlaces = await DownloadOrGetPlacesFromDataBase();
                watcher.Start();
                comingBack = true;
            }
            prgPlaces.Visibility = System.Windows.Visibility.Collapsed;
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
            prgPlaces.IsIndeterminate = false;
            prgPlaces.IsIndeterminate = true;
            prgPlaces.Visibility = System.Windows.Visibility.Visible;
            lstAllPlaces = await DownloadPlacesFromTheInternet();
            prgPlaces.Visibility = System.Windows.Visibility.Collapsed;
            watcher.Start();
        }

        private async Task<List<Place>>DownloadOrGetPlacesFromDataBase()
        {
            if (App.AutoDownloadsPlaces)
                return await DownloadPlacesFromTheInternet();
            else
            {
                return DBPhone.PlacesMethods.GetItems(DBPhone.CategoriesMethods.GetSubCategoriesCodes(categoryCode));
            }
        }

        private async Task<List<Place>> DownloadPlacesFromTheInternet()
        {
            List<Place> lstFromTheInternet = await PlacesMethods.getAllPlacesByCategory(categoryCode, -16.5013, -68.1207);
            if (lstFromTheInternet.Count>0)
            {
                UpdateDataBase(lstFromTheInternet);
                return lstFromTheInternet;
            }
            return null;
        }

        private void UpdateDataBase(List<Place> toAddList)
        {   
            DBPhone.PlacesMethods.RemoveItems(categoryCode);
            DBPhone.PlacesMethods.AddItems(toAddList);
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            latitude = e.Position.Location.Latitude;
            longitude = e.Position.Location.Longitude;
            if (lstAllPlaces != null)
            {
                if (lstAllPlaces.Count > 0)
                {
                    lstAllPlaces = PlacesMethods.getDistancesForEachPlace(-16.5013, -68.1207, lstAllPlaces);
                    PopulateLists(lstAllPlaces);
                }
            }
            watcher.Stop();
        }

        private void PopulateLists(List<Place> lstAllPlaces)
        {
            lstbAllPlaces.ItemsSource = getDajaCategories(lstAllPlaces);
            lstbPopularPlaces.ItemsSource = getDajaCategories(PlacesMethods.getPlacesOrderedByPopularity(lstAllPlaces));
            if (latitude != 150)
            {
                lstbNearPlaces.ItemsSource = getDajaCategories(PlacesMethods.getPlacesNear(lstAllPlaces,App.nearDistance));
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

        private List<bindingCategory> getDajaCategories(List<Place> lstPlaces)
        {
            List<bindingCategory> lstCategoriesDaja = new List<bindingCategory>();
            List<Category> lstSubCategories = staticClasses.selectedCategory.subCategories;
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