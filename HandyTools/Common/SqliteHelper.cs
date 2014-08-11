using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using HandyTools.Shenfen;
using SQLite;
using SQLiteConnection = SQLitePCL.SQLiteConnection;

namespace HandyTools.Common
{
    public static class SqliteHelper
    {
        private const string DbName = "app.db";

        public static async void InitDb()
        {
            StorageFile seedFile = await StorageFile.GetFileFromPathAsync(
                Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, DbName));
            // copy the StorageFile to the ApplicationData folder
            await seedFile.CopyAsync(ApplicationData.Current.LocalFolder, DbName, NameCollisionOption.ReplaceExisting);
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

        public static async Task<List<Category>> GeTypes()
        {
            List<Category> types = new List<Category>();
            try
            {
                SQLiteAsyncConnection db = GetConnection();
                types = await db.QueryAsync<Category>("select * from Category");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return types;
        }

    }
}
