using System.Collections.Generic;
using HandyTools.Shenfen;
using SQLite;

namespace HandyTools.Tuili
{
    public class Category
    {
        [AutoIncrement, PrimaryKey]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string  Description { get; set; }
        public List<Item> Items { get; set; }
    }
}
