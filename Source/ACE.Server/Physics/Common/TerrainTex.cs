namespace ACE.Server.Physics.Common
{
    public class TerrainTex
    {
        public DatLoader.Entity.TerrainTex _terrainTex;

        public uint TexGID;
        public ImgTex BaseTexture;
        public float MinSlope;
        public uint TexTiling;
        public uint MaxVertBright;
        public uint MinVertBright;
        public uint MaxVertSaturate;
        public uint MinVertSaturate;
        public uint MaxVertHue;
        public uint MinVertHue;
        public uint DetailTexTiling;
        public uint DetailTexGID;

        public static readonly TerrainTex NULL;

        public TerrainTex() { }

        public TerrainTex(DatLoader.Entity.TerrainTex terrainTex)
        {
            _terrainTex = terrainTex;

            TexGID = terrainTex.TexGID;
            TexTiling = terrainTex.TexTiling;
            MaxVertBright = terrainTex.MaxVertBright;
            MinVertBright = terrainTex.MinVertBright;
            MaxVertSaturate = terrainTex.MaxVertSaturate;
            MinVertSaturate = terrainTex.MinVertSaturate;
            MaxVertHue = terrainTex.MaxVertHue;
            MinVertHue = terrainTex.MinVertHue;
            DetailTexTiling = terrainTex.DetailTexTiling;
            DetailTexGID = terrainTex.DetailTexGID;
        }

        public bool InitEnd()
        {
            if (TexGID != 0)
                BaseTexture = new ImgTex(DBObj.GetSurfaceTexture(TexGID));

            return BaseTexture != null;
        }
    }
}
