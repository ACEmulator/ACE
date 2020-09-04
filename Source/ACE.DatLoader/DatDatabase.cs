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

        public int Iteration { get { return GetIteration(); } }

        private FileStream stream { get; }

        private static readonly object streamMutex = new object();

        public DatDatabaseHeader Header { get; } = new DatDatabaseHeader();

        public DatDirectory RootDirectory { get; }

        public Dictionary<uint, DatFile> AllFiles { get; } = new Dictionary<uint, DatFile>();

        public ConcurrentDictionary<uint, FileType> FileCache { get; } = new ConcurrentDictionary<uint, FileType>();

        public DatDatabase(string filePath, bool keepOpen = false)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            FilePath = filePath;

            stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            stream.Seek(DAT_HEADER_OFFSET, SeekOrigin.Begin);
            using (var reader = new BinaryReader(stream, System.Text.Encoding.Default, true))
                Header.Unpack(reader);

            RootDirectory = new DatDirectory(Header.BTree, Header.BlockSize);
            RootDirectory.Read(stream);

            if (!keepOpen)
            {
                stream.Close();
                stream.Dispose();
                stream = null;
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
                DatReader dr;

                if (stream != null)
                {
                    lock (streamMutex)
                        dr = new DatReader(stream, file.FileOffset, file.FileSize, Header.BlockSize);
                }
                else
                    dr = new DatReader(FilePath, file.FileOffset, file.FileSize, Header.BlockSize);

                return dr;                    
            }

            if ((fileId & 0xFFFF) == 0xFFFE) // These are LandBlockInfo objects. Not every landblock has extra info (buildings, etc..)
                log.DebugFormat("Unable to find object_id {0:X8} in {1}", fileId, Enum.GetName(typeof(DatDatabaseType), Header.DataSet));
            else
                log.InfoFormat("Unable to find object_id {0:X8} in {1}", fileId, Enum.GetName(typeof(DatDatabaseType), Header.DataSet));

            return null;
        }

        public void ExtractCategorizedPortalContents(string path)
        {
            foreach (KeyValuePair<uint, DatFile> entry in AllFiles)
            {
                string thisFolder;

                if (entry.Value.GetFileType(DatDatabaseType.Portal) != null)
                    thisFolder = Path.Combine(path, entry.Value.GetFileType(DatDatabaseType.Portal).ToString());
                else
                    thisFolder = Path.Combine(path, "UnknownType");

                if (!Directory.Exists(thisFolder))
                    Directory.CreateDirectory(thisFolder);

                string hex = entry.Value.ObjectId.ToString("X8");
                string thisFile = Path.Combine(thisFolder, hex + ".bin");

                // Use the DatReader to get the file data
                DatReader dr = GetReaderForFile(entry.Value.ObjectId);

                File.WriteAllBytes(thisFile, dr.Buffer);
            }
        }

        /// <summary>
        /// Gets the latest iteration from the dat file (basically, it functions like internal versioning system)
        ///
        /// Per InterrogationResponse generate by the client:
        /// PORTAL.DAT is 2072
        /// client_English.dat (idDatFile.Type = 1, idDatFile.Id = 3) = 994
        /// Cell.dat is 982
        ///
        /// For final end-of-retail dat files, this returns:
        /// Cell: 982
        /// portal: 2072
        /// highres: 497
        /// Language: 994
        /// </summary>
        /// <returns>The iteration from the dat file, or 0 if there was an error</returns>
        private int GetIteration()
        {
            var iteration = ReadFromDat<Iteration>(FileTypes.Iteration.FILE_ID);
            if (iteration.Ints.Count > 0)
                return iteration.Ints[0];
            else
            {
                log.Error($"Unable to read iteration from {FilePath}");
                return 0;
            }
        }
    }
}
