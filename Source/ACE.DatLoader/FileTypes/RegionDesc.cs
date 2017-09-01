using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.FileTypes
{
    public class RegionDesc
    {
        private const uint REGION_ID = 0x13000000;

        public uint FileId { get; set; }
        public uint BLoaded { get; set; }
        public uint TimeStamp { get; set; }
        public string RegionName { get; set; }
        public uint PartsMask { get; set; }
        public LandDefs LandDefs { get; set; }
        public GameTime GameTime { get; set; }
        public uint PNext { get; set; }
        public SkyDesc SkyInfo { get; set; }
        public SoundDesc SoundInfo { get; set; }
        public SceneDesc SceneInfo { get; set; }
        public TerrainDesc TerrainInfo { get; set; }
        public RegionMisc RegionMisc { get; set; }

        public static RegionDesc ReadFromDat()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(REGION_ID))
            {
                return (RegionDesc)DatManager.PortalDat.FileCache[REGION_ID];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(REGION_ID);
                RegionDesc region = new RegionDesc();

                region.FileId = datReader.ReadUInt32();
                region.BLoaded = datReader.ReadUInt32();
                region.TimeStamp = datReader.ReadUInt32();
                region.RegionName = datReader.ReadPString(); // "Dereth"
                datReader.AlignBoundary();
                region.PartsMask = datReader.ReadUInt32();

                // There are 7 x 4 byte entries here that are "unknown". We will just skip them.
                datReader.Offset += (7 * 4);

                region.LandDefs = LandDefs.Read(datReader);
                region.GameTime = GameTime.Read(datReader);

                region.PNext = datReader.ReadUInt32();

                if ((region.PNext & 0x10) > 0)
                    region.SkyInfo = SkyDesc.Read(datReader);

                if ((region.PNext & 0x01) > 0)
                    region.SoundInfo = SoundDesc.Read(datReader);

                if ((region.PNext & 0x02) > 0)
                    region.SceneInfo = SceneDesc.Read(datReader);

                region.TerrainInfo = TerrainDesc.Read(datReader);

                if ((region.PNext & 0x0200) > 0)
                    region.RegionMisc = RegionMisc.Read(datReader);

                DatManager.PortalDat.FileCache[REGION_ID] = region;
                return region;
            }
        }
    }
}
