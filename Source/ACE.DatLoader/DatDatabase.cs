using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

using log4net;

using ACE.DatLoader.FileTypes;

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

        public ConcurrentDictionary<uint, FileType> FileCache { get; } = new ConcurrentDictionary<uint, FileType>();

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
        /// This will try to find the object for the given fileId in local cache. If the object was not found, it will be read from the dat and cached.<para />
        /// This function is thread safe.
        /// </summary>
        public T ReadFromDat<T>(uint fileId) where T : FileType, new()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (FileCache.TryGetValue(fileId, out FileType result))
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
            obj = (T)FileCache.GetOrAdd(fileId, obj);

            return obj;
        }

        public DatReader GetReaderForFile(uint fileId)
        {
            if (AllFiles.TryGetValue(fileId, out var file))
            {
                DatReader dr = new DatReader(FilePath, file.FileOffset, file.FileSize, Header.BlockSize);
                return dr;                    
            }

            if ((fileId & 0xFFFF) == 0xFFFE) // These are LandBlockInfo objects. Not every landblock has extra info (buildings, etc..)
                log.DebugFormat("Unable to find object_id {0:X8} in {1}", fileId, Enum.GetName(typeof(DatDatabaseType), Header.DataSet));
            else
                log.InfoFormat("Unable to find object_id {0:X8} in {1}", fileId, Enum.GetName(typeof(DatDatabaseType), Header.DataSet));

            return null;
        }
    }
}
