using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZebrasLib.Classes;

namespace ZebrasLib
{
    namespace Events
    {
        public static partial class EventsMethods
        {
            public static async Task<EventResult> ReportEvent(
                    string facebookCode,
                    double latitude,
                    double longitude,
                    string description,
                    int type)
            {
                string data =
                    "facebookcode=" + facebookCode +
                    "&latitude=" + latitude +
                    "&longitude=" + longitude +
                    "&description=" + description +
                    "&type=" + type;
                string result = await Internet.UploadStringAsync(Main.urlReportProblem, data);
                return JsonConvert.DeserializeObject<EventResult>(result);
            }

            public static async Task<EventResult> GetEvents(double latitude, double longitude)
            {
                string url = Main.urlGetProblems +
                    "latitude=" + latitude +
                    "&longitude=" + longitude;
                return await downloadedInfo(url);
            }

            public static async Task<EventResult> GetEvents(List<string> fbFriendList)
            {
                string friendsList = String.Empty;
                foreach (string friend in fbFriendList)
                    friendsList += friend + ",";

                string uriAddress = Main.urlGetProblemsByFriends +
                    "friends=" + friendsList;

                return await downloadedInfo(uriAddress);
            }

            public static List<Event> GetEventsVerified(List<Event> lstEventsSource)
            {
                IEnumerable<Event> query = from E in lstEventsSource
                                           where E.isVerified
                                           select E;
                return query.ToList();
            }

            public static List<Event> GetEventsReportedByType(List<Event> lstEventsSource, int type)
            {
                IEnumerable<Event> query = from E in lstEventsSource
                                           where E.type == type
                                           select E;
                return query.ToList();
            }

            public static List<Event> GetEventsReportedNear(List<Event> lstEventsSource, double latitude, double longitude, int distanceFromUser)
            {
                IEnumerable<Event> query = from E in lstEventsSource
                                           where isNear(E.latitude, E.longitude, latitude, longitude, distanceFromUser)
                                           select E;
                return query.ToList();
            }
        }
    }
}