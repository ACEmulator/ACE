using System;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Enum
{
    [Flags]
    public enum AttributeMask
    {
        Strength     = 0x1,
        Endurance    = 0x2,
        Quickness    = 0x4,
        Coordination = 0x8,
        Focus        = 0x10,
        Self         = 0x20,
        Health       = 0x40,
        Stamina      = 0x80,
        Mana         = 0x100,
    };

    public static class AttributeMaskHelper
    {
        public static AttributeMask GetAttributeHighlights(Creature creature)
        {
            AttributeMask highlightMask = 0;

            if (creature.Strength.ModifierType != ModifierType.None)
                highlightMask |= AttributeMask.Strength;
            if (creature.Endurance.ModifierType != ModifierType.None)
                highlightMask |= AttributeMask.Endurance;
            if (creature.Quickness.ModifierType != ModifierType.None)
                highlightMask |= AttributeMask.Quickness;
            if (creature.Coordination.ModifierType != ModifierType.None)
                highlightMask |= AttributeMask.Coordination;
            if (creature.Focus.ModifierType != ModifierType.None)
                highlightMask |= AttributeMask.Focus;
            if (creature.Self.ModifierType != ModifierType.None)
                highlightMask |= AttributeMask.Self;
            if (creature.Health.ModifierType != ModifierType.None)
                highlightMask |= AttributeMask.Health;
            if (creature.Stamina.ModifierType != ModifierType.None)
                highlightMask |= AttributeMask.Stamina;
            if (creature.Mana.ModifierType != ModifierType.None)
                highlightMask |= AttributeMask.Mana;

            return highlightMask;
        }

        public static AttributeMask GetAttributeColors(Creature creature)
        {
            AttributeMask colorMask = 0;

            // defaults to debuffed - highlight masked above
            if (creature.Strength.ModifierType == ModifierType.Buffed)
                colorMask |= AttributeMask.Strength;
            if (creature.Endurance.ModifierType == ModifierType.Buffed)
                colorMask |= AttributeMask.Endurance;
            if (creature.Quickness.ModifierType == ModifierType.Buffed)
                colorMask |= AttributeMask.Quickness;
            if (creature.Coordination.ModifierType == ModifierType.Buffed)
                colorMask |= AttributeMask.Coordination;
            if (creature.Focus.ModifierType == ModifierType.Buffed)
                colorMask |= AttributeMask.Focus;
            if (creature.Self.ModifierType == ModifierType.Buffed)
                colorMask |= AttributeMask.Self;
            if (creature.Health.ModifierType == ModifierType.Buffed)
                colorMask |= AttributeMask.Health;
            if (creature.Stamina.ModifierType == ModifierType.Buffed)
                colorMask |= AttributeMask.Stamina;
            if (creature.Mana.ModifierType == ModifierType.Buffed)
                colorMask |= AttributeMask.Mana;

            return colorMask;
        }
    }
}
