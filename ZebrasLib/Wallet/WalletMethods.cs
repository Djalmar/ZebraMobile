using System.Collections.Generic;
using System.Threading.Tasks;
using ZebrasLib.Classes;
using ZebrasLib.Places;
namespace ZebrasLib
{
    namespace Wallet
    {
        public class WalletMethods
        {
            
            //public static async Task<List<Place>> getPlacesWithMaxPrice(int maxPrice, string categoryCode)
            //{
            //    string url = Main.urlWalletBelow +
            //        "maxprice=" + maxPrice + 
            //        "code="+ categoryCode;
            //    return await Main.GetPlaces(url);
            //}

            //public static async Task<List<Place>> getPlacesWithMaxPriceAndQuery(int maxPrice, string query)
            //{
            //    string url = Main.urlWalletBelow +
            //        "maxprice=" + maxPrice +
            //        "&query=" + query;
            //    return await Main.GetPlaces(url);
            //}

            public static async Task<List<Place>> getPlacesBetween(int maxPrice, int minPrice, 
                double latitude, double longitude, string categorie,bool km)
            {
                string url = Main.urlWalletBetween +
                    "longitude=" + longitude +
                    "&latitude=" + latitude +
                    "&maxprice=" + maxPrice +
                    "&minprice=" + minPrice +
                    "&code=" + categorie;
                List<Place> listToReturn = await Main.GetPlaces(url);
                
                return DistancesForThis(listToReturn,latitude,longitude,categorie,km);
            }

            public static async Task<List<Place>> getPlacesBetweenAndQuery(int maxPrice, int minPrice, 
                double latitude, double longitude, string query,bool km)
            {
                string url = Main.urlWalletBetweenQuery +
                    "longitude=" + longitude +
                    "&latitude=" + latitude +
                    "&maxprice=" + maxPrice +
                    "&minprice=" + minPrice +
                    "&query=" + query;
                return DistancesForThis(await Main.GetPlaces(url),latitude,longitude,"",km);
            }

            private static List<Place> DistancesForThis(List<Place> lstToReturn, double latitude, double longitude,string categorie,bool km)
            {
                List<Place> lstToFill = new List<Place>();

                if (lstToReturn != null)
                {
                    lstToFill = PlacesMethods.getDistancesForEachPlace(latitude, longitude, lstToReturn,km);
                    if (lstToFill != null)
                        lstToFill = GetParentCategories(lstToFill, categorie);
                    return lstToFill;
                }
                
                else return null;
            }

            private static List<Place> GetParentCategories(List<Place>lstToReturn, string categorie)
            {
                foreach (Place P in lstToReturn)
                    P.parentCategoryCode = categorie;
                return lstToReturn;
            }
        }
    }
}