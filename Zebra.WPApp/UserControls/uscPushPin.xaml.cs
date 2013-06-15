using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZebrasLib.Classes;

namespace Zebra.WPApp.UserControls
{
    public partial class uscPushPin : UserControl
    {
        public uscPushPin()
        {
            InitializeComponent();
        }
        //aqui llenamos el push pin
        public uscPushPin(Event evento)
        {
            InitializeComponent();
            txbCategory.Text = evento.importance+"";
            if (evento.isVerified)
                elpPush.Fill = new SolidColorBrush(Colors.Red);
            else
                elpPush.Fill = GetColor(evento.reporters);
            imgIconCategory.Source = new BitmapImage(GetIcon(evento.type));
        }

        private Uri GetIcon(int p)
        {
            return new Uri("/images/Icons/cine.png", UriKind.Relative);
        }

        private Brush GetColor(System.Collections.Generic.List<Reporter> list)
        {
            Color color = new Color();
            color.R = 100;
            color.G = 200;
            color.B = 150;
            SolidColorBrush brocha = new SolidColorBrush(color);
            return brocha;
        }
    }
}