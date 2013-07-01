using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebrasLib.Classes;

namespace ZebrasLib
{
    public class Main
    {
        public static string urlGetProblems = "http://cebritas.azurewebsites.net/api/problems/get?";
        public static string urlGetProblemsByFriends = "http://cebritas.azurewebsites.net/api/problems/getbyfriends?";
        public static string urlReportProblem = "http://cebritas.azurewebsites.net/api/problems/report";

        public static string urlCategories = "http://cebritas.azurewebsites.net/api/places/getcategories";
        public static string urlPlacesByCategory = "http://cebritas.azurewebsites.net/api/places/getbycategory?";
        public static string urlPlacesByQuery = "http://cebritas.azurewebsites.net/api/places//getbyquery?";
        public static string urlPlacesRate = "http://cebritas.azurewebsites.net/api/places/rateplace";

        public static string urlWalletBetween = "http://cebritas.azurewebsites.net/api/wallet/getplacesbetween?";
        public static string urlWalletBetweenQuery = "http://cebritas.azurewebsites.net/api/wallet/getplacesbypriceandquery?";

        public static readonly string FacebookAppId = "316949918437312";
        public static string AccessToken = String.Empty;
        public static string FacebookId = String.Empty;

        public static async Task<string> DownloadInfo(string url)
        {
            return await Internet.DownloadStringAsync(url);
        }

        public static bool thereIsNoProblemo(string status)
        {
            switch (status)
            {
                case "200":
                    return true;
                case "400":
                    return false;
                case "403":
                    return false; ;
                case "500":
                    return false;
                default: return false;
            }
        }

        public static double findDistance(double latA, double lonA, double latB, double lonB)
        {
            double x = 69.1 * (latB - latA);
            double y = 53.0 * (lonB - lonA);
            return Math.Sqrt(x * x + y * y);
        }

        #region Traffic and Wallet methods
        public static async Task<List<Place>> GetPlaces(string url)
        {
            string downloadedInfo = await Main.DownloadInfo(url);
            PlacesResult result = JsonConvert.DeserializeObject<PlacesResult>(downloadedInfo);
            if (result != null)
            {
                if (Main.thereIsNoProblemo(result.status))
                    return result.placesList;
                else return null; // There was a problemo jefe
            }
            else return null; //Internet is down :(
        }
        #endregion

        public static int GetValueFromTimeZone()
        {
            string timeZone = TimeZoneInfo.Local.BaseUtcOffset.ToString();
            switch (timeZone)
            {
                case "-12:00:00":
                    return 1;
                case "-11:00:00":
                    return 2;
                case "-10:00:00":
                    return 3;
                case "-09:00:00":
                    return 4;
                case "-08:00:00":
                    return 5;
                case "-07:00:00":
                    return 6;
                case "-06:00:00":
                    return 7;
                case "-05:00:00":
                    return 8;
                case "-04:30:00":
                    return 9;
                case "-04:00:00":
                    return 10;
                case "-03:30:00":
                    return 11;
                case "-03:00:00":
                    return 12;
                case "-02:00:00":
                    return 13;
                case "-01:00:00":
                    return 14;
                case "00:00:00":
                    return 15;
                case "01:00:00":
                    return 16;
                case "02:00:00":
                    return 17;
                case "03:00:00":
                    return 18;
                case "03:30:00":
                    return 19;
                case "04:00:00":
                    return 20;
                case "04:30:00":
                    return 21;
                case "05:00:00":
                    return 22;
                case "05:30:00":
                    return 23;
                case "05:45:00":
                    return 24;
                case "06:00:00":
                    return 25;
                case "06:30:00":
                    return 26;
                case "07:00:00":
                    return 27;
                case "08:00:00":
                    return 28;
                case "09:00:00":
                    return 29;
                case "09:30:00":
                    return 30;
                case "10:00:00":
                    return 31;
                case "11:00:00":
                    return 32;
                case "12:00:00":
                    return 33;
                case "13:00:00":
                    return 34;
                default:
                    return 15;
            }
        }
    }
}