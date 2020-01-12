using System.IO;
using ACE.Entity.Enum;

namespace ACE.DatLoader.Entity
{
    public class CombatManeuver : IUnpackable
    {
        public MotionStance Style { get; private set; }
        public AttackHeight AttackHeight { get; private set; }
        public AttackType AttackType { get; private set; }
        public uint MinSkillLevel { get; private set; }
        public MotionCommand Motion { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Style           = (MotionStance)reader.ReadUInt32();
            AttackHeight    = (AttackHeight)reader.ReadUInt32();
            AttackType      = (AttackType)reader.ReadUInt32();
            MinSkillLevel   = reader.ReadUInt32();
            Motion          = (MotionCommand)reader.ReadUInt32();
        }
    }
}
