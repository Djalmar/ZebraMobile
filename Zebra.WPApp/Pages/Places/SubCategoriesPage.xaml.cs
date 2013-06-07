using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ZebrasLib.Classes;
using System.Windows.Media.Imaging;
using ZebrasLib.Places;

namespace Zebra.WPApp.Pages.Places
{
    public partial class SubCategoriesPage : PhoneApplicationPage
    {
        public SubCategoriesPage()
        {
            InitializeComponent();
            this.Loaded += CategoryDetaill_Loaded;
            lstCategoryList.SelectionChanged+=lstCategoryList_SelectionChanged;
        }

        void CategoryDetaill_Loaded(object sender, RoutedEventArgs e)
        {
            lstCategoryList.ItemsSource = PlacesMethods.MockDataGetSubCategories();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
             base.OnNavigatedTo(e);
             string cat = NavigationContext.QueryString["category"];
             txbCategory.Text = cat;
        }

        void lstCategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category selectedcategory = lstCategoryList.SelectedItem as Category;
            if (selectedcategory.name != null)
                NavigationService.Navigate(new Uri("/Pages/Places/PlacesPage.xaml?category=" + selectedcategory.name, UriKind.Relative));
        }
    }
}
