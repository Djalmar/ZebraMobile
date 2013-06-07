using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZebrasLib
{
    namespace Classes
    {
        public class Place
        {
            [JsonProperty("Id")]
            public long id { get; set; }

            [JsonProperty("Code")]
            public string categoryCode { get; set; }

            [JsonProperty("Name")]
            public string name { get; set; }

            [JsonProperty("Address")]
            public string address { get; set; }

            [JsonProperty("WebSite")]
            public string webSite { get; set; }

            [JsonProperty("MinPrice")]
            public double minPrice { get; set; }

            [JsonProperty("MaxPrice")]
            public double maxPrice { get; set; }

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
            public double rating { get; set; }

            [JsonProperty("Latitude")]
            public double latitude { get; set; }

            [JsonProperty("Longitude")]
            public double longitude { get; set; }
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