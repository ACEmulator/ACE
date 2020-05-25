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

        public static readonly List<MotionCommand> Invalid = new List<MotionCommand>() { MotionCommand.Invalid };

        public List<MotionCommand> GetMotion(MotionStance stance, AttackHeight attackHeight, AttackType attackType, MotionCommand prevMotion)
        {
            if (!Stances.TryGetValue(stance, out var attackHeights))
                return Invalid;

            if (!attackHeights.Table.TryGetValue(attackHeight, out var attackTypes))
                return Invalid;

            if (!attackTypes.Table.TryGetValue(attackType, out var maneuvers))
                return Invalid;

            //if (maneuvers.Count == 1)
                //return maneuvers[0];

            /*Console.WriteLine($"CombatManeuverTable({Id:X8}).GetMotion({stance}, {attackHeight}, {attackType}) - found {maneuvers.Count} maneuvers");
            foreach (var maneuver in maneuvers)
                Console.WriteLine(maneuver);*/

            // CombatManeuverTable(30000000).GetMotion(SwordCombat, Medium, Slash) - found 2 maneuvers
            // SlashMed
            // BackhandMed

            // rng, or alternate?
            /*for (var i = 0; i < maneuvers.Count; i++)
            {
                var maneuver = maneuvers[i];

                if (maneuver == prevMotion)
                {
                    if (i < maneuvers.Count - 1)
                        return maneuvers[i + 1];
                    else
                        return maneuvers[0];
                }
            }
            return maneuvers[0];*/

            // if the CMT contains > 1 entries for this lookup, return both
            // the code determines which motion to use based on the power bar
            return maneuvers;
        }

        public void ShowCombatTable()
        {
            foreach (var stance in Stances)
            {
                Console.WriteLine($"- {stance.Key}");

                foreach (var attackHeight in stance.Value.Table)
                {
                    Console.WriteLine($"  - {attackHeight.Key}");

                    foreach (var attackType in attackHeight.Value.Table)
                    {
                        Console.WriteLine($"    - {attackType.Key}");

                        foreach (var motion in attackType.Value)
                            Console.WriteLine($"      - {motion}");
                    }
                }
            }
        }
    }
}
