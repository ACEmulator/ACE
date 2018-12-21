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
                if (MaxObjectsSpawned)
                {
                    Console.WriteLine($"{_generator.Name}.Enqueue({numObjects}): max objects reached");
                    break;
                }
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

                if (Spawned.Count < MaxCreate)
                {
                    var obj = Spawn();

                    if (obj != null)
                    {
                        var registry = new GeneratorRegistryNode();

                        registry.WeenieClassId = Biota.WeenieClassId;
                        registry.Timestamp = DateTime.UtcNow;
                        registry.WorldObject = obj;
                        obj.Generator = _generator;
                        obj.GeneratorId = _generator.Guid.Full;

                        Spawned.Add(obj.Guid.Full, registry);
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
        /// </summary>
        public WorldObject Spawn()
        {
            switch ((RegenLocationType)Biota.WhereCreate)
            {
                case RegenLocationType.ContainTreasure:
                case RegenLocationType.OnTopTreasure:
                case RegenLocationType.ScatterTreasure:
                case RegenLocationType.SpecificTreasure:
                case RegenLocationType.Treasure:
                case RegenLocationType.WieldTreasure:

                    // not generating for now, until LootGenerationFactory has this API
                    //TreasureGenerator();
                    return null;
            }

            var wo = WorldObjectFactory.CreateNewWorldObject(Biota.WeenieClassId);
            if (wo == null)
            {
                Console.WriteLine($"{_generator.Name}.Spawn(): failed to spawn wcid {Biota.WeenieClassId}");
                return null;
            }

            switch ((RegenLocationType)Biota.WhereCreate)
            {
                // spawns an object at a specific position
                case RegenLocationType.Specific:
                case RegenLocationType.SpecificTreasure:

                    if ((Biota.ObjCellId ?? 0) > 0)  // specific location
                        wo.Location = new ACE.Entity.Position(Biota.ObjCellId ?? 0, Biota.OriginX ?? 0, Biota.OriginY ?? 0, Biota.OriginZ ?? 0, Biota.AnglesX ?? 0, Biota.AnglesY ?? 0, Biota.AnglesZ ?? 0, Biota.AnglesW ?? 0);  // TODO: wrapper
                    else  // offset from generator location
                        wo.Location = new ACE.Entity.Position(_generator.Location.Cell,
                            _generator.Location.PositionX + Biota.OriginX ?? 0, _generator.Location.PositionY + Biota.OriginY ?? 0, _generator.Location.PositionZ + Biota.OriginZ ?? 0,
                            Biota.AnglesX ?? 0, Biota.AnglesY ?? 0, Biota.AnglesZ ?? 0, Biota.AnglesW ?? 0);
                    break;

                // spawns at random position within radius of generator
                case RegenLocationType.Scatter:
                case RegenLocationType.ScatterTreasure:

                    float genRadius = (float)(_generator.GetProperty(PropertyFloat.GeneratorRadius) ?? 0f);
                    var random_x = ThreadSafeRandom.Next(-genRadius, genRadius);
                    var random_y = ThreadSafeRandom.Next(-genRadius, genRadius);
                    wo.Location = new ACE.Entity.Position(_generator.Location);
                    var newPos = wo.Location.Pos + new Vector3(random_x, random_y, 0.0f);
                    if (!_generator.Location.Indoors)
                    {
                        // Based on GDL scatter
                        newPos.X = Math.Clamp(newPos.X, 0.5f, 191.5f);
                        newPos.Y = Math.Clamp(newPos.Y, 0.5f, 191.5f);
                        wo.Location.SetPosition(newPos);
                        newPos.Z = LScape.get_landblock(wo.Location.Cell).GetZ(newPos);
                    }
                    wo.Location.SetPosition(newPos);
                    wo.Location.LandblockId = new LandblockId(wo.Location.GetCell());
                    break;

                // generator is a container, spawns in inventory
                case RegenLocationType.Contain:
                case RegenLocationType.ContainTreasure:

                    var container = _generator as Container;
                    if (container == null || !container.TryAddToInventory(wo))
                    {
                        Console.WriteLine($"Generator({_generator.Name}) - failed to add {wo.Name} to container inventory");
                        wo.Location = new ACE.Entity.Position(_generator.Location);
                    }
                    break;

                default:
                    wo.Location = new ACE.Entity.Position(_generator.Location);
                    break;
            }

            if (wo.Location == null || wo.Location.Landblock != _generator.Location.Landblock)
            {
                //Console.WriteLine($"*** WARNING *** {_generator.Name} spawned {wo.Name} in landblock {wo.Location.Landblock:X4} from {_generator.Location.Landblock:X4} using {(RegenLocationType)Biota.WhereCreate}");
                return null;
            }

            wo.EnterWorld();
            //Console.WriteLine($"Generator({_generator.Name} - {_generator.Guid} @ {_generator.Location.Cell:X8}) spawned {wo.Name} - {(RegenLocationType)Biota.WhereCreate}");
            return wo;
        }

        /// <summary>
        /// Generates a randomized treasure from LootGenerationFactory
        /// </summary>
        public void TreasureGenerator()
        {
            // profile.WeenieClassId is not a weenieClassId,
            // it's a DeathTreasure or WieldedTreasure table DID
            // there is no overlap of DIDs between these 2 tables,
            // so they can be searched in any order..
            var deathTreasure = DatabaseManager.World.GetCachedDeathTreasure(Biota.WeenieClassId);
            if (deathTreasure != null)
            {
                // TODO: get randomly generated death treasure from LootGenerationFactory
            }
            else
            {
                var wieldedTreasure = DatabaseManager.World.GetCachedWieldedTreasure(Biota.WeenieClassId);
                if (wieldedTreasure != null)
                {
                    // TODO: get randomly generated wielded treasure from LootGenerationFactory
                }
                else
                {
                    Console.WriteLine($"Generator({_generator.Name}) - couldn't find death treasure or wielded treasure for ID {Biota.WeenieClassId}");
                }
            }
        }

        /// <summary>
        /// Callback system for objects notifying their generators of events,
        /// ie. item pickup
        /// </summary>
        public void NotifyGenerator(ObjectGuid target, RegenerationType eventType)
        {
            if (Biota.WhenCreate != (uint)eventType)
                return;

            Spawned.TryGetValue(target.Full, out var item);

            if (item != null)
            {
                Spawned.Remove(target.Full);
                Enqueue(1, false);
            }
        }
    }
}
