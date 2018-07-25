using System.IO;

using ACE.Database.Models.World;

namespace ACE.Database.SQLFormatters.World
{
    public class CookBookSQLWriter : SQLWriter
    {
        /// <summary>
        /// Default is formed from: input.RecipeId.ToString("00000")
        /// </summary>
        public string GetDefaultFileName(CookBook input)
        {
            string fileName = input.RecipeId.ToString("00000");
            fileName = IllegalInFileName.Replace(fileName, "_");
            fileName += ".sql";

            return fileName;
        }

        public void CreateSQLDELETEStatement(CookBook input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `cook_book` WHERE `recipe_Id` = {input.RecipeId};");
        }

        public void CreateSQLINSERTStatement(CookBook input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `cook_book` (`recipe_Id`, `source_W_C_I_D`, `target_W_C_I_D`)");

            string sourceLabel = null;
            if (WeenieNames != null)
                WeenieNames.TryGetValue(input.SourceWCID, out sourceLabel);

            string targetLabel = null;
            if (WeenieNames != null)
                WeenieNames.TryGetValue(input.TargetWCID, out targetLabel);

            var output = $"VALUES ({input.RecipeId}, {input.SourceWCID} /* {sourceLabel} */, {input.TargetWCID} /* {targetLabel} */);";

            output = FixNullFields(output);

            writer.WriteLine(output);
        }
    }
}
