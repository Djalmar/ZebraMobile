using Newtonsoft.Json;
using QuickMethods;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using ZebrasLib.Classes;

namespace ZebrasLib
{
    public class Main
    {
        public static string urlWallet = "https://cebritas.com/wallet/getPlaces?";
        public static string urlCategories = "https://cebritas.com/places/getCategories?";
        public static string urlPlaces = "https://cebritas.com/places/getPlaces?";
        public static WebClient client;

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
            client = new WebClient();
            Uri uri = new Uri(url, UriKind.Absolute);
            string downloadedInfo = await Internet.DownloadStringAsync(client, uri);
            return downloadedInfo;
        }

        public static bool thereIsNoProblemo(string status, string message)
        {
            switch (status)
            {
                case "200":
                    return true;
                case "400":
                    MessageBox.Show(message);
                    return false;
                case "403":
                    MessageBox.Show("You don't have permissions");
                    return false; ;
                case "500":
                    MessageBox.Show("There was a problemo jefe");
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