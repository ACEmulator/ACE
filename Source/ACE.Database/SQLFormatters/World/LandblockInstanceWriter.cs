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
            writer.WriteLine($"DELETE FROM `landblock_instance` WHERE `landblock` = 0x{(input[0].ObjCellId >> 16):X4};");
        }

        /// <exception cref="System.Exception">WeenieClassNames must be set, and must have a record for input.ClassId.</exception>
        public void CreateSQLINSERTStatement(IList<LandblockInstance> input, StreamWriter writer)
        {
            var instanceWcids = input.ToDictionary(i => i.Guid, i => i.WeenieClassId);

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
                             $"0x{value.Guid.ToString("X8")}, " +
                             $"{value.WeenieClassId.ToString().PadLeft(5)}, " +
                             $"0x{value.ObjCellId:X8}, " +
                             $"{value.OriginX}, " +
                             $"{value.OriginY}, " +
                             $"{value.OriginZ}, " +
                             $"{value.AnglesW}, " +
                             $"{value.AnglesX}, " +
                             $"{value.AnglesY}, " +
                             $"{value.AnglesZ}, " +
                             $"{value.IsLinkChild.ToString().PadLeft(5)}, " +
                             $"'{value.LastModified:yyyy-MM-dd HH:mm:ss}'" +
                             $"); /* {label} */" +
                             Environment.NewLine + $"/* @teleloc 0x{value.ObjCellId:X8} [{value.OriginX:F6} {value.OriginY:F6} {value.OriginZ:F6}] {value.AnglesW:F6} {value.AnglesX:F6} {value.AnglesY:F6} {value.AnglesZ:F6} */";

                output = FixNullFields(output);

                writer.WriteLine(output);

                if (value.LandblockInstanceLink != null && value.LandblockInstanceLink.Count > 0)
                {
                    writer.WriteLine();
                    CreateSQLINSERTStatement(value.LandblockInstanceLink.OrderBy(r => r.ChildGuid).ToList(), instanceWcids, writer);
                }
            }
        }

        private void CreateSQLINSERTStatement(IList<LandblockInstanceLink> input, Dictionary<uint, uint> instanceWcids, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string label = null;

                if (WeenieNames != null && instanceWcids.TryGetValue(input[i].ChildGuid, out var wcid) && WeenieNames.TryGetValue(wcid, out var weenieName))
                    label = $" /* {weenieName} ({wcid}) */";

                return $"0x{input[i].ParentGuid.ToString("X8")}, 0x{input[i].ChildGuid.ToString("X8")}, '{input[i].LastModified.ToString("yyyy-MM-dd HH:mm:ss")}'){label}";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }
    }
}
