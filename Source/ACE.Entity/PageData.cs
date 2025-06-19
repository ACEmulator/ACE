// UTF-8 BOM removed to ensure consistent encoding
namespace ACE.Entity
{
    public class PageData
    {
        public uint AuthorID;
        public string AuthorName;
        public string AuthorAccount;
        public bool IgnoreAuthor;
        public string PageText;
        public uint PageIdx; // 0 based
    }
}
