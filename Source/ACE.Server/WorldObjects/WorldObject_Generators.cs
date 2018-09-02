using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Physics.Common;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public WorldObject Generator { get; private set; }

        public List<BiotaPropertiesGenerator> GeneratorProfiles = new List<BiotaPropertiesGenerator>();

        public Dictionary<uint, GeneratorRegistryNode> GeneratorRegistry = new Dictionary<uint, GeneratorRegistryNode>();

        public Dictionary<uint, WorldObject> GeneratorCache = new Dictionary<uint, WorldObject>();

        public List<GeneratorQueueNode> GeneratorQueue = new List<GeneratorQueueNode>();

        public List<int> GeneratorProfilesActive = new List<int>();

        /// <summary>
        /// Returns TRUE if this object is a generator
        /// (spawns other world objects)
        /// </summary>
        public bool IsGenerator { get => GeneratorProfiles.Count > 0; }

        public void SelectGeneratorProfiles()
        {
            GeneratorProfilesActive.Clear();

            var random = new System.Random((int)DateTime.UtcNow.Ticks);

            if (GeneratorProfiles.Count > 0)
            {
                foreach (var profile in GeneratorProfiles)
                {
                    int slot = GeneratorProfiles.IndexOf(profile);

                    var rng = random.NextDouble();

                    if (rng < profile.Probability || profile.Probability == -1)
                    {
                        GeneratorProfilesActive.Add(slot);
                    }
                }

            }
        }

        public void UpdateGeneratorInts()
        {
            bool initZero = (InitGeneratedObjects == 0);
            bool maxZero = (MaxGeneratedObjects == 0);

            foreach (int slot in GeneratorProfilesActive)
            {
                if (initZero)
                {
                    InitGeneratedObjects += (int?)GeneratorProfiles[slot].InitCreate;
                }

                if (maxZero)
                {
                    MaxGeneratedObjects += (int?)GeneratorProfiles[slot].MaxCreate;
                }
            }
        }

        public void QueueGenerator()
        {
            foreach (int slot in GeneratorProfilesActive)
            {
                bool slotInUse = false;
                foreach (var obj in GeneratorRegistry)
                {
                    if (obj.Value.Slot == slot)
                    {
                        slotInUse = true;
                        break;
                    }
                }

                foreach (var obj in GeneratorQueue)
                {
                    if (obj.Slot == slot)
                    {
                        slotInUse = true;
                        break;
                    }
                }

                if (slotInUse)
                    continue;

                var queue = new GeneratorQueueNode();
                queue.Slot = (uint)slot;

                if (GeneratorRegistry.Count < InitGeneratedObjects && (CurrentlyPoweringUp ?? false))
                {
                    // initial spawn delay
                    queue.SpawnTime = Time.GetFutureTimestamp(GeneratorInitialDelay ?? 0);
                }
                else
                {
                    // determine the delay for respawning
                    var regenInterval = RegenerationInterval ?? 0;
                    var delay = GeneratorProfiles[0].Delay ?? 0;     //  found encounters with delays only in slot 0?
                    var minRegenTime = delay;
                    var maxRegenTime = delay + regenInterval;
                    var regenTime = Physics.Common.Random.RollDice(minRegenTime, (float)maxRegenTime);
                    //Console.WriteLine($"QueueGenerator({Name}): RegenerationInterval: {regenInterval} - Delay: {delay} - RegenTime: {regenTime}");

                    queue.SpawnTime = Time.GetFutureTimestamp(regenTime);
                }

                // System.Diagnostics.Debug.WriteLine($"Adding {queue.Slot} @ {queue.When} to GeneratorQueue for {Guid.Full}");
                GeneratorQueue.Add(queue);
            }
        }

        /// <summary>
        /// Spawns generator objects at the correct SpawnTime
        /// Called on heartbeat ticks every ~5 seconds
        /// </summary>
        public void ProcessGeneratorQueue()
        {
            var index = 0;
            while (index < GeneratorQueue.Count)
            {
                var currentTime = Time.GetTimestamp();
                if (currentTime < GeneratorQueue[index].SpawnTime)
                {
                    // not time to spawn yet
                    index++;
                    continue;
                }

                if (GeneratorRegistry.Count >= MaxGeneratedObjects)
                {
                    //System.Diagnostics.Debug.WriteLine($"GeneratorRegistry for {Guid.Full} is at MaxGeneratedObjects {MaxGeneratedObjects}");
                    //System.Diagnostics.Debug.WriteLine($"Removing {GeneratorQueue[index].Slot} from GeneratorQueue for {Guid.Full}");
                    GeneratorQueue.RemoveAt(index);
                    continue;
                }

                var profile = GeneratorProfiles[(int)GeneratorQueue[index].Slot];

                var rNode = new GeneratorRegistryNode();

                rNode.WeenieClassId = profile.WeenieClassId;
                rNode.Timestamp = Time.GetTimestamp();
                rNode.Slot = GeneratorQueue[index].Slot;

                var isTreasure = false;

                switch ((RegenLocationType)profile.WhereCreate)
                {
                    case RegenLocationType.ContainTreasure:
                    case RegenLocationType.OnTopTreasure:
                    case RegenLocationType.ScatterTreasure:
                    case RegenLocationType.SpecificTreasure:
                    case RegenLocationType.Treasure:
                    case RegenLocationType.WieldTreasure:

                        isTreasure = true;

                        // profile.WeenieClassId is not a weenieClassId,
                        // it's a DeathTreasure or WieldedTreasure table DID
                        // there is no overlap of DIDs between these 2 tables,
                        // so they can be searched in any order..
                        var deathTreasure = DatabaseManager.World.GetCachedDeathTreasure(profile.WeenieClassId);
                        if (deathTreasure != null)
                        {
                            // TODO: get randomly generated death treasure from LootGenerationFactory
                        }
                        else
                        {
                            var wieldedTreasure = DatabaseManager.World.GetCachedWieldedTreasure(profile.WeenieClassId);
                            if (wieldedTreasure != null)
                            {
                                // TODO: get randomly generated wielded treasure from LootGenerationFactory
                            }
                            else
                            {
                                Console.WriteLine($"Generator({Name}) - couldn't find death treasure or wielded treasure for ID {profile.WeenieClassId}");
                            }
                        }
                        break;
                }

                if (isTreasure)
                {
                    // not generating for now, until LootGenerationFactory has this API
                    GeneratorQueue.RemoveAt(index);
                    continue;
                }

                var wo = WorldObjectFactory.CreateNewWorldObject(profile.WeenieClassId);
                if (wo == null)
                {
                    // System.Diagnostics.Debug.WriteLine($"Removing {GeneratorQueue[index].Slot} from GeneratorQueue for {Guid.Full} because wcid {rNode.WeenieClassId} is not in the database");
                    GeneratorQueue.RemoveAt(index);
                    continue;
                }

                //Console.WriteLine($"Generator({Name} - {Location.Cell:X8}) spawned {wo.Name}");

                switch ((RegenLocationType)profile.WhereCreate)
                {
                    // spawns an object at a specific position
                    case RegenLocationType.Specific:
                    case RegenLocationType.SpecificTreasure:
                        if ((profile.ObjCellId ?? 0) > 0)
                            wo.Location = new ACE.Entity.Position(profile.ObjCellId ?? 0,
                                profile.OriginX ?? 0, profile.OriginY ?? 0, profile.OriginZ ?? 0,
                                profile.AnglesX ?? 0, profile.AnglesY ?? 0, profile.AnglesZ ?? 0, profile.AnglesW ?? 0);
                        else
                            wo.Location = new ACE.Entity.Position(Location.Cell,
                                Location.PositionX + profile.OriginX ?? 0, Location.PositionY + profile.OriginY ?? 0, Location.PositionZ + profile.OriginZ ?? 0,
                                profile.AnglesX ?? 0, profile.AnglesY ?? 0, profile.AnglesZ ?? 0, profile.AnglesW ?? 0);
                        break;

                    // spawns at random position within radius of generator
                    case RegenLocationType.Scatter:
                    case RegenLocationType.ScatterTreasure:
                        float genRadius = (float)(GetProperty(PropertyFloat.GeneratorRadius) ?? 0f);
                        var random_x = Physics.Common.Random.RollDice(-genRadius, genRadius);
                        var random_y = Physics.Common.Random.RollDice(-genRadius, genRadius);
                        wo.Location = new ACE.Entity.Position(Location);
                        var newPos = wo.Location.Pos + new Vector3(random_x, random_y, 0.0f);
                        if (!Location.Indoors)
                        {
                            // Based on GDL scatter
                            newPos.X = Math.Clamp(newPos.X, 0.5f, 191.5f);
                            newPos.Y = Math.Clamp(newPos.Y, 0.5f, 191.5f);
                            wo.Location.SetPosition(newPos);
                            newPos.Z = LScape.get_landblock(wo.Location.Cell).GetZ(newPos);
                        }
                        wo.Location.SetPosition(newPos);
                        break;

                    // generator is a container, spawns in inventory
                    case RegenLocationType.Contain:
                    case RegenLocationType.ContainTreasure:
                        var container = this as Container;
                        if (container == null || !container.TryAddToInventory(wo))
                        {
                            Console.WriteLine($"Generator({Name}) - failed to add {wo.Name} to container inventory");
                            wo.Location = new ACE.Entity.Position(Location);
                        }
                        break;

                    default:
                        wo.Location = new ACE.Entity.Position(Location);
                        break;
                }

                wo.Generator = this;
                wo.GeneratorId = Guid.Full;

                // System.Diagnostics.Debug.WriteLine($"Adding {wo.Guid.Full} | {rNode.Slot} in GeneratorRegistry for {Guid.Full}");
                GeneratorRegistry.Add(wo.Guid.Full, rNode);
                GeneratorCache.Add(wo.Guid.Full, wo);

                // System.Diagnostics.Debug.WriteLine($"Spawning {GeneratorQueue[index].Slot} in GeneratorQueue for {Guid.Full}");
                wo.EnterWorld();

                // System.Diagnostics.Debug.WriteLine($"Removing {GeneratorQueue[index].Slot} from GeneratorQueue for {Guid.Full}");
                GeneratorQueue.RemoveAt(index);
            }
        }

        public void NotifyGenerator(ObjectGuid target, RegenerationType eventType)
        {
            if (GeneratorDisabled ?? false)
                return;

            if (GeneratorRegistry.ContainsKey(target.Full))
            {
                int slot = (int)GeneratorRegistry[target.Full].Slot;

                if (GeneratorProfiles[slot].WhenCreate == (uint)eventType)
                {
                    GeneratorRegistry.Remove(target.Full);
                    GeneratorCache.Remove(target.Full);
                    QueueGenerator();
                }
            }
        }

        public void NotifyOfEvent(RegenerationType regenerationType)
        {
            if (GeneratorId != null)
            {
                //Console.WriteLine($"{Name}.NotifyOfEvent({regenerationType}) -> {Generator.Name}");

                Generator.NotifyGenerator(Guid, regenerationType);

                GeneratorId = null;
                Generator = null;
            }
        }

        public void SelectMoreGeneratorProfiles()
        {
            if (GeneratorProfilesActive.Count >= GeneratorProfiles.Count)
                return;

            var random = new System.Random((int)DateTime.UtcNow.Ticks);

            if (GeneratorProfiles.Count > 0)
            {
                foreach (var profile in GeneratorProfiles)
                {
                    int slot = GeneratorProfiles.IndexOf(profile);

                    var rng = random.NextDouble();

                    if (rng < profile.Probability || profile.Probability == -1)
                    {
                        if (!GeneratorProfilesActive.Contains(slot))
                            GeneratorProfilesActive.Add(slot);
                    }
                }
            }
        }

        public bool? CurrentlyPoweringUp
        {
            get => GetProperty(PropertyBool.CurrentlyPoweringUp);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.CurrentlyPoweringUp); else SetProperty(PropertyBool.CurrentlyPoweringUp, value.Value); }
        }

        public bool? GeneratorDisabled
        {
            get => GetProperty(PropertyBool.GeneratorDisabled);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.GeneratorDisabled); else SetProperty(PropertyBool.GeneratorDisabled, value.Value); }
        }

        public bool? GeneratorStatus
        {
            get => GetProperty(PropertyBool.GeneratorStatus);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.GeneratorStatus); else SetProperty(PropertyBool.GeneratorStatus, value.Value); }
        }

        public bool? GeneratorAutomaticDestruction
        {
            get => GetProperty(PropertyBool.GeneratorAutomaticDestruction);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.GeneratorAutomaticDestruction); else SetProperty(PropertyBool.GeneratorAutomaticDestruction, value.Value); }
        }

        public string GeneratorEvent
        {
            get => GetProperty(PropertyString.GeneratorEvent);
            set { if (value == null) RemoveProperty(PropertyString.GeneratorEvent); else SetProperty(PropertyString.GeneratorEvent, value); }
        }

        public void CheckEventStatus()
        {
            if (GeneratorEvent is null)
                return;

            if (GeneratorEvent == "")
                return;

            var currentState = GeneratorDisabled ?? false;

            if (!EventManager.IsEventAvailable(GeneratorEvent))
                return;

            var enabled = EventManager.IsEventEnabled(GeneratorEvent);
            var started = EventManager.IsEventStarted(GeneratorEvent);

            GeneratorDisabled = !enabled || !started;

            ProcessGeneratorStatus(currentState);
        }

        public void CheckGeneratorStatus()
        {
            switch (GeneratorTimeType ?? ACE.Entity.Enum.GeneratorTimeType.Undef)
            {
                case ACE.Entity.Enum.GeneratorTimeType.RealTime:
                    CheckRealTimeStatus();
                    break;
                case ACE.Entity.Enum.GeneratorTimeType.Event:
                    CheckEventStatus();
                    break;
            }            
        }

        public GeneratorTimeType? GeneratorTimeType
        {
            get => (GeneratorTimeType?)GetProperty(PropertyInt.GeneratorTimeType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GeneratorTimeType); else SetProperty(PropertyInt.GeneratorTimeType, (int)value.Value); }
        }

        public GeneratorDestruct? GeneratorDestructionType
        {
            get => (GeneratorDestruct?)GetProperty(PropertyInt.GeneratorDestructionType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GeneratorDestructionType); else SetProperty(PropertyInt.GeneratorDestructionType, (int)value.Value); }
        }

        public GeneratorDestruct? GeneratorEndDestructionType
        {
            get => (GeneratorDestruct?)GetProperty(PropertyInt.GeneratorEndDestructionType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GeneratorEndDestructionType); else SetProperty(PropertyInt.GeneratorEndDestructionType, (int)value.Value); }
        }

        public GeneratorType? GeneratorType
        {
            get => (GeneratorType?)GetProperty(PropertyInt.GeneratorType);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GeneratorType); else SetProperty(PropertyInt.GeneratorType, (int)value.Value); }
        }

        public int? GeneratorStartTime
        {
            get => GetProperty(PropertyInt.GeneratorStartTime);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GeneratorStartTime); else SetProperty(PropertyInt.GeneratorStartTime, value.Value); }
        }

        public int? GeneratorEndTime
        {
            get => GetProperty(PropertyInt.GeneratorEndTime);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.GeneratorEndTime); else SetProperty(PropertyInt.GeneratorEndTime, value.Value); }
        }

        public double? GeneratorInitialDelay
        {
            get => GetProperty(PropertyFloat.GeneratorInitialDelay);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.GeneratorInitialDelay); else SetProperty(PropertyFloat.GeneratorInitialDelay, value.Value); }
        }

        public void CheckRealTimeStatus()
        {
            var currentState = GeneratorDisabled ?? false;

            var now = (int)Time.GetTimestamp();

            GeneratorDisabled = !(now >= (GeneratorStartTime ?? 0) && (GeneratorEndTime ?? 0) >= now);

            ProcessGeneratorStatus(currentState);
        }

        public void ProcessGeneratorStatus(bool currentState)
        {
            if (currentState != (GeneratorDisabled ?? false))
            {
                if (currentState == false)
                {
                    // generator has been disabled, de-spawn everything in registry and reset back to defaults
                    switch (GeneratorEndDestructionType ?? GeneratorDestruct.Undef)
                    {
                        case GeneratorDestruct.Kill:
                        //foreach (var wo in GeneratorCache.Values)
                        //    wo.Kill();
                        case GeneratorDestruct.Nothing:
                            break;
                        case GeneratorDestruct.Destroy:
                        default:
                            foreach (var wo in GeneratorCache.Values)
                                wo.Destroy();
                            break;
                    }

                    GeneratorRegistry.Clear();
                    GeneratorCache.Clear();
                    GeneratorQueue.Clear();
                    GeneratorProfilesActive.Clear();

                    InitGeneratedObjects = GetProperty(PropertyInt.InitGeneratedObjects);
                    MaxGeneratedObjects = GetProperty(PropertyInt.MaxGeneratedObjects);
                }
                else
                {
                    // generator has been enabled, execute generator initalization.

                    CurrentlyPoweringUp = true;
                    SelectGeneratorProfiles();
                    UpdateGeneratorInts();
                    QueueGenerator();
                    CurrentlyPoweringUp = false;
                }
            }
        }

        /// <summary>
        /// Called every ~5 seconds for object generators
        /// </summary>
        public void Generator_HeartBeat()
        {
            if (!IsGenerator)
                return;

            // fixme: default properties
            if (!(FirstEnterWorldDone ?? false))
                FirstEnterWorldDone = true;

            CheckGeneratorStatus();

            if (!(GeneratorEnteredWorld ?? false) && (FirstEnterWorldDone ?? false))
            {
                if (!(GeneratorDisabled ?? false))
                {
                    // spawn initial object for this generator
                    CurrentlyPoweringUp = true;
                    SelectGeneratorProfiles();
                    UpdateGeneratorInts();
                    QueueGenerator();
                    CurrentlyPoweringUp = false;
                }
                GeneratorEnteredWorld = true;
            }

            if (!(GeneratorDisabled ?? false))
            {
                if (GeneratorRegistry.Count < InitGeneratedObjects)
                {
                    // subsequent objects / respawning
                    SelectMoreGeneratorProfiles();
                    QueueGenerator();
                }

                if (GeneratorQueue.Count > 0)
                    ProcessGeneratorQueue();
            }
        }
    }
}
