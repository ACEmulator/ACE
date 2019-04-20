using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using ACE.Database.Models.World;

namespace ACE.Database.SQLFormatters.World
{
    public class TreasureWieldedSQLWriter : SQLWriter
    {
        /// <summary>
        /// Default is formed from: input.TreasureType.ToString("00000")
        /// </summary>
        public string GetDefaultFileName(TreasureWielded input)
        {
            string fileName = input.TreasureType.ToString("00000");
            fileName = IllegalInFileName.Replace(fileName, "_");
            fileName += ".sql";

            return fileName;
        }

        public void CreateSQLDELETEStatement(IList<TreasureWielded> input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `treasure_wielded` WHERE `treasure_Type` = {input[0].TreasureType};");
        }

        public void CreateSQLINSERTStatement(IList<TreasureWielded> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `treasure_wielded` (`treasure_Type`, `weenie_Class_Id`, `palette_Id`, `unknown_1`, `shade`, `stack_Size`, `stack_Size_Variance`, `probability`, `unknown_3`, `unknown_4`, `unknown_5`, `set_Start`, `has_Sub_Set`, `continues_Previous_Set`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`, `last_Modified`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string weenieLabel = null;
                if (WeenieNames != null)
                    WeenieNames.TryGetValue(input[i].WeenieClassId, out weenieLabel);

                return
                    $"{input[i].TreasureType}, " +
                    $"{input[i].WeenieClassId.ToString().PadLeft(5)}, " +
                    $"{input[i].PaletteId.ToString().PadLeft(2)}, " +
                    $"{input[i].Unknown1}, " +
                    $"{input[i].Shade.ToString(CultureInfo.InvariantCulture).PadLeft(4)}, " +
                    $"{input[i].StackSize}, " +
                    $"{input[i].StackSizeVariance}, " +
                    $"{input[i].Probability.ToString(CultureInfo.InvariantCulture).PadLeft(7)}, " +
                    $"{input[i].Unknown3}, " +
                    $"{input[i].Unknown4}, " +
                    $"{input[i].Unknown5}, " +
                    $"{input[i].SetStart.ToString().PadLeft(5)}, " +
                    $"{input[i].HasSubSet.ToString().PadLeft(5)}, " +
                    $"{input[i].ContinuesPreviousSet.ToString().PadLeft(5)}, " +
                    $"{input[i].Unknown9}, " +
                    $"{input[i].Unknown10}, " +
                    $"{input[i].Unknown11}, " +
                    $"{input[i].Unknown12}, " +
                    $"'{input[i].LastModified:yyyy-MM-dd HH:mm:ss}'" +
                    $") /* {weenieLabel} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }
    }
}
