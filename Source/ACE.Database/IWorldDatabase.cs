using ACE.Entity;
using System;
using System.Collections.Generic;

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

        void CreateRecipe(Recipe recipe);

        void UpdateRecipe(Recipe recipe);

        void DeleteRecipe(Guid recipeGuid);

        List<Content> GetAllContent();
        
        void CreateContent(Content content);

        void UpdateContent(Content content);

        void DeleteContent(Guid contentGuid);
        
        /// <summary>
        /// gets any matching weenie objects, only very shallowly populated.  this is not
        /// a full object.  to get the full object, call GetWeenie on the resulting weenie id.
        /// </summary>
        List<WeenieSearchResult> SearchWeenies(SearchWeeniesCriteria criteria);

        bool UserModifiedFlagPresent();

        /// <summary>
        /// does a full object replacement, deleting all properties prior to insertion
        /// </summary>
        bool ReplaceObject(AceObject aceObject);
    }
}
