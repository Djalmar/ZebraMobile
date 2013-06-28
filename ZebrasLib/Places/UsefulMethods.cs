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
                if (result != null)
                {
                    if (Main.thereIsNoProblemo(result.status))
                        return FormatedCategories(result.categoriesList);
                }
                return null;
            }

            private static List<Category> FormatedCategories(List<Category> list)
            {
                foreach (Category C in list)
                    C.icon = "/Assets/Icons/Categories/" + C.icon;
                return list;
            }
        }
    }
}
