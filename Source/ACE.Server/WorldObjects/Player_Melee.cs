using System;
using System.Collections.Generic;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
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
            /*Console.WriteLine("HandleActionTargetedMeleeAttack");
            Console.WriteLine("Target ID: " + guid.Full.ToString("X8"));
            Console.WriteLine("Attack height: " + attackHeight);
            Console.WriteLine("Power level: " + powerLevel);*/

            // sanity check
            powerLevel = Math.Clamp(powerLevel, 0.0f, 1.0f);

            AttackHeight = (AttackHeight)attackHeight;
            PowerLevel = powerLevel;

            // get world object of target guid
            var target = CurrentLandblock?.GetObject(targetGuid);
            if (target == null)
            {
                log.Warn($"Unknown target guid {targetGuid:X8}");
                return;
            }
            var creatureTarget = target as Creature;
            if (creatureTarget == null)
            {
                log.Warn($"Target GUID not creature {targetGuid:X8}");
                return;
            }

            if (MeleeTarget == null)
            {
                MeleeTarget = target;
                AttackTarget = MeleeTarget;
            }
            else
                return;

            // get distance from target
            var dist = GetDistance(target);

            // get angle to target
            var angle = GetAngle(target);

            //Console.WriteLine("Dist: " + dist);
            //Console.WriteLine("Angle: " + angle);

            // turn / moveto if required
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
                if (GetCharacterOption(CharacterOption.UseChargeAttack))
                {
                    // charge attack
                    MoveTo(target);
                }
                else
                {
                    // move to
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

        /// <summary>
        /// Returns the melee swing animation, based on current stance and weapon
        /// </summary>
        public override MotionCommand GetSwingAnimation()
        {
            MotionCommand motion = new MotionCommand();

            switch (CurrentMotionState.Stance)
            {
                case MotionStance.SwordCombat:
                case MotionStance.SwordShieldCombat:
                case MotionStance.TwoHandedSwordCombat:
                case MotionStance.TwoHandedStaffCombat:
                case MotionStance.DualWieldCombat:
                    {
                        // handle dual wielding weapon alternating
                        if (IsDualWieldAttack) DualWieldAlternate = !DualWieldAlternate;

                        var weapon = GetEquippedMeleeWeapon();
                        var attackType = GetWeaponAttackType(weapon);

                        var action = PowerLevel < 0.33f && attackType.HasFlag(AttackType.Thrust) || !attackType.HasFlag(AttackType.Slash) ? "Thrust" : "Slash";

                        // handle multistrike weapons
                        action = MultiStrike(attackType, action);

                        if (IsDualWieldAttack && !DualWieldAlternate)
                            action = "Offhand" + action;

                        // this is very strange:
                        // sword + no shield has slash, but not thrust
                        // sword + shield has thrust, but not slash...
                        if (CurrentMotionState.Stance == MotionStance.SwordCombat)
                        {
                            if (action.Contains("Double") || action.Contains("Triple"))
                                action = action.Replace("Thrust", "Slash");
                        }
                        else if (CurrentMotionState.Stance == MotionStance.SwordShieldCombat)
                        {
                            if (action.Contains("Double") || action.Contains("Triple"))
                                action = action.Replace("Slash", "Thrust");
                        }

                        Enum.TryParse(action + GetAttackHeight(), out motion);
                        return motion;
                    }
                case MotionStance.HandCombat:
                default:
                    {
                        // is the player holding a weapon?
                        var weapon = GetEquippedMeleeWeapon();

                        // no weapon: power range 1-3
                        // unarmed weapon: power range 1-2
                        if (weapon == null)
                            Enum.TryParse("Attack" + GetAttackHeight() + (int)GetPowerRange(), out motion);
                        else
                            Enum.TryParse("Attack" + GetAttackHeight() + Math.Min((int)GetPowerRange(), 2), out motion);

                        return motion;
                    }
            }
        }

        public bool IsMeleeDistance(WorldObject target)
        {
            // always use spheres?
            var cylDist = (float)Physics.Common.Position.CylinderDistance(PhysicsObj.GetRadius(), PhysicsObj.GetHeight(), PhysicsObj.Position,
                target.PhysicsObj.GetRadius(), target.PhysicsObj.GetHeight(), target.PhysicsObj.Position);

            return cylDist <= 0.6f;
        }

        public bool IsStickyDistance(WorldObject target)
        {
            var cylDist = (float)Physics.Common.Position.CylinderDistance(PhysicsObj.GetRadius(), PhysicsObj.GetHeight(), PhysicsObj.Position,
                target.PhysicsObj.GetRadius(), target.PhysicsObj.GetHeight(), target.PhysicsObj.Position);

            return cylDist <= 4.0f;
        }
    }
}
