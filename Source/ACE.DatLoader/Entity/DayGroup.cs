using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class DayGroup
    {
        public float ChanceOfOccur { get; set; }
        public string DayName { get; set; }


        public static DayGroup Read(DatReader datReader)
        {
            DayGroup obj = new DayGroup();
            Console.WriteLine("DayGroup");
            return obj;
        }
    }
}
