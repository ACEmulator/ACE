using ACE.Common;
using ACE.Entity.Enum;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    [DbTable("ace_recipe")]
    public class Recipe
    {
        [DbField("recipeId", (int)MySqlDbType.UInt32)]
        public uint RecipeId { get; set; }

        /// <summary>
        /// enum RecipeType
        /// </summary>
        [DbField("recipeType", (int)MySqlDbType.Byte)]
        public byte RecipeType { get; set; }

        [DbField("sourceWcid", (int)MySqlDbType.UInt32)]
        public uint SourceWcid { get; set; }

        [DbField("targetWcid", (int)MySqlDbType.UInt32)]
        public uint TargetWcid { get; set; }

        [DbField("skillId", (int)MySqlDbType.UInt16)]
        public ushort? SkillId { get; set; }

        [DbField("skillDifficulty", (int)MySqlDbType.UInt16)]
        public ushort? SkillDifficulty { get; set; }

        [DbField("successMessage", (int)MySqlDbType.Text)]
        public string SuccessMessage { get; set; }

        [DbField("failMessage", (int)MySqlDbType.Text)]
        public string FailMessage { get; set; }

        /// <summary>
        /// used by dyeing for the alt-colors
        /// </summary>
        [DbField("alternateMessage", (int)MySqlDbType.Text)]
        public string AlternateMessage { get; set; }

        /// <summary>
        /// enum RecipeResult
        /// </summary>
        [DbField("resultFlags", (int)MySqlDbType.UInt32)]
        public uint ResultFlags { get; set; }

        [DbField("item1Wcid", (int)MySqlDbType.UInt32)]
        public uint? Item1Wcid { get; set; }

        [DbField("item2Wcid", (int)MySqlDbType.UInt32)]
        public uint? Item2Wcid { get; set; }

        /// <summary>
        /// enum source: ACE.Entity.Enum.Properties.PropertyAttribute
        /// </summary>
        [DbField("healingAttribute", (int)MySqlDbType.UInt16)]
        public ushort? HealingAttribute { get; set; }
    }
}
