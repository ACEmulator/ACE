using System;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Collision;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public Position StartJump;

        public bool InitMoveListener;

        public override void MoveTo(WorldObject target, float runRate = 0.0f)
        {
            if (runRate == 0.0f)
                runRate = GetRunRate();

            //Console.WriteLine($"{Name}.MoveTo({target.Name})");

            var motion = new Motion(this, target, MovementType.MoveToObject);
            motion.MoveToParameters.MovementParameters |= MovementParams.CanCharge | MovementParams.FailWalk | MovementParams.UseFinalHeading | MovementParams.Sticky | MovementParams.MoveAway;
            motion.MoveToParameters.WalkRunThreshold = 1.0f;
            motion.MoveToParameters.Speed = 1.5f;   // charge modifier
            motion.MoveToParameters.FailDistance = 15.0f;
            motion.RunRate = runRate;

            CurrentMotionState = motion;

            EnqueueBroadcastMotion(motion);

            var mvp = GetChargeParameters();
            PhysicsObj.MoveToObject(target.PhysicsObj, mvp);

            IsMoving = true;

            if (!InitMoveListener)
            {
                PhysicsObj.add_moveto_listener(OnMoveComplete);
                InitMoveListener = true;
            }

            MoveTo_Tick();
        }

        public static float MoveToRate = 0.1f;

        public void MoveTo_Tick()
        {
            //Console.WriteLine($"{Name}.MoveTo_Tick()");

            PhysicsObj.update_object();

            if (IsMoving)
                Enqueue_NextMoveTick();
        }

        public void Enqueue_NextMoveTick()
        {
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(MoveToRate);
            actionChain.AddAction(this, MoveTo_Tick);
            actionChain.EnqueueChain();
        }

        public override void OnMoveComplete(WeenieError status)
        {
            //Console.WriteLine($"{Name}.OnMoveComplete()");
            IsMoving = false;

            switch (status)
            {
                case WeenieError.None:
                    Attack(MeleeTarget);
                    break;
                default:
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, status));
                    HandleActionCancelAttack();
                    break;
            }
        }

        public MovementParameters GetChargeParameters()
        {
            var mvp = new MovementParameters();

            // set non-default params for player melee charge
            mvp.Flags &= ~MovementParamFlags.CanWalk;
            mvp.Flags |= MovementParamFlags.CanCharge | MovementParamFlags.FailWalk | MovementParamFlags.UseFinalHeading | MovementParamFlags.Sticky | MovementParamFlags.MoveAway;
            mvp.HoldKeyToApply = HoldKey.Run;
            mvp.FailDistance = 15.0f;
            mvp.Speed = 1.5f;

            return mvp;
        }

        public void HandleFallingDamage(EnvCollisionProfile collision)
        {
            // starting with phat logic
            var jumpVelocity = 0.0f;
            PhysicsObj.WeenieObj.InqJumpVelocity(1.0f, ref jumpVelocity);

            var cachedVelocity = PhysicsObj.CachedVelocity;

            var overspeed = jumpVelocity + cachedVelocity.Z + 4.5f;     // a little leeway

            var ratio = -overspeed / jumpVelocity;

            /*Console.WriteLine($"Collision velocity: {cachedVelocity}");
            Console.WriteLine($"Jump velocity: {jumpVelocity}");
            Console.WriteLine($"Overspeed: {overspeed}");
            Console.WriteLine($"Ratio: {ratio}");*/

            if (ratio > 0.0f)
            {
                var damage = ratio * 40.0f;
                //Console.WriteLine($"Damage: {damage}");

                // bludgeon damage
                // impact damage
                if (damage > 0.0f && (StartJump == null || StartJump.PositionZ - Location.PositionZ > 10.0f))
                    TakeDamage_Falling(damage);
            }
        }

        public void TakeDamage_Falling(float amount)
        {
            if (Invincible ?? false) return;

            // handle lifestone protection?
            if (UnderLifestoneProtection)
            {
                HandleLifestoneProtection();
                return;
            }

            // scale by bludgeon protection?
            var resistance = EnchantmentManager.GetResistanceMod(DamageType.Bludgeon);

            var damage = (uint)Math.Round(amount * resistance);

            var percent = (float)damage / Health.MaxValue;

            var msg = Strings.GetFallMessage(damage, Health.MaxValue);

            Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Combat));
            EnqueueBroadcast(new GameMessageSound(Guid, Sound.Wound3, 1.0f));

            // update health
            var damageTaken = (uint)-UpdateVitalDelta(Health, (int)-damage);
            DamageHistory.Add(this, DamageType.Bludgeon, damageTaken);

            if (Health.Current == 0)
            {
                OnDeath(this, DamageType.Bludgeon, false);
                Die();
                return;
            }
        }
    }
}
