using System;
using System.Collections.Generic;
using System.IO;
using log4net;

namespace ACE.DatLoader
{
    public class DatDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DatDirectory RootDirectory { get; private set; }

        public Dictionary<uint, DatFile> AllFiles { get; private set; }

        // So we can cache the read files. The read methods in the FileTypes will handle the caching and casting.
        public Dictionary<uint, object> FileCache { get; private set; } = new Dictionary<uint, object>();

        public DatDatabaseType DatType { get; private set; }

        public string FilePath { get; private set; }

        public uint SectorSize { get; private set; }

        internal DatDatabase(string filePath, DatDatabaseType type)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            this.FilePath = filePath;
            DatType = type;

            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                byte[] sectorSizeBuffer = new byte[4];
                stream.Seek(0x144u, SeekOrigin.Begin);
                stream.Read(sectorSizeBuffer, 0, sizeof(uint));
                this.SectorSize = BitConverter.ToUInt32(sectorSizeBuffer, 0);

                stream.Seek(0x160u, SeekOrigin.Begin);
                byte[] firstDirBuffer = new byte[4];
                stream.Read(firstDirBuffer, 0, sizeof(uint));
                uint firstDirectoryOffset = BitConverter.ToUInt32(firstDirBuffer, 0);

                RootDirectory = new DatDirectory(firstDirectoryOffset, Convert.ToInt32(this.SectorSize), stream, DatType);
            }

            AllFiles = new Dictionary<uint, DatFile>();
            RootDirectory.AddFilesToList(AllFiles);
        }

        public DatReader GetReaderForFile(uint object_id)
        {
            if (AllFiles.ContainsKey(object_id))
            {
                DatReader dr = new DatReader(FilePath, AllFiles[object_id].FileOffset, AllFiles[object_id].FileSize, SectorSize);
                return dr;                    
            }
            else
            {
                log.InfoFormat("Unable to find object_id {0} in {1}", object_id.ToString(), Enum.GetName(typeof(DatDatabaseType), DatType));
                return null;
            }
        }
    }
}
