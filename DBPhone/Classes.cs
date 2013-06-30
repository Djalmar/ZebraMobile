using System.Collections.Generic;
using System.Data.Linq.Mapping;
namespace DBPhone
{
    [Table]
    public class Place
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int DBid { get; set; }

        [Column]
        public string placecode{ get; set; }

        [Column]
        public string categoryCode { get; set; }

        [Column]
        public string parentCategoryCode { get; set; }

        [Column]
        public string name { get; set; }

        [Column]
        public string address { get; set; }

        [Column]
        public string webSite { get; set; }

        [Column]
        public int minPrice { get; set; }

        [Column]
        public int maxPrice { get; set; }

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
        public float rating { get; set; }

        [Column]
        public double latitude { get; set; }

        [Column]
        public double longitude { get; set; }

        [Column]
        public double distance { get; set; }
    }

    [Table]
    public class Category
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int DBid { get; set; }

        [Column]
        public string categoryCode { get; set; }

        [Column]
        public string name { get; set; }

        [Column]
        public string spanishName { get; set; }

        [Column]
        public string parentCode { get; set; }

        [Column]
        public string icon { get; set; }
    }
}