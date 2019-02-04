using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Database.Models.World;

namespace ACE.Database.SQLFormatters.World
{
    public class LandblockInstanceWriter : SQLWriter
    {
        /// <summary>
        /// Set this to enable auto commenting when creating SQL statements.<para />
        /// If a child link is found in the dictionary, the name will be added in the form of a /* Friendly Instance Name */
        /// </summary>
        public Dictionary<uint, string> InstanceNames;

        /// <summary>
        /// Default is formed from: (input.ObjCellId >> 16).ToString("X4")
        /// </summary>
        public string GetDefaultFileName(LandblockInstance input)
        {
            string fileName = (input.ObjCellId >> 16).ToString("X4");
            fileName = IllegalInFileName.Replace(fileName, "_");
            fileName += ".sql";

            return fileName;
        }

        public void CreateSQLDELETEStatement(IList<LandblockInstance> input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `landblock_instance` WHERE `landblock` = {input[0].ObjCellId >> 16};");
        }

        /// <exception cref="System.Exception">WeenieClassNames must be set, and must have a record for input.ClassId.</exception>
        public void CreateSQLINSERTStatement(IList<LandblockInstance> input, StreamWriter writer)
        {
            input = input.OrderBy(r => r.Guid).ToList();

            foreach (var value in input)
            {
                if (value != input[0])
                    writer.WriteLine();

                writer.WriteLine("INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)");

                string label = null;

                if (WeenieNames != null)
                    WeenieNames.TryGetValue(value.WeenieClassId, out label);

                var output = "VALUES (" +
                             $"{value.Guid.ToString().PadLeft(10)}, " +
                             $"{value.WeenieClassId.ToString().PadLeft(5)}, " +
                             $"{value.ObjCellId}, " +
                             $"{value.OriginX}, " +
                             $"{value.OriginY}, " +
                             $"{value.OriginZ}, " +
                             $"{value.AnglesW}, " +
                             $"{value.AnglesX}, " +
                             $"{value.AnglesY}, " +
                             $"{value.AnglesZ}, " +
                             $"{value.IsLinkChild.ToString().PadLeft(5)}, " +
                             $"'{value.LastModified.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                             $"); /* {label} */" +
                             Environment.NewLine + $"/* @teleloc 0x{value.ObjCellId.ToString("X8")} [{value.OriginX.ToString("F6")} {value.OriginY.ToString("F6")} {value.OriginZ.ToString("F6")}] {value.AnglesW.ToString("F6")} {value.AnglesX.ToString("F6")} {value.AnglesY.ToString("F6")} {value.AnglesZ.ToString("F6")} */";

                output = FixNullFields(output);

                writer.WriteLine(output);

                if (value.LandblockInstanceLink != null && value.LandblockInstanceLink.Count > 0)
                {
                    writer.WriteLine();
                    CreateSQLINSERTStatement(value.LandblockInstanceLink.OrderBy(r => r.ChildGuid).ToList(), writer);
                }
            }
        }

        private void CreateSQLINSERTStatement(IList<LandblockInstanceLink> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string label = null;

                if (InstanceNames != null)
                    InstanceNames.TryGetValue(input[i].ChildGuid, out label);

                return $"{input[i].ParentGuid.ToString().PadLeft(10)}, {input[i].ChildGuid.ToString().PadLeft(10)}, '{input[i].LastModified.ToString("yyyy-MM-dd HH:mm:ss")}') /* {label} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }
    }
}
