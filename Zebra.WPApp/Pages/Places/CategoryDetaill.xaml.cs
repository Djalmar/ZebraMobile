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
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
             base.OnNavigatedTo(e);
             string cat = NavigationContext.QueryString["category"];
             txtCambiar.Title = cat;
        }
    }
}
