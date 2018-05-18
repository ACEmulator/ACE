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

            actionChain.AddDelaySeconds(animLength);

            var player = this as Player;
            if (player != null)
            {
                actionChain.AddAction(this, () => player.Session.Network.EnqueueSend(new GameEventAttackDone(player.Session)));
                actionChain.AddAction(this, () => player.Session.Network.EnqueueSend(new GameEventCombatCommmenceAttack(player.Session)));
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
            var dir = Vector3.Normalize(dest - origin);

            origin += dir * 2.0f;

            arrow.Velocity = GetProjectileVelocity(target, origin, dir, dest, speed, out var time);

            var loc = Location;
            origin = Position.FromGlobal(origin).Pos;
            arrow.Location = new Position(loc.LandblockId.Raw, origin.X, origin.Y, origin.Z, loc.Rotation.X, loc.Rotation.Y, loc.Rotation.Z, loc.RotationW);
            SetProjectilePhysicsState(arrow);

            LandblockManager.AddObject(arrow);
            CurrentLandblock.EnqueueBroadcast(arrow.Location, new GameMessageScript(arrow.Guid, ACE.Entity.Enum.PlayScript.Launch, 1.0f));

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(time);

            // todo: landblock broadcast?
            var player = this as Player;
            if (player != null)
                actionChain.AddAction(arrow, () => player.Session.Network.EnqueueSend(new GameMessageSound(arrow.Guid, Sound.Collision, 1.0f)));

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
    }
}
