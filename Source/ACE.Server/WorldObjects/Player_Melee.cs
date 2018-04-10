using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Extensions;
using System;
using System.Numerics;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public uint AttackHeight;
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

            AttackHeight = attackHeight;
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

        public float GetDistance(WorldObject target)
        {
            return Location.DistanceTo(target.Location);
        }

        public float GetAngle(WorldObject target)
        {
            var currentDir = Location.GetCurrentDir();
            var targetDir = GetDirection(target);

            // get the 2D angle between these vectors
            return GetAngle(currentDir, targetDir);
        }

        /// <summary>
        /// Returns the 2D angle between 2 vectors
        /// </summary>
        public float GetAngle(Vector3 a, Vector3 b)
        {
            var cosTheta = a.Dot2D(b);
            var rads = Math.Acos(cosTheta);
            var angle = rads * (180.0f / Math.PI);
            return (float)angle;
        }

        public Vector3 GetDirection(WorldObject target)
        {
            // draw a line from current location
            // to target location
            var offset = target.Location.Pos - Location.Pos;
            offset = offset.Normalize();

            return offset;
        }

        public void Rotate()
        {

        }

        public void MoveTo()
        {

        }

        public void Attack(WorldObject target)
        {
            if (MeleeTarget == null)
                return;

            var actionChain = DoSwingMotion(target);

            var damage = CalculateDamage(target);
            if (damage > 0.0f)
                target.TakeDamage(this, damage);
            else
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{target.Name} evaded your attack.", ChatMessageType.CombatEnemy));

            var creature = target as Creature;
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
                        var action = PowerLevel < 0.5f ? "Thrust" : "Slash";
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
            if (AttackHeight == 1) return "High";
            else if (AttackHeight == 2) return "Med";
            else return "Low";
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

        public float CalculateDamage(WorldObject target)
        {
            var critical = 0.1f;
            var variance = 0.2f;
            var evade = 0.25f;

            // evasion chance
            if (Physics.Common.Random.RollDice(0.0f, 1.0f) < evade)
                return 0.0f;

            // test: 1/5 of monster total health
            var creature = target as Creature;

            var baseDamage = creature.Health.MaxValue / 5.0f;

            var thisVar = Physics.Common.Random.RollDice(-variance * baseDamage, variance * baseDamage);
            var damage = baseDamage + thisVar;

            var rng = Physics.Common.Random.RollDice(0.0f, 1.0f);
            if (rng < critical)
                damage *= 2.5f;

            return damage;
        }
    }
}
