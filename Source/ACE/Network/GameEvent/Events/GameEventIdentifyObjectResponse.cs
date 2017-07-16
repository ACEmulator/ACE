using ACE.Entity;
using ACE.Network.Enum;
using System.Linq;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.GameMessages.Messages;
using System.IO;
using System.Collections.Generic;
using System;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventIdentifyObjectResponse : GameEventMessage
    {
        public GameEventIdentifyObjectResponse(Session session, WorldObject obj)
            : base(GameEventType.IdentifyObjectResponse, GameMessageGroup.Group09, session)
        {
            System.Type type = obj.GetType();
            // TODO : calculate if we were successful
            const bool successfulId = true;

            Writer.Write(obj.Guid.Full);

            // Set Flags and collect data for Identify Object Processing

            IdentifyResponseFlags flags = IdentifyResponseFlags.None;

            // Excluding some times that are sent later as weapon status Og II

            var propertiesInt = obj.PropertiesInt.Where(x => x.PropertyId < 9000
                                                          && x.PropertyId != (uint)PropertyInt.Damage
                                                          && x.PropertyId != (uint)PropertyInt.DamageType
                                                          && x.PropertyId != (uint)PropertyInt.WeaponSkill
                                                          && x.PropertyId != (uint)PropertyInt.WeaponTime).ToList();

            if (propertiesInt.Count > 0)
            {
                flags |= IdentifyResponseFlags.IntStatsTable;
            }

            var propertiesInt64 = obj.PropertiesInt64.Where(x => x.PropertyId < 9000).ToList();

            if (propertiesInt64.Count > 0)
            {
                flags |= IdentifyResponseFlags.Int64StatsTable;
            }

            var propertiesBool = obj.PropertiesBool.Where(x => x.PropertyId < 9000).ToList();

            if (propertiesBool.Count > 0)
            {
                flags |= IdentifyResponseFlags.BoolStatsTable;
            }

            // the float values 13 - 19 + 165 (nether added way later) are armor resistance and is shown in a different list. Og II
            // 21-22, 26, 62-63 are all sent as part of the weapons profile and not duplicated.
            var propertiesDouble = obj.PropertiesDouble.Where(x => x.PropertyId < 9000
                                                               && (x.PropertyId < (uint)PropertyDouble.ArmorModVsSlash
                                                               || x.PropertyId > (uint)PropertyDouble.ArmorModVsElectric)
                                                               && x.PropertyId != (uint)PropertyDouble.WeaponLength
                                                               && x.PropertyId != (uint)PropertyDouble.DamageVariance
                                                               && x.PropertyId != (uint)PropertyDouble.MaximumVelocity
                                                               && x.PropertyId != (uint)PropertyDouble.WeaponOffense
                                                               && x.PropertyId != (uint)PropertyDouble.DamageMod
                                                               && x.PropertyId != (uint)PropertyDouble.ArmorModVsNether).ToList();
            if (propertiesDouble.Count > 0)
            {
                flags |= IdentifyResponseFlags.FloatStatsTable;
            }

            var propertiesDid = obj.PropertiesDid.Where(x => x.PropertyId < 9000).ToList();

            if (propertiesDid.Count > 0)
            {
                flags |= IdentifyResponseFlags.DidStatsTable;
            }

            var propertiesString = obj.PropertiesString.Where(x => x.PropertyId < 9000).ToList();
/*
#if DEBUG
            // TODO: This needs to come out - only in while we are testing.
            if (propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.LongDesc)?.PropertyValue == null)
            {
                // No long description - we will send just our debugging information.
                AceObjectPropertiesString dbAo = new AceObjectPropertiesString();
                dbAo.AceObjectId = obj.Guid.Full;
                dbAo.PropertyId = (ushort)PropertyString.LongDesc;
                dbAo.PropertyValue = DebugOutputString(type, obj);
                propertiesString.Add(dbAo);
            }
            else
            {
                // A long description already exists - lets append our data to the end.
                if (!propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.LongDesc).PropertyValue.Contains("ACE Debug Output"))
                    propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.LongDesc).PropertyValue += "\n\n" + DebugOutputString(type, obj);
            }
#endif
*/
            var propertiesSpellId = obj.PropertiesSpellId.ToList();

            if (propertiesSpellId.Count > 0)
            {
                flags |= IdentifyResponseFlags.SpellBook;
            }

            var propertiesArmor = obj.PropertiesDouble.Where(x => (x.PropertyId < 9000
                                                         && (x.PropertyId >= (uint)PropertyDouble.ArmorModVsSlash
                                                         && x.PropertyId <= (uint)PropertyDouble.ArmorModVsElectric))
                                                         || x.PropertyId == (uint)PropertyDouble.ArmorModVsNether).ToList();
            if (propertiesArmor.Count > 0)
            {
                flags |= IdentifyResponseFlags.ArmorProfile;
#if DEBUG
                session.Network.EnqueueSend(new GameMessageSystemChat("Armor Profile is Active, redirecting debug output to chat panel for this object.", ChatMessageType.System));
                session.Network.EnqueueSend(new GameMessageSystemChat(propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.LongDesc).PropertyValue, ChatMessageType.System));
#endif
            }

            // Weapons Profile
            var propertiesWeaponsD = obj.PropertiesDouble.Where(x => x.PropertyId < 9000
                                                            && (x.PropertyId == (uint)PropertyDouble.WeaponLength
                                                            || x.PropertyId == (uint)PropertyDouble.DamageVariance
                                                            || x.PropertyId == (uint)PropertyDouble.MaximumVelocity
                                                            || x.PropertyId == (uint)PropertyDouble.WeaponOffense
                                                            || x.PropertyId == (uint)PropertyDouble.DamageMod)).ToList();

            var propertiesWeaponsI = obj.PropertiesInt.Where(x => x.PropertyId < 9000
                                                         && (x.PropertyId == (uint)PropertyInt.Damage
                                                         || x.PropertyId == (uint)PropertyInt.DamageType
                                                         || x.PropertyId == (uint)PropertyInt.WeaponSkill
                                                         || x.PropertyId == (uint)PropertyInt.WeaponTime)).ToList();

            if (propertiesWeaponsI.Count + propertiesWeaponsD.Count > 0)
            {
                flags |= IdentifyResponseFlags.WeaponProfile;
#if DEBUG
                session.Network.EnqueueSend(new GameMessageSystemChat("Weapon Profile is Active, redirecting debug output to chat panel for this object.", ChatMessageType.System));
                session.Network.EnqueueSend(new GameMessageSystemChat(propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.LongDesc).PropertyValue, ChatMessageType.System));
#endif
            }

            if (propertiesString.Count > 0)
            {
                flags |= IdentifyResponseFlags.StringStatsTable;
            }

            // if PropertyPool.NpcLooksLikeObject == true, do not treat as creature data
            var npcLooksLikeObject = propertiesBool.FirstOrDefault(x => x.PropertyId == (uint)PropertyBool.NpcLooksLikeObject)?.PropertyValue;
            if (obj.ItemType == ItemType.Creature && npcLooksLikeObject != true)
            {
                flags |= IdentifyResponseFlags.CreatureProfile;
#if DEBUG
                session.Network.EnqueueSend(new GameMessageSystemChat("Creature Profile is Active, redirecting debug output to chat panel for this object.", ChatMessageType.System));
                session.Network.EnqueueSend(new GameMessageSystemChat(propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.LongDesc).PropertyValue, ChatMessageType.System));
#endif
            }

            // Ok Down to business - let's identify all of this stuff.
            WriteIdentifyObjectHeader(Writer, flags, successfulId);
            WriteIdentifyObjectIntProperties(Writer, flags, propertiesInt);
            WriteIdentifyObjectInt64Properties(Writer, flags, propertiesInt64);
            WriteIdentifyObjectBoolProperties(Writer, flags, propertiesBool);
            WriteIdentifyObjectDoubleProperties(Writer, flags, propertiesDouble);
            WriteIdentifyObjectStringsProperties(Writer, flags, propertiesString);
            WriteIdentifyObjectDidProperties(Writer, flags, propertiesDid);
            WriteIdentifyObjectSpellIdProperties(Writer, flags, propertiesSpellId);
            WriteIdentifyObjectArmorProfile(Writer, flags, propertiesArmor);
            // TODO: There are probably other checks that need to be made here
            if (obj.ItemType == ItemType.Creature && type.Name != "DebugObject")
            {                
                WriteIdentifyObjectCreatureProfile(Writer, (Creature)obj);
            }
            WriteIdentifyObjectWeaponsProfile(Writer, flags, propertiesWeaponsD, propertiesWeaponsI);
        }

        private string DebugOutputString(System.Type type, WorldObject obj)
        {
            string debugOutput = "ACE Debug Output:\n";
            debugOutput += "ACE Class File: " + type.Name + ".cs" + "\n";
            debugOutput += "AceObjectId: " + obj.Guid.Full.ToString() + " (0x" + obj.Guid.Full.ToString("X") + ")" + "\n";

            foreach (var prop in obj.GetType().GetProperties())
            {
                if (prop.GetValue(obj, null) == null)
                    continue;

                switch (prop.Name.ToLower())
                {
                    case "guid":
                        debugOutput += $"{prop.Name} = {obj.Guid.Full.ToString()}" + "\n";
                        break;
                    case "descriptionflags":
                        debugOutput += $"{prop.Name} = {obj.DescriptionFlags.ToString()}" + " (" + (uint)obj.DescriptionFlags + ")" + "\n";
                        break;
                    case "weenieflags":
                        debugOutput += $"{prop.Name} = {obj.WeenieFlags.ToString()}" + " (" + (uint)obj.WeenieFlags + ")" + "\n";
                        break;
                    case "weenieflags2":
                        debugOutput += $"{prop.Name} = {obj.WeenieFlags2.ToString()}" + " (" + (uint)obj.WeenieFlags2 + ")" + "\n";
                        break;
                    case "positionflag":
                        debugOutput += $"{prop.Name} = {obj.PositionFlag.ToString()}" + " (" + (uint)obj.PositionFlag + ")" + "\n";
                        break;
                    case "itemtype":
                        debugOutput += $"{prop.Name} = {obj.ItemType.ToString()}" + " (" + (uint)obj.ItemType + ")" + "\n";
                        break;
                    case "containertype":
                        debugOutput += $"{prop.Name} = {obj.ContainerType.ToString()}" + " (" + (uint)obj.ContainerType + ")" + "\n";
                        break;
                    case "usable":
                        debugOutput += $"{prop.Name} = {obj.Usable.ToString()}" + " (" + (uint)obj.Usable + ")" + "\n";
                        break;
                    case "radarbehavior":
                        debugOutput += $"{prop.Name} = {obj.RadarBehavior.ToString()}" + " (" + (uint)obj.RadarBehavior + ")" + "\n";
                        break;
                    case "physicsdescriptionflags":
                        debugOutput += $"{prop.Name} = {obj.PhysicsDescriptionFlag.ToString()}" + " (" + (uint)obj.PhysicsDescriptionFlag + ")" + "\n";
                        break;
                    case "physicsstate":
                        debugOutput += $"{prop.Name} = {obj.PhysicsState.ToString()}" + " (" + (uint)obj.PhysicsState + ")" + "\n";
                        break;
                    case "propertiesint":
                        foreach (var item in obj.PropertiesInt)
                        {
                            debugOutput += $"PropertyInt.{System.Enum.GetName(typeof(PropertyInt), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                            break;
                    case "propertiesint64":
                        foreach (var item in obj.PropertiesInt64)
                        {
                            debugOutput += $"PropertyInt64.{System.Enum.GetName(typeof(PropertyInt64), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                        break;
                    case "propertiesbool":
                        foreach (var item in obj.PropertiesBool)
                        {
                            debugOutput += $"PropertyBool.{System.Enum.GetName(typeof(PropertyBool), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                        break;
                    case "propertiesstring":
                        foreach (var item in obj.PropertiesString)
                        {
                            debugOutput += $"PropertyString.{System.Enum.GetName(typeof(PropertyString), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                        break;
                    case "propertiesdouble":
                        foreach (var item in obj.PropertiesDouble)
                        {
                            debugOutput += $"PropertyDouble.{System.Enum.GetName(typeof(PropertyDouble), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                        break;
                    case "propertiesdid":
                        foreach (var item in obj.PropertiesDid)
                        {
                            debugOutput += $"PropertyDataId.{System.Enum.GetName(typeof(PropertyDataId), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                        break;
                    case "propertiesiid":
                        foreach (var item in obj.PropertiesIid)
                        {
                            debugOutput += $"PropertyInstanceId.{System.Enum.GetName(typeof(PropertyInstanceId), item.PropertyId)} ({item.PropertyId}) = {item.PropertyValue}" + "\n";
                        }
                        break;
                    case "propertiesspellid":
                        foreach (var item in obj.PropertiesSpellId)
                        {
                            debugOutput += $"PropertySpellId.{System.Enum.GetName(typeof(Spell), item.SpellId)} ({item.SpellId})" + "\n";
                        }
                        break;
                    case "validlocations":
                        debugOutput += $"{prop.Name} = {obj.ValidLocations}" + " (" + (uint)obj.ValidLocations + ")" + "\n";
                        break;
                    case "currentwieldedlocation":
                        debugOutput += $"{prop.Name} = {obj.CurrentWieldedLocation}" + " (" + (uint)obj.CurrentWieldedLocation + ")" + "\n";
                        break;
                    case "priority":
                        debugOutput += $"{prop.Name} = {obj.Priority}" + " (" + (uint)obj.Priority + ")" + "\n";
                        break;
                    case "radarcolor":
                        debugOutput += $"{prop.Name} = {obj.RadarColor}" + " (" + (uint)obj.RadarColor + ")" + "\n";
                        break;
                    default:
                        debugOutput += $"{prop.Name} = {prop.GetValue(obj, null)}" + "\n";
                        break;
                }
            }

            return debugOutput;
        }

        private static void WriteIdentifyObjectHeader(BinaryWriter writer, IdentifyResponseFlags flags, bool success)
        {
            writer.Write((uint)flags); // Flags
            writer.Write(Convert.ToUInt32(success)); // Success bool
        }

        private static void WriteIdentifyObjectIntProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesInt> propertiesInt)
        {
            const ushort tableSize = 16;
            var notNull = propertiesInt.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.IntStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesInt x in notNull)
            {
                writer.Write(x.PropertyId);
                writer.Write(x.PropertyValue.Value);
            }
        }

        private static void WriteIdentifyObjectInt64Properties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesInt64> propertiesInt64)
        {
            const ushort tableSize = 8;
            var notNull = propertiesInt64.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.Int64StatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesInt64 x in notNull)
            {
                writer.Write(x.PropertyId);
                writer.Write(x.PropertyValue.Value);
            }
        }

        private static void WriteIdentifyObjectBoolProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesBool> propertiesBool)
        {
            const ushort tableSize = 8;
            var notNull = propertiesBool.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.BoolStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesBool x in notNull)
            {
                writer.Write(x.PropertyId);
                writer.Write(Convert.ToUInt32(x.PropertyValue.Value));
            }
        }

        private static void WriteIdentifyObjectDoubleProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDouble> propertiesDouble)
        {
            const ushort tableSize = 8;
            var notNull = propertiesDouble.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.FloatStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesDouble x in notNull)
            {
                writer.Write((uint)x.PropertyId);
                writer.Write(x.PropertyValue.Value);
            }
        }

        private static void WriteIdentifyObjectStringsProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesString> propertiesStrings)
        {
            const ushort tableSize = 8;
            var notNull = propertiesStrings.Where(p => !string.IsNullOrWhiteSpace(p.PropertyValue)).ToList();
            if ((flags & IdentifyResponseFlags.StringStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesString x in notNull)
            {
                writer.Write((uint)x.PropertyId);
                writer.WriteString16L(x.PropertyValue);
            }
        }

        private static void WriteIdentifyObjectDidProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDataId> propertiesDid)
        {
            const ushort tableSize = 16;
            var notNull = propertiesDid.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.DidStatsTable) == 0 || (notNull.Count == 0)) return;
            writer.Write((ushort)notNull.Count);
            writer.Write(tableSize);

            foreach (AceObjectPropertiesDataId x in notNull)
            {
                writer.Write(x.PropertyId);
                writer.Write(x.PropertyValue.Value);
            }
        }

        private static void WriteIdentifyObjectSpellIdProperties(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesSpell> propertiesSpellId)
        {
            if ((flags & IdentifyResponseFlags.SpellBook) == 0 || (propertiesSpellId.Count == 0)) return;
            writer.Write((uint)propertiesSpellId.Count);

            foreach (AceObjectPropertiesSpell x in propertiesSpellId)
            {
                writer.Write(x.SpellId);
            }
        }

        private static void WriteIdentifyObjectArmorProfile(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDouble> propertiesArmor)
        {
            var notNull = propertiesArmor.Where(p => p.PropertyValue != null).ToList();
            if ((flags & IdentifyResponseFlags.ArmorProfile) == 0 || (notNull.Count == 0)) return;

            foreach (AceObjectPropertiesDouble x in notNull)
            {
                writer.Write((float)x.PropertyValue.Value);
            }
        }

        private static void WriteIdentifyObjectCreatureProfile(BinaryWriter writer, Creature obj)
        {
            uint header = 8;
            // TODO: for now, we are always succeeding - will need to set this to 0 header for failure.   Og II
            writer.Write(header);
            writer.Write(obj.Health.Current);
            writer.Write(obj.Health.MaxValue);
            if (header == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    writer.Write(0u);
                }
            }
            else
            {
                // TODO: we probably need buffed values here  it may be set my the last flag I don't understand yet. - will need to revisit. Og II
                writer.Write(obj.Strength.UnbuffedValue);
                writer.Write(obj.Endurance.UnbuffedValue);
                writer.Write(obj.Quickness.UnbuffedValue);
                writer.Write(obj.Coordination.UnbuffedValue);
                writer.Write(obj.Focus.UnbuffedValue);
                writer.Write(obj.Self.UnbuffedValue);
                writer.Write(obj.Stamina.UnbuffedValue);
                writer.Write(obj.Mana.UnbuffedValue);
                writer.Write(obj.Stamina.MaxValue);
                writer.Write(obj.Mana.MaxValue);
                // this only gets sent if the header can be masked with 1
                // Writer.Write(0u);
            }
        }

        private static void WriteIdentifyObjectWeaponsProfile(
            BinaryWriter writer,
            IdentifyResponseFlags flags,
            List<AceObjectPropertiesDouble> propertiesWeaponsD,
            List<AceObjectPropertiesInt> propertiesWeaponsI)
        {
            if ((flags & IdentifyResponseFlags.WeaponProfile) == 0) return;
            writer.Write(propertiesWeaponsI.Find(x => x.PropertyId == (uint)PropertyInt.DamageType)?.PropertyValue ?? 0u);
            // Signed
            writer.Write((int?)propertiesWeaponsI.Find(x => x.PropertyId == (int)PropertyInt.WeaponTime)?.PropertyValue ?? 0);
            writer.Write(propertiesWeaponsI.Find(x => x.PropertyId == (uint)PropertyInt.WeaponSkill)?.PropertyValue ?? 0u);
            // Signed
            writer.Write((int?)propertiesWeaponsI.Find(x => x.PropertyId == (int)PropertyInt.Damage)?.PropertyValue ?? 0);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.DamageVariance)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.DamageMod)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.WeaponLength)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.MaximumVelocity)?.PropertyValue ?? 0.00);
            writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.WeaponOffense)?.PropertyValue ?? 0.00);
            // This one looks to be 0 - I did not find one with this calculated.   It is called Max Velocity Calculated
            writer.Write(0u);
        }
    }
}
