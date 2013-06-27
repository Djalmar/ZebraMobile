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
using ZebrasLib.Wallet;
namespace Zebra.WPApp.Pages.Wallet
{
    public partial class ResultPage : PhoneApplicationPage
    {
        string maxmoney, minmoney, people, content, categorie;
        public ResultPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            minmoney = NavigationContext.QueryString["minmoney"];
            maxmoney = NavigationContext.QueryString["maxmoney"];
            people = NavigationContext.QueryString["people"];
            if (NavigationContext.QueryString.TryGetValue("content", out content))
                MessageBox.Show("CON BUSQUEDA");
            else
                if (NavigationContext.QueryString.TryGetValue("categorie", out categorie))
                    MessageBox.Show("CON CATEGORIA");
            
        }
    }
}