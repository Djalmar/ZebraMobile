using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZebrasLib.Classes;
using Newtonsoft.Json;

namespace ZebrasLib
{
    namespace Places
    {
        public partial class PlacesMethods
        {
            private static async Task<List<Category>> GetCategories(string url)
            {
                string downloadedInfo = await Main.DownloadInfo(url);
                CategoryResult result = JsonConvert.DeserializeObject<CategoryResult>(downloadedInfo);
                if (Main.thereIsNoProblemo(result.status))
                    return result.categoriesList;
                else return null;
            }
        }
    }
}
