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
        public static Lifestone CreateLifestone(ushort weenieClassId, Position newPosition, LifestoneType lifestoneType)
        {
            var weenie = WeenieHeaderFlag.Usable | WeenieHeaderFlag.RadarBlipColor | WeenieHeaderFlag.RadarBehavior | WeenieHeaderFlag.UseRadius;
            Lifestone wo = new Lifestone(ObjectType.LifeStone, new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), "Life Stone", weenieClassId, ObjectDescriptionFlag.LifeStone, weenie, newPosition);

            // model id 0x000026 is one of several lifestone IDs
            wo.MTableResourceId = 0x09000026u;
            wo.Stable = 0x20000014u;
            wo.CSetup = (uint)lifestoneType;

            wo.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Position;

            wo.PhysicsState = PhysicsState.IgnoreCollision | PhysicsState.Gravity;

            // game data min required flags;
            // wo.Type = (ushort)0x1355;
            // wo.Icon = (ushort)0x1036;

            wo.Usable = Usable.UsableRemote;
            wo.RadarColor = RadarColor.Blue;
            wo.RadarBehavior = RadarBehavior.ShowAlways;
            wo.UseRadius = 4f;

            return wo;
        }
    }
}
