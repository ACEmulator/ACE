using System;
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
        public new bool IsRanged
        {
            get
            {
                var weapon = GetEquippedMissileWeapon();
                return weapon != null;
            }
        }

        /// <summary>
        /// Starts a monster missile attack
        /// </summary>
        public void RangeAttack()
        {
            var target = AttackTarget as Creature;

            if (target == null || !target.IsAlive)
            {
                Sleep();
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

            // ensure direct line of sight
            if (!IsDirectVisible(AttackTarget))
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

            // launch animation
            var actionChain = new ActionChain();
            var launchTime = EnqueueMotion(actionChain, MotionCommand.AimLevel);
            //Console.WriteLine("LaunchTime: " + launchTime);

            // launch projectile
            float targetTime = 0.0f;
            actionChain.AddAction(this, () =>
            {
                if (IsDead) return;

                var sound = GetLaunchMissileSound(weapon);
                EnqueueBroadcast(new GameMessageSound(Guid, sound, 1.0f));

                // TODO: monster stamina usage

                if (AttackTarget != null)
                {
                    var projectile = LaunchProjectile(ammo, AttackTarget, out targetTime);
                    UpdateAmmoAfterLaunch(ammo);
                }
            });

            // will ammo be depleted?
            if (ammo.StackSize == 1)
            {
                // compare monsters: lugianmontokrenegade /  sclavusse / zombielichtowerarcher
                actionChain.EnqueueChain();
                NextMoveTime = NextAttackTime = Timers.RunningTime + launchTime + MissileDelay;
                return;
            }

            // reload animation
            var reloadTime = EnqueueMotion(actionChain, MotionCommand.Reload);
            //Console.WriteLine("ReloadTime: " + reloadTime);

            // reset for next projectile
            EnqueueMotion(actionChain, MotionCommand.Ready);

            var linkTime = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Reload, MotionCommand.Ready);

            if (weapon.IsThrownWeapon)
            {
                actionChain.EnqueueChain();

                actionChain = new ActionChain();
                actionChain.AddDelaySeconds(linkTime);
            }
            //Console.WriteLine($"Reload time: launchTime({launchTime}) + reloadTime({reloadTime}) + linkTime({linkTime})");

            actionChain.AddAction(this, () => EnqueueBroadcast(new GameMessageParentEvent(this, ammo, (int)ACE.Entity.Enum.ParentLocation.RightHand,
                    (int)ACE.Entity.Enum.Placement.RightHandCombat)));

            actionChain.EnqueueChain();

            var timeOffset = launchTime + reloadTime + linkTime;

            var missileDelay = MissileDelay;
            if (!weapon.IsAmmoLauncher)
                missileDelay *= 1.5f;

            NextMoveTime = NextAttackTime = Timers.RunningTime + timeOffset + missileDelay;
        }

        /// <summary>
        /// Returns missile base damage from a monster attack
        /// </summary>
        public Range GetMissileDamage()
        {
            // FIXME: use actual projectile, instead of currently equipped ammo
            var ammo = GetMissileAmmo();

            return ammo.GetDamageMod(this);
        }
    }
}
