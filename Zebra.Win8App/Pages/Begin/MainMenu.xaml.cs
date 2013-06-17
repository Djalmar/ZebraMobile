using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Zebra.Win8App;

namespace Zebra.Win8App.Pages.Begin
{
    public sealed partial class MainMenu: Page
    {
        public MainMenu()
        {
            this.InitializeComponent();
            this.Loaded += MainMenu_Loaded;
        }

        void MainMenu_Loaded(object sender, RoutedEventArgs e)
        { 

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }
    }
}
