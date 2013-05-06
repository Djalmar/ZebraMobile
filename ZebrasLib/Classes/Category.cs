using Newtonsoft.Json;

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

            [JsonProperty("ParentId")]
            public long parentId { get; set; }

            [JsonProperty("Parent")]
            public Category parent { get; set; }
        }
    }
}