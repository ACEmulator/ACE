namespace ACE.Entity.Enum
{
    public enum SurfaceType : uint
    {
        Base1Solid = 1,
        Base1Image = 2,
        Base1ClipMap = 4,
        Translucent = 16,
        Diffuse = 32,
        Luminous = 64,
        Alpha = 256,
        InvAlpha = 512,
        Additive = 0x10000,
        Detail = 0x20000,
        Gouraud = 0x10000000,
        Stippled = 0x40000000,
        Perspective = 0x80000000
    }
}
