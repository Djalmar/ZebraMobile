using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Zebra.WPApp.Pages.Places;
using ZebrasLib.Classes;

namespace Zebra.WPApp.UserControls
{
    public partial class uscPushPin : UserControl
    {
        public Problem evento;
        public uscPushPin()
        {
            InitializeComponent();
            this.Loaded+=uscPushPin_Loaded2;
        }
        //aqui llenamos el push pin
        public uscPushPin(Problem evento)
        {
            InitializeComponent();
            this.evento = evento;
            this.Loaded += uscPushPin_Loaded;
        }

        void uscPushPin_Loaded2(object sender, System.Windows.RoutedEventArgs e)
        {
            txbCategory.Text = staticClasses.selectedPlace.distance + "Km";
            LayoutRoot.Children.Remove(imgIconCategory);
            Grid.SetRow(txbCategory, 1);
            Grid.SetColumnSpan(txbCategory, 2);
        }
        void uscPushPin_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            txbCategory.Text = evento.reporters.Count + "";
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