using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

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
        /// This will find values that were not output to a values line, for example, if a property is a (int?), and it has no value, you might see ", ," in the sql.<para />
        /// This function will replace that ", ," with ", NULL,".<para />
        /// It will do the same thing for strings that were output as ", '',"<para />
        /// It also removes empty comments like the folliwng: " /*  */"
        /// </summary>
        protected static string FixNullFields(string input)
        {
            // We must do this twice for each to account for adjacent matches since we search inclusive of a start and end comma
            input = input.Replace(", '',", ", NULL,");
            input = input.Replace(", '',", ", NULL,");

            input = input.Replace(", ,", ", NULL,");
            input = input.Replace(", ,", ", NULL,");

            // Fix cases where the last field might be null
            input = input.Replace(", )", ", NULL)");
            input = input.Replace(", '')", ", NULL)");

            // Remove empty comments
            input = input.Replace(" /*  */", "");

            return input;
        }
    }
}
