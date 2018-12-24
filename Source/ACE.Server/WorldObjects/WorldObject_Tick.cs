using System;
using System.Numerics;

using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Physics;
using ACE.Server.Physics.Common;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        private readonly ActionQueue actionQueue = new ActionQueue();

        public const int DefaultHeartbeatInterval = 5;

        protected double? cachedHeartbeatTimestamp;
        protected double cachedHeartbeatInterval;

        public virtual void Tick(double currentUnixTime)
        {
            actionQueue.RunActions();

            if (cachedHeartbeatTimestamp == null)
            {
                cachedHeartbeatInterval = HeartbeatInterval ?? DefaultHeartbeatInterval;
                QueueFirstHeartbeat(currentUnixTime);
            }
            else if (cachedHeartbeatTimestamp + cachedHeartbeatInterval <= currentUnixTime)
                HeartBeat(currentUnixTime);
        }


        /// <summary>
        /// Runs all actions pending on this WorldObject
        /// </summary>
        void IActor.RunActions()
        {
            actionQueue.RunActions();
        }

        /// <summary>
        /// Prepare new action to run on this object
        /// </summary>
        public void EnqueueAction(IAction action)
        {
            actionQueue.EnqueueAction(action);
        }


        /// <summary>
        /// Enqueues the first heartbeat on a staggered 0-5s delay
        /// </summary>
        public void QueueFirstHeartbeat(double currentUnixTime)
        {
            var delay = ThreadSafeRandom.Next(0.0f, DefaultHeartbeatInterval);

            var firstHeartbeat = currentUnixTime + delay;

            cachedHeartbeatTimestamp = firstHeartbeat - cachedHeartbeatInterval;
        }

        /// <summary>
        /// Called every ~5 seconds for WorldObject base
        /// </summary>
        public virtual void HeartBeat(double currentUnixTime)
        {
            Generator_HeartBeat();

            EmoteManager.HeartBeat();

            EnchantmentManager.HeartBeat();

            cachedHeartbeatTimestamp = currentUnixTime;
            SetProperty(PropertyFloat.HeartbeatTimestamp, currentUnixTime);
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
            //Console.WriteLine($"UpdatePlayerPhysics: {newPosition.Cell:X8}, {newPosition.Pos}");

            var player = this as Player;

            // only handles player movement
            if (player == null) return false;

            // possible bug: while teleporting, client can still send AutoPos packets from old landblock
            if (Teleporting && !forceUpdate) return false;

            if (PhysicsObj != null)
            {
                var dist = (newPosition.Pos - PhysicsObj.Position.Frame.Origin).Length();
                if (dist > PhysicsGlobals.EPSILON)
                {
                    var curCell = LScape.get_landcell(newPosition.Cell);
                    if (curCell != null)
                    {
                        //if (PhysicsObj.CurCell == null || curCell.ID != PhysicsObj.CurCell.ID)
                        //PhysicsObj.change_cell_server(curCell);

                        PhysicsObj.set_request_pos(newPosition.Pos, newPosition.Rotation, curCell, Location.LandblockId.Raw);
                        PhysicsObj.update_object_server();

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

            var landblockUpdate = Location.Cell >> 16 != newPosition.Cell >> 16;
            Location = newPosition;

            SendUpdatePosition();

            if (!InUpdate)
                LandblockManager.RelocateObjectForPhysics(this, true);

            return landblockUpdate;
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
            if (PhysicsObj == null || !PhysicsObj.is_active())
                return false;

            // arrows / spell projectiles
            var isMissile = Missile ?? false;

            //var contactPlane = (PhysicsObj.State & PhysicsState.Gravity) != 0 && MotionTableId != 0 && (PhysicsObj.TransientState & TransientStateFlags.Contact) == 0;

            // monsters have separate physics updates
            var creature = this as Creature;
            var monster = creature != null && creature.IsMonster;
            var pet = this as CombatPet;

            // determine if updates should be run for object
            //var runUpdate = !monster && (isMissile || !PhysicsObj.IsGrounded);
            //var runUpdate = isMissile;
            var runUpdate = !monster && (isMissile || /*IsMoving ||*/ /*!PhysicsObj.IsGrounded || */ PhysicsObj.InitialUpdates <= 1 || IsAnimating /*|| contactPlane*/);

            if (creature != null)
            {
                if (LastPhysicsUpdate + UpdateRate_Creature <= PhysicsTimer.CurrentTime)
                    LastPhysicsUpdate = PhysicsTimer.CurrentTime;
                else
                    runUpdate = false;
            }

            if (!runUpdate) return false;

            if (isMissile && physicsCreationTime + ProjectileTimeout <= PhysicsTimer.CurrentTime)
            {
                // only for projectiles?
                //Console.WriteLine("Timeout reached - destroying " + Name);
                PhysicsObj.set_active(false);
                Destroy();
                return false;
            }

            // get position before
            var pos = PhysicsObj.Position.Frame.Origin;
            var prevPos = pos;
            var cellBefore = PhysicsObj.CurCell != null ? PhysicsObj.CurCell.ID : 0;

            //Console.WriteLine($"{Name} - ticking physics");
            var updated = PhysicsObj.update_object();

            // get position after
            pos = PhysicsObj.Position.Frame.Origin;
            var newPos = pos;

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
            if (isMoved)
            {
                if (curCell.ID != cellBefore)
                    Location.LandblockId = new LandblockId(curCell.ID);

                Location.Pos = newPos;
                //if (landblockUpdate)
                //WorldManager.UpdateLandblock.Add(this);
            }

            /*if (PhysicsObj.IsGrounded)
                SendUpdatePosition();*/

            //var dist = Vector3.Distance(ProjectileTarget.Location.Pos, newPos);
            //Console.WriteLine("Dist: " + dist);
            //Console.WriteLine("Velocity: " + PhysicsObj.Velocity);

            var spellProjectile = this as SpellProjectile;
            if (spellProjectile != null && spellProjectile.SpellType == SpellProjectile.ProjectileSpellType.Ring)
            {
                var dist = Vector3.Distance(spellProjectile.SpawnPos.ToGlobal(), Location.ToGlobal());
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
    }
}
