using System;
using System.Collections.Generic;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using System.Numerics;

namespace ACE.Server.Entity
{
    partial class Spell
    {
        // ======================================
        // SpellBase fields - from the client DAT
        // ======================================

        /// <summary>
        /// The spell ID
        /// </summary>
        public uint Id { get => _spellBase.MetaSpellId; }

        /// <summary>
        /// The spell name
        /// </summary>
        public string Name { get => _spellBase.Name; }

        /// <summary>
        /// The spell description that appears in client
        /// </summary>
        public string Description { get => _spellBase.Desc; }

        /// <summary>
        /// The magic school this spell belongs to
        /// </summary>
        public MagicSchool School { get => _spellBase.School; }

        /// <summary>
        /// The spell icon ID for display in client
        /// </summary>
        public uint IconID { get => _spellBase.Icon; }

        /// <summary>
        /// Used for spell stacking, ie. Strength Self I and Strength Self VI will be the same category
        /// </summary>
        public SpellCategory Category { get => _spellBase.Category; }

        /// <summary>
        /// bit flags for the spell
        /// </summary>
        public SpellFlags Flags { get => (SpellFlags)_spellBase.Bitfield; }

        /// <summary>
        /// The base mana cost required for casting the spell
        /// </summary>
        public uint BaseMana { get => _spellBase.BaseMana; }

        /// <summary>
        /// The base maximum distance for casting the spell
        /// </summary>
        public float BaseRangeConstant { get => _spellBase.BaseRangeConstant; }

        /// <summary>
        /// An additive multiplier to BaseRangeConstant
        /// based on caster's skill level
        /// </summary>
        public float BaseRangeMod { get => _spellBase.BaseRangeMod; }

        /// <summary>
        /// The difficulty of casting the spell
        /// </summary>
        public uint Power { get => _spellBase.Power; }

        /// <summary>
        /// Returns the minimum spell power required for some calculations,
        /// such as mana conversion and proficiency points
        /// </summary>
        public uint PowerMod { get => Math.Max(Power, 25); }

        /// <summary>
        /// The modifier for the original spell economy
        /// A legacy of a bygone era
        /// </summary>
        public float SpellEconomyMod { get => _spellBase.SpellEconomyMod; }

        /// <summary>
        /// The version # of the spell formula
        /// </summary>
        public uint FormulaVersion { get => _spellBase.FormulaVersion; }

        /// <summary>
        /// The burn rate for casting the spell
        /// </summary>
        public float ComponentLoss { get => _spellBase.ComponentLoss; }

        /// <summary>
        /// A subtype for the spell
        /// </summary>
        public SpellType MetaSpellType { get => _spellBase.MetaSpellType; }

        /// <summary>
        /// The amount of time the spell lasts
        /// usually for EnchantmentSpell / FellowshipEnchantmentSpells
        /// </summary>
        public double Duration { get => _spell != null && _spell.DotDuration.HasValue ? _spell.DotDuration.Value : _spellBase.Duration; }

        /// <summary>
        /// Unknown what this does?
        /// client visual FX possibly?
        /// </summary>
        public float DegradeModifier { get => _spellBase.DegradeModifier; }

        /// <summary>
        /// Unknown what this does?
        /// client visual FX possibly?
        /// </summary>
        public float DegradeLimit { get => _spellBase.DegradeLimit; }

        /// <summary>
        /// The duration for PortalSummon_SpellType
        /// </summary>
        public double PortalLifetime { get => _spellBase.PortalLifetime; }

        /// <summary>
        /// uint values correspond to the SpellComponentsTable
        /// </summary>
        private List<uint> _formula { get => _spellBase.Formula; }

        /// <summary>
        /// Effect that plays on the caster for this spell (ie. for buffs, protects, etc.)
        /// </summary>
        public PlayScript CasterEffect { get => (PlayScript)_spellBase.CasterEffect; }

        /// <summary>
        /// Effect that plays on the target for this spell (ie. for debuffs, vulns, etc.)
        /// </summary>
        public PlayScript TargetEffect { get => (PlayScript)_spellBase.TargetEffect; }

        /// <summary>
        /// is always zero - all spells have the same fizzle effect
        /// </summary>
        public PlayScript FizzleEffect { get => (PlayScript)_spellBase.FizzleEffect; }

        /// <summary>
        /// is always zero
        /// </summary>
        public double RecoveryInterval { get => _spellBase.RecoveryInterval; }

        /// <summary>
        /// is always zero
        /// </summary>
        public float RecoveryAmount { get => _spellBase.RecoveryAmount; }

        /// <summary>
        /// For sorting in the spell list in the client UI
        /// </summary>
        public uint DisplayOrder { get => _spellBase.DisplayOrder; }

        /// <summary>
        /// Unknown what this does?
        /// </summary>
        public uint NonComponentTargetType { get => _spellBase.NonComponentTargetType; }

        /// <summary>
        /// Additional mana cost per target (e.g. "Incantation of Acid Bane" Mana Cost = 80 + 14 per target)
        /// </summary>
        public uint ManaMod { get => _spellBase.ManaMod; }

        //==================================
        // Spell fields - from the server DB
        //==================================

        /// <summary>
        /// The stat modifier type, usually EnchantmentTypeFlags
        /// </summary>
        public EnchantmentTypeFlags StatModType { get => (EnchantmentTypeFlags)(_spell.StatModType ?? 0); }

        /// <summary>
        /// The stat modifier key, used for lookup in the enchantment registry
        /// </summary>
        public uint StatModKey { get => _spell.StatModKey ?? 0; }

        /// <summary>
        /// The amount to modify a stat
        /// </summary>
        public float StatModVal { get => _spell.StatModVal ?? 0.0f; }

        /// <summary>
        /// The damage type for this spell
        /// </summary>
        public DamageType DamageType { get => (DamageType)(_spell.EType ?? 0); }

        /// <summary>
        /// The base amount of damage for this spell
        /// </summary>
        public int BaseIntensity { get => _spell.BaseIntensity ?? 0; }

        public int MinDamage { get => BaseIntensity; }

        /// <summary>
        /// The maximum additional daamage for this spell
        /// </summary>
        public int Variance { get => _spell.Variance ?? 0; }

        public int MaxDamage { get => BaseIntensity + Variance; }

        /// <summary>
        /// The weenie class ID associated for this spell, ie. the projectile weenie class id
        /// </summary>
        public uint WeenieClassId { get => _spell.Wcid ?? 0; }

        public uint Wcid { get => _spell.Wcid ?? 0; }

        /// <summary>
        /// The total # of projectiles launched for this spell
        /// </summary>
        public int NumProjectiles { get => _spell.NumProjectiles ?? 0; }

        /// <summary>
        /// The maximum # of additional projectiles possibly launched
        /// </summary>
        public int NumProjectilesVariance { get => _spell.NumProjectilesVariance ?? 0; }

        /// <summary>
        /// The total angle for multi-projectile spells,
        /// ie. 90 degrees for 3-5 projectiles, or 360 degrees for ring spells
        /// </summary>
        public float SpreadAngle { get => _spell.SpreadAngle ?? 0; }

        /// <summary>
        /// The vertical angle to launch this spell
        /// </summary>
        public float VerticalAngle { get => _spell.VerticalAngle ?? 0; }

        /// <summary>
        /// The default angle to launch this spell
        /// (relative to player, or global?)
        /// </summary>
        public float DefaultLaunchAngle { get => _spell.DefaultLaunchAngle ?? 0; }

        /// <summary>
        /// The offset to apply to the spawn position
        /// </summary>
        public Vector3 CreateOffsetOrigin { get => new Vector3(_spell.CreateOffsetOriginX ?? 0.0f, _spell.CreateOffsetOriginY ?? 0.0f, _spell.CreateOffsetOriginZ ?? 0.0f); }

        /// <summary>
        /// The minimum amount of padding to ensure for the spell to spawn
        /// </summary>
        public Vector3 PaddingOrigin { get => new Vector3(_spell.PaddingOriginX ?? 0.0f, _spell.PaddingOriginY ?? 0.0f, _spell.PaddingOriginZ ?? 0.0f); }

        /// <summary>
        /// The dimensions of the origin, used for Volley spells?
        /// </summary>
        public Vector3 DimsOrigin { get => new Vector3(_spell.DimsOriginX ?? 0.0f, _spell.DimsOriginY ?? 0.0f, _spell.DimsOriginZ ?? 0.0f); }

        /// <summary>
        /// The maximum variation for spawn position
        /// </summary>
        public Vector3 PeturbationOrigin { get => new Vector3(_spell.PeturbationOriginX ?? 0.0f, _spell.PeturbationOriginY ?? 0.0f, _spell.PeturbationOriginZ ?? 0.0f); }

        /// <summary>
        /// The imbued effect for this spell
        /// </summary>
        public uint ImbuedEffect { get => _spell.ImbuedEffect ?? 0; }

        /// <summary>
        /// The creature class for a slayer spell
        /// </summary>
        public CreatureType SlayerCreatureType { get => (CreatureType)(_spell.SlayerCreatureType ?? 0); }

        /// <summary>
        /// The amount of additional damage for a slayer spell
        /// </summary>
        public float SlayerDamageBonus { get => _spell.SlayerDamageBonus ?? 0; }    // 0 = additive, or 1 = multiplier?

        /// <summary>
        /// The critical chance frequency for this spell
        /// </summary>
        public double CritFrequency { get => _spell.CritFreq ?? 0.0; }  // default: 0, 1, or 0.03?

        /// <summary>
        /// The critical damage multiplier for this spell
        /// </summary>
        public double CritMultiplier { get => _spell.CritMultiplier ?? 1.0; }    // verify default multiplier?

        /// <summary>
        /// If TRUE, ignores magic resistance
        /// </summary>
        public bool IgnoreMagicResist { get => Convert.ToBoolean(_spell.IgnoreMagicResist ?? 0); }  // verify bool?

        /// <summary>
        /// The elemental damage multiplier for this spell
        /// </summary>
        public double ElementalModifier { get => _spell.ElementalModifier ?? 1.0; } // verify default multiplier?

        /// <summary>
        /// The amount of source vital to drain for a life spell
        /// </summary>
        public float DrainPercentage { get => _spell.DrainPercentage ?? 0.0f; }

        /// <summary>
        /// The percentage of DrainPercentage to damage a target for life projectiles
        /// </summary>
        public float DamageRatio { get => _spell.DamageRatio ?? 1.0f; }

        /// <summary>
        /// DamageType used by LifeMagic spells that specifies Health, Mana, or Stamina for the Boost type spells
        /// </summary>
        public DamageType VitalDamageType { get => (DamageType)(_spell.DamageType ?? 0); }

        /// <summary>
        /// The minimum amount of vital boost from a life spell
        /// </summary>
        public int Boost { get => _spell.Boost ?? 0; }

        /// <summary>
        /// Boost + BoostVariance = the maximum amount of vital boost from a life spell
        /// </summary>
        public int BoostVariance { get => _spell.Boost ?? 0; }

        public int MaxBoost { get => Boost + BoostVariance; }

        /// <summary>
        /// The source vital for a life spell
        /// </summary>
        public PropertyAttribute2nd Source { get => (PropertyAttribute2nd)(_spell.Source ?? 0); }

        /// <summary>
        /// The destination vital for a life spell
        /// </summary>
        public PropertyAttribute2nd Destination { get => (PropertyAttribute2nd)(_spell.Destination ?? 0); }

        /// <summary>
        /// The propotion of source vital to transfer to destination vital
        /// </summary>
        public float Proportion { get => _spell.Proportion ?? 1.0f; }

        /// <summary>
        /// The percent of source vital loss for a life magic transfer spell
        /// </summary>
        public float LossPercent { get => _spell.LossPercent ?? 0.0f; }

        /// <summary>
        /// A static amount of source vital loss for a life magic transfer spell?
        /// Unused / unknown?
        /// </summary>
        public int SourceLoss { get => _spell.SourceLoss ?? 0; }

        /// <summary>
        /// The maximum amount of vital transferred by a life magic spell
        /// </summary>
        public int TransferCap { get => _spell.TransferCap ?? 0; }

        /// <summary>
        /// The maximum destination vital boost for a life magic transfer spell?
        /// Unused / unknown?
        /// </summary>
        public int MaxBoostAllowed { get => _spell.MaxBoostAllowed ?? 0; }

        /// <summary>
        /// Indicates the source and destination for life magic transfer spells
        /// </summary>
        public TransferFlags TransferFlags { get => (TransferFlags)(_spell.TransferBitfield ?? 0); }

        /// <summary>
        /// Unknown index?
        /// </summary>
        public int Index { get => _spell.Index ?? 0; }

        /// <summary>
        /// Unknown link field?
        /// </summary>
        public int Link { get => _spell.Link ?? 0; }

        /// <summary>
        /// A destination location for a spell
        /// </summary>
        public Position Position { get => new Position(_spell.PositionObjCellId ?? 0, new Vector3(_spell.PositionOriginX ?? 0.0f, _spell.PositionOriginY ?? 0.0f, _spell.PositionOriginZ ?? 0.0f), new Quaternion(_spell.PositionAnglesX ?? 0.0f, _spell.PositionAnglesY ?? 0.0f, _spell.PositionAnglesZ ?? 0.0f, _spell.PositionAnglesW ?? 0.0f)); }

        /// <summary>
        /// The minimum spell power to dispel (unused?)
        /// </summary>
        public int MinPower { get => _spell.MinPower ?? 0;  }

        /// <summary>
        /// The maximum spell power to dispel
        /// </summary>
        public int MaxPower { get => _spell.MaxPower ?? 0; }

        /// <summary>
        /// Possible RNG for spell power to dispel (unused?)
        /// </summary>
        public float PowerVariance { get => _spell.PowerVariance ?? 0.0f; }

        /// <summary>
        /// The magic school to dispel, or undefined for all schools
        /// </summary>
        public MagicSchool DispelSchool { get => (MagicSchool)(_spell.DispelSchool ?? 0); }

        /// <summary>
        /// The type of spells to dispel
        /// 0 = all spells
        /// 1 = positive
        /// 2 = negative
        /// </summary>
        public DispelType Align { get => (DispelType)(_spell.Align ?? 0); }

        /// <summary>
        /// The maximum # of spells to dispel
        /// </summary>
        public int Number { get =>_spell.Number ?? 0; }

        /// <summary>
        /// Number * NumberVariance = the minimum # of spells to dispel
        /// </summary>
        public float NumberVariance { get => _spell.NumberVariance ?? 0; }
    }
}
