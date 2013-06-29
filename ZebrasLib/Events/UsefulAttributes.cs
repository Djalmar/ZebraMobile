using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ZebrasLib.Classes;

namespace ZebrasLib
{
    namespace Events
    {
        public static partial class ProblemsMethods
        {
            private static async Task<ProblemsResult> downloadedInfo(string uriAddress)
            {
                string result = await Internet.DownloadStringAsync(uriAddress);
                ProblemsResult eventResult = JsonConvert.DeserializeObject<ProblemsResult>(result);
                if (eventResult != null)
                {
                    if (Main.thereIsNoProblemo(eventResult.status))
                        eventResult.problemsList = formatedList(eventResult.problemsList);
                    return eventResult;
                }
                return null;
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

            public static List<Problem> formatedList(List<Problem> unformatedList)
            {
                foreach (Problem E in unformatedList)
                {
                    E.latitude = Convert.ToDouble(E.strlatitude, new CultureInfo("en-US"));
                    E.longitude= Convert.ToDouble(E.strlongitude, new CultureInfo("en-US"));
                    E.reportedAt = UnixTimeToDateTime(Double.Parse(E.reportedAt));
                    E.dtReportedAt = DateTime.Parse(E.reportedAt).ToUniversalTime();
                    E.icon = GetIconForThisType(E.type);
                    if (E.reporters != null)
                    {
                        foreach (Reporter R in E.reporters)
                        {
                            R.reportedAt = UnixTimeToDateTime(Double.Parse(R.reportedAt));
                            R.dtReportedAt = DateTime.Parse(R.reportedAt).ToUniversalTime();
                            R.latitude = Convert.ToDouble(R.strlatitude, new CultureInfo("en-US"));
                            R.longitude= Convert.ToDouble(R.strlongitude, new CultureInfo("en-US"));
                        }
                    }
                    else {
                        E.reporters = new List<Reporter>();
                        E.reporters.Add(new Reporter
                        {
                            description = E.description,
                            dtReportedAt = E.dtReportedAt,
                            facebookCode = E.facebookUserCode,
                            latitude = Convert.ToDouble(E.strlatitude, new CultureInfo("en-US")),
                            longitude = Convert.ToDouble(E.strlongitude, new CultureInfo("en-US")),
                            reportedAt = E.reportedAt,
                            type = E.type
                        }); 
                    }
                }
                return unformatedList;
            }

            private static string GetIconForThisType(int p)
            {
                string path = "/Assets/Icons/Problems/";
                switch (p)
                {
                    case 1:
                        return path + "1.png";
                    case 2:
                        return path + "2.png";
                    case 3:
                        return path + "3.png";
                    case 4:
                        return path + "4.png";
                    case 5:
                        return path + "5.png";
                    default:
                        return path + "6.png";
                }
            }
        }
    }
}