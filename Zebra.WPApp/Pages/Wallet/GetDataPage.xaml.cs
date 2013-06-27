using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Zebra.WPApp.Pages.Wallet
{
    public partial class GetDataPage : PhoneApplicationPage
    {
        public GetDataPage()
        {
            InitializeComponent();
            lspCategory.ItemsSource = DBPhone.CategoriesMethods.GetItems();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Wallet/ResultPage.xaml", UriKind.Relative));
        }
    }
}