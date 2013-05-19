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