using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyTools.Common;
using HandyTools.Shenfen;

namespace HandyTools.Data
{
    public static class AppData
    {
        public static List<Category> Categories;
        public static List<Item> Items;

        internal static async Task<List<Category>> InitCategoriesData()
        {
            Categories = await SqliteHelper.GetCategories();
            Items = new List<Item>();
            foreach (var category in Categories)
            {
                Items.AddRange(category.Items);
            }
            return Categories;
        }

        public static Item Item;
    }
}
