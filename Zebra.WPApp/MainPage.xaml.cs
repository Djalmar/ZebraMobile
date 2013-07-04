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
            (Resources["stbCrossings"] as Storyboard).Begin();
        }

        private void MainPage_Completed(object sender, EventArgs e)
        {
            if(!App.FirstTimeLaunch)
                NavigationService.Navigate(new Uri("/Pages/Begin/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
            else NavigationService.Navigate(new Uri("/Pages/Begin/MenuPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}