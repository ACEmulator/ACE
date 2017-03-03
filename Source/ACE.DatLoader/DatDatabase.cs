using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader
{
    public abstract class DatDatabase
    {
        public DatDirectory RootDirectory { get; private set; }

        public List<DatFile> AllFiles { get; private set; }

        public DatDatabase(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                stream.Seek(0x160u, SeekOrigin.Begin);
                byte[] firstDirBuffer = new byte[4];
                stream.Read(firstDirBuffer, 0, sizeof(uint));
                uint firstDirectoryOffset = BitConverter.ToUInt32(firstDirBuffer, 0);

                RootDirectory = new DatDirectory(firstDirectoryOffset, this.SectorSize, stream);
            }

            AllFiles = new List<DatFile>();
            RootDirectory.AddFilesToList(AllFiles);
        }

        public abstract int SectorSize { get; }
    }
}
