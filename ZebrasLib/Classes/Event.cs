using Newtonsoft.Json;

namespace ZebrasLib
{
    namespace Classes
    {
        public class Event
        {
            [JsonProperty("ReportCount")]
            public int timesReported { get; set; }

            [JsonProperty("Verified")]
            public bool isVerified { get; set; }
            
            [JsonProperty("FbCode")]
            public string fbUserCode { get; set; }
           
            [JsonProperty("Latitude")]
            public double latitude { get; set; }

            [JsonProperty("Longitude")]
            public double longitude { get; set; }
           
            [JsonProperty("Type")]
            public string type { get; set; }

            [JsonProperty("Description")]
            public string description { get; set; }

            [JsonProperty("ReportedAt")]
            public string reportedAt { get; set; }
        }
    }
}