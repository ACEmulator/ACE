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
                double when = Common.Time.GetFutureTimestamp((RegenerationInterval ?? 0) + (GeneratorProfiles[slot].Delay ?? 0));

                if (GeneratorRegistry.Count < InitGeneratedObjects)
                    if (CurrentlyPoweringUp ?? false)
                        when = Common.Time.GetFutureTimestamp((GeneratorInitialDelay ?? 0));

                queue.When = when;

                // System.Diagnostics.Debug.WriteLine($"Adding {queue.Slot} @ {queue.When} to GeneratorQueue for {Guid.Full}");
                GeneratorQueue.Add(queue);
            }
        }

        public void ProcessGeneratorQueue()
        {
            var index = 0;
            while (index < GeneratorQueue.Count)
            {
                double ts = Common.Time.GetTimestamp();
                if (ts >= GeneratorQueue[index].When)
                {
                    if (GeneratorRegistry.Count >= MaxGeneratedObjects)
                    {
                        // System.Diagnostics.Debug.WriteLine($"GeneratorRegistry for {Guid.Full} is at MaxGeneratedObjects {MaxGeneratedObjects}");
                        // System.Diagnostics.Debug.WriteLine($"Removing {GeneratorQueue[index].Slot} from GeneratorQueue for {Guid.Full}");
                        GeneratorQueue.RemoveAt(index);
                        index++;
                        continue;
                    }
                    var profile = GeneratorProfiles[(int)GeneratorQueue[index].Slot];

                    var rNode = new GeneratorRegistryNode();

                    rNode.WeenieClassId = profile.WeenieClassId;
                    rNode.Timestamp = Common.Time.GetTimestamp();
                    rNode.Slot = GeneratorQueue[index].Slot;

                    var wo = WorldObjectFactory.CreateNewWorldObject(profile.WeenieClassId);

                    if (wo != null)
                    {
                        switch ((RegenLocationType)profile.WhereCreate)
                        {
                            case RegenLocationType.SpecificTreasure:
                            case RegenLocationType.Specific:
                                if ((profile.ObjCellId ?? 0) > 0)
                                    wo.Location = new ACE.Entity.Position(profile.ObjCellId ?? 0,
                                        profile.OriginX ?? 0, profile.OriginY ?? 0, profile.OriginZ ?? 0,
                                        profile.AnglesX ?? 0, profile.AnglesY ?? 0, profile.AnglesZ ?? 0, profile.AnglesW ?? 0);
                                else
                                    wo.Location = new ACE.Entity.Position(Location.Cell,
                                        Location.PositionX + profile.OriginX ?? 0, Location.PositionY + profile.OriginY ?? 0, Location.PositionZ + profile.OriginZ ?? 0,
                                        profile.AnglesX ?? 0, profile.AnglesY ?? 0, profile.AnglesZ ?? 0, profile.AnglesW ?? 0);
                                break;
                            case RegenLocationType.ScatterTreasure:
                            case RegenLocationType.Scatter:
                                float genRadius = (float)(GetProperty(PropertyFloat.GeneratorRadius) ?? 0f);
                                var random_x = Physics.Common.Random.RollDice(genRadius * -1, genRadius);
                                var random_y = Physics.Common.Random.RollDice(genRadius * -1, genRadius);
                                var pos = new Physics.Common.Position(Location);

                                if ((pos.ObjCellID & 0xFFFF) < 0x100) // Based on GDL scatter
                                {
                                    pos.Frame.Origin += new System.Numerics.Vector3(random_x, random_y, 0.0f);
                                    pos.Frame.Origin.Z = LScape.get_landblock(pos.ObjCellID).GetZ(pos.Frame.Origin);

                                    if (pos.Frame.Origin.X < 0.5f)
                                        pos.Frame.Origin.X = 0.5f;
                                    if (pos.Frame.Origin.Y < 0.5f)
                                        pos.Frame.Origin.Y = 0.5f;

                                    wo.Location = new ACE.Entity.Position(pos.ObjCellID, pos.Frame.Origin.X, pos.Frame.Origin.Y, pos.Frame.Origin.Z, pos.Frame.Orientation.X, pos.Frame.Orientation.Y, pos.Frame.Orientation.Z, pos.Frame.Orientation.W);
                                }
                                else
                                {
                                    pos.Frame.Origin += new System.Numerics.Vector3(random_x, random_y, 0.0f);

                                    wo.Location = new ACE.Entity.Position(pos.ObjCellID, pos.Frame.Origin.X, pos.Frame.Origin.Y, pos.Frame.Origin.Z, pos.Frame.Orientation.X, pos.Frame.Orientation.Y, pos.Frame.Orientation.Z, pos.Frame.Orientation.W);
                                }
                                    break;
                            default:
                                wo.Location = Location;
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
                    else
                    {
                        // System.Diagnostics.Debug.WriteLine($"Removing {GeneratorQueue[index].Slot} from GeneratorQueue for {Guid.Full} because wcid {rNode.WeenieClassId} is not in the database");
                        GeneratorQueue.RemoveAt(index);
                    }
                }
                else
                    index++;
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
                                wo.Destory();
                            break;
                    }

                    GeneratorRegistry.Clear();
                    GeneratorCache.Clear();
                    GeneratorQueue.Clear();
                    GeneratorProfilesActive.Clear();

                    InitGeneratedObjects = Biota.GetProperty(PropertyInt.InitGeneratedObjects, biotaPropertiesIntLock);
                    MaxGeneratedObjects = Biota.GetProperty(PropertyInt.MaxGeneratedObjects, biotaPropertiesIntLock);
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
                                wo.Destory();
                            break;
                    }

                    GeneratorRegistry.Clear();
                    GeneratorCache.Clear();
                    GeneratorQueue.Clear();
                    GeneratorProfilesActive.Clear();

                    InitGeneratedObjects = Biota.GetProperty(PropertyInt.InitGeneratedObjects, biotaPropertiesIntLock);
                    MaxGeneratedObjects = Biota.GetProperty(PropertyInt.MaxGeneratedObjects, biotaPropertiesIntLock);
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
    }
}
