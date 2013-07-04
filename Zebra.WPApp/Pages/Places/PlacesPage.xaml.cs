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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txbCategory.Title = staticClasses.selectedCategory.name;
            categoryCode = staticClasses.selectedCategory.code;
            prgPlaces.Visibility = System.Windows.Visibility.Collapsed;
            if(!comingBack)
            {
                LoadAppBar();
                watcher.Start();
                comingBack = true;
            }
        }

        #region AppBar
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
            lstAllPlaces = await DownloadPlacesFromTheInternet(latitude,longitude);
            if (lstAllPlaces != null)
            {
                lstAllPlaces = PlacesMethods.getDistancesForEachPlace(latitude, longitude, lstAllPlaces);
                PopulateLists(lstAllPlaces);
                UpdateDataBase(lstAllPlaces);
            }
            prgPlaces.Visibility = System.Windows.Visibility.Collapsed;
        }
        #endregion

        #region GPS
        private void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    MessageBox.Show(AppResources.TxtGPSDisabled);
                    NavigationService.GoBack();
                    break;

                case GeoPositionStatus.NoData:
                    MessageBox.Show(AppResources.TxtGPSNoData);
                    NavigationService.GoBack();
                    break;

                default:
                    break;
            }
        }

        private async void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            latitude = e.Position.Location.Latitude;
            longitude = e.Position.Location.Longitude;
            prgPlaces.Visibility = System.Windows.Visibility.Visible;
            lstAllPlaces = await DownloadOrGetPlacesFromDataBase();
            prgPlaces.Visibility = System.Windows.Visibility.Collapsed;
            if (lstAllPlaces != null)
            {
                lstAllPlaces = PlacesMethods.getDistancesForEachPlace(latitude,longitude, lstAllPlaces);
                PopulateLists(lstAllPlaces);
            }
            watcher.Stop();
        }
        #endregion

        private async Task<List<Place>>DownloadOrGetPlacesFromDataBase()
        {
            List<Place> lstToReturn = new List<Place>();
            if (App.AutoDownloadsPlaces)
            {
                prgPlaces.Visibility = System.Windows.Visibility.Visible;
                lstToReturn = await DownloadPlacesFromTheInternet(latitude, longitude);
                prgPlaces.Visibility = System.Windows.Visibility.Collapsed;
                return lstToReturn;
            }
            else {
                lstToReturn = DBPhone.PlacesMethods.GetItems(DBPhone.CategoriesMethods.GetSubCategoriesCodes(categoryCode));
                if (lstToReturn != null)
                    return lstToReturn;
                else return null;
            }
        }

        private async Task<List<Place>> DownloadPlacesFromTheInternet(double latitude, double longitude)
        {
            List<Place> lstFromTheInternet = await PlacesMethods.getAllPlacesByCategory(categoryCode, latitude,longitude);
            if (lstFromTheInternet != null)
            {
                if (lstFromTheInternet.Count > 0)
                {
                    foreach (Place P in lstFromTheInternet)
                        P.parentCategoryCode = categoryCode;
                    UpdateDataBase(lstFromTheInternet);
                    return lstFromTheInternet;
                }
            }
            return null;
        }

        private void SetVisibilities(string message, Visibility forTheMessages, Visibility forTheLists)
        {
            txtNoPlacesFound.Text = message;
            txtNoNearPlacesFound.Text = message;
            txtNoPopularPlacesFound.Text = message;
            txtNoPlacesFound.Visibility = forTheMessages;
            txtNoNearPlacesFound.Visibility = forTheMessages;
            txtNoPopularPlacesFound.Visibility = forTheMessages;
            lstbAllPlaces.Visibility = forTheLists;
        }

        private void UpdateDataBase(List<Place> toAddList)
        {   
            DBPhone.PlacesMethods.RemoveItems(categoryCode);
            DBPhone.PlacesMethods.AddItems(toAddList);
        }

        private void PopulateLists(List<Place> lstAllPlaces)
        {
            List<Place> lstTempPlaces;
            lstbAllPlaces.ItemsSource = getDajaCategories(lstAllPlaces);
            lstbAllPlaces.Visibility = Visibility.Visible;
            txtNoPlacesFound.Visibility = Visibility.Collapsed;

            lstTempPlaces = PlacesMethods.getPlacesOrderedByPopularity(lstAllPlaces, App.Popularity);
            if (lstTempPlaces.Count > 0)
            {
                lstbPopularPlaces.ItemsSource = getDajaCategories(lstTempPlaces);
                txtNoPlacesFound.Visibility = Visibility.Collapsed;
                lstbPopularPlaces.Visibility = Visibility.Visible;
            }
            else
            {
                txtNoPopularPlacesFound.Text = AppResources.TxtNoPopularPlaces;
                txtNoPopularPlacesFound.Visibility = Visibility.Visible;
                lstbPopularPlaces.Visibility = Visibility.Collapsed;
            } 

            lstTempPlaces = PlacesMethods.getPlacesNear(lstAllPlaces,App.nearDistance);
            if (lstTempPlaces.Count > 0)
            {
                lstbNearPlaces.ItemsSource = getDajaCategories(lstTempPlaces);
                lstbNearPlaces.Visibility = Visibility.Visible;
                txtNoNearPlacesFound.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtNoNearPlacesFound.Text = AppResources.TxtNoNearPlaces;
                lstbNearPlaces.Visibility = Visibility.Collapsed;
                txtNoNearPlacesFound.Visibility = Visibility.Visible;
            } 
        }

        private void placeSelected(object sender, System.Windows.Input.GestureEventArgs e)
        {
            staticClasses.selectedPlace = (((sender as StackPanel).Tag) as Place);
            NavigationService.Navigate(new Uri("/Pages/Places/SelectedPlacePage.xaml?comingFrom=Selected", UriKind.Relative));

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