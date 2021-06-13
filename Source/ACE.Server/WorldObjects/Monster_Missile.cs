using System;
using System.Numerics;

using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics.Animation;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// The delay between missile attacks (todo: find actual value)
        /// </summary>
        public static readonly float MissileDelay = 1.0f;

        /// <summary>
        /// Returns TRUE if monster has physical ranged attacks
        /// </summary>
        public new bool IsRanged => GetEquippedMissileWeapon() != null;

        /// <summary>
        /// Starts a monster missile attack
        /// </summary>
        public void RangeAttack()
        {
            var target = AttackTarget as Creature;

            if (target == null || !target.IsAlive)
            {
                FindNextTarget();
                return;
            }

            var weapon = GetEquippedMissileWeapon();
            var ammo = GetEquippedAmmo();

            if (weapon == null || weapon.IsAmmoLauncher && ammo == null) return;

            // simulate accuracy bar / allow client rotate to fully complete
            var actionChain = new ActionChain();
            //IsTurning = true;
            //actionChain.AddDelaySeconds(0.5f);

            // do missile attack
            actionChain.AddAction(this, LaunchMissile);
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Launches a missile attack from monster to target
        /// </summary>
        public void LaunchMissile()
        {
            //IsTurning = false;

            var weapon = GetEquippedMissileWeapon();
            if (weapon == null || AttackTarget == null) return;

            var ammo = weapon.IsAmmoLauncher ? GetEquippedAmmo() : weapon;
            if (ammo == null) return;

            var launcher = GetEquippedMissileLauncher();

            /*if (!IsDirectVisible(AttackTarget))
            {
                // ensure direct line of sight
                //NextAttackTime = Timers.RunningTime + 1.0f;
                SwitchToMeleeAttack();
                return;
            }*/
            if (SwitchWeaponsPending)
            {
                NextAttackTime = Timers.RunningTime + 1.0f;
                return;
            }

            // should this be called each launch?
            AttackHeight = ChooseAttackHeight();

            var dist = GetDistanceToTarget();
            //Console.WriteLine("RangeAttack: " + dist);

            if (DebugMove)
                Console.WriteLine($"[{Timers.RunningTime}] - {Name} ({Guid}) - LaunchMissile");

            var projectileSpeed = GetProjectileSpeed();

            // get z-angle for aim motion
            var aimVelocity = GetAimVelocity(AttackTarget, projectileSpeed);

            var aimLevel = GetAimLevel(aimVelocity);

            // calculate projectile spawn pos and velocity
            var localOrigin = GetProjectileSpawnOrigin(ammo.WeenieClassId, aimLevel);

            var velocity = CalculateProjectileVelocity(localOrigin, AttackTarget, projectileSpeed, out Vector3 origin, out Quaternion orientation);

            //Console.WriteLine($"Velocity: {velocity}");

            // launch animation
            var actionChain = new ActionChain();
            var launchTime = EnqueueMotion(actionChain, aimLevel);
            //Console.WriteLine("LaunchTime: " + launchTime);

            // launch projectile
            actionChain.AddAction(this, () =>
            {
                if (IsDead) return;

                // handle self-procs
                TryProcEquippedItems(this, this, true, weapon);

                var sound = GetLaunchMissileSound(weapon);
                EnqueueBroadcast(new GameMessageSound(Guid, sound, 1.0f));

                // TODO: monster stamina usage

                if (AttackTarget != null)
                {
                    var projectile = LaunchProjectile(launcher, ammo, AttackTarget, origin, orientation, velocity);
                    UpdateAmmoAfterLaunch(ammo);
                }
            });

            // will ammo be depleted?
            /*if (ammo.StackSize == null || ammo.StackSize <= 1)
            {
                // compare monsters: lugianmontokrenegade /  sclavusse / zombielichtowerarcher
                actionChain.EnqueueChain();
                NextMoveTime = NextAttackTime = Timers.RunningTime + launchTime + MissileDelay;
                return;
            }*/

            // reload animation
            var animSpeed = GetAnimSpeed();
            var reloadTime = EnqueueMotion(actionChain, MotionCommand.Reload, animSpeed);
            //Console.WriteLine("ReloadTime: " + reloadTime);

            // reset for next projectile
            EnqueueMotion(actionChain, MotionCommand.Ready);

            var linkAnim = reloadTime > 0 ? MotionCommand.Reload : aimLevel;

            var linkTime = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, linkAnim, MotionCommand.Ready);

            if (weapon.IsThrownWeapon)
            {
                if (reloadTime > 0)
                {
                    actionChain.EnqueueChain();
                    actionChain = new ActionChain();
                }

                actionChain.AddDelaySeconds(linkTime);
            }

            //log.Info($"{Name}.Reload time: launchTime({launchTime}) + reloadTime({reloadTime}) + linkTime({linkTime})");

            actionChain.AddAction(this, () => EnqueueBroadcast(new GameMessageParentEvent(this, ammo,
                ACE.Entity.Enum.ParentLocation.RightHand, ACE.Entity.Enum.Placement.RightHandCombat)));

            actionChain.EnqueueChain();

            PrevAttackTime = Timers.RunningTime;

            var timeOffset = launchTime + reloadTime + linkTime;

            NextMoveTime = NextAttackTime = PrevAttackTime + timeOffset + MissileDelay;
        }

        /// <summary>
        /// Returns missile base damage from a monster attack
        /// </summary>
        public BaseDamageMod GetMissileDamage()
        {
            // FIXME: use actual projectile, instead of currently equipped ammo
            var ammo = GetMissileAmmo();

            return ammo.GetDamageMod(this);
        }

        // reset between targets?
        public int MonsterProjectile_OnCollideEnvironment_Counter;

        public void MonsterProjectile_OnCollideEnvironment()
        {
            //Console.WriteLine($"{Name}.MonsterProjectile_OnCollideEnvironment()");
            MonsterProjectile_OnCollideEnvironment_Counter++;

            // chance of switching to melee, or static counter in retail?
            /*var rng = ThreadSafeRandom.Next(1, 3);
            if (rng == 3)
                SwitchToMeleeAttack();*/

            if (MonsterProjectile_OnCollideEnvironment_Counter >= 3)
                TrySwitchToMeleeAttack();
        }

        public bool SwitchWeaponsPending;

        public void TrySwitchToMeleeAttack()
        {
            // 24139 - Invisible Assailant never switches to melee?
            if (AiAllowedCombatStyle == CombatStyle.StubbornMissile || Visibility) return;

            SwitchWeaponsPending = true;

            if (NextMoveTime > Timers.RunningTime)
            {
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(NextMoveTime - Timers.RunningTime);
                actionChain.AddAction(this, () => SwitchToMeleeAttack());
                actionChain.EnqueueChain();
            }
            else
                SwitchToMeleeAttack();
        }

        public void SwitchToMeleeAttack()
        {
            if (IsDead) return;

            var weapon = GetEquippedMissileWeapon();
            var ammo = GetEquippedAmmo();

            if (weapon == null && ammo == null)
                return;

            var actionChain = new ActionChain();

            EnqueueMotion_Force(actionChain, MotionStance.NonCombat, MotionCommand.Ready, (MotionCommand)CurrentMotionState.Stance);

            EnqueueMotion_Force(actionChain, MotionStance.HandCombat, MotionCommand.Ready, MotionCommand.NonCombat);

            actionChain.AddAction(this, () =>
            {
                if (IsDead) return;

                // actually destroys the missile weapon + ammo here,
                // to ensure they can't be re-selected from inventory
                if (weapon != null)
                {
                    TryUnwieldObjectWithBroadcasting(weapon.Guid, out _, out _);
                    weapon.Destroy();
                }

                if (ammo != null)
                {
                    TryUnwieldObjectWithBroadcasting(ammo.Guid, out _, out _);
                    ammo.Destroy();
                }

                EquipInventoryItems(true);

                var innerChain = new ActionChain();

                EnqueueMotion_Force(innerChain, MotionStance.NonCombat, MotionCommand.Ready, (MotionCommand)CurrentMotionState.Stance);

                innerChain.AddAction(this, () =>
                {
                    if (IsDead) return;

                    //DoAttackStance();

                    // inlined DoAttackStance() / slightly modified -- do not rely on SetCombatMode() for stance swapping time in 1 action,
                    // as it doesn't support that anymore

                    var newStanceTime = SetCombatMode(CombatMode.Melee);

                    NextMoveTime = NextAttackTime = Timers.RunningTime + newStanceTime;

                    PrevAttackTime = NextMoveTime - (AiUseMagicDelay ?? 3.0f);

                    PhysicsObj.StartTimer();

                    // end inline

                    ResetAttack();

                    SwitchWeaponsPending = false;

                    // this is an unfortunate hack to fix the following scenario:

                    // since this function can be called at any point in time now,
                    // including when LaunchMissile -> EnqueueMotion is in the middle of an action queue,
                    // CurrentMotionState.Stance can get reset to the previous combat stance if that happens

                    var newStance = CurrentMotionState.Stance;

                    var swapChain = new ActionChain();
                    swapChain.AddDelaySeconds(2.0f);
                    swapChain.AddAction(this, () => CurrentMotionState.Stance = newStance);
                    swapChain.EnqueueChain();

                });
                innerChain.EnqueueChain();
            });
            actionChain.EnqueueChain();
        }
    }
}
