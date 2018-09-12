using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Server.Factories;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public List<LandblockInstance> LinkedInstances = new List<LandblockInstance>();

        public WorldObject ParentLink;
        public List<WorldObject> ChildLinks = new List<WorldObject>();

        public void ActivateLinks(List<LandblockInstance> sourceObjects, List<Biota> biotas)
        {
            if (LinkedInstances.Count == 0) return;

            if (IsGenerator)
            {
                AddGeneratorLinks();
                return;
            }

            foreach (var link in LinkedInstances)
            {
                WorldObject wo = null;
                var biota = biotas.FirstOrDefault(b => b.Id == link.Guid);
                if (biota == null)
                    wo = WorldObjectFactory.CreateWorldObject(DatabaseManager.World.GetCachedWeenie(link.WeenieClassId), new ObjectGuid(link.Guid));
                else
                {
                    wo = WorldObjectFactory.CreateWorldObject(biota);
                    //Console.WriteLine("Loaded child biota " + wo.Name);
                }

                if (wo == null) continue;

                wo.Location = new Position(link.ObjCellId, link.OriginX, link.OriginY, link.OriginZ, link.AnglesX, link.AnglesY, link.AnglesZ, link.AnglesW);
                SetLinkProperties(wo);
                CurrentLandblock?.AddWorldObject(wo);

                wo.ParentLink = this;
                ChildLinks.Add(wo);

                // process nested links recursively
                foreach (var subLink in link.LandblockInstanceLink)
                {
                    var linkInstance = sourceObjects.FirstOrDefault(x => x.Guid == subLink.ChildGuid);

                    if (linkInstance != null)
                        wo.LinkedInstances.Add(linkInstance);
                }

                if (wo.LinkedInstances.Count > 0)
                    wo.ActivateLinks(sourceObjects, biotas);
            }
        }

        public virtual void SetLinkProperties(WorldObject wo)
        {
            // empty base
            Console.WriteLine($"{Name}.SetLinkProperties({wo.Name}) called for unknown parent type: {WeenieType}");
        }
    }
}
