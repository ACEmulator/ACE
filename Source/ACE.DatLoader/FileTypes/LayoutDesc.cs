using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are in the client_local_English.dat in the 0x21 range and have details on different UI Elements
    /// The enum for these LayoutDesc can be looked up in DidMapper file 0x2500000E
    /// </summary>
    [DatFileType(DatFileType.UiLayout)]
    public class LayoutDesc : FileType
    {
        public int DisplayWidth;
        public int DisplayHeight;

        // Enum name for the Keys can be looked up in EnumMapper file 0x2200001B
        public Dictionary<uint, ElementDesc> Elements = new Dictionary<uint, ElementDesc>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            DisplayWidth = reader.ReadInt32();
            DisplayHeight = reader.ReadInt32();

            reader.ReadByte();
            var totalObjects = reader.ReadByte();
            for (int i = 0; i < totalObjects; i++)
            {
                var key = reader.ReadUInt32();

                var item = new ElementDesc();
                item.Unpack(reader);
                Elements.Add(key, item);
            }
        }
    }
}
