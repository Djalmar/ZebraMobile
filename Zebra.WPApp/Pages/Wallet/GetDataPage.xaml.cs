using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Zebra.WPApp.Resources;
using ZebrasLib.Classes;
namespace Zebra.WPApp.Pages.Wallet
{
    public partial class GetDataPage : PhoneApplicationPage
    {
        ApplicationBarIconButton btnFind;
        public GetDataPage()
        {
            InitializeComponent();
            this.Loaded += GetDataPage_Loaded;
            btnFind = new ApplicationBarIconButton();
            pivotMain.SelectionChanged += pivotMain_SelectionChanged;
            lspCategory.ItemsSource = DBPhone.CategoriesMethods.GetItems();
        }

        void pivotMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(pivotMain.SelectedIndex==0)
                btnFind.IconUri = new Uri("/Assets/AppBar/next.png", UriKind.Relative);
            else btnFind.IconUri = new Uri("/Assets/AppBar/check.png", UriKind.Relative);
        }

        void GetDataPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAppBar();
        }

        private void LoadAppBar()
        {
            ApplicationBar = new ApplicationBar();
            btnFind.IconUri = new Uri("/Assets/AppBar/next.png",UriKind.Relative);
            btnFind.Text = AppResources.TxtFind;
            btnFind.Click += btnFind_Click;
            ApplicationBar.Buttons.Add(btnFind);
        }

        void btnFind_Click(object sender, EventArgs e)
        {
            string urlplusData = "/Pages/Wallet/ResultPage.xaml";
            if (pivotMain.SelectedIndex == 0)
                pivotMain.SelectedIndex = 1;
            else 
            {
                if (txtMinMoney.Text.Length > 0)
                {
                    if (txtMaxMoney.Text.Length > 0)
                    {
                        if (txtPeople.Text.Length > 0)
                        {
                            if (txtSearch.Text.Length > 0 || lspCategory.SelectedItem != null)
                            {
                                if (txtSearch.Text.Length > 0)
                                    urlplusData += "?maxmoney=" + txtMaxMoney.Text +
                                        "&minmoney=" + txtMinMoney.Text + 
                                        "&people=" + txtPeople.Text +
                                        "&content=" + txtSearch.Text+
                                        "&comingFrom=Search";
                                else
                                    urlplusData += "?maxmoney=" + txtMaxMoney.Text +
                                        "&minmoney=" + txtMinMoney.Text + 
                                        "&people=" + txtPeople.Text +
                                        "&categorie=" + (lspCategory.SelectedItem as Category).code+
                                        "&comingFrom=Place";

                                NavigationService.Navigate(new Uri(urlplusData, UriKind.Relative));
                            }
                            else MessageBox.Show(AppResources.TxtWalletNoTextIntroduced);
                        }
                        else MessageBox.Show(AppResources.TxtWalletNoPeople);
                    }
                    else MessageBox.Show(AppResources.TxtWalletNoMaxMoney);
                }
                else MessageBox.Show(AppResources.TxtWalletNoMinMoney);
            }
        }
    }
}