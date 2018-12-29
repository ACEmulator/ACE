using System;
using System.Linq;
using System.Numerics;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    public sealed class HousePortal : Portal
    {
        public House House => ParentLink as House;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public HousePortal(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public HousePortal(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// House Portals are on Use activated, rather than collision based activation
        /// The actual portal process is wrapped to the base portal class ActOnUse, after ACL check are performed
        /// </summary>
        /// <param name="worldObject"></param>
        public override void ActOnUse(WorldObject worldObject)
        {
            if (worldObject is Player)
            {
                var player = worldObject as Player;

                // for house portal usage, verify house owner or guest
                if (House.HasPermission(player))
                {
                    // if house portal in dungeon,
                    // set destination to outdoor house slumlord
                    if (CurrentLandblock != null && CurrentLandblock.IsDungeon)
                    {
                        var biota = DatabaseManager.Shard.GetBiotasByWcid(House.WeenieClassId).FirstOrDefault(b => b.BiotaPropertiesPosition.FirstOrDefault(p => p.PositionType == (ushort)PositionType.Location).ObjCellId >> 16 != House.Location.Landblock);
                        if (biota != null)
                        {
                            var outdoorHouseGuid = biota.Id;
                            var house = House.Load(outdoorHouseGuid);
                            SetPosition(PositionType.Destination, new Position(house.SlumLord.Location));
                        }
                    }

                    base.ActOnUse(player);
                }
                else
                {
                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustBeHouseGuestToUsePortal));
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
                }
            }
        }

        public override void SetLinkProperties(WorldObject wo)
        {
            // get properties from parent?
            wo.HouseId = House.HouseId;
            wo.HouseOwner = House.HouseOwner;
            wo.HouseInstance = House.HouseInstance;

            if (wo.IsLinkSpot)
            {
                var housePortals = House.GetHousePortals();
                if (housePortals.Count == 0)
                {
                    Console.WriteLine($"{Name}.SetLinkProperties({wo.Name}): found LinkSpot, but empty HousePortals");
                    return;
                }
                var i = housePortals[0];
                var destination = new Position(i.ObjCellId, new Vector3(i.OriginX, i.OriginY, i.OriginZ), new Quaternion(i.AnglesX, i.AnglesY, i.AnglesZ, i.AnglesW));

                wo.SetPosition(PositionType.Destination, destination);

                // set portal destination directly?
                SetPosition(PositionType.Destination, destination);
            }

            //if (HouseOwner != null)
            //Console.WriteLine($"{Name}.SetLinkProperties({wo.Name}) - houseID: {HouseId:X8}, owner: {HouseOwner:X8}, instance: {HouseInstance:X8}")        }
        }
    }
}
