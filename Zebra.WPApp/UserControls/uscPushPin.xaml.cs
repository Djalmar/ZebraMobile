using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZebrasLib.Classes;

namespace Zebra.WPApp.UserControls
{
    public partial class uscPushPin : UserControl
    {
        public Event evento;
        public Place place;
        public uscPushPin()
        {
            InitializeComponent();
        }
        //aqui llenamos el push pin
        public uscPushPin(Event evento)
        {
            InitializeComponent();
            this.evento = evento;
            this.Loaded += uscPushPin_Loaded;
        }
        public uscPushPin(Place place)
        {
            this.place = place;
        }
        void uscPushPin_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            txbCategory.Text = evento.reporters.Count+"";
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
            color.B = 200;
            color.G = 100;
            for (int i = 0; i < list.Count; i++)
            {
                color.R += 10;
                color.G += 20;
            }
            color.A = 255;
            SolidColorBrush brocha = new SolidColorBrush(color);
            return brocha;
        }
    }
}