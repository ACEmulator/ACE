using System;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics.Animation;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// The delay between missile attacks (todo: find actual value)
        /// </summary>
        public static readonly float MissileDelay = 2.0f;

        /// <summary>
        /// Returns TRUE if monster has physical ranged attacks
        /// </summary>
        public bool IsRanged
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
            var weapon = GetEquippedMissileWeapon();
            var ammo = GetEquippedAmmo();

            if (weapon == null || weapon.IsAmmoLauncher && ammo == null) return;

            // simulate accuracy bar / allow client rotate to fully complete
            var actionChain = new ActionChain();
            IsTurning = true;
            actionChain.AddDelaySeconds(0.5f);

            // do missile attack
            actionChain.AddAction(this, LaunchMissile);
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Launches a missile attack from monster to target
        /// </summary>
        public void LaunchMissile()
        {
            IsTurning = false;

            var weapon = GetEquippedMissileWeapon();
            if (weapon == null || AttackTarget == null) return;

            var ammo = weapon.IsAmmoLauncher ? GetEquippedAmmo() : weapon;
            if (ammo == null) return;

            // should this be called each launch?
            AttackHeight = ChooseAttackHeight();

            var dist = GetDistanceToTarget();
            //Console.WriteLine("RangeAttack: " + dist);

            // launch animation
            var actionChain = new ActionChain();
            var launchTime = EnqueueMotion(actionChain, MotionCommand.AimLevel);
            //Console.WriteLine("LaunchTime: " + launchTime);

            // launch projectile
            float targetTime = 0.0f;
            actionChain.AddAction(this, () =>
            {
                var sound = GetLaunchMissileSound(weapon);
                EnqueueBroadcast(new GameMessageSound(Guid, sound, 1.0f));

                // TODO: monster stamina usage

                var projectile = LaunchProjectile(ammo, AttackTarget, out targetTime);
                UpdateAmmoAfterLaunch(ammo);
            });

            // reload animation
            var reloadTime = EnqueueMotion(actionChain, MotionCommand.Reload);
            //Console.WriteLine("ReloadTime: " + reloadTime);

            // reset for next projectile
            EnqueueMotion(actionChain, MotionCommand.Ready);
            var linkTime = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Ready, MotionCommand.Reload);
            //Console.WriteLine("LinkTime: " + linkTime);

            actionChain.AddAction(this, () => EnqueueBroadcast(new GameMessageParentEvent(this, ammo, (int)ACE.Entity.Enum.ParentLocation.RightHand,
                (int)ACE.Entity.Enum.Placement.RightHandCombat)));

            actionChain.EnqueueChain();

            var timeOffset = launchTime + reloadTime + linkTime;

            if (timeOffset < MissileDelay)
                timeOffset = MissileDelay;

            NextAttackTime = Timer.CurrentTime + timeOffset;
        }

        /// <summary>
        /// Returns missile base damage from a monster attack
        /// </summary>
        public Range GetMissileDamage()
        {
            var ammo = GetMissileAmmo();

            return ammo.GetDamageMod(this);
        }
    }
}
