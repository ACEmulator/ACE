using System;
using System.Collections.Generic;
using System.IO;

using log4net;

namespace ACE.DatLoader
{
    public class DatDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly uint DAT_HEADER_OFFSET = 0x140;


        public string FilePath { get; }


        public DatDatabaseHeader Header { get; } = new DatDatabaseHeader();

        public DatDirectory RootDirectory { get; }

        public Dictionary<uint, DatFile> AllFiles { get; } = new Dictionary<uint, DatFile>();

        // So we can cache the read files. The read methods in the FileTypes will handle the caching and casting.
        public Dictionary<uint, object> FileCache { get; } = new Dictionary<uint, object>();


        public DatDatabase(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            FilePath = filePath;

            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                stream.Seek(DAT_HEADER_OFFSET, SeekOrigin.Begin);
                using (var reader = new BinaryReader(stream, System.Text.Encoding.Default, true))
                    Header.Unpack(reader);

                RootDirectory = new DatDirectory(Header.BTree, Header.BlockSize, Header.DataSet, stream);
            }

            RootDirectory.AddFilesToList(AllFiles);
        }

        public DatReader GetReaderForFile(uint fileId)
        {
            if (AllFiles.ContainsKey(fileId))
            {
                DatReader dr = new DatReader(FilePath, AllFiles[fileId].FileOffset, AllFiles[fileId].FileSize, Header.BlockSize);
                return dr;                    
            }

            log.InfoFormat("Unable to find object_id {0} in {1}", fileId, Enum.GetName(typeof(DatDatabaseType), Header.DataSet));
            return null;
        }
    }
}
