using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebrasLib.Classes;

namespace ZebrasLib
{
    namespace Events
    {
        public static partial class EventsMethods
        {
            private static async Task<EventResult> downloadedInfo(string uriAddress)
            {
                string result = await Internet.DownloadStringAsync(uriAddress);

                EventResult eventResult = JsonConvert.DeserializeObject<EventResult>(result);
                if (Main.thereIsNoProblemo(eventResult.status))
                    eventResult.eventsList = formatedList(eventResult.eventsList);

                return eventResult;
            }

            private static string UnixTimeToDateTime(double secondsPastEpoch)
            {
                DateTime currentTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                currentTime = currentTime.AddSeconds(secondsPastEpoch).ToLocalTime();
                return currentTime.ToLocalTime().ToString();
            }

            private static string DateTimeToUnixTime(DateTime currentTime)
            {
                DateTime unixTime = new DateTime(1970, 1, 1);
                return currentTime.Subtract(unixTime).TotalSeconds.ToString();
            }

            private static bool isNear(double latOne, double lonOne, double latTwo, double lonTwo, int distanceFromUser)
            {
                double distance = Main.findDistance(latOne, lonOne, latTwo, lonTwo);
                if (distance < distanceFromUser)
                    return true;
                return false;
            }

            public static List<Event> formatedList(List<Event> unformatedList)
            {
                foreach (Event E in unformatedList)
                {
                    E.reportedAt = UnixTimeToDateTime(Double.Parse(E.reportedAt));
                    foreach (Reporter R in E.reporters)
                        R.reportedAt = UnixTimeToDateTime(Double.Parse(R.reportedAt));
                }
                return unformatedList;
            }
        }
    }
}