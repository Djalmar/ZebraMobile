using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ZebrasLib.Classes;

namespace ZebrasLib
{
    namespace Places
    {
        public class PlacesMethods
        {
            public static async Task<List<Category>> getCategories()
            {
                return await Main.GetCategoriesList(Main.urlCategories);
            }

            public static async Task<List<Category>> getSubCategoriesByCategoy(string categoryCode)
            {
                string url = Main.urlCategories +
                    "parentCat=" + categoryCode;
                return await Main.GetCategoriesList(url);
            }

            public static async Task<List<Place>> getAllPlaces()
            {
                return await Main.GetPlacesList(Main.urlPlaces);
            }

            public static async Task<List<Place>> getAllPlaces(string categoryCode)
            {
                string url = Main.urlPlaces +
                    "catCode=" + categoryCode;
                return await Main.GetPlacesList(url);
            }

            public static async Task<List<Place>> getNearPlaces(double latitude, double longitude, int nearDistance)
            {
                string url = Main.urlPlaces +
                    "lat=" + latitude +
                    "&lon=" + longitude +
                    "&distance=" + nearDistance;
                return await Main.GetPlacesList(url);
            }

            public static async Task<List<Place>> getNearPlaces(string categoryCode, double latitude, double longitude, int nearDistance)
            {
                string url = Main.urlPlaces +
                    "catCode=" + categoryCode +
                    "&lat=" + latitude +
                    "&lon=" + longitude +
                    "&distance=" + nearDistance;
                return await Main.GetPlacesList(url);
            }

            public static async Task<List<Place>> getPlacesByCategory(string categoryCode)
            {
                string url = Main.urlPlaces +
                    "catCode=" + categoryCode;
                return await Main.GetPlacesList(url);
            }

            public static async Task<List<Place>> getPlacesByCategory(string categoryCode, double latitude, double longitude, int nearDistance)
            {
                string url = Main.urlPlaces +
                    "catCode=" + categoryCode +
                    "&lat=" + latitude +
                    "&lon=" + longitude +
                    "&distance=" + nearDistance;
                return await Main.GetPlacesList(url);
            }

            public static async Task<List<Place>> getPlacesByQuery(string query)
            {
                string url = Main.urlPlaces +
                    "query=" + query;
                return await Main.GetPlacesList(url);
            }

            public static async Task<List<Place>> getPlacesByQuery(string query, double latitude, double longitude, int nearDistance)
            {
                string url = Main.urlPlaces +
                    "query=" + query +
                    "&lat=" + latitude +
                    "&lon=" + longitude +
                    "&distance=" + nearDistance;
                return await Main.GetPlacesList(url);
            }

            public static async Task<Place> getPlaceByCode(string placeCode)
            {
                string url = Main.urlPlaces +
                   "placeCode=" + placeCode;
                return (await Main.GetPlacesList(url)).First();
            }

            public static List<Category> MockDataGetCategories()
            {
                string direction = "MockData/CategoriesResult.json";

                return GetCategoriesForThisJson(direction);
            }

            public static List<Category> MockDataGetSubCategories()
            {
                string direction = "MockData/SubCategoriesResult.json";

                return GetCategoriesForThisJson(direction);
            }

            private static List<Category> GetCategoriesForThisJson(string direction)
            {
                var streamInfo = Application.GetResourceStream(new Uri(direction, UriKind.Relative));
                string result = "";
                if (null != streamInfo)
                {
                    using (var sr = new StreamReader(streamInfo.Stream))
                    {
                        result = sr.ReadToEnd();
                    }
                }

                return JsonConvert.DeserializeObject<List<Category>>(result);
            }
            public static PlacesResult MockDataGetPlaces()
            {
                string direction = "MockData/PlacesResult.json";

                #region getListFromJsonFile

                var streamInfo = Application.GetResourceStream(new Uri(direction, UriKind.Relative));
                string result = "";
                if (null != streamInfo)
                {
                    using (var sr = new StreamReader(streamInfo.Stream))
                    {
                        result = sr.ReadToEnd();
                    }
                }

                #endregion getListFromJsonFile

                PlacesResult res = JsonConvert.DeserializeObject<PlacesResult>(result);
                return res;
            }
        }
    }
}