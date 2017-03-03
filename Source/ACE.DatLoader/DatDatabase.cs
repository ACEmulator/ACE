using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader
{
    public class DatDatabase
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
                byte[] sectorSizeBuffer = new byte[4];
                stream.Seek(0x144u, SeekOrigin.Begin);
                stream.Read(sectorSizeBuffer, 0, sizeof(uint));
                uint sectorSize = BitConverter.ToUInt32(sectorSizeBuffer, 0);

                stream.Seek(0x160u, SeekOrigin.Begin);
                byte[] firstDirBuffer = new byte[4];
                stream.Read(firstDirBuffer, 0, sizeof(uint));
                uint firstDirectoryOffset = BitConverter.ToUInt32(firstDirBuffer, 0);

                RootDirectory = new DatDirectory(firstDirectoryOffset, Convert.ToInt32(sectorSize), stream);
            }

            AllFiles = new List<DatFile>();
            RootDirectory.AddFilesToList(AllFiles);
        }
    }
}
