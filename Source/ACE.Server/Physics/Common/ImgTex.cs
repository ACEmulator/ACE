using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;

namespace ACE.Server.Physics.Common
{
    public class ImgTex
    {
        public uint ID;
        public RenderSurface ImageData;
        public Palette Palette;
        public int Pitch;
        public uint TextureCode;
        public bool IsLocked;

        public Texture _texture;
        public SurfaceTexture _surfaceTexture;

        public static ImageScaleType LandTextureScale;
        public static ImageScaleType ClipTextureScale;
        public static ImageScaleType RGBATextureScale;
        public static ImageScaleType IndexedTextureScale;
        public static ImageScaleType CurrentTextureScale;

        public static HashSet<ImgTex> CustomTextureTable;

        public static uint MinTexSize;

        static ImgTex()
        {
            CustomTextureTable = new HashSet<ImgTex>();
        }

        public ImgTex()
        {

        }

        public ImgTex(Texture texture)
        {
            _texture = texture;
        }

        public ImgTex(SurfaceTexture surfaceTexture)
        {
            _surfaceTexture = surfaceTexture;

            if (surfaceTexture.Textures == null || surfaceTexture.Textures.Count < 1)
            {
                Console.WriteLine($"ImgTex({surfaceTexture.Id:X8}): no textures");
                return;
            }

            ID = _surfaceTexture.Id;
            var textureID = TextureCode = surfaceTexture.Textures[0];   // use texturecode here?
            //Console.WriteLine($"Loading texture {textureID:X8}");
            var renderSurface = DatManager.PortalDat.ReadFromDat<Texture>(textureID);
            ImageData = new RenderSurface(renderSurface);
        }

        public static ImgTex CreateLScapeTexture(byte[] rawData, uint i_width, uint i_height)
        {
            var texture = new ImgTex();     // 0x88 / 136
            CurrentTextureScale = ImageScaleType.Full;
            if (!texture.LoadCSI(rawData, i_width, i_height))
                return null;

            CustomTextureTable.Add(texture);
            return texture;
        }

        public bool LoadCSI(byte[] csi_data, uint csi_width, uint csi_height)
        {
            var surface = new RenderSurface();  // RenderDevice.CreateSurface
            surface.Create(csi_width, csi_height, SurfacePixelFormat.PFID_X8R8G8B8, true);
            ImageData = surface;

            var tempBuffer = GetTempBuffer(csi_width, csi_height, SurfacePixelFormat.PFID_X8R8G8B8);
            //var data = tempBuffer.GetData();
            tempBuffer.CSI2TGA(csi_data, csi_width, csi_height, ImageData.Data, csi_width, csi_height, tempBuffer.Pitch);

            Load(ImageData.Data, csi_width, tempBuffer);  // loads onto gpu?
            return true;
        }

        public bool CSI2TGA(byte[] csi_data, uint csi_width, uint csi_height, byte[] tga_data, uint tga_width, uint tga_height, int tga_pitch)
        {
            if (csi_width != tga_width || csi_height != tga_height)
                return false;

            // qmemcpy(dst, src, size)
            // qmemcpy(tga_data, csi_data, (4 * tga_height * tga_width) / 2)
            Array.Copy(csi_data, tga_data, csi_width * csi_height * 4);

            return true;
        }

        public static void CopyCSI(byte[] data, uint height, uint width, ImgTex csi_tex, uint tiling)
        {
            if (csi_tex != null)
            {
                TileCSI(data, height, width, csi_tex, tiling);
            }
            else if (width * height != 0)
            {
                //memset32(data, 65280, width * height);    // FF00
                data = new byte[width * height * 4];
                for (var i = 0; i < width * height * 4; i += 4)
                    data[i] = 0xFF;
            }
        }

        public static void TileCSI(byte[] data, uint dest_height, uint dest_width, ImgTex csi_tex, uint tiling)
        {
            var src_data = csi_tex.ImageData.Data;

            for (var i = 0; i < tiling * 2; i++)
                Array.Copy(src_data, 0, data, src_data.Length * i, src_data.Length);
        }

        public static void MergeTexture(byte[] data, uint dest_height, uint dest_width, ImgTex csi_merge_tex, uint tiling, ImgTex alphaMap, LandDefs.Rotation rot)
        {
            var imageData = csi_merge_tex.ImageData;

            var src_height = imageData != null ? imageData.Height : 0;
            var src_width = imageData != null ? imageData.Width : 0;

            var alphaData = alphaMap.ImageData;
            var alpha_width = alphaData != null ? alphaData.Width : 0;
            var alpha_height = alphaData != null ? alphaData.Height : 0;

            var alpha_scale = (float)dest_width / alpha_width;

            var pixelIdx = 0;
            var stepX = 0;
            var stepY = 0;

            switch (rot)
            {
                case LandDefs.Rotation.Rot0:
                    pixelIdx = 0;
                    stepX = 1;
                    stepY = src_width;
                    break;
                case LandDefs.Rotation.Rot90:
                    pixelIdx = src_width - 1;
                    stepX = src_width;
                    stepY = -1;
                    break;
                case LandDefs.Rotation.Rot180:
                    pixelIdx = src_width * src_height - 1;
                    stepX = -1;
                    stepY = -src_width;
                    break;
                case LandDefs.Rotation.Rot270:
                    pixelIdx = src_width * (src_height - 1);
                    stepX = -src_width;
                    stepY = -1;
                    break;
            }

            // alphaStep handles upscaling / downscaling
            var alphaStep = 1.0f / alpha_scale;

            // copy csi_merge_tex into dest buffer (data)
            Array.Copy(imageData.Data, data, imageData.Data.Length);

            // start walking the alpha map
            for (var _alphaY = 0.0f; _alphaY < alpha_height; _alphaY += alphaStep)
            {
                var alphaY = (int)Math.Round(_alphaY);

                for (var _alphaX = 0.0f; _alphaX < alpha_width; _alphaX += alphaStep)
                {
                    // TODO: handle rotation
                    var alphaX = (int)Math.Round(_alphaX);

                    var idx = (alphaY * alpha_width + alphaX) * 4;
                    var a = alphaData.Data[idx];
                    data[idx] = a;
                }
            }
        }

        public static ImgTex GetTempBuffer(uint _width, uint _height, SurfacePixelFormat _image_type)
        {
            // bunch of dictionary stuff
            return AllocateTempBuffer(_width, _height, _image_type);
        }

        public static ImgTex AllocateTempBuffer(uint _width, uint _height, SurfacePixelFormat _image_type)
        {
            var tempBuffer = new ImgTex();  // 0x88 / 136
            tempBuffer.ImageData = new RenderSurface();
            tempBuffer.ImageData.Create(_width, _height, _image_type, true);
            CustomTextureTable.Add(tempBuffer);
            return tempBuffer;
        }

        public byte[] GetData()
        {
            return ImageData.Data;
        }

        public bool Load(byte[] data, uint width, ImgTex texture)
        {
            // get pixel format and width/height of this and texture
            // if they are all equal, call CreateD3DTexture()
            return false;
        }

        public bool CreateD3DTexture(byte[] data, uint width, ImgTex texture)
        {
            return true;
        }
    }
}
