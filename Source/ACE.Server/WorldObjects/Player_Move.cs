using System;
using System.Threading;
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
        private TimeSpan defaultMoveToTimeout = TimeSpan.FromSeconds(15); // This is just a starting point number. It may be far off from retail.

        private int moveToChainCounter;
        private DateTime moveToChainStartTime;

        private int lastCompletedMove;

        public bool IsPlayerMovingTo => moveToChainCounter > lastCompletedMove;

        private int GetNextMoveToChainNumber()
        {
            return Interlocked.Increment(ref moveToChainCounter);
        }

        public void StopExistingMoveToChains()
        {
            Interlocked.Increment(ref moveToChainCounter);

            lastCompletedMove = moveToChainCounter;
        }

        public void CreateMoveToChain(WorldObject target, Action<bool> callback, float? useRadius = null, bool rotate = true)
        {
            if (FastTick)
            {
                CreateMoveToChain2(target, callback, useRadius, rotate);
                return;
            }

            var thisMoveToChainNumber = GetNextMoveToChainNumber();

            if (target.Location == null)
            {
                StopExistingMoveToChains();
                log.Error($"{Name}.CreateMoveToChain({target.Name}): target.Location is null");

                callback(false);
                return;
            }

            // fix bug in magic combat mode after walking to target,
            // crouch animation steps out of range
            if (useRadius == null)
                useRadius = target.UseRadius ?? 0.6f;

            if (CombatMode == CombatMode.Magic)
                useRadius = Math.Max(0.0f, useRadius.Value - 0.2f);

            // already within use distance?
            var withinUseRadius = CurrentLandblock.WithinUseRadius(this, target.Guid, out var targetValid, useRadius);
            if (withinUseRadius)
            {
                if (rotate)
                {
                    // send TurnTo motion
                    var rotateTime = Rotate(target);
                    var actionChain = new ActionChain();
                    actionChain.AddDelaySeconds(rotateTime);
                    actionChain.AddAction(this, () =>
                    {
                        lastCompletedMove = thisMoveToChainNumber;
                        callback(true);
                    });
                    actionChain.EnqueueChain();
                }
                else
                {
                    lastCompletedMove = thisMoveToChainNumber;
                    callback(true);
                }
                return;
            }

            if (target.WeenieType == WeenieType.Portal)
                MoveToPosition(target.Location);
            else
                MoveToObject(target, useRadius);

            moveToChainStartTime = DateTime.UtcNow;

            MoveToChain(target, thisMoveToChainNumber, callback, useRadius);
        }

        public void MoveToChain(WorldObject target, int thisMoveToChainNumber, Action<bool> callback, float? useRadius = null)
        {
            if (thisMoveToChainNumber != moveToChainCounter)
            {
                if (thisMoveToChainNumber > lastCompletedMove)
                    lastCompletedMove = thisMoveToChainNumber;

                callback(false);
                return;
            }

            // Break loop if CurrentLandblock == null (we portaled or logged out)
            if (CurrentLandblock == null)
            {
                StopExistingMoveToChains(); // This increments our moveToChainCounter and thus, should stop any additional actions in this chain
                callback(false);
                return;
            }

            // Have we timed out?
            if (moveToChainStartTime + defaultMoveToTimeout <= DateTime.UtcNow)
            {
                StopExistingMoveToChains(); // This increments our moveToChainCounter and thus, should stop any additional actions in this chain
                callback(false);
                return;
            }

            // Are we within use radius?
            var success = CurrentLandblock.WithinUseRadius(this, target.Guid, out var targetValid, useRadius);

            // If one of the items isn't on a landblock
            if (!targetValid)
            {
                StopExistingMoveToChains(); // This increments our moveToChainCounter and thus, should stop any additional actions in this chain
                callback(false);
                return;
            }

            if (!success)
            {
                // target not reached yet
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(0.1f);
                actionChain.AddAction(this, () => MoveToChain(target, thisMoveToChainNumber, callback, useRadius));
                actionChain.EnqueueChain();
            }
            else
            {
                if (thisMoveToChainNumber > lastCompletedMove)
                    lastCompletedMove = thisMoveToChainNumber;

                callback(true);
            }
        }

        public Position StartJump;

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
            //Console.WriteLine($"{Name}.OnMoveComplete({status})");

            IsMoving = false;

            if (IsPlayerMovingTo2)
            {
                OnMoveComplete_MoveTo2(status);

                if (MagicState.IsCasting)
                    OnMoveComplete_Magic(status);

                return;
            }

            switch (status)
            {
                case WeenieError.None:

                    Attack(MeleeTarget, AttackSequence);
                    break;

                default:

                    SendWeenieError(status);
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

            // jumping skill sort of used as a damping factor here
            //var jumpVelocity = 0.0f;
            //PhysicsObj.WeenieObj.InqJumpVelocity(1.0f, out jumpVelocity);
            var jumpVelocity = 11.25434f;   // TODO: figure out how to scale this better

            //var currVelocity = FastTick ? PhysicsObj.Velocity : PhysicsObj.CachedVelocity;
            var currVelocity = PhysicsObj.Velocity;

            var overspeed = jumpVelocity + currVelocity.Z + 4.5f;     // a little leeway

            var ratio = -overspeed / jumpVelocity;

            /*Console.WriteLine($"Jump velocity: {jumpVelocity}");
            Console.WriteLine($"Velocity: {currVelocity}");
            Console.WriteLine($"Overspeed: {overspeed}");
            Console.WriteLine($"Ratio: {ratio}");*/

            if (ratio > 0.0f)
            {
                //var damage = ratio * 40.0f;
                var damage = ratio * 87.293810f;
                //Console.WriteLine($"Damage: {damage}");

                // bludgeon damage
                // impact damage
                //if (damage > 0.0f && (FastTick || StartJump == null || StartJump.PositionZ - PhysicsObj.Position.Frame.Origin.Z > 10.0f))
                if (damage > 0.0f)
                    TakeDamage_Falling(damage);
            }
        }

        public void TakeDamage_Falling(float amount)
        {
            if (IsDead || Invincible) return;

            // handle lifestone protection?
            if (UnderLifestoneProtection)
            {
                HandleLifestoneProtection();
                return;
            }

            // scale by bludgeon protection
            var resistance = GetResistanceMod(DamageType.Bludgeon, null, null);
            var damage = (uint)Math.Round(amount * resistance);

            // update health
            var damageTaken = (uint)-UpdateVitalDelta(Health, (int)-damage);
            DamageHistory.Add(this, DamageType.Bludgeon, damageTaken);

            var msg = Strings.GetFallMessage(damageTaken, Health.MaxValue);

            SendMessage(msg, ChatMessageType.Combat);

            if (Health.Current <= 0)
            {
                OnDeath(new DamageHistoryInfo(this), DamageType.Bludgeon, false);
                Die();
            }
            else
                EnqueueBroadcast(new GameMessageSound(Guid, Sound.Wound3, 1.0f));
        }
    }
}
