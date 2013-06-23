using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Zebra.WPApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            (Resources["stbCrossings"] as Storyboard).Completed += MainPage_Completed;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //la animacion va a tener un tiempo de duracion aleatorio
            (Resources["stbCrossings"] as Storyboard).Begin();
        }

        private void MainPage_Completed(object sender, EventArgs e)
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