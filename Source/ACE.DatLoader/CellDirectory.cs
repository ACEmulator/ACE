using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader
{
    public class CellDirectory
    {
        private uint rootSectorOffset;

        private uint fileCount = 0;
        
        public CellDirectory(uint rootSectorOffset, FileStream stream)
        {
            this.rootSectorOffset = rootSectorOffset;
            Read(stream);
        }
        
        public List<CellFile> Files { get; private set; } = new List<CellFile>();

        public List<CellDirectory> Directories { get; private set; } = new List<CellDirectory>();

        private void Read(FileStream stream)
        {
            byte[] sector = new byte[256];
            stream.Seek(this.rootSectorOffset, SeekOrigin.Begin);

            // read the first sector, which contains subdirectories and file count
            stream.Read(sector, 0, sector.Length);

            uint nextSector = BitConverter.ToUInt32(sector, 0);
            fileCount = BitConverter.ToUInt32(sector, 252);

            // directory is allowed to have files + 1 subdirectories
            int directories = 0;
            uint directory = BitConverter.ToUInt32(sector, sizeof(uint));
            List<uint> directoryList = new List<uint>();

            while (directories < (fileCount + 1) && directory > 0)
            {
                directoryList.Add(directory);
                directories++;
                directory = BitConverter.ToUInt32(sector, (1 + directories) * sizeof(uint));
            }

            // directories done, on to the other sectors for files

            int totalSectors = 1;
            int arrayUsage = 0;

            // maximum size we'll need
            sector = new byte[252 * 6];

            // separate buffer for the next sector
            byte[] nextSectorBuffer = new byte[4];

            while (nextSector > 0 && totalSectors < 7)
            {
                // go to this sector
                stream.Seek(nextSector, SeekOrigin.Begin);

                // read the next sector pointer
                stream.Read(nextSectorBuffer, 0, sizeof(uint));

                // read this sector
                stream.Read(sector, arrayUsage, 252);

                // maths for next loop
                arrayUsage += 252;
                nextSector = BitConverter.ToUInt32(nextSectorBuffer, 0);
                totalSectors++;
            }

            // sector has all the daters
            for (int i = 0; i < fileCount; i++)
            {
                Files.Add(CellFile.FromBuffer(sector, i * 6 * sizeof(uint)));
            }

            // files done, go back and iterate directories
            foreach (uint directoryOffset in directoryList)
            {
                Directories.Add(new CellDirectory(directoryOffset, stream));
            }
        }

        public void AddFilesToList(List<CellFile> files)
        {
            files.AddRange(this.Files);
            this.Directories.ForEach(d => d.AddFilesToList(files));
        }
    }
}
