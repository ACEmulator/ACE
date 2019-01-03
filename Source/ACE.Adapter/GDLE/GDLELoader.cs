using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

using ACE.Adapter.GDLE.Models;
using ACE.Database.Models.World;

namespace ACE.Adapter.GDLE
{
    public static class GDLELoader
    {
        // 1 worldspawns
        // 2 events quests spells
        // 3 recipes

        public static bool TryLoadWorldSpawns(string file, out WorldSpawns result)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                result = JsonConvert.DeserializeObject<WorldSpawns>(fileText);

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static bool TryLoadWorldSpawnsConverted(string file, out List<LandblockInstance> results, out List<LandblockInstanceLink> links)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                var gdleModel = JsonConvert.DeserializeObject<WorldSpawns>(fileText);

                results = new List<LandblockInstance>();
                links = new List<LandblockInstanceLink>();

                foreach (var value in gdleModel.landblocks)
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
    }
}
