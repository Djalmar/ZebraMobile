using Newtonsoft.Json;

namespace ZebrasLib
{
    namespace Classes
    {
        public class Reporter
        {
            [JsonProperty("facebookCode")]
            public string facebookCode { get; set; }

            [JsonProperty("reportedAt")]
            public string reportedAt { get; set; }

            [JsonProperty("description")]
            public string description { get; set; }
        }
    }
}