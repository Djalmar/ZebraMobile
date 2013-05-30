using System;
using System.Windows.Controls;
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
        public uscPushPin(string category, SolidColorBrush color, Uri direccion)
        {
            InitializeComponent();
            txbCategory.Text = category;
            elpPush.Fill = color;
            imgIconCategory.Source = new BitmapImage(direccion);
        }
    }
}