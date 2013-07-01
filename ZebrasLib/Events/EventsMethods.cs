using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ZebrasLib.Classes;

namespace ZebrasLib
{
    namespace Events
    {
        public static partial class ProblemsMethods
        {
            //public static async Task<ProblemsResult> ReportProblem(
            //        string facebookCode,
            //        double latitude,
            //        double longitude,
            //        string description,
            //        int type)
            //{
            //    string strlatitude = Convert.ToString(latitude, new CultureInfo("en-US"));
            //    string strlongitude = Convert.ToString(longitude, new CultureInfo("en-US"));
            //    string data =
            //        "facebookcode=" + facebookCode +
            //        "&latitude=" + strlatitude +
            //        "&longitude=" + strlongitude +
            //        "&description=" + description +
            //        "&type=" + type;
            //    string result = await Internet.UploadStringAsync(Main.urlReportProblem, data);
            //    return JsonConvert.DeserializeObject<ProblemsResult>(result);
            //}

            public static async Task<List<Problem>> GetProblems(double latitude, double longitude, int timeZone)
            {
                string strlatitude = Convert.ToString(latitude, new CultureInfo("en-US"));
                string strlongitude = Convert.ToString(longitude, new CultureInfo("en-US"));
                string url = Main.urlGetProblems +
                    "latitude=" + strlatitude +
                    "&longitude=" + strlongitude +
                    "&timezone=" + timeZone;
                ProblemsResult result = await downloadedInfo(url);
                if (result != null)
                {
                    if (Main.thereIsNoProblemo(result.status))
                        return result.problemsList;
                    else return null;
                }
                return null;
                
            }

            public static async Task<List<Problem>> GetProblems(List<string> fbFriendList, int timeZone)
            {
                string friendsList = String.Empty;
                foreach (string friend in fbFriendList)
                    friendsList += friend + ",";

                string url= Main.urlGetProblemsByFriends +
                    "friends=" + friendsList +
                    "&timezone=" + timeZone;

                ProblemsResult result = await downloadedInfo(url);
                if (Main.thereIsNoProblemo(result.status))
                    return result.problemsList;
                else return null;
            }

            public static List<Problem> GetProblemsVerified(List<Problem> lstEventsSource)
            {
                IEnumerable<Problem> query = from E in lstEventsSource
                                           where E.isVerified
                                           select E;
                return query.ToList();
            }

            public static List<Problem> GetProblemsReportedByType(List<Problem> lstProblemsSource, int type)
            {
                IEnumerable<Problem> query = from E in lstProblemsSource
                                           where E.type == type
                                           select E;
                return query.ToList();
            }

            public static List<Problem> GetProblemsReportedNear(List<Problem> lstProblemsSource, 
                double latitude, 
                double longitude, 
                int distanceFromUser)
            {
                IEnumerable<Problem> query = from E in lstProblemsSource
                                           where isNear(E.latitude, E.longitude, latitude, longitude, distanceFromUser)
                                           select E;
                return query.ToList();
            }
        }
    }
}