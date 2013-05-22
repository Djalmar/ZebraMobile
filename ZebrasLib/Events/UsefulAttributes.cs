using Newtonsoft.Json;
using QuickMethods;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ZebrasLib.Classes;

namespace ZebrasLib
{
    namespace Events
    {
        public static partial class EventsMethods
        {
            public static Uri ReportProblemUri = new Uri("http://stcbolivia.net/api/alertas/reportar", UriKind.Absolute);
            public static string GetProblemsUri = "http://stcbolivia.net/api/alertas/get";

            public static WebClient client;

            public class EventResult
            {
                [JsonProperty("Error")]
                public string errorMessage { get; set; }

                [JsonProperty("Success")]
                public string successMessage { get; set; }

                [JsonProperty("Events")]
                public List<Event> eventsList { get; set; }
            }

            private static async Task<EventResult> downloadedInfo(Uri uriAddress)
            {
                client = new WebClient();
                string result = await Internet.DownloadStringAsync(client, uriAddress);
                return JsonConvert.DeserializeObject<EventResult>(result);
            }
        }
    }
}