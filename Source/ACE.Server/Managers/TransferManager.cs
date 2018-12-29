using ACE.Common;
using ACE.Database;
using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Server.Network;
using ACE.Server.TransferServer;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ACE.Server.Managers
{
    /// <summary>
    /// ServerManager handles server transfers.
    /// </summary>
    public static class TransferManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static TransferServerWrapper Server = null;
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
                    log.Debug($"{deletionCount} stale transfer{plural} deleted.");
                }
            }

            Server = new TransferServerWrapper();
            string listeningHost = ConfigManager.Config.Server.Network.Host;
            int listeningPort = (int)ConfigManager.Config.Server.Network.Port + 2;
            log.Info($"Binding transfer server to {listeningHost}:{listeningPort}");
            try
            {
                Server.Listen(listeningHost, listeningPort);
            }
            catch (Exception exception)
            {
                log.FatalFormat("Transfer server has thrown: {0}", exception.Message);
            }
        }

        public static void Stop()
        {
            log.Info($"Shutting down transfer server.");
            Server.Dispose();
        }

        private static bool CookieIngredientsOK(string cookie)
        {
            foreach (char c in cookie)
            {
                if (!ThreadSafeRandom.CookieChars.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }

        public static string GetTransferFilePath(string cookie)
        {
            if (!CookieIngredientsOK(cookie))
            {
                return null;
            }
            string filePath = Path.Combine(ServerManager.TransferPath, cookie + ".zip");
            if (File.Exists(filePath))
            {
                return filePath;
            }
            else
            {
                return null;
            }
        }

        public static void DeleteTransfer(string cookie)
        {
            if (!CookieIngredientsOK(cookie))
            {
                return;
            }
            try
            {
                string filePath = Path.Combine(ServerManager.TransferPath, cookie + ".zip");
                File.Delete(filePath);
                // also delete character here?
            }
            catch (Exception ex)
            {
                log.Warn(ex);
            }
        }

        public static async Task<string> Import(Session session, params string[] parameters)
        {
            if (parameters.Length != 1)
            {
                return null;
            }

            string tmpFilePath = Path.GetTempFileName();
            DirectoryInfo diTmpDirPath = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(tmpFilePath), Path.GetFileNameWithoutExtension(tmpFilePath)));
            string signerCertPath = Path.Combine(diTmpDirPath.FullName, "signer.crt");

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(parameters[0], tmpFilePath);
            }
            using (ZipArchive zip = ZipFile.OpenRead(tmpFilePath))
            {
                zip.ExtractToDirectory(diTmpDirPath.FullName);
            }
            File.Delete(tmpFilePath);

            if (!File.Exists(signerCertPath))
            {
                Directory.Delete(diTmpDirPath.FullName, true);
                return null;
            }

            using (X509Certificate2 signer = new X509Certificate2(signerCertPath))
            {
                //verify that the signer is in the trusted signers list
                if (!ConfigManager.Config.Server.TrustedServerCertThumbprints.Any(k => k == signer.Thumbprint))
                {
                    Directory.Delete(diTmpDirPath.FullName, true);
                    session.WorldBroadcast($"The originating server isn't trusted.  Please contact the server operator and provide the thumbprint {signer.Thumbprint} so they can add it to the trusted server cert thumbprints list.");
                    log.Info($"Untrusted transfer attempted.  Thumbprint: {signer.Thumbprint}");
                    return null;
                }
                //verify that the signatures are valid
                FileInfo[] files = diTmpDirPath.GetFiles("*.json");
                foreach (FileInfo fil in files)
                {
                    if (!CryptoManager.VerifySignature(fil.FullName, signer))
                    {
                        Directory.Delete(diTmpDirPath.FullName, true);
                        return null;
                    }
                }
            }


            return "";
        }
        public static async Task<string> Export(Session session)
        {
            string cookie = ThreadSafeRandom.NextCookie(8);

            session.WorldBroadcast($"Preparing for character export.");
            // log.Info($"Character being exported. name: {session.Player.Name}");
            uint accountId = 1; // session.AccountId
            uint charId = 1342177281; // session.Player.Guid.Full
            CharacterSnapshot snapshot = new CharacterSnapshot() { };
            TaskCompletionSource<object> tsc = new TaskCompletionSource<object>();
            DatabaseManager.Shard.GetCharacters(accountId, false, new Action<List<Character>>(new Action<List<Character>>((chars) =>
            {
                snapshot.Character = chars.FirstOrDefault(k => k.Id == charId);
                snapshot.Player = session.Player.Biota;
                DatabaseManager.Shard.GetPossessedBiotasInParallel(snapshot.Character.Id, new Action<PossessedBiotas>((pb) =>
                {
                    snapshot.PossessedBiotas = pb;
                    tsc.SetResult(new object());
                }));
            })));
            await tsc.Task;

            string basePath = Path.Combine(ServerManager.TransferPath, cookie);
            Directory.CreateDirectory(basePath);

            // save and sign
            string playerPath = Path.Combine(basePath, snapshot.Player.Id + ".json");
            File.WriteAllText(playerPath, JsonConvert.SerializeObject(snapshot.Player, serializationSettings));
            CryptoManager.SignFile(playerPath);

            foreach (Biota biota in snapshot.PossessedBiotas.Inventory.Union(snapshot.PossessedBiotas.WieldedItems))
            {
                string biotaPath = Path.Combine(basePath, biota.Id + ".json");
                File.WriteAllText(biotaPath, JsonConvert.SerializeObject(biota, serializationSettings));
                CryptoManager.SignFile(biotaPath);
            }
            CryptoManager.ExportCert(Path.Combine(basePath, "signer.crt"));

            // compress
            string zipPath = Path.Combine(ServerManager.TransferPath, cookie + ".zip");
            ZipFile.CreateFromDirectory(basePath, zipPath);

            // delete temporary files
            Directory.Delete(basePath, true);

            return $"http://{ConfigManager.Config.Server.ExternalIPAddressOrDomainName}:{ConfigManager.Config.Server.Network.Port + 2}/?get={cookie}";
        }

        private static readonly JsonSerializerSettings serializationSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.None
        };


    }
    public class CharacterSnapshot
    {
        public CharacterSnapshot() { }
        public Character Character { get; set; }
        public Biota Player { get; set; }
        public PossessedBiotas PossessedBiotas { get; set; }
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
