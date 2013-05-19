using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Animation;

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

        void SplashPage_Loaded(object sender, RoutedEventArgs e)
        {
            stbTxbTitle.Begin();
        }

        void SplashPage_Completed(object sender, EventArgs e)
        {
            mdeVideo.Play();
        }

        void mdeVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            stkButtons.Visibility = System.Windows.Visibility.Visible;
            stbStkButtons.Begin();
            stbStkButtons.Completed += stbStkButtons_Completed;
        }

        void stbStkButtons_Completed(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Begin/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}