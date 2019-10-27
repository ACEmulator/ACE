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
        public WorldObject MeleeTarget;

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
            if (MeleeTarget != null)
                return;

            // get world object for target creature
            var target = CurrentLandblock?.GetObject(targetGuid);

            if (target == null)
            {
                log.Warn($"{Name}.HandleActionTargetdMeleeAttack({targetGuid:X8}, {AttackHeight}, {powerLevel}) - couldn't find target guid");
                return;
            }

            var creatureTarget = target as Creature;
            if (creatureTarget == null)
            {
                log.Warn($"{Name}.HandleActionTargetdMeleeAttack({targetGuid:X8}, {AttackHeight}, {powerLevel}) - target guid not creature");
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

            MeleeTarget = target;
            AttackTarget = MeleeTarget;

            if (IsStickyDistance(target) && IsDirectVisible(target))
            {
                // sticky melee
                var rotateTime = Rotate(target);
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(rotateTime);
                actionChain.AddAction(this, () => Attack(target));
                actionChain.EnqueueChain();
            }
            else
            {
                // turn / move to required
                if (GetCharacterOption(CharacterOption.UseChargeAttack))
                {
                    // charge attack
                    MoveTo(target);
                }
                else
                {
                    
                    CreateMoveToChain(target, (success) =>
                    {
                        if (success)
                            Attack(target);
                    });
                }
            }
        }

        /// <summary>
        /// called when client sends the 'Cancel attack' network message
        /// </summary>
        public void HandleActionCancelAttack()
        {
            //Console.WriteLine("HandleActionCancelAttack");

            MeleeTarget = null;
            MissileTarget = null;

            PhysicsObj.cancel_moveto();
        }

        /// <summary>
        /// Performs a player melee attack against a target
        /// </summary>
        public void Attack(WorldObject target)
        {
            if (CombatMode != CombatMode.Melee || MeleeTarget == null || !IsAlive)
                return;

            var creature = target as Creature;
            if (creature == null || !creature.IsAlive)
                return;

            var animLength = DoSwingMotion(target, out var attackFrames);
            if (animLength == 0)
                return;

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
                    if (IsDead) return;

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

                if (creature.IsAlive && GetCharacterOption(CharacterOption.AutoRepeatAttacks))
                {
                    Session.Network.EnqueueSend(new GameEventCombatCommenceAttack(Session));
                    Session.Network.EnqueueSend(new GameEventAttackDone(Session));

                    // powerbar refill timing
                    var refillMod = IsDualWieldAttack ? 0.8f : 1.0f;    // dual wield powerbar refills 20% faster

                    var nextAttack = new ActionChain();
                    nextAttack.AddDelaySeconds(PowerLevel * refillMod);
                    nextAttack.AddAction(this, () => Attack(target));
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

        private static readonly float KickThreshold = 0.75f;

        public AttackType AttackType { get; set; }

        /// <summary>
        /// Returns the melee swing animation - based on weapon,
        /// current stance, power bar, and attack height
        /// </summary>
        public override MotionCommand GetSwingAnimation()
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

            var motion = CombatTable.GetMotion(CurrentMotionState.Stance, AttackHeight.Value, AttackType);

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
