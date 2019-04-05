using System;
using System.Collections.Generic;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Physics.Common;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// An active generator profile
    /// </summary>
    public class Generator
    {
        /// <summary>
        /// The biota with all the generator profile info
        /// </summary>
        public BiotaPropertiesGenerator Biota;

        /// <summary>
        /// A list of objects that have been spawned by this generator
        /// Mapping of object guid => registry node, which provides a bunch of detailed info about the spawn
        /// </summary>
        public Dictionary<uint, GeneratorRegistryNode> Spawned;

        /// <summary>
        /// The list of pending times awaiting respawning
        /// </summary>
        public List<DateTime> SpawnQueue;

        /// <summary>
        /// Returns TRUE if this profile is a placeholder object
        /// Placeholder objects are used for linkable generators,
        /// and are used as a template for the real items contained in the links.
        /// </summary>
        public bool IsPlaceholder { get => Biota.WeenieClassId == 3666; }

        /// <summary>
        /// The total # of active spawned objects + awaiting spawning
        /// </summary>
        public int CurrentCreate { get => Spawned.Count + SpawnQueue.Count; }

        /// <summary>
        /// Returns the MaxCreate for this generator profile
        /// If set to -1 in the database, use MaxCreate from generator
        /// </summary>
        public int MaxCreate { get => Biota.MaxCreate > -1 ? Biota.MaxCreate : _generator.MaxCreate; }

        /// <summary>
        /// Returns TRUE if the initial # of objects have been spawned
        /// </summary>
        public bool InitObjectsSpawned { get => CurrentCreate >= Biota.InitCreate; }

        /// <summary>
        /// Returns TRUE if the maximum # of objects have been spawned
        /// </summary>
        public bool MaxObjectsSpawned { get => CurrentCreate >= MaxCreate; }

        /// <summary>
        /// The delay for respawning objects
        /// </summary>
        public float Delay { get => Biota.Delay ?? _generator.GeneratorProfiles[0].Biota.Delay ?? 0.0f; }

        /// <summary>
        /// The parent for this generator profile
        /// </summary>
        public WorldObject _generator;

        public RegenLocationType RegenLocationType => (RegenLocationType)Biota.WhereCreate;

        /// <summary>
        /// Constructs a new active generator profile
        /// from a biota generator
        /// </summary>
        public Generator(WorldObject generator, BiotaPropertiesGenerator biota)
        {
            _generator = generator;
            Biota = biota;

            Spawned = new Dictionary<uint, GeneratorRegistryNode>();
            SpawnQueue = new List<DateTime>();
        }

        /// <summary>
        /// Called every ~5 seconds for generator
        /// </summary>
        public void HeartBeat()
        {
            if (SpawnQueue.Count > 0)
                ProcessQueue();
        }

        /// <summary>
        /// Determines the spawn times for initial object spawning,
        /// and for respawning
        /// </summary>
        public DateTime GetSpawnTime()
        {
            if (_generator.CurrentlyPoweringUp)
            {
                // initial spawn delay
                if (_generator.GeneratorInitialDelay == 6000)   // spawn repair golem immediately?
                    _generator.GeneratorInitialDelay = 0;

                if (_generator.GeneratorInitialDelay == 900)    // spawn menhir drummers immmediately for testing
                    _generator.GeneratorInitialDelay = 0;

                if (_generator.GeneratorInitialDelay == 1800)   // spawn queen early
                    _generator.GeneratorInitialDelay = 0;

                if (_generator.GeneratorInitialDelay > 300)     // max spawn time: 5 mins
                    _generator.GeneratorInitialDelay = 300;


                return DateTime.UtcNow.AddSeconds(_generator.GeneratorInitialDelay);
            }
            else
                return DateTime.UtcNow;
        }

        /// <summary>
        /// Enqueues 1 or multiple objects from this generator profile
        /// adds these items to the spawn queue
        /// </summary>
        public void Enqueue(int numObjects = 1, bool initialSpawn = true)
        {
            for (var i = 0; i < numObjects; i++)
            {
                /*if (MaxObjectsSpawned)
                {
                    Console.WriteLine($"{_generator.Name}.Enqueue({numObjects}): max objects reached");
                    break;
                }*/
                SpawnQueue.Add(GetSpawnTime());
                if (initialSpawn)
                    _generator.CurrentCreate++;
            }
        }

        /// <summary>
        /// Spawns generator objects at the correct SpawnTime
        /// Called on heartbeat ticks every ~5 seconds
        /// </summary>
        public void ProcessQueue()
        {
            var index = 0;
            while (index < SpawnQueue.Count)
            {
                var queuedTime = SpawnQueue[index];

                if (queuedTime > DateTime.UtcNow)
                {
                    // not time to spawn yet
                    index++;
                    continue;
                }

                if ((RegenLocationType & RegenLocationType.Treasure) != 0)
                    RemoveTreasure();

                if (Spawned.Count < MaxCreate)
                {
                    var objects = Spawn();

                    if (objects != null)
                    {
                        foreach (var obj in objects)
                        {
                            var registry = new GeneratorRegistryNode();

                            registry.WeenieClassId = Biota.WeenieClassId;
                            registry.Timestamp = DateTime.UtcNow;
                            registry.WorldObject = obj;
                            obj.Generator = _generator;
                            obj.GeneratorId = _generator.Guid.Full;

                            Spawned.Add(obj.Guid.Full, registry);
                        }
                    }
                    else
                    {
                        //_generator.CurrentCreate--;
                    }
                }
                else
                {
                    // this shouldn't happen (hopefully)
                    Console.WriteLine($"GeneratorProfile: objects enqueued for {_generator.Name}, but MaxCreate({MaxCreate}) already reached!");
                }
                SpawnQueue.RemoveAt(index);
            }
        }

        /// <summary>
        /// Spawns an object from the generator queue
        /// for RNG treasure, can spawn multiple objects
        /// </summary>
        public List<WorldObject> Spawn()
        {
            var objects = new List<WorldObject>();

            if (RegenLocationType.HasFlag(RegenLocationType.Treasure))
            {
                objects = TreasureGenerator();
            }
            else
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(Biota.WeenieClassId);
                if (wo == null)
                {
                    Console.WriteLine($"{_generator.Name}.Spawn(): failed to create wcid {Biota.WeenieClassId}");
                    return null;
                }
                objects.Add(wo);
            }

            foreach (var obj in objects)
            {
                //Console.WriteLine($"{_generator.Name}.Spawn({obj.Name})");

                if (RegenLocationType.HasFlag(RegenLocationType.Specific))
                    Spawn_Specific(obj);

                else if (RegenLocationType.HasFlag(RegenLocationType.Scatter))
                    Spawn_Scatter(obj);

                else if (RegenLocationType.HasFlag(RegenLocationType.Contain))
                    Spawn_Container(obj);

                else if (RegenLocationType.HasFlag(RegenLocationType.Shop))
                    Spawn_Shop(obj);

                else
                    Spawn_Default(obj);
            }
            return objects;
        }

        /// <summary>
        /// Spawns an object at a specific position
        /// </summary>
        public void Spawn_Specific(WorldObject obj)
        {
            // specific position
            if ((Biota.ObjCellId ?? 0) > 0)
                obj.Location = new ACE.Entity.Position(Biota.ObjCellId ?? 0, Biota.OriginX ?? 0, Biota.OriginY ?? 0, Biota.OriginZ ?? 0, Biota.AnglesX ?? 0, Biota.AnglesY ?? 0, Biota.AnglesZ ?? 0, Biota.AnglesW ?? 0);

            // offset from generator location
            else
                obj.Location = new ACE.Entity.Position(_generator.Location.Cell, _generator.Location.PositionX + Biota.OriginX ?? 0, _generator.Location.PositionY + Biota.OriginY ?? 0, _generator.Location.PositionZ + Biota.OriginZ ?? 0, Biota.AnglesX ?? 0, Biota.AnglesY ?? 0, Biota.AnglesZ ?? 0, Biota.AnglesW ?? 0);

            if (!VerifyLandblock(obj) || !VerifyWalkableSlope(obj)) return;

            obj.EnterWorld();
        }

        public void Spawn_Scatter(WorldObject obj)
        {
            float genRadius = (float)(_generator.GetProperty(PropertyFloat.GeneratorRadius) ?? 0f);
            obj.Location = new ACE.Entity.Position(_generator.Location);

            // we are going to delay this scatter logic until the physics engine,
            // where the remnants of this function are in the client (SetScatterPositionInternal)

            // this is due to each randomized position being required to go through the full InitialPlacement process, to verify success
            // if InitialPlacement fails, then we retry up to maxTries

            obj.ScatterPos = new SetPosition(new Physics.Common.Position(obj.Location), SetPositionFlags.RandomScatter, genRadius);

            obj.EnterWorld();

            obj.ScatterPos = null;
        }

        public void Spawn_Container(WorldObject obj)
        {
            var container = _generator as Container;

            if (container == null || !container.TryAddToInventory(obj))
                Console.WriteLine($"{_generator.Name}.Spawn_Container({obj.Name}) - failed to add to container inventory");
        }

        public void Spawn_Shop(WorldObject obj)
        {
            // spawn item in vendor shop inventory
            var vendor = _generator as Vendor;

            if (vendor == null)
            {
                Console.WriteLine($"{_generator.Name}.Spawn_Shop({obj.Name}) - generator is not a vendor type");
                return;
            }
            vendor.AddDefaultItem(obj);
        }

        public void Spawn_Default(WorldObject obj)
        {
            // default location handler?
            //Console.WriteLine($"{_generator.Name}.Spawn_Default({obj.Name}): default handler for RegenLocationType {RegenLocationType}");

            obj.Location = new ACE.Entity.Position(_generator.Location);

            obj.EnterWorld();
        }

        public bool VerifyLandblock(WorldObject obj)
        {
            if (obj.Location == null || obj.Location.Landblock != _generator.Location.Landblock)
            {
                //Console.WriteLine($"{_generator.Name}.VerifyLandblock({obj.Name}) - spawn location is invalid landblock");
                return false;
            }
            return true;
        }

        public bool VerifyWalkableSlope(WorldObject obj)
        {
            if (!obj.Location.Indoors && !obj.Location.IsWalkable())
            {
                //Console.WriteLine($"{_generator.Name}.VerifyWalkableSlope({obj.Name}) - spawn location is unwalkable slope");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Generates a randomized treasure from LootGenerationFactory
        /// </summary>
        public List<WorldObject> TreasureGenerator()
        {
            // profile.WeenieClassId is not a weenieClassId,
            // it's a DeathTreasure or WieldedTreasure table DID
            // there is no overlap of DIDs between these 2 tables,
            // so they can be searched in any order..
            var deathTreasure = DatabaseManager.World.GetCachedDeathTreasure(Biota.WeenieClassId);
            if (deathTreasure != null)
            {
                // TODO: get randomly generated death treasure from LootGenerationFactory
                //Console.WriteLine($"{_generator.Name}.TreasureGenerator(): found death treasure {Biota.WeenieClassId}");
                return LootGenerationFactory.CreateRandomLootObjects(deathTreasure);
            }
            else
            {
                var wieldedTreasure = DatabaseManager.World.GetCachedWieldedTreasure(Biota.WeenieClassId);
                if (wieldedTreasure != null)
                {
                    // TODO: get randomly generated wielded treasure from LootGenerationFactory
                    //Console.WriteLine($"{_generator.Name}.TreasureGenerator(): found wielded treasure {Biota.WeenieClassId}");

                    // roll into the wielded treasure table
                    var table = new TreasureWieldedTable(wieldedTreasure);
                    return _generator.GenerateWieldedTreasureSets(table);
                }
                else
                {
                    Console.WriteLine($"{_generator.Name}.TreasureGenerator(): couldn't find death treasure or wielded treasure for ID {Biota.WeenieClassId}");
                    return new List<WorldObject>();
                }
            }
        }

        /// <summary>
        /// Removes all of the objects from a container for this profile
        /// </summary>
        public void RemoveTreasure()
        {
            var container = _generator as Container;
            if (container == null)
            {
                Console.WriteLine($"{_generator.Name}.RemoveTreasure(): container not found");
                return;
            }
            foreach (var spawned in Spawned.Keys)
            {
                var inventoryObjGuid = new ObjectGuid(spawned);
                if (!container.Inventory.TryGetValue(inventoryObjGuid, out var inventoryObj))
                {
                    Console.WriteLine($"{_generator.Name}.RemoveTreasure(): couldn't find {inventoryObjGuid}");
                    continue;
                }
                container.TryRemoveFromInventory(inventoryObjGuid);
                inventoryObj.Destroy();
            }
            Spawned.Clear();
        }


        /// <summary>
        /// Callback system for objects notifying their generators of events,
        /// ie. item pickup
        /// </summary>
        public void NotifyGenerator(ObjectGuid target, RegenerationType eventType)
        {
            //Console.WriteLine($"{_generator.Name}.NotifyGenerator({target:X8}, {eventType})");

            if (Biota.WhenCreate != (uint)eventType)
                return;

            Spawned.TryGetValue(target.Full, out var obj);

            if (obj == null) return;

            //Console.WriteLine($"{_generator.Name}.NotifyGenerator({target}, {eventType}) - RegenerationInterval: {_generator.RegenerationInterval} - Delay: {Biota.Delay} - Link Delay: {_generator.GeneratorProfiles[0].Biota.Delay}");
            var delay = Delay;
            if (_generator is Chest || _generator.RegenerationInterval == 0)
                delay = 0;

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(delay);
            actionChain.AddAction(_generator, () => FreeSlot(obj));
            actionChain.EnqueueChain();
            //Enqueue(1, false);
        }

        public void FreeSlot(GeneratorRegistryNode node)
        {
            Spawned.Remove(node.WorldObject.Guid.Full);
            _generator.CurrentCreate--;
        }
    }
}
