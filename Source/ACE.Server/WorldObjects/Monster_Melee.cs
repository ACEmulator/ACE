using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Physics.Animation;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// The delay between melee attacks (todo: find actual value)
        /// </summary>
        public static readonly float MeleeDelay = 1.5f;
        public static readonly float MeleeDelayMin = 0.5f;
        public static readonly float MeleeDelayMax = 2.0f;

        /// <summary>
        /// Returns TRUE if creature can perform a melee attack
        /// </summary>
        public bool MeleeReady()
        {
            return IsMeleeRange() && Timers.RunningTime >= NextAttackTime;
        }

        /// <summary>
        /// Performs a melee attack for the monster
        /// </summary>
        /// <returns>The length in seconds for the attack animation</returns>
        public float MeleeAttack()
        {
            var target = AttackTarget as Creature;

            if (target == null || !target.IsAlive)
            {
                Sleep();
                return 0.0f;
            }

            // choose a random combat maneuver
            var maneuver = GetCombatManeuver();
            if (maneuver == null)
            {
                Console.WriteLine($"Combat maneuver null! Stance {CurrentMotionState.Stance}, MotionTable {MotionTableId:X8}");
                return 0.0f;
            }

            AttackHeight = maneuver.AttackHeight;

            // select random body part @ current attack height
            var bodyPart = BodyParts.GetBodyPart(AttackHeight.Value);

            DoSwingMotion(AttackTarget, maneuver, out float animLength);
            PhysicsObj.stick_to_object(AttackTarget.PhysicsObj.ID);

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(animLength / 3.0f);     // TODO: get attack frame?
            actionChain.AddAction(this, () =>
            {
                if (AttackTarget == null) return;

                var critical = false;
                var damageType = DamageType.Undef;
                var shieldMod = 1.0f;
                var damage = CalculateDamage(ref damageType, maneuver, bodyPart, ref critical, ref shieldMod);

                var player = AttackTarget as Player;

                if (damage > 0.0f)
                {
                    player.TakeDamage(this, damageType, damage, bodyPart, critical);

                    if (shieldMod != 1.0f)
                    {
                        var shieldSkill = player.GetCreatureSkill(Skill.Shield);
                        Proficiency.OnSuccessUse(player, shieldSkill, shieldSkill.Current); // ?
                    }
                }
                else
                    player.OnEvade(this, AttackType.Melee);
            });
            actionChain.EnqueueChain();

            // TODO: figure out exact speed / delay formula
            var meleeDelay = Physics.Common.Random.RollDice(MeleeDelayMin, MeleeDelayMax);
            NextAttackTime = Timers.RunningTime + animLength + meleeDelay;;
            return animLength;
        }

        /// <summary>
        /// Selects a random combat maneuver for a monster's next attack
        /// </summary>
        public CombatManeuver GetCombatManeuver()
        {
            //ShowCombatTable();

            // for some reason, the combat maneuvers table can return stance motions that don't exist in the motion table
            // ie. skeletons (combat maneuvers table 0x30000000, motion table 0x09000025)
            // for sword combat, they have double and triple strikes (dagger / two-handed only?)

            var stanceManeuvers = CombatTable.CMT.Where(m => m.Style == (MotionCommand)CurrentMotionState.Stance).ToList();

            if (stanceManeuvers.Count == 0)
                return null;

            var motionTable = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.MotionTable>(MotionTableId);
            if (motionTable == null)
                return null;

            var stanceKey = (uint)CurrentMotionState.Stance << 16 | ((uint)MotionCommand.Ready & 0xFFFFF);
            motionTable.Links.TryGetValue(stanceKey, out var motions);
            if (motions == null)
                return null;

            while (true)    // limiter?
            {
                var rng = Physics.Common.Random.RollDice(0, stanceManeuvers.Count - 1);
                //Console.WriteLine("Selecting combat maneuver #" + rng);

                var combatManeuver = stanceManeuvers[rng];

                // ensure combat maneuver exists for this monster's motion table
                motions.TryGetValue((uint)combatManeuver.Motion, out var motionData);
                if (motionData != null)
                    return combatManeuver;
            };
        }

        /// <summary>
        /// Shows debug info for this monster's combat maneuvers table
        /// </summary>
        public void ShowCombatTable()
        {
            Console.WriteLine($"CombatManeuverTable ID: {CombatTable.Id:X8}");
            for (var i = 0; i < CombatTable.CMT.Count; i++)
            {
                var maneuver = CombatTable.CMT[i];
                Console.WriteLine($"{i} - {maneuver.Style} - {maneuver.Motion} - {maneuver.AttackHeight}");
            }
        }

        /// <summary>
        /// Perform the melee attack swing animation
        /// </summary>
        public void DoSwingMotion(WorldObject target, CombatManeuver maneuver, out float animLength)
        {
            var animSpeed = GetAnimSpeed();
            animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, maneuver.Motion, animSpeed);

            var motion = new Motion(this, maneuver.Motion, animSpeed);
            motion.MotionState.TurnSpeed = 2.25f;
            motion.MotionFlags |= MotionFlags.StickToObject;
            motion.TargetGuid = target.Guid;
            CurrentMotionState = motion;

            EnqueueBroadcastMotion(motion);
        }

        /// <summary>
        /// Returns the current melee swing motion for the monster
        /// </summary>
        public virtual MotionCommand GetSwingAnimation()
        {
            MotionCommand motion = new MotionCommand();

            //Console.WriteLine("MotionStance: " + CurrentMotionState.Stance);

            switch (CurrentMotionState.Stance)
            {
                case MotionStance.DualWieldCombat:
                case MotionStance.SwordCombat:
                case MotionStance.SwordShieldCombat:
                case MotionStance.ThrownShieldCombat:
                case MotionStance.ThrownWeaponCombat:
                case MotionStance.TwoHandedStaffCombat:
                case MotionStance.TwoHandedSwordCombat:
                    {
                        Enum.TryParse("Slash" + GetAttackHeight(), out motion);
                        return motion;
                    }

                case MotionStance.HandCombat:
                default:
                    {
                        Enum.TryParse("Attack" + GetAttackHeight() + (int)GetPowerRange(), out motion);
                        return motion;
                    }
            }
        }

        /// <summary>
        /// Returns base damage range for next monster attack
        /// </summary>
        public Range GetBaseDamage(BiotaPropertiesBodyPart attackPart)
        {
            if (CurrentAttack == AttackType.Missile && GetMissileAmmo() != null)
                return GetMissileDamage();

            // use weapon damage for every attack?
            var weapon = GetEquippedMeleeWeapon();
            if (weapon != null)
            {
                var weaponDamage = weapon.GetBaseDamage();
                //Console.WriteLine($"{Name} using weapon damage: {weaponDamage}");
                return weaponDamage;
            }

            var maxDamage = attackPart.DVal;
            var variance = attackPart.DVar;
            var minDamage = maxDamage - maxDamage * variance;

            var baseDamage = new Range(minDamage, maxDamage);
            //Console.WriteLine($"{Name} using base damage: {baseDamage}");
            return baseDamage;
        }

        /// <summary>
        /// Returns the chance for player to avoid monster attack
        /// </summary>
        public float GetEvadeChance()
        {
            // get monster attack skill
            var player = AttackTarget as Player;
            var attackSkill = GetCreatureSkill(GetCurrentAttackSkill()).Current;
            var offenseMod = GetWeaponOffenseModifier(this);
            attackSkill = (uint)Math.Round(attackSkill * offenseMod);

            if (IsExhausted)
                attackSkill = GetExhaustedSkill(attackSkill);

            // get player defense skill
            var defenseSkill = CurrentAttack == AttackType.Missile ? Skill.MissileDefense : Skill.MeleeDefense;
            var defenseMod = defenseSkill == Skill.MeleeDefense ? GetWeaponMeleeDefenseModifier(AttackTarget as Creature) : 1.0f;
            var difficulty = (uint)Math.Round(player.GetCreatureSkill(defenseSkill).Current * defenseMod);

            if (player.IsExhausted) difficulty = 0;

            /*var baseStr = offenseMod != 1.0f ? $" (base: {GetCreatureSkill(GetCurrentAttackSkill()).Current})" : "";
            Console.WriteLine("Attack skill: " + attackSkill + baseStr);

            baseStr = defenseMod != 1.0f ? $" (base: {player.GetCreatureSkill(defenseSkill).Current})" : "";
            Console.WriteLine("Defense skill: " + difficulty + baseStr);*/

            var evadeChance = 1.0f - SkillCheck.GetSkillChance((int)attackSkill, (int)difficulty);
            return (float)evadeChance;
        }

        /// <summary>
        /// Calculates the player damage for a physical monster attack
        /// </summary>
        /// <param name="bodyPart">The player body part the monster is targeting</param>
        /// <param name="criticalHit">Is TRUE if monster rolls a critical hit</param>
        public float CalculateDamage(ref DamageType damageType, CombatManeuver maneuver, BodyPart bodyPart, ref bool criticalHit, ref float shieldMod)
        {
            // evasion chance
            var evadeChance = GetEvadeChance();
            if (Physics.Common.Random.RollDice(0.0f, 1.0f) < evadeChance)
                return 0.0f;

            // get base damage
            var attackPart = GetAttackPart(maneuver);
            damageType = GetDamageType(attackPart);
            var damageRange = GetBaseDamage(attackPart);
            var baseDamage = Physics.Common.Random.RollDice(damageRange.Min, damageRange.Max);

            var damageRatingMod = GetRatingMod(EnchantmentManager.GetDamageRating());

            var player = AttackTarget as Player;
            var recklessnessMod = player != null ? player.GetRecklessnessMod() : 1.0f;

            // monster weapon / attributes
            var weapon = GetEquippedWeapon();

            // critical hit
            var critical = 0.1f;
            if (Physics.Common.Random.RollDice(0.0f, 1.0f) < critical)
                criticalHit = true;

            // attribute damage modifier (verify)
            var attributeMod = GetAttributeMod(AttackType.Melee);

            // get armor piece
            var armor = GetArmor(bodyPart);

            // get armor modifiers
            var armorMod = GetArmorMod(armor, damageType);

            // get resistance modifiers (protect/vuln)
            var resistanceMod = AttackTarget.EnchantmentManager.GetResistanceMod(damageType);

            var damageResistRatingMod = GetNegativeRatingMod(AttackTarget.EnchantmentManager.GetDamageResistRating());

            // get shield modifier
            var attackTarget = AttackTarget as Creature;
            shieldMod = attackTarget.GetShieldMod(this, damageType);

            // scale damage by modifiers
            var damage = baseDamage * damageRatingMod * attributeMod * armorMod * shieldMod * resistanceMod * damageResistRatingMod;

            if (!criticalHit)
                damage *= recklessnessMod;
            else
                damage *= 2;    // fixme: target recklessness mod still in effect?

            return damage;
        }

        /// <summary>
        /// Returns the player armor for a body part
        /// </summary>
        public List<WorldObject> GetArmor(BodyPart bodyPart)
        {
            var player = AttackTarget as Player;

            //Console.WriteLine("BodyPart: " + bodyPart);
            //Console.WriteLine("===");

            var bodyLocation = BodyParts.GetFlags(BodyParts.GetEquipMask(bodyPart));

            var equipped = player.EquippedObjects.Values.Where(e => e is Clothing && BodyParts.HasAny(e.CurrentWieldedLocation, bodyLocation)).ToList();

            return equipped;
        }

        /// <summary>
        /// Returns the percent of damage absorbed by layered armor + clothing
        /// </summary>
        /// <param name="armors">The list of armor/clothing covering the targeted body part</param>
        public float GetArmorMod(List<WorldObject> armors, DamageType damageType)
        {
            var effectiveAL = 0.0f;

            foreach (var armor in armors)
                effectiveAL += GetArmorMod(armor, damageType);

            // life spells
            // additive: armor/imperil
            var bodyArmorMod = AttackTarget.EnchantmentManager.GetBodyArmorMod();
            //Console.WriteLine("==");
            //Console.WriteLine("Armor Self: " + bodyArmorMod);
            effectiveAL += bodyArmorMod;

            var armorMod = SkillFormula.CalcArmorMod(effectiveAL);

            //Console.WriteLine("Total AL: " + effectiveAL);
            //Console.WriteLine("Armor mod: " + armorMod);

            return armorMod;
        }

        /// <summary>
        /// Returns the effective AL for 1 piece of armor/clothing
        /// </summary>
        /// <param name="armor">A piece of armor or clothing</param>
        public float GetArmorMod(WorldObject armor, DamageType damageType)
        {
            // get base armor/resistance level
            var baseArmor = armor.GetProperty(PropertyInt.ArmorLevel) ?? 0;
            var armorType = armor.GetProperty(PropertyInt.ArmorType) ?? 0;
            var resistance = GetResistance(armor, damageType);

            /*Console.WriteLine(armor.Name);
            Console.WriteLine("--");
            Console.WriteLine("Base AL: " + baseArmor);
            Console.WriteLine("Base RL: " + resistance);*/

            // armor level additives
            var player = AttackTarget as Player;
            var armorMod = armor.EnchantmentManager.GetArmorMod();
            // Console.WriteLine("Impen: " + armorMod);
            var effectiveAL = baseArmor + armorMod;

            // resistance additives
            var armorBane = armor.EnchantmentManager.GetArmorModVsType(damageType);
            // Console.WriteLine("Bane: " + armorBane);
            var effectiveRL = (float)(resistance + armorBane);

            // resistance cap
            if (effectiveRL > 2.0f)
                effectiveRL = 2.0f;

            /*Console.WriteLine("Effective AL: " + effectiveAL);
            Console.WriteLine("Effective RL: " + effectiveRL);
            Console.WriteLine();*/

            return effectiveAL * effectiveRL;
        }

        /// <summary>
        /// Returns the natural resistance to DamageType for a piece of armor
        /// </summary>
        public double GetResistance(WorldObject armor, DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Slash:
                    return armor.GetProperty(PropertyFloat.ArmorModVsSlash) ?? 0.0;
                case DamageType.Pierce:
                    return armor.GetProperty(PropertyFloat.ArmorModVsPierce) ?? 0.0;
                case DamageType.Bludgeon:
                    return armor.GetProperty(PropertyFloat.ArmorModVsBludgeon) ?? 0.0;
                case DamageType.Fire:
                    return armor.GetProperty(PropertyFloat.ArmorModVsFire) ?? 0.0;
                case DamageType.Cold:
                    return armor.GetProperty(PropertyFloat.ArmorModVsCold) ?? 0.0;
                case DamageType.Acid:
                    return armor.GetProperty(PropertyFloat.ArmorModVsAcid) ?? 0.0;
                case DamageType.Electric:
                    return armor.GetProperty(PropertyFloat.ArmorModVsElectric) ?? 0.0;
                case DamageType.Nether:
                    return armor.GetProperty(PropertyFloat.ArmorModVsNether) ?? 0.0;
                default:
                    return 0.0;
            }
        }

        /// <summary>
        /// Displays all of the natural resistances for a piece of armor
        /// </summary>
        public void ShowResistance(WorldObject armor)
        {
            Console.WriteLine("Resistance:");
            Console.WriteLine("Slashing: " + armor.GetProperty(PropertyFloat.ArmorModVsSlash));
            Console.WriteLine("Piercing: " + armor.GetProperty(PropertyFloat.ArmorModVsPierce));
            Console.WriteLine("Bludgeoning: " + armor.GetProperty(PropertyFloat.ArmorModVsBludgeon));
            Console.WriteLine("Fire: " + armor.GetProperty(PropertyFloat.ArmorModVsFire));
            Console.WriteLine("Ice: " + armor.GetProperty(PropertyFloat.ArmorModVsCold));
            Console.WriteLine("Acid: " + armor.GetProperty(PropertyFloat.ArmorModVsAcid));
            Console.WriteLine("Lightning: " + armor.GetProperty(PropertyFloat.ArmorModVsElectric));
            Console.WriteLine("Nether: " + armor.GetProperty(PropertyFloat.ArmorModVsNether));
        }

        /// <summary>
        /// Returns the power range for the current melee attack
        /// </summary>
        public virtual PowerAccuracy GetPowerRange()
        {
            return PowerAccuracy.Low; // always low for monsters?
        }

        /// <summary>
        /// Returns the monster body part performing the next attack
        /// </summary>
        public BiotaPropertiesBodyPart GetAttackPart(CombatManeuver maneuver)
        {
            List<BiotaPropertiesBodyPart> parts = null;
            var attackHeight = (uint)AttackHeight;
            if (maneuver != null)
            {
                var motionName = ((MotionCommand)maneuver.Motion).ToString();
                if (motionName.Contains("Special"))
                    parts = Biota.BiotaPropertiesBodyPart.Where(b => b.DVal != 0 && b.BH == 0).ToList();
            }
            if (parts == null)
                parts = Biota.BiotaPropertiesBodyPart.Where(b => b.DVal != 0 && b.BH != 0).ToList();

            var part = parts[Physics.Common.Random.RollDice(0, parts.Count - 1)];

            return part;
        }
    }
}
