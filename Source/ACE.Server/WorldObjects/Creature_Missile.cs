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
using PhysicsState = ACE.Server.Physics.PhysicsState;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
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

            var ammo = GetEquippedAmmo();
            if (ammo != null)
                actionChain.AddAction(this, () => CurrentLandblock.EnqueueBroadcast(Location,
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

        public Vector3 GetDir2D(Vector3 source, Vector3 dest)
        {
            var diff = dest - source;
            diff.Z = 0;
            return Vector3.Normalize(diff);
        }

        /// <summary>
        /// Launches a projectile from player to target
        /// </summary>
        public WorldObject LaunchProjectile(WorldObject target, out float time)
        {
            var ammo = GetEquippedAmmo();
            var arrow = WorldObjectFactory.CreateNewWorldObject(ammo.WeenieClassId);

            arrow.ProjectileSource = this;
            arrow.ProjectileTarget = target;

            var origin = Location.GlobalPos;
            origin.Z += Height;

            var dest = target.Location.GlobalPos;
            //var dest = target.Location.Pos;
            dest.Z += target.Height / GetAimHeight(target);

            var speed = 35.0f;
            var dir = GetDir2D(origin, dest);
            origin += dir * 2.0f;

            var velocity = GetProjectileVelocity(target, origin, dir, dest, speed, out time);
            arrow.Velocity = new AceVector3(velocity.X, velocity.Y, velocity.Z);

            origin = Location.FromGlobal(origin).Pos;
            var rotation = Location.Rotation;
            arrow.Location = new Position(Location.LandblockId.Raw, origin.X, origin.Y, origin.Z, rotation.X, rotation.Y, rotation.Z, rotation.W);
            SetProjectilePhysicsState(arrow, target);

            // TODO: Get correct aim level based on arrow velocity and add aim motion delay.
            var motion = new UniversalMotion(CurrentMotionState.Stance);
            motion.MovementData.CurrentStyle = (uint)CurrentMotionState.Stance;
            motion.MovementData.ForwardCommand = (uint)MotionCommand.AimLevel;
            CurrentMotionState = motion;

            DoMotion(motion);
            //actionChain.AddDelaySeconds(animLength);

            LandblockManager.AddObject(arrow);
            CurrentLandblock.EnqueueBroadcast(arrow.Location, new GameMessagePickupEvent(ammo));

            var player = this as Player;
            // TODO: Add support for monster ammo depletion. For now only players will use up ammo.
            if (player != null)
                UpdateAmmoAfterLaunch(ammo);
            // Not sure why this would be needed but it is sent in retail pcaps.
            CurrentLandblock.EnqueueBroadcast(arrow.Location, new GameMessageSetStackSize(arrow));

            if (player != null)
            {
                CurrentLandblock.EnqueueBroadcast(arrow.Location, new GameMessagePublicUpdatePropertyInt(
                    arrow, PropertyInt.PlayerKillerStatus, (int)(player.PlayerKillerStatus ?? ACE.Entity.Enum.PlayerKillerStatus.NPK) ));
            }
            else
            {
                CurrentLandblock.EnqueueBroadcast(arrow.Location, new GameMessagePublicUpdatePropertyInt(
                    arrow, PropertyInt.PlayerKillerStatus, (int)ACE.Entity.Enum.PlayerKillerStatus.Creature));
            }
            
            CurrentLandblock.EnqueueBroadcast(arrow.Location, new GameMessageScript(arrow.Guid, ACE.Entity.Enum.PlayScript.Launch, 0f));

            // detonate point-blank projectiles immediately
            var radsum = target.PhysicsObj.GetRadius() + arrow.PhysicsObj.GetRadius();
            var dist = Vector3.Distance(origin, dest);
            if (dist < radsum)
                arrow.OnCollideObject(target);

            return arrow;
        }

        /// <summary>
        /// Updates the ammo count or destroys the ammo after launching the projectile.
        /// </summary>
        /// <param name="ammo">The missile ammo object</param>
        public void UpdateAmmoAfterLaunch(WorldObject ammo)
        {
            var player = this as Player;

            if (ammo.StackSize == 1)
            {
                TryDequipObject(ammo.Guid);
                player?.Session.Network.EnqueueSend(new GameMessageDeleteObject(ammo));
                CurrentLandblock.EnqueueActionBroadcast(Location, Landblock.MaxObjectRange, p => p.StopTrackingObject(ammo, true));
            }
            else
            {
                ammo.StackSize--;
                player?.Session.Network.EnqueueSend(new GameMessageSetStackSize(ammo));
                CurrentLandblock.EnqueueBroadcast(Location, new GameMessagePickupEvent(ammo));
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
                var numSolutions = Trajectory.solve_ballistic_arc(origin, speed, dest, targetVelocity, gravity, out s0, out s1);

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
            obj.ReportCollisions = true;
            obj.Missile = true;
            obj.AlignPath = true;
            obj.PathClipped = true;
            obj.Ethereal = false;
            obj.IgnoreCollisions = false;

            obj.PhysicsObj.State |= PhysicsState.ReportCollisions | PhysicsState.Missile | PhysicsState.AlignPath | PhysicsState.PathClipped;
            obj.PhysicsObj.State &= ~(PhysicsState.Ethereal | PhysicsState.IgnoreCollisions);

            var pos = obj.Location.Pos;
            var rotation = obj.Location.Rotation;
            obj.PhysicsObj.Position.Frame.Origin = pos;
            obj.PhysicsObj.Position.Frame.Orientation = rotation;
            obj.Placement = ACE.Entity.Enum.Placement.MissileFlight;
            obj.CurrentMotionState = null;

            var velocity = obj.Velocity.Get();
            velocity = Vector3.Transform(velocity, Matrix4x4.Transpose(Matrix4x4.CreateFromQuaternion(rotation)));
            obj.PhysicsObj.Velocity = velocity;
            obj.PhysicsObj.ProjectileTarget = target.PhysicsObj;

            obj.PhysicsObj.set_active(true);
        }
    }
}
