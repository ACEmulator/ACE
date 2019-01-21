using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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
            {
                // determine the delay for respawning
                var delay = Biota.Delay ?? 0;
                if (delay == 0)
                    delay = _generator.GeneratorProfiles[0].Biota.Delay ?? 0;   // only for link generators?

                if (_generator is Chest) delay = 0.0f;

                //Console.WriteLine($"QueueGenerator({_generator.Name}): RegenerationInterval: {_generator.RegenerationInterval} - Delay: {delay}");
                return DateTime.UtcNow.AddSeconds(delay);
            }
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

            if ((RegenLocationType & RegenLocationType.Treasure) != 0)
            {
                objects = TreasureGenerator();
                //Console.WriteLine($"{_generator.Name}.WhereCreate: {(RegenLocationType)Biota.WhereCreate}");
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

            for (var i = 0; i < objects.Count; i++)
            {
                var obj = objects[i];

                SetLocation(obj);

                if ((obj.Location == null || obj.Location.Landblock != _generator.Location.Landblock) && (RegenLocationType & RegenLocationType.Contain) == 0)
                {
                    var landblock = obj.Location != null ? obj.Location.Landblock.ToString("X4") : "null";
                    //Console.WriteLine($"*** WARNING *** {_generator.Name} spawned {obj.Name} in landblock {landblock} from {_generator.Location.Landblock:X4} using {(RegenLocationType)Biota.WhereCreate}");
                    //objects[i] = null;
                    continue;
                }

                obj.EnterWorld();
            }

            //Console.WriteLine($"Generator({_generator.Name} - {_generator.Guid} @ {_generator.Location.Cell:X8}) spawned {wo.Name} - {(RegenLocationType)Biota.WhereCreate}");
            return objects;
        }

        public void SetLocation(WorldObject obj)
        {
            var regenLocationType = (RegenLocationType)Biota.WhereCreate;

            if ((regenLocationType & RegenLocationType.Specific) != 0)
            {
                // spawns an object at a specific position
                if ((Biota.ObjCellId ?? 0) > 0)  // specific location
                    obj.Location = new ACE.Entity.Position(Biota.ObjCellId ?? 0, Biota.OriginX ?? 0, Biota.OriginY ?? 0, Biota.OriginZ ?? 0, Biota.AnglesX ?? 0, Biota.AnglesY ?? 0, Biota.AnglesZ ?? 0, Biota.AnglesW ?? 0);  // TODO: wrapper
                else  // offset from generator location
                    obj.Location = new ACE.Entity.Position(_generator.Location.Cell,
                        _generator.Location.PositionX + Biota.OriginX ?? 0, _generator.Location.PositionY + Biota.OriginY ?? 0, _generator.Location.PositionZ + Biota.OriginZ ?? 0,
                        Biota.AnglesX ?? 0, Biota.AnglesY ?? 0, Biota.AnglesZ ?? 0, Biota.AnglesW ?? 0);
            }

            else if ((regenLocationType & RegenLocationType.Scatter) != 0)
            {
                // spawns at random position within radius of generator
                float genRadius = (float)(_generator.GetProperty(PropertyFloat.GeneratorRadius) ?? 0f);
                var random_x = ThreadSafeRandom.Next(-genRadius, genRadius);
                var random_y = ThreadSafeRandom.Next(-genRadius, genRadius);
                obj.Location = new ACE.Entity.Position(_generator.Location);
                var newPos = obj.Location.Pos + new Vector3(random_x, random_y, 0.0f);
                if (!_generator.Location.Indoors)
                {
                    // Based on GDL scatter
                    newPos.X = Math.Clamp(newPos.X, 0.5f, 191.5f);
                    newPos.Y = Math.Clamp(newPos.Y, 0.5f, 191.5f);
                    obj.Location.SetPosition(newPos);
                    newPos.Z = LScape.get_landblock(obj.Location.Cell).GetZ(newPos);
                }
                obj.Location.SetPosition(newPos);
                obj.Location.LandblockId = new LandblockId(obj.Location.GetCell());
            }

            else if ((regenLocationType & RegenLocationType.Contain) != 0)
            {
                // generator is a container, spawns in inventory

                //Console.WriteLine($"{_generator.Name}.Spawn.Contain: adding {obj.Name} ({obj.Guid:X8})");
                var container = _generator as Container;
                if (container == null || !container.TryAddToInventory(obj))
                {
                    Console.WriteLine($"Generator({_generator.Name}) - failed to add {obj.Name} to container inventory");
                    obj.Location = new ACE.Entity.Position(_generator.Location);
                }
            }
            else
            {
                // default
                obj.Location = new ACE.Entity.Position(_generator.Location);
            }
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

            Spawned.TryGetValue(target.Full, out var item);

            if (item != null)
            {
                //Console.WriteLine("Found, removing and queueing...");
                Spawned.Remove(target.Full);
                Enqueue(1, false);
            }
        }
    }
}
