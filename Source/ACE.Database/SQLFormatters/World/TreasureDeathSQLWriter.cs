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
            throw new NotImplementedException();
        }

        public void CreateSQLINSERTStatement(TreasureDeath input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `treasure_death` (`treasure_Type`, `tier`, `unknown_1`, `unknown_2`, `unknown_3`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `unknown_8`, `unknown_9`, `unknown_10`, `unknown_11`, `unknown_12`, `unknown_13`, `unknown_14`)");

            var output = "VALUES (" +
                $"{input.TreasureType}, " +
                $"{input.Tier}, " +
                $"{input.Unknown1}, " +
                $"{input.Unknown2}, " +
                $"{input.Unknown3}, " +
                $"{input.Unknown4}, " +
                $"{input.Unknown5}, " +
                $"{input.Unknown6}, " +
                $"{input.Unknown7}, " +
                $"{input.Unknown8}, " +
                $"{input.Unknown9}, " +
                $"{input.Unknown10}, " +
                $"{input.Unknown11}, " +
                $"{input.Unknown12}, " +
                $"{input.Unknown13}, " +
                $"{input.Unknown14}" +
                ");";

            output = FixNullFields(output);

            writer.WriteLine(output);
        }
    }
}
