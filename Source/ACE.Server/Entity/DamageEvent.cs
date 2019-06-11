using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
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

        public BaseDamageMod BaseDamageMod;
        public float BaseDamage { get; set; }

        public float AttributeMod;
        public float PowerMod;
        public float SlayerMod;

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

        public bool IgnoreMagicArmor  => GetWeaponProperty(PropertyBool.IgnoreMagicArmor);      // ignores impen / banes

        public bool IgnoreMagicResist => GetWeaponProperty(PropertyBool.IgnoreMagicResist);     // ignores life armor / prots


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

            damageEvent.HandleLogging(attacker as Player, defender as Player);

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

            if (defender.Invincible)
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

            // ratings
            DamageRatingBaseMod = Creature.GetPositiveRatingMod(attacker.GetDamageRating());
            RecklessnessMod = Creature.GetRecklessnessMod(attacker, defender);
            SneakAttackMod = attacker.GetSneakAttackMod(defender);
            HeritageMod = attacker.GetHeritageBonus(Weapon) ? 1.05f : 1.0f;

            DamageRatingMod = Creature.AdditiveCombine(DamageRatingBaseMod, RecklessnessMod, SneakAttackMod, HeritageMod);

            // damage before mitigation
            DamageBeforeMitigation = BaseDamage * AttributeMod * PowerMod * SlayerMod * DamageRatingMod;

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
                    DamageBeforeMitigation = BaseDamageMod.MaxDamage * AttributeMod * PowerMod * SlayerMod * DamageRatingMod * CriticalDamageMod;
                }
            }

            // get armor rending mod here?
            var armorRendingMod = 1.0f;
            if (Weapon != null && Weapon.HasImbuedEffect(ImbuedEffectType.ArmorRending))
                armorRendingMod = WorldObject.GetArmorRendingMod(attackSkill);

            var ignoreMagicResist = IgnoreMagicResist;
            var ignoreMagicArmor = IgnoreMagicArmor;

            // get body part / armor pieces / armor modifier
            if (playerDefender != null)
            {
                // select random body part @ current attack height
                GetBodyPart(AttackHeight);

                // get player armor pieces
                Armor = attacker.GetArmorLayers(playerDefender, BodyPart);

                // get armor modifiers
                ArmorMod = attacker.GetArmorMod(DamageType, Armor, ignoreMagicResist, ignoreMagicArmor, armorRendingMod);
            }
            else
            {
                // select random body part @ current attack height
                GetBodyPart(AttackHeight, defender);
                if (Evaded)
                    return 0.0f;

                Armor = CreaturePart.GetArmorLayers((CombatBodyPart)BiotaPropertiesBodyPart.Key);

                // get target armor
                ArmorMod = CreaturePart.GetArmorMod(DamageType, Armor, ignoreMagicResist, ignoreMagicArmor, armorRendingMod);
            }

            // get resistance modifiers
            WeaponResistanceMod = WorldObject.GetWeaponResistanceModifier(attacker, attackSkill, DamageType);

            if (playerDefender != null)
            {
                ResistanceMod = playerDefender.GetResistanceMod(DamageType, ignoreMagicResist, WeaponResistanceMod);
            }
            else
            {
                var resistanceType = Creature.GetResistanceType(DamageType);
                ResistanceMod = (float)defender.GetResistanceMod(resistanceType, ignoreMagicResist, WeaponResistanceMod);
            }

            // damage resistance rating
            DamageResistanceRatingMod = Creature.GetNegativeRatingMod(defender.GetDamageResistRating());

            // get shield modifier
            ShieldMod = defender.GetShieldMod(attacker, DamageType, ignoreMagicArmor);

            // calculate final output damage
            Damage = DamageBeforeMitigation * ArmorMod * ShieldMod * ResistanceMod * DamageResistanceRatingMod;
            DamageMitigated = DamageBeforeMitigation - Damage;

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
            if (DamageSource.ItemType == ItemType.MissileWeapon)
            {
                DamageType = (DamageType)DamageSource.GetProperty(PropertyInt.DamageType);

                // handle prismatic arrows
                if (DamageType == DamageType.Base)
                {
                    var weapon = attacker.GetEquippedWeapon();
                    if (weapon != null && (weapon.W_DamageType ?? 0) != 0)
                        DamageType = (DamageType)weapon.W_DamageType;
                    else
                        DamageType = DamageType.Pierce;
                }
            }
            else
                DamageType = attacker.GetDamageType();

            // TODO: combat maneuvers for player?
            BaseDamageMod = attacker.GetBaseDamageMod();

            if (DamageSource.ItemType == ItemType.MissileWeapon)
                BaseDamageMod.ElementalBonus = WorldObject.GetMissileElementalDamageModifier(attacker, DamageType);

            BaseDamage = ThreadSafeRandom.Next(BaseDamageMod.MinDamage, BaseDamageMod.MaxDamage);
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

            BaseDamageMod = attacker.GetBaseDamage(AttackPart);
            BaseDamage = ThreadSafeRandom.Next(BaseDamageMod.MinDamage, BaseDamageMod.MaxDamage);

            DamageType = attacker.GetDamageType(AttackPart);

            if (attacker is CombatPet combatPet)
                DamageType = combatPet.DamageType;
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
            CreaturePart = new Creature_BodyPart(defender, BiotaPropertiesBodyPart);
        }

        public void ShowInfo(Player player)
        {
            var targetInfo = PlayerManager.GetOnlinePlayer(player.DebugDamageTarget);
            if (targetInfo == null)
            {
                player.DebugDamage = Player.DebugDamageType.None;
                return;
            }

            // setup
            var info = $"Attacker: {Attacker.Name} ({Attacker.Guid})\n";
            info += $"Defender: {Defender.Name} ({Defender.Guid})\n";

            info += $"CombatType: {CombatType}\n";

            info += $"DamageSource: {DamageSource.Name} ({DamageSource.Guid})\n";
            info += $"DamageType: {DamageType}\n";

            var weaponName = Weapon != null ? $"{Weapon.Name} ({Weapon.Guid})" : "None\n";
            info += $"Weapon: {weaponName}\n";

            info += $"AttackType: {AttackType}\n";
            info += $"AttackHeight: {AttackHeight}\n";

            // lifestone protection
            info += $"LifestoneProtection: {LifestoneProtection}\n";

            // evade
            info += $"AccuracyMod: {AccuracyMod}\n";
            info += $"EffectiveAttackSkill: {EffectiveAttackSkill}\n";
            info += $"EffectiveDefenseSkill: {EffectiveDefenseSkill}\n";
            info += $"EvasionChance: {EvasionChance}\n";
            info += $"Evaded: {Evaded}\n";

            if (!(Attacker is Player))
            {
                info += $"CombatManeuver: {CombatManeuver.Style} - {CombatManeuver.Motion}\n";
                if (AttackPart != null)
                    info += $"AttackPart: {(CombatBodyPart)AttackPart.Key}\n";
            }

            // base damage
            if (BaseDamageMod != null)
                info += $"BaseDamageRange: {BaseDamageMod.Range}\n";


            info += $"BaseDamage: {BaseDamage}\n";

            // damage modifiers
            info += $"AttributeMod: {AttributeMod}\n";
            info += $"PowerMod: {PowerMod}\n";
            info += $"SlayerMod: {SlayerMod}\n";

            if (BaseDamageMod != null)
            {
                info += $"ElementalDamageBonus: {BaseDamageMod.ElementalBonus}\n";
                info += $"MissileWeaponModifier: {BaseDamageMod.DamageMod}\n";
                info += $"BloodDrinker/ThirstTotal: {BaseDamageMod.DamageBonus}\n";
            }

            // damage ratings
            if (!(Defender is Player))
                info += $"DamageRatingBaseMod: {DamageRatingBaseMod}\n";

            info += $"HeritageMod: {HeritageMod}\n";
            info += $"RecklessnessMod: {RecklessnessMod}\n";
            info += $"SneakAttackMod: {SneakAttackMod}\n";
            info += $"DamageRatingMod: {DamageRatingMod}\n";

            // critical hit
            info += $"CriticalChance: {CriticalChance}\n";
            info += $"CriticalHit: {IsCritical}\n";
            info += $"CriticalDamageMod: {CriticalDamageMod}\n";

            if (BodyPart != 0)
            {
                // player body part
                info += $"BodyPart: {BodyPart}\n";
            }
            if (Armor != null && Armor.Count > 0)
            {
                info += $"Armors: {string.Join(", ", Armor.Select(i => i.Name))}\n";
            }

            if (CreaturePart != null)
            {
                // creature body part
                info += $"BodyPart: {(CombatBodyPart)BiotaPropertiesBodyPart.Key}\n";
                info += $"BaseArmor: {CreaturePart.Biota.BaseArmor}\n";
            }

            // damage mitigation
            info += $"ArmorMod: {ArmorMod}\n";
            info += $"ResistanceMod: {ResistanceMod}\n";
            info += $"ShieldMod: {ShieldMod}\n";
            info += $"WeaponResistanceMod: {WeaponResistanceMod}\n";

            info += $"DamageResistanceRatingMod: {DamageResistanceRatingMod}\n";

            if (IgnoreMagicArmor)
                info += $"IgnoreMagicArmor: {IgnoreMagicArmor}\n";
            if (IgnoreMagicResist)
                info += $"IgnoreMagicResist: {IgnoreMagicResist}\n";

            // final damage
            info += $"DamageBeforeMitigation: {DamageBeforeMitigation}\n";
            info += $"DamageMitigated: {DamageMitigated}\n";
            info += $"Damage: {Damage}\n";

            info += "----";

            targetInfo.Session.Network.EnqueueSend(new GameMessageSystemChat(info, ChatMessageType.Broadcast));
        }

        public void HandleLogging(Player attacker, Player defender)
        {
            if (attacker != null && (attacker.DebugDamage & Player.DebugDamageType.Attacker) != 0)
            {
                ShowInfo(attacker);
                return;
            }
            if (defender != null && (defender.DebugDamage & Player.DebugDamageType.Defender) != 0)
            {
                ShowInfo(defender);
                return;
            }
        }

        public bool GetWeaponProperty(PropertyBool property)
        {
            if (DamageSource == null)
                return false;

            // melee weapons
            var result = DamageSource.GetProperty(property) ?? false;

            if (result || DamageSource.ItemType != ItemType.MissileWeapon)
                return result;

            // missile weapons
            var weapon = Attacker.GetEquippedWeapon();

            return weapon != null && (weapon.GetProperty(property) ?? false);
        }
    }
}
