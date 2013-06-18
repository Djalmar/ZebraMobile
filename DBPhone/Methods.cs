using System.Collections.Generic;
using System.Linq;

namespace DBPhone
{
    internal class Methods
    {
        private static Context context;

        private static Context GetDatabase()
        {
            context = new Context("isostore:/Ringtones.sdf");

            if (!context.DatabaseExists())
                context.CreateDatabase();
            return context;
        }

        public static void AddPlaces(List<Place> lstPlaces)
        {
            context = GetDatabase();
            context.places.InsertAllOnSubmit(lstPlaces);
            context.SubmitChanges();
            context.Dispose();
        }

        public static void RemovePlaces()
        {
            context = GetDatabase();
            context.DeleteDatabase();
        }

        public static List<Place> GetPlaces()
        {
            context = GetDatabase();
            IEnumerable<Place> queryResult = from selectedPlace
                                             in context.places
                                             select selectedPlace;

            return queryResult.ToList();
        }
    }
}