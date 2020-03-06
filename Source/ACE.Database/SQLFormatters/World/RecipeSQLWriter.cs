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
        public string GetDefaultFileName(Recipe input, IList<CookBook> cookBooks, bool descOnly = false)
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

            if (string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(alternateDescription))
                description = alternateDescription;

            if (description == "Cooking Pot" && !string.IsNullOrEmpty(alternateDescription))
                description = alternateDescription;

            if (descOnly)
                return description;

            string fileName = input.Id.ToString("00000");
            if (!string.IsNullOrEmpty(description))
                fileName += " " + description;
            fileName = IllegalInFileName.Replace(fileName, "_");
            fileName += ".sql";

            return fileName;
        }

        public void CreateSQLDELETEStatement(Recipe input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `recipe` WHERE `id` = {input.Id};");
        }

        public void CreateSQLINSERTStatement(Recipe input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe` (`id`, `unknown_1`, `skill`, `difficulty`, `salvage_Type`, `success_W_C_I_D`, `success_Amount`, `success_Message`, `fail_W_C_I_D`, `fail_Amount`, `fail_Message`, " +
                             "`success_Destroy_Source_Chance`, `success_Destroy_Source_Amount`, `success_Destroy_Source_Message`, `success_Destroy_Target_Chance`, `success_Destroy_Target_Amount`, `success_Destroy_Target_Message`, " +
                             "`fail_Destroy_Source_Chance`, `fail_Destroy_Source_Amount`, `fail_Destroy_Source_Message`, `fail_Destroy_Target_Chance`, `fail_Destroy_Target_Amount`, `fail_Destroy_Target_Message`, " +
                             "`data_Id`, `last_Modified`)");

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
                         $"{input.Id}, " +
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
                         $"{input.SuccessDestroySourceChance}, " +
                         $"{input.SuccessDestroySourceAmount}, " +
                         $"{GetSQLString(input.SuccessDestroySourceMessage)}, " +
                         $"{input.SuccessDestroyTargetChance}, " +
                         $"{input.SuccessDestroyTargetAmount}, " +
                         $"{GetSQLString(input.SuccessDestroyTargetMessage)}, " +
                         $"{input.FailDestroySourceChance}, " +
                         $"{input.FailDestroySourceAmount}, " +
                         $"{GetSQLString(input.FailDestroySourceMessage)}, " +
                         $"{input.FailDestroyTargetChance}, " +
                         $"{input.FailDestroyTargetAmount}, " +
                         $"{GetSQLString(input.FailDestroyTargetMessage)}, " +
                         $"{input.DataId}, " +
                         $"'{input.LastModified:yyyy-MM-dd HH:mm:ss}'" +
                         ");";

            output = FixNullFields(output);

            writer.WriteLine(output);

            if (input.RecipeRequirementsInt != null && input.RecipeRequirementsInt.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.RecipeRequirementsInt.ToList(), writer);
            }
            if (input.RecipeRequirementsDID != null && input.RecipeRequirementsDID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.RecipeRequirementsDID.ToList(), writer);
            }
            if (input.RecipeRequirementsIID != null && input.RecipeRequirementsIID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.RecipeRequirementsIID.ToList(), writer);
            }
            if (input.RecipeRequirementsFloat != null && input.RecipeRequirementsFloat.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.RecipeRequirementsFloat.ToList(), writer);
            }
            if (input.RecipeRequirementsString != null && input.RecipeRequirementsString.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.RecipeRequirementsString.ToList(), writer);
            }
            if (input.RecipeRequirementsBool != null && input.RecipeRequirementsBool.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.RecipeRequirementsBool.ToList(), writer);
            }

            if (input.RecipeMod != null && input.RecipeMod.Count > 0)
            {
                //writer.WriteLine(); // This is not needed because CreateSQLINSERTStatement will take care of it for us on each Recipe.
                CreateSQLINSERTStatement(input.Id, input.RecipeMod.ToList(), writer);
            }
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeRequirementsInt> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_requirements_int` (`recipe_Id`, `index`, `stat`, `value`, `enum`, `message`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string propertyValueDescription = GetValueEnumName((PropertyInt)input[i].Stat, input[i].Value);

                var comment = Enum.GetName(typeof(PropertyInt), input[i].Stat);
                if (propertyValueDescription != null)
                    comment += " - " + propertyValueDescription;

                return $"{recipeId}, {input[i].Index}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {GetSQLString(input[i].Message)}) /* {((RequirementType)input[i].Index).ToString()}.{comment} {((CompareType)input[i].Enum).ToString()} {input[i].Value} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeRequirementsDID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_requirements_d_i_d` (`recipe_Id`, `index`, `stat`, `value`, `enum`, `message`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].Index}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {GetSQLString(input[i].Message)}) /* {((RequirementType)input[i].Index).ToString()}.{Enum.GetName(typeof(PropertyDataId), input[i].Stat)} {((CompareType)input[i].Enum).ToString()} {input[i].Value} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeRequirementsIID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_requirements_i_i_d` (`recipe_Id`, `index`, `stat`, `value`, `enum`, `message`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].Index}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {GetSQLString(input[i].Message)}) /* {((RequirementType)input[i].Index).ToString()}.{Enum.GetName(typeof(PropertyInstanceId), input[i].Stat)} {((CompareType)input[i].Enum).ToString()} {input[i].Value} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeRequirementsFloat> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_requirements_float` (`recipe_Id`, `index`, `stat`, `value`, `enum`, `message`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].Index}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {GetSQLString(input[i].Message)}) /* {((RequirementType)input[i].Index).ToString()}.{Enum.GetName(typeof(PropertyFloat), input[i].Stat)} {((CompareType)input[i].Enum).ToString()} {input[i].Value} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeRequirementsString> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_requirements_string` (`recipe_Id`, `index`, `stat`, `value`, `enum`, `message`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].Index}, {input[i].Stat.ToString().PadLeft(3)}, {GetSQLString(input[i].Value)}, {input[i].Enum}, {GetSQLString(input[i].Message)}) /* {((RequirementType)input[i].Index).ToString()}.{Enum.GetName(typeof(PropertyString), input[i].Stat)} {((CompareType)input[i].Enum).ToString()} {input[i].Value} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeRequirementsBool> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_requirements_bool` (`recipe_Id`, `index`, `stat`, `value`, `enum`, `message`)");

            var lineGenerator = new Func<int, string>(i => $"{recipeId}, {input[i].Index}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {GetSQLString(input[i].Message)}) /* {((RequirementType)input[i].Index).ToString()}.{Enum.GetName(typeof(PropertyBool), input[i].Stat)} {((CompareType)input[i].Enum).ToString()} {input[i].Value} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint recipeId, IList<RecipeMod> input, StreamWriter writer)
        {
            foreach (var value in input)
            {
                writer.WriteLine();
                writer.WriteLine("INSERT INTO `recipe_mod` (`recipe_Id`, `executes_On_Success`, `health`, `stamina`, `mana`, `unknown_7`, `data_Id`, `unknown_9`, `instance_Id`)");

                var output = $"VALUES ({recipeId}, {value.ExecutesOnSuccess}, {value.Health}, {value.Stamina}, {value.Mana}, {value.Unknown7}, {value.DataId}, {value.Unknown9}, {value.InstanceId});";

                output = FixNullFields(output);

                writer.WriteLine(output);

                if ((value.RecipeModsInt != null && value.RecipeModsInt.Count > 0) ||
                    (value.RecipeModsDID != null && value.RecipeModsDID.Count > 0) ||
                    (value.RecipeModsIID != null && value.RecipeModsIID.Count > 0) ||
                    (value.RecipeModsFloat != null && value.RecipeModsFloat.Count > 0) ||
                    (value.RecipeModsString != null && value.RecipeModsString.Count > 0) ||
                    (value.RecipeModsBool != null && value.RecipeModsBool.Count > 0))
                {
                    writer.WriteLine();
                    writer.WriteLine("SET @parent_id = LAST_INSERT_ID();");
                }

                if (value.RecipeModsInt != null && value.RecipeModsInt.Count > 0)
                {
                    writer.WriteLine();
                    CreateSQLINSERTStatement(value.RecipeModsInt.ToList(), writer);
                }
                if (value.RecipeModsDID != null && value.RecipeModsDID.Count > 0)
                {
                    writer.WriteLine();
                    CreateSQLINSERTStatement(value.RecipeModsDID.ToList(), writer);
                }
                if (value.RecipeModsIID != null && value.RecipeModsIID.Count > 0)
                {
                    writer.WriteLine();
                    CreateSQLINSERTStatement(value.RecipeModsIID.ToList(), writer);
                }
                if (value.RecipeModsFloat != null && value.RecipeModsFloat.Count > 0)
                {
                    writer.WriteLine();
                    CreateSQLINSERTStatement(value.RecipeModsFloat.ToList(), writer);
                }
                if (value.RecipeModsString != null && value.RecipeModsString.Count > 0)
                {
                    writer.WriteLine();
                    CreateSQLINSERTStatement(value.RecipeModsString.ToList(), writer);
                }
                if (value.RecipeModsBool != null && value.RecipeModsBool.Count > 0)
                {
                    writer.WriteLine();
                    CreateSQLINSERTStatement(value.RecipeModsBool.ToList(), writer);
                }
            }
        }

        private void CreateSQLINSERTStatement(IList<RecipeModsInt> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_mods_int` (`recipe_Mod_Id`, `index`, `stat`, `value`, `enum`, `source`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string propertyValueDescription = GetValueEnumName((PropertyInt)input[i].Stat, input[i].Value);

                var comment = Enum.GetName(typeof(PropertyInt), input[i].Stat);
                if (propertyValueDescription != null)
                    comment += " - " + propertyValueDescription;

                return $"@parent_id, {input[i].Index}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {input[i].Source}) /* On {((RecipeSourceType)input[i].Source).ToString()}.{((ModificationType)input[i].Index).ToString()} {((ModificationOperation)input[i].Enum).ToString()}{(comment == null ? "" : $" {comment}")}{(comment != null && comment.Contains(" - ") || input[i].Enum == 7 ? "" : $" {input[i].Value}")}{(input[i].Enum == 7 ? $" {((SpellId)input[i].Stat).ToString()}" : "")} to {((ModificationType)input[i].Index).ToString().Substring(7)} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        private void CreateSQLINSERTStatement(IList<RecipeModsDID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_mods_d_i_d` (`recipe_Mod_Id`, `index`, `stat`, `value`, `enum`, `source`)");

            var lineGenerator = new Func<int, string>(i => $"@parent_id, {input[i].Index}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {input[i].Source}) /* On {((RecipeSourceType)input[i].Source).ToString()}.{((ModificationType)input[i].Index).ToString()} {((ModificationOperation)input[i].Enum).ToString()} {Enum.GetName(typeof(PropertyDataId), input[i].Stat)} to {((ModificationType)input[i].Index).ToString().Substring(7)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        private void CreateSQLINSERTStatement(IList<RecipeModsIID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_mods_i_i_d` (`recipe_Mod_Id`, `index`, `stat`, `value`, `enum`, `source`)");

            var lineGenerator = new Func<int, string>(i => $"@parent_id, {input[i].Index}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {input[i].Source}) /* On {((RecipeSourceType)input[i].Source).ToString()}.{((ModificationType)input[i].Index).ToString()} {((ModificationOperation)input[i].Enum).ToString()} {Enum.GetName(typeof(PropertyInstanceId), input[i].Stat)} to {((ModificationType)input[i].Index).ToString().Substring(7)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        private void CreateSQLINSERTStatement(IList<RecipeModsFloat> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_mods_float` (`recipe_Mod_Id`, `index`, `stat`, `value`, `enum`, `source`)");

            var lineGenerator = new Func<int, string>(i => $"@parent_id, {input[i].Index}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {input[i].Source}) /* On {((RecipeSourceType)input[i].Source).ToString()}.{((ModificationType)input[i].Index).ToString()} {((ModificationOperation)input[i].Enum).ToString()} {Enum.GetName(typeof(PropertyFloat), input[i].Stat)} to {((ModificationType)input[i].Index).ToString().Substring(7)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        private void CreateSQLINSERTStatement(IList<RecipeModsString> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_mods_string` (`recipe_Mod_Id`, `index`, `stat`, `value`, `enum`, `source`)");

            var lineGenerator = new Func<int, string>(i => $"@parent_id, {input[i].Index}, {input[i].Stat.ToString().PadLeft(3)}, {GetSQLString(input[i].Value)}, {input[i].Enum}, {input[i].Source}) /* On {((RecipeSourceType)input[i].Source).ToString()}.{((ModificationType)input[i].Index).ToString()} {((ModificationOperation)input[i].Enum).ToString()} {Enum.GetName(typeof(PropertyString), input[i].Stat)} to {((ModificationType)input[i].Index).ToString().Substring(7)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        private void CreateSQLINSERTStatement(IList<RecipeModsBool> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `recipe_mods_bool` (`recipe_Mod_Id`, `index`, `stat`, `value`, `enum`, `source`)");

            var lineGenerator = new Func<int, string>(i => $"@parent_id, {input[i].Index}, {input[i].Stat.ToString().PadLeft(3)}, {input[i].Value}, {input[i].Enum}, {input[i].Source}) /* On {((RecipeSourceType)input[i].Source).ToString()}.{((ModificationType)input[i].Index).ToString()} {((ModificationOperation)input[i].Enum).ToString()} {Enum.GetName(typeof(PropertyBool), input[i].Stat)} to {((ModificationType)input[i].Index).ToString().Substring(7)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }
    }
}
