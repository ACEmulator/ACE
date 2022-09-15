using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

using ACE.Common.Performance;
using ACE.DatLoader;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;

using log4net;

namespace ACE.Server.Managers
{
    public static class DDDManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool Debug = false;

        public static Dictionary<DatDatabaseType, Dictionary<uint, List<uint>>> Iterations;

        public static Dictionary<DatDatabaseType, ConcurrentDictionary<uint, (int UncompressedFileSize, int CompressedFileSize)>> DatFileSizes;

        public static Dictionary<DatDatabaseType, ConcurrentDictionary<uint, byte[]>> CompressedDatFilesCache;

        /// <summary>
        /// The rate at which DDDManager.Tick() executes
        /// </summary>
        private static readonly RateLimiter dddDataQueueRateLimiter = new RateLimiter(1000, TimeSpan.FromMinutes(1));

        private static readonly Queue<(Session Session, uint DatFileId, DatDatabaseType DatDatabaseType)> dddDataQueue = new();

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

            Parallel.ForEach(datDatabase.AllFiles, file =>
            {
                var fileName = file.Value.ObjectId;
                var fileIter = file.Value.Iteration;

                Iterations[datDatabaseType].TryAdd(fileIter, new());

                var datFile = datDatabase.GetReaderForFile(fileName);
                var uncompressedFileSize = datFile.Buffer.Length;
                //var compressedDatFile = Ionic.Zlib.ZlibStream.CompressBuffer(datFile.Buffer);
                var compressedDatFile = Compress(datFile.Buffer);
                var compressedFileSize = compressedDatFile.Length;
                var useCompressedFile = compressedFileSize + 4 < uncompressedFileSize;
                var fileSizeToSend = useCompressedFile ? compressedFileSize : 0;

                Iterations[datDatabaseType][fileIter].Add(fileName);
                DatFileSizes[datDatabaseType].TryAdd(fileName, (uncompressedFileSize, fileSizeToSend));

                //if (useCompressedFile)
                //    CompressedDatFilesCache[datDatabaseType].TryAdd(fileName, compressedDatFile);
            });
        }

        public static byte[] Compress(byte[] data)
        {
            using (var input = new MemoryStream(data))
            using (var output = new MemoryStream())
            using (var compressor = new ZLibStream(output, CompressionLevel.SmallestSize))
            //using (var compressor = new Ionic.Zlib.ZlibStream(output, Ionic.Zlib.CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestCompression))
            {
                input.CopyTo(compressor);
                compressor.Close();
                return output.ToArray();
            }
        }

        public static uint GetMissingIterations(int clientPortalDatIteration, int clientCellDatIteration, int clientLanguageDatIteration, out uint totalFileSize, out Dictionary<DatDatabaseType, Dictionary<uint, List<uint>>> iterations)
        {
            uint totalMissingIterations = 0;
            totalFileSize = 0;
            iterations = new();

            GetMissingIterations(DatDatabaseType.Portal, clientPortalDatIteration, ref totalFileSize, iterations, ref totalMissingIterations);
            GetMissingIterations(DatDatabaseType.Cell, clientCellDatIteration, ref totalFileSize, iterations, ref totalMissingIterations);
            GetMissingIterations(DatDatabaseType.Language, clientLanguageDatIteration, ref totalFileSize, iterations, ref totalMissingIterations);

            return totalMissingIterations;
        }

        private static void GetMissingIterations(DatDatabaseType datDatabaseType, int clientDatIteration, ref uint totalFileSize, Dictionary<DatDatabaseType, Dictionary<uint, List<uint>>> iterations, ref uint totalMissingIterations)
        {
            if (!Iterations.ContainsKey(datDatabaseType))
                return;

            var x = Iterations[datDatabaseType].OrderBy(i => i.Key).SkipWhile(i => i.Key <= clientDatIteration);

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

                        }
                    }
                }
                totalFileSize += (uint)compressedFiles * 4;
                //totalFileSize += (uint)uncompressedFiles * 4;
            }
        }

        public static void Tick()
        {
            if (dddDataQueueRateLimiter.GetSecondsToWaitBeforeNextEvent() > 0)
                return;

            //dddDataQueueRateLimiter.RegisterEvent();

            while (dddDataQueue.Count > 0 && dddDataQueueRateLimiter.GetSecondsToWaitBeforeNextEvent() <= 0)
            {
                var x = dddDataQueue.TryDequeue(out var y);

                if (x)
                {
                    y.Session.Network.EnqueueSend(new GameMessageDDDDataMessage(y.DatFileId, y.DatDatabaseType));
                    dddDataQueueRateLimiter.RegisterEvent();
                }
            }
        }

        public static bool AddToQueue(Session session, uint datFileId, DatDatabaseType datDatabaseType)
        {
            dddDataQueue.Enqueue((session, datFileId, datDatabaseType));

            return true;
        }
    }
}
