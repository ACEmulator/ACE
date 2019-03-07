using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class DamageEvent
    {
        // factors:
        // - lifestone protection
        // - evade
        //   - offense mod (heart seeker)
        //      - accuracy mod (missile)
        //   - defense mod (defender)
        //      - stamina mod
        // - base damage / mod
        // - damage rating / mod
        //   - recklessness
        //   - sneak attack
        //   - heritage bonus
        // - damage resistance rating /mod
        // - power meter mod
        // - critical (chance % mod / critical damage mod)
        // - attribute mod
        // - armor / mod (base al, impen / bane, life armor / imperil)
        // - elemental damage bonus
        // - slayer mod
        // - resistance mod (natural, prot, vuln)
        //   - resistance cleaving
        // - shield mod
        // - rending mod

        public Creature Attacker;
        public Creature Defender;

        public CombatType CombatType;   // melee / missile / magic

        public WorldObject DamageSource;
        public DamageType DamageType;

        public WorldObject Weapon;      // the attacker's weapon. this can be different from DamageSource,
                                        // ie. for a missile attack, the missile would the DamageSource,
                                        // and the buffs would come from the Weapon

        public AttackType AttackType;   // slash / thrust / punch / kick / offhand / multistrike
        public AttackHeight AttackHeight;

        public bool LifestoneProtection;

        public float EvasionChance;
        public uint EffectiveAttackSkill;
        public uint EffectiveDefenseSkill;
        public float AccuracyMod;

        public bool Evaded;

        public Range BaseDamageRange;
        public float BaseDamage;

        public float AttributeMod;
        public float PowerMod;
        public float SlayerMod;

        public float ElementalDamageBonus;

        public float DamageRatingBaseMod;
        public float RecklessnessMod;
        public float SneakAttackMod;
        public float HeritageMod;

        public float DamageRatingMod;

        public bool IsCritical;

        public float CriticalChance;
        public float CriticalDamageMod;

        public float DamageBeforeMitigation;

        public float ArmorMod;
        public float ResistanceMod;
        public float ShieldMod;
        public float WeaponResistanceMod;

        public float DamageResistanceRatingMod;

        public float DamageMitigated;

        // creature attacker
        public CombatManeuver CombatManeuver;
        public BiotaPropertiesBodyPart AttackPart;      // the body part this monster is attacking with

        public bool IgnoreMagicArmor => DamageSource != null ? DamageSource.IgnoreMagicArmor : false;       // ignores impen / banes
        public bool IgnoreMagicResist => DamageSource != null ? DamageSource.IgnoreMagicResist : false;     // ignores life armor / prots

        // player defender
        public BodyPart BodyPart;
        public List<WorldObject> Armor;

        // creature defender
        public BiotaPropertiesBodyPart BiotaPropertiesBodyPart;
        public Creature_BodyPart CreaturePart;

        public float Damage;

        public bool GeneralFailure;

        public bool HasDamage => !Evaded && !LifestoneProtection;

        public static DamageEvent CalculateDamage(Creature attacker, Creature defender, WorldObject damageSource, CombatManeuver combatManeuver = null)
        {
            var damageEvent = new DamageEvent();
            damageEvent.CombatManeuver = combatManeuver;
            if (damageSource == null)
                damageSource = attacker;

            var damage = damageEvent.DoCalculateDamage(attacker, defender, damageSource);

            return damageEvent;
        }

        private float DoCalculateDamage(Creature attacker, Creature defender, WorldObject damageSource)
        {
            var playerAttacker = attacker as Player;
            var playerDefender = defender as Player;

            Attacker = attacker;
            Defender = defender;

            CombatType = attacker.GetCombatType();

            DamageSource = damageSource;

            Weapon = attacker.GetEquippedWeapon();

            AttackType = attacker.GetAttackType(Weapon, CombatManeuver);
            AttackHeight = attacker.AttackHeight ?? AttackHeight.Medium;

            // check lifestone protection
            if (playerDefender != null && playerDefender.UnderLifestoneProtection)
            {
                LifestoneProtection = true;
                playerDefender.HandleLifestoneProtection();
                return 0.0f;
            }

            if (defender.Invincible ?? false)
                return 0.0f;

            // evasion chance
            EvasionChance = GetEvadeChance(attacker, defender);
            if (EvasionChance > ThreadSafeRandom.Next(0.0f, 1.0f))
            {
                Evaded = true;
                return 0.0f;
            }

            // get base damage
            if (playerAttacker != null)
                GetBaseDamage(playerAttacker, CombatManeuver);
            else
                GetBaseDamage(attacker, CombatManeuver);

            if (GeneralFailure) return 0.0f;

            // get damage modifiers
            PowerMod = attacker.GetPowerMod(Weapon);
            AttributeMod = attacker.GetAttributeMod(Weapon);
            SlayerMod = WorldObject.GetWeaponCreatureSlayerModifier(attacker, defender);

            // additive?
            ElementalDamageBonus = WorldObject.GetMissileElementalDamageModifier(attacker, defender, DamageType);

            // ratings
            DamageRatingBaseMod = Creature.GetPositiveRatingMod(attacker.EnchantmentManager.GetDamageRating());
            RecklessnessMod = Creature.GetRecklessnessMod(attacker, defender);
            SneakAttackMod = attacker.GetSneakAttackMod(defender);
            HeritageMod = attacker.GetHeritageBonus(Weapon) ? 1.05f : 1.0f;

            DamageRatingMod = Creature.AdditiveCombine(DamageRatingBaseMod, RecklessnessMod, SneakAttackMod, HeritageMod);

            // damage before mitigation
            DamageBeforeMitigation = BaseDamage * AttributeMod * PowerMod * SlayerMod * DamageRatingMod + ElementalDamageBonus;   // additives on the end?

            // critical hit?
            var attackSkill = attacker.GetCreatureSkill(attacker.GetCurrentWeaponSkill());
            CriticalChance = WorldObject.GetWeaponCritChanceModifier(attacker, attackSkill, defender);
            if (CriticalChance > ThreadSafeRandom.Next(0.0f, 1.0f))
            {
                var criticalDefended = false;
                if (playerDefender != null && playerDefender.AugmentationCriticalDefense > 0)
                {
                    var criticalDefenseMod = playerAttacker != null ? 0.05f : 0.25f;
                    var criticalDefenseChance = playerDefender.AugmentationCriticalDefense * criticalDefenseMod;

                    if (criticalDefenseChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                        criticalDefended = true;
                }

                if (!criticalDefended)
                {
                    IsCritical = true;

                    CriticalDamageMod = 1.0f + WorldObject.GetWeaponCritDamageMod(attacker, attackSkill, defender);

                    // recklessness excluded from crits
                    RecklessnessMod = 1.0f;
                    DamageRatingMod = Creature.AdditiveCombine(DamageRatingBaseMod, SneakAttackMod, HeritageMod);
                    DamageBeforeMitigation = BaseDamageRange.Max * AttributeMod * PowerMod * SlayerMod * DamageRatingMod * CriticalDamageMod + ElementalDamageBonus;
                }
            }

            // get armor rending mod here?
            var armorRendingMod = 1.0f;
            if (Weapon != null && Weapon.HasImbuedEffect(ImbuedEffectType.ArmorRending))
                armorRendingMod = WorldObject.GetArmorRendingMod(attackSkill);

            // get body part / armor pieces / armor modifier
            if (playerDefender != null)
            {
                // select random body part @ current attack height
                GetBodyPart(AttackHeight);

                // get armor pieces
                Armor = attacker.GetArmorLayers(BodyPart);    // this uses attacker.AttackTarget

                // get armor modifiers
                ArmorMod = attacker.GetArmorMod(DamageType, Armor, DamageSource, armorRendingMod);
            }
            else
            {
                // select random body part @ current attack height
                GetBodyPart(AttackHeight, defender);
                if (Evaded)
                    return 0.0f;

                Armor = CreaturePart.GetArmorLayers((CombatBodyPart)BiotaPropertiesBodyPart.Key);

                // get target armor
                ArmorMod = CreaturePart.GetArmorMod(DamageType, Armor, DamageSource, armorRendingMod);
            }

            // get resistance modifiers
            WeaponResistanceMod = WorldObject.GetWeaponResistanceModifier(attacker, attackSkill, DamageType);

            if (playerDefender != null)
            {
                ResistanceMod = playerDefender.GetResistanceMod(DamageType, DamageSource, WeaponResistanceMod);
            }
            else
            {
                var resistanceType = Creature.GetResistanceType(DamageType);
                ResistanceMod = (float)defender.GetResistanceMod(resistanceType, DamageSource, WeaponResistanceMod);
            }

            // damage resistance rating
            DamageResistanceRatingMod = Creature.GetNegativeRatingMod(defender.EnchantmentManager.GetDamageResistRating());

            // get shield modifier
            ShieldMod = defender.GetShieldMod(attacker, DamageType);

            // calculate final output damage
            Damage = DamageBeforeMitigation * ArmorMod * ShieldMod * ResistanceMod * DamageResistanceRatingMod;
            DamageMitigated = DamageBeforeMitigation - Damage;

            HandleLogging(playerAttacker, playerDefender);

            return Damage;
        }

        /// <summary>
        /// Returns the chance for creature to avoid monster attack
        /// </summary>
        public float GetEvadeChance(Creature attacker, Creature defender)
        {
            AccuracyMod = attacker.GetAccuracyMod(Weapon);

            EffectiveAttackSkill = attacker.GetEffectiveAttackSkill();
            EffectiveDefenseSkill = defender.GetEffectiveDefenseSkill(attacker.CurrentAttack ?? CombatType.Melee);

            var evadeChance = 1.0f - SkillCheck.GetSkillChance(EffectiveAttackSkill, EffectiveDefenseSkill);
            return (float)evadeChance;
        }

        /// <summary>
        /// Returns the base damage for a player attacker
        /// </summary>
        public void GetBaseDamage(Player attacker, CombatManeuver maneuver)
        {
            // TODO: combat maneuvers for player?
            BaseDamageRange = attacker.GetBaseDamage();
            BaseDamage = ThreadSafeRandom.Next(BaseDamageRange.Min, BaseDamageRange.Max);

            DamageType = attacker.GetDamageType();

            if (DamageSource.ItemType == ItemType.MissileWeapon)
                DamageType = (DamageType)DamageSource.GetProperty(PropertyInt.DamageType);
            else
                DamageType = attacker.GetDamageType();
        }

        /// <summary>
        /// Returns the base damage for a non-player attacker
        /// </summary>
        public void GetBaseDamage(Creature attacker, CombatManeuver maneuver)
        {
            AttackPart = attacker.GetAttackPart(maneuver);
            if (AttackPart == null)
            {
                GeneralFailure = true;
                return;
            }

            BaseDamageRange = attacker.GetBaseDamage(AttackPart);
            BaseDamage = ThreadSafeRandom.Next(BaseDamageRange.Min, BaseDamageRange.Max);

            DamageType = attacker.GetDamageType(AttackPart);
        }

        /// <summary>
        /// Returns a body part for a player defender
        /// </summary>
        public void GetBodyPart(AttackHeight attackHeight)
        {
            // select random body part @ current attack height
            BodyPart = BodyParts.GetBodyPart(attackHeight);
        }

        /// <summary>
        /// Returns a body part for a creature defender
        /// </summary>
        public void GetBodyPart(AttackHeight attackHeight, Creature defender)
        {
            // select random body part @ current attack height
            BiotaPropertiesBodyPart = BodyParts.GetBodyPart(defender, attackHeight);

            if (BiotaPropertiesBodyPart == null)
            {
                Evaded = true;
                return;
            }
            CreaturePart = new Creature_BodyPart(defender, BiotaPropertiesBodyPart, IgnoreMagicArmor, IgnoreMagicResist);
        }

        public void ShowInfo()
        {
            // setup
            Console.WriteLine($"Attacker: {Attacker.Name} ({Attacker.Guid})");
            Console.WriteLine($"Defender: {Defender.Name} ({Defender.Guid})");

            Console.WriteLine($"CombatType: {CombatType}");

            Console.WriteLine($"DamageSource: {DamageSource.Name} ({DamageSource.Guid})");
            Console.WriteLine($"DamageType: {DamageType}");

            var weaponName = Weapon != null ? $"{Weapon.Name} ({Weapon.Guid})" : "None";
            Console.WriteLine($"Weapon: {weaponName}");

            Console.WriteLine($"AttackType: {AttackType}");
            Console.WriteLine($"AttackHeight: {AttackHeight}");

            // lifestone protection
            Console.WriteLine($"LifestoneProtection: {LifestoneProtection}");

            // evade
            Console.WriteLine($"AccuracyMod: {AccuracyMod}");
            Console.WriteLine($"EffectiveAttackSkill: {EffectiveAttackSkill}");
            Console.WriteLine($"EffectiveDefenseSkill: {EffectiveDefenseSkill}");
            Console.WriteLine($"EvasionChance: {EvasionChance}");
            Console.WriteLine($"Evaded: {Evaded}");

            if (!(Attacker is Player))
            {
                Console.WriteLine($"CombatManeuver: {CombatManeuver.Style} - {CombatManeuver.Motion}");
                Console.WriteLine($"AttackPart: {(CombatBodyPart)AttackPart.Key}");
            }

            // base damage
            Console.WriteLine($"BaseDamageRange: {BaseDamageRange}");
            Console.WriteLine($"BaseDamage: {BaseDamage}");

            // damage modifiers
            Console.WriteLine($"AttributeMod: {AttributeMod}");
            Console.WriteLine($"PowerMod: {PowerMod}");
            Console.WriteLine($"SlayerMod: {SlayerMod}");
            Console.WriteLine($"ElementalDamageBonus: {ElementalDamageBonus}");

            // damage ratings
            if (!(Defender is Player))
                Console.WriteLine($"DamageRatingBaseMod: {DamageRatingBaseMod}");

            Console.WriteLine($"HeritageMod: {HeritageMod}");
            Console.WriteLine($"RecklessnessMod: {RecklessnessMod}");
            Console.WriteLine($"SneakAttackMod: {SneakAttackMod}");
            Console.WriteLine($"DamageRatingMod: {DamageRatingMod}");

            // critical hit
            Console.WriteLine($"CriticalChance: {CriticalChance}");
            Console.WriteLine($"CriticalHit: {IsCritical}");
            Console.WriteLine($"CriticalDamageMod: {CriticalDamageMod}");

            if (BodyPart != 0)
            {
                // player body part
                Console.WriteLine($"BodyPart: {BodyPart}");
            }
            if (Armor.Count > 0)
            {
                Console.WriteLine($"Armors: {string.Join(", ", Armor.Select(i => i.Name))}");
            }

            if (CreaturePart != null)
            {
                // creature body part
                Console.WriteLine($"BodyPart: {(CombatBodyPart)BiotaPropertiesBodyPart.Key}");
                Console.WriteLine($"BaseArmorMod: {CreaturePart.BaseArmorMod}");
            }

            // damage mitigation
            Console.WriteLine($"ArmorMod: {ArmorMod}");
            Console.WriteLine($"ResistanceMod: {ResistanceMod}");
            Console.WriteLine($"ShieldMod: {ShieldMod}");
            Console.WriteLine($"WeaponResistanceMod: {WeaponResistanceMod}");

            Console.WriteLine($"DamageResistanceRatingMod: {DamageResistanceRatingMod}");

            if (IgnoreMagicArmor)
                Console.WriteLine($"IgnoreMagicArmor: {IgnoreMagicArmor}");
            if (IgnoreMagicResist)
                Console.WriteLine($"IgnoreMagicResist: {IgnoreMagicResist}");

            // final damage
            Console.WriteLine($"DamageBeforeMitigation: {DamageBeforeMitigation}");
            Console.WriteLine($"DamageMitigated: {DamageMitigated}");
            Console.WriteLine($"Damage: {Damage}");

            Console.WriteLine("----");
        }

        public void HandleLogging(Player attacker, Player defender)
        {
            if (attacker != null && (attacker.DebugDamage & Player.DebugDamageType.Attacker) != 0)
            {
                ShowInfo();
                return;
            }
            if (defender != null && (defender.DebugDamage & Player.DebugDamageType.Defender) != 0)
            {
                ShowInfo();
                return;
            }
        }
    }
}
