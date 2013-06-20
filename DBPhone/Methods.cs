using System.Collections.Generic;
using System.Linq;
using ZebrasLib.Classes;
namespace DBPhone
{
    public class Methods
    {

        private static Context GetDatabase()
        {
            Context context = new Context("isostore:/Zebritas.sdf");

            if (!context.DatabaseExists())
                context.CreateDatabase();
            return context;
        }

        public static void AddPlaces(List<ZebrasLib.Classes.Place> lstPlaces)
        {
            List<Place> listToAdd = ConvertToDBPlaces(lstPlaces);
            Context context = GetDatabase();
            context.places.InsertAllOnSubmit(listToAdd);
            context.SubmitChanges();
            context.Dispose();
        }

        public static void RemovePlaces()
        {
            Context context = GetDatabase();
            context.DeleteDatabase();
        }

        public static List<ZebrasLib.Classes.Place> GetPlaces()
        {
            Context context = GetDatabase();
            IEnumerable<Place> queryResult = from selectedPlace
                                             in context.places
                                             select selectedPlace;

            return ConvertToZebraPlaces(queryResult.ToList());
        }

        private static List<ZebrasLib.Classes.Place> ConvertToZebraPlaces(List<Place> list)
        {
            List<ZebrasLib.Classes.Place> listToReturn = new List<ZebrasLib.Classes.Place>();
            foreach (Place P in list)
            {
                listToReturn.Add(new ZebrasLib.Classes.Place { 
                    address = P.address,
                    categoryCode = P.categoryCode,
                    delivery = P.delivery,
                    distance = P.distance,
                    holidays = P.holidays,
                    id = P.id,
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

        private static List<Place> ConvertToDBPlaces(List<ZebrasLib.Classes.Place> list)
        {
            List<Place> listToReturn = new List<Place>();
            foreach (ZebrasLib.Classes.Place P in list)
            {
                listToReturn.Add(new Place
                {
                    address = P.address,
                    categoryCode = P.categoryCode,
                    delivery = P.delivery,
                    distance = P.distance,
                    holidays = P.holidays,
                    id = P.id,
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
    }
}