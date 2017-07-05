using ACE.Entity;
using ACE.Network.Enum;
using System.Linq;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventIdentifyObjectResponse : GameEventMessage
    {
        public GameEventIdentifyObjectResponse(Session session, ObjectGuid objectId, WorldObject obj)
            : base(GameEventType.IdentifyObjectResponse, GameMessageGroup.Group09, session)
        {
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

            // TODO: This needs to come out - only in while we are testing.
            if (propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.ShortDesc)?.PropertyValue == null)
            {
                // No short description - we will send just our debugging information.
                string debugOutput = "baseAceObjectId: " + objectId + " (0x" + objectId.Full.ToString("X") + ")";
                debugOutput += "\n" + "weenieClassId: " + obj.WeenieClassid + " (0x" + obj.WeenieClassid.ToString("X") + ")";
                debugOutput += "\n" + "Object Type: " + obj.Type;
                debugOutput += "\n" + "defaultCombatStyle: " + obj.DefaultCombatStyle;
                AceObjectPropertiesString dbAo = new AceObjectPropertiesString();
                dbAo.AceObjectId = obj.Guid.Full;
                dbAo.PropertyId = (ushort)PropertyString.ShortDesc;
                dbAo.PropertyValue = debugOutput;
                propertiesString.Add(dbAo);
            }
            else
            {
                // A short description already exists - lets append our data to the end.
                string debugOutput = "\n" + "baseAceObjectId: " + objectId + " (0x" + objectId.Full.ToString("X") + ")";
                debugOutput += "\n" + "weenieClassId: " + obj.WeenieClassid + " (0x" + obj.WeenieClassid.ToString("X") + ")";
                debugOutput += "\n" + "Object Type: " + obj.Type;
                debugOutput += "\n" + "defaultCombatStyle: " + obj.DefaultCombatStyle;
                propertiesString.Find(x => x.PropertyId == (ushort)PropertyString.ShortDesc).PropertyValue += debugOutput;
            }

            if (propertiesString.Count > 0)
            {
                flags |= IdentifyResponseFlags.StringStatsTable;
            }

            if (obj.Type == ObjectType.Creature)
            {
                flags |= IdentifyResponseFlags.CreatureProfile;
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
            if (obj.Type == ObjectType.Creature)
            {
                session.Player.WriteIdentifyObjectCreatureProfile(Writer, (Creature)obj);
            }
            session.Player.WriteIdentifyObjectWeaponsProfile(Writer, flags, propertiesWeaponsD, propertiesWeaponsI);
        }
    }
}