using Newtonsoft.Json;
using QuickMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                client = new WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers["Connection"] = "keep-alive";
                client.Headers["Cache-Control"] = "max-age=0";
                client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.22 (KHTML, like Gecko) Chrome/25.0.1364.152 Safari/537.22";
                client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                client.Headers["Accept-Encoding"] = "gzip,deflate,sdch";
                client.Headers["Accept-Language"] = "en-US,en;q=0.8";
                client.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.3";

                string data =
                    "fbUserCode=" + facebookCode +
                    "&latitude=" + latitude +
                    "&longitude=" + longitude +
                    "&description=" + description +
                    "&type=" + type;
                string result = await Internet.UploadStringAsync(client, ReportProblemUri, data);
                return JsonConvert.DeserializeObject<EventResult>(result);
            }

            public static async Task<EventResult> GetEvents(double latitude, double longitude)
            {
                Uri uriAddress = new Uri(GetProblemsUri +
                    "?latitude=" + latitude +
                    "&longitude=" + longitude, UriKind.Absolute);
                return await downloadedInfo(uriAddress);
            }

            public static async Task<EventResult> GetEvents(List<string> fbFriendList)
            {
                string friendsList = String.Empty;
                foreach (string friend in fbFriendList)
                    friendsList += friend + ",";

                Uri uriAddress = new Uri(GetProblemsByFriendsUri +
                    "?friends=" + friendsList, UriKind.Absolute);

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