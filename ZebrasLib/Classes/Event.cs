using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebrasLib
{
    namespace Classes
    {
        public class Event
        {
            [JsonProperty("code")]
            public string code { get; set; }

            [JsonProperty("fbUserCode")]
            public string fbUserCode { get; set; }

            [JsonProperty("type")]
            public string type{ get; set; }

            [JsonProperty("description")]
            public string description { get; set; }

            [JsonProperty("reportedAt")]
            public string reportedAt { get; set; }

            [JsonProperty("accuracyDegree")]
            public int accuracyDegree {get; set; }

            [JsonProperty("latitude")]
            public double latitude{ get; set; }

            [JsonProperty("longitude")]
            public double longitude { get; set; }
        }
    }
}
