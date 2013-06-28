using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZebrasLib.Classes;

namespace ZebrasLib
{
    namespace Places
    {
        public partial class PlacesMethods
        {
            public static async Task<List<Category>> getCategories()
            {
                return await GetCategories(Main.urlCategories);
            }

            public static async Task<List<Place>> getAllPlacesByCategory(string categoryCode, double latitude, double longitude)
            {
                string url = Main.urlPlacesByCategory +
                    "code=" + categoryCode +
                    "&latitude= " + latitude +
                    "&longitude=" + longitude;
                List<Place> lstPlace = await Main.GetPlaces(url);
                return lstPlace;
            }

            public static async Task<List<Place>> getPlacesByQuery(string query, double latitude, double longitude)
            {
                //tienen que estar ordenados por distancia
                string url = Main.urlPlacesByQuery +
                    "query=" + query +
                    "&latitude= " + latitude +
                    "&longitude=" + longitude;

                List<Place> lstPlaces = await Main.GetPlaces(url);
                if (lstPlaces != null)
                {
                    lstPlaces = getDistancesForEachPlace(latitude, longitude, lstPlaces);
                    return getPlacesOrderedByDistance(lstPlaces);
                }
                else return null;
                
            }

            public static async Task<List<Place>> getAllPlacesFromThisCategories(List<Category> lstCategories, double latitude, double longitude)
            {
                List<Place> allPlaces = new List<Place>();
                foreach (Category category in lstCategories)
                {
                    List<Place> placesFromThisCategory = await getAllPlacesByCategory(category.code, latitude, longitude);
                    allPlaces.AddRange(placesFromThisCategory);
                }
                return allPlaces;
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

            public static List<Place> getPlacesNear(List<Place> lstPlaces, double distance)
            {
                IEnumerable<Place> newList = from allPlaces
                                             in lstPlaces
                                             where allPlaces.distance <= distance
                                             orderby allPlaces.distance descending
                                             select allPlaces;
                return newList.ToList();
            }

            public static List<Place> getPlacesOrderedByDistance(List<Place> lstPlaces)
            {
                IEnumerable<Place> newList = from allPlaces
                                            in lstPlaces
                                             orderby allPlaces.distance ascending
                                             select allPlaces;
                return newList.ToList();
            }

            public static List<Place> getDistancesForEachPlace(double latitude, double longitude, List<Place> lstPlaces)
            {
                foreach (Place P in lstPlaces)
                    P.distance = Main.findDistance(P.latitude, P.longitude, latitude, longitude);
                return lstPlaces;
            }
        }
    }
}