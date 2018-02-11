using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// This is the client_portal.dat file starting with 0x13 -- There is only one of these, which is why REGION_ID is a constant.
    /// </summary>
    [DatFileType(DatFileType.Region)]
    public class RegionDesc : FileType
    {
        internal const uint FILE_ID = 0x13000000;

        public uint RegionNumber { get; private set; }
        public uint Version { get; private set; }
        public string RegionName { get; private set; }

        public LandDefs LandDefs { get; } = new LandDefs();
        public GameTime GameTime { get; } = new GameTime();

        public uint PartsMask { get; private set; }

        public SkyDesc SkyInfo { get; } = new SkyDesc();
        public SoundDesc SoundInfo { get; } = new SoundDesc();
        public SceneDesc SceneInfo { get; } = new SceneDesc();
        public TerrainDesc TerrainInfo { get; } = new TerrainDesc();
        public RegionMisc RegionMisc { get; } = new RegionMisc();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            RegionNumber    = reader.ReadUInt32();
            Version         = reader.ReadUInt32();
            RegionName      = reader.ReadPString(); // "Dereth"
            reader.AlignBoundary();

            LandDefs.Unpack(reader);
            GameTime.Unpack(reader);

            PartsMask = reader.ReadUInt32();

            if ((PartsMask & 0x10) != 0)
                SkyInfo.Unpack(reader);

            if ((PartsMask & 0x01) != 0)
                SoundInfo.Unpack(reader);

            if ((PartsMask & 0x02) != 0)
                SceneInfo.Unpack(reader);

            TerrainInfo.Unpack(reader);

            if ((PartsMask & 0x0200) != 0)
                RegionMisc.Unpack(reader);
        }
    }
}
