using System;
using System.IO;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// The content of an individual page for parchment and tomes
    /// </summary>
    public class PageData
    {
        public uint AuthorGuid;
        public string AuthorName;
        public string AuthorAccount;
        public uint Version = 0xFFFF0002;   // if HIWORD is not 0xFFFF, this is textIncluded. For our purpose this should always be 0xFFFF0002
        public bool HasText;
        public bool IgnoreAuthor;

        public string PageText;
    }

    public static class PageDataExtensions
    {
        public static void Write(this BinaryWriter writer, PageData page)
        {
            writer.Write(page.AuthorGuid);
            writer.WriteString16L(page.AuthorName);
            writer.WriteString16L(page.AuthorAccount);
            writer.Write(page.Version);
            writer.Write(Convert.ToUInt32(page.HasText));
            writer.Write(Convert.ToUInt32(page.IgnoreAuthor));

            if (page.HasText)
                writer.WriteString16L(page.PageText);
        }
    }
}
