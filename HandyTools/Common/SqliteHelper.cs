using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyTools.Shenfen;
using SQLitePCL;

namespace HandyTools.Common
{
    public class SqliteHelper
    {
        private static SQLiteConnection GetConnection()
        {
            SQLiteConnection connection = null;
            try
            {
                connection = new SQLiteConnection("app.db");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return connection;
        }

        public static List<Category> GeTypes()
        {
            List<Category> types = new List<Category>();
            try
            {
                SQLiteConnection connection = GetConnection();
                using (var statement = connection.Prepare(@"select id, name, url from categories"))
                {
                    Category type = new Category()
                    {
                        Id = (long)statement[0],
                        Name = (string)statement[1],
                        Url = (string)statement[2]
                    };
                    types.Add(type);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return types;
        } 

    }
}
