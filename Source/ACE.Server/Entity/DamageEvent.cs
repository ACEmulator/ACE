using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class DamageEvent
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        public MotionCommand? AttackMotion;
        public KeyValuePair<CombatBodyPart, PropertiesBodyPart> AttackPart;      // the body part this monster is attacking with

        // creature defender
        public Quadrant Quadrant;

        public bool IgnoreMagicArmor =>  (Weapon?.IgnoreMagicArmor ?? false) || (Attacker?.IgnoreMagicArmor ?? false);      // ignores impen / banes

        public bool IgnoreMagicResist => (Weapon?.IgnoreMagicResist ?? false) || (Attacker?.IgnoreMagicResist ?? false);    // ignores life armor / prots

        public bool Overpower;


        // player defender
        public BodyPart BodyPart;
        public List<WorldObject> Armor;

        // creature defender
        public KeyValuePair<CombatBodyPart, PropertiesBodyPart> PropertiesBodyPart;
        public Creature_BodyPart CreaturePart;

        public float Damage;

        public bool GeneralFailure;

        public bool HasDamage => !Evaded && !LifestoneProtection;

        public bool CriticalDefended;

        public static HashSet<uint> AllowDamageTypeUndef = new HashSet<uint>()
        {
            22545   // Obsidian Spines
        };

        public static DamageEvent CalculateDamage(Creature attacker, Creature defender, WorldObject damageSource, MotionCommand? attackMotion = null)
        {
            var damageEvent = new DamageEvent();
            damageEvent.AttackMotion = attackMotion;
            if (damageSource == null)
                damageSource = attacker;

            var damage = damageEvent.DoCalculateDamage(attacker, defender, damageSource);

            damageEvent.HandleLogging(attacker, defender);

            return damageEvent;
        }

        private float DoCalculateDamage(Creature attacker, Creature defender, WorldObject damageSource)
        {
            var playerAttacker = attacker as Player;
            var playerDefender = defender as Player;

            Attacker = attacker;
            Defender = defender;

            CombatType = damageSource.ProjectileSource == null ? attacker.GetCombatType() : CombatType.Missile;

            DamageSource = damageSource;

            Weapon = damageSource.ProjectileSource == null ? attacker.GetEquippedWeapon() : damageSource.ProjectileLauncher;

            AttackType = attacker.AttackType;
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

            // overpower
            if (attacker.Overpower != null)
                Overpower = Creature.GetOverpower(attacker, defender);

            // evasion chance
            if (!Overpower)
            {
                EvasionChance = GetEvadeChance(attacker, defender);
                if (EvasionChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                {
                    Evaded = true;
                    return 0.0f;
                }
            }

            // get base damage
            if (playerAttacker != null)
                GetBaseDamage(playerAttacker);
            else
                GetBaseDamage(attacker, AttackMotion ?? MotionCommand.Invalid);

            if (DamageType == DamageType.Undef && !AllowDamageTypeUndef.Contains(damageSource.WeenieClassId))
            {
                log.Error($"DamageEvent.DoCalculateDamage({attacker?.Name} ({attacker?.Guid}), {defender?.Name} ({defender?.Guid}), {damageSource?.Name} ({damageSource?.Guid})) - DamageType == DamageType.Undef");
                GeneralFailure = true;
            }

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
            CriticalChance = WorldObject.GetWeaponCriticalChance(attacker, attackSkill, defender);
            if (CriticalChance > ThreadSafeRandom.Next(0.0f, 1.0f))
            {
                if (playerDefender != null && playerDefender.AugmentationCriticalDefense > 0)
                {
                    var criticalDefenseMod = playerAttacker != null ? 0.05f : 0.25f;
                    var criticalDefenseChance = playerDefender.AugmentationCriticalDefense * criticalDefenseMod;

                    if (criticalDefenseChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                        CriticalDefended = true;
                }

                if (!CriticalDefended)
                {
                    IsCritical = true;

                    CriticalDamageMod = 1.0f + WorldObject.GetWeaponCritDamageMod(attacker, attackSkill, defender);

                    // recklessness excluded from crits
                    RecklessnessMod = 1.0f;
                    DamageRatingMod = Creature.AdditiveCombine(DamageRatingBaseMod, SneakAttackMod, HeritageMod);
                    DamageBeforeMitigation = BaseDamageMod.MaxDamage * AttributeMod * PowerMod * SlayerMod * DamageRatingMod * CriticalDamageMod;
                }
            }

            // armor rending and cleaving
            var armorRendingMod = 1.0f;
            if (Weapon != null && Weapon.HasImbuedEffect(ImbuedEffectType.ArmorRending))
                armorRendingMod = WorldObject.GetArmorRendingMod(attackSkill);

            var armorCleavingMod = attacker.GetArmorCleavingMod(Weapon);

            var ignoreArmorMod = Math.Min(armorRendingMod, armorCleavingMod);

            // get body part / armor pieces / armor modifier
            if (playerDefender != null)
            {
                // select random body part @ current attack height
                GetBodyPart(AttackHeight);

                // get player armor pieces
                Armor = attacker.GetArmorLayers(playerDefender, BodyPart);

                // get armor modifiers
                ArmorMod = attacker.GetArmorMod(DamageType, Armor, Weapon, ignoreArmorMod);
            }
            else
            {
                // determine height quadrant
                Quadrant = GetQuadrant(Defender, Attacker, AttackHeight, DamageSource);

                // select random body part @ current attack height
                GetBodyPart(Defender, Quadrant);
                if (Evaded)
                    return 0.0f;

                Armor = CreaturePart.GetArmorLayers(PropertiesBodyPart.Key);

                // get target armor
                ArmorMod = CreaturePart.GetArmorMod(DamageType, Armor, Attacker, Weapon, ignoreArmorMod);
            }

            if (Weapon != null && Weapon.HasImbuedEffect(ImbuedEffectType.IgnoreAllArmor))
                ArmorMod = 1.0f;

            // get resistance modifiers
            WeaponResistanceMod = WorldObject.GetWeaponResistanceModifier(attacker, attackSkill, DamageType);

            if (playerDefender != null)
            {
                ResistanceMod = playerDefender.GetResistanceMod(DamageType, Attacker, Weapon, WeaponResistanceMod);
            }
            else
            {
                var resistanceType = Creature.GetResistanceType(DamageType);
                ResistanceMod = (float)Math.Max(0.0f, defender.GetResistanceMod(resistanceType, Attacker, Weapon, WeaponResistanceMod));
            }

            // damage resistance rating
            DamageResistanceRatingMod = Creature.GetNegativeRatingMod(defender.GetDamageResistRating(CombatType));

            // get shield modifier
            ShieldMod = defender.GetShieldMod(attacker, DamageType, Weapon);

            // calculate final output damage
            Damage = DamageBeforeMitigation * ArmorMod * ShieldMod * ResistanceMod * DamageResistanceRatingMod;
            DamageMitigated = DamageBeforeMitigation - Damage;

            return Damage;
        }

        public Quadrant GetQuadrant(Creature defender, Creature attacker, AttackHeight attackHeight, WorldObject damageSource)
        {
            var quadrant = attackHeight.ToQuadrant();

            var wo = damageSource.CurrentLandblock != null ? damageSource : attacker;

            quadrant |= wo.GetRelativeDir(defender);

            return quadrant;
        }

        /// <summary>
        /// Returns the chance for creature to avoid monster attack
        /// </summary>
        public float GetEvadeChance(Creature attacker, Creature defender)
        {
            AccuracyMod = attacker.GetAccuracyMod(Weapon);

            EffectiveAttackSkill = attacker.GetEffectiveAttackSkill();

            var attackType = attacker.GetCombatType();

            EffectiveDefenseSkill = defender.GetEffectiveDefenseSkill(attackType);

            var evadeChance = 1.0f - SkillCheck.GetSkillChance(EffectiveAttackSkill, EffectiveDefenseSkill);
            return (float)evadeChance;
        }

        /// <summary>
        /// Returns the base damage for a player attacker
        /// </summary>
        public void GetBaseDamage(Player attacker)
        {
            if (DamageSource.ItemType == ItemType.MissileWeapon)
            {
                DamageType = DamageSource.W_DamageType;

                // handle prismatic arrows
                if (DamageType == DamageType.Base)
                {
                    if (Weapon != null && Weapon.W_DamageType != DamageType.Undef)
                        DamageType = Weapon.W_DamageType;
                    else
                        DamageType = DamageType.Pierce;
                }
            }
            else
                DamageType = attacker.GetDamageType();

            // TODO: combat maneuvers for player?
            BaseDamageMod = attacker.GetBaseDamageMod(DamageSource);

            // some quest bows can have built-in damage bonus
            if (Weapon?.WeenieType == WeenieType.MissileLauncher)
                BaseDamageMod.DamageBonus += Weapon.Damage ?? 0;

            if (DamageSource.ItemType == ItemType.MissileWeapon)
                BaseDamageMod.ElementalBonus = WorldObject.GetMissileElementalDamageBonus(attacker, DamageType);

            BaseDamage = ThreadSafeRandom.Next(BaseDamageMod.MinDamage, BaseDamageMod.MaxDamage);
        }

        /// <summary>
        /// Returns the base damage for a non-player attacker
        /// </summary>
        public void GetBaseDamage(Creature attacker, MotionCommand motionCommand)
        {
            AttackPart = attacker.GetAttackPart(motionCommand);
            if (AttackPart.Value == null)
            {
                GeneralFailure = true;
                return;
            }

            BaseDamageMod = attacker.GetBaseDamage(AttackPart.Value);
            BaseDamage = ThreadSafeRandom.Next(BaseDamageMod.MinDamage, BaseDamageMod.MaxDamage);

            DamageType = attacker.GetDamageType(AttackPart.Value, CombatType);
        }

        /// <summary>
        /// Returns a body part for a player defender
        /// </summary>
        public void GetBodyPart(AttackHeight attackHeight)
        {
            // select random body part @ current attack height
            BodyPart = BodyParts.GetBodyPart(attackHeight);
        }

        public static readonly Quadrant LeftRight = Quadrant.Left | Quadrant.Right;
        public static readonly Quadrant FrontBack = Quadrant.Front | Quadrant.Back;

        /// <summary>
        /// Returns a body part for a creature defender
        /// </summary>
        public void GetBodyPart(Creature defender, Quadrant quadrant)
        {
            // get cached body parts table
            var bodyParts = Creature.GetBodyParts(defender.WeenieClassId);

            // rng roll for body part
            var bodyPart = bodyParts.RollBodyPart(quadrant);

            if (bodyPart == CombatBodyPart.Undefined)
            {
                log.Debug($"DamageEvent.GetBodyPart({defender?.Name} ({defender?.Guid}) ) - couldn't find body part for wcid {defender.WeenieClassId}, Quadrant {quadrant}");
                Evaded = true;
                return;
            }

            //Console.WriteLine($"AttackHeight: {AttackHeight}, Quadrant: {quadrant & FrontBack}{quadrant & LeftRight}, AttackPart: {bodyPart}");

            defender.Biota.PropertiesBodyPart.TryGetValue(bodyPart, out var value);
            PropertiesBodyPart = new KeyValuePair<CombatBodyPart, PropertiesBodyPart>(bodyPart, value);

            // select random body part @ current attack height
            /*BiotaPropertiesBodyPart = BodyParts.GetBodyPart(defender, attackHeight);

            if (BiotaPropertiesBodyPart == null)
            {
                Evaded = true;
                return;
            }*/

            CreaturePart = new Creature_BodyPart(defender, PropertiesBodyPart);
        }

        public void ShowInfo(Creature creature)
        {
            var targetInfo = PlayerManager.GetOnlinePlayer(creature.DebugDamageTarget);
            if (targetInfo == null)
            {
                creature.DebugDamage = Creature.DebugDamageType.None;
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

            if (Attacker.Overpower != null)
                info += $"Overpower: {Overpower} ({Creature.GetOverpowerChance(Attacker, Defender)})\n";

            info += $"EvasionChance: {EvasionChance}\n";
            info += $"Evaded: {Evaded}\n";

            if (!(Attacker is Player))
            {
                if (AttackMotion != null)
                    info += $"AttackMotion: {AttackMotion}\n";
                if (AttackPart.Value != null)
                    info += $"AttackPart: {AttackPart.Key}\n";
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
            info += $"CriticalDefended: {CriticalDefended}\n";
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
                info += $"BodyPart: {PropertiesBodyPart.Key}\n";
                info += $"BaseArmor: {CreaturePart.Biota.Value.BaseArmor}\n";
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

        public void HandleLogging(Creature attacker, Creature defender)
        {
            if (attacker != null && (attacker.DebugDamage & Creature.DebugDamageType.Attacker) != 0)
            {
                ShowInfo(attacker);
                return;
            }
            if (defender != null && (defender.DebugDamage & Creature.DebugDamageType.Defender) != 0)
            {
                ShowInfo(defender);
                return;
            }
        }

        public AttackConditions AttackConditions
        {
            get
            {
                var attackConditions = new AttackConditions();

                if (CriticalDefended)
                    attackConditions |= AttackConditions.CriticalProtectionAugmentation;
                if (RecklessnessMod > 1.0f)
                    attackConditions |= AttackConditions.Recklessness;
                if (SneakAttackMod > 1.0f)
                    attackConditions |= AttackConditions.SneakAttack;
                if (Overpower)
                    attackConditions |= AttackConditions.Overpower;

                return attackConditions;
            }
        }
    }
}
