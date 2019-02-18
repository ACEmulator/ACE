using System;
using System.Numerics;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics;
using ACE.Server.Physics.Animation;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public float ReloadMissileAmmo(ActionChain actionChain = null)
        {
            var weapon = GetEquippedMissileWeapon();
            var ammo = GetEquippedAmmo();

            if (weapon == null || ammo == null) return 0.0f;

            var newChain = actionChain == null;
            if (newChain)
                actionChain = new ActionChain();

            var animLength = 0.0f;
            if (weapon.IsAmmoLauncher)
            {
                animLength = EnqueueMotion(actionChain, MotionCommand.Reload);   // start pulling out next arrow
                EnqueueMotion(actionChain, MotionCommand.Ready);    // finish reloading
            }

            // ensure ammo visibility for players
            actionChain.AddAction(this, () =>
            {
                EnqueueActionBroadcast(p => p.TrackEquippedObject(this, ammo));
                EnqueueBroadcast(new GameMessageParentEvent(this, ammo, (int)ACE.Entity.Enum.ParentLocation.RightHand, (int)ACE.Entity.Enum.Placement.RightHandCombat));
            });

            if (newChain)
                actionChain.EnqueueChain();

            var animLength2 = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Reload, MotionCommand.Ready);
            //Console.WriteLine($"AnimLength: {animLength} + {animLength2}");

            return animLength + animLength2;
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

            var matchIndoors = Location.Indoors == target.Location.Indoors;
            var origin = matchIndoors ? Location.ToGlobal() : Location.Pos;
            origin.Z += Height;

            var dest = matchIndoors ? target.Location.ToGlobal() : target.Location.Pos;
            dest.Z += target.Height / GetAimHeight(target);

            var speed = 35.0f;  // TODO: get correct speed
            var dir = GetDir2D(origin, dest);
            origin += dir * 2.0f;

            var velocity = GetProjectileVelocity(target, origin, dir, dest, speed, out time);
            proj.Velocity = new AceVector3(velocity.X, velocity.Y, velocity.Z);

            proj.Location = matchIndoors ? Location.FromGlobal(origin) : new Position(Location.Cell, origin, Location.Rotation);
            if (!matchIndoors)
                proj.Location.LandblockId = new LandblockId(proj.Location.GetCell());

            SetProjectilePhysicsState(proj, target);

            LandblockManager.AddObject(proj);

            var player = this as Player;
            var pkStatus = player?.PlayerKillerStatus ?? PlayerKillerStatus.Creature;

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
            ammo.SetStackSize(ammo.StackSize - 1);

            if (ammo.StackSize == 0)
            {
                if (this is Player player)
                    player.TryDequipObjectWithNetworking(ammo.Guid, out _, Player.DequipObjectAction.ConsumeItem);
                else
                {
                    TryUnwieldObjectWithBroadcasting(ammo.Guid, out _, out _);
                    ammo.Destroy();
                }
            }
            else
            {
                EnqueueBroadcast(new GameMessagePickupEvent(ammo), new GameMessageSetStackSize(ammo));
            }
        }

        /// <summary>
        /// Calculates the velocity to launch the projectile from origin to dest
        /// </summary>
        public Vector3 GetProjectileVelocity(WorldObject target, Vector3 origin, Vector3 dir, Vector3 dest, float speed, out float time, bool useGravity = true)
        {
            time = 0.0f;
            Vector3 s0;
            float t0;

            var gravity = useGravity ? -PhysicsGlobals.Gravity : 0.00001f;

            var targetVelocity = target.PhysicsObj.CachedVelocity;
            if (!targetVelocity.Equals(Vector3.Zero))
            {
                // use movement quartic solver
                var numSolutions = Trajectory.solve_ballistic_arc(origin, speed, dest, targetVelocity, gravity, out s0, out _, out time);

                if (numSolutions > 0)
                    return s0;
            }

            // use stationary solver
            Trajectory.solve_ballistic_arc(origin, speed, dest, gravity, out s0, out _, out t0, out _);

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

        public static readonly float MetersToYards = 1.094f;    // 1.09361
        public static readonly float MissileRangeCap = 85.0f / MetersToYards;   // 85 yards = ~77.697 meters w/ ac formula
        public static readonly float DefaultMaxVelocity = 20.0f;    // ?

        public float GetMaxMissileRange()
        {
            var weapon = GetEquippedWeapon();
            var maxVelocity = weapon != null ? weapon.GetProperty(PropertyFloat.MaximumVelocity) ?? DefaultMaxVelocity : DefaultMaxVelocity;

            //var missileRange = (float)Math.Pow(maxVelocity, 2.0f) * 0.1020408163265306f;
            var missileRange = (float)Math.Pow(maxVelocity, 2.0f) * 0.0682547266398198f;

            var strengthMod = SkillFormula.GetAttributeMod(PropertyAttribute.Strength, (int)Strength.Current);
            var maxRange = Math.Min(missileRange * strengthMod, MissileRangeCap);

            // any kind of other caps for monsters specifically?
            // throwing lugian rocks @ 85 yards seems a bit far...

            //Console.WriteLine($"{Name}.GetMaxMissileRange(): maxVelocity={maxVelocity}, strengthMod={strengthMod}, maxRange={maxRange}");

            // for client display
            /*var maxRangeYards = maxRange * MetersToYards;
            if (maxRangeYards >= 10.0f)
                maxRangeYards -= maxRangeYards % 5.0f;
            else
                maxRangeYards = (float)Math.Ceiling(maxRangeYards);*/

            return maxRange;
        }
    }
}
