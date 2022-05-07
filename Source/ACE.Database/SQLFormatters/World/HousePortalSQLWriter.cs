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

            var lineGenerator = new Func<int, string>(i => $"{input[i].HouseId}, 0x{input[i].ObjCellId:X8}, {TrimNegativeZero(input[i].OriginX):0.######}, {TrimNegativeZero(input[i].OriginY):0.######}, {TrimNegativeZero(input[i].OriginZ):0.######}, {TrimNegativeZero(input[i].AnglesW):0.######}, {TrimNegativeZero(input[i].AnglesX):0.######}, {TrimNegativeZero(input[i].AnglesY):0.######}, {TrimNegativeZero(input[i].AnglesZ):0.######}, '{input[i].LastModified:yyyy-MM-dd HH:mm:ss}')" + Environment.NewLine + $"/* @teleloc 0x{input[i].ObjCellId:X8} [{TrimNegativeZero(input[i].OriginX):F6} {TrimNegativeZero(input[i].OriginY):F6} {TrimNegativeZero(input[i].OriginZ):F6}] {TrimNegativeZero(input[i].AnglesW):F6} {TrimNegativeZero(input[i].AnglesX):F6} {TrimNegativeZero(input[i].AnglesY):F6} {TrimNegativeZero(input[i].AnglesZ):F6} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }
    }
}
