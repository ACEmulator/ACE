using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ACE.Entity;

namespace ACE.Database
{
    public interface IWorldCachedDatabase
    {
        Task<List<TeleportLocation>> GetPointsOfInterest();

        Task<List<CachedWeenieClass>> GetRandomWeeniesOfType(uint typeId, uint numWeenies);

        Task<AceObject> GetAceObjectByWeenie(uint weenieClassId);

        Task<uint> GetCurrentId(uint min, uint max);

        Task<List<AceObject>> GetWeenieInstancesByLandblock(ushort landblock);

        Task<List<Recipe>> GetAllRecipes();

        Task CreateRecipe(Recipe recipe);

        Task UpdateRecipe(Recipe recipe);

        Task DeleteRecipe(Guid recipeGuid);

        Task<List<Content>> GetAllContent();
        
        Task CreateContent(Content content);

        Task UpdateContent(Content content);

        Task DeleteContent(Guid contentGuid);

        Task<List<AceObject>> GetObjectsByLandblock(ushort landblock);
        
        /// <summary>
        /// gets any matching weenie objects, only very shallowly populated.  this is not
        /// a full object.  to get the full object, call GetWeenie on the resulting weenie id.
        /// </summary>
        Task<List<WeenieSearchResult>> SearchWeenies(SearchWeeniesCriteria criteria);

        bool UserModifiedFlagPresent();

        /// <summary>
        /// does a full object replacement, deleting all properties prior to insertion
        /// </summary>
        Task<bool> ReplaceObject(AceObject aceObject);
    }
}
