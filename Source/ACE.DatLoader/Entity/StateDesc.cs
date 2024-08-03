using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ACE.DatLoader.Entity
{
    public class StateDesc : IUnpackable
    {

        // Enum name for the StateId can be looked up in EnumMapper file 0x2200001C
        public uint StateId;
        public bool PassToChildren;
        public IncorporationFlags UiIncorporationFlags;
        public Dictionary<uint, BaseProperty> Properties = new Dictionary<uint, BaseProperty>();
        public List<MediaDesc> Media = new List<MediaDesc>();

        public virtual void Unpack(BinaryReader reader)
        {
            StateId = reader.ReadUInt32();
            PassToChildren = reader.ReadBoolean();
            UiIncorporationFlags = (IncorporationFlags)reader.ReadUInt32();

            reader.ReadByte();
            var totalObjects = reader.ReadByte();
            for (int i = 0; i < totalObjects; i++)
            {
                var key = reader.ReadUInt32();

                var item = new BaseProperty();
                item.Unpack(reader);
                Properties.Add(key, item);
            }

            var totalMedia = reader.ReadByte();
            for(var i  = 0; i < totalMedia; i++)
            {
                var item = new MediaDesc();
                item.Unpack(reader);
                Media.Add(item);
            }
        }

    }
}
