using System;
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

        public Dictionary<uint, FileType> FileCache { get; } = new Dictionary<uint, FileType>();

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
        public T ReadFromDat<T>(uint fileId) where T : FileType, new()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (FileCache.TryGetValue(fileId, out FileType result))
                return (T)result;

            DatReader datReader = null;
            if (AllFiles.ContainsKey(fileId))
                datReader = GetReaderForFile(fileId);
            else
            {
                // This file doesn't exist in this dat... We will now check to see if it's applicable to check Language & Highres

                // If it's a PortalDatDatabase, we can check the others to see if it exists in there.
                // We will still store these in the PortalDat.FileCache so there is a single point to access all of these files
                if (GetType() == typeof(PortalDatDatabase))
                {
                    if (DatManager.HighResDat != null && DatManager.HighResDat.AllFiles.ContainsKey(fileId))
                        datReader = DatManager.HighResDat.GetReaderForFile(fileId);
                    else if (DatManager.LanguageDat != null && DatManager.LanguageDat.AllFiles.ContainsKey(fileId))
                        datReader = DatManager.LanguageDat.GetReaderForFile(fileId);
                }
            }
            //  We couldn't find a file with this fileId
            if (datReader == null)
                return null;

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

            if ((fileId & 0xFFFF) == 0xFFFE) // These are LandBlockInfo objects. Not every landblock has extra info (buildings, etc..)
                log.DebugFormat("Unable to find object_id {0:X8} in {1}", fileId, Enum.GetName(typeof(DatDatabaseType), Header.DataSet));
            else
                log.InfoFormat("Unable to find object_id {0:X8} in {1}", fileId, Enum.GetName(typeof(DatDatabaseType), Header.DataSet));

            return null;
        }
    }
}
