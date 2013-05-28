using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZebrasLib
{
    namespace Classes
    {
        public class Event
        {
            [JsonProperty("facebookCode")]
            public string facebookUserCode { get; set; }

            [JsonProperty("importance")]
            public int importance { get; set; }

            [JsonProperty("verified")]
            public bool isVerified { get; set; }
           
            [JsonProperty("latitude")]
            public double latitude { get; set; }

            [JsonProperty("longitude")]
            public double longitude { get; set; }

            [JsonProperty("reportedAt")]
            public string reportedAt { get; set; }

            [JsonProperty("type")]
            public int type { get; set; }

            [JsonProperty("description")]
            public string description { get; set; }

            [JsonProperty ("reporters")]
            public List<Reporter> reporters { get; set; }
        }
    }
}