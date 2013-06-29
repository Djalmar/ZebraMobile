using System;
using System.Collections.Generic;
using System.Linq;
using ZebrasLib.Classes;
namespace DBPhone
{
    public class PlacesMethods
    {
        public static void AddItems(List<ZebrasLib.Classes.Place> lstItems)
        {
            List<Place> listToAdd = ConvertToDBItems(lstItems);
            Context context = Context.GetDatabase();
            context.places.InsertAllOnSubmit(listToAdd);
            context.SubmitChanges();
            context.Dispose();
        }

        public static void RemoveItems(string code)
        {
            Context context = Context.GetDatabase();
            List<string> codes = CategoriesMethods.GetSubCategoriesCodes(code);

            IEnumerable < Place > queryResult = from selectedPlace
                                                in context.places
                                                where codes.Contains(selectedPlace.categoryCode)
                                                select selectedPlace;

            foreach (Place P in queryResult)
                context.places.DeleteOnSubmit(P);
            context.SubmitChanges();
        }

        public static List<ZebrasLib.Classes.Place> GetItems(List<string> codes)
        {
            Context context = Context.GetDatabase();
            IEnumerable<Place> queryResult = from selectedPlace
                                             in context.places
                                             where codes.Contains(selectedPlace.categoryCode)
                                             select selectedPlace;
            List<Place> listToReturn = queryResult.ToList();
            context.Dispose();
            return ConvertToZebraItems(listToReturn);
        }

        private static List<ZebrasLib.Classes.Place> ConvertToZebraItems(List<Place> lstItems)
        {
            List<ZebrasLib.Classes.Place> listToReturn = new List<ZebrasLib.Classes.Place>();
            foreach (Place P in lstItems)
            {
                listToReturn.Add(new ZebrasLib.Classes.Place
                {
                    address = P.address,
                    categoryCode = P.categoryCode,
                    parentCategoryCode = P.parentCategoryCode,
                    delivery = P.delivery,
                    distance = P.distance,
                    holidays = P.holidays,
                    code = P.placecode,
                    kidsArea = P.kidsArea,
                    latitude = P.latitude,
                    longitude = P.longitude,
                    maxPrice = P.maxPrice,
                    minPrice = P.minPrice,
                    name = P.name,
                    parking = P.parking,
                    rating = P.rating,
                    smokingArea = P.smokingArea,
                    webSite = P.webSite
                });
            }
            return listToReturn;
        }

        private static List<Place> ConvertToDBItems(List<ZebrasLib.Classes.Place> lstItems)
        {
            List<Place> listToReturn = new List<Place>();
            foreach (ZebrasLib.Classes.Place P in lstItems)
            {
                listToReturn.Add(new Place
                {
                    address = P.address,
                    categoryCode = P.categoryCode,
                    parentCategoryCode = P.parentCategoryCode,
                    delivery = P.delivery,
                    distance = P.distance,
                    holidays = P.holidays,
                    placecode = P.code,
                    kidsArea = P.kidsArea,
                    latitude = P.latitude,
                    longitude = P.longitude,
                    maxPrice = P.maxPrice,
                    minPrice = P.minPrice,
                    name = P.name,
                    parking = P.parking,
                    rating = P.rating,
                    smokingArea = P.smokingArea,
                    webSite = P.webSite
                });

            }
            return listToReturn;
        }

        public static List<ZebrasLib.Classes.Place> getRelatedPlacesBasedOn(int minPrice, int maxPrice, string parentCategoryCode,string placeCode)
        {
            int minRateHigh = (minPrice * 20 / 100) + minPrice;
            int minRateLow = minPrice - (minPrice * 20 / 100);
            int maxRateHigh = (maxPrice * 20 / 100) + maxPrice;
            int maxRateLow = maxPrice - (maxPrice * 20 / 100);
            Context context = Context.GetDatabase();
            IEnumerable<Place> newList = from allPlaces
                                            in context.places
                                            where allPlaces.minPrice <= minRateHigh
                                            && allPlaces.minPrice >= minRateLow
                                            && allPlaces.maxPrice <= maxRateHigh
                                            && allPlaces.maxPrice >= maxRateLow
                                            && allPlaces.placecode != placeCode
                                            && allPlaces.parentCategoryCode == parentCategoryCode
                                            select allPlaces;
            
            return ConvertToZebraItems(newList.ToList());
        }

        public static List<ZebrasLib.Classes.Place> getRelatedPlacesBasedOn(bool childsArea, bool smokingArea, string parentCategoryCode, string placeCode)
        {
            Context context = Context.GetDatabase();
            IEnumerable<Place> newList =    from allPlaces
                                            in context.places
                                            where allPlaces.smokingArea == smokingArea
                                            && allPlaces.kidsArea == childsArea
                                            && allPlaces.placecode != placeCode
                                            && allPlaces.parentCategoryCode == parentCategoryCode
                                            select allPlaces;
            return ConvertToZebraItems(newList.ToList());
        }
    }
}