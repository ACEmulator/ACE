using System.IO;

namespace ACE.DatLoader.Entity
{
    public class CombatManeuver : IUnpackable
    {
        public uint Style { get; private set; }
        public uint AttackHeight { get; private set; }
        public uint AttackType { get; private set; }
        public uint MinSkillLevel { get; private set; }
        public uint Motion { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Style           = reader.ReadUInt32();
            AttackHeight    = reader.ReadUInt32();
            AttackType      = reader.ReadUInt32();
            MinSkillLevel   = reader.ReadUInt32();
            Motion          = reader.ReadUInt32();
        }
    }
}
