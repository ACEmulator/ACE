using ACE.Entity.Enum;

namespace ACE.Server.Physics.Common
{
    public class RenderSurface
    {
        public DatLoader.FileTypes.RenderSurface _renderSurface;

        public int Width;
        public int Height;
        public SurfacePixelFormat Format;
        public int Length;
        public byte[] Data;
        public uint? DefaultPaletteID;

        public RenderSurface()
        {
        }

        public RenderSurface(DatLoader.FileTypes.RenderSurface renderSurface)
        {
            _renderSurface = renderSurface;

            Width = renderSurface.Width;
            Height = renderSurface.Height;
            Format = renderSurface.Format;
            Length = renderSurface.Length;
            Data = renderSurface.SourceData;
            DefaultPaletteID = renderSurface.DefaultPaletteId;
        }

        // RenderSurfaceD3D.Create()
        public bool Create(uint width, uint height, SurfacePixelFormat format, bool localData)
        {
            Width = (int)width;
            Height = (int)height;
            Format = format;

            // get bits per pixel in PFDesc
            var bitsPerPixel = 32;
            Length = (int)(width * height * (bitsPerPixel / 8));
            Data = new byte[Length];
            return true;
        }
    }
}
