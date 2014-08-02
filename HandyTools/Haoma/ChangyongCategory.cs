using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyTools.Haoma
{
    public class ChangyongCategory
    {
        public string UpdateTime { get; set; }
        public string Title { get; set; }
        public List<Changyong> Changyongs { get; set; }
        public double Width { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ChangyongCategory category = (ChangyongCategory) obj;
            return category.Title.Equals(this.Title);

        }

// override object.GetHashCode
        public override int GetHashCode()
        {
            return this.Title.GetHashCode();
        }
    }
}
