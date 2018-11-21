namespace ACE.Server.Physics.Common
{
    public class TMTerrainDesc
    {
        public DatLoader.Entity.TMTerrainDesc _terrainDesc;

        public LandDefs.TerrainType TerrainType;
        public TerrainTex TerrainTex;

        public TMTerrainDesc(DatLoader.Entity.TMTerrainDesc terrainDesc)
        {
            _terrainDesc = terrainDesc;

            TerrainType = (LandDefs.TerrainType)terrainDesc.TerrainType;
            TerrainTex = new TerrainTex(_terrainDesc.TerrainTex);
        }
    }
}
