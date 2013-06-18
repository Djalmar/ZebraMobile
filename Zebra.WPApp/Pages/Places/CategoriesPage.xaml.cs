using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using ZebrasLib.Places;
using ZebrasLib.Classes;
namespace Zebra.WPApp.Pages.Places
{
    public partial class CategoriesPage : PhoneApplicationPage
    {
        public CategoriesPage()
        {
            InitializeComponent();
            this.Loaded += CategoriesPage_Loaded;
            lstCategoryList.SelectionChanged += lstCategoryList_SelectionChanged;
        }

        async void CategoriesPage_Loaded(object sender, RoutedEventArgs e)
        {
            lstCategoryList.ItemsSource = await MockData.MockDataGetCategories();   
        }

        void lstCategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category selectedcategory = lstCategoryList.SelectedItem as Category;

            if (selectedcategory != null)
                if (selectedcategory.name != "")
                    NavigationService.Navigate(new Uri("/Pages/Places/PlacesPage.xaml?category=" + selectedcategory.name, UriKind.Relative));
        }

        private void PhoneTextBox_ActionIconTapped(object sender, EventArgs e)
        {
            //SEARCH PLACE
        }
    }
}