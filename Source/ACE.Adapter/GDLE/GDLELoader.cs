using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

using ACE.Database.Models.World;

namespace ACE.Adapter.GDLE
{
    public static class GDLELoader
    {
        public static bool TryLoadLandblock(string file, out Models.Landblock result)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                result = JsonConvert.DeserializeObject<Models.Landblock>(fileText);

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static bool TryLoadLandblocksInParallel(string folder, out List<Models.Landblock> results)
        {
            try
            {
                var files = Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories);

                var landblocks = new ConcurrentBag<Models.Landblock>();

                Parallel.ForEach(files, file =>
                {
                    if (TryLoadLandblock(file, out var result))
                        landblocks.Add(result);
                });

                results = new List<Models.Landblock>(landblocks);

                return true;
            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static bool TryLoadWorldSpawns(string file, out Models.WorldSpawns result)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                result = JsonConvert.DeserializeObject<Models.WorldSpawns>(fileText);

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// This will sanitize the Guids for ACE use to the following format: 0x7LBID### where ### starts from [startingIdOffset]
        /// </summary>
        public static bool TryLoadLandblocksConverted(string folder, out List<LandblockInstance> results, out List<LandblockInstanceLink> links, ushort startingIdOffset = 0)
        {
            try
            {
                TryLoadLandblocksInParallel(folder, out var landblocks);

                if (landblocks == null)
                {
                    results = null;
                    links = null;
                    return false;
                }

                ReGuidAndConvertLandblocks(out results, out links, startingIdOffset, landblocks);

                return true;

            }
            catch
            {
                results = null;
                links = null;
                return false;
            }
        }

        private static void ReGuidAndConvertLandblocks(out List<LandblockInstance> results, out List<LandblockInstanceLink> links, ushort startingIdOffset, List<Models.Landblock> landblocks)
        {
            var idChanges = new Dictionary<uint /*LBID*/, Dictionary<uint /*from*/, uint /*to*/>>();

            // First we convert all weenies
            foreach (var landblock in landblocks)
            {
                var currentOffset = startingIdOffset;

                if (!idChanges.ContainsKey(landblock.key))
                    idChanges.Add(landblock.key, new Dictionary<uint, uint>());

                foreach (var weenie in landblock.value.weenies)
                {
                    var newGuid = (0x70000000 | ((weenie.pos.objcell_id & 0xFFFF0000) >> 4) | currentOffset);
                    currentOffset++;

                    if (!idChanges[landblock.key].ContainsKey(weenie.id))
                    {
                        idChanges[landblock.key].Add(weenie.id, newGuid);
                        weenie.id = newGuid;
                    }
                }
            }

            // Then we update all the links
            foreach (var landblock in landblocks)
            {
                if (landblock.value.links == null)
                    continue;

                foreach (var link in landblock.value.links)
                {
                    if (idChanges[landblock.key].TryGetValue(link.source, out var source))
                        link.source = source;
                    else
                        link.source = 0;

                    if (idChanges[landblock.key].TryGetValue(link.target, out var target))
                        link.target = target;
                    else
                        link.target = 0;
                }
            }

            TryConvertLandblocks(out results, out links, landblocks);
        }

        private static void TryConvertLandblocks(out List<LandblockInstance> results, out List<LandblockInstanceLink> links, List<Models.Landblock> landblocks)
        {
            results = new List<LandblockInstance>();
            links = new List<LandblockInstanceLink>();

            foreach (var value in landblocks)
            {
                if (GDLEConverter.TryConvert(value, out var part1, out var part2))
                {
                    foreach (var instance in part1)
                    {
                        foreach (var link in part2)
                        {
                            if (link.ParentGuid == instance.Guid)
                                instance.LandblockInstanceLink.Add(link);
                        }

                        results.Add(instance);
                    }

                    foreach (var part in part2)
                    {
                        links.Add(part);

                        var child = results.FirstOrDefault(x => x.Guid == part.ChildGuid);
                        if (child != null)
                            child.IsLinkChild = true;
                    }
                }
            }
        }

        /// <summary>
        /// This will sanitize the Guids for ACE use to the following format: 0x7LBID### where ### starts from [startingIdOffset]
        /// </summary>
        public static bool TryLoadWorldSpawnsConverted(string file, out List<LandblockInstance> results, out List<LandblockInstanceLink> links, ushort startingIdOffset = 0)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                var gdleModel = JsonConvert.DeserializeObject<Models.WorldSpawns>(fileText);


                if (gdleModel.Landblocks == null)
                {
                    results = null;
                    links = null;
                    return false;
                }

                ReGuidAndConvertLandblocks(out results, out links, startingIdOffset, gdleModel.Landblocks);

                return true;

            }
            catch
            {
                results = null;
                links = null;
                return false;
            }
        }


        public static bool TryLoadEvents(string file, out List<Models.Event> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                results = JsonConvert.DeserializeObject<List<Models.Event>>(fileText);

                return true;
            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static bool TryLoadEventsConverted(string file, out List<Event> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                var gdleModel = JsonConvert.DeserializeObject<List<Models.Event>>(fileText);

                results = new List<Event>();

                foreach (var value in gdleModel)
                {
                    if (GDLEConverter.TryConvert(value, out var result))
                        results.Add(result);
                }

                return true;

            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static bool TryLoadQuest(string file, out Models.Quest result)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                result = JsonConvert.DeserializeObject<Models.Quest>(fileText);

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static bool TryLoadQuests(string file, out List<Models.Quest> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                results = JsonConvert.DeserializeObject<List<Models.Quest>>(fileText);

                return true;
            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static bool TryLoadQuestsConverted(string file, out List<Quest> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                var gdleModel = JsonConvert.DeserializeObject<List<Models.Quest>>(fileText);

                results = new List<Quest>();

                foreach (var value in gdleModel)
                {
                    if (GDLEConverter.TryConvert(value, out var result))
                        results.Add(result);
                }

                return true;

            }
            catch
            {
                results = null;
                return false;
            }
        }


        public static bool TryLoadSpells(string file, out Models.Spells result)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                result = JsonConvert.DeserializeObject<Models.Spells>(fileText);

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static bool TryLoadSpellsConverted(string file, out List<Spell> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                var gdleModel = JsonConvert.DeserializeObject<Models.Spells>(fileText);

                results = new List<Spell>();

                foreach (var value in gdleModel.Table.SpellBaseHash)
                {
                    if (GDLEConverter.TryConvert(value.Key, value.Value, out var result))
                        results.Add(result);
                }

                return true;

            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static bool TryLoadRecipe(string file, out Models.Recipe result)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                result = JsonConvert.DeserializeObject<Models.Recipe>(fileText);

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static bool TryLoadRecipeCombined(string file, out Models.RecipeCombined result)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                result = JsonConvert.DeserializeObject<Models.RecipeCombined>(fileText);

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static bool TryLoadRecipeCombinedInParallel(string folder, out List<Models.RecipeCombined> results)
        {
            try
            {
                var files = Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories);

                var recipes = new ConcurrentBag<Models.RecipeCombined>();

                Parallel.ForEach(files, file =>
                {
                    if (TryLoadRecipeCombined(file, out var result))
                        recipes.Add(result);
                });

                results = new List<Models.RecipeCombined>(recipes);

                return true;
            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static bool TryLoadRecipeCombinedConverted(string folder, out List<Recipe> recipes, out List<CookBook> cookBooks)
        {
            try
            {
                TryLoadRecipeCombinedInParallel(folder, out var gdleModel);

                if (gdleModel == null)
                {
                    recipes = null;
                    cookBooks = null;
                    return false;
                }

                recipes = new List<Recipe>();

                cookBooks = new List<CookBook>();

                foreach (var value in gdleModel)
                {
                    if (GDLEConverter.TryConvert(value.recipe, out var result))
                    {
                        foreach (var precursor in value.precursors)
                        {
                            if (GDLEConverter.TryConvert(precursor, out var result2))
                            {
                                result2.RecipeId = value.key;
                                cookBooks.Add(result2);
                            }
                        }
                        recipes.Add(result);
                    }
                }

                return true;

            }
            catch
            {
                recipes = null;
                cookBooks = null;
                return false;
            }
        }

        public static bool TryLoadRecipesConverted(string file, out List<Recipe> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                var gdleModel = JsonConvert.DeserializeObject<List<Models.Recipe>>(fileText);

                results = new List<Recipe>();

                foreach (var value in gdleModel)
                {
                    if (GDLEConverter.TryConvert(value, out var result))
                        results.Add(result);
                }

                return true;

            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static bool TryLoadRecipePrecursors(string file, out List<Models.RecipePrecursor> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                results = JsonConvert.DeserializeObject<List<Models.RecipePrecursor>>(fileText);

                return true;
            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static bool TryLoadRecipePrecursorsConverted(string file, out List<CookBook> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                var gdleModel = JsonConvert.DeserializeObject<List<Models.RecipePrecursor>>(fileText);

                results = new List<CookBook>();

                foreach (var value in gdleModel)
                {
                    if (GDLEConverter.TryConvert(value, out var result))
                        results.Add(result);
                }

                return true;

            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static bool TryLoadRegion(string file, out Models.Region result)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                result = JsonConvert.DeserializeObject<Models.Region>(fileText);

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static bool TryLoadTerrainData(string file, out List<Models.TerrainData> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                results = JsonConvert.DeserializeObject<List<Models.TerrainData>>(fileText);

                return true;
            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static bool TryLoadWieldedTreasureTable(string file, out List<Models.WieldedTreasureTable> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                results = JsonConvert.DeserializeObject<List<Models.WieldedTreasureTable>>(fileText);

                return true;
            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static bool TryLoadWieldedTreasureTableConverted(string file, out List<TreasureWielded> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                var gdleModel = JsonConvert.DeserializeObject<List<Models.WieldedTreasureTable>>(fileText);

                results = new List<TreasureWielded>();

                foreach (var value in gdleModel)
                {
                    if (GDLEConverter.TryConvert(value, out var result))
                        results.AddRange(result);
                }

                return true;

            }
            catch
            {
                results = null;
                return false;
            }
        }
    }
}
