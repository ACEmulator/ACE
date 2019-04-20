using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ACE.DatLoader.Entity
{
    public class TabooTableEntry : IUnpackable
    {

        // This always seems to be 0x00010101
        public uint Unknown1 { get; set; }

        // This always seems to be 0
        public ushort Unknown2 { get; set; }

        /// <summary>
        /// All patterns are lower case<para />
        /// Patterns are expected in the following format: [*]word[*]<para />
        /// The asterisk is optional. They can be used to forbid strings that contain a pattern, require the pattern to be the whole word, or require the word to either start/end with the pattern.
        /// </summary>
        public List<string> BannedPatterns { get; } = new List<string>();

        public void Unpack(BinaryReader reader)
        {
            Unknown1 = reader.ReadUInt32();
            Unknown2 = reader.ReadUInt16();

            uint count = reader.ReadUInt32();

            for (int i = 0; i < count; i++)
                BannedPatterns.Add(reader.ReadString());
        }

        /// <summary>
        /// This will search all the BannedPatterns to see if the input passes or fails.
        /// </summary>
        public bool ContainsBadWord(string input)
        {
            // Our entire banned patterns list should be lower case
            input = input.ToLower();

            // First, we need to split input into separate words
            var words = input.Split(' ');

            foreach (var word in words)
            {
                foreach (var bannedPattern in BannedPatterns)
                {
                    if (Regex.IsMatch(word, "^" + bannedPattern.Replace("*", ".*") + "$"))
                        return true;
                }
            }

            return false;
        }
    }
}
