using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DatFileTypeIdRangeAttribute : Attribute
    {
        public uint BeginRange { get; set; }

        public uint EndRange { get; set; }

        public DatFileTypeIdRangeAttribute(uint beginRange, uint endRange)
        {
            this.BeginRange = beginRange;
            this.EndRange = endRange;
        }
    }
}
