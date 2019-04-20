using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.TabooTable)]
    public class TabooTable : FileType
    {
        internal const uint FILE_ID = 0x0E00001E;

        /// <summary>
        /// The key is a 32 bit flag variable, and only one flag is set per entry.<para />
        /// In the current dats, this isn't used for anything. All tables share the same values for any given flag.<para />
        /// It's possible the intended use for the flags was to separate words based on the type of offense, ie: racist, sexual, harassment, etc...
        /// </summary>
        public Dictionary<uint, TabooTableEntry> TabooTableEntries { get; } = new Dictionary<uint, TabooTableEntry>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            // I don't actually know the structure of TabooTableEntries. It could be a Dictionary as I have it defined, or it could be a List where the key is just a variable in TabooTableEntry
            // I was unable to find the unpack code in the client. If someone can point me to it, I can make sure we match what the client is doing. - Mag

            /*var x01 = */reader.ReadByte();
            var length = reader.ReadByte();

            TabooTableEntries.Unpack(reader, length);
        }

        /// <summary>
        /// This will search all the first entry to see if the input passes or fails.<para />
        /// Only the first entry is searched (for now) because they're all the same.
        /// </summary>
        public bool ContainsBadWord(string input)
        {
            foreach (var kvp in TabooTableEntries)
            {
                if (kvp.Value.ContainsBadWord(input))
                    return true;

                // If in the future, the dat is changed so that each entry has unique patterns, remove this break.
                break;
            }

            return false;
        }
    }
}
