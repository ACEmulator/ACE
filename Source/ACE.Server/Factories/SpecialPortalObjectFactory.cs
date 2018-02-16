using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    /// <summary>
    /// factory class for creating special portal objects
    /// </summary>
    public class SpecialPortalObjectFactory
    {
        public enum PortalWcid : ushort
        {
            HummingCrystal  = 9071,
            Orphanage       = 27298,
            GolemSanctum    = 7934,
            FloatingCity    = 8190
        }

        /// <summary>
        /// creates a portal of the specified weenie at the position provided
        /// </summary>
        public static void SpawnPortal(PortalWcid weenieClassId, Position newPosition, float despawnTime)
        {
            WorldObject portal = WorldObjectFactory.CreateNewWorldObject((uint)weenieClassId);

            portal.Positions.Add(PositionType.Location, newPosition);

            LandblockManager.AddObject(portal);

            // Create portal decay
            ActionChain despawnChain = new ActionChain();
            despawnChain.AddDelaySeconds(despawnTime);
            despawnChain.AddAction(portal, () => portal.CurrentLandblock.RemoveWorldObject(portal.Guid, false));
            despawnChain.EnqueueChain();
        }
    }
}
