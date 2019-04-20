using System.IO;

using ACE.Database.Models.World;

namespace ACE.Database.SQLFormatters.World
{
    public class QuestSQLWriter : SQLWriter
    {
        /// <summary>
        /// Default is formed from: input.Name
        /// </summary>
        public string GetDefaultFileName(Quest input)
        {
            string fileName = input.Name;
            fileName = IllegalInFileName.Replace(fileName, "_");
            fileName += ".sql";

            return fileName;
        }

        public void CreateSQLDELETEStatement(Quest input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `quest` WHERE `name` = {GetSQLString(input.Name)};");
        }

        public void CreateSQLINSERTStatement(Quest input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `quest` (`name`, `min_Delta`, `max_Solves`, `message`, `last_Modified`)");

            var output = $"VALUES ({GetSQLString(input.Name)}, {input.MinDelta}, {input.MaxSolves}, {GetSQLString(input.Message)}, '{input.LastModified:yyyy-MM-dd HH:mm:ss}');";

            output = FixNullFields(output);

            writer.WriteLine(output);
        }
    }
}
