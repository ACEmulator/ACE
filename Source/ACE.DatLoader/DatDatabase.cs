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


        public Dictionary<uint, IUnpackable> FileCache { get; } = new Dictionary<uint, IUnpackable>();


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

                RootDirectory = new DatDirectory(Header.BTree, Header.BlockSize);
                RootDirectory.Read(stream);
            }

            RootDirectory.AddFilesToList(AllFiles);
        }

        /// <summary>
        /// This will try to find the object for the given fileId in local cache. If the object was not found, it will be read from the dat and cached.
        /// </summary>
        public T ReadFromDat<T>(uint fileId) where T : IUnpackable, new()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (FileCache.TryGetValue(fileId, out IUnpackable result))
                return (T)result;

            var datReader = GetReaderForFile(fileId);

            var obj = new T();

            if (datReader != null)
            {
                using (var memoryStream = new MemoryStream(datReader.Buffer))
                using (var reader = new BinaryReader(memoryStream))
                    obj.Unpack(reader);
            }

            // Store this object in the FileCache
            FileCache[fileId] = obj;

            return obj;
        }

        public DatReader GetReaderForFile(uint fileId)
        {
            if (AllFiles.TryGetValue(fileId, out var file))
            {
                DatReader dr = new DatReader(FilePath, file.FileOffset, file.FileSize, Header.BlockSize);
                return dr;                    
            }

            log.InfoFormat("Unable to find object_id {0} in {1}", fileId, Enum.GetName(typeof(DatDatabaseType), Header.DataSet));
            return null;
        }
    }
}
