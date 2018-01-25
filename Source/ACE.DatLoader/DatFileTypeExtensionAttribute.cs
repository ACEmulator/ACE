using System;

namespace ACE.DatLoader
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class DatFileTypeExtensionAttribute : Attribute
    {
        public string FileExtension { get; set; }

        public DatFileTypeExtensionAttribute(string fileExtension)
        {
            FileExtension = fileExtension;
        }
    }
}
