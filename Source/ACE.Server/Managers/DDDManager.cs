using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ACE.Common;
using ACE.DatLoader;
using ACE.Server.Network;
using ACE.Server.Network.Structure;

using log4net;

namespace ACE.Server.Managers
{
    public static class DDDManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool Debug = false;

        public static Dictionary<DatDatabaseType, Dictionary<uint, ConcurrentBag<uint>>> Iterations;

        public static Dictionary<DatDatabaseType, ConcurrentDictionary<uint, (int UncompressedFileSize, int CompressedFileSize)>> DatFileSizes;

        public static Dictionary<DatDatabaseType, ConcurrentDictionary<uint, byte[]>> CompressedDatFilesCache;

        /// <summary>
        /// The rate at which DDDManager.Tick() executes
        /// </summary>
        //private static readonly RateLimiter dddDataQueueRateLimiter = new RateLimiter(1000, TimeSpan.FromMinutes(1));

        //private static readonly Queue<(Session Session, uint DatFileId, DatDatabaseType DatDatabaseType)> dddDataQueue = new();

        static DDDManager()
        {
            Iterations = new();

            DatFileSizes = new();

            CompressedDatFilesCache = new();
        }

        public static void Initialize()
        {
            InitIterations(DatDatabaseType.Portal, DatManager.PortalDat);
            InitIterations(DatDatabaseType.Cell, DatManager.CellDat);
            InitIterations(DatDatabaseType.Language, DatManager.LanguageDat);

            log.DebugFormat("DDDManager Initialized.");
        }

        private static void InitIterations(DatDatabaseType datDatabaseType, DatDatabase datDatabase)
        {
            Iterations.TryAdd(datDatabaseType, new());
            DatFileSizes.TryAdd(datDatabaseType, new());
            CompressedDatFilesCache.TryAdd(datDatabaseType, new());

            for (var i = 1; i <= datDatabase.Iteration; i++)
                Iterations[datDatabaseType].TryAdd((uint)i, new());

            var precacheCompressedDATFiles = ConfigManager.Config.DDD.PrecacheCompressedDATFiles;

            var fileCount = 0;
            Parallel.ForEach(datDatabase.AllFiles, file =>
            {
                var fileName = file.Value.ObjectId;
                var fileIter = file.Value.Iteration;

                Iterations[datDatabaseType].TryAdd(fileIter, new());

                var datFile = datDatabase.GetReaderForFile(fileName);
                var uncompressedFileSize = datFile.Buffer.Length;
                var compressedDatFile = Compress(datFile.Buffer);
                var compressedFileSize = compressedDatFile.Length;
                var useCompressedFile = compressedFileSize + 4 < uncompressedFileSize;
                var fileSizeToSend = useCompressedFile ? compressedFileSize : 0;

                Iterations[datDatabaseType][fileIter].Add(fileName);
                Interlocked.Increment(ref fileCount);
                DatFileSizes[datDatabaseType].TryAdd(fileName, (uncompressedFileSize, fileSizeToSend));

                if (useCompressedFile && precacheCompressedDATFiles)
                    CompressedDatFilesCache[datDatabaseType].TryAdd(fileName, PrependUncompressedFileSize(compressedDatFile, (uint)uncompressedFileSize));
            });
            log.Info($"Iterations for {datDatabaseType} initialized. Iterations.Count={Iterations[datDatabaseType].Count} | FileCount={fileCount} | DatFileSizes.Count={DatFileSizes[datDatabaseType].Count}{(precacheCompressedDATFiles ? " | precached Compressed files" : "")}");
        }

        /// <summary>
        /// Compresses data with ZLib
        /// </summary>
        /// <param name="data">The data to compress</param>
        /// <returns>The ZLib compressed data</returns>
        public static byte[] Compress(byte[] data)
        {
            using (var input = new MemoryStream(data))
            using (var output = new MemoryStream())
            using (var compressor = new ZLibStream(output, CompressionLevel.SmallestSize))
            {
                input.CopyTo(compressor);
                compressor.Close();
                return output.ToArray();
            }
        }

        /// <summary>
        /// Combines the uncompressed file size with the compressed data for transmission to client
        /// </summary>
        /// <param name="compressedData">Data that has been previously compressed</param>
        /// <param name="uncompressedFileSize">The file size of the compressed data when uncompressed</param>
        /// <returns></returns>
        public static byte[] PrependUncompressedFileSize(byte[] compressedData, uint uncompressedFileSize)
        {
            var uncompressedFileSizeBytes = BitConverter.GetBytes(uncompressedFileSize);
            var output = uncompressedFileSizeBytes.Concat(compressedData).ToArray();

            return output;
        }

        /// <summary>
        /// Tries to search for and return the file contents for a DatFileId of the specified DAT file, suitable for transmission to client
        /// </summary>
        /// <param name="datFileId">The id of the file to transmit</param>
        /// <param name="datDatabaseType">The type of DAT to search for <paramref name="datFileId"/></param>
        /// <param name="datFile">The DatFile found in <paramref name="datDatabaseType"/></param>
        /// <param name="isCompressed">Indicates if the data has been ZLib compressed for transmission</param>
        /// <returns>If found, the data contents for <paramref name="datFileId"/> prepared for transmission, the <paramref name="datFile"/> and if the contents <paramref name="isCompressed"/>. null is returned if the file is not found.</returns>
        public static byte[] TryGetDatFileContentsForTransmission(uint datFileId, DatDatabaseType datDatabaseType, out DatFile datFile, out bool isCompressed)
        {
            DatDatabase datDatabase = null;
            isCompressed = false;

            switch (datDatabaseType)
            {
                case DatDatabaseType.Portal:

                    datDatabase = DatManager.PortalDat;
                    break;

                case DatDatabaseType.Cell:

                    datDatabase = DatManager.CellDat;
                    break;

                case DatDatabaseType.Language:

                    datDatabase = DatManager.LanguageDat;
                    break;
            }

            var datFileFound = datDatabase.AllFiles.TryGetValue(datFileId, out datFile);

            if (!datFileFound)
                return null;

            var cachedDatFileSizes = DatFileSizes[datDatabaseType][datFileId];

            var compressDatFile = cachedDatFileSizes.CompressedFileSize > 0;

            if (compressDatFile)
            {
                isCompressed = true;

                if (CompressedDatFilesCache[datDatabaseType].TryGetValue(datFileId, out var compressedData))
                    return compressedData;

                compressedData = PrependUncompressedFileSize(Compress(datDatabase.GetReaderForFile(datFileId).Buffer), datFile.FileSize);

                CompressedDatFilesCache[datDatabaseType].TryAdd(datFileId, compressedData);

                return compressedData;
            }
            else
            {
                return datDatabase.GetReaderForFile(datFileId).Buffer;
            }
        }

        public static uint GetMissingIterations(CMostlyConsecutiveIntSet clientPortalDatIntSet, CMostlyConsecutiveIntSet clientCellDatIntSet, CMostlyConsecutiveIntSet clientLanguageDatIntSet, out uint totalFileSize, out Dictionary<DatDatabaseType, Dictionary<uint, List<uint>>> iterations)
        {
            uint totalMissingIterations = 0;
            totalFileSize = 0;
            iterations = new();

            GetMissingIterations(DatDatabaseType.Portal, clientPortalDatIntSet, ref totalFileSize, iterations, ref totalMissingIterations);
            GetMissingIterations(DatDatabaseType.Cell, clientCellDatIntSet, ref totalFileSize, iterations, ref totalMissingIterations);
            GetMissingIterations(DatDatabaseType.Language, clientLanguageDatIntSet, ref totalFileSize, iterations, ref totalMissingIterations);

            return totalMissingIterations;
        }

        private static void GetMissingIterations(DatDatabaseType datDatabaseType, CMostlyConsecutiveIntSet clientDatIterations, ref uint totalFileSize, Dictionary<DatDatabaseType, Dictionary<uint, List<uint>>> iterations, ref uint totalMissingIterations)
        {
            if (!Iterations.ContainsKey(datDatabaseType))
                return;

            // Generate a list of the missing iterations the client may have
            var missingIterations = GetMissingIterationsFromClient(datDatabaseType, clientDatIterations);

            // If the list is empty, we all good here!
            if (missingIterations.Count == 0)
                return;

            // Get all the files for the iterations we are missing
            var x = Iterations[datDatabaseType].OrderBy(i => i.Key).Where(i => missingIterations.Contains(i.Key));

            if (x.Any())
            {
                var compressedFiles = 0;
                var uncompressedFiles = 0;
                iterations.TryAdd(datDatabaseType, new());
                foreach (var y in x)
                {
                    totalMissingIterations++;
                    iterations[datDatabaseType].TryAdd(y.Key, new());
                    foreach (var z in y.Value.OrderBy(z => z))
                    {
                        iterations[datDatabaseType][y.Key].Add(z);

                        if (datDatabaseType != DatDatabaseType.Cell)
                        {
                            if (DatFileSizes[datDatabaseType][z].CompressedFileSize > 0)
                            {
                                compressedFiles++;
                                totalFileSize += (uint)DatFileSizes[datDatabaseType][z].CompressedFileSize;
                            }
                            else
                            {
                                uncompressedFiles++;
                                totalFileSize += (uint)DatFileSizes[datDatabaseType][z].UncompressedFileSize;
                            }
                        }
                        else
                        {
                            // do nothing, files from Cell DAT are not included in totalFileSize calculations because these files are requested/sent on demand and not part of initial patching.
                        }
                    }
                }
                totalFileSize += (uint)compressedFiles * 4;
                //totalFileSize += (uint)uncompressedFiles * 4;
            }
        }

        /// <summary>
        /// Helper function to parse an Iteration list received from the client on login and return any missing iterations it may have
        /// </summary>
        /// <param name="datDatabaseType">The type of DAT we are looking for missing iterations from</param>
        /// <param name="clientIterations">The CMostlyConsecutiveIntSet from the client for a single DAT</param>
        /// <returns>List of missing iterations</returns>
        private static List<uint> GetMissingIterationsFromClient(DatDatabaseType datDatabaseType, CMostlyConsecutiveIntSet clientIterations)
        {
            var allIterations = new HashSet<uint>();

            var totalIterations = Iterations[datDatabaseType].Keys.Max(); // Highest key will be the total iterations. We already know the datDatabaseType exists from an earlier check.

            // Store all the possible Iterations here
            for (uint i = 1; i <= totalIterations; i++)
                allIterations.Add(i);

            var dbFile = "";
            switch (datDatabaseType)
            {
                case DatDatabaseType.Portal:
                    dbFile = "client_portal.dat";
                    break;
                case DatDatabaseType.Cell:
                    dbFile = "client_cell_1.dat";
                    break;
                case DatDatabaseType.Language:
                    dbFile = "client_Local_English.dat";
                    break;
            }
            var debugStr = dbFile + Environment.NewLine + "Completed Iterations:" + Environment.NewLine;

            // Going to remove all the Iterations the client has
            var iterations = 0;
            var rangeStart = 0;
            var rangeEnd = 0;
            foreach (var ints in clientIterations.Ints)
            {
                if (Math.Abs(ints) == totalIterations)
                    return new();

                if (ints < 0)
                {
                    rangeEnd = ints;
                    continue;
                }

                if (ints > 0 && rangeStart == 0 && rangeEnd < 0)
                {
                    rangeStart = ints;
                    //continue;
                }

                if (rangeStart != 0 && rangeEnd != 0)
                {
                    rangeEnd = rangeStart + Math.Abs(rangeEnd);
                    for (var i = (uint)rangeStart; i < rangeEnd; i++)
                    {
                        if (allIterations.Remove(i))
                        {
                            iterations++;
                        }
                    }
                    if (Debug)
                    {
                        debugStr += $"  |    {rangeStart:####} - {rangeEnd - 1:####}" + Environment.NewLine;
                    }
                    rangeStart = 0;
                    rangeEnd = 0;
                    continue;
                }

                if (allIterations.Remove((uint)ints))
                {
                    iterations++;

                    if (Debug)
                        //debugStr += $"  |    {ints:####} - {ints:####}" + Environment.NewLine;
                        debugStr += $"  |    {ints:####}" + Environment.NewLine;
                }
            }

            if (Debug)
                Console.WriteLine(debugStr + Environment.NewLine + $"Total Completed Iterations: {iterations} of {totalIterations} ({(double)iterations / totalIterations:P2})" + Environment.NewLine);

            // The missing Iterations are what is left
            var missing = allIterations.ToList();

            if (Debug)
            {
                debugStr = dbFile + Environment.NewLine + "Missing Iterations:" + Environment.NewLine;
                debugStr += allIterations.ToArray().Ranges().Show();
                Console.WriteLine(debugStr + Environment.NewLine + Environment.NewLine + $"Total Missing Iterations: {allIterations.Count} of {totalIterations} ({(double)allIterations.Count / totalIterations:P2})" + Environment.NewLine);
            }

            return missing;
        }

        public static IEnumerable<(uint begin, uint end)> Ranges(this IEnumerable<uint> nums)
        {
            var e = nums.GetEnumerator();
            if (e.MoveNext())
            {
                var begin = e.Current;
                var end = begin + 1;
                while (e.MoveNext())
                {
                    if (e.Current != end)
                    {
                        yield return (begin, end);
                        begin = end = e.Current;
                    }
                    end++;
                }
                yield return (begin, end);
            }
        }

        public static string Show(this IEnumerable<(uint begin, uint end)> ranges)
        {
            //return "[" + string.Join(",", ranges.Select(r => r.end - r.begin == 1 ? $"{r.begin}" : $"{r.begin}-{r.end - 1}")) + "]";
            return string.Join(Environment.NewLine, ranges.Select(r => r.end - r.begin == 1 ? $"  |    {r.begin}" : $"  |    {r.begin:####} - {r.end - 1:####}"));
        }

        //public static void Tick()
        //{
        //    if (dddDataQueueRateLimiter.GetSecondsToWaitBeforeNextEvent() > 0)
        //        return;

        //    //dddDataQueueRateLimiter.RegisterEvent();

        //    while (dddDataQueue.Count > 0 && dddDataQueueRateLimiter.GetSecondsToWaitBeforeNextEvent() <= 0)
        //    {
        //        var x = dddDataQueue.TryDequeue(out var y);

        //        if (x)
        //        {
        //            y.Session.Network.EnqueueSend(new GameMessageDDDDataMessage(y.DatFileId, y.DatDatabaseType));
        //            dddDataQueueRateLimiter.RegisterEvent();
        //        }
        //    }
        //}

        public static bool AddToQueue(Session session, uint datFileId, DatDatabaseType datDatabaseType)
        {
            //dddDataQueue.Enqueue((session, datFileId, datDatabaseType));
            //session.AddToDDDQueue(datFileId, datDatabaseType);

            return session.AddToDDDQueue(datFileId, datDatabaseType);
        }
    }
}
