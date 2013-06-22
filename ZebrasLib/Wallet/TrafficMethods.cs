using System.Collections.Generic;
using System.Threading.Tasks;
using ZebrasLib.Classes;

namespace ZebrasLib
{
    namespace Wallet
    {
        public class WalletMethods
        {
            public static async Task<List<Place>> getPlacesWithMaxPrice(int maxPrice, string categoryCode)
            {
                string url = Main.urlWalletBelow +
                    "maxprice=" + maxPrice + 
                    "code="+ categoryCode;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesWithMaxPriceAndQuery(int maxPrice, string query)
            {
                string url = Main.urlWalletBelow +
                    "maxprice=" + maxPrice +
                    "&query=" + query;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesBetween(int maxPrice, int minPrice, string categoryCode)
            {
                string url = Main.urlWalletBetween +
                    "maxprice=" + maxPrice +
                    "&minprice=" + minPrice +
                    "&code=" + categoryCode;
                return await Main.GetPlaces(url);
            }

            public static async Task<List<Place>> getPlacesBetweenAndQuery(int maxPrice, int minPrice, string query)
            {
                string url = Main.urlWalletBetween +
                    "maxprice=" + maxPrice +
                    "&minprice" + minPrice +
                    "&query=" + query;
                return await Main.GetPlaces(url);
            }
        }
    }
}