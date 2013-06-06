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

namespace Zebra.WPApp.Pages.Places
{
    public partial class PlacesBegin : PhoneApplicationPage
    {
        public PlacesBegin()
        {
            InitializeComponent();
           
        }
        
        // Selection changed event
        private void CategorySelected(object sender, SelectionChangedEventArgs e)
        {
            //navegacion a la pagina SubCategories le mandas como queryString "category"
        }

    }
}