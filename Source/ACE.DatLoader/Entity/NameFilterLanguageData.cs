using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class NameFilterLanguageData : IUnpackable
    {
        public uint MaximumVowelsInARow; 
        public uint FirstNCharactersMustHaveAVowel;
        public uint VowelContainingSubstringLength;
        public uint ExtraAllowedCharacters;
        public byte Unknown;

        public List<string> CompoundLetterGroups = new List<string>();

        public void Unpack(BinaryReader reader)
        {
            MaximumVowelsInARow = reader.ReadUInt32();
            FirstNCharactersMustHaveAVowel = reader.ReadUInt32();
            VowelContainingSubstringLength = reader.ReadUInt32();
            ExtraAllowedCharacters = reader.ReadUInt32();

            Unknown = reader.ReadByte(); // Not sure what this is...

            uint numLetterGroup = reader.ReadUInt32();
            for (uint i = 0; i < numLetterGroup; i++)
                CompoundLetterGroups.Add(reader.ReadUnicodeString());
        }
    }
}
