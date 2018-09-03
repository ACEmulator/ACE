using System;
using System.Numerics;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
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
    partial class Creature
    {
        public float ReloadMissileAmmo()
        {
            var weapon = GetEquippedMissileWeapon();
            var ammo = GetEquippedAmmo();

            if (weapon == null || ammo == null) return 0.0f;

            var actionChain = new ActionChain();

            var animLength = 0.0f;
            if (weapon.IsBow)
            {
                EnqueueMotion(actionChain, MotionCommand.Reload);   // start pulling out next arrow
                EnqueueMotion(actionChain, MotionCommand.Ready);    // finish reloading

                animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Reload, MotionCommand.Ready);
                actionChain.AddDelaySeconds(animLength);
            }

            // ensure ammo visibility for players
            actionChain.AddAction(this, () =>
            {
                EnqueueBroadcast(new GameMessageParentEvent(this, ammo, (int)ACE.Entity.Enum.ParentLocation.RightHand, (int)ACE.Entity.Enum.Placement.RightHandCombat));
                EnqueueActionBroadcast((Player p) => p.TrackObject(this));
            });

            actionChain.EnqueueChain();

            var animLength2 = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Ready, MotionCommand.Reload);
            //Console.WriteLine($"AnimLength: {animLength} + {animLength2}");

            return animLength + animLength2;
        }

        /// <summary>
        /// TODO: deprecated
        /// </summary>
        public float ReloadMotion()
        {
            var weapon = GetEquippedMissileWeapon();
            if (weapon == null) return 0.0f;

            var ammo = weapon.IsBow ? GetEquippedAmmo() : weapon;

            var actionChain = new ActionChain();
            var animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Reload);

            var motion = new UniversalMotion(CurrentMotionState.Stance);
            motion.MovementData.CurrentStyle = (uint)CurrentMotionState.Stance;
            motion.MovementData.ForwardCommand = (uint)MotionCommand.Reload;
            motion.MovementData.TurnSpeed = 2.25f;
            //motion.HasTarget = true;
            //motion.TargetGuid = target.Guid;
            CurrentMotionState = motion;

            actionChain.AddAction(this, () => DoMotion(motion));
            actionChain.AddDelaySeconds(animLength);

            actionChain.AddAction(this, () =>
            {
                motion.MovementData.ForwardCommand = (uint)MotionCommand.Invalid;
                DoMotion(motion);
                CurrentMotionState = motion;
            });

            actionChain.AddAction(this, () => EnqueueBroadcast(
                new GameMessageParentEvent(this, ammo, (int)ACE.Entity.Enum.ParentLocation.RightHand,
                    (int)ACE.Entity.Enum.Placement.RightHandCombat)));

            actionChain.AddDelaySeconds(animLength);

            var player = this as Player;
            if (player != null)
            {
                actionChain.AddAction(this, () => player.Session.Network.EnqueueSend(new GameEventAttackDone(player.Session)));
                actionChain.AddAction(this, () => player.Session.Network.EnqueueSend(new GameEventCombatCommmenceAttack(player.Session)));
                // TODO: This gets rid of the hourglass but doesn't seem to be sent in retail pcaps...
                actionChain.AddAction(this, () => player.Session.Network.EnqueueSend(new GameEventAttackDone(player.Session)));
            }
            actionChain.EnqueueChain();

            switch (weapon.DefaultCombatStyle)
            {
                case CombatStyle.Bow:
                    return animLength * 1.6f;
                case CombatStyle.Crossbow:
                    return animLength * 3.2f;
                default:
                    return animLength * 1.0f;
            }
        }

        public Vector3 GetDir2D(Vector3 source, Vector3 dest)
        {
            var diff = dest - source;
            diff.Z = 0;
            return Vector3.Normalize(diff);
        }

        /// <summary>
        /// Launches a projectile from player to target
        /// </summary>
        public WorldObject LaunchProjectile(WorldObject ammo, WorldObject target, out float time)
        {
            var proj = WorldObjectFactory.CreateNewWorldObject(ammo.WeenieClassId);

            proj.ProjectileSource = this;
            proj.ProjectileTarget = target;

            var origin = Location.ToGlobal();
            origin.Z += Height;

            var dest = target.Location.ToGlobal();
            dest.Z += target.Height / GetAimHeight(target);

            var speed = 35.0f;  // TODO: get correct speed
            var dir = GetDir2D(origin, dest);
            origin += dir * 2.0f;

            var velocity = GetProjectileVelocity(target, origin, dir, dest, speed, out time);
            proj.Velocity = new AceVector3(velocity.X, velocity.Y, velocity.Z);

            proj.Location = Location.FromGlobal(origin);

            SetProjectilePhysicsState(proj, target);

            LandblockManager.AddObject(proj);

            var player = this as Player;
            var pkStatus = player == null ? PlayerKillerStatus.Creature : player.PlayerKillerStatus;

            proj.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(proj, PropertyInt.PlayerKillerStatus, (int)pkStatus));
            proj.EnqueueBroadcast(new GameMessageScript(proj.Guid, ACE.Entity.Enum.PlayScript.Launch, 0f));

            // detonate point-blank projectiles immediately
            var radsum = target.PhysicsObj.GetRadius() + proj.PhysicsObj.GetRadius();
            var dist = Vector3.Distance(origin, dest);
            if (dist < radsum)
                proj.OnCollideObject(target);

            return proj;
        }

        /// <summary>
        /// Updates the ammo count or destroys the ammo after launching the projectile.
        /// </summary>
        /// <param name="ammo">The equipped missile ammo object</param>
        public void UpdateAmmoAfterLaunch(WorldObject ammo)
        {
            if (ammo.StackSize == 1)
            {
                TryDequipObject(ammo.Guid);
                EnqueueActionBroadcast(p => p.RemoveTrackedObject(ammo, true));
                TryRemoveFromInventory(ammo.Guid);
            }
            else
            {
                ammo.StackSize--;
                EnqueueBroadcast(new GameMessagePickupEvent(ammo), new GameMessageSetStackSize(ammo));
            }
        }

        /// <summary>
        /// Calculates the velocity to launch the projectile from origin to dest
        /// </summary>
        public Vector3 GetProjectileVelocity(WorldObject target, Vector3 origin, Vector3 dir, Vector3 dest, float speed, out float time, bool useGravity = true)
        {
            var velocity = dir * speed;

            time = 0.0f;
            Vector3 s0, s1;
            float t0, t1;

            var gravity = useGravity ? -PhysicsGlobals.Gravity : 0.00001f;

            var targetVelocity = target.PhysicsObj.CachedVelocity;
            if (!targetVelocity.Equals(Vector3.Zero))
            {
                // use movement quartic solver
                var numSolutions = Trajectory.solve_ballistic_arc(origin, speed, dest, targetVelocity, gravity, out s0, out s1, out time);

                if (numSolutions > 0)
                    return s0;
            }

            // use stationary solver
            Trajectory.solve_ballistic_arc(origin, speed, dest, gravity, out s0, out s1, out t0, out t1);

            time = t0;
            return s0;
        }

        /// <summary>
        /// Sets the physics state for a launched projectile
        /// </summary>
        public void SetProjectilePhysicsState(WorldObject obj, WorldObject target)
        {
            obj.InitPhysicsObj();

            obj.ReportCollisions = true;
            obj.Missile = true;
            obj.AlignPath = true;
            obj.PathClipped = true;
            obj.Ethereal = false;
            obj.IgnoreCollisions = false;

            var pos = obj.Location.Pos;
            var rotation = obj.Location.Rotation;
            obj.PhysicsObj.Position.Frame.Origin = pos;
            obj.PhysicsObj.Position.Frame.Orientation = rotation;
            obj.Placement = ACE.Entity.Enum.Placement.MissileFlight;
            obj.CurrentMotionState = null;

            var velocity = obj.Velocity.Get();

            obj.PhysicsObj.Velocity = velocity;
            obj.PhysicsObj.ProjectileTarget = target.PhysicsObj;

            obj.PhysicsObj.set_active(true);
        }

        public Sound GetLaunchMissileSound(WorldObject weapon)
        {
            switch (weapon.DefaultCombatStyle)
            {
                case CombatStyle.Bow:
                    return Sound.BowRelease;
                case CombatStyle.Crossbow:
                    return Sound.CrossbowRelease;
                default:
                    return Sound.ThrownWeaponRelease1;
            }
        }
    }
}
