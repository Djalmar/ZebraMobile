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

            public static async Task<List<Place>> downloadAllPlacesFromThisCategories(List<Category> list)
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
        }
    }
}