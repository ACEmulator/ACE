using System.IO;

namespace ACE.DatLoader.Entity
{
    public class AnimationPartChange : IUnpackable
    {
        public ushort PartIndex { get; private set; }
        public uint PartID { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            PartIndex   = reader.ReadUInt16();
            PartID      = reader.ReadAsDataIDOfKnownType(0x01000000);
        }
    }
}
