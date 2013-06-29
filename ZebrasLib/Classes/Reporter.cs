using Newtonsoft.Json;
using System;
namespace ZebrasLib
{
    namespace Classes
    {
        public class Reporter
        {
            [JsonProperty("FacebookCode")]
            public string facebookCode { get; set; }

            [JsonProperty("Latitude")]
            public string strlatitude { get; set; }

            [JsonProperty("Longitude")]
            public string strlongitude { get; set; }

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

            public double latitude { get; set; }
            public double longitude { get; set; }
            public DateTime dtReportedAt { get; set; }
        }
    }
}