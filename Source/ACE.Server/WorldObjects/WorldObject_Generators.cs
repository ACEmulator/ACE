using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Factories;
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

                    if (random.NextDouble() < profile.Probability) // || profile.Probability == -1)
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

                if (GeneratorRegistry.Count < InitGeneratedObjects && !(GeneratorEnteredWorld ?? false))
                    when = Common.Time.GetTimestamp();

                queue.When = when;

                // System.Diagnostics.Debug.WriteLine($"Adding {queue.Slot} @ {queue.When} to GeneratorQueue for {Guid.Full}");
                GeneratorQueue.Add(queue);

                if (GeneratorQueue.Count >= InitGeneratedObjects)
                    GeneratorEnteredWorld = true;
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
                                        //Location.RotationX + profile.AnglesX ?? 0, Location.RotationY + profile.AnglesY ?? 0, Location.RotationZ + profile.AnglesZ ?? 0, Location.RotationW + profile.AnglesW ?? 0);
                                break;
                            case RegenLocationType.ScatterTreasure:
                            case RegenLocationType.Scatter:
                                float genRadius = (float)(wo.GetProperty(PropertyFloat.GeneratorRadius) ?? 0f);
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
            if (GeneratorRegistry.ContainsKey(target.Full))
            {
                int slot = (int)GeneratorRegistry[target.Full].Slot;

                if (GeneratorProfiles[slot].WhenCreate == (uint)eventType)
                {
                    GeneratorRegistry.Remove(target.Full);
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
    }
}
