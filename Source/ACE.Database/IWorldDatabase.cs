using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database
{
    public interface IWorldDatabase : ICommonDatabase
    {
        List<TeleportLocation> GetPointsOfInterest();

        List<CachedWeenieClass> GetRandomWeeniesOfType(uint typeId, uint numWeenies);

        AceObject GetAceObjectByWeenie(uint weenieClassId);

        uint GetCurrentId(uint min, uint max);

        List<AceObject> GetWeenieInstancesByLandblock(ushort landblock);

        List<Recipe> GetAllRecipes();
    }
}