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

        public static string urlWallet = "http://cebritas.azurewebsites.net/api/wallet/getPlaces?";
        public static string urlCategories = "http://cebritas.azurewebsites.net/api/places/getCategories?";
        public static string urlPlaces = "http://cebritas.azurewebsites.net/api/places/getPlaces?";

        public static readonly string FacebookAppId = "316949918437312";
        public static string AccessToken = String.Empty;
        public static string FacebookId = String.Empty;

        public static async Task<List<Place>> GetPlacesList(string url)
        {
            string downloadedInfo = await DownloadInfo(url);
            return JsonConvert.DeserializeObject<List<Place>>(downloadedInfo);
        }

        public static async Task<List<Category>> GetCategoriesList(string url)
        {
            string downloadedInfo = await DownloadInfo(url);
            return JsonConvert.DeserializeObject<List<Category>>(downloadedInfo);
        }

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
    }
}