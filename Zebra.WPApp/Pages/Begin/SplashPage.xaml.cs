using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace Zebra.WPApp.Pages.Begin
{
    public partial class SplashPage : PhoneApplicationPage
    {
        public SplashPage()
        {
            InitializeComponent();
            stbTxbTitle.Completed += SplashPage_Completed;
            mdeVideo.MediaEnded += mdeVideo_MediaEnded;
            this.Loaded += SplashPage_Loaded;
            
        }

        private void SplashPage_Loaded(object sender, RoutedEventArgs e)
        {
            stbTxbTitle.Begin();
        }

        private void SplashPage_Completed(object sender, EventArgs e)
        {
            txbTitle.Visibility = System.Windows.Visibility.Collapsed;
            mdeVideo.Play();
        }

        private void mdeVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Begin/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}