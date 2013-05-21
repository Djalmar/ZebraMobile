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
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            (Resources["stbZebraWalking"] as Storyboard).Completed += MainPage_Completed;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //la animacion va a tener un tiempo de duracion aleatorio
            (Resources["stbZebraWalking"] as Storyboard).Begin();
        }
        void MainPage_Completed(object sender, EventArgs e)
        {
            if (!App.FirstTimeLaunch)
            {
                //comienza la animacion y navegamos a instrucciones y settings
                NavigationService.Navigate(new Uri("/Pages/Begin/SplashPage.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/Begin/MenuPage.xaml", UriKind.RelativeOrAbsolute));
            }
        }
    }
}