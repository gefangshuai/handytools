using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using HandyTools.Shenfen;
using HandyTools.Tuili;
using SQLite;

namespace HandyTools.Common
{
    public static class SqliteHelper
    {
        private const string DbName = "app.db";

        public static async void InitDb()
        {
            try
            {
                StorageFile seedFile = await StorageFile.GetFileFromPathAsync(
                    Path.Combine(Package.Current.InstalledLocation.Path, DbName));
                // copy the StorageFile to the ApplicationData folder
                await seedFile.CopyAsync(ApplicationData.Current.LocalFolder, DbName, NameCollisionOption.FailIfExists);
            }
            catch (Exception)
            {
            }
        }

        private static SQLiteAsyncConnection GetConnection()
        {
            SQLiteAsyncConnection connection = null;
            try
            {
                connection =
                    new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return connection;
        }


        public static async Task<List<Category>> GetCategories()
        {
            List<Category> types = new List<Category>();
            try
            {
                SQLiteAsyncConnection db = GetConnection();
                types = await db.QueryAsync<Category>("select * from Category");
                foreach (var category in types)
                {
                    List<Item> items = await db.QueryAsync<Item>("select * from Item where TypeId=" + category.Id);
                    category.Items = items;
                    category.Description = string.Format("包含{0}、{1}等{2}个梦境", items[0].Title, items[1].Title, items.Count);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return types;
        }

        public static async Task<List<StarDay>> GetStarDays(string day, int starId)
        {
            List<StarDay> starDays = new List<StarDay>();
            try
            {
                SQLiteAsyncConnection db = GetConnection();
                starDays = await db.QueryAsync<StarDay>("select * from StarDay where Day = ? and StarId = ?", day, starId );
            }
            catch (Exception)
            {
                throw;
            }
            return starDays;
        }

        public static async void SaveStarDay(StarDay star)
        {
            SQLiteAsyncConnection db = GetConnection();
            await db.InsertAsync(star);
        }

        internal static async Task ClearStarDay()
        {
            SQLiteAsyncConnection db = GetConnection();
            await db.ExecuteScalarAsync<StarDay>("delete from StarDay");
        }
    }
}
