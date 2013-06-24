using System.Data.Linq;

namespace DBPhone
{
    public class Context:DataContext
    {
        public Table<Place> places;
        public Table<Category> categories;

        public Context(string connectionString):
            base(connectionString){}

        public static Context GetDatabase()
        {
            Context context = new Context("isostore:/Zebritas.sdf");

            if (!context.DatabaseExists())
                context.CreateDatabase();
            return context;
        }

    }
}