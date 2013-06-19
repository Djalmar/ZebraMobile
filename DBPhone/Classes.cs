using System.Data.Linq.Mapping;
namespace DBPhone
{
    [Table]
    public class Place
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int DBid { get; set; }

        [Column]
        public long id { get; set; }

        [Column]
        public string categoryCode { get; set; }

        [Column]
        public string name { get; set; }

        [Column]
        public string address { get; set; }

        [Column]
        public string webSite { get; set; }

        [Column]
        public double minPrice { get; set; }

        [Column]
        public double maxPrice { get; set; }

        [Column]
        public bool parking { get; set; }

        [Column]
        public bool holidays { get; set; }

        [Column]
        public bool smokingArea { get; set; }

        [Column]
        public bool kidsArea { get; set; }

        [Column]
        public bool delivery { get; set; }

        [Column]
        public double rating { get; set; }

        [Column]
        public double latitude { get; set; }

        [Column]
        public double longitude { get; set; }

        [Column]
        public double distance { get; set; }
    }
}