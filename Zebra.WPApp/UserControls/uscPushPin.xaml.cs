using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Zebra.WPApp.UserControls
{
    public partial class uscPushPin : UserControl
    {
        public uscPushPin()
        {
            InitializeComponent();
        }
        //aqui llenamos el push pin
        public uscPushPin(string category, SolidColorBrush color,Uri direccion)
        {
            InitializeComponent();
            txbCategory.Text = category;
            elpPush.Fill = color;
            imgIconCategory.Source = new BitmapImage(direccion);
        }
    }
}
