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
                log.Warn("Unknown target guid " + guid);
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
            var creature = target as Creature;
            if (MissileTarget == null || creature.Health.Current <= 0)
            {
                MissileTarget = null;
                return;
            }

            var weapon = GetEquippedWeapon();
            var sound = weapon.DefaultCombatStyle == CombatStyle.Crossbow ? Sound.CrossbowRelease : Sound.BowRelease;
            Session.Network.EnqueueSend(new GameMessageSound(Guid, sound, 1.0f));

            float targetTime = 0.0f;
            targetTime = LaunchProjectile(target);
            var animLength = ReloadMotion();

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(targetTime);
            actionChain.AddAction(this, () => DamageTarget(target));
            if (creature.Health.Current > 0 && GetCharacterOption(CharacterOption.AutoRepeatAttacks))
            {
                // reload animation, accuracy bar refill
                actionChain.AddDelaySeconds(animLength + AccuracyLevel);
                actionChain.AddAction(this, () => { LaunchMissile(target); });
            }
            else
                MissileTarget = null;

            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Executes the weapon reload animation for the player
        /// </summary>
        public float ReloadMotion()
        {
            var reloadAnimation = new MotionItem(GetReloadAnimation(), 1.0f);
            var animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, reloadAnimation);

            var motion = new UniversalMotion(CurrentMotionState.Stance);
            motion.MovementData.CurrentStyle = (uint)CurrentMotionState.Stance;
            motion.MovementData.ForwardCommand = (uint)MotionCommand.Reload;
            motion.MovementData.TurnSpeed = 2.25f;
            //motion.HasTarget = true;
            //motion.TargetGuid = target.Guid;
            CurrentMotionState = motion;

            var actionChain = new ActionChain();
            actionChain.AddAction(this, () => DoMotion(motion));
            actionChain.AddDelaySeconds(animLength);
            actionChain.AddAction(this, () =>
            {
                motion.MovementData.ForwardCommand = (uint)MotionCommand.Invalid;
                DoMotion(motion);
                CurrentMotionState = motion;
            });
            actionChain.AddDelaySeconds(animLength);
            actionChain.AddAction(this, () => Session.Network.EnqueueSend(new GameEventAttackDone(Session)));
            actionChain.AddAction(this, () => Session.Network.EnqueueSend(new GameEventCombatCommmenceAttack(Session)));
            actionChain.AddAction(this, () => Session.Network.EnqueueSend(new GameEventAttackDone(Session)));
            actionChain.EnqueueChain();

            var weapon = GetEquippedWeapon();
            var reloadTime = weapon.DefaultCombatStyle == CombatStyle.Crossbow ? 3.2f : 1.6f;
            return animLength * reloadTime;
        }

        /// <summary>
        /// Gets the reload animation for the current weapon
        /// </summary>
        public MotionCommand GetReloadAnimation()
        {
            MotionCommand motion = new MotionCommand();

            switch (CurrentMotionState.Stance)
            {
                case MotionStance.BowAttack:
                case MotionStance.CrossBowAttack:
                    var action = "Reload";
                    Enum.TryParse(action, out motion);
                    return motion;
            }
            return motion;
        }

        /// <summary>
        /// Launches a projectile from player to target
        /// </summary>
        public float LaunchProjectile(WorldObject target)
        {
            var ammo = GetEquippedAmmo();
            var arrow = WorldObjectFactory.CreateNewWorldObject(ammo.WeenieClassId);

            var origin = Location.ToGlobal();
            origin.Z += Height;

            var dest = target.Location.ToGlobal();
            dest.Z += target.Height / GetAimHeight(target);

            var speed = 35.0f;
            var dir = Vector3.Normalize(target.Location.Pos - Location.Pos);

            origin += dir * 2.0f;

            arrow.Velocity = GetProjectileVelocity(target, origin, dir, dest, speed, out var time);

            var loc = Location;
            arrow.Location = new Position(loc.LandblockId.Raw, origin.X, origin.Y, origin.Z, loc.Rotation.X, loc.Rotation.Y, loc.Rotation.Z, loc.RotationW);
            SetProjectilePhysicsState(arrow);

            LandblockManager.AddObject(arrow);
            CurrentLandblock.EnqueueBroadcast(arrow.Location, new GameMessageScript(arrow.Guid, ACE.Entity.Enum.PlayScript.Launch, 1.0f));

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(time);
            actionChain.AddAction(arrow, () => Session.Network.EnqueueSend(new GameMessageSound(arrow.Guid, Sound.Collision, 1.0f)));
            actionChain.AddAction(arrow, () => CurrentLandblock.RemoveWorldObject(arrow.Guid, false));
            actionChain.EnqueueChain();

            return time;
        }

        /// <summary>
        /// Calculates the velocity to launch the projectile from origin to dest
        /// </summary>
        public AceVector3 GetProjectileVelocity(WorldObject target, Vector3 origin, Vector3 dir, Vector3 dest, float speed, out float time)
        {
            var velocity = dir * speed;

            Vector3 s0, s1;
            float t0, t1;
            Trajectory.solve_ballistic_arc(origin, speed, dest, -PhysicsGlobals.Gravity, out s0, out s1, out t0, out t1);

            /*Console.WriteLine("s0: " + s0);
            Console.WriteLine("s1: " + s1);
            Console.WriteLine("t0: " + t0);
            Console.WriteLine("t1: " + t1);*/

            time = t0 + target.PhysicsObj.GetRadius() / speed;
            return new AceVector3(s0.X, s0.Y, s0.Z);
        }

        /// <summary>
        /// Sets the physics state for a launched projectile
        /// </summary>
        public void SetProjectilePhysicsState(WorldObject obj)
        {
            obj.ReportCollisions = true;
            obj.Missile = true;
            obj.AlignPath = true;
            obj.PathClipped = true;
            obj.Ethereal = false;
            obj.IgnoreCollisions = false;
        }

        public float GetAimHeight(WorldObject target)
        {
            switch (AttackHeight)
            {
                case AttackHeight.High: return 1.0f;
                case AttackHeight.Medium: return 2.0f;
                case AttackHeight.Low: return target.Height;
            }
            return 2.0f;
        }
    }
}
