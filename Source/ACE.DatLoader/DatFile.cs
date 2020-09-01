using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACE.DatLoader
{
    public class DatFile : IUnpackable
    {
        internal static readonly uint ObjectSize = (sizeof(uint) * 6);

        
        //public uint BitFlags { get; private set; }

        public uint ObjectId { get; private set; }

        public uint FileOffset { get; private set; }

        public uint FileSize { get; private set; }

        //public uint Date { get; private set; }

        //public uint Iteration { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            /*BitFlags    =*/ reader.ReadUInt32();
            ObjectId    = reader.ReadUInt32();
            FileOffset  = reader.ReadUInt32();
            FileSize    = reader.ReadUInt32();
            /*Date        =*/ reader.ReadUInt32();
            /*Iteration   =*/ reader.ReadUInt32();
        }


        /*private DatFileType? fileType;

        private static List<DatFileType> EnumTypes = Enum.GetValues(typeof(DatFileType)).Cast<DatFileType>().ToList();

        public DatFileType? GetFileType(DatDatabaseType datDatabaseType)
        {
            if (fileType != null)
                return fileType.Value;

            var type = typeof(DatFileType);

            foreach (var enumType in EnumTypes)
            {
                var memInfo = type.GetMember(enumType.ToString());
                var datType = memInfo[0].GetCustomAttributes(typeof(DatDatabaseTypeAttribute), false).Cast<DatDatabaseTypeAttribute>().ToList();

                if (datType?.Count > 0 && datType[0].Type == datDatabaseType)
                {
                    // file type matches, now check id range
                    var idRange = memInfo[0].GetCustomAttributes(typeof(DatFileTypeIdRangeAttribute), false).Cast<DatFileTypeIdRangeAttribute>().ToList();
                    if (idRange?.Count > 0 && idRange[0].BeginRange <= ObjectId && idRange[0].EndRange >= ObjectId)
                    {
                        // id range matches
                        fileType = enumType;
                        break;
                    }
                }
            }

            return fileType;
        }*/

        public DatFileType? GetFileType(DatDatabaseType datDatabaseType)
        {
            if (datDatabaseType == DatDatabaseType.Cell)
            {
                if ((ObjectId & 0xFFFF) == 0xFFFF)
                    return DatFileType.LandBlock;

                if ((ObjectId & 0xFFFF) == 0xFFFE)
                    return DatFileType.LandBlockInfo;

                return DatFileType.EnvCell;
            }

            if (datDatabaseType == DatDatabaseType.Portal)
            {
                switch (ObjectId >> 24)
                {
                    case 0x01:
                        return DatFileType.GraphicsObject;
                    case 0x02:
                        return DatFileType.Setup;
                    case 0x03:
                        return DatFileType.Animation;
                    case 0x04:
                        return DatFileType.Palette;
                    case 0x05:
                        return DatFileType.SurfaceTexture;
                    case 0x06:
                        return DatFileType.Texture;
                    case 0x08:
                        return DatFileType.Surface;
                    case 0x09:
                        return DatFileType.MotionTable;
                    case 0x0A:
                        return DatFileType.Wave;
                    case 0x0D:
                        return DatFileType.Environment;
                    case 0x0F:
                        return DatFileType.PaletteSet;
                    case 0x10:
                        return DatFileType.Clothing;
                    case 0x11:
                        return DatFileType.DegradeInfo;
                    case 0x12:
                        return DatFileType.Scene;
                    case 0x13:
                        return DatFileType.Region;
                    case 0x14:
                        return DatFileType.KeyMap;
                    case 0x15:
                        return DatFileType.RenderTexture;
                    case 0x16:
                        return DatFileType.RenderMaterial;
                    case 0x17:
                        return DatFileType.MaterialModifier;
                    case 0x18:
                        return DatFileType.MaterialInstance;
                    case 0x20:
                        return DatFileType.SoundTable;
                    case 0x22:
                        return DatFileType.EnumMapper;
                    case 0x25:
                        return DatFileType.DidMapper;
                    case 0x26:
                        return DatFileType.ActionMap;
                    case 0x27:
                        return DatFileType.DualDidMapper;
                    case 0x30:
                        return DatFileType.CombatTable;
                    case 0x31:
                        return DatFileType.String;
                    case 0x32:
                        return DatFileType.ParticleEmitter;
                    case 0x33:
                        return DatFileType.PhysicsScript;
                    case 0x34:
                        return DatFileType.PhysicsScriptTable;
                    case 0x39:
                        return DatFileType.MasterProperty;
                    case 0x40:
                        return DatFileType.Font;
                    case 0x78:
                        return DatFileType.DbProperties;
                }

                switch (ObjectId >> 16)
                {
                    case 0x0E01:
                        return DatFileType.QualityFilter;
                    case 0x0E02:
                        return DatFileType.MonitoredProperties;
                }

                if (ObjectId == 0x0E000002)
                    return DatFileType.CharacterGenerator;
                if (ObjectId == 0x0E000003)
                    return DatFileType.SecondaryAttributeTable;
                if (ObjectId == 0x0E000004)
                    return DatFileType.SkillTable;
                if (ObjectId == 0x0E000007)
                    return DatFileType.ChatPoseTable;
                if (ObjectId == 0x0E00000D)
                    return DatFileType.ObjectHierarchy;
                if (ObjectId == 0x0E00000E)
                    return DatFileType.SpellTable;
                if (ObjectId == 0x0E00000F)
                    return DatFileType.SpellComponentTable;
                if (ObjectId == 0x0E000018)
                    return DatFileType.XpTable;
                if (ObjectId == 0xE00001A)
                    return DatFileType.BadData;
                if (ObjectId == 0x0E00001D)
                    return DatFileType.ContractTable;
                if (ObjectId == 0x0E00001E)
                    return DatFileType.TabooTable;
                if (ObjectId == 0x0E00001F)
                    return DatFileType.FileToId;
                if (ObjectId == 0x0E000020)
                    return DatFileType.NameFilterTable;
            }

            if (datDatabaseType == DatDatabaseType.Language)
            {
                switch (ObjectId >> 24)
                {
                    case 0x21:
                        return DatFileType.UiLayout;
                    case 0x23:
                        return DatFileType.StringTable;
                    case 0x41:
                        return DatFileType.StringState;
                }
            }

            Console.WriteLine($"Unknown file type: {ObjectId:X8}");
            return null;
        }
    }
}
