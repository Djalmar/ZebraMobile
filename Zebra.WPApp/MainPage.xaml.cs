using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Zebra.WPApp.Resources;
using System.Windows.Media.Animation;

namespace Zebra.WPApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        public bool FirstTimeLoaded { get; set; }
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            FirstTimeLoaded = true;
            (Resources["stbZebraWalking"] as Storyboard).Completed += MainPage_Completed;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            (Resources["stbZebraWalking"] as Storyboard).Begin();
            
        }

        void MainPage_Completed(object sender, EventArgs e)
        {
            if (FirstTimeLoaded)
            {
                if (!App.FirstTimeLaunch)
                {
                    //comienza la animacion y navegamos a instrucciones y settings
                    NavigationService.Navigate(new Uri("/Pages/Begin/SplashPage.xaml", UriKind.RelativeOrAbsolute));
                    App.FirstTimeLaunch = true;
                }
                else
                {
                    NavigationService.Navigate(new Uri("/Pages/Begin/MenuPage.xaml", UriKind.RelativeOrAbsolute));
                }
            }
            FirstTimeLoaded = false;
        }
        
        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}