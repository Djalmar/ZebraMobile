﻿using System;
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
using System.Windows.Input;
namespace Zebra.WPApp.Pages.Places
{
    public partial class CategoriesPage : PhoneApplicationPage
    {
        bool comingBack;
        public CategoriesPage()
        {
            InitializeComponent();
            this.Loaded += CategoriesPage_Loaded;
            comingBack = false;
            lstCategoryList.SelectionChanged += lstCategoryList_SelectionChanged;
            txtSearch.ActionIconTapped+=txtSearch_ActionIconTapped;
        }

        async void txtSearch_ActionIconTapped(object sender, EventArgs e)
        {
            lstSearchResults.Focus();
            lstSearchResults.ItemsSource = await PlacesMethods.getPlacesByQuery(txtSearch.Text, -16.5013, -68.1207);
        }

        void CategoriesPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!comingBack)
            {
                lstCategoryList.ItemsSource = DBPhone.CategoriesMethods.GetItems();
                comingBack = true;
            }
            
        }

        void lstCategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category selectedcategory = lstCategoryList.SelectedItem as Category;
            if (selectedcategory != null)
                if (selectedcategory.name != "")
                {
                    staticClasses.selectedCategory = selectedcategory;
                    NavigationService.Navigate(new Uri("/Pages/Places/PlacesPage.xaml",UriKind.Relative));
                }
                
        }
    }
}