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

        public static string urlWallet = "http://cebritas.azurewebsites.net/api/wallet/getPlaces?";

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
            if (Main.thereIsNoProblemo(result.status))
                return result.placesList;
            else return null;

        }
        #endregion
    }
}