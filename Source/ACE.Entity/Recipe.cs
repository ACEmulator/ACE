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
        public Guid RecipeGuid { get; set; }

        [DbField("recipeGuid", (int)MySqlDbType.Binary, IsCriteria = true, Update = false)]
        public byte[] RecipeGuid_Binder
        {
            get { return RecipeGuid.ToByteArray(); }
            set { RecipeGuid = new Guid(value); }
        }

        /// <summary>
        /// This is a mocked property that will set a flag in the database any time this object is altered.  this flag
        /// will allow us to detect objects that have changed post-installation and generate changesetss
        /// </summary>
        [DbField("userModified", (int)MySqlDbType.Bit)]
        public virtual bool UserModified
        {
            get { return true; }
            set { } // method intentionally not implemented
        }

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

        [DbField("successItem1Wcid", (int)MySqlDbType.UInt32)]
        public uint? SuccessItem1Wcid { get; set; }

        [DbField("successItem2Wcid", (int)MySqlDbType.UInt32)]
        public uint? SuccessItem2Wcid { get; set; }

        [DbField("failureItem1Wcid", (int)MySqlDbType.UInt32)]
        public uint? FailureItem1Wcid { get; set; }

        [DbField("failureItem2Wcid", (int)MySqlDbType.UInt32)]
        public uint? FailureItem2Wcid { get; set; }

        /// <summary>
        /// enum source: ACE.Entity.Enum.Properties.PropertyAttribute
        /// </summary>
        [DbField("healingAttribute", (int)MySqlDbType.UInt16)]
        public ushort? HealingAttribute { get; set; }
    }
}
