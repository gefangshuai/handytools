using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyTools.Common;
using HandyTools.Shenfen;
using HandyTools.Tuili;

namespace HandyTools.Data
{
    public static class AppData
    {
        public static List<Category> Categories;
        public static List<Item> Items;
        public static Item Item;

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

        /// <summary>
        /// 获取所有星座数据
        /// </summary>
        /// <returns></returns>
        public static List<Star> GetStars()
        {
            return new List<Star>()
            {
                new Star(){Id = 0, Title = "白羊座", StartDate = "03/21", EndDate = "04/19"},
                new Star(){Id = 1, Title = "金牛座", StartDate = "04/20", EndDate = "05/20"},
                new Star(){Id = 2, Title = "双子座", StartDate = "05/21", EndDate = "06/21"},
                new Star(){Id = 3, Title = "巨蟹座", StartDate = "06/22", EndDate = "07/22"},
                new Star(){Id = 4, Title = "狮子座", StartDate = "07/23", EndDate = "08/22"},
                new Star(){Id = 5, Title = "处女座", StartDate = "08/23", EndDate = "09/22"},
                new Star(){Id = 6, Title = "天秤座", StartDate = "09/23", EndDate = "10/23"},
                new Star(){Id = 7, Title = "天蝎座", StartDate = "10/24", EndDate = "11/22"},
                new Star(){Id = 8, Title = "射手座", StartDate = "11/23", EndDate = "12/21"},
                new Star(){Id = 9, Title = "魔羯座", StartDate = "12/22", EndDate = "01/19"},
                new Star(){Id = 10, Title = "水瓶座", StartDate = "01/20", EndDate = "02/18"},
                new Star(){Id = 11, Title = "双鱼座", StartDate = "02/19", EndDate = "03/20"}
            };
        } 

    }

    public class Star
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Title { get; set; }

        public override string ToString()
        {
            return string.Format("{0}({1}-{2})", Title, StartDate, EndDate);
        }
    }
}
