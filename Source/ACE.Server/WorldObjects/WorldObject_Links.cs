using System.Collections.Generic;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Server.Factories;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public List<LandblockInstance> LinkedInstances = new List<LandblockInstance>();

        public virtual void ActivateLinks()
        {
            if (LinkedInstances.Count == 0) return;

            if (IsGenerator)
            {
                AddGeneratorLinks();
                return;
            }

            foreach (var link in LinkedInstances)
            {
                var wo = WorldObjectFactory.CreateWorldObject(DatabaseManager.World.GetCachedWeenie(link.WeenieClassId), new ObjectGuid(link.Guid));
                if (wo == null) continue;

                wo.Location = new Position(link.ObjCellId, link.OriginX, link.OriginY, link.OriginZ, link.AnglesX, link.AnglesY, link.AnglesZ, link.AnglesW);
                wo.ActivationTarget = Guid.Full;
                CurrentLandblock?.AddWorldObject(wo);
            }

        }
    }
}
