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
            throw new NotImplementedException();
        }

        public void CreateSQLINSERTStatement(IList<HousePortal> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `house_portal` (`house_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_X`, `angles_Y`, `angles_Z`, `angles_W`)");

            var lineGenerator = new Func<int, string>(i => $"{input[i].HouseId}, {input[i].ObjCellId}, {input[i].OriginX}, {input[i].OriginY}, {input[i].OriginZ}, {input[i].AnglesX}, {input[i].AnglesY}, {input[i].AnglesZ}, {input[i].AnglesW})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }
    }
}
