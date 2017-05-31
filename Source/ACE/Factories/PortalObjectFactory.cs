﻿using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Factories
{
    /// <summary>
    /// factory class for creating objects related to administration or content-recreation
    /// </summary>
    public class PortalObjectFactory
    {
        /// <summary>
        /// creates a portal at the position provided.  TODO: load the weenie from the database and use a different
        /// constructor on the Portal object.
        /// </summary>
        public static Portal CreatePortal(ushort weenieClassId, Position newPosition, string portalTitle, PortalType portalType)
        {
            var weenie = WeenieHeaderFlag.Usable | WeenieHeaderFlag.RadarBlipColor | WeenieHeaderFlag.RadarBehavior | WeenieHeaderFlag.UseRadius;
            Portal wo = new Portal(ObjectType.Portal, new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), portalTitle, weenieClassId, ObjectDescriptionFlag.Portal, weenie, newPosition);

            wo.PhysicsData.MTableResourceId = 0x09000003u;
            wo.PhysicsData.CSetup = (uint)portalType;

            wo.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Position;

            wo.PhysicsData.PhysicsState = PhysicsState.Ethereal | PhysicsState.ReportCollision | PhysicsState.LightingOn | PhysicsState.Gravity;

            // game data min required flags;
            // wo.Type = (ushort)0x82D;
            // wo.Icon = (ushort)0x106B;

            wo.Usable = Usable.UsableRemote;
            wo.RadarColor = RadarColor.Portal;
            wo.RadarBehavior = RadarBehavior.ShowAlways;
            wo.UseRadius = 4f;

            return wo;
        }
    }
}
