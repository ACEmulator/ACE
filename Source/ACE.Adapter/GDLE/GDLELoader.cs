using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

using ACE.Database.Models.World;

namespace ACE.Adapter.GDLE
{
    public static class GDLELoader
    {
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
        public static bool TryLoadWorldSpawnsConverted(string file, out List<LandblockInstance> results, out List<LandblockInstanceLink> links, ushort startingIdOffset = 0)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                var gdleModel = JsonConvert.DeserializeObject<Models.WorldSpawns>(fileText);

                var idChanges = new Dictionary<uint /*from*/, uint /*to*/>();

                // First we convert all weenies
                foreach (var landblock in gdleModel.Landblocks)
                {
                    var currentOffset = startingIdOffset;

                    foreach (var weenie in landblock.Value.Weenies)
                    {
                        var newGuid = (0x70000000 | ((weenie.Position.ObjCellId & 0xFFFF0000) >> 4) | currentOffset);
                        currentOffset++;

                        idChanges[weenie.Id] = newGuid;
                        weenie.Id = newGuid;
                    }
                }

                // Then we update all the links
                foreach (var landblock in gdleModel.Landblocks)
                {
                    if (landblock.Value.Links == null)
                        continue;

                    foreach (var link in landblock.Value.Links)
                    {
                        if (idChanges.TryGetValue(link.Source, out var source))
                            link.Source = source;
                        else
                            link.Source = 0;

                        if (idChanges.TryGetValue(link.Source, out var target))
                            link.Target = target;
                        else
                            link.Target = 0;
                    }
                }

                results = new List<LandblockInstance>();
                links = new List<LandblockInstanceLink>();

                foreach (var value in gdleModel.Landblocks)
                {
                    if (GDLEConverter.TryConvert(value, out var part1, out var part2))
                    {
                        foreach (var part in part1)
                            results.Add(part);

                        foreach (var part in part2)
                            links.Add(part);
                    }
                }

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

        public static bool TryLoadEventsConverted(string file, out List<Database.Models.World.Event> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                var gdleModel = JsonConvert.DeserializeObject<List<Models.Event>>(fileText);

                results = new List<Database.Models.World.Event>();

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

        public static bool TryLoadQuestsConverted(string file, out List<Database.Models.World.Quest> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                var gdleModel = JsonConvert.DeserializeObject<List<Models.Quest>>(fileText);

                results = new List<Database.Models.World.Quest>();

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

        public static bool TryLoadSpellsConverted(string file, out List<Database.Models.World.Spell> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                var gdleModel = JsonConvert.DeserializeObject<Models.Spells>(fileText);

                results = new List<Database.Models.World.Spell>();

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


        public static bool TryLoadRecipes(string file, out List<Models.Recipe> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                results = JsonConvert.DeserializeObject<List<Models.Recipe>>(fileText);

                return true;
            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static bool TryLoadRecipesConverted(string file, out List<Database.Models.World.Recipe> results)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                var gdleModel = JsonConvert.DeserializeObject<List<Models.Recipe>>(fileText);

                results = new List<Database.Models.World.Recipe>();

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
    }
}
