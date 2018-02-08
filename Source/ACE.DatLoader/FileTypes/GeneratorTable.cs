using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// Class for reading the File 0x0E00000D from the portal.dat.
    /// Thanks alot Widgeon of Leafcull for his ACDataTools which helped understanding this structure.
    /// And thanks alot to Pea as well whos hard work surely helped in the creation of those Tools too.
    /// </summary>
    [DatFileType(DatFileType.ObjectHierarchy)]
    public class GeneratorTable : FileType
    {
        internal const uint FILE_ID = 0x0E00000D;

        public Generator Generators { get; } = new Generator();

        /// <summary>
        /// This is just a shortcut to Generators.Items[0].Items
        /// </summary>
        public List<Generator> PlayDayItems { get; private set; } = new List<Generator>();
        /// <summary>
        /// This is just a shortcut to Generators.Items[1].Items
        /// </summary>
        public List<Generator> WeenieObjectsItems { get; private set; } = new List<Generator>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            Generators.Unpack(reader);

            PlayDayItems = Generators.Items[0].Items;
            WeenieObjectsItems = Generators.Items[1].Items;
        }
    }
}
