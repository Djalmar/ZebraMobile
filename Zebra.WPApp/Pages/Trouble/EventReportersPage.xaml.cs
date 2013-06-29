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

namespace Zebra.WPApp.Pages.Trouble
{
    public partial class EventReportersPage : PhoneApplicationPage
    {
        public EventReportersPage()
        {
            InitializeComponent();
            this.Loaded+=EventReportersPage_Loaded;
            DataContext = staticClasses.selectedEvent;
        }

        void EventReportersPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}