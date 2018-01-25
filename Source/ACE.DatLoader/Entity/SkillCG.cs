using System.IO;

namespace ACE.DatLoader.Entity
{
    public class SkillCG : IUnpackable
    {
        public uint SkillNum { get; private set; }
        public uint NormalCost { get; private set; }
        public uint PrimaryCost { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            SkillNum    = reader.ReadUInt32();
            NormalCost  = reader.ReadUInt32();
            PrimaryCost = reader.ReadUInt32();
        }
    }
}
