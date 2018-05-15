using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.Motion;
using ACE.Server.Physics.Animation;
using System;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public WorldObject MeleeTarget;

        public AttackHeight AttackHeight;
        public float PowerLevel;

        public override int GetPowerRange()
        {
            if (PowerLevel < 0.33f)
                return 1;
            else if (PowerLevel < 0.66f)
                return 2;
            else
                return 3;
        }

        public override string GetAttackHeight()
        {
            return AttackHeight.GetString();
        }

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
                log.Warn("Unknown target guid " + guid.Full.ToString("X8"));
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
            Rotate(target);
            MoveTo(target);

            // do melee attack
            Attack(target);
        }

        public void HandleActionCancelAttack()
        {
            MeleeTarget = null;
            MissileTarget = null;
        }

        public void Attack(WorldObject target)
        {
            if (MeleeTarget == null)
                return;

            var creature = target as Creature;
            var actionChain = DoSwingMotion(target, out float animLength);

            DamageTarget(target);

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

        public override ActionChain DoSwingMotion(WorldObject target, out float animLength)
        {
            var swingAnimation = new MotionItem(GetSwingAnimation(), 1.25f);
            animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, swingAnimation);

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

        public override MotionCommand GetSwingAnimation()
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
    }
}
