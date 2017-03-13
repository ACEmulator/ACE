using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader
{
    /// <summary>
    /// File types inside .dat files.  These constants were extracted from the
    /// decompiled client
    /// </summary>
    public enum DatFileType : uint
    {
        [DatDatabaseType(DatDatabaseType.Cell)]
        LandBlock               = 1, // DB_TYPE_LANDBLOCK

        [DatDatabaseType(DatDatabaseType.Cell)]
        [DatFileTypeExtension("lbi")]
        LandBlockInfo           = 2, // DB_TYPE_LBI

        [DatDatabaseType(DatDatabaseType.Cell)]
        Cell                    = 3, // DB_TYPE_CELL

        /// <summary>
        /// usage of this is currently unknown.  exists in the client, but has no discernable
        /// source dat file.  appears to be a server file not distributed to clients.
        /// </summary>
        [DatFileTypeExtension("lbo")]
        LandBlockObjects        = 4, // DB_TYPE_LBO

        /// <summary>
        /// usage of this is currently unknown.  exists in the client, but has no discernable
        /// source dat file.  appears to be a server file not distributed to clients.
        /// </summary>
        [DatFileTypeExtension("ins")]
        Instantiation           = 5, // DB_TYPE_INSTANTIATION

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("obj")]
        [DatFileTypeIdRange(0x01000000, 0x0100FFFF)]
        GraphicsObject          = 6, // DB_TYPE_GFXOBJ

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("set")]
        [DatFileTypeIdRange(0x02000000, 0x0200FFFF)]
        Setup                   = 7, // DB_TYPE_SETUP
        
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("anm")]
        [DatFileTypeIdRange(0x03000000, 0x0300FFFF)]
        Animation               = 8, // DB_TYPE_ANIM

        /// <summary>
        /// usage of this is currently unknown.  exists in the client, but has no discernable
        /// source dat file.  appears to be a server file not distributed to clients.
        /// </summary>
        [DatFileTypeExtension("hk")]
        AnimationHook           = 9, // DB_TYPE_ANIMATION_HOOK

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("pal")]
        [DatFileTypeIdRange(0x04000000, 0x0400FFFF)]
        Palette                 = 10, // DB_TYPE_PALETTE

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("texture")]
        [DatFileTypeIdRange(0x05000000, 0x05FFFFFF)]
        SurfaceTexture          = 11, // DB_TYPE_SURFACETEXTURE

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("jpg")]
        [DatFileTypeExtension("dds")]
        [DatFileTypeExtension("tga")]
        [DatFileTypeExtension("iff")]
        [DatFileTypeExtension("256")]
        [DatFileTypeExtension("csi")]
        [DatFileTypeExtension("alp")]
        [DatFileTypeIdRange(0x06000000, 0x07FFFFFF)]
        RenderSurface           = 12, // DB_TYPE_RENDERSURFACE

        /// <summary>
        /// indexed in client as "materials" for some reason
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("surface")]
        [DatFileTypeIdRange(0x08000000, 0x0800FFFF)]
        Surface                 = 13, // DB_TYPE_SURFACE

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("dsc")]
        [DatFileTypeIdRange(0x09000000, 0x0900FFFF)]
        ModelTable              = 14, // DB_TYPE_MTABLE

        /// <summary>
        /// indexed as "sound" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("wav")]
        [DatFileTypeIdRange(0x0A000000, 0x0A00FFFF)]
        Wave                    = 15, // DB_TYPE_WAVE

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("env")]
        [DatFileTypeIdRange(0x0D000000, 0x0D00FFFF)]
        Environment             = 16, // DB_TYPE_ENVIRONMENT

        /// <summary>
        /// indexed as "ui" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("cps")]
        [DatFileTypeIdRange(0x0E000007, 0x0E000007)]
        ChatPoseTable           = 17, // DB_TYPE_CHAT_POSE_TABLE

        /// <summary>
        /// indexed as "DungeonCfgs" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("hrc")]
        [DatFileTypeIdRange(0x0E00000D, 0x0E00000D)]
        ObjectHierarchy         = 18, // DB_TYPE_OBJECT_HIERARCHY

        /// <summary>
        /// indexed as "weenie" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("bad")]
        [DatFileTypeIdRange(0x0E00001A, 0x0E00001A)]
        BadData                 = 19, // DB_TYPE_BADDATA

        /// <summary>
        /// indexed as "weenie" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("taboo")]
        [DatFileTypeIdRange(0x0E00001E, 0x0E00001E)]
        TabooTable              = 20, // DB_TYPE_TABOO_TABLE

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeIdRange(0x0E00001F, 0x0E00001F)]
        FileToId                = 21, // DB_TYPE_FILE2ID_TABLE

        /// <summary>
        /// indexed as "namefilter" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("nft")]
        [DatFileTypeIdRange(0x0E000020, 0x0E000020)]
        NameFilterTable         = 22, // DB_TYPE_NAME_FILTER_TABLE

        /// <summary>
        /// indexed as "properties" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("monprop")]
        [DatFileTypeIdRange(0x0E020000, 0x0E02FFFF)]
        MonitoredProperties     = 23, // DB_TYPE_MONITOREDPROPERTIES

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("pst")]
        [DatFileTypeIdRange(0x0F000000, 0x0F00FFFF)]
        PaletteSet              = 24, // DB_TYPE_PAL_SET

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("clo")]
        [DatFileTypeIdRange(0x10000000, 0x1000FFFF)]
        Clothing                = 25, // DB_TYPE_CLOTHING

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("deg")]
        [DatFileTypeIdRange(0x11000000, 0x1100FFFF)]
        DegradeInfo             = 26, // DB_TYPE_DEGRADEINFO

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("scn")]
        [DatFileTypeIdRange(0x12000000, 0x1200FFFF)]
        Scene                   = 27, // DB_TYPE_SCENE 

        /// <summary>
        /// indexed as "landscape" by the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("rgn")]
        [DatFileTypeIdRange(0x13000000, 0x1300FFFF)]
        Region                  = 28, // DB_TYPE_REGION

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("keymap")]
        [DatFileTypeIdRange(0x14000000, 0x1400FFFF)]
        KeyMap                  = 29, // DB_TYPE_KEYMAP

        /// <summary>
        /// indexed as "textures" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("rtexture")]
        [DatFileTypeIdRange(0x15000000, 0x15FFFFFF)]
        RenderTexture           = 30, // DB_TYPE_RENDERTEXTURE 

        /// <summary>
        /// indexed as "materials" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("mat")]
        [DatFileTypeIdRange(0x16000000, 0x16FFFFFF)]
        RenderMaterial          = 31, // DB_TYPE_RENDERMATERIAL 

        /// <summary>
        /// indexed as "materials" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("mm")]
        [DatFileTypeIdRange(0x17000000, 0x17FFFFFF)]
        MaterialModifier        = 32, // DB_TYPE_MATERIALMODIFIER 

        /// <summary>
        /// indexed as "materials" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("mi")]
        [DatFileTypeIdRange(0x18000000, 0x18FFFFFF)]
        MaterialInstance        = 33, // DB_TYPE_MATERIALINSTANCE

        /// <summary>
        /// suspected to be Submodel Table.  rename this if it can be confirmed.
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("stb")]
        [DatFileTypeIdRange(0x20000000, 0x2000FFFF)]
        STable                  = 34, // DB_TYPE_STABLE

        /// <summary>
        /// client pdb implies this was in a server-only file.
        /// </summary>
        [DatFileTypeExtension("uil")]
        [DatFileTypeIdRange(0x21000000, 0x21FFFFFF)]
        UiLayout                = 35, // DB_TYPE_UI_LAYOUT

        /// <summary>
        /// indexed as "emp" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("emp")]
        [DatFileTypeIdRange(0x22000000, 0x22FFFFFF)]
        EnumMapper              = 36, // DB_TYPE_ENUM_MAPPER

        /// <summary>
        /// client pdb implies this was in a server-only file.
        /// </summary>
        [DatFileTypeExtension("stt")]
        [DatFileTypeIdRange(0x23000000, 0x24FFFFFF)]
        StringTable             = 37, // DB_TYPE_STRING_TABLE 

        /// <summary>
        /// indexed as "emp/idmap" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("imp")]
        [DatFileTypeIdRange(0x25000000, 0x25FFFFFF)]
        DidMapper               = 38, // DB_TYPE_DID_MAPPER 

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("actionmap")]
        [DatFileTypeIdRange(0x26000000, 0x2600FFFF)]
        ActionMap               = 39, // DB_TYPE_ACTIONMAP 

        /// <summary>
        /// indexed as "emp/idmap" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("dimp")]
        [DatFileTypeIdRange(0x27000000, 0x27FFFFFF)]
        DualDidMapper           = 40, // DB_TYPE_DUAL_DID_MAPPER

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("str")]
        [DatFileTypeIdRange(0x31000000, 0x3100FFFF)]
        String                  = 41, // DB_TYPE_STRING

        /// <summary>
        /// inedexed as "emt" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("emt")]
        [DatFileTypeIdRange(0x32000000, 0x3200FFFF)]
        ParticleEmitter         = 42, // DB_TYPE_PARTICLE_EMITTER 

        /// <summary>
        /// inedexed as "pes" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("pes")]
        [DatFileTypeIdRange(0x33000000, 0x3300FFFF)]
        PhysicsScript           = 43, // DB_TYPE_PHYSICS_SCRIPT 

        /// <summary>
        /// inedexed as "pet" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("pet")]
        [DatFileTypeIdRange(0x34000000, 0x3400FFFF)]
        PhysicsScriptTable      = 44, // DB_TYPE_PHYSICS_SCRIPT_TABLE 

        /// <summary>
        /// inedexed as "emt/property" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("mpr")]
        [DatFileTypeIdRange(0x39000000, 0x39FFFFFF)]
        MasterProperty          = 45, // DB_TYPE_MASTER_PROPERTY 

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("font")]
        [DatFileTypeIdRange(0x40000000, 0x40000FFF)]
        Font                    = 46, // DB_TYPE_FONT 

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("font_local")]
        [DatFileTypeIdRange(0x40001000, 0x400FFFFF)]
        FontLocal               = 47, // DB_TYPE_FONT_LOCAL 

        /// <summary>
        /// client pdb implies this was in a server-only file.  also, indexed as
        /// "stringtable" in the client
        /// </summary>
        [DatFileTypeExtension("sti")]
        [DatFileTypeIdRange(0x41000000, 0x41FFFFFF)]
        StringState = 48, // DB_TYPE_STRING_STATE

        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("dbpc")]
        [DatFileTypeExtension("pmat")]
        [DatFileTypeIdRange(0x78000000, 0x7FFFFFFF)]
        DbProperties            = 49, // DB_TYPE_DBPROPERTIES

        /// <summary>
        /// indexed as "mesh" in the client
        /// </summary>
        [DatDatabaseType(DatDatabaseType.Portal)]
        [DatFileTypeExtension("rendermesh")]
        [DatFileTypeIdRange(0x19000000, 0x19FFFFFF)]
        RenderMesh              = 50, // DB_TYPE_RENDER_MESH
    }
}
