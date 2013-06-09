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

            public static async Task<List<Place>> getAllPlacesByCategory(string categoryCode)
            {
                string url = Main.urlPlaces +
                    "catCode=" + categoryCode;
                return await Main.GetPlacesList(url);
            }

            public static async Task<List<Place>> getPlacesNearByCategory(string categoryCode, double latitude, double longitude, int nearDistance)
            {
                string url = Main.urlPlaces +
                    "catCode=" + categoryCode +
                    "&lat=" + latitude +
                    "&lon=" + longitude +
                    "&distance=" + nearDistance;
                return await Main.GetPlacesList(url);
            }

            public static async Task<List<Place>> getPlacesByQuery(string query, double latitude, double longitude)
            {
                //tienen que estar ordenados por distancia
                string url = Main.urlPlaces +
                    "query=" + query;
                List<Place> lstPlaces = await Main.GetPlacesList(url);

                return GetListOrderedByDistance(latitude, longitude, lstPlaces);
            }

            private static List<Place> GetListOrderedByDistance(double latitude, double longitude, List<Place> lstPlaces)
            {
                foreach (Place P in lstPlaces)
                    P.distance = Main.findDistance(P.latitude, P.longitude, latitude, longitude);

                IEnumerable<Place> newList = from allPlaces
                                            in lstPlaces
                                            orderby allPlaces.distance ascending
                                            select allPlaces;
                return newList.ToList();
            }

            public static List<Place> getListOrderedByPopularity(List<Place> lstPlaces)
            {
                IEnumerable<Place> newList =    from allPlaces
                                                in lstPlaces
                                                where allPlaces.rating >=7
                                                orderby allPlaces.distance descending
                                                select allPlaces;
                return newList.ToList();
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