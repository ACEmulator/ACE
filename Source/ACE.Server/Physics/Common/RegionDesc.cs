using ACE.DatLoader.Entity;

namespace ACE.Server.Physics.Common
{
    public class RegionDesc
    {
        public uint RegionNum;
        public string Name;
        public int Version;
        public bool MinimizePal;
        public uint PartsMask;
        //public FileNameDesc FileInfo;
        public SkyDesc SkyInfo;
        public SoundDesc SoundInfo;
        public SceneDesc SceneInfo;
        public TerrainDesc TerrainInfo;
        //public EncounterDesc EncounterInfo;
        //public WaterDesc WaterInfo;
        //public FogDesc FogInfo;
        //public DistanceFogDesc DistFogInfo;
        //public RegionMapDesc RegionMapInfo;
        public RegionMisc RegionMisc;

        public RegionDesc(uint gid)
        {
            Version = -1;
            MinimizePal = true;
        }
    }
}
