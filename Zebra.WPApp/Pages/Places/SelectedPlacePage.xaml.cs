using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Zebra.WPApp;

namespace Zebra.WPApp.Pages.Places
{
    public partial class SelectedPlacePage : PhoneApplicationPage
    {
        public SelectedPlacePage()
        {
            InitializeComponent();
            this.Loaded += SelectedPlacePage_Loaded;
        }

        void SelectedPlacePage_Loaded(object sender, RoutedEventArgs e)
        {
            staticClasses.selectedPlace.rating /= 2;
            panorama.DataContext = staticClasses.selectedPlace;
            List<Service> listaServicios=CrearListadeServicios();
            lstFeatures.ItemsSource = listaServicios;
        }

        private List<Service> CrearListadeServicios()
        {
            return new List<Service>()
            {
                new Service(){Name="delivery",Exist=staticClasses.selectedPlace.delivery},
                new Service(){Name="holidays",Exist=staticClasses.selectedPlace.holidays},
                new Service(){Name="kids area",Exist=staticClasses.selectedPlace.kidsArea},
                new Service(){Name="delivery",Exist=staticClasses.selectedPlace.delivery}
            };
        }

        
        public class Service
        {
            public string Name { get; set; }
            public bool Exist { get; set; }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            //panPlace.Title = staticClasses.selectedPlace.name;
            //txtLocationAddress.Text = staticClasses.selectedPlace.address;
            //txtLocationDistance.Text = staticClasses.selectedPlace.distance.ToString();
        }
    }
}