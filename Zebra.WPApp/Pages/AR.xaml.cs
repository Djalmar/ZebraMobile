using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GART;
using GART.Controls;
using GART.Data;
namespace Zebra.WPApp.Pages
{
    public partial class AR : PhoneApplicationPage
    {
        public AR()
        {
            InitializeComponent();
            gartDisplay.Tap += gartDisplay_Tap;
        }

        void gartDisplay_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            gartDisplay.ARItems = staticClasses.lstGartItems;
            gartDisplay.StartServices();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            gartDisplay.StopServices(); 
        }
    }
}