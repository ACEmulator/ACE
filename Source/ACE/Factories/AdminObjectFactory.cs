using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Factories
{
    /// <summary>
    /// factory class for creating objects related to administration or content-recreation
    /// </summary>
    public class AdminObjectFactory
    {
        /// <summary>
        /// creates a lifestone directly in fron of the player's position provided
        /// </summary>
        public static WorldObject CreateLifestone(Position newPosition)
        {
            var weenie = WeenieHeaderFlag.Usable | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar | WeenieHeaderFlag.UseRadius;
            ImmutableWorldObject wo = new ImmutableWorldObject(ObjectType.LifeStone, new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), "Life Stone", 0, ObjectDescriptionFlag.LifeStone, weenie, newPosition);

            // model id 0x000026 is one of several lifestone IDs
            wo.PhysicsData.MTableResourceId = 0x09000026u;
            wo.PhysicsData.Stable = 0x20000014u;
            wo.PhysicsData.CSetup = (uint)0x020002EEu;

            wo.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Position;

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
