using Newtonsoft.Json;
using QuickMethods;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ZebrasLib.Classes;

namespace ZebrasLib
{
    namespace Events
    {
        public static partial class EventsMethods
        {
            public static Uri ReportProblemUri = new Uri("http://stcbolivia.net/api/problems/report", UriKind.Absolute);
            public static string GetProblemsUri = "http://stcbolivia.net/api/problems/get";
            public static string GetProblemsByFriendsUri = "http://stcbolivia.net/api/problems/getbyfriends";

            public static WebClient client;

            public class EventResult
            {
                [JsonProperty("Error")]
                public string errorMessage { get; set; }

                [JsonProperty("Success")]
                public string successMessage { get; set; }

                [JsonProperty("Events")]
                public List<Event> eventsList { get; set; }
            }

            private static async Task<EventResult> downloadedInfo(Uri uriAddress)
            {
                client = new WebClient();
                string result = await Internet.DownloadStringAsync(client, uriAddress);
                EventResult eventResult = JsonConvert.DeserializeObject<EventResult>(result);
                eventResult.eventsList = formatedList(eventResult.eventsList);
                return eventResult;
            }

            private static string UnixTimeToDateTime(double secondsPastEpoch)
            {
                DateTime currentTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                currentTime = currentTime.AddSeconds(secondsPastEpoch).ToLocalTime();
                return currentTime.ToShortTimeString();
            }

            private static string DateTimeToUnixTime(DateTime currentTime)
            {
                DateTime unixTime = new DateTime(1970, 1, 1);
                return currentTime.Subtract(unixTime).TotalSeconds.ToString();
            }

            private static bool isNear(double latOne, double lonOne, double latTwo, double lonTwo, int distanceFromUser)
            {
                double x = 69.1 * (latTwo - latOne);
                double y = 53.0 * (lonTwo - lonOne);
                double distance = Math.Sqrt(x * x + y * y);

                if (distance < distanceFromUser)
                    return true;
                return false;
            }

            private static List<Event> formatedList(List<Event> unformatedList)
            {
                foreach (Event E in unformatedList)
                    E.reportedAt = UnixTimeToDateTime(Double.Parse(E.reportedAt));
                return unformatedList;
            }
        }
    }
}