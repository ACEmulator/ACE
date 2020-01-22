using System;
using System.Numerics;

using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
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
using ACE.Server.Physics.Extensions;

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
                var animSpeed = GetAnimSpeed();
                //Console.WriteLine($"AnimSpeed: {animSpeed}");

                animLength = EnqueueMotion(actionChain, MotionCommand.Reload, animSpeed);   // start pulling out next arrow
                EnqueueMotion(actionChain, MotionCommand.Ready);    // finish reloading
            }

            // ensure ammo visibility for players
            actionChain.AddAction(this, () =>
            {
                EnqueueActionBroadcast(p => p.TrackEquippedObject(this, ammo));

                var delayChain = new ActionChain();
                delayChain.AddDelaySeconds(0.001f);     // ensuring this message gets sent after player broadcasts above...
                delayChain.AddAction(this, () =>
                {
                    EnqueueBroadcast(new GameMessageParentEvent(this, ammo, ACE.Entity.Enum.ParentLocation.RightHand, ACE.Entity.Enum.Placement.RightHandCombat));
                });
                delayChain.EnqueueChain();
            });

            if (newChain)
                actionChain.EnqueueChain();

            var animLength2 = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.Reload, MotionCommand.Ready);
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
        public WorldObject LaunchProjectile(WorldObject weapon, WorldObject ammo, WorldObject target, out float time)
        {
            var proj = WorldObjectFactory.CreateNewWorldObject(ammo.WeenieClassId);

            proj.ProjectileSource = this;
            proj.ProjectileTarget = target;

            proj.ProjectileLauncher = weapon;

            var localOrigin = GetProjectileSpawnOrigin(proj);

            var dir = GetDir2D(Location.Pos, target.Location.Pos);

            var angle = Math.Atan2(-dir.X, dir.Y);

            var rot = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)angle);

            var origin = Location.Pos + Vector3.Transform(localOrigin, rot);

            var dest = target.Location.Pos;
            dest.Z += target.Height / GetAimHeight(target);

            var speed = 35.0f;  // TODO: get correct speed

            var velocity = GetProjectileVelocity(target, origin, dir, dest, speed, out time);

            var player = this as Player;
            if (!velocity.IsValid())
            {
                if (player != null)
                    player.SendWeenieError(WeenieError.YourAttackMisfired);

                return null;
            }

            proj.Location = new Position(Location);
            proj.Location.Pos = origin;
            proj.Location.Rotation = rot;

            proj.Velocity = velocity;

            SetProjectilePhysicsState(proj, target);

            Console.WriteLine($"Trying to spawn {proj.Name} @ {proj.Location}");

            var success = LandblockManager.AddObject(proj);

            if (!success || proj.PhysicsObj == null)
            {
                if (!proj.HitMsg && player != null)
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat("Your missile attack hit the environment.", ChatMessageType.Broadcast));

                return null;
            }

            var pkStatus = player?.PlayerKillerStatus ?? PlayerKillerStatus.Creature;

            proj.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(proj, PropertyInt.PlayerKillerStatus, (int)pkStatus));
            proj.EnqueueBroadcast(new GameMessageScript(proj.Guid, PlayScript.Launch, 0f));

            // detonate point-blank projectiles immediately
            /*var radsum = target.PhysicsObj.GetRadius() + proj.PhysicsObj.GetRadius();
            var dist = Vector3.Distance(origin, dest);
            if (dist < radsum)
            {
                Console.WriteLine($"Point blank");
                proj.OnCollideObject(target);
            }*/

            return proj;
        }

        public static readonly float ProjSpawnHeight = 0.85f;

        /// <summary>
        /// Returns the origin to spawn the projectile in the attacker local space
        /// </summary>
        public Vector3 GetProjectileSpawnOrigin(WorldObject projectile)
        {
            var attackerRadius = PhysicsObj.GetPhysicsRadius();
            var projectileRadius = GetProjectileRadius(projectile);

            Console.WriteLine($"{Name} radius: {attackerRadius}");
            Console.WriteLine($"{projectile.Name} radius: {projectileRadius}");

            var radsum = attackerRadius * 2.0f + projectileRadius * 2.0f + PhysicsGlobals.EPSILON;

            return new Vector3(0, radsum, Height * ProjSpawnHeight);
        }

                /// <summary>
        /// Returns the cached physics radius for a projectile wcid,
        /// before it has been instantiated as a PhysicsObj
        /// </summary>
        private static float GetProjectileRadius(WorldObject projectile)
        {
            var projectileWcid = projectile.WeenieClassId;

            if (ProjectileRadiusCache.TryGetValue(projectileWcid, out var radius))
                return radius;

            var setup = DatManager.PortalDat.ReadFromDat<SetupModel>(projectile.SetupTableId);

            var scale = projectile.ObjScale ?? 1.0f;

            return ProjectileRadiusCache[projectileWcid] = setup.Spheres[0].Radius * scale;
        }

        /// <summary>
        /// Updates the ammo count or destroys the ammo after launching the projectile.
        /// </summary>
        /// <param name="ammo">The equipped missile ammo object</param>
        public virtual void UpdateAmmoAfterLaunch(WorldObject ammo)
        {
            // hide previously held ammo
            EnqueueBroadcast(new GameMessagePickupEvent(ammo));

            // monsters have infinite ammo?

            /*if (ammo.StackSize == null || ammo.StackSize <= 1)
            {
                TryUnwieldObjectWithBroadcasting(ammo.Guid, out _, out _);
                ammo.Destroy();
            }
            else
            {
                ammo.SetStackSize(ammo.StackSize - 1);
                EnqueueBroadcast(new GameMessageSetStackSize(ammo));
            }*/
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

            var velocity = obj.Velocity;

            obj.PhysicsObj.Velocity = velocity.Value;
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
            var maxVelocity = weapon?.MaximumVelocity ?? DefaultMaxVelocity;

            //var missileRange = (float)Math.Pow(maxVelocity, 2.0f) * 0.1020408163265306f;
            var missileRange = (float)Math.Pow(maxVelocity, 2.0f) * 0.0682547266398198f;

            var strengthMod = SkillFormula.GetAttributeMod((int)Strength.Current);
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
