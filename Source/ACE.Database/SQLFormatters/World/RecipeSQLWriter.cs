using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.SQLFormatters.World
{
    public class RecipeSQLWriter : SQLWriter
    {
        /// <summary>
        /// Default is formed from: input.RecipeId.ToString("00000") + " " + [SuccessWeenieName or Cook Book Source]
        /// </summary>
        public string GetDefaultFileName(Recipe input, IList<CookBook> cookBooks)
        {
            string description = null;

            if (WeenieNames != null)
            {
                if (WeenieNames.TryGetValue(input.SuccessWCID, out var weenieName))
                    description = weenieName;
            }

            string alternateDescription = null;

            if (cookBooks != null && cookBooks.Count > 0 && WeenieNames != null)
            {
                WeenieNames.TryGetValue(cookBooks[0].SourceWCID, out alternateDescription);

                for (int i = 1; i < cookBooks.Count; i++)
                {
                    if (WeenieNames.TryGetValue(cookBooks[i].SourceWCID, out var sourceWeenieName) && sourceWeenieName != alternateDescription)
                    {
                        alternateDescription = null;
                        break;
                    }
                }
            }

            if (String.IsNullOrEmpty(description) && !String.IsNullOrEmpty(alternateDescription))
                description = alternateDescription;

            if (description == "Cooking Pot" && !String.IsNullOrEmpty(alternateDescription))
                description = alternateDescription;

            string fileName = input.RecipeId.ToString("00000");
            if (!String.IsNullOrEmpty(description))
                fileName += " " + description;
            fileName = IllegalInFileName.Replace(fileName, "_");
            fileName += ".sql";

            return fileName;
        }

        public void CreateSQLDELETEStatement(Recipe input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `recipe` WHERE `recipe_Id` = {input.RecipeId};");
        }

        public void CreateSQLINSERTStatement(Recipe input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe` (`recipe_Id`, `unknown_1`, `skill`, `difficulty`, `salvage_Type`, `success_W_C_I_D`, `success_Amount`, `success_Message`, `fail_W_C_I_D`, `fail_Amount`, `fail_Message`, `data_Id`)");

            string skillLabel = null;
            if (input.Skill != 0)
                skillLabel = Enum.GetName(typeof(Skill), input.Skill);

            string successWeenieLabel = null;
            if (WeenieNames != null)
                WeenieNames.TryGetValue(input.SuccessWCID, out successWeenieLabel);

            string failWeenieLabel = null;
            if (WeenieNames != null)
                WeenieNames.TryGetValue(input.FailWCID, out failWeenieLabel);

            var output = "VALUES (" +
                             $"{input.RecipeId}, " +
                             $"{input.Unknown1}, " +
                             $"{input.Skill} /* {skillLabel} */, " +
                             $"{input.Difficulty}, " +
                             $"{input.SalvageType}, " +
                             $"{input.SuccessWCID} /* {successWeenieLabel} */, " +
                             $"{input.SuccessAmount}, " +
                             $"{GetSQLString(input.SuccessMessage)}, " +
                             $"{input.FailWCID} /* {failWeenieLabel} */, " +
                             $"{input.FailAmount}, " +
                             $"{GetSQLString(input.FailMessage)}, " +
                             $"{input.DataId}" +
                             ");";

            output = FixNullFields(output);

            writer.WriteLine(output);

            if (input.RecipeComponent != null && input.RecipeComponent.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeComponent.ToList(), writer);
            }

            if (input.RecipeRequirementsInt != null && input.RecipeRequirementsInt.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeRequirementsInt.ToList(), writer);
            }
            if (input.RecipeRequirementsDID != null && input.RecipeRequirementsDID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeRequirementsDID.ToList(), writer);
            }
            if (input.RecipeRequirementsIID != null && input.RecipeRequirementsIID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeRequirementsIID.ToList(), writer);
            }
            if (input.RecipeRequirementsFloat != null && input.RecipeRequirementsFloat.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeRequirementsFloat.ToList(), writer);
            }
            if (input.RecipeRequirementsString != null && input.RecipeRequirementsString.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeRequirementsString.ToList(), writer);
            }
            if (input.RecipeRequirementsBool != null && input.RecipeRequirementsBool.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeRequirementsBool.ToList(), writer);
            }

            if (input.RecipeMod != null && input.RecipeMod.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeMod.ToList(), writer);
            }
            if (input.RecipeModsInt != null && input.RecipeModsInt.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeModsInt.ToList(), writer);
            }
            if (input.RecipeModsDID != null && input.RecipeModsDID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeModsDID.ToList(), writer);
            }
            if (input.RecipeModsIID != null && input.RecipeModsIID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeModsIID.ToList(), writer);
            }
            if (input.RecipeModsFloat != null && input.RecipeModsFloat.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeModsFloat.ToList(), writer);
            }
            if (input.RecipeModsString != null && input.RecipeModsString.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeModsString.ToList(), writer);
            }
            if (input.RecipeModsBool != null && input.RecipeModsBool.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.RecipeId, input.RecipeModsBool.ToList(), writer);
            }
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeComponent> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_component` (`recipe_Id`, `destroy_Chance`, `destroy_Amount`, `destroy_Message`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].DestroyChance}, {input[i].DestroyAmount}, {GetSQLString(input[i].DestroyMessage)})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeRequirementsInt> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_requirements_int` (`recipe_Id`, `stat`, `value`, `enum`, `message`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string propertyValueDescription = GetIntPropertyDescription((PropertyInt)input[i].Stat, input[i].Value);

                var comment = Enum.GetName(typeof(PropertyInt), input[i].Stat);
                if (propertyValueDescription != null)
                    comment += " - " + propertyValueDescription;

                return $"{recipeId}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {GetSQLString(input[i].Message)}) /* {comment} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeRequirementsDID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_requirements_d_i_d` (`recipe_Id`, `stat`, `value`, `enum`, `message`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {GetSQLString(input[i].Message)}) /* {Enum.GetName(typeof(PropertyDataId), input[i].Stat)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeRequirementsIID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_requirements_i_i_d` (`recipe_Id`, `stat`, `value`, `enum`, `message`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {GetSQLString(input[i].Message)}) /* {Enum.GetName(typeof(PropertyInstanceId), input[i].Stat)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeRequirementsFloat> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_requirements_float` (`recipe_Id`, `stat`, `value`, `enum`, `message`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {GetSQLString(input[i].Message)}) /* {Enum.GetName(typeof(PropertyFloat), input[i].Stat)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeRequirementsString> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_requirements_string` (`recipe_Id`, `stat`, `value`, `enum`, `message`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].Stat.ToString().PadLeft(3)}, {GetSQLString(input[i].Value)}, {input[i].Enum}, {GetSQLString(input[i].Message)}) /* {Enum.GetName(typeof(PropertyString), input[i].Stat)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeRequirementsBool> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_requirements_bool` (`recipe_Id`, `stat`, `value`, `enum`, `message`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {GetSQLString(input[i].Message)}) /* {Enum.GetName(typeof(PropertyBool), input[i].Stat)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeMod> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_mod` (`recipe_Id`, `mod_Set_Id`, `health`, `unknown_2`, `mana`, `unknown_4`, `unknown_5`, `unknown_6`, `unknown_7`, `data_Id`, `unknown_9`, `instance_Id`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].ModSetId}, {input[i].Health}, {input[i].Unknown2}, {input[i].Mana}, {input[i].Unknown4}, {input[i].Unknown5}, {input[i].Unknown6}, {input[i].Unknown7}, {input[i].DataId}, {input[i].Unknown9}, {input[i].InstanceId})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeModsInt> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_mods_int` (`recipe_Id`, `mod_Set_Id`, `stat`, `value`, `enum`, `unknown_1`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string propertyValueDescription = GetIntPropertyDescription((PropertyInt)input[i].Stat, input[i].Value);

                var comment = Enum.GetName(typeof(PropertyInt), input[i].Stat);
                if (propertyValueDescription != null)
                    comment += " - " + propertyValueDescription;

                return $"{recipeId}, {input[i].ModSetId}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {input[i].Unknown1}) /* {comment} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeModsDID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_mods_d_i_d` (`recipe_Id`, `mod_Set_Id`, `stat`, `value`, `enum`, `unknown_1`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].ModSetId}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {input[i].Unknown1}) /* {Enum.GetName(typeof(PropertyDataId), input[i].Stat)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeModsIID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_mods_i_i_d` (`recipe_Id`, `mod_Set_Id`, `stat`, `value`, `enum`, `unknown_1`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].ModSetId}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {input[i].Unknown1}) /* {Enum.GetName(typeof(PropertyInstanceId), input[i].Stat)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeModsFloat> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_mods_float` (`recipe_Id`, `mod_Set_Id`, `stat`, `value`, `enum`, `unknown_1`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].ModSetId}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {input[i].Unknown1}) /* {Enum.GetName(typeof(PropertyFloat), input[i].Stat)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeModsString> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_mods_string` (`recipe_Id`, `mod_Set_Id`, `stat`, `value`, `enum`, `unknown_1`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].ModSetId}, {input[i].Stat.ToString().PadLeft(3)}, {GetSQLString(input[i].Value)}, {input[i].Enum}, {input[i].Unknown1}) /* {Enum.GetName(typeof(PropertyString), input[i].Stat)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeModsBool> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_mods_bool` (`recipe_Id`, `mod_Set_Id`, `stat`, `value`, `enum`, `unknown_1`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].ModSetId}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {input[i].Unknown1}) /* {Enum.GetName(typeof(PropertyBool), input[i].Stat)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }
    }
}
