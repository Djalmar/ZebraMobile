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
        }
    }
}