using Newtonsoft.Json;

namespace ZebrasLib
{
    namespace Classes
    {
        public class Category
        {
            [JsonProperty("code")]
            public string code { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("spanishName")]
            public string spanishName { get; set; }

            [JsonProperty("icon")]
            public string icon { get; set; }

            [JsonProperty("parentId")]
            public string parentId { get; set; }

            [JsonProperty("parent")]
            public Category parent { get; set; }
        }
    }
}