using System;

namespace ACE.DatLoader
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DatFileTypeIdRangeAttribute : Attribute
    {
        public uint BeginRange { get; set; }

        public uint EndRange { get; set; }

        public DatFileTypeIdRangeAttribute(uint beginRange, uint endRange)
        {
            BeginRange = beginRange;
            EndRange = endRange;
        }
    }
}
