using System;
using System.Collections.Generic;
using System.IO;

using ACE.Common;
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

                if (!attackTypes.Table.TryGetValue(maneuver.AttackType, out var motionCommands))
                {
                    motionCommands = new List<MotionCommand>();
                    attackTypes.Table.Add(maneuver.AttackType, motionCommands);
                }

                motionCommands.Add(maneuver.Motion);
            }
        }

        public class AttackHeights
        {
            public Dictionary<AttackHeight, AttackTypes> Table = new Dictionary<AttackHeight, AttackTypes>();
        }

        public class AttackTypes
        {
            // technically there is another MinSkillLevels here in the data,
            // but every MinSkillLevel in the client dats are always 0
            public Dictionary<AttackType, List<MotionCommand>> Table = new Dictionary<AttackType, List<MotionCommand>>();
        }

        public MotionCommand GetMotion(MotionStance stance, AttackHeight attackHeight, AttackType attackType)
        {
            if (!Stances.TryGetValue(stance, out var attackHeights))
                return MotionCommand.Invalid;

            if (!attackHeights.Table.TryGetValue(attackHeight, out var attackTypes))
                return MotionCommand.Invalid;

            if (!attackTypes.Table.TryGetValue(attackType, out var maneuvers))
                return MotionCommand.Invalid;

            if (maneuvers.Count == 1)
                return maneuvers[0];

            // this should always be 1 for player table
            // if not, return random maneuver?
            Console.WriteLine($"CombatManeuverTable({Id:X8}).GetMotion({stance}, {attackHeight}, {attackType}) - found {maneuvers.Count} maneuvers");

            var rng = ThreadSafeRandom.Next(0, maneuvers.Count - 1);
            return maneuvers[rng];
        }
    }
}
