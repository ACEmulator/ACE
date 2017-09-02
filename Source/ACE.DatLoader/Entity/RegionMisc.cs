using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class RegionMisc
    {
        public uint Version { get; set; }
        public uint GameMapID { get; set; }
        public uint AutotestMapId { get; set; }
        public uint AutotestMapSize { get; set; }
        public uint ClearCellId { get; set; }
        public uint ClearMonsterId { get; set; }

        public static RegionMisc Read(DatReader datReader)
        {
            RegionMisc obj = new RegionMisc();

            obj.Version = datReader.ReadUInt32();
            obj.GameMapID = datReader.ReadUInt32();
            obj.AutotestMapId = datReader.ReadUInt32();
            obj.AutotestMapSize = datReader.ReadUInt32();
            obj.ClearCellId = datReader.ReadUInt32();
            obj.ClearMonsterId = datReader.ReadUInt32();

            return obj;
        }
    }
}
