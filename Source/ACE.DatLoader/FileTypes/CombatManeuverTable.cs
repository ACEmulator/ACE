using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;
using ACE.Entity.Enum;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x30. 
    /// </summary>
    [DatFileType(DatFileType.CombatTable)]
    public class CombatManeuverTable : FileType
    {
        public List<CombatManeuver> CMT { get; } = new List<CombatManeuver>();

        public Dictionary<MotionStance, AttackHeights> Stances;

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32(); // This should always equal the fileId

            CMT.Unpack(reader);

            Stances = new Dictionary<MotionStance, AttackHeights>();

            foreach (var maneuver in CMT)
            {
                if (!Stances.TryGetValue(maneuver.Style, out var attackHeights))
                {
                    attackHeights = new AttackHeights();
                    Stances.Add(maneuver.Style, attackHeights);
                }

                if (!attackHeights.Table.TryGetValue(maneuver.AttackHeight, out var attackTypes))
                {
                    attackTypes = new AttackTypes();
                    attackHeights.Table.Add(maneuver.AttackHeight, attackTypes);
                }

                if (!attackTypes.Table.TryGetValue(maneuver.AttackType, out var minSkillLevels))
                {
                    minSkillLevels = new MinSkillLevels();
                    attackTypes.Table.Add(maneuver.AttackType, minSkillLevels);
                }

                minSkillLevels.Table[maneuver.MinSkillLevel] = maneuver.Motion;
            }
        }

        public class AttackHeights
        {
            public Dictionary<AttackHeight, AttackTypes> Table = new Dictionary<AttackHeight, AttackTypes>();
        }

        public class AttackTypes
        {
            public Dictionary<AttackType, MinSkillLevels> Table = new Dictionary<AttackType, MinSkillLevels>();
        }

        public class MinSkillLevels
        {
            public SortedDictionary<uint, MotionCommand> Table = new SortedDictionary<uint, MotionCommand>(ReverseComparer);
        }

        public static ReverseComparer ReverseComparer = new ReverseComparer();

        public MotionCommand GetMotion(MotionStance stance, AttackHeight attackHeight, AttackType attackType, uint minSkillLevel = 0)
        {
            if (!Stances.TryGetValue(stance, out var attackHeights))
                return MotionCommand.Invalid;

            if (!attackHeights.Table.TryGetValue(attackHeight, out var attackTypes))
                return MotionCommand.Invalid;

            if (!attackTypes.Table.TryGetValue(attackType, out var minSkillLevels))
                return MotionCommand.Invalid;

            foreach (var kvp in minSkillLevels.Table)
            {
                if (kvp.Key > minSkillLevel)
                    continue;

                return kvp.Value;
            }

            return MotionCommand.Invalid;
        }
    }

    public class ReverseComparer : IComparer<uint>
    {
        public int Compare(uint a, uint b)
        {
            return b.CompareTo(a);
        }
    }
}
