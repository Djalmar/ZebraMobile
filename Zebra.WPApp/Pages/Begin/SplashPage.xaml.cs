using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Zebra.WPApp.Pages.Begin
{
    public partial class SplashPage : PhoneApplicationPage
    {
        public SplashPage()
        {
            InitializeComponent();
            mdeVideo.MediaEnded += mdeVideo_MediaEnded;
        }

        void mdeVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Begin/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}