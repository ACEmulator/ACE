using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// This is called PixelFormat in the client, but renaming it due to conflict with built in Enum PixelFormat.
    ///
    /// These are the different image formats that textures (RenderSurface) are stored in the dat files.
    /// While these are all defined, only a handful are actually used.
    /// </summary>
    public enum SurfacePixelFormat : uint
    {
        PFID_UNKNOWN = 0,
        PFID_R8G8B8 = 20,
        PFID_A8R8G8B8 = 21,
        PFID_X8R8G8B8 = 22,
        PFID_R5G6B5 = 23,
        PFID_X1R5G5B5 = 24,
        PFID_A1R5G5B5 = 25,
        PFID_A4R4G4B4 = 26,
        PFID_R3G3B2 = 27,
        PFID_A8 = 28,
        PFID_A8R3G3B2 = 29,
        PFID_X4R4G4B4 = 30,
        PFID_A2B10G10R10 = 31,
        PFID_A8B8G8R8 = 32,
        PFID_X8B8G8R8 = 33,
        PFID_A2R10G10B10 = 35,
        PFID_A8P8 = 40,
        PFID_P8 = 41,
        PFID_L8 = 50,
        PFID_A8L8 = 51,
        PFID_A4L4 = 52,
        PFID_V8U8 = 60,
        PFID_L6V5U5 = 61,
        PFID_X8L8V8U8 = 62,
        PFID_Q8W8V8U8 = 63,
        PFID_V16U16 = 64,
        PFID_A2W10V10U10 = 67,
        PFID_D16_LOCKABLE = 70,
        PFID_D32 = 71,
        PFID_D15S1 = 73,
        PFID_D24S8 = 75,
        PFID_D24X8 = 77,
        PFID_D24X4S4 = 79,
        PFID_D16 = 80,
        PFID_VERTEXDATA = 100,
        PFID_INDEX16 = 101,
        PFID_INDEX32 = 102,
        PFID_CUSTOM_R8G8B8A8 = 240,
        PFID_CUSTOM_FIRST = 240,
        PFID_CUSTOM_A8B8G8R8 = 241,
        PFID_CUSTOM_B8G8R8 = 242,
        PFID_CUSTOM_LSCAPE_R8G8B8 = 243,
        PFID_CUSTOM_LSCAPE_ALPHA = 244,
        PFID_CUSTOM_LAST = 500,
        PFID_CUSTOM_RAW_JPEG = 500,
        PFID_DXT1 = 827611204,
        PFID_DXT2 = 844388420,
        PFID_YUY2 = 844715353,
        PFID_DXT3 = 861165636,
        PFID_DXT4 = 877942852,
        PFID_DXT5 = 894720068,
        PFID_G8R8_G8B8 = 1111970375,
        PFID_R8G8_B8G8 = 1195525970,
        PFID_UYVY = 1498831189,
        PFID_INVALID = 2147483647,
    }
}
