using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader
{
    public class DatDirectory
    {
        private uint rootSectorOffset;

        private uint fileCount = 0;

        private int sectorSize;

        public DatDirectory(uint rootSectorOffset, int sectorSize, FileStream stream, DatDatabaseType type)
        {
            this.rootSectorOffset = rootSectorOffset;
            this.sectorSize = sectorSize;
            DatType = type;
            Read(stream);
        }

        public Dictionary<uint, DatFile> Files { get; private set; } = new Dictionary<uint, DatFile>();

        public List<DatDirectory> Directories { get; private set; } = new List<DatDirectory>();

        public DatDatabaseType DatType { get; private set; }

        private void Read(FileStream stream)
        {
            var sectorHeader = new byte[sectorSize];
            stream.Seek(rootSectorOffset, SeekOrigin.Begin);

            // read the first sector header, which contains subdirectories and file count
            stream.Read(sectorHeader, 0, sectorHeader.Length);

            var nextSector = BitConverter.ToUInt32(sectorHeader, 0);
            fileCount = BitConverter.ToUInt32(sectorHeader, 252);

            // directory is allowed to have files + 1 subdirectories
            var directories = 0;
            var directory = BitConverter.ToUInt32(sectorHeader, sizeof(uint));
            var directoryList = new List<uint>();

            while (directories < (fileCount + 1) && directory > 0)
            {
                directoryList.Add(directory);
                directories++;
                directory = BitConverter.ToUInt32(sectorHeader, (1 + directories) * sizeof(uint));
            }

            // directories done, on to files

            var totalSectors = 1;
            var arrayUsage = 0;

            // maximum size we'll need
            var sector = new byte[(sectorSize - 4) * 7];

            // copy over content from the first sector if it is bigger than just the header (happens in portal.dat, but not cell.dat)
            if (sectorSize > 256)
            {
                Array.Copy(sectorHeader, 256, sector, 0, sectorSize - 256);
                arrayUsage += (sectorSize - 256);
            }

            // separate buffer for the next sector location
            var nextSectorBuffer = new byte[4];

            while (nextSector > 0 && totalSectors < 6)
            {
                // go to this sector
                stream.Seek(nextSector, SeekOrigin.Begin);

                // read the next sector pointer
                stream.Read(nextSectorBuffer, 0, sizeof(uint));

                // read this sector
                stream.Read(sector, arrayUsage, (sectorSize - 4));

                // maths for next loop
                arrayUsage += (sectorSize - 4);
                nextSector = BitConverter.ToUInt32(nextSectorBuffer, 0);
                totalSectors++;
            }

            // sector has all the daters
            for (var i = 0; i < fileCount; i++)
            {
                var datfile = DatFile.FromBuffer(sector, i * 6 * sizeof(uint), DatType);
                if (!Files.ContainsKey(datfile.ObjectId))
                    Files.Add(datfile.ObjectId, datfile);
            }

            // files done, go back and iterate directories
            foreach (var directoryOffset in directoryList)
            {
                Directories.Add(new DatDirectory(directoryOffset, sectorSize, stream, DatType));
            }
        }

        public void AddFilesToList(Dictionary<uint, DatFile> dicFiles)
        {
            // files.Union(this.DictionaryFiles);
            foreach (var item in Files)
            {
                dicFiles[item.Key] = item.Value;
            }
            Directories.ForEach(d => d.AddFilesToList(dicFiles));
        }
    }
}
