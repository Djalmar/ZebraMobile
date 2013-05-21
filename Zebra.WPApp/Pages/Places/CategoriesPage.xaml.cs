using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Windows;
using ZebrasLib.Classes;

namespace Zebra.WPApp.Pages.Places
{
    public partial class CategoriesPage : PhoneApplicationPage
    {
        public CategoriesPage()
        {
            InitializeComponent();
            this.Loaded += CategoriesPage_Loaded;
        }

        private void CategoriesPage_Loaded(object sender, RoutedEventArgs e)
        {
            List<Category> listCategories = new List<Category>();
            listCategories.Add(new Category { name = "Coffe" });
            listCategories.Add(new Category { name = "Coffe1" });
            listCategories.Add(new Category { name = "Coffe2" });
            listCategories.Add(new Category { name = "Coffe3" });
            listCategories.Add(new Category { name = "Coffe4" });

            lstCategories.ItemsSource = listCategories;
        }
    }
}