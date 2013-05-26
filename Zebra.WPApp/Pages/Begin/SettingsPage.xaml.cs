using Microsoft.Phone.Controls;
using System;
using System.Windows.Navigation;
using ZebrasLib;
using ZebrasLib.Facebook;

namespace Zebra.WPApp.Pages.Begin
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void brdNextButton_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //podriamos considerar preguntarle al usuario si esta seguro de continuar, ademas avisarle q podra cambiar esto en
            //el menu de configuraciones

            NavigationService.Navigate(new Uri("/Pages/Begin/MenuPage.xaml", UriKind.RelativeOrAbsolute));
            //aqui recien decimos q el usuario entro por primera vez a la app
            App.FirstTimeLaunch = true;

            //tenemos que guardar la configuracion que el usuario a ingresado -> IsoaltedStorge
        }

        private async void Border_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            prgLoginFacebook.Visibility = System.Windows.Visibility.Collapsed;
            bool isAuthenticated = await FacebookMethods.canAuthenticate();
            if (isAuthenticated)
            {
                App.isAuthenticated = true;
                App.facebookAccessToken = Main.AccessToken;
                App.facebookId = Main.FacebookId;
                NavigationService.GoBack();
            }
            prgLoginFacebook.Visibility = System.Windows.Visibility.Visible;
            //cambiar el texto a ya esta logueado o algo asi
        }
    }
}