using System;
using System.Collections.Generic;
using System.Globalization;
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
                string strlatitude = Convert.ToString(latitude, new CultureInfo("en-US"));
                string strlongitude = Convert.ToString(longitude, new CultureInfo("en-US"));
                string url = Main.urlPlacesByCategory +
                    "code=" + categoryCode +
                    "&latitude= " + strlatitude +
                    "&longitude=" + strlongitude;
                return await FormatLocalizationAndGetDistances(latitude, longitude, url);
            }

            public static async Task<List<Place>> getPlacesByQuery(string query, double latitude, double longitude)
            {
                string strlatitude = Convert.ToString(latitude, new CultureInfo("en-US"));
                string strlongitude = Convert.ToString(longitude, new CultureInfo("en-US"));
                string url = Main.urlPlacesByQuery +
                    "query=" + query +
                    "&latitude= " + strlatitude +
                    "&longitude=" + strlongitude;

                return await FormatLocalizationAndGetDistances(latitude, longitude, url);
            }

            private static async Task<List<Place>> FormatLocalizationAndGetDistances(double latitude, double longitude, string url)
            {
                List<Place> lstPlaces = await Main.GetPlaces(url);
                if (lstPlaces != null)
                {
                    foreach (Place P in lstPlaces)
                    {
                        P.latitude = Convert.ToDouble(P.strlatitude, new CultureInfo("en-US"));
                        P.longitude = Convert.ToDouble(P.strlongitude, new CultureInfo("en-US"));
                    }
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

            public static List<Place> getPlacesOrderedByPopularity(List<Place> lstPlaces, int popularity)
            {
                IEnumerable<Place> newList = from allPlaces
                                             in lstPlaces
                                             where allPlaces.rating >=popularity
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
                {
                    P.latitude = Convert.ToDouble(P.strlatitude, new CultureInfo("en-US"));
                    P.longitude = Convert.ToDouble(P.strlongitude, new CultureInfo("en-US"));
                    P.distance = Main.findDistance(P.latitude, P.longitude, latitude, longitude);
                }
                return lstPlaces;
            }
        }
    }
}