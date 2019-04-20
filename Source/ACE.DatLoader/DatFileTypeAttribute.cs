using System;

namespace ACE.DatLoader
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatFileTypeAttribute : Attribute
    {
        public DatFileType FileType { get; set; }

        public DatFileTypeAttribute(DatFileType fileType)
        {
            FileType = fileType;
        }
    }
}
