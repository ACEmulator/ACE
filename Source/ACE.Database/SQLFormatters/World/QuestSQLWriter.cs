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
            writer.WriteLine($"DELETE FROM `quest` WHERE `name` = '{input.Name.Replace("'", "''")}';");
        }

        public void CreateSQLINSERTStatement(Quest input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `quest` (`name`, `min_Delta`, `max_Solves`, `message`)");

            writer.WriteLine($"VALUES ('{input.Name.Replace("'", "''")}', {input.MinDelta}, {input.MaxSolves}, '{input.Message.Replace("'", "''")}');");
        }
    }
}
