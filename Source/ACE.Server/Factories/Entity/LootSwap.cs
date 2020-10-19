using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Entity
{
    public static class LootSwap
    {
        public static void UpdateTables(string path)
        {
            var timer = Stopwatch.StartNew();

            var types = Reflection.GetTypes("ACE.Server.Factories.Tables");

            var files = new Dictionary<string, Dictionary<string, object>>();

            ParseFolder(path, files);

            UpdateTables(types, files);

            timer.Stop();

            // ~0.2s
            Console.WriteLine();
            Console.WriteLine($"Updated loot tables in {timer.Elapsed.TotalSeconds}s");
            Console.WriteLine();
        }

        private static HashSet<string> excludeList = new HashSet<string>()
        {
            "GemMaterialChance.cs",         // todo
            "TreasureItemTypeChances.cs"
        };

        private static void ParseFolder(string folder, Dictionary<string, Dictionary<string, object>> files)
        {
            var di = new DirectoryInfo(folder);

            if (!di.Exists)
            {
                Console.WriteLine($"{folder} not found");
                return;
            }

            var _files = di.GetFiles();

            foreach (var file in _files)
            {
                if (!file.Name.EndsWith(".cs"))
                    continue;

                if (excludeList.Contains(file.Name))
                    continue;

                var className = file.Name.Replace(".cs", "");
                var result = LootParser.ParseFile(file.FullName);
                files.Add(className, result);
            }

            var subfolders = di.GetDirectories();

            foreach (var subfolder in subfolders)
                ParseFolder(subfolder.FullName, files);
        }

        private static void UpdateTables(Dictionary<string, Type> types, Dictionary<string, Dictionary<string, object>> files)
        {
            foreach (var kvp in types)
            {
                var className = kvp.Key;
                var type = kvp.Value;

                if (!files.TryGetValue(className, out var newTables))
                {
                    //Console.WriteLine($"Couldn't find {className} in files");
                    continue;
                }

                var fields = Reflection.GetFields(type);

                Console.WriteLine($"Updated {className}");

                foreach (var field in fields)
                {
                    if (!newTables.TryGetValue(field.field.Name, out var newTable))
                    {
                        //Console.WriteLine($"Couldn't find {field.field.Name} in {className}.cs");
                        continue;
                    }
                    //Console.WriteLine($" - {field.field.Name}");
                    UpdateTable(field.field, newTable);
                }
            }
        }

        private static void UpdateTable(FieldInfo field, object newTable)
        {
            //Console.WriteLine($"Updating {field.Name}");

            field.SetValue(null, newTable);
        }

        public static class Reflection
        {
            public static Dictionary<string, Type> GetTypes(string prefix)
            {
                var assembly = Assembly.GetExecutingAssembly();

                var types = assembly.GetTypes();

                return types.Where(i => i.FullName.StartsWith(prefix)).ToDictionary(i => i.FullName.Substring(i.FullName.LastIndexOf('.') + 1), i => i);
            }

            public static List<(TreasureTableType tableType, FieldInfo field)> GetFields(Type type)
            {
                var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Static);

                var filtered = new List<(TreasureTableType tableType, FieldInfo field)>();

                foreach (var field in fields)
                {
                    var fieldName = field.FieldType.FullName;

                    if (!fieldName.Contains("ChanceTable"))
                        continue;

                    if (fieldName.Contains("System.Collections.Generic.List"))
                        continue;

                    if (fieldName.Contains("System.Int32"))
                        filtered.Add((TreasureTableType.ChanceInt, field));
                    else if (fieldName.Contains("SpellId"))
                        filtered.Add((TreasureTableType.ChanceSpell, field));
                    else if (fieldName.Contains("WeenieClassName"))
                        filtered.Add((TreasureTableType.ChanceWcid, field));
                    else if (fieldName.Contains("Boolean"))
                        filtered.Add((TreasureTableType.ChanceBool, field));
                    else if (fieldName.Contains("TreasureItemType_Orig"))
                        filtered.Add((TreasureTableType.ChanceItemType, field));
                    else if (fieldName.Contains("TreasureHeritageGroup"))
                        filtered.Add((TreasureTableType.ChanceHeritage, field));
                    else if (fieldName.Contains("TreasureArmorType"))
                        filtered.Add((TreasureTableType.ChanceArmorType, field));
                    else if (fieldName.Contains("TreasureWeaponType"))
                        filtered.Add((TreasureTableType.ChanceWeaponType, field));
                }
                return filtered;
            }
        }
    }
}
