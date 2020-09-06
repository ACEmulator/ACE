using System.IO;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x31.
    /// This is called a "String" in the client; It has been renamed to avoid conflicts with the generic "String" class.
    /// </summary>
    [DatFileType(DatFileType.String)]
    public class LanguageString : FileType
    {
        public string CharBuffer;

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            uint strLen = reader.ReadCompressedUInt32();
            if (strLen > 0)
            {
                byte[] thestring = reader.ReadBytes((int)strLen);
                CharBuffer = System.Text.Encoding.Default.GetString(thestring);
            }
        }
    }
}
