using System;

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


        public double lastDist;

        public static double ProjectileTimeout = 30.0f;

        private readonly double physicsCreationTime = PhysicsTimer.CurrentTime;

        public double LastPhysicsUpdate;

        public static double UpdateRate_Creature = 0.2f;

        /// <summary>
        /// Handles calling the physics engine for non-player objects
        /// </summary>
        public virtual bool UpdateObjectPhysics()
        {
            if (PhysicsObj == null || !PhysicsObj.is_active())
                return false;

            // arrows / spell projectiles
            var isMissile = Missile ?? false;

            //var contactPlane = (PhysicsObj.State & PhysicsState.Gravity) != 0 && MotionTableId != 0 && (PhysicsObj.TransientState & TransientStateFlags.Contact) == 0;

            // monsters have separate physics updates
            var creature = this as Creature;
            var monster = creature != null && creature.IsMonster;
            //var pet = this as CombatPet;

            // determine if updates should be run for object
            //var runUpdate = !monster && (isMissile || !PhysicsObj.IsGrounded);
            //var runUpdate = isMissile;
            var runUpdate = !monster && (isMissile || /*IsMoving ||*/ /*!PhysicsObj.IsGrounded || */ PhysicsObj.InitialUpdates <= 1 || PhysicsObj.IsAnimating /*|| contactPlane*/);

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
    }
}
