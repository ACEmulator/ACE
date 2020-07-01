using System;
using System.Collections.Generic;
using System.IO;

using ACE.Database.Models.World;

namespace ACE.Database.SQLFormatters.World
{
    public class HousePortalSQLWriter : SQLWriter
    {
        /// <summary>
        /// Default is formed from: input.HouseId.ToString("00000")
        /// </summary>
        public string GetDefaultFileName(HousePortal input)
        {
            string fileName = input.HouseId.ToString("00000");
            fileName = IllegalInFileName.Replace(fileName, "_");
            fileName += ".sql";

            return fileName;
        }

        public void CreateSQLDELETEStatement(IList<HousePortal> input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `house_portal` WHERE `house_Id` = {input[0].HouseId};");
        }

        public void CreateSQLINSERTStatement(IList<HousePortal> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `house_portal` (`house_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `last_Modified`)");

            var lineGenerator = new Func<int, string>(i => $"{input[i].HouseId}, 0x{input[i].ObjCellId:X8}, {input[i].OriginX}, {input[i].OriginY}, {input[i].OriginZ}, {input[i].AnglesW}, {input[i].AnglesX}, {input[i].AnglesY}, {input[i].AnglesZ}, '{input[i].LastModified:yyyy-MM-dd HH:mm:ss}')" + Environment.NewLine + $"/* @teleloc 0x{input[i].ObjCellId:X8} [{input[i].OriginX:F6} {input[i].OriginY:F6} {input[i].OriginZ:F6}] {input[i].AnglesW:F6} {input[i].AnglesX:F6} {input[i].AnglesY:F6} {input[i].AnglesZ:F6} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }
    }
}
