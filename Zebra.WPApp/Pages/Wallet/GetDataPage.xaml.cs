using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Zebra.WPApp.Pages.Wallet
{
    public partial class GetDataPage : PhoneApplicationPage
    {
        public GetDataPage()
        {
            InitializeComponent();
        }

        private void sldMoney_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sldMoney != null)
            {
                double resta = e.NewValue % 10;
                if (resta > 5)
                    sldMoney.Value = e.NewValue + resta;
                else
                    sldMoney.Value = e.NewValue - resta;
            }
        }

        private void sldPeople_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sldPeople != null)
            {
                sldPeople.Value = Math.Truncate(e.NewValue);
            }
        }
    }
}