using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZebrasLib.Classes;
namespace DBPhone
{
    public class CategoriesMethods
    {
        public static void AddItems(List<ZebrasLib.Classes.Category> lstItems)
        {   
            List<Category> listToAdd = ConvertToDBItems(lstItems);
            Context context = Context.GetDatabase();
            context.categories.InsertAllOnSubmit(listToAdd);
            context.SubmitChanges();
            context.Dispose();
        }

        public static List<ZebrasLib.Classes.Category> GetItems()
        {
            Context context = Context.GetDatabase();
            IEnumerable<Category> queryResult = from selectedItem
                                             in context.categories
                                             orderby selectedItem.name ascending
                                             select selectedItem;
            return ConvertToZebraItems(queryResult.ToList());
        }

        public static List<string> GetSubCategoriesCodes(string code)
        {
            Context context = Context.GetDatabase();
            IEnumerable<string> queryResult = from selectedItem
                                                in context.categories
                                                where selectedItem.parentCode == code
                                                select selectedItem.categoryCode;
            return queryResult.ToList();
        }
        private static List<ZebrasLib.Classes.Category> ConvertToZebraItems(List<Category> lstItems)
        {
            List<ZebrasLib.Classes.Category> listToReturn = new List<ZebrasLib.Classes.Category>();
            foreach (Category C in lstItems)
            {
                #region subCategories
                IEnumerable<Category> query = from AllCategories
                                                      in lstItems
                                              where AllCategories.parentCode == C.categoryCode
                                              select AllCategories;
                List<ZebrasLib.Classes.Category> subCategories = new List<ZebrasLib.Classes.Category>();
                foreach (Category subC in query)
                {
                    subCategories.Add(new ZebrasLib.Classes.Category
                    {
                        code = subC.categoryCode,
                        icon = subC.icon,
                        name = subC.name,
                        parentCode = subC.parentCode,
                        spanishName = subC.spanishName
                    });
                }
                #endregion
                if (C.parentCode == "")
                    listToReturn.Add(new ZebrasLib.Classes.Category
                    {
                        code = C.categoryCode,
                        icon = C.icon,
                        name = C.name,
                        parentCode = C.parentCode,
                        spanishName = C.spanishName,
                        subCategories = subCategories
                    });
            }
            return listToReturn;
        }

        private static List<Category> ConvertToDBItems(List<ZebrasLib.Classes.Category> lstItems)
        {
            List<Category> listToReturn = new List<Category>();
            foreach (ZebrasLib.Classes.Category P in lstItems)
            {
                listToReturn.Add(new Category
                {
                    categoryCode = P.code,
                    icon = P.icon,
                    name = P.name,
                    parentCode = P.parentCode,
                    spanishName = P.spanishName,
                });

                foreach (ZebrasLib.Classes.Category C in P.subCategories)
                {
                    listToReturn.Add(new Category
                    {
                        categoryCode = C.code,
                        icon = C.icon,
                        name = C.name,
                        parentCode = C.parentCode,
                        spanishName = C.spanishName,
                    });
                }
            }
            return listToReturn;
        }

        public static string GetParentCode(string categoryCodeForThisPlace)
        {
            Context context = Context.GetDatabase();
            IEnumerable<string> categorie = from thiscategory
                                              in context.categories
                                            where thiscategory.categoryCode == categoryCodeForThisPlace
                                            select thiscategory.parentCode;
            if (categorie.Count() > 0)
                return categorie.First().ToString();
            else return null;
        }

        public static bool isChildForThisCategory(string categoryCode,string parentCategory)
        {
            Context context = Context.GetDatabase();
            IEnumerable<Category> categorie = from thiscategory
                                              in context.categories
                                            where thiscategory.categoryCode == categoryCode
                                            && thiscategory.parentCode == parentCategory
                                            select thiscategory;
            List<Category> results = categorie.ToList();
            if (results.Count() > 0)
                return true;
            else return false;
        }
    }
}
