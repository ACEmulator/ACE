using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class NameFilterLanguageData : IUnpackable
    {
        public uint MaximumSameCharactersInARow;
        public uint MaximumVowelsInARow; 
        public uint FirstNCharactersMustHaveAVowel;
        public uint VowelContainingSubstringLength;
        public string ExtraAllowedCharacters;

        public List<string> CompoundLetterGroups = new List<string>();

        public void Unpack(BinaryReader reader)
        {
            MaximumSameCharactersInARow = reader.ReadUInt32();
            MaximumVowelsInARow = reader.ReadUInt32();
            FirstNCharactersMustHaveAVowel = reader.ReadUInt32();
            VowelContainingSubstringLength = reader.ReadUInt32();
            ExtraAllowedCharacters = reader.ReadUnicodeString();

            uint numLetterGroup = reader.ReadUInt32();
            for (uint i = 0; i < numLetterGroup; i++)
                CompoundLetterGroups.Add(reader.ReadUnicodeString());
        }
    }
}
