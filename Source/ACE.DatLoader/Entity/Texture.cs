using System;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class Texture : IUnpackable
    {
        public int Id { get; set; }
        public int Unknown { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Format { get; set; }
        public D3DTextureFormat D3DFormat { get; set; } = D3DTextureFormat.D3DFMT_UNKNOWN;
        public int Length { get; set; }
        public byte[] Data { get; set; }
        public uint? DefaultPaletteId { get; set; }

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadInt32();
            Unknown = reader.ReadInt32();
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
            Format = reader.ReadInt32();
            D3DFormat = (D3DTextureFormat)Format;
            Length = reader.ReadInt32();
            Data = reader.ReadBytes(Length);
            DefaultPaletteId = (D3DFormat == D3DTextureFormat.D3DFMT_P8 || D3DFormat == D3DTextureFormat.D3DFMT_INDEX16) ? reader.ReadUInt32() : (uint?)null;

            if (reader.BaseStream.Length - reader.BaseStream.Position != 0)
                throw new Exception($"Texture file length exceeds expected length.");
        }
    }

    public enum D3DTextureFormat
    {
        D3DFMT_UNKNOWN = 0,
        D3DFMT_R8G8B8 = 20,
        D3DFMT_A8R8G8B8 = 21,
        D3DFMT_X8R8G8B8 = 22,
        D3DFMT_R5G6B5 = 23,
        D3DFMT_X1R5G5B5 = 24,
        D3DFMT_A1R5G5B5 = 25,
        D3DFMT_A4R4G4B4 = 26,
        D3DFMT_R3G3B2 = 27,
        D3DFMT_A8 = 28,
        D3DFMT_A8R3G3B2 = 29,
        D3DFMT_X4R4G4B4 = 30,
        D3DFMT_A2B10G10R10 = 31,
        D3DFMT_A8B8G8R8 = 32,
        D3DFMT_X8B8G8R8 = 33,
        D3DFMT_G16R16 = 34,
        D3DFMT_A2R10G10B10 = 35,
        D3DFMT_A16B16G16R16 = 36,
        D3DFMT_A8P8 = 40,
        D3DFMT_P8 = 41,
        D3DFMT_L8 = 50,
        D3DFMT_A8L8 = 51,
        D3DFMT_A4L4 = 52,
        D3DFMT_V8U8 = 60,
        D3DFMT_L6V5U5 = 61,
        D3DFMT_X8L8V8U8 = 62,
        D3DFMT_Q8W8V8U8 = 63,
        D3DFMT_V16U16 = 64,
        D3DFMT_A2W10V10U10 = 67,
        D3DFMT_UYVY = 1498831189,
        D3DFMT_R8G8_B8G8 = 1195525970,
        D3DFMT_YUY2 = 844715353,
        D3DFMT_G8R8_G8B8 = 1111970375,
        D3DFMT_DXT1 = 827611204,
        D3DFMT_DXT2 = 844388420,
        D3DFMT_DXT3 = 861165636,
        D3DFMT_DXT4 = 877942852,
        D3DFMT_DXT5 = 894720068,
        D3DFMT_D16_LOCKABLE = 70,
        D3DFMT_D32 = 71,
        D3DFMT_D15S1 = 73,
        D3DFMT_D24S8 = 75,
        D3DFMT_D24X8 = 77,
        D3DFMT_D24X4S4 = 79,
        D3DFMT_D16 = 80,
        D3DFMT_D32F_LOCKABLE = 82,
        D3DFMT_D24FS8 = 83,
        D3DFMT_D32_LOCKABLE = 84,
        D3DFMT_S8_LOCKABLE = 85,
        D3DFMT_L16 = 81,
        D3DFMT_VERTEXDATA = 100,
        D3DFMT_INDEX16 = 101,
        D3DFMT_INDEX32 = 102,
        D3DFMT_Q16W16V16U16 = 110,
        D3DFMT_MULTI2_ARGB8 = 827606349,
        D3DFMT_R16F = 111,
        D3DFMT_G16R16F = 112,
        D3DFMT_A16B16G16R16F = 113,
        D3DFMT_R32F = 114,
        D3DFMT_G32R32F = 115,
        D3DFMT_A32B32G32R32F = 116,
        D3DFMT_CxV8U8 = 117,
        D3DFMT_A1 = 118,
        D3DFMT_A2B10G10R10_XR_BIAS = 119,
        D3DFMT_BINARYBUFFER = 199,
        D3DFMT_FORCE_DWORD = 2147483647
    }
}
