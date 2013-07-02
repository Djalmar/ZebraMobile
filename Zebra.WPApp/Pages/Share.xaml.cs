using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using QuickMethods;
using System.Windows.Media;
using Zebra.WPApp;
namespace Zebra.WPApp
{
    public partial class Share : PhoneApplicationPage
    {
        string title, message;
        Uri link;
        public Share()
        {
            InitializeComponent();
            txtMail.Tap += txtMail_Tap;
            txtSMS.Tap += txtSMS_Tap;
            txtSocial.Tap += txtSocial_Tap;
            this.Loaded += Share_Loaded;
        }

        void Share_Loaded(object sender, RoutedEventArgs e)
        {
            title = ShareContent.title;
            message = ShareContent.message;
            link = ShareContent.link;    
        }


        void txtSocial_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            txtSocial.Foreground = this.Resources["PhoneAccentBrush"] as SolidColorBrush;
            SocialNetworks.shareLink(link,message,title);
        }

        void txtSMS_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            txtSMS.Foreground = this.Resources["PhoneAccentBrush"] as SolidColorBrush;
            Phone.sendSMS(title + "\n" + message + "\n" + link);
        }

        void txtMail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            txtMail.Foreground = this.Resources["PhoneAccentBrush"] as SolidColorBrush;
            Email.sendEmail(message + "\n\n\n" + link, title);
        }
    }
}