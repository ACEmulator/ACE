using ACE.Entity;
using ACE.Network.Enum;
using System;
using System.Linq;

namespace ACE.Network.GameEvent.Events
{
    using global::ACE.Entity.Enum;
    using global::ACE.Entity.Enum.Properties;

    public class GameEventIdentifyObjectResponse : GameEventMessage
    {
        public GameEventIdentifyObjectResponse(Session session, ObjectGuid objectId, WorldObject obj)
            : base(GameEventType.IdentifyObjectResponse, GameMessageGroup.Group09, session)
        {
            Writer.Write(objectId.Full);

            IdentifyResponseFlags flags = IdentifyResponseFlags.None;

            // Started adding real data to this - I needed the information to debug combat stance
            // we really need to kill debug object.   Og II
            // flags = IdentifyResponseFlags.StringStatsTable;

            // Excluding some times that are sent later as weapon status Og II

            var propertiesInt =
                obj.PropertiesInt.Where(
                    x =>
                        x.PropertyId < 9000 && x.PropertyId != 44 && x.PropertyId != 45 && x.PropertyId != 48
                        && x.PropertyId != 49).ToList();
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
            var propertiesDouble =
                obj.PropertiesDouble.Where(
                    x =>
                        x.PropertyId < 9000 && (x.PropertyId < 13 || x.PropertyId > 19) && x.PropertyId != 21
                        && x.PropertyId != 22 && x.PropertyId != 26 && x.PropertyId != 62 && x.PropertyId != 63
                        && x.PropertyId != 165).ToList();
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

            var propertiesArmor =
                obj.PropertiesDouble.Where(
                        x => (x.PropertyId < 9000 && (x.PropertyId >= 13 && x.PropertyId <= 19)) || x.PropertyId == 165)
                    .ToList();
            if (propertiesArmor.Count > 0)
            {
                flags |= IdentifyResponseFlags.ArmorProfile;
            }

            // Weapons Profile
            var propertiesWeaponsD =
                obj.PropertiesDouble.Where(
                    x =>
                        x.PropertyId < 9000
                        && (x.PropertyId == 21 || x.PropertyId == 22 || x.PropertyId == 26 || x.PropertyId == 62
                            || x.PropertyId == 63)).ToList();

            var propertiesWeaponsI =
                obj.PropertiesInt.Where(
                        x =>
                            x.PropertyId < 9000
                            && (x.PropertyId == 44 || x.PropertyId == 45 || x.PropertyId == 48 || x.PropertyId == 49))
                    .ToList();

            if (propertiesWeaponsI.Count + propertiesWeaponsD.Count > 0)
            {
                flags |= IdentifyResponseFlags.WeaponProfile;
            }

            var propertiesString = obj.PropertiesString.Where(x => x.PropertyId < 9000).ToList();

            // TODO: This needs to come out - only in while we are testing.
            if (propertiesString.Find(x => x.PropertyId == 16)?.PropertyValue == null)
            {
                string debugOutput = "baseAceObjectId: " + objectId + " (0x" + objectId.Full.ToString("X") + ")";
                debugOutput += "\n" + "weenieClassId: " + obj.WeenieClassid + " (0x" + obj.WeenieClassid.ToString("X")
                               + ")";
                debugOutput += "\n" + "defaultCombatStyle: " + obj.DefaultCombatStyle;
                AceObjectPropertiesString dbAo = new AceObjectPropertiesString();
                dbAo.AceObjectId = obj.Guid.Full;
                dbAo.PropertyId = 16;
                dbAo.PropertyValue = debugOutput;
                propertiesString.Add(dbAo);
            }
            else
            {
                // A long description already exists - lets append our data to the end.
                string debugOutput = "\n" + "baseAceObjectId: " + objectId + " (0x" + objectId.Full.ToString("X") + ")";
                debugOutput += "\n" + "weenieClassId: " + obj.WeenieClassid + " (0x" + obj.WeenieClassid.ToString("X")
                               + ")";
                debugOutput += "\n" + "defaultCombatStyle: " + obj.DefaultCombatStyle;
                propertiesString.Find(x => x.PropertyId == 16).PropertyValue += debugOutput;
            }

            if (propertiesString.Count > 0)
            {
                flags |= IdentifyResponseFlags.StringStatsTable;
            }

            if (obj.Type == ObjectType.Creature)
            {
                flags |= IdentifyResponseFlags.CreatureProfile;
            }

            Writer.Write((uint)flags); // Flags
            // TODO: We need to put in the logic for appraisal success or failure Og II
            Writer.Write(1u); // Success bool

            if ((flags & IdentifyResponseFlags.IntStatsTable) != 0)
            {
                if (propertiesInt.Count != 0)
                {
                    Writer.Write((ushort)propertiesInt.Count);
                    Writer.Write((ushort)16u);

                    foreach (var x in propertiesInt)
                    {
                        Writer.Write(x.PropertyId);
                        Writer.Write(x.PropertyValue);
                    }
                }
            }

            if ((flags & IdentifyResponseFlags.Int64StatsTable) != 0)
            {
                if (propertiesInt64.Count != 0)
                {
                    Writer.Write((ushort)propertiesInt64.Count);
                    Writer.Write((ushort)8u);

                    foreach (var x in propertiesInt64)
                    {
                        Writer.Write(x.PropertyId);
                        Writer.Write(x.PropertyValue);
                    }
                }
            }

            if ((flags & IdentifyResponseFlags.BoolStatsTable) != 0)
            {
                if (propertiesBool.Count != 0)
                {
                    Writer.Write((ushort)propertiesBool.Count);
                    Writer.Write((ushort)8);

                    foreach (var x in propertiesBool)
                    {
                        Writer.Write(x.PropertyId);
                        Writer.Write(Convert.ToUInt32(x.PropertyValue));
                    }
                }
            }

            if ((flags & IdentifyResponseFlags.FloatStatsTable) != 0)
            {
                if (propertiesDouble.Count != 0)
                {
                    Writer.Write((ushort)propertiesDouble.Count);
                    Writer.Write((ushort)8);

                    foreach (var x in propertiesDouble)
                    {
                        Writer.Write((uint)x.PropertyId);
                        Writer.Write(x.PropertyValue);
                    }
                }
            }

            if ((flags & IdentifyResponseFlags.StringStatsTable) != 0)
            {
                if (propertiesString.Count != 0)
                {
                    Writer.Write((ushort)propertiesString.Count);
                    Writer.Write((ushort)8);

                    foreach (var x in propertiesString)
                    {
                        Writer.Write((uint)x.PropertyId);
                        Writer.WriteString16L(x.PropertyValue);
                    }
                }
            }

            if ((flags & IdentifyResponseFlags.DidStatsTable) != 0)
            {
                if (propertiesDid.Count != 0)
                {
                    Writer.Write((ushort)propertiesDid.Count);
                    Writer.Write((ushort)16u);

                    foreach (var x in propertiesDid)
                    {
                        Writer.Write(x.PropertyId);
                        Writer.Write(x.PropertyValue);
                    }
                }
            }

            if ((flags & IdentifyResponseFlags.SpellBook) != 0)
            {
                if (propertiesSpellId.Count != 0)
                {
                    Writer.Write((uint)propertiesSpellId.Count);

                    foreach (var x in propertiesSpellId)
                    {
                        Writer.Write(x.SpellId);
                    }
                }
            }

            // Unlike all the others this seems to have no key - it is positional. Og II
            if ((flags & IdentifyResponseFlags.ArmorProfile) != 0)
            {
                if (propertiesArmor.Count != 0)
                {
                    foreach (var x in propertiesArmor)
                    {
                        Writer.Write((float)x.PropertyValue);
                    }
                }
            }

            // TODO: There are probably other checks that need to be made here
            if (obj.Type == ObjectType.Creature)
            {
                uint header = 8;
                // TODO: for now, we are always succeeding - will need to set this to 0 header for failure.   Og II
                Writer.Write(header);
                Writer.Write(((Creature)obj).Health.Current);
                Writer.Write(((Creature)obj).Health.MaxValue);
                if (header == 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Writer.Write(0u);
                    }
                }
                else
                {
                    // TODO: we probably need buffed values here  it may be set my the last flag I don't understand yet. - will need to revisit. Og II
                    Writer.Write(((Creature)obj).Strength.UnbuffedValue);
                    Writer.Write(((Creature)obj).Endurance.UnbuffedValue);
                    Writer.Write(((Creature)obj).Quickness.UnbuffedValue);
                    Writer.Write(((Creature)obj).Coordination.UnbuffedValue);
                    Writer.Write(((Creature)obj).Focus.UnbuffedValue);
                    Writer.Write(((Creature)obj).Self.UnbuffedValue);
                    Writer.Write(((Creature)obj).Stamina.UnbuffedValue);
                    Writer.Write(((Creature)obj).Mana.UnbuffedValue);
                    Writer.Write(((Creature)obj).Stamina.MaxValue);
                    Writer.Write(((Creature)obj).Mana.MaxValue);
                    // this only gets sent if the header can be masked with 1
                    // Writer.Write(0u);
                }
            }

            // Again, these are positional
            if ((flags & IdentifyResponseFlags.WeaponProfile) != 0)
            {
                Writer.Write(propertiesWeaponsI.Find(x => x.PropertyId == (uint)PropertyInt.DamageType)?.PropertyValue ?? 0u);
                // Signed
                Writer.Write((int?)propertiesWeaponsI.Find(x => x.PropertyId == (int)PropertyInt.WeaponTime)?.PropertyValue ?? 0);
                Writer.Write(propertiesWeaponsI.Find(x => x.PropertyId == (uint)PropertyInt.WeaponSkill)?.PropertyValue ?? 0u);
                // Signed
                Writer.Write((int?)propertiesWeaponsI.Find(x => x.PropertyId == (int)PropertyInt.Damage)?.PropertyValue ?? 0);
                Writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.DamageVariance)?.PropertyValue ?? 0.00);
                Writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.DamageMod)?.PropertyValue ?? 0.00);
                Writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.WeaponLength)?.PropertyValue ?? 0.00);
                Writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.MaximumVelocity)?.PropertyValue ?? 0.00);
                Writer.Write(propertiesWeaponsD.Find(x => x.PropertyId == (uint)PropertyDouble.WeaponOffense)?.PropertyValue ?? 0.00);
                // This one looks to be 0 - I did not find one with this calculated.   It is called Max Velocity Calculated
                Writer.Write(0u);
            }
        }
    }
}