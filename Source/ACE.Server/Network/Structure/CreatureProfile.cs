using System;
using System.IO;
using ACE.Server.Network.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    [Flags]
    public enum CreatureProfileFlags
    {
        HasBuffsDebuffs = 0x1,
        Unknown1        = 0x2,      // TODO: decode flags
        Unknown2        = 0x4,
        ShowAttributes  = 0x8
    };

    /// <summary>
    /// Handles the assessment of creatures
    /// (monsters / players)
    /// </summary>
    public class CreatureProfile
    {
        // These flags indicate which members will be available for assessment
        public CreatureProfileFlags Flags;

        public uint Health;         // Current health
        public uint HealthMax;      // Maximum health

        // Choose valid sections by masking against Flags
        // 0x8:
        public uint Strength;       // Current Strength
        public uint Endurance;      // Current Endurance
        public uint Quickness;      // Current Quickness
        public uint Coordination;   // Current Coordination
        public uint Focus;          // Current Focus
        public uint Self;           // Current Self

        public uint Stamina;        // Current Stamina
        public uint Mana;           // Current Mana
        public uint StaminaMax;     // Maximum Stamina
        public uint ManaMax;        // Maximum Mana

        // 0x1:
        public AttributeMask AttributeHighlights;      // Highlight enable bitmask: 0=no, 1=yes
        public AttributeMask AttributeColors;          // Highlight color bitmask: 0=red, 1=green

        public CreatureProfile(Creature creature, bool success = true)
        {
            if (success)
                Flags |= CreatureProfileFlags.ShowAttributes;

            Health = creature.Health.Current;
            HealthMax = creature.Health.MaxValue;

            if (!success) return;

            Strength = creature.Strength.Current;
            Endurance = creature.Endurance.Current;
            Quickness = creature.Quickness.Current;
            Coordination = creature.Coordination.Current;
            Focus = creature.Focus.Current;
            Self = creature.Self.Current;

            Stamina = creature.Stamina.Current;
            Mana = creature.Mana.Current;
            StaminaMax = creature.Stamina.MaxValue;
            ManaMax = creature.Mana.MaxValue;

            AttributeHighlights = AttributeMaskHelper.GetAttributeHighlights(creature);
            AttributeColors = AttributeMaskHelper.GetAttributeColors(creature);

            if (AttributeHighlights != 0)
                Flags |= CreatureProfileFlags.HasBuffsDebuffs;
        }
    }

    public static class CreatureProfileExtensions
    {
        public static void Write(this BinaryWriter writer, CreatureProfile profile)
        {
            writer.Write((uint)profile.Flags);
            writer.Write(profile.Health);
            writer.Write(profile.HealthMax);

            // has flags & 0x8?
            if (profile.Flags.HasFlag(CreatureProfileFlags.ShowAttributes))
            {
                writer.Write(profile.Strength);
                writer.Write(profile.Endurance);
                writer.Write(profile.Quickness);
                writer.Write(profile.Coordination);
                writer.Write(profile.Focus);
                writer.Write(profile.Self);

                writer.Write(profile.Stamina);
                writer.Write(profile.Mana);
                writer.Write(profile.StaminaMax);
                writer.Write(profile.ManaMax);
            }

            // has flags & 0x1?
            if (profile.Flags.HasFlag(CreatureProfileFlags.HasBuffsDebuffs))
            {
                writer.Write((ushort)profile.AttributeHighlights);
                writer.Write((ushort)profile.AttributeColors);
            }
        }
    }
}
