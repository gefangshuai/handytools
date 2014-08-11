using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace HandyTools.Shenfen
{
    public class Category
    {
        [AutoIncrement, PrimaryKey]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
