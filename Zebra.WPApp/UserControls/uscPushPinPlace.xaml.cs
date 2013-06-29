using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Zebra.WPApp.Pages.Places;

namespace Zebra.WPApp.UserControls
{
    public partial class uscPushPinPlace : UserControl
    {
        public uscPushPinPlace()
        {
            InitializeComponent();
            this.Loaded += uscPushPinPlace_Loaded;
        }

        void uscPushPinPlace_Loaded(object sender, RoutedEventArgs e)
        {
            if (staticClasses.selectedPlace != null)
            {
                double distance = Math.Round(staticClasses.selectedPlace.distance, 1);
                if (App.usesKilometers)
                {
                    txbUnits.Text = "Km";
                    txbDistance.Text = distance + "";
                }
                else
                {
                    txbUnits.Text = "Miles";
                    txbDistance.Text = Math.Round(distance * 1.6, 1) + "";
                }
            }
            else MessageBox.Show("cagaste");

        }
    }
}
