using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACE.DatLoader
{
    public class DatFile
    {
        private DatFileType? fileType = null;

        /// <summary>
        /// private to force loading from the static method
        /// </summary>
        private DatFile()
        {

        }

        public uint BitFlags { get; private set; }

        public uint ObjectId { get; private set; }

        public uint FileOffset { get; private set; }

        public uint FileSize { get; private set; }

        public uint Date { get; private set; }

        public uint Iteration { get; private set; }

        public DatDatabaseType DatType {get; private set;}

        /// <summary>
        /// populates a new CellFile from the specified buffer.
        /// </summary>
        public static DatFile FromBuffer(byte[] buffer, int offset, DatDatabaseType type)
        {
            DatFile cf = new DatFile()
            {
                BitFlags = BitConverter.ToUInt32(buffer, offset),
                ObjectId = BitConverter.ToUInt32(buffer, offset + 4),
                FileOffset = BitConverter.ToUInt32(buffer, offset + 8) + 4,
                FileSize = BitConverter.ToUInt32(buffer, offset + 12),
                Date = BitConverter.ToUInt32(buffer, offset + 16),
                Iteration = BitConverter.ToUInt32(buffer, offset + 20),
                DatType = type
            };

            return cf;
        }

        public DatFileType? GetFileType()
        {
            if (fileType != null)
                return fileType.Value;

            var type = typeof(DatFileType);
            var enumTypes = Enum.GetValues(typeof(DatFileType)).Cast<DatFileType>().ToList();

            foreach(var fileType in enumTypes)
            {
                var memInfo = type.GetMember(fileType.ToString());
                var datType = memInfo[0].GetCustomAttributes(typeof(DatDatabaseTypeAttribute), false).Cast<DatDatabaseTypeAttribute>().ToList();

                if (datType?.Count > 0 && datType[0].Type == this.DatType)
                {
                    // file type matches, now check id range
                    var idRange = memInfo[0].GetCustomAttributes(typeof(DatFileTypeIdRangeAttribute), false).Cast<DatFileTypeIdRangeAttribute>().ToList();
                    if (idRange?.Count > 0 && idRange[0].BeginRange <= this.ObjectId && idRange[0].EndRange >= this.ObjectId)
                    {
                        // id range matches
                        this.fileType = fileType;
                        break;
                    }
                }
            }

            return fileType;
        }

        /// <summary>
        /// uses the open stream to read content as a binary blob.
        /// </summary>
        public byte[] GetContent(FileStream stream)
        {
            stream.Seek(this.FileOffset, SeekOrigin.Begin);
            byte[] content = new byte[FileSize];
            stream.Read(content, 0, Convert.ToInt32(FileSize));
            return content;
        }
    }
}
