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
            public static FacebookSessionClient FacebookSessionClient;
            private static FacebookSession session;
            public static async Task<bool> canAuthenticate()
            {
                try
                {
                    FacebookSessionClient = new FacebookSessionClient(Main.FacebookAppId);
                    session = await FacebookSessionClient.LoginAsync("user_about_me,read_stream");
                    Main.AccessToken = session.AccessToken;
                    Main.FacebookId = session.FacebookId;
                    return true;
                }

                catch (InvalidOperationException e)
                {
                    MessageBox.Show("No se pudo iniciar la sesion, intentalo de nuevo por favor");
                    session = new FacebookSession();
                    return false;
                }
            }
        }
    }
}
