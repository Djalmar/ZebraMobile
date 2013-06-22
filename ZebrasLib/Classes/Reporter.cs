using Newtonsoft.Json;

namespace ZebrasLib
{
    namespace Classes
    {
        public class Reporter
        {
            [JsonProperty("FacebookCode")]
            public string facebookCode { get; set; }

            [JsonProperty("Latitude")]
            public double latitude { get; set; }

            [JsonProperty("Longitude")]
            public double longitude { get; set; }

            [JsonProperty("Type")]
            public int type { get; set; }

            [JsonProperty("Description")]
            public string description { get; set; }

            [JsonProperty("ReportedAt")]
            public string reportedAt { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("picture")]
            public string picture { get; set; }
        }
    }
}