using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader
{
    public class DatDatabase
    {
        public DatDirectory RootDirectory { get; private set; }

        public Dictionary<uint, DatFile> AllFiles { get; private set; }

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
    }
}
