using Facebook.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace ZebrasLib
{
    namespace Facebook
    {
        public class LogIn
        {
            public static FacebookSessionClient FacebookSessionClient = new FacebookSessionClient(Main.FacebookAppId);
            private static FacebookSession session;
            public static async Task Authenticate()
            {
                string message = String.Empty;
                try
                {
                    session = await FacebookSessionClient.LoginAsync("user_about_me,read_stream");
                    Main.AccessToken = session.AccessToken;
                    Main.FacebookId = session.FacebookId;
                }
                catch (InvalidOperationException e)
                {
                    message = "Jodiste!!! Mira lo que paso: " + e.Message;
                    MessageBox.Show(message);
                }
            }
        }
    }
}
