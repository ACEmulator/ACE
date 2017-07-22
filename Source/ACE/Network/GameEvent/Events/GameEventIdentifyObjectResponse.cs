using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using System;
using System.Linq;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventIdentifyObjectResponse : GameEventMessage
    {
        public GameEventIdentifyObjectResponse(Session session, WorldObject obj, bool success)
            : base(GameEventType.IdentifyObjectResponse, GameMessageGroup.Group09, session)
        {
            IdentifyResponseFlags flags = IdentifyResponseFlags.None;

            // TODO : calculate if we were successful
            // const bool successfulId = true;
            Writer.Write(obj.Guid.Full);

            obj.SerializeIdentifyObjectResponse(Writer, success, flags);

            ////Writer.Write((uint)flags); // Flags
            ////Writer.Write(Convert.ToUInt32(true)); // Success bool

            ////var propertiesInt = obj.PropertiesInt.Where(x => x.PropertyId < 9000
            ////                                              && x.PropertyId != (uint)PropertyInt.Damage
            ////                                              && x.PropertyId != (uint)PropertyInt.DamageType
            ////                                              && x.PropertyId != (uint)PropertyInt.WeaponSkill
            ////                                              && x.PropertyId != (uint)PropertyInt.WeaponTime).ToList();

            ////if (propertiesInt.Count > 0)
            ////{
            ////    flags |= IdentifyResponseFlags.IntStatsTable;
            ////}

            ////var propertiesInt64 = obj.PropertiesInt64.Where(x => x.PropertyId < 9000).ToList();

            ////if (propertiesInt64.Count > 0)
            ////{
            ////    flags |= IdentifyResponseFlags.Int64StatsTable;
            ////}

            ////var propertiesBool = obj.PropertiesBool.Where(x => x.PropertyId < 9000).ToList();

            ////if (propertiesBool.Count > 0)
            ////{
            ////    flags |= IdentifyResponseFlags.BoolStatsTable;
            ////}

            ////// the float values 13 - 19 + 165 (nether added way later) are armor resistance and is shown in a different list. Og II
            ////// 21-22, 26, 62-63 are all sent as part of the weapons profile and not duplicated.
            ////var propertiesDouble = obj.PropertiesDouble.Where(x => x.PropertyId < 9000
            ////                                                   && (x.PropertyId < (uint)PropertyDouble.ArmorModVsSlash
            ////                                                   || x.PropertyId > (uint)PropertyDouble.ArmorModVsElectric)
            ////                                                   && x.PropertyId != (uint)PropertyDouble.WeaponLength
            ////                                                   && x.PropertyId != (uint)PropertyDouble.DamageVariance
            ////                                                   && x.PropertyId != (uint)PropertyDouble.MaximumVelocity
            ////                                                   && x.PropertyId != (uint)PropertyDouble.WeaponOffense
            ////                                                   && x.PropertyId != (uint)PropertyDouble.DamageMod
            ////                                                   && x.PropertyId != (uint)PropertyDouble.ArmorModVsNether).ToList();
            ////if (propertiesDouble.Count > 0)
            ////{
            ////    flags |= IdentifyResponseFlags.FloatStatsTable;
            ////}

            ////var propertiesDid = obj.PropertiesDid.Where(x => x.PropertyId < 9000).ToList();

            ////if (propertiesDid.Count > 0)
            ////{
            ////    flags |= IdentifyResponseFlags.DidStatsTable;
            ////}

            ////var propertiesString = obj.PropertiesString.Where(x => x.PropertyId < 9000).ToList();

            ////var propertiesSpellId = obj.PropertiesSpellId.ToList();

            ////if (propertiesSpellId.Count > 0)
            ////{
            ////    flags |= IdentifyResponseFlags.SpellBook;
            ////}

            ////// TODO: Move to Armor class
            ////var propertiesArmor = obj.PropertiesDouble.Where(x => (x.PropertyId < 9000
            ////                                             && (x.PropertyId >= (uint)PropertyDouble.ArmorModVsSlash
            ////                                             && x.PropertyId <= (uint)PropertyDouble.ArmorModVsElectric))
            ////                                             || x.PropertyId == (uint)PropertyDouble.ArmorModVsNether).ToList();
            ////if (propertiesArmor.Count > 0)
            ////{
            ////    flags |= IdentifyResponseFlags.ArmorProfile;
            ////}

            ////// TODO: Move to Weapon class
            ////// Weapons Profile
            ////var propertiesWeaponsD = PropertiesDouble.Where(x => x.PropertyId < 9000
            ////                                                && (x.PropertyId == (uint)PropertyDouble.WeaponLength
            ////                                                || x.PropertyId == (uint)PropertyDouble.DamageVariance
            ////                                                || x.PropertyId == (uint)PropertyDouble.MaximumVelocity
            ////                                                || x.PropertyId == (uint)PropertyDouble.WeaponOffense
            ////                                                || x.PropertyId == (uint)PropertyDouble.DamageMod)).ToList();

            ////var propertiesWeaponsI = PropertiesInt.Where(x => x.PropertyId < 9000
            ////                                             && (x.PropertyId == (uint)PropertyInt.Damage
            ////                                             || x.PropertyId == (uint)PropertyInt.DamageType
            ////                                             || x.PropertyId == (uint)PropertyInt.WeaponSkill
            ////                                             || x.PropertyId == (uint)PropertyInt.WeaponTime)).ToList();

            ////if (propertiesWeaponsI.Count + propertiesWeaponsD.Count > 0)
            ////{
            ////    flags |= IdentifyResponseFlags.WeaponProfile;
            ////}

            ////if (propertiesString.Count > 0)
            ////{
            ////    flags |= IdentifyResponseFlags.StringStatsTable;
            ////}

            ////// obj.WriteIdentifyObjectHeader(writer, flags, successfulId);
            ////WriteIdentifyObjectIntProperties(writer, flags, propertiesInt);
            ////WriteIdentifyObjectInt64Properties(writer, flags, propertiesInt64);
            ////WriteIdentifyObjectBoolProperties(writer, flags, propertiesBool);
            ////WriteIdentifyObjectDoubleProperties(writer, flags, propertiesDouble);
            ////WriteIdentifyObjectStringsProperties(writer, flags, propertiesString);
            ////WriteIdentifyObjectDidProperties(writer, flags, propertiesDid);
            ////WriteIdentifyObjectSpellIdProperties(writer, flags, propertiesSpellId);

            ////// TODO: Move to Armor class
            ////WriteIdentifyObjectArmorProfile(writer, flags, propertiesArmor);

            ////////// TODO: There are probably other checks that need to be made here
            ////////if (ItemType == ItemType.Creature && GetType().Name != "DebugObject")
            ////////{
            ////////    WriteIdentifyObjectCreatureProfile(writer, (Creature)this);
            ////////}

            ////// TODO: Move to Weapon class
            ////WriteIdentifyObjectWeaponsProfile(writer, flags, propertiesWeaponsD, propertiesWeaponsI);
        }
    }
}
