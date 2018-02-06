using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader
{
    public class DatDirectory
    {
        private uint rootSectorOffset { get; }

        private uint blockSize { get; }


        public DatDirectoryHeader DatDirectoryHeader { get; } = new DatDirectoryHeader();

        public List<DatDirectory> Directories { get; } = new List<DatDirectory>();

        /// <summary>
        /// This is just "wrapper" around DatDirectoryHeader.Entries limited by DatDirectoryHeader.EntryCount
        /// </summary>
        public Dictionary<uint, DatFile> Files { get; } = new Dictionary<uint, DatFile>();


        public DatDirectory(uint rootSectorOffset, uint blockSize)
        {
            this.rootSectorOffset = rootSectorOffset;
            this.blockSize = blockSize;
        }

        public void Read(FileStream stream)
        {
            var headerReader = new DatReader(stream, rootSectorOffset, DatDirectoryHeader.ObjectSize, blockSize);

            using (var memoryStream = new MemoryStream(headerReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                DatDirectoryHeader.Unpack(reader);

            // directory is allowed to have files + 1 subdirectories
            if (DatDirectoryHeader.Branches[0] != 0)
            {
                for (int i = 0; i < DatDirectoryHeader.EntryCount + 1; i++)
                {
                    var directory = new DatDirectory(DatDirectoryHeader.Branches[i], blockSize);
                    directory.Read(stream);
                    Directories.Add(directory);
                }
            }

            for (int i = 0; i < DatDirectoryHeader.EntryCount; i++)
                Files.Add(DatDirectoryHeader.Entries[i].ObjectId, DatDirectoryHeader.Entries[i]);
        }

        public void AddFilesToList(Dictionary<uint, DatFile> dicFiles)
        {
            Directories.ForEach(d => d.AddFilesToList(dicFiles));

            foreach (KeyValuePair<uint, DatFile> item in Files)
                dicFiles[item.Key] = item.Value;
        }
    }
}
