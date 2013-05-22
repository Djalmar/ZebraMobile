using Facebook;
using Facebook.Client;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ZebrasLib.Classes;

namespace ZebrasLib
{
    namespace Facebook
    {
        public class FacebookMethods
        {
            private static FacebookSessionClient FacebookSessionClient;
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

                catch
                {
                    MessageBox.Show("No se pudo iniciar la sesion, intentalo de nuevo por favor");
                    session = new FacebookSession();
                    return false;
                }
            }

            public static Task<List<Friend>> downloadFriendsList(string accessToken)
            {
                var downloadedList = new TaskCompletionSource<List<Friend>>();
                FacebookClient fb = new FacebookClient(accessToken);

                fb.GetCompleted += (o, e) =>
                {
                    if (e.Error != null)
                    {
                        downloadedList.SetException(e.Error);
                        return;
                    }

                    string result = e.GetResultData().ToString();
                    FacebookData data = JsonConvert.DeserializeObject<FacebookData>(result);
                    
                    IEnumerable<Friend> formatedList = from Friend F in data.friends
                                                       where F.usesApp == true
                                                       select F;
                    downloadedList.SetResult(formatedList.ToList());
                };

                fb.GetTaskAsync("/me/friends?fields=installed,name,picture");
                return downloadedList.Task;
            }
        }
    }
}