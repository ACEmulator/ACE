using ACE.Entity;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Managers
{
    public class RecipeCache
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<uint, Dictionary<uint, Recipe>> _cache = new Dictionary<uint, Dictionary<uint, Recipe>>();

        public RecipeCache(List<Recipe> sourceData)
        {
            foreach (Recipe recipe in sourceData)
            {
                if (!_cache.ContainsKey(recipe.SourceWcid))
                    _cache.Add(recipe.SourceWcid, new Dictionary<uint, Recipe>());

                if (!_cache[recipe.SourceWcid].ContainsKey(recipe.TargetWcid))
                    _cache[recipe.SourceWcid].Add(recipe.TargetWcid, recipe);
                else
                    log.Debug("duplicate/unusable recipe detected: " + recipe.RecipeGuid);
            }
        }

        public Recipe GetRecipe(uint source, uint target)
        {
            if (_cache.ContainsKey(source) && _cache[source].ContainsKey(target))
                return _cache[source][target];
            else
                return null;
        }
    }
}
