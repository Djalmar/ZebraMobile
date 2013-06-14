using Microsoft.Phone.Controls;
using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using ZebrasLib;
using ZebrasLib.Places;
using ZebrasLib.Classes;
using System.Collections.Generic;
using OurFacebook;
namespace Zebra.WPApp.Pages.Begin
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            lstCategories.SelectionMode = SelectionMode.Multiple;
            borderFacebook.Tap += borderFacebook_Tap;
            borderNextButton.Tap += borderNextButton_Tap;
            this.Loaded += SettingsPage_Loaded;
        }

        async void borderNextButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (pivotMain.SelectedIndex == 2)
            {
                List<Category> categories = lstCategories.SelectedItems as List<Category>;
                List<Place> lstDownloadedPlaces;
                if(categories.Count>0)
                    lstDownloadedPlaces = await PlacesMethods.DownloadAllPlacesFromThisCategories(categories);
                NavigationService.Navigate(new Uri("/Pages/Begin/MenuPage.xaml", UriKind.Relative));
            }
            else pivotMain.SelectedIndex++;
        }

        private async void SettingsPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lstCategories.ItemsSource = await PlacesMethods.MockDataGetCategories();
            lstCategories.DisplayMemberPath = "name";
        }

        private async void borderFacebook_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            prgLoginFacebook.Visibility = System.Windows.Visibility.Collapsed;
            bool isAuthenticated = await FacebookMethods.canAuthenticate();
            if (isAuthenticated)
            {
                App.isAuthenticated = true;
                App.facebookAccessToken = Main.AccessToken;
                App.facebookId = Main.FacebookId;
            }
            prgLoginFacebook.Visibility = System.Windows.Visibility.Visible;
            //cambiar el texto a ya esta logueado o algo asi
        }
    }
}