using ACE.Entity;
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
        /// creates a portal at the position provided
        /// </summary>
        public static Portal CreatePortal(ushort weenieClassId, Position newPosition, string portalTitle, PortalType portalType)
        {
            var weenie = WeenieHeaderFlag.Usable | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.RadarBehavior | WeenieHeaderFlag.UseRadius;
            Portal wo = new Portal(ObjectType.Portal, new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), portalTitle, weenieClassId, ObjectDescriptionFlag.Portal, weenie, newPosition);

            wo.PhysicsData.MTableResourceId = 0x09000003u;
            wo.PhysicsData.CSetup = (uint)portalType;

            wo.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Position;

            wo.PhysicsData.PhysicsState = PhysicsState.Ethereal | PhysicsState.ReportCollision | PhysicsState.LightingOn | PhysicsState.Gravity;

            // game data min required flags;
            // wo.GameData.Type = (ushort)0x82D;
            // wo.GameData.Icon = (ushort)0x106B;

            wo.GameData.Usable = Usable.UsableRemote;
            wo.GameData.RadarColour = RadarColor.Portal;
            wo.GameData.RadarBehavior = RadarBehavior.ShowAlways;
            wo.GameData.UseRadius = 4f;

            return wo;
        }
    }
}
