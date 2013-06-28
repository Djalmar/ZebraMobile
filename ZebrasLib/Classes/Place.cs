using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZebrasLib
{
    namespace Classes
    {
        public class Place
        {
            [JsonProperty("Code")]
            public string code { get; set; }

            [JsonProperty("Name")]
            public string name { get; set; }

            [JsonProperty("Address")]
            public string address { get; set; }

            [JsonProperty("WebSite")]
            public string webSite { get; set; }

            [JsonProperty("MinPrice")]
            public int minPrice { get; set; }

            [JsonProperty("MaxPrice")]
            public int maxPrice { get; set; }

            [JsonProperty("Parking")]
            public bool parking { get; set; }

            [JsonProperty("Holidays")]
            public bool holidays { get; set; }

            [JsonProperty("SmokingArea")]
            public bool smokingArea { get; set; }

            [JsonProperty("KidsArea")]
            public bool kidsArea { get; set; }

            [JsonProperty("Delivery")]
            public bool delivery { get; set; }

            [JsonProperty("Rating")]
            public int rating { get; set; }

            [JsonProperty("Latitude")]
            public double latitude { get; set; }

            [JsonProperty("Longitude")]
            public double longitude { get; set; }

            [JsonProperty("CategoryCode")]
            public string categoryCode { get; set; }

            public double distance { get; set; }

            public string parentCategoryCode { get; set; }
        }

        public class PlacesResult
        {
            [JsonProperty("Status")]
            public string status { get; set; }

            [JsonProperty("Message")]
            public string message { get; set; }

            [JsonProperty("Data")]
            public List<Place> placesList { get; set; }
        }
    }
}