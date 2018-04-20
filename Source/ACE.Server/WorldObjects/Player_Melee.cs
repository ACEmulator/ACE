using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Extensions;
using System;
using System.Linq;
using System.Numerics;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public AttackHeight AttackHeight;
        public float PowerLevel;

        public WorldObject MeleeTarget;

        public void HandleActionTargetedMeleeAttack(ObjectGuid guid, uint attackHeight, float powerLevel)
        {
            /*Console.WriteLine("HandleActionTargetedMeleeAttack");
            Console.WriteLine("Target ID: " + guid.Full.ToString("X8"));
            Console.WriteLine("Attack height: " + attackHeight);
            Console.WriteLine("Power level: " + powerLevel);*/

            // sanity check
            powerLevel = Math.Clamp(powerLevel, 0.0f, 1.0f);

            AttackHeight = (AttackHeight)attackHeight;
            PowerLevel = powerLevel;

            // get world object of target guid
            var target = CurrentLandblock.GetObject(guid);
            if (target == null)
            {
                log.Warn("Unknown target guid " + guid);
                return;
            }
            if (MeleeTarget == null)
                MeleeTarget = target;
            else
                return;

            // get distance from target
            var dist = GetDistance(target);

            // get angle to target
            var angle = GetAngle(target);

            //Console.WriteLine("Dist: " + dist);
            //Console.WriteLine("Angle: " + angle);

            // turn / moveto if required
            Rotate();
            MoveTo();

            // do melee attack
            Attack(target);
        }

        public void HandleActionCancelAttack()
        {
            MeleeTarget = null;
        }

        public void Attack(WorldObject target)
        {
            if (MeleeTarget == null)
                return;

            var creature = target as Creature;
            var actionChain = DoSwingMotion(target);

            if (creature.Health.Current <= 0)
                return;

            var critical = false;
            var damage = CalculateDamage(target, ref critical);
            if (damage > 0.0f)
                target.TakeDamage(this, damage, critical);
            else
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{target.Name} evaded your attack.", ChatMessageType.CombatEnemy));

            if (damage > 0.0f && creature.Health.Current > 0)
            {
                // notify attacker
                var intDamage = (uint)Math.Round(damage);
                Session.Network.EnqueueSend(new GameEventAttackerNotification(Session, target.Name, GetDamageType(), (float)intDamage / creature.Health.MaxValue, intDamage, critical, new AttackConditions()));

                // splatter effects
                Session.Network.EnqueueSend(new GameMessageSound(target.Guid, Sound.HitFlesh1, 0.5f));
                if (damage >= creature.Health.MaxValue * 0.25f)
                {
                    var painSound = (Sound)Enum.Parse(typeof(Sound), "Wound" + Physics.Common.Random.RollDice(1, 3), true);
                    Session.Network.EnqueueSend(new GameMessageSound(target.Guid, painSound, 1.0f));
                }
                var splatter = (PlayScript)Enum.Parse(typeof(PlayScript), "Splatter" + GetSplatterHeight() + GetSplatterDir(target));
                Session.Network.EnqueueSend(new GameMessageScript(target.Guid, splatter));
            }
            if (creature.Health.Current > 0 && GetCharacterOption(CharacterOption.AutoRepeatAttacks))
            { 
                // powerbar refill timing
                actionChain.AddDelaySeconds(PowerLevel);
                actionChain.AddAction(this, () => Attack(target));
            }
            else
                MeleeTarget = null;
                
            actionChain.EnqueueChain();
        }

        public ActionChain DoSwingMotion(WorldObject target)
        {
            var swingAnimation = new MotionItem(GetSwingAnimation(), 1.25f);
            var animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, swingAnimation);

            var motion = new UniversalMotion(CurrentMotionState.Stance, swingAnimation);
            motion.MovementData.CurrentStyle = (uint)CurrentMotionState.Stance;
            motion.MovementData.TurnSpeed = 2.25f;
            motion.HasTarget = true;
            motion.TargetGuid = target.Guid;
            CurrentMotionState = motion;

            var actionChain = new ActionChain();
            actionChain.AddAction(this, () => DoMotion(motion));
            actionChain.AddDelaySeconds(animLength);
            actionChain.AddAction(this, () => Session.Network.EnqueueSend(new GameEventAttackDone(Session)));
            actionChain.AddAction(this, () => Session.Network.EnqueueSend(new GameEventCombatCommmenceAttack(Session)));
            actionChain.AddAction(this, () => Session.Network.EnqueueSend(new GameEventAttackDone(Session)));
            return actionChain;
        }

        public MotionCommand GetSwingAnimation()
        {
            MotionCommand motion = new MotionCommand();

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
                        var action = PowerLevel < 0.33f ? "Thrust" : "Slash";
                        Enum.TryParse(action + GetAttackHeight(), out motion);
                        return motion;
                    }
                case MotionStance.UaNoShieldAttack:
                default:
                    {
                        // is the player holding a weapon?
                        var weapon = GetEquippedWeapon();

                        // no weapon: power range 1-3
                        // unarmed weapon: power range 1-2
                        if (weapon == null)
                            Enum.TryParse("Attack" + GetAttackHeight() + GetPowerRange(), out motion);
                        else
                            Enum.TryParse("Attack" + GetAttackHeight() + Math.Min(GetPowerRange(), 2), out motion);

                        return motion;
                    }
            }
        }

        public string GetAttackHeight()
        {
            return AttackHeight.GetString();
        }

        public string GetSplatterHeight()
        {
            switch (AttackHeight)
            {
                case AttackHeight.Low: return "Low";
                case AttackHeight.Medium: return "Mid";
                case AttackHeight.High: default:  return "Up";
            }
        }

        public int GetPowerRange()
        {
            if (PowerLevel < 0.33f)
                return 1;
            else if (PowerLevel < 0.66f)
                return 2;
            else
                return 3;
        }

        public Skill GetCurrentWeaponSkill()
        {
            // hack for converting pre-MoA skills
            var unarmed = GetCreatureSkill(Skill.UnarmedCombat);
            var light = GetCreatureSkill(Skill.LightWeapons);
            var heavy = GetCreatureSkill(Skill.HeavyWeapons);

            var maxMelee = unarmed;
            if (light.Current > maxMelee.Current)
                maxMelee = light;
            if (heavy.Current > maxMelee.Current)
                maxMelee = heavy;

            return maxMelee.Skill;
        }

        public float GetEvadeChance(WorldObject target)
        {
            // get player attack skill
            var attackSkill = GetCreatureSkill(GetCurrentWeaponSkill());

            // get target defense skill
            var creature = target as Creature;
            var defenseSkill = creature.GetCreatureSkill(Skill.MeleeDefense);

            var evadeChance = 1.0f - SkillCheck.GetSkillChance((int)attackSkill.Current, (int)defenseSkill.Current);
            return (float)evadeChance;
        }

        public float CalculateDamage(WorldObject target, ref bool criticalHit)
        {
            // evasion chance
            var evadeChance = GetEvadeChance(target);
            if (Physics.Common.Random.RollDice(0.0f, 1.0f) < evadeChance)
                return 0.0f;

            // get weapon base damage
            var player = this as Player;
            var weapon = GetEquippedWeapon();
            var baseDamageRange = weapon != null ? weapon.GetDamageMod(player) : new Range(1, 5);
            var baseDamage = Physics.Common.Random.RollDice(baseDamageRange.Min, baseDamageRange.Max);

            // get damage mods
            var powerBarMod = PowerLevel + 0.5f;
            var attributeMod = SkillFormula.GetAttributeMod(PropertyAttribute.Strength, (int)player.Strength.Current);
            var damage = baseDamage * attributeMod * powerBarMod;

            // critical hit
            var critical = 0.1f;
            if (Physics.Common.Random.RollDice(0.0f, 1.0f) < critical)
            {
                damage = baseDamageRange.Max * attributeMod * powerBarMod * 2.0f;
                criticalHit = true;
            }

            // get random body part @ attack height
            var bodyPart = BodyParts.GetBodyPart(AttackHeight);

            // get target armor
            var armor = GetArmor(target, bodyPart);

            // get target resistance
            var damageType = GetDamageType();
            var resistance = GetResistance(target, bodyPart, damageType);

            // scale damage for armor
            damage *= SkillFormula.CalcArmorMod(resistance);

            return damage;
        }

        public Creature_BodyPart GetBodyPart(WorldObject target, BodyPart bodyPart)
        {
            var creature = target as Creature;

            BiotaPropertiesBodyPart part = null;
            var idx = BodyParts.Indices[bodyPart];
            if (creature.Biota.BiotaPropertiesBodyPart.Count > idx)
                part = creature.Biota.BiotaPropertiesBodyPart.ElementAt(idx);
            else
                part = creature.Biota.BiotaPropertiesBodyPart.FirstOrDefault();

            return new Creature_BodyPart(creature, part);
        }

        public float GetArmor(WorldObject target, BodyPart bodyPart)
        {
            var part = GetBodyPart(target, bodyPart);

            return part.BaseArmorMod;
        }

        public float GetResistance(WorldObject target, BodyPart bodyPart, DamageType damageType)
        {
            var part = GetBodyPart(target, bodyPart);

            var resistance = 1.0f;

            switch (damageType)
            {
                case DamageType.Slash:
                    resistance = part.ArmorVsSlash;
                    break;

                case DamageType.Pierce:
                    resistance = part.ArmorVsPierce;
                    break;

                case DamageType.Bludgeon:
                    resistance = part.ArmorVsBludgeon;
                    break;

                case DamageType.Fire:
                    resistance = part.ArmorVsFire;
                    break;

                case DamageType.Cold:
                    resistance = part.ArmorVsCold;
                    break;

                case DamageType.Acid:
                    resistance = part.ArmorVsAcid;
                    break;

                case DamageType.Electric:
                    resistance = part.ArmorVsElectric;
                    break;

                case DamageType.Nether:
                    resistance = part.ArmorVsNether;
                    break;
            }

            return resistance;
        }

        public string GetSplatterDir(WorldObject target)
        {
            var sourcePos = new Vector3(Location.PositionX, Location.PositionY, 0);
            var targetPos = new Vector3(target.Location.PositionX, target.Location.PositionY, 0);
            var targetDir = new AFrame(target.Location.Pos, target.Location.Rotation).get_vector_heading();

            targetDir.Z = 0;
            targetDir = targetDir.Normalize();

            var sourceToTarget = (sourcePos - targetPos).Normalize();

            var dir = Vector3.Dot(sourceToTarget, targetDir);
            var angle = Vector3.Cross(sourceToTarget, targetDir);

            var frontBack = dir >= 0 ? "Front" : "Back";
            var leftRight = angle.Z <= 0 ? "Left" : "Right";

            return leftRight + frontBack;
        }
    }
}
