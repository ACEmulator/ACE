using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Server.Command.Handlers.Processors;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ACE.Server.Managers
{
    /// <summary>
    /// ServerManager handles server transfers properly.
    /// </summary>
    /// <remarks>
    ///   Possibly useful for:
    ///     1. Character transfers
    ///   Known issue:
    ///     1. Uses only self-signed certificates.
    /// </remarks>
    public static class TransferManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Initialize()
        {
            DirectoryInfo di = new DirectoryInfo(ServerManager.TransferPath);
            FileInfo[] files = di.GetFiles();
            int fileCount = files.Count();
            if (fileCount > 0)
            {
                string plural = (fileCount > 1 || fileCount == 0) ? "s" : "";
                log.Info($"{fileCount} pending transfer{plural}.");
                int deletionCount = 0;
                List<FileInfo> stales = files.Where(k => DateTime.Now - k.CreationTime > TimeSpan.FromDays(1)).ToList();
                stales.ForEach(file =>
                {
                    File.Delete(file.FullName);
                    deletionCount++;
                });
                if (deletionCount > 0)
                {
                    plural = (fileCount > 1 || fileCount == 0) ? "s" : "";
                    log.Info($"{deletionCount} stale transfer{plural} deleted.");
                }
            }
        }
        public static async Task<string> PrepareTransfer(CharacterSnapshot character)
        {
            string snapshotSerialized = JsonConvert.SerializeObject(character, serializationSettings);

            var cookie = ThreadSafeRandom.NextCookie(8);

            var filename = Path.Combine(ServerManager.TransferPath, cookie + ".json");

            File.WriteAllText(filename, snapshotSerialized);
            return cookie;
        }
        private static readonly JsonSerializerSettings serializationSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            Formatting = Formatting.None,
            PreserveReferencesHandling = PreserveReferencesHandling.All
        };
    }
    public class CharacterSnapshot
    {
        public CharacterSnapshot() { }
        public Character Character { get; set; }
        public PossessedBiotas PossessedBiotas { get; set; }
        public List<Biota> PlayerBiotas { get; set; }
    }
    public class CharacterSnapshotWrapper
    {
        public CharacterSnapshot CharacterSnapshot { get; set; }
        public Signature Signature { get; set; }
    }
    public class Signature
    {
        public byte[] Snapshot { get; set; }
    }
}
