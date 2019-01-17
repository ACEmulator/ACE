using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.SQLWriters
{
    static class SpellsSQLWriter
    {
        public static void WriteFiles(IEnumerable<ACE.Database.Models.World.Spell> input, string outputFolder, Dictionary<uint, string> weenieNames, bool includeDELETEStatementBeforeInsert = false)
        {
            Parallel.ForEach(input, value =>
            //foreach (var value in input)
            {
                WriteFile(value, outputFolder, weenieNames, includeDELETEStatementBeforeInsert);
            });
        }

        public static void WriteFile(ACE.Database.Models.World.Spell input, string outputFolder, Dictionary<uint, string> weenieNames, bool includeDELETEStatementBeforeInsert = false)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            var sqlWriter = new ACE.Database.SQLFormatters.World.SpellSQLWriter();

            sqlWriter.WeenieNames = weenieNames;

            string fileName = sqlWriter.GetDefaultFileName(input);

            using (StreamWriter writer = new StreamWriter(outputFolder + fileName))
            {
                if (includeDELETEStatementBeforeInsert)
                {
                    sqlWriter.CreateSQLDELETEStatement(input, writer);
                    writer.WriteLine();
                }

                sqlWriter.CreateSQLINSERTStatement(input, writer);
            }
        }
    }
}
