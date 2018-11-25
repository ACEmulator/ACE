namespace ACE.Server.Physics.Common
{
    public class TerrainAlphaMap
    {
        public DatLoader.Entity.TerrainAlphaMap _alphaMap;

        public uint TCode;
        public uint TexGID;
        public ImgTex Texture;

        public static readonly TerrainAlphaMap NULL;

        public TerrainAlphaMap() { }

        public TerrainAlphaMap(DatLoader.Entity.TerrainAlphaMap alphaMap)
        {
            _alphaMap = alphaMap;

            TCode = alphaMap.TCode;
            TexGID = alphaMap.TexGID;
        }
    }
}
