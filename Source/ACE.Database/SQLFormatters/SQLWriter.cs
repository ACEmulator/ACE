using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using ACE.Entity.Enum.Properties;

namespace ACE.Database.SQLFormatters
{
    public abstract class SQLWriter
    {
        protected static readonly Regex IllegalInFileName = new Regex($"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]", RegexOptions.Compiled);

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// </summary>
        public Dictionary<uint, string> WeenieClassNames;

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// If a weenie id is found in the dictionary, the name will be added in the form of a /* Friendly Weenie Name */
        /// </summary>
        public Dictionary<uint, string> WeenieNames;

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// If a spell id is found in the dictionary, the name will be added in the form of a /* Friendly Spell Name */
        /// </summary>
        public Dictionary<uint, string> SpellNames;

        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// If a opcode is found in the dictionary, the name will be added in the form of a /* Friendly Opcode Name */
        /// </summary>
        public Dictionary<uint, string> PacketOpCodes;

        /// <summary>
        /// lineGenerator should generate the entire line after the first (. It should include the trailing ) and any comments after.<para />
        /// It should consist of only a single line.<para />
        /// This will automatically call FixNullFields on the output created by lineGenerator()
        /// </summary>
        protected static void ValuesWriter(int count, Func<int, string> lineGenerator, StreamWriter writer)
        {
            for (int i = 0; i < count; i++)
            {
                string output;

                if (i == 0)
                    output = "VALUES (";
                else
                    output = "     , (";

                output += lineGenerator(i);

                if (i == count - 1)
                    output += ";";

                output = FixNullFields(output);

                writer.WriteLine(output);
            }
        }

        /// <summary>
        /// If input is null, NULL will be returned.<para />
        /// If input is not null, a string surrounded in ' will be returned, and any ' found within the string will be replaced wtih ''
        /// </summary>
        protected static string GetSQLString(string input)
        {
            if (input == null)
                return null;

            return "'" + input.Replace("'", "''") + "'";
        }

        /// <summary>
        /// This will find values that were not output to a values line, for example, if a property is a (int?), and it has no value, you might see ", ," in the sql.<para />
        /// This function will replace that ", ," with ", NULL,".<para />
        /// It also removes empty comments like the folliwng: " /*  */"
        /// </summary>
        protected static string FixNullFields(string input)
        {
            input = input.Replace(", ,", ", NULL,");
            input = input.Replace(", ,", ", NULL,");

            // Fix cases where the last field might be null
            input = input.Replace(", )", ", NULL)");

            // Remove empty comments
            input = input.Replace(" /*  */", "");

            return input;
        }

        protected string GetValueEnumName(PropertyInt property, int value)
        {
            switch (property)
            {
                case PropertyInt.ActivationCreateClass:
                    if (WeenieNames != null)
                    {
                        WeenieNames.TryGetValue((uint) value, out var propertyValueDescription);
                        return propertyValueDescription;
                    }
                    break;
            }

            return property.GetValueEnumName(value);
        }

        protected string GetValueEnumName(PropertyDataId property, uint value)
        {
            switch (property)
            {
                case PropertyDataId.AlternateCurrency:
                case PropertyDataId.AugmentationCreateItem:
                case PropertyDataId.LastPortal:
                case PropertyDataId.LinkedPortalOne:
                case PropertyDataId.LinkedPortalTwo:
                case PropertyDataId.OriginalPortal:
                case PropertyDataId.UseCreateItem:
                case PropertyDataId.VendorsClassId:
                    if (WeenieNames != null)
                    {
                        WeenieNames.TryGetValue(value, out var propertyValueDescription);
                        return propertyValueDescription;
                    }
                    break;
                case PropertyDataId.BlueSurgeSpell:
                case PropertyDataId.DeathSpell:
                case PropertyDataId.ProcSpell:
                case PropertyDataId.RedSurgeSpell:
                case PropertyDataId.Spell:
                case PropertyDataId.YellowSurgeSpell:
                    if (SpellNames != null)
                    {
                        SpellNames.TryGetValue(value, out var propertyValueDescription);
                        return propertyValueDescription;
                    }
                    break;
            }

            return property.GetValueEnumName(value);
        }
    }
}
