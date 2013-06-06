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

namespace Zebra.WPApp.Pages.Places
{
    public partial class CategoryDetaill : PhoneApplicationPage
    {
        public CategoryDetaill()
        {
            InitializeComponent();
            this.Loaded += CategoryDetaill_Loaded;
        }

        void CategoryDetaill_Loaded(object sender, RoutedEventArgs e)
        {
            //llenamos a los 3 stackPanels All, Popular, Near
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
             base.OnNavigatedTo(e);
             string cat = NavigationContext.QueryString["category"];
             txbCategory.Title = cat;
        }
    }
}
