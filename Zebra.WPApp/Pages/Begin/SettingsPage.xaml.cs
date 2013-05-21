using Microsoft.Phone.Controls;
using System;
using System.Windows.Navigation;

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
        }
    }
}