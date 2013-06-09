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
using ZebrasLib.Events;
using ZebrasLib;
namespace Zebra.WPApp.Pages.Trouble
{
    public partial class TroublesPage : PhoneApplicationPage
    {
        private List<Event> troublesList;
        public List<Event> TroublesList
        {
            get { return troublesList; }
            set { troublesList = value; }
        }
        private EventResult result;

        public EventResult Result
        {
            get { return result; }
            set { result = value; }
        }
        
        public TroublesPage()
        {
            InitializeComponent();
            this.Loaded += TroublesPage_Loaded;
        }

        void TroublesPage_Loaded(object sender, RoutedEventArgs e)
        {
            result = EventsMethods.MockDataGetEvents();
            if (Main.thereIsNoProblemo(result.status,result.message))
            {
                troublesList = result.eventsList;
                lstTroubles.ItemsSource = troublesList;
            }
        }
    }
}