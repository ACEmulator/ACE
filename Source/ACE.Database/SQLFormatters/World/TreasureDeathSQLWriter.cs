using System;
using System.IO;

using ACE.Database.Models.World;

namespace ACE.Database.SQLFormatters.World
{
    public class TreasureDeathSQLWriter : SQLWriter
    {
        /// <summary>
        /// Default is formed from: input.TreasureType.ToString("00000")
        /// </summary>
        public string GetDefaultFileName(TreasureDeath input)
        {
            string fileName = input.TreasureType.ToString("00000");
            fileName = IllegalInFileName.Replace(fileName, "_");
            fileName += ".sql";

            return fileName;
        }

        public void CreateSQLDELETEStatement(TreasureDeath input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `treasure_death` WHERE `treasure_Type` = {input.TreasureType};");
        }

        public void CreateSQLINSERTStatement(TreasureDeath input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `treasure_death` (`treasure_Type`, `tier`, `loot_Quality_Mod`, `unknown_Chances`, `item_Chance`, `item_Min_Amount`, `item_Max_Amount`, `item_Treasure_Type_Selection_Chances`, `magic_Item_Chance`, `magic_Item_Min_Amount`, `magic_Item_Max_Amount`, `magic_Item_Treasure_Type_Selection_Chances`, `mundane_Item_Chance`, `mundane_Item_Min_Amount`, `mundane_Item_Max_Amount`, `mundane_Item_Type_Selection_Chances`, `last_Modified`)");

            var output = "VALUES (" +
                $"{input.TreasureType}, " +
                $"{input.Tier}, " +
                $"{input.LootQualityMod}, " +
                $"{input.UnknownChances}, " +
                $"{input.ItemChance}, " +
                $"{input.ItemMinAmount}, " +
                $"{input.ItemMaxAmount}, " +
                $"{input.ItemTreasureTypeSelectionChances}, " +
                $"{input.MagicItemChance}, " +
                $"{input.MagicItemMinAmount}, " +
                $"{input.MagicItemMaxAmount}, " +
                $"{input.MagicItemTreasureTypeSelectionChances}, " +
                $"{input.MundaneItemChance}, " +
                $"{input.MundaneItemMinAmount}, " +
                $"{input.MundaneItemMaxAmount}, " +
                $"{input.MundaneItemTypeSelectionChances}, " +
                $"'{input.LastModified:yyyy-MM-dd HH:mm:ss}'" +
                ");";

            output = FixNullFields(output);

            writer.WriteLine(output);
        }
    }
}
