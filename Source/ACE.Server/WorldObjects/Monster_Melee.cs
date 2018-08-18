using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Physics.Animation;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// The delay between melee attacks (todo: find actual value)
        /// </summary>
        public static readonly float MeleeDelay = 2.0f;

        /// <summary>
        /// Returns TRUE if creature can perform a melee attack
        /// </summary>
        public bool MeleeReady()
        {
            return IsMeleeRange() && Timer.CurrentTime >= NextAttackTime;
        }

        /// <summary>
        /// Performs a melee attack for the monster
        /// </summary>
        /// <returns>The length in seconds for the attack animation</returns>
        public float MeleeAttack()
        {
            var player = AttackTarget as Player;
            if (player.Health.Current <= 0) return 0.0f;

            // select random body part @ current attack height
            var bodyPart = GetBodyPart();

            DoSwingMotion(AttackTarget, out float animLength);
            PhysicsObj.stick_to_object(AttackTarget.PhysicsObj.ID);

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(animLength / 2.0f);
            actionChain.AddAction(this, () =>
            {
                if (AttackTarget == null) return;

                var critical = false;
                var damageType = DamageType.Undef;
                var damage = CalculateDamage(ref damageType, bodyPart, ref critical);

                if (damage > 0.0f)
                    player.TakeDamage(this, damageType, damage, bodyPart, critical);
                else
                    player.OnEvade(this, AttackType.Melee);
            });
            actionChain.EnqueueChain();

            // TODO: figure out exact speed / delay formula
            NextAttackTime = Timer.CurrentTime + animLength + MeleeDelay;
            return animLength;
        }

        /// <summary>
        /// Perform the melee attack swing animation
        /// </summary>
        public virtual ActionChain DoSwingMotion(WorldObject target, out float animLength)
        {
            var swingAnimation = new MotionItem(GetSwingAnimation(), 1.25f);
            animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, swingAnimation);

            var motion = new UniversalMotion(CurrentMotionState.Stance, swingAnimation);
            motion.MovementData.CurrentStyle = (uint)CurrentMotionState.Stance;
            motion.MovementData.TurnSpeed = 2.25f;
            motion.HasTarget = true;
            motion.TargetGuid = target.Guid;
            CurrentMotionState = motion;

            if (CurrentLandblock != null)
                CurrentLandblock?.EnqueueBroadcastMotion(this, motion);

            return null;
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
                case MotionStance.DualWieldAttack:
                case MotionStance.MeleeNoShieldAttack:
                case MotionStance.MeleeShieldAttack:
                case MotionStance.ThrownShieldCombat:
                case MotionStance.ThrownWeaponAttack:
                case MotionStance.TwoHandedStaffAttack:
                case MotionStance.TwoHandedSwordAttack:
                    {
                        Enum.TryParse("Slash" + GetAttackHeight(), out motion);
                        return motion;
                    }

                case MotionStance.UaNoShieldAttack:
                default:
                    {
                        Enum.TryParse("Attack" + GetAttackHeight() + (int)GetPowerRange(), out motion);
                        return motion;
                    }
            }
        }

        /// <summary>
        /// Returns base damage range for monster body part
        /// </summary>
        public Range GetBaseDamage(BiotaPropertiesBodyPart attackPart)
        {
            if (CurrentAttack == AttackType.Missile)
                return GetMissileDamage();

            var maxDamage = attackPart.DVal;
            var variance = attackPart.DVar;
            var minDamage = maxDamage - maxDamage * variance;
            //Console.WriteLine(string.Format("Base damage: {0}-{1}", minDamage, maxDamage));
            return new Range(minDamage, maxDamage);
        }

        public Skill GetCurrentAttackSkill()
        {
            var weapon = GetEquippedWeapon();
            if (weapon == null) return Skill.UnarmedCombat;

            var combatStyle = weapon.DefaultCombatStyle;
            switch (combatStyle)
            {
                case CombatStyle.Bow: return Skill.Bow;
                case CombatStyle.Crossbow: return Skill.Crossbow;

                // TODO: weapon skills
                default: return Skill.UnarmedCombat;
            }
        }

        /// <summary>
        /// Returns the chance for player to avoid monster attack
        /// </summary>
        public float GetEvadeChance()
        {
            // get monster attack skill
            var attackSkill = GetCreatureSkill(GetCurrentAttackSkill()).Current;

            if (IsExhausted)
                attackSkill = GetExhaustedSkill(attackSkill);

            // get player defense skill
            var player = AttackTarget as Player;

            var defenseSkill = CurrentAttack == AttackType.Missile ? Skill.MissileDefense : Skill.MeleeDefense;
            var difficulty = player.GetCreatureSkill(defenseSkill).Current;

            if (player.IsExhausted) difficulty = 0;

            //Console.WriteLine("Attack skill: " + attackSkill);
            //Console.WriteLine("Defense skill: " + difficulty);

            var evadeChance = 1.0f - SkillCheck.GetSkillChance((int)attackSkill, (int)difficulty);
            return (float)evadeChance;
        }

        /// <summary>
        /// Calculates the player damage for a physical monster attack
        /// </summary>
        /// <param name="bodyPart">The player body part the monster is targeting</param>
        /// <param name="criticalHit">Is TRUE if monster rolls a critical hit</param>
        public float CalculateDamage(ref DamageType damageType, BodyPart bodyPart, ref bool criticalHit)
        {
            // evasion chance
            var evadeChance = GetEvadeChance();
            if (Physics.Common.Random.RollDice(0.0f, 1.0f) < evadeChance)
                return 0.0f;

            // get base damage
            var attackPart = GetAttackPart();
            damageType = GetDamageType(attackPart);
            var damageRange = GetBaseDamage(attackPart);
            var baseDamage = Physics.Common.Random.RollDice(damageRange.Min, damageRange.Max);

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

            // get shield modifier
            var attackTarget = AttackTarget as Creature;
            var shieldMod = attackTarget.GetShieldMod(this, damageType);

            // scale damage by modifiers
            var damage = baseDamage * attributeMod * armorMod * shieldMod * resistanceMod;

            if (criticalHit) damage *= 2;

            return damage;
        }

        /// <summary>
        /// Selects a random body part at current attack height
        /// </summary>
        public BodyPart GetBodyPart()
        {
            if (AttackHeight == null) GetAttackHeight();

            return BodyParts.GetBodyPart(AttackHeight.Value);
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
        /// Returns the attack height for the current monster
        /// </summary>
        public virtual string GetAttackHeight()
        {
            // if not attack height has been selected
            //if (AttackHeight == null)
            AttackHeight = ((AttackHeight)Physics.Common.Random.RollDice(1, 3));

            return AttackHeight.Value.GetString();
        }

        /// <summary>
        /// Returns the power range for the current melee attack
        /// </summary>
        public virtual PowerAccuracy GetPowerRange()
        {
            return PowerAccuracy.Low; // always low for monsters?
        }

        /// <summary>
        /// Returns a random attackable body part
        /// </summary>
        public BiotaPropertiesBodyPart GetAttackPart()
        {
            var parts = GetAttackParts();
            var part = parts[Physics.Common.Random.RollDice(0, parts.Count - 1)];
            return part;
        }

        /// <summary>
        /// Returns the body parts which have damage values
        /// </summary>
        public List<BiotaPropertiesBodyPart> GetAttackParts()
        {
            return Biota.BiotaPropertiesBodyPart.Where(b => b.DVal != 0).ToList();
        }
    }
}
