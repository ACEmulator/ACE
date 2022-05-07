using System;
using System.Numerics;

using log4net;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    public sealed class HousePortal : Portal
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

                if (i.ObjCellId == Location.Cell)
                {
                    if (housePortals.Count > 1)
                        i = housePortals[1];
                    else
                    { // there are some houses that for some reason, don't have return locations, so we'll fake the entry with a reference to the root house portal location mimicking other database entries.
                        i = new Database.Models.World.HousePortal { ObjCellId = House.RootHouse.HousePortal.Location.Cell,
                                                                      OriginX = House.RootHouse.HousePortal.Location.PositionX,
                                                                      OriginY = House.RootHouse.HousePortal.Location.PositionY,
                                                                      OriginZ = House.RootHouse.HousePortal.Location.PositionZ,
                                                                      AnglesX = House.RootHouse.HousePortal.Location.RotationX,
                                                                      AnglesY = House.RootHouse.HousePortal.Location.RotationY,
                                                                      AnglesZ = House.RootHouse.HousePortal.Location.RotationZ,
                                                                      AnglesW = House.RootHouse.HousePortal.Location.RotationW };
                    }
                }

                var destination = new Position(i.ObjCellId, new Vector3(i.OriginX, i.OriginY, i.OriginZ), new Quaternion(i.AnglesX, i.AnglesY, i.AnglesZ, i.AnglesW));

                wo.SetPosition(PositionType.Destination, destination);

                // set portal destination directly?
                SetPosition(PositionType.Destination, destination);
            }
        }

        public override ActivationResult CheckUseRequirements(WorldObject activator)
        {
            var rootHouse = House?.RootHouse;

            if (activator == null || rootHouse == null)
            {
                log.Warn($"HousePortal.CheckUseRequirements: 0x{Guid} - {Location.ToLOCString()}");
                log.Warn($"HousePortal.CheckUseRequirements: activator is null - {activator == null} | House is null - {House == null} | RootHouse is null - {rootHouse == null}");
                return new ActivationResult(false);
            }

            if (!(activator is Player player))
                return new ActivationResult(false);

            if (player.IsOlthoiPlayer)
                return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.OlthoiMayNotUsePortal));

            if (player.CurrentLandblock.IsDungeon && Destination.LandblockId != player.CurrentLandblock.Id)
                return new ActivationResult(true);   // allow escape to overworld always

            if (player.IgnorePortalRestrictions)
                return new ActivationResult(true);

            var houseOwner = rootHouse.HouseOwner;

            if (houseOwner == null)
                //return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.YouMustBeHouseGuestToUsePortal));
                return new ActivationResult(true);

            if (rootHouse.OpenToEveryone)
                return new ActivationResult(true);

            if (!rootHouse.HasPermission(player))
                return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.YouMustBeHouseGuestToUsePortal));

            return new ActivationResult(true);
        }

        /// <summary>
        /// House Portals are on Use activated, rather than collision based activation
        /// The actual portal process is wrapped to the base portal class ActOnUse, after ACL check are performed
        /// </summary>
        /// <param name="worldObject"></param>
        public override void ActOnUse(WorldObject worldObject)
        {
            // if house portal in dungeon,
            // set destination to outdoor house slumlord
            if (CurrentLandblock != null && CurrentLandblock.IsDungeon && Destination.LandblockId == CurrentLandblock.Id)
                SetPosition(PositionType.Destination, new Position(House.RootHouse.SlumLord.Location));

            base.ActOnUse(worldObject);
        }
    }
}
