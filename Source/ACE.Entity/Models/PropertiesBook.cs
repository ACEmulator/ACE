using System;

namespace ACE.Entity.Models
{
    public class PropertiesBook
    {
        public int MaxNumPages { get; set; }
        public int MaxNumCharsPerPage { get; set; }

        public PropertiesBook Clone()
        {
            var result = new PropertiesBook
            {
                MaxNumPages = MaxNumPages,
                MaxNumCharsPerPage = MaxNumCharsPerPage,
            };

            return result;
        }
    }
}
