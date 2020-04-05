using System;
using System.Numerics;

using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics;
using ACE.Server.Physics.Animation;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        private float _accuracyLevel;

        public float AccuracyLevel
        {
            get => IsExhausted ? 0.0f : _accuracyLevel;
            set => _accuracyLevel = value;
        }

        public Creature MissileTarget;

        public PowerAccuracy GetAccuracyRange()
        {
            if (AccuracyLevel < 0.33f)
                return PowerAccuracy.Low;
            else if (AccuracyLevel < 0.66f)
                return PowerAccuracy.Medium;
            else
                return PowerAccuracy.High;
        }

        /// <summary>
        /// Called by network packet handler 0xA - GameActionTargetedMissileAttack
        /// </summary>
        /// <param name="targetGuid">The target guid</param>
        /// <param name="attackHeight">The attack height 1-3</param>
        /// <param name="accuracyLevel">The 0-1 accuracy bar level</param>
        public void HandleActionTargetedMissileAttack(uint targetGuid, uint attackHeight, float accuracyLevel)
        {
            //log.Info($"-");

            if (CombatMode != CombatMode.Missile)
            {
                log.Error($"{Name}.HandleActionTargetedMissileAttack({targetGuid:X8}, {attackHeight}, {accuracyLevel}) - CombatMode mismatch {CombatMode}, LastCombatMode: {LastCombatMode}");

                if (LastCombatMode == CombatMode.Missile)
                    CombatMode = CombatMode.Missile;
                else
                    return;
            }

            if (IsBusy || Teleporting || suicideInProgress)
            {
                SendWeenieError(WeenieError.YoureTooBusy);
                return;
            }

            if (FastTick && !PhysicsObj.TransientState.HasFlag(TransientStateFlags.OnWalkable))
            {
                SendWeenieError(WeenieError.YouCantDoThatWhileInTheAir);
                return;
            }

            if (PKLogout)
            {
                SendWeenieError(WeenieError.YouHaveBeenInPKBattleTooRecently);
                return;
            }

            var weapon = GetEquippedMissileWeapon();
            var ammo = GetEquippedAmmo();

            // sanity check
            accuracyLevel = Math.Clamp(accuracyLevel, 0.0f, 1.0f);

            if (weapon == null || weapon.IsAmmoLauncher && ammo == null) return;

            AttackHeight = (AttackHeight)attackHeight;
            AttackQueue.Add(accuracyLevel);

            if (MissileTarget == null)
                AccuracyLevel = accuracyLevel;

            // get world object of target guid
            var target = CurrentLandblock?.GetObject(targetGuid) as Creature;
            if (target == null || target.Teleporting)
            {
                //log.Warn($"{Name}.HandleActionTargetedMissileAttack({targetGuid:X8}, {AttackHeight}, {accuracyLevel}) - couldn't find creature target guid");
                return;
            }

            if (Attacking || MissileTarget != null && MissileTarget.IsAlive)
                return;

            if (!CanDamage(target))
                return;     // werror?

            //log.Info($"{Name}.HandleActionTargetedMissileAttack({targetGuid:X8}, {attackHeight}, {accuracyLevel})");

            AttackTarget = target;
            MissileTarget = target;

            var attackSequence = ++AttackSequence;

            // turn if required
            var rotateTime = Rotate(target);
            var actionChain = new ActionChain();

            var delayTime = rotateTime;
            if (NextRefillTime > DateTime.UtcNow.AddSeconds(delayTime))
                delayTime = (float)(NextRefillTime - DateTime.UtcNow).TotalSeconds;

            actionChain.AddDelaySeconds(delayTime);

            // do missile attack
            actionChain.AddAction(this, () => LaunchMissile(target, attackSequence));
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Launches a missile attack from player to target
        /// </summary>
        public void LaunchMissile(WorldObject target, int attackSequence)
        {
            if (AttackSequence != attackSequence)
                return;

            var weapon = GetEquippedMissileWeapon();
            if (weapon == null || CombatMode == CombatMode.NonCombat)
            {
                OnAttackDone();
                return;
            }

            var ammo = weapon.IsAmmoLauncher ? GetEquippedAmmo() : weapon;
            if (ammo == null)
            {
                OnAttackDone();
                return;
            }

            var creature = target as Creature;
            if (!IsAlive || IsBusy || MissileTarget == null || creature == null || !creature.IsAlive || suicideInProgress)
            {
                OnAttackDone();
                return;
            }

            // launch animation
            // point of no return beyond this point -- cannot be cancelled
            Attacking = true;

            // get z-angle for aim motion
            var aimVelocity = GetAimVelocity(target);

            var aimLevel = GetAimLevel(aimVelocity);

            // calculate projectile spawn pos and velocity
            var localOrigin = GetProjectileSpawnOrigin(ammo.WeenieClassId, aimLevel);

            var velocity = CalculateProjectileVelocity(localOrigin, target, out Vector3 origin, out Quaternion orientation);

            //Console.WriteLine($"Velocity: {velocity}");

            var actionChain = new ActionChain();
            var launchTime = EnqueueMotion(actionChain, aimLevel);

            // launch projectile
            actionChain.AddAction(this, () =>
            {
                // handle self-procs
                TryProcEquippedItems(this, true);

                var sound = GetLaunchMissileSound(weapon);
                EnqueueBroadcast(new GameMessageSound(Guid, sound, 1.0f));

                // stamina usage
                // TODO: ensure enough stamina for attack
                // TODO: verify formulas - double/triple cost for bow/xbow?
                var staminaCost = GetAttackStamina(GetAccuracyRange());
                UpdateVitalDelta(Stamina, -staminaCost);

                var projectile = LaunchProjectile(weapon, ammo, target, origin, orientation, velocity);
                UpdateAmmoAfterLaunch(ammo);
            });

            // ammo remaining?
            if (ammo.StackSize == null || ammo.StackSize <= 1)
            {
                actionChain.AddAction(this, () =>
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You are out of ammunition!"));
                    SetCombatMode(CombatMode.NonCombat);
                    Attacking = false;
                    OnAttackDone();
                });

                actionChain.EnqueueChain();
                return;
            }

            // reload animation
            var animSpeed = GetAnimSpeed();
            var reloadTime = EnqueueMotion(actionChain, MotionCommand.Reload, animSpeed);

            // reset for next projectile
            EnqueueMotion(actionChain, MotionCommand.Ready);
            var linkTime = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Reload, MotionCommand.Ready);
            //var cycleTime = MotionTable.GetCycleLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Ready);

            actionChain.AddAction(this, () => EnqueueBroadcast(new GameMessageParentEvent(this, ammo, ACE.Entity.Enum.ParentLocation.RightHand,
                ACE.Entity.Enum.Placement.RightHandCombat)));

            actionChain.AddDelaySeconds(linkTime);

            actionChain.AddAction(this, () =>
            {
                Session.Network.EnqueueSend(new GameEventAttackDone(Session));
                Attacking = false;

                if (creature.IsAlive && GetCharacterOption(CharacterOption.AutoRepeatAttacks) && !IsBusy)
                {
                    Session.Network.EnqueueSend(new GameEventCombatCommenceAttack(Session));
                    Session.Network.EnqueueSend(new GameEventAttackDone(Session));

                    AccuracyLevel = AttackQueue.Fetch();

                    // can be cancelled, but cannot be pre-empted with another attack
                    var nextAttack = new ActionChain();
                    var nextRefillTime = AccuracyLevel + 0.1f;

                    NextRefillTime = DateTime.UtcNow.AddSeconds(nextRefillTime);
                    nextAttack.AddDelaySeconds(nextRefillTime);

                    // perform next attack
                    nextAttack.AddAction(this, () => { LaunchMissile(target, attackSequence); });
                    nextAttack.EnqueueChain();
                }
                else
                    OnAttackDone();
            });

            actionChain.EnqueueChain();

            if (UnderLifestoneProtection)
                LifestoneProtectionDispel();
        }

        // TODO: the damage pipeline currently uses the creature ammo instead of the projectile
        // for calculating damage. when the last arrow is launched, the player ammo will be null
        // give projectiles an owner, and have the damage pipeline take the actual damage source object
        // (ie. the arrow-in-flight, or a melee weapon)

        public override float GetAimHeight(WorldObject target)
        {
            switch (AttackHeight.Value)
            {
                case ACE.Entity.Enum.AttackHeight.High: return 1.0f;
                case ACE.Entity.Enum.AttackHeight.Medium: return 2.0f;
                //case AttackHeight.Low: return target.Height;
                case ACE.Entity.Enum.AttackHeight.Low: return 3.0f;
            }
            return 2.0f;
        }

        public override void UpdateAmmoAfterLaunch(WorldObject ammo)
        {
            // hide previously held ammo
            EnqueueBroadcast(new GameMessagePickupEvent(ammo));

            if (ammo.StackSize == null || ammo.StackSize <= 1)
                TryDequipObjectWithNetworking(ammo.Guid, out _, DequipObjectAction.ConsumeItem);
            else
                TryConsumeFromInventoryWithNetworking(ammo, 1);
        }
    }
}
