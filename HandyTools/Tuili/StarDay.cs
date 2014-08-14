using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace HandyTools.Tuili
{
    /// <summary>
    /// 每日星座运势
    /// </summary>
    public class StarDay
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Rank { get; set; }
        public string Value { get; set; }
        public string Day { get; set; }
        public int StarId { get; set; }
    }
}
