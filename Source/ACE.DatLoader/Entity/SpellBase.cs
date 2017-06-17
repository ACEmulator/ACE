using System.Collections.Generic;
using ACE.Entity.Enum;

namespace ACE.DatLoader.Entity
{
    public class SpellBase
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public MagicSchool School { get; set; }
        public uint Icon { get; set; }
        public uint Category { get; set; } // All related levels of the same spell. Same category spells will not stack. (Strength Self I & Strength Self II)
        public uint Bitfield { get; set; }
        public uint BaseMana { get; set; } // Mana Cost
        public float BaseRangeConstant { get; set; }
        public float BaseRangeMod { get; set; }
        public uint Power { get; set; } // Used to determine which spell in the catgory is the strongest.
        public float SpellEconomyMod { get; set; } // A legacy of a bygone era
        public uint FormulaVersion { get; set; }
        public uint ComponentLoss { get; set; } // Burn rate
        public SpellType MetaSpellType { get; set; }
        public uint MetaSpellId { get; set; } // Just the spell id again
        
        // Only on EnchantmentSpell/FellowshipEnchantmentSpells
        public double Duration { get; set; }
        public float DegradeModifier { get; set; } // Unknown what this does
        public float DegradeLimit { get; set; }  // Unknown what this does

        public double PortalLifetime { get; set; } // Only for PortalSummon_SpellType

        public List<uint> Formula { get; set; } // UInt Values correspond to the SpellComponentsTable
        public uint CasterEffect { get; set; }  // effect that playes on the caster of the casted spell (e.g. for buffs, protects, etc)
        public uint TargetEffect { get; set; } // effect that playes on the target of the casted spell (e.g. for debuffs, vulns, etc)
        public uint FizzleEffect { get; set; } // is always zero. All spells have the same fizzle effect.
        public double RecoveryInterval { get; set; } // is always zero
        public float RecoveryAmount { get; set; } // is always zero
        public uint DisplayOrder { get; set; } // for soring in the spell list in the client UI
        public uint NonComponentTargetType { get; set; } // Unknown what this does
        public uint ManaMod { get; set; } // Additional mana cost per target (e.g. "Incantation of Acid Bane" Mana Cost = 80 + 14 per target)
    }
}
