using QuickMethods;
using System;
using Microsoft.Phone.Controls;

namespace Zebra.WPApp.Pages
{
    public partial class Share : PhoneApplicationPage
    {
        private string title;
        private Uri url;
        private string message;

        public Share()
        {
            InitializeComponent();
            txtMail.Tap += txtMail_Tap;
            txtSMS.Tap += txtSMS_Tap;
            txtSocial.Tap += txtSocial_Tap;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            message = NavigationContext.QueryString["message"];
            url = new Uri(NavigationContext.QueryString["url"], UriKind.Absolute);
            title = NavigationContext.QueryString["title"];
        }

        private void txtSocial_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SocialNetworks.shareLink(url, message, title);
        }

        private void txtSMS_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Phone.sendSMS(message);
        }

        private void txtMail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Email.sendEmail(message, title);
        }
    }
}