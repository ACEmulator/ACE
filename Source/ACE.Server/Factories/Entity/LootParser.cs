using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using ACE.Entity.Enum;
using ACE.Server.Factories.Enum;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories.Entity
{
    public class LootParser
    {
        public static Dictionary<string, object> ParseFile(string filename)
        {
            var lines = File.ReadAllLines(filename);

            var tables = new Dictionary<string, object>();

            //Console.WriteLine($"Parsed {filename}");

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (line.Contains("List<"))
                    continue;

                if (!line.Contains("ChanceTable<") || !line.Contains(" = new") || !line.Contains("()"))
                    continue;

                if (line.Contains("readonly") || line.EndsWith(";"))
                    continue;

                var table = ParseChanceTable(lines, i);

                tables.Add(table.tableName, table.table);
            }
            return tables;
        }

        private static (string tableName, object table) ParseChanceTable(string[] lines, int startLine)
        {
            var line = lines[startLine];

            var tableType = GetTableType(line);
            var tableName = GetTableName(line);

            //Console.WriteLine($" - {tableName}");

            object chanceTable = null;

            switch (tableType)
            {
                case TreasureTableType.ChanceInt:
                    chanceTable = ParseChanceTable_Int(lines, startLine);
                    break;

                case TreasureTableType.ChanceSpell:
                    chanceTable = ParseChanceTable_Spell(lines, startLine);
                    break;

                case TreasureTableType.ChanceWcid:
                    chanceTable = ParseChanceTable_Wcid(lines, startLine);
                    break;

                case TreasureTableType.ChanceBool:
                    chanceTable = ParseChanceTable_Bool(lines, startLine);
                    break;

                case TreasureTableType.ChanceHeritage:
                    chanceTable = ParseChanceTable_Heritage(lines, startLine);
                    break;

                case TreasureTableType.ChanceItemType:
                    chanceTable = ParseChanceTable_ItemType(lines, startLine);
                    break;

                case TreasureTableType.ChanceArmorType:
                    chanceTable = ParseChanceTable_ArmorType(lines, startLine);
                    break;

                case TreasureTableType.ChanceWeaponType:
                    chanceTable = ParseChanceTable_WeaponType(lines, startLine);
                    break;

                default:
                    Console.WriteLine($"Unknown table type {tableType}");
                    break;
            }

            return (tableName, chanceTable);
        }

        public static ChanceTable<int> ParseChanceTable_Int(string[] lines, int startLine)
        {
            var chanceTable = new ChanceTable<int>();

            for (var i = startLine + 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();

                if (line.Length < 2)
                    continue;

                if (line.Length == 2)
                    break;

                if (line.StartsWith("//"))
                    continue;

                var match = Regex.Match(line, @"(\d+),\s+([\d.]+)");

                if (!match.Success || !int.TryParse(match.Groups[1].Value, out var val) || !float.TryParse(match.Groups[2].Value, out var chance))
                {
                    Console.WriteLine($"Couldn't parse {line}");
                    continue;
                }
                chanceTable.Add((val, chance));
            }

            return chanceTable;
        }

        public static ChanceTable<SpellId> ParseChanceTable_Spell(string[] lines, int startLine)
        {
            var chanceTable = new ChanceTable<SpellId>();

            for (var i = startLine + 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();

                if (line.Length < 2)
                    continue;

                if (line.Length == 2)
                    break;

                if (line.StartsWith("//"))
                    continue;

                var match = Regex.Match(line, @"SpellId.([^,]+),\s+([\d.]+)");

                if (!match.Success || !System.Enum.TryParse(match.Groups[1].Value, out SpellId spell) || !float.TryParse(match.Groups[2].Value, out var chance))
                {
                    Console.WriteLine($"Couldn't parse {line}");
                    continue;
                }
                chanceTable.Add((spell, chance));
            }

            return chanceTable;
        }

        public static ChanceTable<WeenieClassName> ParseChanceTable_Wcid(string[] lines, int startLine)
        {
            var chanceTable = new ChanceTable<WeenieClassName>();

            for (var i = startLine + 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();

                if (line.Length < 2)
                    continue;

                if (line.Length == 2)
                    break;

                if (line.StartsWith("//"))
                    continue;

                var match = Regex.Match(line, @"WeenieClassName.([^,]+),\s+([\d.]+)");

                if (!match.Success || !System.Enum.TryParse(match.Groups[1].Value, out WeenieClassName wcid) || !float.TryParse(match.Groups[2].Value, out var chance))
                {
                    Console.WriteLine($"Couldn't parse {line}");
                    continue;
                }
                chanceTable.Add((wcid, chance));
            }

            return chanceTable;
        }

        public static ChanceTable<bool> ParseChanceTable_Bool(string[] lines, int startLine)
        {
            var chanceTable = new ChanceTable<bool>();

            for (var i = startLine + 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();

                if (line.Length < 2)
                    continue;

                if (line.Length == 2)
                    break;

                if (line.StartsWith("//"))
                    continue;

                var match = Regex.Match(line, @"\(\s+([^,]+),\s+([\d.]+)");

                if (!match.Success || !bool.TryParse(match.Groups[1].Value, out var val) || !float.TryParse(match.Groups[2].Value, out var chance))
                {
                    Console.WriteLine($"Couldn't parse {line}");
                    continue;
                }
                chanceTable.Add((val, chance));
            }

            return chanceTable;
        }

        public static ChanceTable<TreasureHeritageGroup> ParseChanceTable_Heritage(string[] lines, int startLine)
        {
            var chanceTable = new ChanceTable<TreasureHeritageGroup>();

            for (var i = startLine + 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();

                if (line.Length < 2)
                    continue;

                if (line.Length == 2)
                    break;

                if (line.StartsWith("//"))
                    continue;

                var match = Regex.Match(line, @"TreasureHeritageGroup.([^,]+),\s+([\d.]+)");

                if (!match.Success || !System.Enum.TryParse(match.Groups[1].Value, out TreasureHeritageGroup heritage) || !float.TryParse(match.Groups[2].Value, out var chance))
                {
                    Console.WriteLine($"Couldn't parse {line}");
                    continue;
                }
                chanceTable.Add((heritage, chance));
            }

            return chanceTable;
        }

        public static ChanceTable<TreasureItemType_Orig> ParseChanceTable_ItemType(string[] lines, int startLine)
        {
            var chanceTable = new ChanceTable<TreasureItemType_Orig>();

            for (var i = startLine + 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();

                if (line.Length < 2)
                    continue;

                if (line.Length == 2)
                    break;

                if (line.StartsWith("//"))
                    continue;

                var match = Regex.Match(line, @"TreasureItemType_Orig.([^,]+),\s+([\d.]+)");

                if (!match.Success || !System.Enum.TryParse(match.Groups[1].Value, out TreasureItemType_Orig itemType) || !float.TryParse(match.Groups[2].Value, out var chance))
                {
                    Console.WriteLine($"Couldn't parse {line}");
                    continue;
                }
                chanceTable.Add((itemType, chance));
            }

            return chanceTable;
        }

        public static ChanceTable<TreasureArmorType> ParseChanceTable_ArmorType(string[] lines, int startLine)
        {
            var chanceTable = new ChanceTable<TreasureArmorType>();

            for (var i = startLine + 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();

                if (line.Length < 2)
                    continue;

                if (line.Length == 2)
                    break;

                if (line.StartsWith("//"))
                    continue;

                var match = Regex.Match(line, @"TreasureArmorType.([^,]+),\s+([\d.]+)");

                if (!match.Success || !System.Enum.TryParse(match.Groups[1].Value, out TreasureArmorType armorType) || !float.TryParse(match.Groups[2].Value, out var chance))
                {
                    Console.WriteLine($"Couldn't parse {line}");
                    continue;
                }
                chanceTable.Add((armorType, chance));
            }

            return chanceTable;
        }

        public static ChanceTable<TreasureWeaponType> ParseChanceTable_WeaponType(string[] lines, int startLine)
        {
            var chanceTable = new ChanceTable<TreasureWeaponType>();

            for (var i = startLine + 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();

                if (line.Length < 2)
                    continue;

                if (line.Length == 2)
                    break;

                if (line.StartsWith("//"))
                    continue;

                var match = Regex.Match(line, @"TreasureWeaponType.([^,]+),\s+([\d.]+)");

                if (!match.Success || !System.Enum.TryParse(match.Groups[1].Value, out TreasureWeaponType weaponType) || !float.TryParse(match.Groups[2].Value, out var chance))
                {
                    Console.WriteLine($"Couldn't parse {line}");
                    continue;
                }
                chanceTable.Add((weaponType, chance));
            }

            return chanceTable;
        }

        public static TreasureTableType GetTableType(string line)
        {
            var match = Regex.Match(line, @"<([^>]+)");

            if (!match.Success)
                return TreasureTableType.Undef;

            switch (match.Groups[1].Value)
            {
                case "int":
                    return TreasureTableType.ChanceInt;
                case "SpellId":
                    return TreasureTableType.ChanceSpell;
                case "WeenieClassName":
                    return TreasureTableType.ChanceWcid;
                case "bool":
                    return TreasureTableType.ChanceBool;
                case "TreasureHeritageGroup":
                    return TreasureTableType.ChanceHeritage;
                case "TreasureItemType_Orig":
                    return TreasureTableType.ChanceItemType;
                case "TreasureArmorType":
                    return TreasureTableType.ChanceArmorType;
                case "TreasureWeaponType":
                    return TreasureTableType.ChanceWeaponType;
            }
            return TreasureTableType.Undef;
        }

        public static string GetTableName(string line)
        {
            var match = Regex.Match(line, @"> ([^ ]+)");

            if (!match.Success)
                return null;

            return match.Groups[1].Value;
        }
    }
}
