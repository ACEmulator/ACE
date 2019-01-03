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
    }
}
