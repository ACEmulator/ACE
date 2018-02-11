using System;
using System.IO;
using System.Linq;

namespace ACE.DatLoader
{
    public class DatFile : IUnpackable
    {
        internal static readonly uint ObjectSize = (sizeof(uint) * 6);

        
        public uint BitFlags { get; private set; }

        public uint ObjectId { get; private set; }

        public uint FileOffset { get; private set; }

        public uint FileSize { get; private set; }

        public uint Date { get; private set; }

        public uint Iteration { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            BitFlags    = reader.ReadUInt32();
            ObjectId    = reader.ReadUInt32();
            FileOffset  = reader.ReadUInt32();
            FileSize    = reader.ReadUInt32();
            Date        = reader.ReadUInt32();
            Iteration   = reader.ReadUInt32();
        }


        private DatFileType? fileType;

        public DatFileType? GetFileType(DatDatabaseType datDatabaseType)
        {
            if (fileType != null)
                return fileType.Value;

            var type = typeof(DatFileType);
            var enumTypes = Enum.GetValues(typeof(DatFileType)).Cast<DatFileType>().ToList();

            foreach (var enumType in enumTypes)
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
        }
    }
}
