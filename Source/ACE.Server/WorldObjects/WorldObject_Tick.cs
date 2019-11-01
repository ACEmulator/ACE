using System;
using System.Diagnostics;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Physics;
using ACE.Server.Physics.Common;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        // Used for cumulative ServerPerformanceMonitor event recording
        protected readonly Stopwatch stopwatch = new Stopwatch();

        private const int heartbeatSpreadInterval = 5;

        protected double CachedHeartbeatInterval;
        /// <summary>
        /// A value of Double.MaxValue indicates that there is no NextHeartbeat
        /// </summary>
        public double NextHeartbeatTime;

        private double cachedRegenerationInterval;
        /// <summary>
        /// A value of Double.MaxValue indicates that there is no NextGeneratorHeartbeat
        /// </summary>
        public double NextGeneratorUpdateTime;
        /// <summary>
        /// A value of Double.MaxValue indicates that there is no NextGeneratorRegeneration
        /// </summary>
        public double NextGeneratorRegenerationTime;

        private void InitializeHeartbeats()
        {
            var currentUnixTime = Time.GetUnixTime();

            if (WeenieType == WeenieType.GamePiece)
                HeartbeatInterval = 1.0f;

            if (HeartbeatInterval == null)
                HeartbeatInterval = 5.0f;

            if (RegenerationInterval < 0)
            {
                log.Warn($"{Name} ({Guid}).InitializeHeartBeats() - RegenerationInterval {RegenerationInterval}, setting to 0");
                RegenerationInterval = 0;
            }

            CachedHeartbeatInterval = HeartbeatInterval ?? 0;

            if (CachedHeartbeatInterval > 0)
            {
                // The intention of this code was just to spread the heartbeat ticks out a little over a 0-5s range,
                var delay = ThreadSafeRandom.Next(0.0f, heartbeatSpreadInterval);

                NextHeartbeatTime = currentUnixTime + delay;
            }
            else
            {
                NextHeartbeatTime = double.MaxValue; // Disable future HeartBeats
            }

            cachedRegenerationInterval = RegenerationInterval;

            if (IsGenerator)
            {
                NextGeneratorUpdateTime = currentUnixTime; // Generators start right away
                if (cachedRegenerationInterval == 0)
                    NextGeneratorRegenerationTime = double.MaxValue;
            }
            else
            {
                NextGeneratorUpdateTime = double.MaxValue; // Disable future GeneratorHeartBeats
                NextGeneratorRegenerationTime = double.MaxValue;
            }
        }

        /// <summary>
        /// Should only be used by Landblocks
        /// </summary>
        public void ReinitializeHeartbeats()
        {
            InitializeHeartbeats();
        }

        /// <summary>
        /// Called every ~5 seconds for WorldObject base
        /// </summary>
        public virtual void Heartbeat(double currentUnixTime)
        {
            if (EnchantmentManager.HasEnchantments)
                EnchantmentManager.HeartBeat(CachedHeartbeatInterval);

            if (RemainingLifespan != null)
            {
                RemainingLifespan -= (int)CachedHeartbeatInterval;

                if (RemainingLifespan <= 0)
                    DeleteObject();
            }

            SetProperty(PropertyFloat.HeartbeatTimestamp, currentUnixTime);
            NextHeartbeatTime = currentUnixTime + CachedHeartbeatInterval;
        }

        /// <summary>
        /// Called every 5 seconds for WorldObject base
        /// </summary>
        public void GeneratorUpdate(double currentUnixTime)
        {
            Generator_Update();

            SetProperty(PropertyFloat.GeneratorUpdateTimestamp, currentUnixTime);

            NextGeneratorUpdateTime = currentUnixTime + 5;
        }

        /// <summary>
        /// Called every [RegenerationInterval] seconds for WorldObject base
        /// </summary>
        public void GeneratorRegeneration(double currentUnixTime)
        {
            //Console.WriteLine($"{Name}.GeneratorRegeneration({currentUnixTime})");

            Generator_Regeneration();

            SetProperty(PropertyFloat.RegenerationTimestamp, currentUnixTime);

            if (cachedRegenerationInterval > 0)
                NextGeneratorRegenerationTime = currentUnixTime + cachedRegenerationInterval;

            //Console.WriteLine($"{Name}.NextGeneratorRegenerationTime({NextGeneratorRegenerationTime})");
        }

        /// <summary>
        /// Enqueue work to be done on this objects Landblock..<para />
        /// If this is a detached creature, the work will be discarded.<para />
        /// If this is a detached non-creature object, the work will be enqueued onto WorldManager.
        /// </summary>
        public virtual void EnqueueAction(IAction action)
        {
            if (CurrentLandblock == null)
            {
                if (IsDestroyed)
                {
                    // Item is gone, no more work can be done to it
                }
                else if (decayCompleted)
                {
                    // Item probably completed decay and started the fade-out process right before the landblock unloaded.
                }
                else if (this is Creature)
                {
                    // If we've hit this point, something is asking to add work to a detached creature
                    // It's likely a DelayManager processing a NextAct() action, and that action is likely an emote.
                    // We don't need to emote Creatures.
                    // If we find that there is a case where Creatures need to act after they've been detached from the landblock,
                    // that work should be enqueued onto WorldManager
                }
                else if (this is SpellProjectile)
                {
                    // Do no more work for detached spell projectiles
                }
                else if (IsGenerator)
                {
                    // This is a detached generator, we don't need to further the action chain
                }
                else
                {
                    // Enqueue work for detached objects onto our thread-safe WorldManager

                    // Here we filter out warnings for known cases where work may be queued onto an item that doesn't exist on a landblock
                    if (this is SlumLord)
                    {
                        // Slumlords (housing) can be loaded without its landblock
                    }
                    else if (this is Container container && !container.InventoryLoaded)
                    {
                        // Containers enqueue the loading of their inventory from callbacks. It's possible the callback happened before the container was added to the landblock
                    }
                    else
                    {
                        if (!(OwnerId.HasValue && OwnerId.Value > 0))
                            log.WarnFormat("Item 0x{0:X8}:{1} has enqueued an action but is not attached to a landblock.", Guid.Full, Name);
                    }

                    WorldManager.EnqueueAction(action);
                }
            }
            else
                CurrentLandblock.EnqueueAction(action);
        }


        public uint prevCell;
        public bool InUpdate;

        /// <summary>
        /// Used by physics engine to actually update a player position
        /// Automatically notifies clients of updated position
        /// </summary>
        /// <param name="newPosition">The new position being requested, before verification through physics engine</param>
        /// <returns>TRUE if object moves to a different landblock</returns>
        public bool UpdatePlayerPhysics(ACE.Entity.Position newPosition, bool forceUpdate = false)
        {
            //Console.WriteLine($"{Name}.UpdatePlayerPhysics({newPosition}, {forceUpdate}, {Teleporting})");

            var player = this as Player;

            // only handles player movement
            if (player == null) return false;

            // possible bug: while teleporting, client can still send AutoPos packets from old landblock
            if (Teleporting && !forceUpdate) return false;

            try
            {
                if (!forceUpdate) // This is needed beacuse this function might be called recursively
                    stopwatch.Restart();

                var success = true;

                if (PhysicsObj != null)
                {
                    var distSq = Location.SquaredDistanceTo(newPosition);

                    if (distSq > PhysicsGlobals.EpsilonSq)
                    {
                        var curCell = LScape.get_landcell(newPosition.Cell);
                        if (curCell != null)
                        {
                            //if (PhysicsObj.CurCell == null || curCell.ID != PhysicsObj.CurCell.ID)
                            //PhysicsObj.change_cell_server(curCell);

                            PhysicsObj.set_request_pos(newPosition.Pos, newPosition.Rotation, curCell, Location.LandblockId.Raw);
                            success = PhysicsObj.update_object_server();

                            if (PhysicsObj.CurCell == null)
                                PhysicsObj.CurCell = curCell;

                            player.CheckMonsters();

                            if (curCell.ID != prevCell)
                            {
                                //prevCell = curCell.ID;
                                //Console.WriteLine("Player cell: " + curCell.ID.ToString("X8"));
                                //var envCell = curCell as Physics.Common.EnvCell;
                                //var seenOutside = envCell != null ? envCell.SeenOutside : true;
                                //Console.WriteLine($"CurCell: {curCell.ID:X8}, SeenOutside: {seenOutside}");
                            }
                        }
                    }
                }

                // double update path: landblock physics update -> updateplayerphysics() -> update_object_server() -> Teleport() -> updateplayerphysics() -> return to end of original branch
                if (Teleporting && !forceUpdate) return true;

                if (!success) return false;

                var landblockUpdate = Location.Cell >> 16 != newPosition.Cell >> 16;
                Location = newPosition;

                SendUpdatePosition();

                if (!InUpdate)
                    LandblockManager.RelocateObjectForPhysics(this, true);

                return landblockUpdate;
            }
            finally
            {
                if (!forceUpdate) // This is needed beacuse this function might be called recursively
                {
                    var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                    ServerPerformanceMonitor.AddToCumulativeEvent(ServerPerformanceMonitor.CumulativeEventHistoryType.WorldObject_Tick_UpdatePlayerPhysics, elapsedSeconds);
                    if (elapsedSeconds >= 1) // Yea, that ain't good....
                        log.Warn($"[PERFORMANCE][PHYSICS] {Guid}:{Name} took {(elapsedSeconds * 1000):N1} ms to process UpdatePlayerPhysics() at loc: {Location}");
                    else if (elapsedSeconds >= 0.010)
                        log.Debug($"[PERFORMANCE][PHYSICS] {Guid}:{Name} took {(elapsedSeconds * 1000):N1} ms to process UpdatePlayerPhysics() at loc: {Location}");
                }
            }
        }

        public double lastDist;

        public static double ProjectileTimeout = 30.0f;

        private readonly double physicsCreationTime = PhysicsTimer.CurrentTime;

        public double LastPhysicsUpdate;

        public static double UpdateRate_Creature = 0.2f;

        /// <summary>
        /// Handles calling the physics engine for non-player objects
        /// </summary>
        public bool UpdateObjectPhysics()
        {
            // TODO: Almost all of the CPU time is spent between this note and the first Try block. Mag-nus 2019-10-21
            // TODO: In the future we should look at improving the way UpdateObjectPhysics() is called from Landblock
            // TODO: We should exclude objects that never tick physics (Monsters)
            // TODO: Perhaps for objects that have a throttle (Creatures), we use a list and only iterate through the pending creatures

            if (PhysicsObj == null || !PhysicsObj.is_active())
                return false;

            bool isDying = false;

            if (this is Creature creature)
            {
                if (LastPhysicsUpdate + UpdateRate_Creature > PhysicsTimer.CurrentTime)
                    return false;

                LastPhysicsUpdate = PhysicsTimer.CurrentTime;

                // monsters have separate physics updates,
                // except during the first frame of spawning, idle emotes, and dying
                isDying = creature.IsDead;

                // determine if updates should be run for object
                var runUpdate = PhysicsObj.IsAnimating && (!creature.IsMonster || !creature.IsAwake) || isDying || PhysicsObj.InitialUpdates <= 1;

                if (!runUpdate)
                    return false;
            }
            else
            {
                // arrows / spell projectiles
                //var isMissile = Missile ?? false;
                if ((PhysicsObj.State & PhysicsState.Missile) != 0) // This is a bit more performant than the line above
                {
                    if (physicsCreationTime + ProjectileTimeout <= PhysicsTimer.CurrentTime)
                    {
                        // only for projectiles?
                        //Console.WriteLine("Timeout reached - destroying " + Name);
                        PhysicsObj.set_active(false);
                        Destroy();
                        return false;
                    }

                    // missiles always run an update
                }
                else
                {
                    // determine if updates should be run for object
                    var runUpdate = PhysicsObj.IsAnimating || PhysicsObj.InitialUpdates <= 1;

                    if (!runUpdate)
                        return false;
                }
            }

            try
            {
                stopwatch.Restart();

                // get position before
                var prevPos = PhysicsObj.Position.Frame.Origin;
                var cellBefore = PhysicsObj.CurCell != null ? PhysicsObj.CurCell.ID : 0;

                //Console.WriteLine($"{Name} - ticking physics");
                var updated = PhysicsObj.update_object();

                // get position after
                var newPos = PhysicsObj.Position.Frame.Origin;

                // handle landblock / cell change
                var isMoved = (prevPos != newPos);
                var curCell = PhysicsObj.CurCell;

                if (PhysicsObj.CurCell == null)
                {
                    //Console.WriteLine("CurCell is null");
                    PhysicsObj.set_active(false);
                    Destroy();
                    return false;
                }

                var landblockUpdate = (cellBefore >> 16) != (curCell.ID >> 16);

                if (isMoved || isDying)
                {
                    if (curCell.ID != cellBefore)
                        Location.LandblockId = new LandblockId(curCell.ID);

                    Location.Pos = newPos;
                    Location.Rotation = PhysicsObj.Position.Frame.Orientation;

                    //if (landblockUpdate)
                    //WorldManager.UpdateLandblock.Add(this);
                }

                /*if (PhysicsObj.IsGrounded)
                    SendUpdatePosition();*/

                //var dist = Vector3.Distance(ProjectileTarget.Location.Pos, newPos);
                //Console.WriteLine("Dist: " + dist);
                //Console.WriteLine("Velocity: " + PhysicsObj.Velocity);

                if (this is SpellProjectile spellProjectile && spellProjectile.SpellType == SpellProjectile.ProjectileSpellType.Ring)
                {
                    var dist = spellProjectile.SpawnPos.DistanceTo(Location);
                    var maxRange = spellProjectile.Spell.BaseRangeConstant;
                    //Console.WriteLine("Max range: " + maxRange);
                    if (dist > maxRange)
                    {
                        PhysicsObj.set_active(false);
                        spellProjectile.ProjectileImpact();
                        return false;
                    }
                }

                return landblockUpdate;
            }
            finally
            {
                var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                ServerPerformanceMonitor.AddToCumulativeEvent(ServerPerformanceMonitor.CumulativeEventHistoryType.WorldObject_Tick_UpdateObjectPhysics, elapsedSeconds);
                if (elapsedSeconds >= 1) // Yea, that ain't good....
                    log.Warn($"[PERFORMANCE][PHYSICS] {Guid}:{Name} took {(elapsedSeconds * 1000):N1} ms to process UpdateObjectPhysics() at loc: {Location}");
                else if (elapsedSeconds >= 0.010)
                    log.Debug($"[PERFORMANCE][PHYSICS] {Guid}:{Name} took {(elapsedSeconds * 1000):N1} ms to process UpdateObjectPhysics() at loc: {Location}");
            }
        }
    }
}
