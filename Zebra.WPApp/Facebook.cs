using Facebook;
using Facebook.Client;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using ZebrasLib;
using ZebrasLib.Classes;

namespace OurFacebook
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
                session = new FacebookSession();
                return false;
            }
        }

        public static Task<List<facebookUser>> downloadFriendsList(string accessToken)
        {
            var downloadedList = new TaskCompletionSource<List<facebookUser>>();
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

                IEnumerable<facebookUser> formatedList = from facebookUser F in data.friends
                                                         where F.usesApp == true
                                                         select F;
                downloadedList.SetResult(formatedList.ToList());
            };

            fb.GetTaskAsync("/me/friends?fields=installed,name,picture");
            return downloadedList.Task;
        }

        public static async Task<List<Reporter>> GetFbInfoForTheseReporters(List<Reporter> list, string accessToken)
        {
            facebookUser user = new facebookUser();
            foreach (Reporter reporter in list)
            {
                user = await FacebookMethods.getUserInfo(accessToken, reporter.facebookCode);
                reporter.name = user.Name;
                reporter.picture = user.picture.data.url;
            }
            return list;
        }

        private static async Task<facebookUser> getUserInfo(string accessToken, string facebookId)
        {
            var downloadedUser = new TaskCompletionSource<facebookUser>();
            FacebookClient fb = new FacebookClient(accessToken);

            WebClient client = new WebClient();
            string x = await Internet.DownloadStringAsync("https://graph.facebook.com/" + facebookId + "?fields=name,picture");
            facebookUser data = JsonConvert.DeserializeObject<facebookUser>(x);
            return data;
        }
    }
}