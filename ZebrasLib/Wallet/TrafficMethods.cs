using System.Collections.Generic;
using System.Threading.Tasks;
using ZebrasLib.Classes;

namespace ZebrasLib
{
    namespace Wallet
    {
        public class WalletMethods
        {
            public static async Task<List<Place>> getPlacesWithMaxPrice(double maxPrice)
            {
                string url = Main.urlWallet +
                    "maxPrice=" + maxPrice;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesWithMaxPrice(double maxPrice, string query)
            {
                string url = Main.urlWallet +
                    "maxPrice=" + maxPrice +
                    "&query=" + query;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesWithMaxPrice(double maxPrice, double latitude, double longitude, int nearDistance)
            {
                string url = Main.urlWallet +
                    "maxPrice=" + maxPrice +
                    "&lat=" + latitude +
                    "&lon=" + longitude +
                    "&distance=" + nearDistance;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesWithMaxPrice(double maxPrice, string query, double latitude, double longitude, int nearDistance)
            {
                string url = Main.urlWallet +
                    "maxPrice=" + maxPrice +
                    "&query=" + query +
                    "&lat=" + latitude +
                    "&lon=" + longitude +
                    "&distance=" + nearDistance;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesWithMinPrice(double minPrice)
            {
                string url = Main.urlWallet +
                    "maxPrice=" + minPrice;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesWithMinPrice(double minPrice, string query)
            {
                string url = Main.urlWallet +
                    "maxPrice=" + minPrice +
                    "&query=" + query;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesWithMinPrice(double minPrice, double latitude, double longitude, int nearDistance)
            {
                string url = Main.urlWallet +
                    "maxPrice=" + minPrice +
                    "&lat=" + latitude +
                    "&lon=" + longitude +
                    "&distance=" + nearDistance;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesWithMinPrice(double minPrice, string query, double latitude, double longitude, int nearDistance)
            {
                string url = Main.urlWallet +
                    "maxPrice=" + minPrice +
                    "&query=" + query +
                    "&lat=" + latitude +
                    "&lon=" + longitude +
                    "&distance=" + nearDistance;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesBetween(double maxPrice, double minPrice)
            {
                string url = Main.urlWallet +
                    "maxPrice=" + maxPrice +
                    "&minPrice" + minPrice;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesBetween(double maxPrice, double minPrice, string query)
            {
                string url = Main.urlWallet +
                    "maxPrice=" + maxPrice +
                    "&minPrice" + minPrice +
                    "&query=" + query;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesBetween(double maxPrice, double minPrice, double latitude, double longitude, int nearDistance)
            {
                string url = Main.urlWallet +
                    "maxPrice=" + maxPrice +
                    "&minPrice" + minPrice +
                    "&lat=" + latitude +
                    "&lon=" + longitude +
                    "&distance=" + nearDistance;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesBetween(double maxPrice, double minPrice, string query, double latitude, double longitude, int nearDistance)
            {
                string url = Main.urlWallet +
                    "maxPrice=" + maxPrice +
                    "&minPrice" + minPrice +
                    "&query=" + query +
                    "&lat=" + latitude +
                    "&lon=" + longitude +
                    "&distance=" + nearDistance;
                return await Main.GetPlaces(url);
            }
        }
    }
}