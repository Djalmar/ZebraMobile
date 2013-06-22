using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZebrasLib
{
    namespace Classes
    {
        public class Category
        {
            [JsonProperty("Code")]
            public string code { get; set; }

            [JsonProperty("Name")]
            public string name { get; set; }

            [JsonProperty("SpanishName")]
            public string spanishName { get; set; }

            [JsonProperty("ParentCode")]
            public string parentCode { get; set; }

            [JsonProperty("Icon")]
            public string icon { get; set; }

            [JsonProperty("SubCategories")]
            public List<Category> subCategories{ get; set; }
            

        }

        public class CategoryResult
        {
            [JsonProperty("Status")]
            public string status { get; set; }

            [JsonProperty("Message")]
            public string message { get; set; }

            [JsonProperty("Data")]
            public List<Category> categoriesList { get; set; }
        }
    }
}