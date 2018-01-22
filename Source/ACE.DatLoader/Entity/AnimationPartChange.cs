using System.IO;

namespace ACE.DatLoader.Entity
{
    // TODO: Refactor to merge with existing AnimationOverride object in ACE.
    public class AnimationPartChange : IUnpackable
    {
        public byte PartIndex { get; /*private */set; }
        public uint PartID { get; /*private */set; }

        public void Unpack(BinaryReader reader)
        {
            PartIndex   = reader.ReadByte();
            PartID      = reader.ReadAsDataIDOfKnownType(0x01000000);
        }
    }
}
