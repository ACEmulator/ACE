namespace ACE.Server.Physics.Common
{
    public class RoadAlphaMap
    {
        public DatLoader.Entity.RoadAlphaMap _alphaMap;

        public uint RCode;
        public uint RoadTexGID;
        public ImgTex Texture;

        public static readonly RoadAlphaMap NULL;

        public RoadAlphaMap(DatLoader.Entity.RoadAlphaMap alphaMap)
        {
            _alphaMap = alphaMap;

            RCode = alphaMap.RCode;
            RoadTexGID = alphaMap.RoadTexGID;
        }
    }
}
