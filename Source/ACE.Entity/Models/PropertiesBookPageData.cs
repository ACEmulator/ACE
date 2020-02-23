using System;

namespace ACE.Entity.Models
{
    public class PropertiesBookPageData
    {
        public uint AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAccount { get; set; }
        public bool IgnoreAuthor { get; set; }
        public string PageText { get; set; }

        public PropertiesBookPageData Clone()
        {
            var result = new PropertiesBookPageData
            {
                AuthorId = AuthorId,
                AuthorName = AuthorName,
                AuthorAccount = AuthorAccount,
                IgnoreAuthor = IgnoreAuthor,
                PageText = PageText,
            };

            return result;
        }
    }
}
