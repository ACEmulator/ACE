using System;
using System.Collections.Generic;
using System.IO;

using ACE.Database.Models.World;

namespace ACE.Database.SQLFormatters.World
{
    public class LandblockInstancesWriter : SQLWriter
    {
        /// <summary>
        /// Default is formed from: (input.ObjCellId >> 16).ToString("X4")
        /// </summary>
        public string GetDefaultFileName(LandblockInstances input)
        {
            string fileName = (input.ObjCellId >> 16).ToString("X4");
            fileName = IllegalInFileName.Replace(fileName, "_");
            fileName += ".sql";

            return fileName;
        }

        public void CreateSQLDELETEStatement(IList<LandblockInstances> input, StreamWriter writer)
        {
            throw new NotImplementedException();
        }

        /// <exception cref="System.Exception">WeenieClassNames must be set, and must have a record for input.ClassId.</exception>
        public void CreateSQLINSERTStatement(IList<LandblockInstances> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `landblock_instances` (`weenie_Class_Id`, `guid`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `link_Slot`, `link_Controller`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string label = null;

                if (WeenieNames != null)
                    WeenieNames.TryGetValue(input[i].WeenieClassId, out label);

                return $"{input[i].WeenieClassId.ToString().PadLeft(5)}, " +
                       $"{input[i].Guid}, " +
                       $"{input[i].ObjCellId}, " +
                       $"{input[i].OriginX}, " +
                       $"{input[i].OriginY}, " +
                       $"{input[i].OriginZ}, " +
                       $"{input[i].AnglesW}, " +
                       $"{input[i].AnglesX}, " +
                       $"{input[i].AnglesY}, " +
                       $"{input[i].AnglesZ}, " +
                       $"{input[i].LinkSlot}, " +
                       $"{input[i].LinkController}) " +
                       $"/* {label} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }
    }
}
