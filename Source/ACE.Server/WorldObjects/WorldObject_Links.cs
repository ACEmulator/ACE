
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using System;
using System.Collections.Generic;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public List<LandblockInstances> LinkedInstances = new List<LandblockInstances>();

        public virtual void ActivateLinks()
        {
            if (LinkedInstances.Count > 0 && GeneratorProfiles.Count > 0)
            {
                var profileTemplate = GeneratorProfiles[0];

                foreach (var link in LinkedInstances)
                {
                    var profile = new BiotaPropertiesGenerator();
                    profile.WeenieClassId = link.WeenieClassId;
                    profile.ObjCellId = link.ObjCellId;
                    profile.OriginX = link.OriginX;
                    profile.OriginY = link.OriginY;
                    profile.OriginZ = link.OriginZ;
                    profile.AnglesW = link.AnglesW;
                    profile.AnglesX = link.AnglesX;
                    profile.AnglesY = link.AnglesY;
                    profile.AnglesZ = link.AnglesZ;
                    //profile.Probability = Math.Abs(profileTemplate.Probability);
                    profile.Probability = profileTemplate.Probability;
                    profile.InitCreate = profileTemplate.InitCreate;
                    profile.MaxCreate = profileTemplate.MaxCreate;
                    profile.WhenCreate = profileTemplate.WhenCreate;
                    profile.WhereCreate = profileTemplate.WhereCreate;

                    GeneratorProfiles.Add(profile);
                }

                //SelectGeneratorProfiles();
                //UpdateGeneratorInts();
                //QueueGenerator();
            }
        }
    }
}
