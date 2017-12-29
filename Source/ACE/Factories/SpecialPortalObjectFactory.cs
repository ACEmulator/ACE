using System;
using System.Threading.Tasks;

using ACE.Managers;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Actions;

namespace ACE.Factories
{
    /// <summary>
    /// factory class for creating special portal objects
    /// </summary>
    public class SpecialPortalObjectFactory
    {
        public enum PortalWcid : ushort
        {
            HummingCrystal = 9071,

            Orphanage = 27298,

            GolemSanctum = 7934,

            FloatingCity = 8190
        }
        /// <summary>
        /// creates a portal of the specified weenie at the position provided
        /// </summary>
        public static async Task SpawnPortal(PortalWcid weenieClassId, Position newPosition, float despawnTime)
        {
            AceObject aceO = await DatabaseManager.World.GetAceObjectByWeenie((ushort)weenieClassId);

            aceO.AceObjectPropertiesPositions.Add(PositionType.Location, newPosition);

            Portal portal = await WorldObject.CreateWorldObject<Portal>(aceO);

            portal.Guid = GuidManager.NewItemGuid();

            await LandblockManager.AddObject(portal);

            // Create portal decay
            await Task.Delay(TimeSpan.FromSeconds(despawnTime));
            portal.CurrentLandblock.RemoveWorldObject(portal.Guid, false);
        }
    }
}
