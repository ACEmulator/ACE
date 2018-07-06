using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.SecondaryAttributeTable)]
    public class SecondaryAttributeTable : FileType
    {
        internal const uint FILE_ID = 0x0E000003;

        public Attribute2ndBase MaxHealth { get; private set; } = new Attribute2ndBase();
        public Attribute2ndBase MaxStamina { get; private set; } = new Attribute2ndBase();
        public Attribute2ndBase MaxMana { get; private set; } = new Attribute2ndBase();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            MaxHealth.Unpack(reader);
            MaxStamina.Unpack(reader);
            MaxMana.Unpack(reader);
        }
    }
}
