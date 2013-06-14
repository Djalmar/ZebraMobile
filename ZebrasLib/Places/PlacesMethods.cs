using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            public static async Task<List<Place>> DownloadAllPlacesFromThisCategories(List<Category> list)
            {
                List<Place> allPlaces = new List<Place>();
                foreach (Category category in list)
                    allPlaces.AddRange(await getAllPlacesByCategory(category.code));
                return allPlaces;
            }

            public static List<Place> getPlacesOrderedByDistance(double latitude, double longitude, List<Place> lstPlaces)
            {
                IEnumerable<Place> newList = from allPlaces
                                            in lstPlaces
                                             orderby allPlaces.distance ascending
                                             select allPlaces;
                return newList.ToList();
            }

            public static List<Place> getPlacesOrderedByPopularity(List<Place> lstPlaces)
            {
                IEnumerable<Place> newList = from allPlaces
                                             in lstPlaces
                                             where allPlaces.rating >= 7
                                             orderby allPlaces.distance descending
                                             select allPlaces;
                return newList.ToList();
            }

            public static List<Place> getDistancesForEachPlace(double latitude, double longitude, List<Place> lstPlaces)
            {
                foreach (Place P in lstPlaces)
                    P.distance = Main.findDistance(P.latitude, P.longitude, latitude, longitude);
                return lstPlaces;
            }

            public static async Task<List<Place>> getPlacesByQuery(string query, double latitude, double longitude)
            {
                //tienen que estar ordenados por distancia
                string url = Main.urlPlaces +
                    "query=" + query;
                List<Place> lstPlaces = await Main.GetPlacesList(url);

                return getPlacesOrderedByDistance(latitude, longitude, lstPlaces);
            }

            #region MockData

            public static Task<List<Category>> MockDataGetCategories()
            {
                string direction = "MockData/CategoriesResult.json";

                return GetCategoriesForThisJson(direction);
            }

            public static Task<List<Category>> MockDataGetSubCategories()
            {
                string direction = "MockData/SubCategoriesResult.json";

                return GetCategoriesForThisJson(direction);
            }

            public static async Task<PlacesResult> MockDataGetPlaces()
            {
                string direction = "MockData/PlacesResult.json";
                string result = await Main.GetStringFromStream(direction);

                PlacesResult res = JsonConvert.DeserializeObject<PlacesResult>(result);
                return res;
            }

            private static async Task<List<Category>> GetCategoriesForThisJson(string direction)
            {
                string result = await Main.GetStringFromStream(direction);

                return JsonConvert.DeserializeObject<List<Category>>(result);
            }

            #endregion MockData
        }
    }
}