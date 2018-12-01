using ACE.Entity.Enum;

namespace ACE.Server.Physics.Common
{
    public class Surface
    {
        public uint Type;
        public SurfaceHandler Handler;
        public int ColorValue;
        public int SolidIndex;
        public uint IndexedTextureID;
        public ImgTex Base1Map;
        //public Palette Base1Pal;
        public float Translucency;
        public float Luminosity;
        public float Diffuse;
        public uint OrigTextureID;
        public uint OrigPaletteID;
        public float OrigLuminosity;
        public float OrigDiffuse;
        public TextureMergeInfo Info;

        public Surface()
        {
            // GraphicsResource
            ColorValue = -1;
            SolidIndex = -1;
            // dbobj / graphicsresource vtable (constructor?)
            IndexedTextureID = 0;   // stru_845060
            Diffuse = 1.0f;
            OrigTextureID = 0;   // stru_845060
            OrigDiffuse = 1.0f;
            // palette id?
            
            // SetAutoRestore()
        }

        public static Surface MakeCustomSurface(SurfaceHandler sh)
        {
            if (sh == SurfaceHandler.Database || sh == SurfaceHandler.Invalid)
                return null;

            var surface = new Surface();    // 0x90 / 144
            surface.Handler = sh;
            return surface;
        }

        public bool UseTextureMap(ImgTex texture, SurfaceHandler sh)
        {
            if (sh != Handler || texture == null)
                return false;

            Type = 2;
            Base1Map = texture;
            // vfptr->AddRef(texture)
            if (OrigTextureID == 0) // stru_845060
                OrigTextureID = Base1Map.ID;

            //Base1Pal = null;

            return true;
        }
    }
}
