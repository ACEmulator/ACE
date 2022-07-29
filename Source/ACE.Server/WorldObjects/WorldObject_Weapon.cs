using System;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public Skill WeaponSkill
        {
            get => (Skill)(GetProperty(PropertyInt.WeaponSkill) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.WeaponSkill); else SetProperty(PropertyInt.WeaponSkill, (int)value); }
        }

        public DamageType W_DamageType
        {
            get => (DamageType)(GetProperty(PropertyInt.DamageType) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.DamageType); else SetProperty(PropertyInt.DamageType, (int)value); }
        }

        public AttackType W_AttackType
        {
            get => (AttackType)(GetProperty(PropertyInt.AttackType) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.AttackType); else SetProperty(PropertyInt.AttackType, (int)value); }
        }

        public WeaponType W_WeaponType
        {
            get => (WeaponType)(GetProperty(PropertyInt.WeaponType) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt.WeaponType); else SetProperty(PropertyInt.WeaponType, (int)value); }
        }

        public bool AutoWieldLeft
        {
            get => GetProperty(PropertyBool.AutowieldLeft) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.AutowieldLeft); else SetProperty(PropertyBool.AutowieldLeft, value); }
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

        /// <summary>
        /// Returns the primary weapon equipped by a creature
        /// (melee, missile, or wand)
        /// </summary>
        private static WorldObject GetWeapon(Creature wielder, bool forceMainHand = false)
        {
            if (wielder == null)
                return null;

            WorldObject weapon = wielder.GetEquippedWeapon(forceMainHand);

            if (weapon == null)
                weapon = wielder.GetEquippedWand();

            return weapon;
        }

        private const float defaultModifier = 1.0f;

        /// <summary>
        /// Returns the Melee Defense skill modifier for the current weapon
        /// </summary>
        public static float GetWeaponMeleeDefenseModifier(Creature wielder)
        {
            // creatures only receive defense bonus in combat mode
            if (wielder == null || wielder.CombatMode == CombatMode.NonCombat)
                return defaultModifier;

            var mainhand = GetWeapon(wielder, true);
            var offhand = wielder.GetDualWieldWeapon();

            if (offhand == null)
            {
                return GetWeaponMeleeDefenseModifier(wielder, mainhand);
            }
            else
            {
                var mainhand_defenseMod = GetWeaponMeleeDefenseModifier(wielder, mainhand);
                var offhand_defenseMod = GetWeaponMeleeDefenseModifier(wielder, offhand);

                return Math.Max(mainhand_defenseMod, offhand_defenseMod);
            }
        }

        private static float GetWeaponMeleeDefenseModifier(Creature wielder, WorldObject weapon)
        {
            if (weapon == null)
                return defaultModifier;

            //var defenseMod = (float)(weapon.WeaponDefense ?? defaultModifier) + weapon.EnchantmentManager.GetDefenseMod();

            // TODO: Resolve this issue a better way?
            // Because of the way ACE handles default base values in recipe system (or rather the lack thereof)
            // we need to check the following weapon properties to see if they're below expected minimum and adjust accordingly
            // The issue is that the recipe system likely added 0.01 to 0 instead of 1, which is what *should* have happened.
            var baseWepDef = (float)(weapon.WeaponDefense ?? defaultModifier);
            if (weapon.WeaponDefense > 0 && weapon.WeaponDefense < 1 && ((weapon.GetProperty(PropertyInt.ImbueStackingBits) ?? 0) & 4) != 0)
                baseWepDef += 1;

            var defenseMod = baseWepDef + weapon.EnchantmentManager.GetDefenseMod();

            if (weapon.IsEnchantable)
                defenseMod += wielder.EnchantmentManager.GetDefenseMod();

            return defenseMod;
        }

        /// <summary>
        /// Returns the Missile Defense skill modifier for the current weapon
        /// </summary>
        public static float GetWeaponMissileDefenseModifier(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null || wielder.CombatMode == CombatMode.NonCombat)
                return defaultModifier;

            //// no enchantments?
            //return (float)(weapon.WeaponMissileDefense ?? 1.0f);

            var baseWepDef = (float)(weapon.WeaponMissileDefense ?? 1.0f);
            // TODO: Resolve this issue a better way?
            // Because of the way ACE handles default base values in recipe system (or rather the lack thereof)
            // we need to check the following weapon properties to see if they're below expected minimum and adjust accordingly
            // The issue is that the recipe system likely added 0.005 to 0 instead of 1, which is what *should* have happened.
            if (weapon.WeaponMissileDefense > 0 && weapon.WeaponMissileDefense < 1 && ((weapon.GetProperty(PropertyInt.ImbueStackingBits) ?? 0) & 1) == 1)
                baseWepDef += 1;

            // no enchantments?
            return baseWepDef;
        }

        /// <summary>
        /// Returns the Magic Defense skill modifier for the current weapon
        /// </summary>
        public static float GetWeaponMagicDefenseModifier(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null || wielder.CombatMode == CombatMode.NonCombat)
                return defaultModifier;

            //// no enchantments?
            //return (float)(weapon.WeaponMagicDefense ?? 1.0f);

            var baseWepDef = (float)(weapon.WeaponMagicDefense ?? 1.0f);
            // TODO: Resolve this issue a better way?
            // Because of the way ACE handles default base values in recipe system (or rather the lack thereof)
            // we need to check the following weapon properties to see if they're below expected minimum and adjust accordingly
            // The issue is that the recipe system likely added 0.005 to 0 instead of 1, which is what *should* have happened.
            if (weapon.WeaponMagicDefense > 0 && weapon.WeaponMagicDefense < 1 && ((weapon.GetProperty(PropertyInt.ImbueStackingBits) ?? 0) & 1) == 1)
                baseWepDef += 1;

            // no enchantments?
            return baseWepDef;
        }

        /// <summary>
        /// Returns the attack skill modifier for the current weapon
        /// </summary>
        public static float GetWeaponOffenseModifier(Creature wielder)
        {
            // creatures only receive offense bonus in combat mode
            if (wielder == null || wielder.CombatMode == CombatMode.NonCombat)
                return defaultModifier;

            var mainhand = GetWeapon(wielder, true);
            var offhand = wielder.GetDualWieldWeapon();

            if (offhand == null)
            {
                return GetWeaponOffenseModifier(wielder, mainhand);
            }
            else
            {
                var mainhand_attackMod = GetWeaponOffenseModifier(wielder, mainhand);
                var offhand_attackMod = GetWeaponOffenseModifier(wielder, offhand);

                return Math.Max(mainhand_attackMod, offhand_attackMod);
            }
        }

        private static float GetWeaponOffenseModifier(Creature wielder, WorldObject weapon)
        {
            /* Excerpt from http://acpedia.org/wiki/Announcements_-_2002/07_-_Repercussions#Letter_to_the_Players
             The second issue will, in some ways, be both more troubling and more inconsequential for players. HeartSeeker does not affect missile launchers.
             It never has. Bows, crossbows, and atlatls get no benefit from the HeartSeeker spell or from innate attack bonuses (such as those found on the Singularity Bow).
             The only variables that determine whether a missile character hits their target is their bow/xbow/tw skill, the missile defense of the target, and where they set their accuracy meter while they are attacking.
             However, the Defender spell, as well as innate defensive bonuses, do work on missile launchers.
             The AC Live team has been aware of this for the last several months. Once we knew the situation, the question became what to do about it. Should we “fix” an issue that probably isn't broken?
             Almost no archer/atlatler complains about not being able to hit their target.
             They have a built in “HeartSeeker” all the time.
             If anything, most monsters' missile defense scores have historically been so low that many players regard archery as the fastest way to level a character up through the first 30-40 levels.
             We did not feel that “fixing” such a system would improve the game balance for anyone in Asheron's Call, archer or no.
             Ultimately, we decided to resolve the situation through our changes to the treasure system this month. From now on, missile launchers will have a chance of having an innate defensive bonus, but not an offensive one.
             While many old quest weapons still retain their (useless) attack bonus, we will not be putting any new ones into the system.
             */
            if (weapon == null || weapon.IsRanged /* see note above */)
                return defaultModifier;

            var offenseMod = (float)(weapon.WeaponOffense ?? defaultModifier) + weapon.EnchantmentManager.GetAttackMod();

            if (weapon.IsEnchantable)
                offenseMod += wielder.EnchantmentManager.GetAttackMod();

            return offenseMod;
        }

        /// <summary>
        /// Returns the Mana Conversion skill modifier for the current weapon
        /// </summary>
        public static float GetWeaponManaConversionModifier(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultModifier;

            if (wielder.CombatMode != CombatMode.NonCombat)
            {
                // hermetic link / void

                // base mod starts at 0
                var baseMod = (float)(weapon.ManaConversionMod ?? 0.0f);

                // enchantments are multiplicative, so they are only effective if there is a base mod
                var manaConvMod = weapon.EnchantmentManager.GetManaConvMod();

                var auraManaConvMod = 1.0f;

                if (weapon.IsEnchantable)
                    auraManaConvMod = wielder?.EnchantmentManager.GetManaConvMod() ?? 1.0f;

                var enchantmentMod = manaConvMod * auraManaConvMod;

                return 1.0f + baseMod * enchantmentMod;
            }

            return defaultModifier;
        }

        private const uint defaultSpeed = 40;   // TODO: find default speed

        /// <summary>
        /// Returns the weapon speed, with enchantments factored in
        /// </summary>
        public static uint GetWeaponSpeed(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            var baseSpeed = weapon?.WeaponTime ?? (int)defaultSpeed;

            var speedMod = weapon != null ? weapon.EnchantmentManager.GetWeaponSpeedMod() : 0;
            var auraSpeedMod = wielder != null ? wielder.EnchantmentManager.GetWeaponSpeedMod() : 0;

            return (uint)Math.Max(0, baseSpeed + speedMod + auraSpeedMod);
        }

        /// <summary>
        /// Biting Strike
        /// </summary>
        public double? CriticalFrequency
        {
            get => GetProperty(PropertyFloat.CriticalFrequency);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.CriticalFrequency); else SetProperty(PropertyFloat.CriticalFrequency, value.Value); }
        }

        private const float defaultPhysicalCritFrequency = 0.1f;    // 10% base chance

        /// <summary>
        /// Returns the critical chance for the attack weapon
        /// </summary>
        public static float GetWeaponCriticalChance(WorldObject weapon, Creature wielder, CreatureSkill skill, Creature target)
        {
            var critRate = (float)(weapon?.CriticalFrequency ?? defaultPhysicalCritFrequency);

            if (weapon != null && weapon.HasImbuedEffect(ImbuedEffectType.CriticalStrike))
            {
                var criticalStrikeBonus = GetCriticalStrikeMod(skill);

                critRate = Math.Max(critRate, criticalStrikeBonus);
            }

            if (wielder != null)
                critRate += wielder.GetCritRating() * 0.01f;

            // mitigation
            var critResistRatingMod = Creature.GetNegativeRatingMod(target.GetCritResistRating());
            critRate *= critResistRatingMod;

            return critRate;
        }

        // http://acpedia.org/wiki/Announcements_-_2002/08_-_Atonement#Letter_to_the_Players - 2% originally

        // http://acpedia.org/wiki/Announcements_-_2002/11_-_The_Iron_Coast#Release_Notes
        // The chance for causing a critical hit with magic, both with and without a Critical Strike wand, has been increased.
        // what this was actually increased to for base, was never stated directly in the dev notes
        // speculation is that it was 5%, to align with the minimum that CS magic scales from

        private const float defaultMagicCritFrequency = 0.05f;

        /// <summary>
        /// Returns the critical chance for the caster weapon
        /// </summary>
        public static float GetWeaponMagicCritFrequency(WorldObject weapon, Creature wielder, CreatureSkill skill, Creature target)
        {
            // TODO : merge with above function

            if (weapon == null)
                return defaultMagicCritFrequency;

            var critRate = (float)(weapon.GetProperty(PropertyFloat.CriticalFrequency) ?? defaultMagicCritFrequency);

            if (weapon.HasImbuedEffect(ImbuedEffectType.CriticalStrike))
            {
                var isPvP = wielder is Player && target is Player;

                var criticalStrikeMod = GetCriticalStrikeMod(skill, isPvP);

                critRate = Math.Max(critRate, criticalStrikeMod);
            }

            critRate += wielder.GetCritRating() * 0.01f;

            // mitigation
            var critResistRatingMod = Creature.GetNegativeRatingMod(target.GetCritResistRating());
            critRate *= critResistRatingMod;

            return critRate;
        }

        private const float defaultCritDamageMultiplier = 1.0f;

        /// <summary>
        /// Returns the critical damage multiplier for the attack weapon
        /// </summary>
        public static float GetWeaponCritDamageMod(WorldObject weapon, Creature wielder, CreatureSkill skill, Creature target)
        {
            var critDamageMod = (float)(weapon?.GetProperty(PropertyFloat.CriticalMultiplier) ?? defaultCritDamageMultiplier);

            if (weapon != null && weapon.HasImbuedEffect(ImbuedEffectType.CripplingBlow))
            {
                var cripplingBlowMod = GetCripplingBlowMod(skill);

                critDamageMod = Math.Max(critDamageMod, cripplingBlowMod); 
            }
            return critDamageMod;
        }

        /// <summary>
        /// PvP damaged is halved, automatically displayed in the client
        /// </summary>
        public static readonly float ElementalDamageBonusPvPReduction = 0.5f;

        /// <summary>
        /// Returns a multiplicative elemental damage modifier for the magic caster weapon type
        /// </summary>
        public static float GetCasterElementalDamageModifier(WorldObject weapon, Creature wielder, Creature target, DamageType damageType)
        {
            if (wielder == null || !(weapon is Caster) || weapon.W_DamageType != damageType)
                return 1.0f;

            var elementalDamageMod = weapon.ElementalDamageMod ?? 1.0f;

            // additive to base multiplier
            var wielderEnchantments = wielder.EnchantmentManager.GetElementalDamageMod();
            var weaponEnchantments = weapon.EnchantmentManager.GetElementalDamageMod();

            var enchantments = wielderEnchantments + weaponEnchantments;

            var modifier = (float)(elementalDamageMod + enchantments);

            if (modifier > 1.0f && target is Player)
                modifier = 1.0f + (modifier - 1.0f) * ElementalDamageBonusPvPReduction;

            return modifier;
        }

        /// <summary>
        /// Returns an additive elemental damage bonus for the missile launcher weapon type
        /// </summary>
        public static int GetMissileElementalDamageBonus(WorldObject weapon, Creature wielder, DamageType damageType)
        {
            if (weapon is MissileLauncher && weapon.ElementalDamageBonus != null)
            {
                var elementalDamageType = weapon.W_DamageType;

                if (elementalDamageType != DamageType.Undef && elementalDamageType == damageType)
                    return weapon.ElementalDamageBonus.Value;
            }
            return 0;
        }

        public CreatureType? SlayerCreatureType
        {
            get => (CreatureType?)GetProperty(PropertyInt.SlayerCreatureType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.SlayerCreatureType); else SetProperty(PropertyInt.SlayerCreatureType, (int)value.Value); }
        }

        public double? SlayerDamageBonus
        {
            get => GetProperty(PropertyFloat.SlayerDamageBonus);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.SlayerDamageBonus); else SetProperty(PropertyFloat.SlayerDamageBonus, value.Value); }
        }

        /// <summary>
        /// Returns the slayer damage multiplier for the attack weapon
        /// against a particular creature type
        /// </summary>
        public static float GetWeaponCreatureSlayerModifier(WorldObject weapon, Creature wielder, Creature target)
        {
            if (weapon != null && weapon.SlayerCreatureType != null && weapon.SlayerDamageBonus != null &&
                target != null && weapon.SlayerCreatureType == target.CreatureType)
            {
                // TODO: scale with base weapon skill?
                return (float)weapon.SlayerDamageBonus;
            }
            else
                return defaultModifier;
        }

        public DamageType? ResistanceModifierType
        {
            get => (DamageType?)GetProperty(PropertyInt.ResistanceModifierType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ResistanceModifierType); else SetProperty(PropertyInt.ResistanceModifierType, (int)value.Value); }
        }

        public double? ResistanceModifier
        {
            get => GetProperty(PropertyFloat.ResistanceModifier);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResistanceModifier); else SetProperty(PropertyFloat.ResistanceModifier, value.Value); }
        }

        /// <summary>
        /// Returns the resistance modifier or rending modifier
        /// </summary>
        public static float GetWeaponResistanceModifier(WorldObject weapon, Creature wielder, CreatureSkill skill, DamageType damageType)
        {
            float resistMod = defaultModifier;

            if (wielder == null || weapon == null)
                return defaultModifier;

            // handle quest weapon fixed resistance cleaving
            if (weapon.ResistanceModifierType != null && weapon.ResistanceModifierType == damageType)
                resistMod = 1.0f + (float)(weapon.ResistanceModifier ?? defaultModifier);       // 1.0 in the data, equivalent to a level 5 vuln

            // handle elemental resistance rending
            var rendDamageType = GetRendDamageType(damageType);

            if (rendDamageType == ImbuedEffectType.Undef)
                log.Debug($"{wielder.Name}.GetRendDamageType({damageType}) unexpected damage type for {weapon.Name} ({weapon.Guid})");

            if (rendDamageType != ImbuedEffectType.Undef && weapon.HasImbuedEffect(rendDamageType) && skill != null)
            {
                var rendingMod = GetRendingMod(skill);

                resistMod = Math.Max(resistMod, rendingMod);
            }

            return resistMod;
        }

        public ImbuedEffectType GetImbuedEffects()
        {
            return (ImbuedEffectType)(
                (GetProperty(PropertyInt.ImbuedEffect) ?? 0) |
                (GetProperty(PropertyInt.ImbuedEffect2) ?? 0) |
                (GetProperty(PropertyInt.ImbuedEffect3) ?? 0) |
                (GetProperty(PropertyInt.ImbuedEffect4) ?? 0) |
                (GetProperty(PropertyInt.ImbuedEffect5) ?? 0));
        }

        public bool HasImbuedEffect(ImbuedEffectType type)
        {
            return ImbuedEffect.HasFlag(type);
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
                    return ImbuedEffectType.NetherRending;
                default:
                    //log.Debug($"GetRendDamageType({damageType}) unexpected damage type");
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

        // http://acpedia.org/wiki/Announcements_-_2004/07_-_Treaties_in_Stone#Letter_to_the_Players

        // For PvE only:

        // Critical Strike for War Magic currently scales from 5% critical hit chance to 25% critical hit chance at maximum effectiveness.
        // In July, the maximum effectiveness will be increased to 50% chance.

        //public static float MinCriticalStrikeMagicMod = 0.05f;

        public static float MaxCriticalStrikeMod = 0.5f;

        public static float GetCriticalStrikeMod(CreatureSkill skill, bool isPvP = false)
        {
            var baseMod = 0.0f;

            var skillType = GetImbuedSkillType(skill);

            var baseSkill = GetBaseSkillImbued(skill);

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

            // http://acpedia.org/wiki/Announcements_-_2004/07_-_Treaties_in_Stone#Letter_to_the_Players

            // For PvE only:

            // Critical Strike for War Magic currently scales from 5% critical hit chance to 25% critical hit chance at maximum effectiveness.
            // In July, the maximum effectiveness will be increased to 50% chance.

            if (skillType == ImbuedSkillType.Magic && isPvP)
                baseMod *= 0.5f;

            // In the original formula for CS Magic pre-July 2004, (BS - 60) / 1200.0f, the minimum 5% crit rate would have been achieved at BS 120,
            // which is exactly equal to the minimum base skill for CS Missile becoming effective.

            // CS Magic is slightly different from all the other skill/imbue combinations, in that the MinCriticalStrikeMagicMod
            // is different from the defaultMagicCritFrequency (5% vs. 2%)

            // If we simply clamp to min. 5% here, then a player will be getting a +3% bonus from from base skill 0-90 in PvE,
            // and base skill 0-120 in PvP

            // This code is checking if the player has reached the skill threshold for receiving the 5% bonus
            // (base skill 90 in PvE, base skill 120 in PvP)

            /*var criticalStrikeMod = skillType == ImbuedSkillType.Magic ? defaultMagicCritFrequency : defaultPhysicalCritFrequency;

            var minEffective = skillType == ImbuedSkillType.Magic ? MinCriticalStrikeMagicMod : defaultPhysicalCritFrequency;

            if (baseMod >= minEffective)
                criticalStrikeMod = baseMod;*/

            var defaultCritFrequency = skillType == ImbuedSkillType.Magic ? defaultMagicCritFrequency : defaultPhysicalCritFrequency;

            var criticalStrikeMod = Math.Max(defaultCritFrequency, baseMod);

            //Console.WriteLine($"CriticalStrikeMod: {criticalStrikeMod}");

            return criticalStrikeMod;
        }

        public static float MaxCripplingBlowMod = 6.0f;

        public static float GetCripplingBlowMod(CreatureSkill skill)
        {
            // increases the critical damage multiplier, additive

            // http://acpedia.org/wiki/Announcements_-_2004/07_-_Treaties_in_Stone#Letter_to_the_Players

            // PvP only:

            // Crippling Blow for War Magic currently scales from adding 50% of the spells damage on critical hits to adding 100% at maximum effectiveness.
            // In July, the maximum effectiveness will be increased to adding up to 500% of the spell's damage.

            // ( +500% sounds like it would be 6.0 multiplier)

            var baseSkill = GetBaseSkillImbued(skill);

            var baseMod = 1.0f;

            switch(GetImbuedSkillType(skill))
            {
                case ImbuedSkillType.Melee:
                    baseMod = Math.Max(0, baseSkill - 40) / 60.0f;
                    break;

                case ImbuedSkillType.Missile:
                case ImbuedSkillType.Magic:

                    baseMod = baseSkill / 60.0f;
                    break;
            }

            var cripplingBlowMod = Math.Max(1.0f, baseMod);

            //Console.WriteLine($"CripplingBlowMod: {cripplingBlowMod}");

            return cripplingBlowMod;
        }

        // elemental rending cap, equivalent to level 6 vuln
        public static float MaxRendingMod = 2.5f;

        public static float GetRendingMod(CreatureSkill skill)
        {
            var baseSkill = GetBaseSkillImbued(skill);

            var rendingMod = 1.0f;

            switch (GetImbuedSkillType(skill))
            {
                case ImbuedSkillType.Melee:
                    rendingMod = baseSkill / 160.0f;
                    break;

                case ImbuedSkillType.Missile:
                case ImbuedSkillType.Magic:
                    rendingMod = baseSkill / 144.0f;
                    break;
            }

            rendingMod = Math.Clamp(rendingMod, 1.0f, MaxRendingMod);

            //Console.WriteLine($"RendingMod: {rendingMod}");

            return rendingMod;
        }

        public static float MaxArmorRendingMod = 0.6f;

        public static float GetArmorRendingMod(CreatureSkill skill)
        {
            // % of armor ignored, min 0%, max 60%

            var baseSkill = GetBaseSkillImbued(skill);

            var armorRendingMod = 1.0f;

            switch (GetImbuedSkillType(skill))
            {
                case ImbuedSkillType.Melee:
                    armorRendingMod -= Math.Max(0, baseSkill - 160) / 400.0f;
                    break;

                case ImbuedSkillType.Missile:
                    armorRendingMod -= Math.Max(0, baseSkill - 144) / 360.0f;
                    break;
            }

            //Console.WriteLine($"ArmorRendingMod: {armorRendingMod}");

            return armorRendingMod;
        }

        /// <summary>
        /// Armor Cleaving
        /// </summary>
        public double? IgnoreArmor
        {
            get => GetProperty(PropertyFloat.IgnoreArmor);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.IgnoreArmor); else SetProperty(PropertyFloat.IgnoreArmor, value.Value); }
        }

        public float GetArmorCleavingMod(WorldObject weapon)
        {
            // investigate: should this value be on creatures directly?
            var creatureMod = GetArmorCleavingMod();
            var weaponMod = weapon != null ? weapon.GetArmorCleavingMod() : 1.0f;

            return Math.Min(creatureMod, weaponMod);
        }

        public float GetArmorCleavingMod()
        {
            if (IgnoreArmor == null)
                return 1.0f;

            // FIXME: data
            var maxSpellLevel = GetMaxSpellLevel();

            // thanks to moro for this formula
            return 1.0f - (0.1f + maxSpellLevel * 0.05f);
        }

        public double? IgnoreShield
        {
            get => GetProperty(PropertyFloat.IgnoreShield);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.IgnoreShield); else SetProperty(PropertyFloat.IgnoreShield, value.Value); }
        }

        public float GetIgnoreShieldMod(WorldObject weapon)
        {
            var creatureMod = IgnoreShield ?? 0.0f;
            var weaponMod = weapon?.IgnoreShield ?? 0.0f;

            return 1.0f - (float)Math.Max(creatureMod, weaponMod);
        }

        public static int GetBaseSkillImbued(CreatureSkill skill)
        {
            switch (GetImbuedSkillType(skill))
            {
                case ImbuedSkillType.Melee:
                    return (int)Math.Min(skill.Base, 400);

                case ImbuedSkillType.Missile:
                case ImbuedSkillType.Magic:
                default:
                    return (int)Math.Min(skill.Base, 360);
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
            switch (skill?.Skill)
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
                case Skill.LifeMagic:   // Martyr's Hecatomb

                    return ImbuedSkillType.Magic;

                default:
                    log.Debug($"WorldObject_Weapon.GetImbuedSkillType({skill?.Skill}): unexpected skill");
                    return ImbuedSkillType.Undef;
            }
        }

        /// <summary>
        /// Returns the base skill multiplier to the maximum bonus
        /// </summary>
        public static float GetImbuedInterval(CreatureSkill skill, bool useMin = true)
        {
            var skillType = GetImbuedSkillType(skill);

            var min = 0;
            if (useMin)
                min = skillType == ImbuedSkillType.Melee ? 150 : 125;
            var max = skillType == ImbuedSkillType.Melee ? 400 : 360;

            return GetInterval((int)skill.Base, min, max);
        }

        /// <summary>
        /// Returns an interval between 0-1
        /// </summary>
        public static float GetInterval(int num, int min, int max)
        {
            if (num <= min) return 0.0f;
            if (num >= max) return 1.0f;

            var range = max - min;

            return (float)(num - min) / range;
        }

        /// <summary>
        /// Projects a 0-1 interval between min and max
        /// </summary>
        public static float SetInterval(float interval, float min, float max)
        {
            var range = max - min;

            return interval * range + min;
        }

        /// Spell ID for 'Cast on Strike'
        /// </summary>
        public uint? ProcSpell
        {
            get => GetProperty(PropertyDataId.ProcSpell);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.ProcSpell); else SetProperty(PropertyDataId.ProcSpell, value.Value); }
        }

        /// <summary>
        /// The chance for activating 'Cast on strike' spell
        /// </summary>
        public double? ProcSpellRate
        {
            get => GetProperty(PropertyFloat.ProcSpellRate);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ProcSpellRate); else SetProperty(PropertyFloat.ProcSpellRate, value.Value); }
        }

        /// <summary>
        /// If TRUE, 'Cast on strike' spell targets self
        /// instead of the target
        /// </summary>
        public bool ProcSpellSelfTargeted
        {
            get => GetProperty(PropertyBool.ProcSpellSelfTargeted) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.ProcSpellSelfTargeted); else SetProperty(PropertyBool.ProcSpellSelfTargeted, value); }
        }

        /// <summary>
        /// Returns TRUE if this item has a proc / 'cast on strike' spell
        /// </summary>
        public bool HasProc => ProcSpell != null;

        /// <summary>
        /// Returns TRUE if this item has a proc spell
        /// that matches the input spell
        /// </summary>
        public bool HasProcSpell(uint spellID)
        {
            return HasProc && ProcSpell == spellID;
        }

        public void TryProcItem(WorldObject attacker, Creature target)
        {
            // roll for a chance of casting spell
            var chance = ProcSpellRate ?? 0.0f;

            // special handling for aetheria
            if (Aetheria.IsAetheria(WeenieClassId) && attacker is Creature wielder)
                chance = Aetheria.CalcProcRate(this, wielder);

            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            if (rng >= chance)
                return;

            var spell = new Spell(ProcSpell.Value);

            if (spell.NotFound)
            {
                if (attacker is Player player)
                {
                    if (spell._spellBase == null)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"SpellId {ProcSpell.Value} Invalid.", ChatMessageType.System));
                    else
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                }
                return;
            }

            var itemCaster = this is Creature ? null : this;

            if (spell.NonComponentTargetType == ItemType.None)
                attacker.TryCastSpell(spell, null, itemCaster, itemCaster, true, true);
            else
                attacker.TryCastSpell(spell, target, itemCaster, itemCaster, true, true);
        }

        private bool? isMasterable;

        public bool IsMasterable
        {
            get
            {
                // should be based on this, but a bunch of the weapon data probably needs to be updated...
                //return W_WeaponType != WeaponType.Undef;

                // cache this?
                if (isMasterable == null)
                    isMasterable = LongDesc == null || !LongDesc.Contains("This weapon seems tough to master.", StringComparison.OrdinalIgnoreCase);

                return isMasterable.Value;
            }
        }

        // from the Dark Majesty strategy guide, page 150:

        // -   0 - 1/3 sec. Power-up Time = High Stab
        // - 1/3 - 2/3 sec. Power-up Time = High Backhand
        // -       2/3 sec+ Power-up Time = High Slash

        public static readonly float ThrustThreshold = 0.33f;

        /// <summary>
        /// Returns TRUE if this is a thrust/slash weapon,
        /// or if this weapon uses 2 different attack types based on the ThrustThreshold
        /// </summary>
        public bool IsThrustSlash
        {
            get
            {
                return W_AttackType.HasFlag(AttackType.Slash | AttackType.Thrust) ||
                       W_AttackType.HasFlag(AttackType.DoubleSlash | AttackType.DoubleThrust) ||
                       W_AttackType.HasFlag(AttackType.TripleSlash | AttackType.TripleThrust) ||
                       W_AttackType.HasFlag(AttackType.DoubleSlash);  // stiletto
            }
        }

        public AttackType GetAttackType(MotionStance stance, float powerLevel, bool offhand)
        {
            if (offhand)
                return GetOffhandAttackType(stance, powerLevel);

            var attackType = W_AttackType;

            if ((attackType & AttackType.Offhand) != 0)
            {
                log.Warn($"{Name} ({Guid}, {WeenieClassId}).GetAttackType(): {attackType}");
                attackType &= ~AttackType.Offhand;
            }

            if (stance == MotionStance.DualWieldCombat)
            {
                if (attackType.HasFlag(AttackType.TripleThrust | AttackType.TripleSlash))
                {
                    if (powerLevel >= ThrustThreshold)
                        attackType = AttackType.TripleSlash;
                    else
                        attackType = AttackType.TripleThrust;
                }
                else if (attackType.HasFlag(AttackType.DoubleThrust | AttackType.DoubleSlash))
                {
                    if (powerLevel >= ThrustThreshold)
                        attackType = AttackType.DoubleSlash;
                    else
                        attackType = AttackType.DoubleThrust;
                }

                // handle old bugged stilettos that only have DoubleThrust
                // handle old bugged rapiers w/ Thrust, DoubleThrust
                else if (attackType.HasFlag(AttackType.DoubleThrust))
                {
                    if (powerLevel >= ThrustThreshold || !attackType.HasFlag(AttackType.Thrust))
                        attackType = AttackType.DoubleThrust;
                    else
                        attackType = AttackType.Thrust;
                }

                // handle old bugged poniards and newer tachis
                else if (attackType.HasFlag(AttackType.Thrust | AttackType.DoubleSlash))
                {
                    if (powerLevel >= ThrustThreshold)
                        attackType = AttackType.DoubleSlash;
                    else
                        attackType = AttackType.Thrust;
                }

                // gaerlan sword / py16 (iasparailaun)
                else if (attackType.HasFlag(AttackType.Thrust | AttackType.TripleSlash))
                {
                    if (powerLevel >= ThrustThreshold)
                        attackType = AttackType.TripleSlash;
                    else
                        attackType = AttackType.Thrust;
                }
            }
            else if (stance == MotionStance.SwordShieldCombat)
            {
                // force thrust animation when using a shield with a multi-strike weapon
                if (attackType.HasFlag(AttackType.TripleThrust))
                {
                    if (powerLevel >= ThrustThreshold || !attackType.HasFlag(AttackType.Thrust))
                        attackType = AttackType.TripleThrust;
                    else
                        attackType = AttackType.Thrust;
                }
                else if (attackType.HasFlag(AttackType.DoubleThrust))
                {
                    if (powerLevel >= ThrustThreshold || !attackType.HasFlag(AttackType.Thrust))
                        attackType = AttackType.DoubleThrust;
                    else
                        attackType = AttackType.Thrust;
                }

                // handle old bugged poniards and newer tachis w/ Thrust, DoubleSlash
                // and gaerlan sword / py16 (iasparailaun) w/ Thrust, TripleSlash
                else if (attackType.HasFlag(AttackType.Thrust) && (attackType & (AttackType.DoubleSlash | AttackType.TripleSlash)) != 0)
                    attackType = AttackType.Thrust;
            }
            else if (stance == MotionStance.SwordCombat)
            {
                // force slash animation when using no shield with a multi-strike weapon
                if (attackType.HasFlag(AttackType.TripleSlash))
                {
                    if (powerLevel >= ThrustThreshold || !attackType.HasFlag(AttackType.Thrust))
                        attackType = AttackType.TripleSlash;
                    else
                        attackType = AttackType.Thrust;
                }
                else if (attackType.HasFlag(AttackType.DoubleSlash))
                {
                    if (powerLevel >= ThrustThreshold || !attackType.HasFlag(AttackType.Thrust))
                        attackType = AttackType.DoubleSlash;
                    else
                        attackType = AttackType.Thrust;
                }

                // handle old bugged stilettos that only have DoubleThrust
                else if (attackType.HasFlag(AttackType.DoubleThrust))
                    attackType = AttackType.Thrust;
            }

            if (attackType.HasFlag(AttackType.Thrust | AttackType.Slash))
            {
                if (powerLevel >= ThrustThreshold)
                    attackType = AttackType.Slash;
                else
                    attackType = AttackType.Thrust;
            }

            return attackType;
        }

        public AttackType GetOffhandAttackType(MotionStance stance, float powerLevel)
        {
            var attackType = W_AttackType;

            if ((attackType & AttackType.Offhand) != 0)
            {
                log.Warn($"{Name} ({Guid}, {WeenieClassId}).GetOffhandAttackType(): {attackType}");
                attackType &= ~AttackType.Offhand;
            }

            if (attackType.HasFlag(AttackType.TripleThrust | AttackType.TripleSlash))
            {
                if (powerLevel >= ThrustThreshold)
                    attackType = AttackType.OffhandTripleSlash;
                else
                    attackType = AttackType.OffhandTripleThrust;
            }
            else if (attackType.HasFlag(AttackType.DoubleThrust | AttackType.DoubleSlash))
            {
                if (powerLevel >= ThrustThreshold)
                    attackType = AttackType.OffhandDoubleSlash;
                else
                    attackType = AttackType.OffhandDoubleThrust;
            }

            // handle old bugged stilettos that only have DoubleThrust
            // handle old bugged rapiers w/ Thrust, DoubleThrust
            else if (attackType.HasFlag(AttackType.DoubleThrust))
            {
                if (powerLevel >= ThrustThreshold || !attackType.HasFlag(AttackType.Thrust))
                    attackType = AttackType.OffhandDoubleThrust;
                else
                    attackType = AttackType.OffhandThrust;
            }

            // handle old bugged poniards and newer tachis w/ Thrust, DoubleSlash
            else if (attackType.HasFlag(AttackType.Thrust | AttackType.DoubleSlash))
            {
                if (powerLevel >= ThrustThreshold)
                    attackType = AttackType.OffhandDoubleSlash;
                else
                    attackType = AttackType.OffhandThrust;
            }

            // gaerlan sword / py16 (iasparailaun) w/ Thrust, TripleSlash
            else if (attackType.HasFlag(AttackType.Thrust | AttackType.TripleSlash))
            {
                if (powerLevel >= ThrustThreshold)
                    attackType = AttackType.OffhandTripleSlash;
                else
                    attackType = AttackType.OffhandThrust;
            }

            else if (attackType.HasFlag(AttackType.Thrust | AttackType.Slash))
            {
                if (powerLevel >= ThrustThreshold)
                    attackType = AttackType.OffhandSlash;
                else
                    attackType = AttackType.OffhandThrust;
            }
            else
            {
                switch (attackType)
                {
                    case AttackType.Thrust:
                        attackType = AttackType.OffhandThrust;
                        break;

                    case AttackType.Slash:
                        attackType = AttackType.OffhandSlash;
                        break;

                    case AttackType.Punch:
                        attackType = AttackType.OffhandPunch;
                        break;
                }
            }
            return attackType;
        }
    }
}
