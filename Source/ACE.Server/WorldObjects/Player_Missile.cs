using System;
using System.Numerics;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Physics;
using ACE.Server.Physics.Animation;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public float AccuracyLevel;

        public WorldObject MissileTarget;

        /// <summary>
        /// Called by network packet handler 0xA - GameActionTargetedMissileAttack
        /// </summary>
        /// <param name="guid">The target guid</param>
        /// <param name="attackHeight">The attack height 1-3</param>
        /// <param name="accuracyLevel">The 0-1 accuracy bar level</param>
        public void HandleActionTargetedMissileAttack(ObjectGuid guid, uint attackHeight, float accuracyLevel)
        {
            // sanity check
            accuracyLevel = Math.Clamp(accuracyLevel, 0.0f, 1.0f);

            if (GetEquippedAmmo() == null) return;

            AttackHeight = (AttackHeight)attackHeight;
            AccuracyLevel = accuracyLevel;

            // get world object of target guid
            var target = CurrentLandblock.GetObject(guid);
            if (target == null)
            {
                log.Warn("Unknown target guid " + guid.Full.ToString("X8"));
                return;
            }
            if (MissileTarget == null)
                MissileTarget = target;
            else
                return;

            // turn if required
            var rotateTime = Rotate(target);
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(rotateTime);

            // do missile attack
            actionChain.AddAction(this, () => LaunchMissile(target));
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Launches a missile attack from player to target
        /// </summary>
        public void LaunchMissile(WorldObject target)
        {
            if (GetEquippedAmmo() == null || CombatMode == CombatMode.NonCombat)
                return;

            var creature = target as Creature;
            if (MissileTarget == null || creature.Health.Current <= 0)
            {
                MissileTarget = null;
                return;
            }

            var weapon = GetEquippedWeapon();
            var sound = weapon.DefaultCombatStyle == CombatStyle.Crossbow ? Sound.CrossbowRelease : Sound.BowRelease;
            CurrentLandblock.EnqueueBroadcast(Location, new GameMessageSound(Guid, sound, 1.0f));

            float targetTime = 0.0f;
            var damageSource = LaunchProjectile(target, out targetTime);

            // todo: get correct animlengths for shoot + reload + aim
            var animLength = ReloadMotion() * 2.5f;

            var actionChain = new ActionChain();
            //actionChain.AddDelaySeconds(targetTime);
            //actionChain.AddAction(this, () => DamageTarget(target, damageSource));

            if (creature.Health.Current > 0 && GetCharacterOption(CharacterOption.AutoRepeatAttacks))
            {
                // reload animation, accuracy bar refill
                actionChain.AddDelaySeconds(animLength + AccuracyLevel);
                actionChain.AddAction(this, () => { LaunchMissile(target); });
                actionChain.EnqueueChain();
            }
            else
                MissileTarget = null;
        }

        public override float GetAimHeight(WorldObject target)
        {
            switch (AttackHeight)
            {
                case AttackHeight.High: return 1.0f;
                case AttackHeight.Medium: return 2.0f;
                //case AttackHeight.Low: return target.Height;
                case AttackHeight.Low: return 3.0f;
            }
            return 2.0f;
        }
    }
}
