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
        public GameEventIdentifyObjectResponse(Session session, ObjectGuid objectId, WorldObject obj)
            : base(GameEventType.IdentifyObjectResponse, GameMessageGroup.Group09, session)
        {
            System.Type type = obj.GetType();
            // TODO : calculate if we were successful
            const bool successfulId = true;

            Writer.Write(objectId.Full);

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
                dbAo.PropertyValue = DebugOutputString(type, obj, objectId);
                propertiesString.Add(dbAo);
            }
            else
            {
                // A short description already exists - lets append our data to the end.
                if (!propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.ShortDesc).PropertyValue.Contains("ACE Debug Output"))
                    propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.ShortDesc).PropertyValue += "\n\n" + DebugOutputString(type, obj, objectId);
            }
#endif

            if (propertiesString.Count > 0)
            {
                flags |= IdentifyResponseFlags.StringStatsTable;
            }

            if (obj.Type == ObjectType.Creature)
            {
                flags |= IdentifyResponseFlags.CreatureProfile;
#if DEBUG
                session.Network.EnqueueSend(new GameMessageSystemChat("Creature Panel is Active, redirecting debug output to chat panel for this object.", ChatMessageType.System));
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

        private string DebugOutputString(System.Type type, WorldObject obj, ObjectGuid objectId)
        {
            string debugOutput = "ACE Debug Output:\n";
            debugOutput += "Class: " + type.Name + ".cs" + "\n";
            debugOutput += "AceObjectId: " + objectId.Full.ToString() + " (0x" + objectId.Full.ToString("X") + ")" + "\n";
            ////debugOutput += "WeenieClassId: " + obj.WeenieClassId + " (0x" + obj.WeenieClassId.ToString("X") + ")" + "\n";
            ////debugOutput += "Item Type: " + obj.Type + "\n";
            ////if (obj.Name != null)
            ////    debugOutput += "\n" + "Name: " + obj.Name + "\n";
            ////if (obj.NamePlural != null)
            ////    debugOutput += "\n" + "NamePlural: " + obj.NamePlural + "\n";
            ////if (obj.DefaultCombatStyle != null)
            ////    debugOutput += "DefaultCombatStyle: " + obj.DefaultCombatStyle + "\n";

            foreach (var prop in obj.GetType().GetProperties())
            {
                // Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(obj, null));
               debugOutput += $"{prop.Name} = {prop.GetValue(obj, null)}" + "\n";
            }

            return debugOutput;
        }
    }
}
