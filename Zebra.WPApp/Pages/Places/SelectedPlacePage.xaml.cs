using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Zebra.WPApp.Pages.Places
{
    public partial class SelectedPlacePage : PhoneApplicationPage
    {
        public SelectedPlacePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            panPlace.Title = staticClasses.selectedPlace.name;
            txtLocationAddress.Text = staticClasses.selectedPlace.address;
            txtLocationDistance.Text = staticClasses.selectedPlace.distance.ToString();
        }
    }
}