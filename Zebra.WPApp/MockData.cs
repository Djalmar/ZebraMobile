using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;
using ZebrasLib.Classes;
namespace Zebra.WPApp
{
    class MockData
    {
        public static async Task<string> GetStringFromStream(string direction)
        {
            Uri uri = new Uri(direction, UriKind.Relative);
            StreamResourceInfo stream = Application.GetResourceStream(uri);
            StreamReader reader = new StreamReader(stream.Stream);
            return await reader.ReadToEndAsync();
        }

        public static async Task<ProblemsResult> MockDataGetEvents()
        {
            string direction = "MockData/EventsResult.json";

            string result = await GetStringFromStream(direction);

            ProblemsResult eventResult = JsonConvert.DeserializeObject<ProblemsResult>(result);
            return eventResult;
        }

        public static Task<List<Category>> MockDataGetCategories()
        {
            string direction = "MockData/CategoriesResult.json";

            return GetCategoriesForThisJson(direction);
        }

        public static Task<List<Category>> MockDataGetSubCategories()
        {
            string direction = "MockData/SubCategoriesResult.json";

            return GetCategoriesForThisJson(direction);
        }

        public static async Task<PlacesResult> MockDataGetPlaces()
        {
            string direction = "MockData/PlacesResult.json";
            string result = await GetStringFromStream(direction);

            PlacesResult res = JsonConvert.DeserializeObject<PlacesResult>(result);
            return res;
        }

        private static async Task<List<Category>> GetCategoriesForThisJson(string direction)
        {
            string result = await GetStringFromStream(direction);

            return JsonConvert.DeserializeObject<List<Category>>(result);
        }
    }
}
