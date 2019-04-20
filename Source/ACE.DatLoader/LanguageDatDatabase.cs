using ACE.DatLoader.FileTypes;

namespace ACE.DatLoader
{
    public class LanguageDatDatabase : DatDatabase
    {
        public LanguageDatDatabase(string filename, bool keepOpen = false) : base(filename, keepOpen)
        {
            CharacterTitles = ReadFromDat<StringTable>(StringTable.CharacterTitle_FileID);
        }

        public StringTable CharacterTitles { get; }
    }
}
