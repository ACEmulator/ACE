using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            byte[] sectorHeader = new byte[this.sectorSize];
            stream.Seek(this.rootSectorOffset, SeekOrigin.Begin);

            // read the first sector header, which contains subdirectories and file count
            stream.Read(sectorHeader, 0, sectorHeader.Length);

            uint nextSector = BitConverter.ToUInt32(sectorHeader, 0);
            fileCount = BitConverter.ToUInt32(sectorHeader, 252);

            // directory is allowed to have files + 1 subdirectories
            int directories = 0;
            uint directory = BitConverter.ToUInt32(sectorHeader, sizeof(uint));
            List<uint> directoryList = new List<uint>();

            while (directories < (fileCount + 1) && directory > 0)
            {
                directoryList.Add(directory);
                directories++;
                directory = BitConverter.ToUInt32(sectorHeader, (1 + directories) * sizeof(uint));
            }

            // directories done, on to files

            int totalSectors = 1;
            int arrayUsage = 0;

            // maximum size we'll need
            byte[] sector = new byte[(this.sectorSize - 4) * 7];

            // copy over content from the first sector if it is bigger than just the header (happens in portal.dat, but not cell.dat)
            if (this.sectorSize > 256)
            {
                Array.Copy(sectorHeader, 256, sector, 0, this.sectorSize - 256);
                arrayUsage += (this.sectorSize - 256);
            }

            // separate buffer for the next sector location
            byte[] nextSectorBuffer = new byte[4];

            while (nextSector > 0 && totalSectors < 6)
            {
                // go to this sector
                stream.Seek(nextSector, SeekOrigin.Begin);

                // read the next sector pointer
                stream.Read(nextSectorBuffer, 0, sizeof(uint));

                // read this sector
                stream.Read(sector, arrayUsage, (this.sectorSize - 4));

                // maths for next loop
                arrayUsage += (this.sectorSize - 4);
                nextSector = BitConverter.ToUInt32(nextSectorBuffer, 0);
                totalSectors++;
            }

            // sector has all the daters
            for (int i = 0; i < fileCount; i++)
            {
                DatFile datfile = DatFile.FromBuffer(sector, i * 6 * sizeof(uint), DatType);
                if (!Files.ContainsKey(datfile.ObjectId))
                    Files.Add(datfile.ObjectId, datfile);
            }

            // files done, go back and iterate directories
            foreach (uint directoryOffset in directoryList)
            {
                Directories.Add(new DatDirectory(directoryOffset, this.sectorSize, stream, DatType));
            }
        }

        public void AddFilesToList(Dictionary<uint, DatFile> dicFiles)
        {
            // files.Union(this.DictionaryFiles);
            foreach (KeyValuePair<uint, DatFile> item in this.Files)
            {
                dicFiles[item.Key] = item.Value;
            }
            this.Directories.ForEach(d => d.AddFilesToList(dicFiles));
        }
    }
}
