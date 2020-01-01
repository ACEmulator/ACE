using System;
using System.Collections.Generic;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics.Animation;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Player melee attack
    /// </summary>
    partial class Player
    {
        /// <summary>
        /// The target this player is currently performing a melee attack on
        /// </summary>
        public Creature MeleeTarget;

        private float _powerLevel;

        /// <summary>
        /// The power bar level, a value between 0-1
        /// </summary>
        public float PowerLevel
        {
            get => IsExhausted ? 0.0f : _powerLevel;
            set => _powerLevel = value;
        }

        public override PowerAccuracy GetPowerRange()
        {
            if (PowerLevel < 0.33f)
                return PowerAccuracy.Low;
            else if (PowerLevel < 0.66f)
                return PowerAccuracy.Medium;
            else
                return PowerAccuracy.High;
        }

        /// <summary>
        /// Called when a player first initiates a melee attack
        /// </summary>
        public void HandleActionTargetedMeleeAttack(uint targetGuid, uint attackHeight, float powerLevel)
        {
            //log.Info($"-");

            if (CombatMode != CombatMode.Melee)
                return;

            if (PKLogout)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveBeenInPKBattleTooRecently));
                return;
            }

            // verify input
            powerLevel = Math.Clamp(powerLevel, 0.0f, 1.0f);

            AttackHeight = (AttackHeight)attackHeight;
            PowerLevel = powerLevel;

            // already in melee loop?
            if (Attacking || MeleeTarget != null && MeleeTarget.IsAlive)
                return;

            // get world object for target creature
            var target = CurrentLandblock?.GetObject(targetGuid);

            if (target == null)
            {
                log.Warn($"{Name}.HandleActionTargetedMeleeAttack({targetGuid:X8}, {AttackHeight}, {powerLevel}) - couldn't find target guid");
                return;
            }

            var creatureTarget = target as Creature;
            if (creatureTarget == null)
            {
                log.Warn($"{Name}.HandleActionTargetedMeleeAttack({targetGuid:X8}, {AttackHeight}, {powerLevel}) - target guid not creature");
                return;
            }

            // perform verifications
            if (IsBusy || Teleporting)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YoureTooBusy));
                return;
            }

            if (target.Teleporting)
                return;     // werror?

            //log.Info($"{Name}.HandleActionTargetedMeleeAttack({targetGuid:X8}, {attackHeight}, {powerLevel})");

            MeleeTarget = creatureTarget;
            AttackTarget = MeleeTarget;

            // reset PrevMotionCommand / DualWieldAlternate each time button is clicked
            PrevMotionCommand = MotionCommand.Invalid;
            DualWieldAlternate = false;

            var attackSequence = ++AttackSequence;

            if (IsStickyDistance(target) && IsDirectVisible(target))
            {
                // sticky melee
                var rotateTime = Rotate(target);

                var delayTime = rotateTime;
                if (NextRefillTime > DateTime.UtcNow.AddSeconds(delayTime))
                    delayTime = (float)(NextRefillTime - DateTime.UtcNow).TotalSeconds;

                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(delayTime);
                actionChain.AddAction(this, () => Attack(target, attackSequence));
                actionChain.EnqueueChain();
            }
            else
            {
                // turn / move to required
                if (GetCharacterOption(CharacterOption.UseChargeAttack))
                {
                    //log.Info($"{Name}.MoveTo({target.Name})");

                    // charge attack
                    MoveTo(target);
                }
                else
                {
                    //log.Info($"{Name}.CreateMoveToChain({target.Name})");

                    CreateMoveToChain(target, (success) =>
                    {
                        log.Info($"{Name}.CreateMoveToChain({target.Name}) complete - {success}");

                        if (success)
                            Attack(target, attackSequence);
                    });
                }
            }
        }

        /// <summary>
        /// called when client sends the 'Cancel attack' network message
        /// </summary>
        public void HandleActionCancelAttack()
        {
            //Console.WriteLine($"{Name}.HandleActionCancelAttack()");

            MeleeTarget = null;
            MissileTarget = null;

            PhysicsObj.cancel_moveto();
        }

        /// <summary>
        /// Performs a player melee attack against a target
        /// </summary>
        public void Attack(WorldObject target, int attackSequence)
        {
            //log.Info($"{Name}.Attack({target.Name}, {attackSequence})");

            if (CombatMode != CombatMode.Melee || MeleeTarget == null || !IsAlive || AttackSequence != attackSequence)
                return;

            var creature = target as Creature;
            if (creature == null || !creature.IsAlive)
                return;

            var animLength = DoSwingMotion(target, out var attackFrames);
            if (animLength == 0)
                return;

            // point of no return beyond this point -- cannot be cancelled
            Attacking = true;

            var weapon = GetEquippedMeleeWeapon();
            var attackType = GetWeaponAttackType(weapon);
            var numStrikes = GetNumStrikes(attackType);
            var swingTime = animLength / numStrikes / 1.5f;

            var actionChain = new ActionChain();

            // stamina usage
            // TODO: ensure enough stamina for attack
            var staminaCost = GetAttackStamina(GetPowerRange());
            UpdateVitalDelta(Stamina, -staminaCost);

            if (numStrikes != attackFrames.Count)
            {
                //log.Warn($"{Name}.GetAttackFrames(): MotionTableId: {MotionTableId:X8}, MotionStance: {CurrentMotionState.Stance}, Motion: {GetSwingAnimation()}, AttackFrames.Count({attackFrames.Count}) != NumStrikes({numStrikes})");
                numStrikes = attackFrames.Count;
            }

            // handle self-procs
            TryProcEquippedItems(this, true);

            var prevTime = 0.0f;
            bool targetProc = false;

            for (var i = 0; i < numStrikes; i++)
            {
                // are there animation hooks for damage frames?
                //if (numStrikes > 1 && !TwoHandedCombat)
                //actionChain.AddDelaySeconds(swingTime);
                actionChain.AddDelaySeconds(attackFrames[i] * animLength - prevTime);
                prevTime = attackFrames[i] * animLength;

                actionChain.AddAction(this, () =>
                {
                    if (IsDead)
                    {
                        Attacking = false;
                        return;
                    }

                    var damageEvent = DamageTarget(creature, weapon);

                    // handle target procs
                    if (damageEvent != null && damageEvent.HasDamage && !targetProc)
                    {
                        TryProcEquippedItems(creature, false);
                        targetProc = true;
                    }

                    if (weapon != null && weapon.IsCleaving)
                    {
                        var cleave = GetCleaveTarget(creature, weapon);
                        foreach (var cleaveHit in cleave)
                            DamageTarget(cleaveHit, weapon);

                        // target procs don't happen for cleaving
                    }
                });

                //if (numStrikes == 1 || TwoHandedCombat)
                    //actionChain.AddDelaySeconds(swingTime);
            }

            //actionChain.AddDelaySeconds(animLength - swingTime * numStrikes);
            actionChain.AddDelaySeconds(animLength - prevTime);

            actionChain.AddAction(this, () =>
            {
                Session.Network.EnqueueSend(new GameEventAttackDone(Session));
                Attacking = false;

                if (creature.IsAlive && GetCharacterOption(CharacterOption.AutoRepeatAttacks))
                {
                    Session.Network.EnqueueSend(new GameEventCombatCommenceAttack(Session));
                    Session.Network.EnqueueSend(new GameEventAttackDone(Session));

                    // powerbar refill timing
                    var refillMod = IsDualWieldAttack ? 0.8f : 1.0f;    // dual wield powerbar refills 20% faster

                    var nextRefillTime = PowerLevel * refillMod;
                    NextRefillTime = DateTime.UtcNow.AddSeconds(nextRefillTime);

                    var nextAttack = new ActionChain();
                    nextAttack.AddDelaySeconds(nextRefillTime);
                    nextAttack.AddAction(this, () => Attack(target, attackSequence));
                    nextAttack.EnqueueChain();
                }
                else
                    MeleeTarget = null;
            });

            actionChain.EnqueueChain();

            if (UnderLifestoneProtection)
                LifestoneProtectionDispel();
        }

        /// <summary>
        /// Performs the player melee swing animation
        /// </summary>
        public float DoSwingMotion(WorldObject target, out List<float> attackFrames)
        {
            // get the proper animation speed for this attack,
            // based on weapon speed and player quickness
            var baseSpeed = GetAnimSpeed();
            var animSpeedMod = IsDualWieldAttack ? 1.2f : 1.0f;     // dual wield swing animation 20% faster
            var animSpeed = baseSpeed * animSpeedMod;

            var swingAnimation = GetSwingAnimation();
            var animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, swingAnimation, animSpeed);
            //Console.WriteLine($"AnimSpeed: {animSpeed}, AnimLength: {animLength}");

            attackFrames = MotionTable.GetAttackFrames(MotionTableId, CurrentMotionState.Stance, swingAnimation);
            //Console.WriteLine($"Attack frames: {string.Join(",", attackFrames)}");

            // broadcast player swing animation to clients
            var motion = new Motion(this, swingAnimation, animSpeed);
            motion.MotionState.TurnSpeed = 2.25f;
            motion.MotionFlags |= MotionFlags.StickToObject;
            motion.TargetGuid = target.Guid;
            CurrentMotionState = motion;

            EnqueueBroadcastMotion(motion);
            return animLength;
        }

        public static readonly float KickThreshold = 0.75f;

        public MotionCommand PrevMotionCommand;

        /// <summary>
        /// Returns the melee swing animation - based on weapon,
        /// current stance, power bar, and attack height
        /// </summary>
        public MotionCommand GetSwingAnimation()
        {
            if (IsDualWieldAttack)
                DualWieldAlternate = !DualWieldAlternate;

            var offhand = IsDualWieldAttack && !DualWieldAlternate;

            var weapon = GetEquippedMeleeWeapon();

            if (weapon != null)
            {
                AttackType = weapon.GetAttackType(CurrentMotionState.Stance, PowerLevel, offhand);
            }
            else
            {
                AttackType = PowerLevel > KickThreshold ? AttackType.Kick : AttackType.Punch;
            }

            var motion = CombatTable.GetMotion(CurrentMotionState.Stance, AttackHeight.Value, AttackType, PrevMotionCommand);
            PrevMotionCommand = motion;

            //Console.WriteLine($"{motion}");

            return motion;
        }

        public bool IsStickyDistance(WorldObject target)
        {
            var cylDist = (float)Physics.Common.Position.CylinderDistance(PhysicsObj.GetRadius(), PhysicsObj.GetHeight(), PhysicsObj.Position,
                target.PhysicsObj.GetRadius(), target.PhysicsObj.GetHeight(), target.PhysicsObj.Position);

            return cylDist <= 4.0f;
        }
    }
}
