using ACE.Entity;
using ACE.Network.Enum;
using System.Linq;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.GameMessages.Messages;

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

#if DEBUG
            // TODO: This needs to come out - only in while we are testing.
            if (propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.ShortDesc)?.PropertyValue == null)
            {
                // No short description - we will send just our debugging information.
                AceObjectPropertiesString dbAo = new AceObjectPropertiesString();
                dbAo.AceObjectId = obj.Guid.Full;
                dbAo.PropertyId = (ushort)PropertyString.ShortDesc;
                dbAo.PropertyValue = DebugOutputString(type, obj);
                propertiesString.Add(dbAo);
            }
            else
            {
                // A short description already exists - lets append our data to the end.
                if (!propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.ShortDesc).PropertyValue.Contains("ACE Debug Output"))
                    propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.ShortDesc).PropertyValue += "\n\n" + DebugOutputString(type, obj);
            }
#endif

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
                session.Network.EnqueueSend(new GameMessageSystemChat(propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.ShortDesc).PropertyValue, ChatMessageType.System));
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
                session.Network.EnqueueSend(new GameMessageSystemChat(propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.ShortDesc).PropertyValue, ChatMessageType.System));
#endif
            }

            if (propertiesString.Count > 0)
            {
                flags |= IdentifyResponseFlags.StringStatsTable;
            }

            if (obj.Type == ObjectType.Creature)
            {
                flags |= IdentifyResponseFlags.CreatureProfile;
#if DEBUG
                session.Network.EnqueueSend(new GameMessageSystemChat("Creature Profile is Active, redirecting debug output to chat panel for this object.", ChatMessageType.System));
                session.Network.EnqueueSend(new GameMessageSystemChat(propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.ShortDesc).PropertyValue, ChatMessageType.System));
#endif
            }

            // Ok Down to business - let's identify all of this stuff.
            session.Player.WriteIdentifyObjectHeader(Writer, flags, successfulId);
            session.Player.WriteIdentifyObjectIntProperties(Writer, flags, propertiesInt);
            session.Player.WriteIdentifyObjectInt64Properties(Writer, flags, propertiesInt64);
            session.Player.WriteIdentifyObjectBoolProperties(Writer, flags, propertiesBool);
            session.Player.WriteIdentifyObjectDoubleProperties(Writer, flags, propertiesDouble);
            session.Player.WriteIdentifyObjectStringsProperties(Writer, flags, propertiesString);
            session.Player.WriteIdentifyObjectDidProperties(Writer, flags, propertiesDid);
            session.Player.WriteIdentifyObjectSpellIdProperties(Writer, flags, propertiesSpellId);
            session.Player.WriteIdentifyObjectArmorProfile(Writer, flags, propertiesArmor);
            // TODO: There are probably other checks that need to be made here
            if (obj.Type == ObjectType.Creature && type.Name != "DebugObject")
            {                
                session.Player.WriteIdentifyObjectCreatureProfile(Writer, (Creature)obj);
            }
            session.Player.WriteIdentifyObjectWeaponsProfile(Writer, flags, propertiesWeaponsD, propertiesWeaponsI);
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
                    case "type":
                        debugOutput += $"{prop.Name} = {obj.Type.ToString()}" + " (" + (uint)obj.Type + ")" + "\n";
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
                    default:
                        debugOutput += $"{prop.Name} = {prop.GetValue(obj, null)}" + "\n";
                        break;
                }
            }

            return debugOutput;
        }
    }
}
