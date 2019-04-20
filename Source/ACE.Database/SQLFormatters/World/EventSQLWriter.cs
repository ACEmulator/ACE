using System;
using System.Globalization;
using System.IO;

using ACE.Database.Models.World;

namespace ACE.Database.SQLFormatters.World
{
    public class EventSQLWriter : SQLWriter
    {
        /// <summary>
        /// Default is formed from: input.name
        /// </summary>
        public string GetDefaultFileName(Event input)
        {
            string fileName = input.Name;
            fileName = IllegalInFileName.Replace(fileName, "_");
            fileName += ".sql";

            return fileName;
        }

        public void CreateSQLDELETEStatement(Event input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `event` WHERE `name` = {GetSQLString(input.Name)};");
        }

        public void CreateSQLINSERTStatement(Event input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `event` (`name`, `start_Time`, `end_Time`, `state`, `last_Modified`)");

            var output = "VALUES (" +
                             $"{GetSQLString(input.Name)}, " +
                             $"{(input.StartTime == -1 ? $"{input.StartTime}" : $"{input.StartTime} /* {DateTimeOffset.FromUnixTimeSeconds(input.StartTime).DateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture)} */")}, " +
                             $"{(input.EndTime == -1 ? $"{input.EndTime}" : $"{input.EndTime} /* {DateTimeOffset.FromUnixTimeSeconds(input.EndTime).DateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture)} */")}, " +
                             $"{input.State}, " +
                             $"'{input.LastModified:yyyy-MM-dd HH:mm:ss}'" +
                             ");";

            output = FixNullFields(output);

            writer.WriteLine(output);
        }
    }
}
