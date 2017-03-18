using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Factories
{
    /// <summary>
    /// factory class for creating objects related to administration or content-recreation
    /// </summary>
    public class LifestoneObjectFactory
    {
        /// <summary>
        /// creates a lifestone directly in fron of the player's position provided
        /// </summary>
        public static ImmutableWorldObject CreateLifestone(ushort weenieClassId, Position newPosition, LifestoneType lifestoneType)
        {
            var weenie = WeenieHeaderFlag.Usable | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar | WeenieHeaderFlag.UseRadius;
            ImmutableWorldObject wo = new ImmutableWorldObject(ObjectType.LifeStone, new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), "Life Stone", weenieClassId, ObjectDescriptionFlag.LifeStone, weenie, newPosition);

            // model id 0x000026 is one of several lifestone IDs
            wo.PhysicsData.MTableResourceId = 0x09000026u;
            wo.PhysicsData.Stable = 0x20000014u;
            wo.PhysicsData.CSetup = (uint)lifestoneType;

            wo.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Position;

            //wo.PhysicsData.PhysicsState = PhysicsState.IgnoreCollision | PhysicsState.Gravity;

            //game data min required flags;
            wo.GameData.Type = (ushort)0x1355;
            wo.GameData.Icon = (ushort)0x1036;

            wo.GameData.Usable = Usable.UsableRemote;
            wo.GameData.RadarColour = RadarColor.Blue;
            wo.GameData.RadarBehavior = RadarBehavior.ShowAlways;
            wo.GameData.UseRadius = 4f;

            return wo;
        }
    }
}
