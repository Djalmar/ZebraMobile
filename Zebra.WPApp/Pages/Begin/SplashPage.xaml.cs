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
            btnContinue.Tap += btnContinue_Tap;
            btnReplay.Tap += btnReplay_Tap;
            this.Loaded += SplashPage_Loaded;
        }

        void SplashPage_Loaded(object sender, RoutedEventArgs e)
        {
            stbTxbTitle.Begin();
        }

        void SplashPage_Completed(object sender, EventArgs e)
        {
            txbTitle.Visibility = System.Windows.Visibility.Collapsed;
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
            //a ver si le metemos alguna animacion
        }

        void btnReplay_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            mdeVideo.Play();
            //podriamos revertir la animacion
            stkButtons.Visibility = System.Windows.Visibility.Collapsed;
        }

        void btnContinue_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //Preguntar al usuario si esta seguro de continuar
            var result = MessageBox.Show("Are you sure ??", "Continue", new MessageBoxButton());
            if (MessageBoxResult.OK == result)
            {
                NavigationService.Navigate(new Uri("/Pages/Begin/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
            }
        }

    }
}