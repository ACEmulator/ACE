using System;
using System.Numerics;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
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

            AttackHeight = (AttackHeight)attackHeight;
            AccuracyLevel = accuracyLevel;

            // get world object of target guid
            var target = CurrentLandblock.GetObject(guid);
            if (target == null)
            {
                log.Warn("Unknown target guid " + guid);
                return;
            }
            //if (MissileTarget == null)
                MissileTarget = target;
            //else
                //return;

            // turn if required
            Rotate();

            // do missile attack
            LaunchMissile(target);
        }

        /// <summary>
        /// Launches a missile attack from player to target
        /// </summary>
        public void LaunchMissile(WorldObject target)
        {
            if (MissileTarget == null)
                return;

            var actionChain = new ActionChain();
            float targetTime = 0.0f;
            actionChain.AddAction(this, () =>
            {
                Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.BowRelease, 1.0f));
                targetTime = LaunchProjectile(target);
                ReloadMotion(actionChain);
                actionChain.AddDelaySeconds(targetTime);
                actionChain.AddAction(this, () => DamageTarget(target));
            });
            //actionChain.AddAction(this, () => Session.Network.EnqueueSend(new GameEventAttackDone(Session)));
            //actionChain.AddAction(this, () => Session.Network.EnqueueSend(new GameEventCombatCommmenceAttack(Session)));
            //actionChain.AddAction(this, () => Session.Network.EnqueueSend(new GameEventAttackDone(Session)));
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Appends the reload animation to an action chain
        /// </summary>
        public void ReloadMotion(ActionChain actionChain)
        {
            var reloadAnimation = new MotionItem(GetReloadAnimation(), 1.25f);
            var animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, reloadAnimation);

            var motion = new UniversalMotion(CurrentMotionState.Stance);
            motion.MovementData.CurrentStyle = (uint)CurrentMotionState.Stance;
            motion.MovementData.ForwardCommand = (uint)MotionCommand.Reload;
            motion.MovementData.TurnSpeed = 2.25f;
            //motion.HasTarget = true;
            //motion.TargetGuid = target.Guid;
            CurrentMotionState = motion;

            actionChain.AddAction(this, () => DoMotion(motion));
            actionChain.AddDelaySeconds(0.25f);
            actionChain.AddAction(this, () =>
            {
                motion.MovementData.ForwardCommand = (uint)MotionCommand.Invalid;
                DoMotion(motion);
                CurrentMotionState = motion;
            });
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

            var origin = Location.InFrontOf().Pos;
            origin.Z += Height / 2.0f;

            var dest = target.Location.Pos;
            dest.Z += target.Height / 2.0f;

            var speed = 35.0f;
            //var dir = GetAngle(target);
            var dir = Location.GetCurrentDir();

            arrow.Velocity = GetProjectileVelocity(target, origin, dir, dest, speed, out var time);

            var loc = Location;
            arrow.Location = new Position(loc.LandblockId.Raw, origin.X, origin.Y, origin.Z, loc.Rotation.X, loc.Rotation.Y, loc.Rotation.Z, loc.RotationW);
            SetProjectilePhysicsState(arrow);

            LandblockManager.AddObject(arrow);
            CurrentLandblock.EnqueueBroadcast(arrow.Location, new GameMessageScript(arrow.Guid, ACE.Entity.Enum.PlayScript.Launch, 1.0f));

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

            time = t0 - target.PhysicsObj.GetRadius() / speed;
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
    }
}
