using System;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public AttackType MAttackType
        {
            get => (AttackType)(GetProperty(PropertyInt.AttackType) ?? 0);
            set { if (value == AttackType.Undef) RemoveProperty(PropertyInt.AttackType); else SetProperty(PropertyInt.AttackType, (int)value); }
        }

        /// <summary>
        /// Returns TRUE if this weapon cleaves
        /// </summary>
        public bool IsCleaving { get => GetProperty(PropertyInt.Cleaving) != null;  }

        /// <summary>
        /// Returns the number of cleave targets for this weapon
        /// If cleaving weapon, this is PropertyInt.Cleaving - 1
        /// </summary>
        public int CleaveTargets
        {
            get
            {
                if (!IsCleaving)
                    return 0;

                return GetProperty(PropertyInt.Cleaving).Value - 1;
            }
        }

        const float defaultPhysicalCritFrequency = 0.10f;
        const float defaultMagicCritFrequency = 0.02f;
        const float defaultCritMultiplier = 1.0f;
        const float defaultBonusModifier = 1.0f;
        const uint defaultSpeed = 40;   // TODO: find default speed

        /// <summary>
        /// Returns the primary weapon equipped by a player
        /// (melee, missile, or wand)
        /// </summary>
        private static WorldObject GetWeapon(Player wielder)
        {
            if (wielder == null)
                return null;

            WorldObject weapon = wielder.GetEquippedWeapon();

            if (weapon == null)
                weapon = wielder.GetEquippedWand();

            return weapon;
        }

        /// <summary>
        /// Returns the weapon speed, with enchantments factored in
        /// </summary>
        public static uint GetWeaponSpeed(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            var baseSpeed = weapon != null ? weapon.GetProperty(PropertyInt.WeaponTime) ?? (int)defaultSpeed : (int)defaultSpeed;

            var speedMod = weapon != null ? weapon.EnchantmentManager.GetWeaponSpeedMod() : 0;
            var auraSpeedMod = wielder != null ? wielder.EnchantmentManager.GetWeaponSpeedMod() : 0;

            return (uint)Math.Max(0, baseSpeed + speedMod + auraSpeedMod);
        }

        /// <summary>
        /// Returns the Mana Conversion skill modifier for the current weapon
        /// </summary>
        public static float GetWeaponManaConversionModifier(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultBonusModifier;

            return defaultBonusModifier + (float)(weapon.GetProperty(PropertyFloat.ManaConversionMod) ?? 0.0f) * wielder.EnchantmentManager.GetManaConvMod();
        }

        /// <summary>
        /// Returns the Melee Defense skill modifier for the current weapon
        /// </summary>
        public static float GetWeaponMeleeDefenseModifier(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultBonusModifier;

            if (wielder.CombatMode != CombatMode.NonCombat)
            {
                var defenseMod = (float)(weapon.GetProperty(PropertyFloat.WeaponDefense) + weapon.EnchantmentManager.GetDefenseMod() ?? defaultBonusModifier);

                if (weapon.IsEnchantable)
                    defenseMod += wielder.EnchantmentManager.GetDefenseMod();

                return defenseMod;
            }

            return defaultBonusModifier;
        }

        /// <summary>
        /// Returns the attack skill modifier for the current weapon
        /// </summary>
        public static float GetWeaponOffenseModifier(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultBonusModifier;

            if (wielder.CombatMode != CombatMode.NonCombat)
            {
                var offenseMod = (float)(weapon.GetProperty(PropertyFloat.WeaponOffense) + weapon.EnchantmentManager.GetAttackMod() ?? defaultBonusModifier);

                if (weapon.IsEnchantable)
                    offenseMod += wielder.EnchantmentManager.GetAttackMod();

                return offenseMod;
            }

            return defaultBonusModifier;
        }

        /// <summary>
        /// Returns the critical chance modifier for the current weapon
        /// </summary>
        public static float GetWeaponCritChanceModifier(Creature wielder, CreatureSkill skill, Creature target)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultPhysicalCritFrequency;

            var critRateMod = (float)(weapon.GetProperty(PropertyFloat.CriticalFrequency) ?? defaultPhysicalCritFrequency);

            // multipliers before additives?
            var chanceRatingMod = Creature.GetPositiveRatingMod(wielder.CritRating ?? 0);
            critRateMod *= chanceRatingMod;

            // TODO: handle AlwaysCritical upstream
            if (weapon.HasImbuedEffect(ImbuedEffectType.CriticalStrike))
            {
                var criticalStrikeMod = GetCriticalStrikeMod(skill);
                critRateMod += criticalStrikeMod;
            }

            if (wielder is Player player && player.AugmentationCriticalExpertise > 0)
                critRateMod += player.AugmentationCriticalExpertise * 0.01f;

            // mitigation
            var chanceResistRatingMod = Creature.GetNegativeRatingMod(target.CritResistRating ?? 0);
            critRateMod *= chanceResistRatingMod;

            // 50% cap here, or only in criticalStrikeMod?
            critRateMod = Math.Min(critRateMod, 0.5f);

            return critRateMod;
        }

        public ImbuedEffectType GetImbuedEffects()
        {
            return (ImbuedEffectType)(
                (GetProperty(PropertyInt.ImbuedEffect) ?? 0) |
                (GetProperty(PropertyInt.ImbuedEffect2) ?? 0) |
                (GetProperty(PropertyInt.ImbuedEffect3) ?? 0) |
                (GetProperty(PropertyInt.ImbuedEffect4) ?? 0) |
                (GetProperty(PropertyInt.ImbuedEffect5) ?? 0) );
        }

        public bool HasImbuedEffect(ImbuedEffectType type)
        {
            return (GetImbuedEffects() & type) != 0;
        }

        /// <summary>
        /// Returns the magic critical chance modifier for the current weapon
        /// </summary>
        public static float GetWeaponMagicCritFrequencyModifier(Creature wielder, CreatureSkill skill, Creature target)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultMagicCritFrequency;

            var critRateMod = (float)(weapon.GetProperty(PropertyFloat.CriticalFrequency) ?? defaultMagicCritFrequency);

            // multipliers before additives?
            var chanceRatingMod = Creature.GetPositiveRatingMod(wielder.CritRating ?? 0);
            critRateMod *= chanceRatingMod;

            // TODO: handle AlwaysCritical upstream
            if (weapon.HasImbuedEffect(ImbuedEffectType.CriticalStrike) && skill != null)
            {
                var criticalStrikeMod = GetCriticalStrikeMod(skill);
                critRateMod += criticalStrikeMod;
            }

            if (wielder is Player player && player.AugmentationCriticalExpertise > 0)
                critRateMod += player.AugmentationCriticalExpertise * 0.01f;

            // mitigation
            var chanceResistRatingMod = Creature.GetNegativeRatingMod(target.CritResistRating ?? 0);
            critRateMod *= chanceResistRatingMod;

            // 50% cap here, or only in criticalStrikeMod?
            critRateMod = Math.Min(critRateMod, 0.5f);

            return critRateMod;
        }

        /// <summary>
        /// Returns the critical damage multiplier for the current weapon
        /// </summary>
        public static float GetWeaponCritDamageMod(Creature wielder, CreatureSkill skill, Creature target)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultCritMultiplier;

            var critDamageMod = (float)(weapon.GetProperty(PropertyFloat.CriticalMultiplier) ?? defaultCritMultiplier);

            // multipliers before additive?
            var critDamageRatingMod = Creature.GetPositiveRatingMod(wielder.CritDamageRating ?? 0);
            critDamageMod *= critDamageRatingMod;

            if (weapon.HasImbuedEffect(ImbuedEffectType.CripplingBlow))
            {
                var cripplingBlowMod = GetCripplingBlowMod(skill);
                critDamageMod += cripplingBlowMod;      // additive float?
            }

            if (wielder is Player player && player.AugmentationCriticalPower > 0)
                critDamageMod += player.AugmentationCriticalPower * 0.03f;

            // mitigation
            var critDamageResistRatingMod = Creature.GetNegativeRatingMod(target.CritDamageResistRating ?? 0);
            critDamageMod *= critDamageResistRatingMod;

            // caps at 6x upstream?
            critDamageMod = Math.Min(critDamageMod, 5.0f);

            return critDamageMod;
        }

        /// <summary>
        /// Returns the slayer 2x+ damage bonus for the current weapon
        /// against a particular creature type
        /// </summary>
        public static float GetWeaponCreatureSlayerModifier(Creature wielder, Creature target)
        {
            float modifier = defaultBonusModifier;

            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return modifier;

            if (weapon.GetProperty(PropertyInt.SlayerCreatureType) != null && target != null)
                if ((CreatureType)weapon.GetProperty(PropertyInt.SlayerCreatureType) == target.CreatureType)
                    modifier = (float)(weapon.GetProperty(PropertyFloat.SlayerDamageBonus) ?? modifier);
            return modifier;
        }

        /// <summary>
        /// Returns a multiplicative elemental damage bonus for the magic caster weapon type
        /// </summary>
        public static float GetCasterElementalDamageModifier(Creature wielder, Creature target, DamageType damageType)
        {
            // A multiplicative bonus, so the default is 1
            float elementalDmgBonusPvPReduction = 0.5f;
            float modifier = defaultBonusModifier;

            var wielderAsPlayer = wielder as Player;
            var targetAsPlayer = target as Player;

            WorldObject weapon = GetWeapon(wielderAsPlayer);

            if (weapon is Caster)
            {
                var elementalDamageModType = weapon.GetProperty(PropertyInt.DamageType) ?? (int)DamageType.Undef;
                if (elementalDamageModType != (int)DamageType.Undef && elementalDamageModType == (int)damageType)
                {
                    var casterElementalDmgMod = (float)(weapon.GetProperty(PropertyFloat.ElementalDamageMod) ?? modifier) + wielder.EnchantmentManager.GetElementalDamageMod();
                    if (casterElementalDmgMod > modifier)
                    {
                        modifier = casterElementalDmgMod;
                        if (wielderAsPlayer != null && targetAsPlayer != null)
                            modifier = 1.0f + (casterElementalDmgMod - 1.0f) * elementalDmgBonusPvPReduction;
                    }
                }
            }

            return modifier;
        }

        /// <summary>
        /// Returns an additive elemental damage bonus for the missile launcher weapon type
        /// </summary>
        public static int GetMissileElementalDamageModifier(Creature wielder, Creature target, DamageType damageType)
        {
            // An additive bonus, so the default is zero
            int modifier = 0;

            var wielderAsPlayer = wielder as Player;
            var targetAsPlayer = target as Player;

            WorldObject weapon = GetWeapon(wielderAsPlayer);

            if (weapon is MissileLauncher)
            {
                var elementalDamageModType = weapon.GetProperty(PropertyInt.DamageType) ?? (int)DamageType.Undef;
                if (elementalDamageModType != (int)DamageType.Undef && elementalDamageModType == (int)damageType)
                {
                    var launcherElementalDmgMod = weapon.GetProperty(PropertyInt.ElementalDamageBonus) ?? modifier;
                    modifier = launcherElementalDmgMod;
                }
            }

            return modifier;
        }

        /// <summary>
        /// Quest weapon fixed Resistance Cleaving equivalent to Level 5 Life Vulnerability spell
        /// </summary>
        public static float GetWeaponResistanceModifier(Creature wielder, CreatureSkill skill, DamageType damageType)
        {
            float resistMod = defaultBonusModifier;

            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultBonusModifier;

            var weaponResistanceModifierType = weapon.GetProperty(PropertyInt.ResistanceModifierType) ?? (int)DamageType.Undef;
            if (weaponResistanceModifierType != (int)DamageType.Undef)
                if (weaponResistanceModifierType == (int)damageType)
                    resistMod = (float)(weapon.GetProperty(PropertyFloat.ResistanceModifier) ?? defaultBonusModifier) + 1.0f;

            // handle rends

            // should this be combined with weapon resistance, or elemental damage mod?
            // i think its the latter, but its already here, and resistance mod is already being handled in both physical and magic damage calcs,
            // and it seems like the calcs should work out the same (although the cap is now applied in different places)

            var rendDamageType = GetRendDamageType(damageType);
            if (rendDamageType != ImbuedEffectType.Undef && weapon.HasImbuedEffect(rendDamageType) && skill != null)
            {
                var rendingMod = GetRendingMod(skill);
                resistMod *= rendingMod;
            }

            // caps at 250% here?
            resistMod = Math.Min(resistMod, 2.5f);

            return resistMod;
        }

        public static ImbuedEffectType GetRendDamageType(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Slash:
                    return ImbuedEffectType.SlashRending;
                case DamageType.Pierce:
                    return ImbuedEffectType.PierceRending;
                case DamageType.Bludgeon:
                    return ImbuedEffectType.BludgeonRending;
                case DamageType.Fire:
                    return ImbuedEffectType.FireRending;
                case DamageType.Cold:
                    return ImbuedEffectType.ColdRending;
                case DamageType.Acid:
                    return ImbuedEffectType.AcidRending;
                case DamageType.Electric:
                    return ImbuedEffectType.ElectricRending;
                case DamageType.Nether:
                    return ImbuedEffectType.Undef;  // none?
                default:
                    Console.WriteLine($"GetRendDamageType({damageType}) unexpected damage type");
                    return ImbuedEffectType.Undef;
            }
        }

        /// <summary>
        /// Returns TRUE if this item is enchantable, as per the client formula.
        /// </summary>
        public bool IsEnchantable => (ResistMagic ?? 0) < 9999;

        public int? ResistMagic
        {
            get => GetProperty(PropertyInt.ResistMagic);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ResistMagic); else SetProperty(PropertyInt.ResistMagic, value.Value); }
        }

        public bool IgnoreMagicArmor
        {
            get => GetProperty(PropertyBool.IgnoreMagicArmor) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IgnoreMagicArmor); else SetProperty(PropertyBool.IgnoreMagicArmor, value); }
        }

        public bool IgnoreMagicResist
        {
            get => GetProperty(PropertyBool.IgnoreMagicResist) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.IgnoreMagicResist); else SetProperty(PropertyBool.IgnoreMagicResist, value); }
        }

        //
        // Rending
        //
        // Rending gives the weapon the ability to make its opponent vulnerable to attacks of a certain specific element.
        // The amount of vulnerability depends on the attack skill of the wielder.
        // This effect does not stack with Life Magic Vulnerability spells.
        // Rending properties can be added to loot weapons by imbuing them with certain types of Salvage using the Weapon Tinkering skill.
        //
        // Rending can be found on some quest reward weapons, although it is much more common for them to have Resistance Cleaving,
        // which is a fixed effect instead of being skill-based like Rending.
        //

        // From Anon's formula docs:

        // Imbues for melee:
        // BS = base skill. These cap at 400 base. Additional base ranks do nothing more.
        // AR (Armor Rending): (BS - 160) / 400 = % of armor ignored, min 0%, max 60%
        // CS (Critical Strike): (BS - 100) / 600 = crit rate bonus, min 10%, max 50%
        // CB (Crippling Blow): (BS - 40) / 60, crit damage bonus, minimum of 2x, caps at 6x (7x damage cap upstream?)
        // Resistance Rending: BS / 160, elemental vuln damage modifier, capped at 2.5

        // Imbues for missile / war / void:
        // BS = base skill. These cap at 360 base. Additional base ranks do nothing more.
        // AR (Armor Rending) (missile only): (BS - 144) / 360
        // CS (Critical Strike): (BS - 60) / 600
        // CB (Crippling Blow): BS / 60
        // Resistance Rending: BS / 144

        // Later modified:
        // Accurate imbue formulas for CS/CB:

        // CS/Melee: (BS - 100) / 600
        // CB/Melee: (BS - 40) / 60

        // CS/Missile_Magic: (BS - 60) / 600
        // CB/Missile_Magic: BS / 60

        // Note that CS/CB scaling / modifiers / caps are different for PVP

        // Imbue effect hard caps, PVE (repeating some of the info in the doc):
        // - AR minimum of 0%, max of 60% armor ignored
        // - CS minimum of 5% (war/void), or 10% (melee/missile) to max 50% crit chance
        // - CB minimum of 1x (magic) or 2x (melee/missile) to max 6x crit dmg mod (see note below)
        // - Resist Rends min of 1.0x to max 2.5x (equivalent of vuln 6)

        // CB crit damage calc:
        // - Melee/missile: this is a direct multiplier of your maximum non-crit damage.
        // - Magic: my best guess here is that the CB modifier modifies the additional crit damage (see note-2), but i'm not 100% on this

        public static float GetArmorRendingMod(CreatureSkill skill)
        {
            // % of armor ignored, min 0%, max 60%

            var baseSkill = GetBaseSkillImbued(skill);

            switch (GetImbuedSkillType(skill))
            {
                case ImbuedSkillType.Melee:
                    return 1.0f - Math.Max(0, baseSkill - 160) / 400.0f;

                case ImbuedSkillType.Missile:
                    return 1.0f - Math.Max(0, baseSkill - 144) / 360.0f;

                default:
                    return 1.0f;
            }
        }

        public static float GetCriticalStrikeMod(CreatureSkill skill)
        {
            // increases crit chance (additive?)
            // maximum 50% bonus

            var baseSkill = GetBaseSkillImbued(skill);

            var baseMod = 0.0f;
            var skillType = GetImbuedSkillType(skill);
            switch (skillType)
            {
                case ImbuedSkillType.Melee:
                    baseMod = Math.Max(0, baseSkill - 100) / 600.0f;
                    break;

                case ImbuedSkillType.Missile:
                case ImbuedSkillType.Magic:
                    baseMod = Math.Max(0, baseSkill - 60) / 600.0f;
                    break;

                default:
                    return 0.0f;
            }

            switch (skillType)
            {
                // minimum 5% for magic
                case ImbuedSkillType.Magic:
                    return Math.Max(0.05f, baseMod);

                // minimum 10% for melee / missile
                case ImbuedSkillType.Melee:
                case ImbuedSkillType.Missile:
                    return Math.Max(0.10f, baseMod);
            }
            return 0.0f;
        }

        public static float GetCripplingBlowMod(CreatureSkill skill)
        {
            // increases crit damage (additive?)
            // caps at 6x here, possibly 7x cap upstream?

            var baseSkill = GetBaseSkillImbued(skill);

            var baseMod = 0.0f;
            var skillType = GetImbuedSkillType(skill);
            switch (skillType)
            {
                case ImbuedSkillType.Melee:
                    baseMod = Math.Max(0, baseSkill - 40) / 60.0f;
                    break;

                case ImbuedSkillType.Missile:
                case ImbuedSkillType.Magic:
                    baseMod = baseSkill / 60.0f;
                    break;

                default:
                    return 0.0f;
            }

            switch (skillType)
            {
                // minimum bonus for physical only?
                case ImbuedSkillType.Melee:
                case ImbuedSkillType.Missile:
                    return Math.Max(1.0f, baseMod);
            }
            return baseMod;
        }

        public static float GetRendingMod(CreatureSkill skill)
        {
            // elemental vuln damage modifier, capped at 2.5

            var baseSkill = GetBaseSkillImbued(skill);

            switch (GetImbuedSkillType(skill))
            {
                case ImbuedSkillType.Melee:
                    return baseSkill / 160.0f;

                case ImbuedSkillType.Missile:
                case ImbuedSkillType.Magic:
                    return baseSkill / 144.0f;

                default:
                    return 1.0f;
            }
        }

        public static uint GetBaseSkillImbued(CreatureSkill skill)
        {
            switch (GetImbuedSkillType(skill))
            {
                case ImbuedSkillType.Melee:
                    return Math.Min(skill.Base, 400);

                case ImbuedSkillType.Missile:
                case ImbuedSkillType.Magic:
                default:
                    return Math.Min(skill.Base, 360);
            }
        }

        public enum ImbuedSkillType
        {
            Undef,
            Melee,
            Missile,
            Magic
        }

        public static ImbuedSkillType GetImbuedSkillType(CreatureSkill skill)
        {
            switch (skill.Skill)
            {
                case Skill.LightWeapons:
                case Skill.HeavyWeapons:
                case Skill.FinesseWeapons:
                case Skill.DualWield:
                case Skill.TwoHandedCombat:

                // legacy
                case Skill.Axe:
                case Skill.Dagger:
                case Skill.Mace:
                case Skill.Spear:
                case Skill.Staff:
                case Skill.Sword:
                case Skill.UnarmedCombat:

                    return ImbuedSkillType.Melee;

                case Skill.MissileWeapons:

                // legacy
                case Skill.Bow:
                case Skill.Crossbow:
                case Skill.Sling:
                case Skill.ThrownWeapon:

                    return ImbuedSkillType.Missile;


                case Skill.WarMagic:
                case Skill.VoidMagic:

                    return ImbuedSkillType.Magic;

                default:
                    Console.WriteLine($"WorldObject_Weapon.GetImbuedSkillType({skill.Skill}): unexpected skill");
                    return ImbuedSkillType.Undef;
            }
        }
    }
}
