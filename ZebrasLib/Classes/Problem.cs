using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ZebrasLib
{
    namespace Classes
    {
        public class Problem
        {
            [JsonProperty("Code")]
            public string eventCode{ get; set; }

            [JsonProperty("FacebookCode")]
            public string facebookUserCode { get; set; }

            [JsonProperty("Importance")]
            public int importance { get; set; }

            [JsonProperty("Verified")]
            public bool isVerified { get; set; }

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
            
            [JsonProperty("Reporters")]
            public List<Reporter> reporters { get; set; }

            public DateTime dtReportedAt{ get; set; }
            public string icon { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public int distance { get; set; }
        }

        public class ProblemsResult
        {
            [JsonProperty("Status")]
            public string status { get; set; }

            [JsonProperty("Message")]
            public string message { get; set; }

            [JsonProperty("Data")]
            public List<Problem> problemsList { get; set; }
        }
    }
}