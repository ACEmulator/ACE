using System.IO;

namespace ACE.DatLoader.Entity
{
    public class AnimationPartChange : IUnpackable
    {
        public byte PartIndex { get; private set; }
        public uint PartID { get; private set; }

        // ReplaceObjectHook reads the PartIndex in as two bytes for some reason, hence this somewhat hacky flag.
        public bool PartIsOneByte = true;

        public void Unpack(BinaryReader reader)
        {
            if(PartIsOneByte)
                PartIndex   = reader.ReadByte();
            else
            {
                ushort twoBytePart = reader.ReadUInt16();
                PartIndex = (byte)(twoBytePart & 255);
            }
            PartID      = reader.ReadAsDataIDOfKnownType(0x01000000);
        }
    }
}
