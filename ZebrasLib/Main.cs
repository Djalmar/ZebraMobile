using Newtonsoft.Json;
using QuickMethods;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
        internal static string AccessToken = String.Empty;
        internal static string FacebookId = String.Empty;
        public static bool isAuthenticated = false;

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
    }
}