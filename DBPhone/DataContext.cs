using System.Data.Linq;

namespace DBPhone
{
    public class Context:DataContext
    {
        public Table<Place> places;

        public Context(string connectionString):
            base(connectionString){}
    }
}