using ACE.Common;
using ACE.Database;
using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network;
using ACE.Server.WorldObjects;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Server.Managers
{
    /// <summary>
    /// ServerManager handles server transfers.
    /// </summary>
    public static class TransferManager
    {
        public const int CookieLength = 8;
        public const string CookieChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly JsonSerializerSettings serializationSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.None
        };

        public static void Initialize()
        {
            DirectoryInfo di = new DirectoryInfo(ServerManager.TransferPath);
            FileInfo[] files = di.GetFiles();
            int fileCount = files.Count();
            if (fileCount > 0)
            {
                string plural = (fileCount > 1 || fileCount == 0) ? "s" : "";
                log.Info($"{fileCount} pending migration{plural}.");
                int deletionCount = 0;
                List<FileInfo> stales = files.Where(k => DateTime.Now - k.CreationTime > TimeSpan.FromDays(1)).ToList();
                stales.ForEach(file =>
                {
                    File.Delete(file.FullName);
                    deletionCount++;
                });
                //if (deletionCount > 0)
                //{
                //    plural = (fileCount > 1 || fileCount == 0) ? "s" : "";
                //    log.Debug($"{deletionCount} stale transfer{plural} deleted.");
                //}
            }
        }

        /// <summary>
        /// Checks for the existence of a snapshot package file
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns>null if non-existent, or if existent the path to the transfer package file</returns>
        public static string GetTransferPackageFilePath(string cookie)
        {
            if (CookieContainsInvalidChars(cookie))
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

        /// <summary>
        /// delete an existent snapshot package file
        /// </summary>
        /// <param name="cookie"></param>
        public static void DeleteTransferPackageFile(string cookie)
        {
            if (CookieContainsInvalidChars(cookie))
            {
                return;
            }
            try
            {
                string filePath = Path.Combine(ServerManager.TransferPath, cookie + ".zip");
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                log.Warn(ex);
            }
        }

        public class TransferManagerCharacterMigrationDownloadResponseModel
        {
            public bool Success { get; set; }
            public string Cookie { get; set; }
            public byte[] SnapshotPackage { get; set; }
        }

        public class TransferManagerTransferConfigResponseModel
        {
            public string MyThumbprint { get; set; }
            public List<string> AllowMigrationFrom { get; set; }
            public List<string> AllowImportFrom { get; set; }
            public bool AllowBackup { get; set; }
            public bool AllowImport { get; set; }
            public bool AllowMigrate { get; set; }
        }

        /// <summary>
        /// Import or migrate a character.
        /// </summary>
        /// <returns>the result</returns>
        public static ImportAndMigrateResult ImportAndMigrate(PackageMetadata metadata, byte[] importBytes = null)
        {
            if ((metadata.PackageType == PackageType.Backup && !ConfigManager.Config.Transfer.AllowImport) || (metadata.PackageType == PackageType.Migrate && !ConfigManager.Config.Transfer.AllowMigrate))
            {
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.OperationNotAllowed };
            }
            bool NameIsGood = false;
            ManualResetEvent mre = new ManualResetEvent(false);
            DatabaseManager.Shard.IsCharacterNameAvailable(metadata.NewCharacterName, isAvailable =>
            {
                NameIsGood = isAvailable;
                mre.Set();
            });
            mre.WaitOne();
            if (!NameIsGood)
            {
                // TO-DO: prevent abuse (use to make list of taken names)
                // TO-DO: implement taboo-table lookup and other char name validation
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.NameIsUnavailable };
            }
            TransferManagerCharacterMigrationDownloadResponseModel snapshotPack = null;
            CharacterTransfer xfer = null;
            if (importBytes == null)
            {
                mre = new ManualResetEvent(false);
                DatabaseManager.Shard.GetCharacterTransfers((xfers) =>
                {
                    xfer = xfers.FirstOrDefault(k => k.Cookie == metadata.Cookie);
                    mre.Set();
                });
                mre.WaitOne();
                if (xfer != null)
                {
                    // don't fail here, prevents inter-account same-server transfers
                    // return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.CookieAlreadyUsed };
                }

                // try to verify we trust the server before downloading package, since migrations cause source server to delete the original char upon download of the package
                string unverifiedThumbprintsSerialized = null;
                try
                {
                    using (InsecureWebClient iwc = new InsecureWebClient())
                    {
                        unverifiedThumbprintsSerialized = iwc.DownloadString(metadata.ImportUrl.ToString() + "api/transferConfig");
                    }
                }
                catch
                {
                    return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.CannotContactSourceServer };
                }
                TransferManagerTransferConfigResponseModel unverifiedThumbprints = null;
                try
                {
                    unverifiedThumbprints = JsonConvert.DeserializeObject<TransferManagerTransferConfigResponseModel>(unverifiedThumbprintsSerialized, serializationSettings);
                }
                catch
                {
                    return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.ProtocolError };
                }

                if ((metadata.PackageType == PackageType.Migrate && !ConfigManager.Config.Transfer.AllowMigrationFrom.Any(k => k == unverifiedThumbprints.MyThumbprint)) || (metadata.PackageType == PackageType.Backup && !ConfigManager.Config.Transfer.AllowImportFrom.Any(k => k == unverifiedThumbprints.MyThumbprint)))
                {
                    return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.UnverifiedSourceServerNotAllowed };
                }

                try
                {
                    using (InsecureWebClient iwc = new InsecureWebClient())
                    {
                        string TransferManagerCharacterMigrationDownloadResponseModelSerialized = iwc.DownloadString(metadata.ImportUrl.ToString() + $"api/character/migrationDownload?Cookie={metadata.Cookie}");
                        snapshotPack = JsonConvert.DeserializeObject<TransferManagerCharacterMigrationDownloadResponseModel>(TransferManagerCharacterMigrationDownloadResponseModelSerialized, serializationSettings);
                    }
                }
                catch (Exception)
                {
                    return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.ProtocolError };
                }
                if (snapshotPack == null)
                {
                    return new ImportAndMigrateResult();
                }
                if (!snapshotPack.Success)
                {
                    return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.SourceServerRejectedRequest };
                }
            }

            string tmpFilePath = Path.GetTempFileName();
            DirectoryInfo diTmpDirPath = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(tmpFilePath), Path.GetFileNameWithoutExtension(tmpFilePath)));
            string signerCertPath = Path.Combine(diTmpDirPath.FullName, "signer.crt");

            if (importBytes == null)
            {
                File.WriteAllBytes(tmpFilePath, snapshotPack.SnapshotPackage);
            }
            else
            {
                File.WriteAllBytes(tmpFilePath, importBytes);
            }

            using (ZipArchive zip = ZipFile.OpenRead(tmpFilePath))
            {
                zip.ExtractToDirectory(diTmpDirPath.FullName);
            }
            File.Delete(tmpFilePath);

            if (!File.Exists(signerCertPath))
            {
                Directory.Delete(diTmpDirPath.FullName, true);
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.PackageUnsigned };
            }

            string verifiedSourceThumbprint = string.Empty;

            using (X509Certificate2 signer = new X509Certificate2(signerCertPath))
            {
                verifiedSourceThumbprint = signer.Thumbprint;
                // verify that the signer is in the trusted signers list
                if ((metadata.PackageType == PackageType.Migrate && !ConfigManager.Config.Transfer.AllowMigrationFrom.Any(k => k == verifiedSourceThumbprint)) || (metadata.PackageType == PackageType.Backup && !ConfigManager.Config.Transfer.AllowImportFrom.Any(k => k == verifiedSourceThumbprint)))
                {
                    Directory.Delete(diTmpDirPath.FullName, true);
                    return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.VerifiedSourceServerNotAllowed };
                }
                // verify that the signatures are valid
                foreach (FileInfo fil in diTmpDirPath.GetFiles("*.json"))
                {
                    if (!CryptoManager.VerifySignature(fil.FullName, signer))
                    {
                        Directory.Delete(diTmpDirPath.FullName, true);
                        return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.Forgery };
                    }
                }
            }

            // deserialize
            PackageMetadata packInfo = null;
            List<Biota> snapshot = new List<Biota>();
            foreach (FileInfo fil in diTmpDirPath.GetFiles("*.json"))
            {
                if (fil.Name == "packinfo.json")
                {
                    packInfo = JsonConvert.DeserializeObject<PackageMetadata>(File.ReadAllText(fil.FullName), serializationSettings);
                }
                else
                {
                    snapshot.Add(JsonConvert.DeserializeObject<Biota>(File.ReadAllText(fil.FullName), serializationSettings));
                }
            }

            if (packInfo == null)
            {
                Directory.Delete(diTmpDirPath.FullName, true);
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.PackInfoNotFound };
            }

            if (packInfo.PackageType != metadata.PackageType)
            {
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.WrongPackageType };
            }

            if (importBytes == null)
            {
                xfer = null;
                mre = new ManualResetEvent(false);
                DatabaseManager.Shard.GetCharacterTransfers((xfers) =>
                {
                    xfer = xfers.FirstOrDefault(k => k.SourceId == packInfo.CharacterId);
                    mre.Set();
                });
                mre.WaitOne();

                if (xfer != null)
                {
                    return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.CharacterAlreadyPresent };
                }
            }

            // isolate player biota
            List<Biota> playerCollection = snapshot.Where(k =>
                (
                    k.WeenieType == (uint)WeenieType.Admin ||
                    k.WeenieType == (uint)WeenieType.Sentinel ||
                    k.WeenieType == (uint)WeenieType.Creature
                )
            ).ToList();

            if (playerCollection.Count > 1)
            {
                Directory.Delete(diTmpDirPath.FullName, true);
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.FoundMoreThanOneCharacter };
            }
            if (playerCollection.Count < 1)
            {
                Directory.Delete(diTmpDirPath.FullName, true);
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.CannotFindCharacter };
            }
            IEnumerable<Biota> possessedBiotas2 = snapshot.Except(playerCollection).ToList();
            Biota newCharBiota = playerCollection.First();
            uint playerOrigId = newCharBiota.Id;

            // refactor
            ObjectGuid guid = GuidManager.NewPlayerGuid();
            newCharBiota = SetGuidAndScrubPKs(newCharBiota, guid.Full);
            List<BiotaPropertiesString> nameProp = newCharBiota.BiotaPropertiesString.Where(k => k.Type == (ushort)PropertyString.Name).ToList();
            if (nameProp.Count != 1)
            {
                Directory.Delete(diTmpDirPath.FullName, true);
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.MalformedCharacterData };
            }
            string FormerCharName = nameProp.First().Value;
            nameProp.First().Value = metadata.NewCharacterName;

            Collection<(Biota biota, ReaderWriterLockSlim rwLock)> possessedBiotas = new Collection<(Biota biota, ReaderWriterLockSlim rwLock)>();
            foreach (Biota possession in possessedBiotas2)
            {
                possessedBiotas.Add((SetGuidAndScrubPKs(possession, GuidManager.NewDynamicGuid().Full), new ReaderWriterLockSlim()));
            }
            foreach ((Biota biota, ReaderWriterLockSlim rwLock) item in possessedBiotas)
            {
                IEnumerable<BiotaPropertiesIID> instances = item.biota.BiotaPropertiesIID.Where(k => k.Value == playerOrigId);
                foreach (BiotaPropertiesIID instance in instances)
                {
                    instance.Value = guid.Full;
                }
            }
            foreach ((Biota biota, ReaderWriterLockSlim rwLock) item in possessedBiotas)
            {
                IEnumerable<BiotaPropertiesBookPageData> instances = item.biota.BiotaPropertiesBookPageData.Where(k => k.AuthorId == playerOrigId);
                foreach (BiotaPropertiesBookPageData instance in instances)
                {
                    instance.AuthorId = guid.Full;
                }
                //TO-DO: scrub other authors?
            }

            // build            
            Weenie weenie = DatabaseManager.World.GetCachedWeenie(newCharBiota.WeenieClassId);
            weenie.Type = newCharBiota.WeenieType;
            Player newPlayer = new Player(weenie, guid, metadata.AccountId)
            {
                Location = new Position(0xD655002C, 126.918549f, 81.756134f, 49.698814f, 0.794878f, 0.000000f, 0.000000f, -0.606769f), // Shoushi starter area
                Name = metadata.NewCharacterName,
            };
            newPlayer.Character.Name = metadata.NewCharacterName;

            // insert
            mre = new ManualResetEvent(false);
            bool addCharResult = false;
            DatabaseManager.Shard.AddCharacterInParallel(newCharBiota, newPlayer.BiotaDatabaseLock, possessedBiotas, newPlayer.Character, newPlayer.CharacterDatabaseLock, new Action<bool>((res2) =>
            {
                addCharResult = res2;
                mre.Set();
            }));
            mre.WaitOne();
            if (addCharResult)
            {
                // update server
                PlayerManager.AddOfflinePlayer(DatabaseManager.Shard.GetBiota(guid.Full));
                DatabaseManager.Shard.GetCharacters(metadata.AccountId, false, new Action<List<Character>>((chars) =>
                {
                    Session session = WorldManager.FindSessionByAccountId(metadata.AccountId);
                    if (session != null)
                    {
                        session.Characters.Add(chars.First(k => k.Id == guid.Full));
                    }
                }));
                DatabaseManager.Shard.SaveCharacterTransfer(new CharacterTransfer()
                {
                    AccountId = metadata.AccountId,
                    SourceId = packInfo.CharacterId,
                    TransferType = (uint)metadata.PackageType,
                    TransferTime = (ulong)Time.GetUnixTime(),
                    Cookie = metadata.Cookie,
                    SourceBaseUrl = (importBytes == null) ? metadata.ImportUrl.ToString() : null,
                    SourceThumbprint = verifiedSourceThumbprint,
                    TargetId = guid.Full,
                }, new ReaderWriterLockSlim(), null);
            }
            else
            {
                Directory.Delete(diTmpDirPath.FullName, true);
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.AddCharacterFailed };
            }

            // cleanup
            Directory.Delete(diTmpDirPath.FullName, true);

            // done
            return new ImportAndMigrateResult()
            {
                Success = true,
                NewCharacterName = metadata.NewCharacterName,
                NewCharacterId = guid.Full
            };
        }

        public enum PackageType
        {
            Migrate,
            Backup
        }
        public class PackageMetadata
        {
            public PackageType PackageType { get; set; }
            public string Cookie { get; set; }
            public uint CharacterId { get; set; }
            public uint AccountId { get; set; }
            public string FilePath { get; set; }
            public string NewCharacterName { get; set; }
            public Uri ImportUrl { get; set; }
        }
        public enum MigrationCloseType
        {
            Cancel,
            Download
        }
        public class MigrateCloseResult
        {
            public bool Success { get; set; } = false;
            public byte[] SnapshotPackage { get; set; }
        }
        public enum ImportAndMigrateFailiureReason
        {
            None,
            Unknown,
            OperationNotAllowed,
            UnverifiedSourceServerNotAllowed,
            VerifiedSourceServerNotAllowed,
            PackInfoNotFound,
            CannotContactSourceServer,
            ProtocolError,
            SourceServerRejectedRequest,
            CharacterAlreadyPresent,
            WrongPackageType,
            PackageTypeNotAllowed,
            NameIsUnavailable,
            FoundMoreThanOneCharacter,
            CannotFindCharacter,
            MalformedCharacterData,
            AddCharacterFailed,
            Forgery,
            PackageUnsigned,
            CookieAlreadyUsed
        }
        public class ImportAndMigrateResult
        {
            public ImportAndMigrateFailiureReason FailReason { get; set; } = ImportAndMigrateFailiureReason.Unknown;
            public bool Success { get; set; } = false;
            public string NewCharacterName { get; set; }
            public uint NewCharacterId { get; set; }
        }

        public static MigrateCloseResult CloseMigration(PackageMetadata metadata, MigrationCloseType type)
        {
            CharacterTransfer xfer = null;
            ManualResetEvent mre = new ManualResetEvent(false);
            DatabaseManager.Shard.GetCharacterTransfers((xfers) =>
            {
                xfer = xfers.FirstOrDefault(k => k.Cookie == metadata.Cookie);
                mre.Set();
            });
            mre.WaitOne();
            if (xfer == null)
            {
                return new MigrateCloseResult(); // non existant migration
            }
            if (xfer.TransferType != (uint)PackageType.Migrate)
            {
                return new MigrateCloseResult(); // not a migration
            }
            if (xfer.DownloadTime != null)
            {
                return new MigrateCloseResult(); // already downloaded, can't be cancelled
            }
            if (xfer.CancelTime != null)
            {
                return new MigrateCloseResult(); // already cancelled, can't be cancelled again
            }
            Character character = null;
            mre = new ManualResetEvent(false);
            DatabaseManager.Shard.GetCharacters(xfer.AccountId, false, new Action<List<Character>>(new Action<List<Character>>((chars) =>
            {
                character = chars.FirstOrDefault(k => k.Id == xfer.PackageSourceId);
                mre.Set();
            })));
            mre.WaitOne();
            if (character == null)
            {
                return new MigrateCloseResult(); // character not found...
            }
            if (!character.IsReadOnly)
            {
                return new MigrateCloseResult(); // character state is wrong
            }
            string packageFilePath = GetTransferPackageFilePath(metadata.Cookie);
            if (!File.Exists(packageFilePath))
            {
                return new MigrateCloseResult(); // package file should be there
            }
            if (type == MigrationCloseType.Cancel)
            {
                character.IsReadOnly = false;
                DatabaseManager.Shard.SaveCharacter(character, new ReaderWriterLockSlim(), null);
                xfer.CancelTime = (ulong)Time.GetUnixTime();
                DatabaseManager.Shard.SaveCharacterTransfer(xfer, new ReaderWriterLockSlim(), null);
                DeleteTransferPackageFile(metadata.Cookie);
                return new MigrateCloseResult()
                {
                    Success = true
                };
            }
            else if (type == MigrationCloseType.Download)
            {
                character.IsReadOnly = false;
                character.IsDeleted = true;
                character.DeleteTime = (ulong)Time.GetUnixTime();
                DatabaseManager.Shard.SaveCharacter(character, new ReaderWriterLockSlim(), null);
                xfer.DownloadTime = (ulong)Time.GetUnixTime();
                DatabaseManager.Shard.SaveCharacterTransfer(xfer, new ReaderWriterLockSlim(), null);
                MigrateCloseResult res = new MigrateCloseResult()
                {
                    Success = true,
                    SnapshotPackage = File.ReadAllBytes(packageFilePath)
                };
                DeleteTransferPackageFile(metadata.Cookie);
                return res;
            }
            else
            {
                return new MigrateCloseResult();
            }
        }

        /// <summary>
        /// Package the currently logged in character.
        /// </summary>
        /// <returns></returns>
        public static async Task<PackageMetadata> CreatePackage(PackageMetadata metadata)
        {
            metadata.Cookie = ThreadSafeRandom.NextString(CookieChars, CookieLength);

            // obtain character snapshot
            CharacterSnapshot snapshot = new CharacterSnapshot();
            TaskCompletionSource<object> tsc = new TaskCompletionSource<object>();

            OfflinePlayer offlinePlayer = PlayerManager.GetOfflinePlayer(metadata.CharacterId);
            if (offlinePlayer == null)
            {
                return null; // character must be logged out
            }
            snapshot.Player = offlinePlayer.Biota;
            DatabaseManager.Shard.GetCharacters(metadata.AccountId, false, new Action<List<Character>>(new Action<List<Character>>((chars) =>
            {
                snapshot.Character = chars.FirstOrDefault(k => k.Id == metadata.CharacterId);

                if (snapshot.Character == null)
                {
                    tsc.SetResult(new object());
                    return;
                }

                if (snapshot.Character.IsReadOnly)
                {
                    snapshot.Character = null;
                    tsc.SetResult(new object());
                    return;
                }

                if (metadata.PackageType == PackageType.Migrate)
                {
                    // place character in migrating state
                    snapshot.Character.IsReadOnly = true;
                    DatabaseManager.Shard.SaveCharacter(snapshot.Character, new ReaderWriterLockSlim(), null);
                }

                DatabaseManager.Shard.GetPossessedBiotasInParallel(snapshot.Character.Id, new Action<PossessedBiotas>((pb) =>
                {
                    snapshot.PossessedBiotas = pb;
                    tsc.SetResult(new object());
                }));
            })));
            await tsc.Task;

            if (snapshot.Character == null)
            {
                return null; // character not found
            }

            // prepare scratch directory
            string basePath = Path.Combine(ServerManager.TransferPath, metadata.Cookie);
            Directory.CreateDirectory(basePath);

            // serialize, save, and sign
            string playerPath = Path.Combine(basePath, snapshot.Player.Id + ".json");
            File.WriteAllText(playerPath, JsonConvert.SerializeObject(snapshot.Player, serializationSettings));
            CryptoManager.SignFile(playerPath);
            foreach (Biota biota in snapshot.PossessedBiotas.Inventory.Union(snapshot.PossessedBiotas.WieldedItems))
            {
                string biotaPath = Path.Combine(basePath, biota.Id + ".json");
                File.WriteAllText(biotaPath, JsonConvert.SerializeObject(biota, serializationSettings));
                CryptoManager.SignFile(biotaPath);
            }

            string metaPath = Path.Combine(basePath, "packinfo.json");
            File.WriteAllText(metaPath, JsonConvert.SerializeObject(metadata, serializationSettings));
            CryptoManager.SignFile(metaPath);

            CryptoManager.ExportCert(Path.Combine(basePath, "signer.crt"));

            // compress
            metadata.FilePath = Path.Combine(ServerManager.TransferPath, metadata.Cookie + ".zip");
            ZipFile.CreateFromDirectory(basePath, metadata.FilePath);

            // cleanup
            Directory.Delete(basePath, true);

            // save
            DatabaseManager.Shard.SaveCharacterTransfer(new CharacterTransfer()
            {
                AccountId = metadata.AccountId,
                PackageSourceId = metadata.CharacterId,
                TransferType = (uint)metadata.PackageType,
                TransferTime = (ulong)Time.GetUnixTime(),
                Cookie = metadata.Cookie,
            }, new ReaderWriterLockSlim(), null);

            return metadata;
        }

        /// <summary>
        /// copied from Biota.Clone extension method and modified
        /// </summary>
        /// <param name="biota"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static Biota SetGuidAndScrubPKs(Biota biota, uint guid)
        {
            Biota result = new Biota
            {
                Id = guid,
                WeenieClassId = biota.WeenieClassId,
                WeenieType = biota.WeenieType,
                PopulatedCollectionFlags = biota.PopulatedCollectionFlags
            };

            if (biota.BiotaPropertiesBook != null)
            {
                result.BiotaPropertiesBook = new BiotaPropertiesBook
                {
                    Id = 0,
                    ObjectId = guid
                };
                result.BiotaPropertiesBook.MaxNumPages = biota.BiotaPropertiesBook.MaxNumPages;
                result.BiotaPropertiesBook.MaxNumCharsPerPage = biota.BiotaPropertiesBook.MaxNumCharsPerPage;
            }

            foreach (BiotaPropertiesAnimPart value in biota.BiotaPropertiesAnimPart)
            {
                result.BiotaPropertiesAnimPart.Add(new BiotaPropertiesAnimPart
                {
                    Id = 0,
                    ObjectId = guid,
                    Index = value.Index,
                    AnimationId = value.AnimationId,
                    Order = value.Order,
                });
            }

            foreach (BiotaPropertiesAttribute value in biota.BiotaPropertiesAttribute)
            {
                result.BiotaPropertiesAttribute.Add(new BiotaPropertiesAttribute
                {
                    Id = 0,
                    ObjectId = guid,
                    Type = value.Type,
                    InitLevel = value.InitLevel,
                    LevelFromCP = value.LevelFromCP,
                    CPSpent = value.CPSpent,
                });
            }

            foreach (BiotaPropertiesAttribute2nd value in biota.BiotaPropertiesAttribute2nd)
            {
                result.BiotaPropertiesAttribute2nd.Add(new BiotaPropertiesAttribute2nd
                {
                    Id = 0,
                    ObjectId = guid,
                    Type = value.Type,
                    InitLevel = value.InitLevel,
                    LevelFromCP = value.LevelFromCP,
                    CPSpent = value.CPSpent,
                    CurrentLevel = value.CurrentLevel,
                });
            }

            foreach (BiotaPropertiesBodyPart value in biota.BiotaPropertiesBodyPart)
            {
                result.BiotaPropertiesBodyPart.Add(new BiotaPropertiesBodyPart
                {
                    Id = 0,
                    ObjectId = guid,
                    Key = value.Key,
                    DType = value.DType,
                    DVal = value.DVal,
                    DVar = value.DVar,
                    BaseArmor = value.BaseArmor,
                    ArmorVsSlash = value.ArmorVsSlash,
                    ArmorVsPierce = value.ArmorVsPierce,
                    ArmorVsBludgeon = value.ArmorVsBludgeon,
                    ArmorVsCold = value.ArmorVsCold,
                    ArmorVsFire = value.ArmorVsFire,
                    ArmorVsAcid = value.ArmorVsAcid,
                    ArmorVsElectric = value.ArmorVsElectric,
                    ArmorVsNether = value.ArmorVsNether,
                    BH = value.BH,
                    HLF = value.HLF,
                    MLF = value.MLF,
                    LLF = value.LLF,
                    HRF = value.HRF,
                    MRF = value.MRF,
                    LRF = value.LRF,
                    HLB = value.HLB,
                    MLB = value.MLB,
                    LLB = value.LLB,
                    HRB = value.HRB,
                    MRB = value.MRB,
                    LRB = value.LRB,
                });
            }

            foreach (BiotaPropertiesBookPageData value in biota.BiotaPropertiesBookPageData)
            {
                result.BiotaPropertiesBookPageData.Add(new BiotaPropertiesBookPageData
                {
                    Id = 0,
                    ObjectId = guid,
                    PageId = value.PageId,
                    AuthorId = value.AuthorId,
                    AuthorName = value.AuthorName,
                    AuthorAccount = value.AuthorAccount,
                    IgnoreAuthor = value.IgnoreAuthor,
                    PageText = value.PageText,
                });
            }

            foreach (BiotaPropertiesBool value in biota.BiotaPropertiesBool)
            {
                result.BiotaPropertiesBool.Add(new BiotaPropertiesBool
                {
                    Id = 0,
                    ObjectId = guid,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (BiotaPropertiesCreateList value in biota.BiotaPropertiesCreateList)
            {
                result.BiotaPropertiesCreateList.Add(new BiotaPropertiesCreateList
                {
                    Id = 0,
                    ObjectId = guid,
                    DestinationType = value.DestinationType,
                    WeenieClassId = value.WeenieClassId,
                    StackSize = value.StackSize,
                    Palette = value.Palette,
                    Shade = value.Shade,
                    TryToBond = value.TryToBond,
                });
            }

            foreach (BiotaPropertiesDID value in biota.BiotaPropertiesDID)
            {
                result.BiotaPropertiesDID.Add(new BiotaPropertiesDID
                {
                    Id = 0,
                    ObjectId = guid,
                    Type = value.Type,
                    Value = value.Value,
                });
            }


            foreach (BiotaPropertiesEmote value in biota.BiotaPropertiesEmote)
            {
                BiotaPropertiesEmote emote = new BiotaPropertiesEmote
                {
                    Id = 0,
                    ObjectId = guid,
                    Category = value.Category,
                    Probability = value.Probability,
                    WeenieClassId = value.WeenieClassId,
                    Style = value.Style,
                    Substyle = value.Substyle,
                    Quest = value.Quest,
                    VendorType = value.VendorType,
                    MinHealth = value.MinHealth,
                    MaxHealth = value.MaxHealth,
                };

                foreach (BiotaPropertiesEmoteAction value2 in value.BiotaPropertiesEmoteAction)
                {
                    BiotaPropertiesEmoteAction action = new BiotaPropertiesEmoteAction
                    {
                        Id = 0,
                        EmoteId = value2.EmoteId,
                        Order = value2.Order,
                        Type = value2.Type,
                        Delay = value2.Delay,
                        Extent = value2.Extent,
                        Motion = value2.Motion,
                        Message = value2.Message,
                        TestString = value2.TestString,
                        Min = value2.Min,
                        Max = value2.Max,
                        Min64 = value2.Min64,
                        Max64 = value2.Max64,
                        MinDbl = value2.MinDbl,
                        MaxDbl = value2.MaxDbl,
                        Stat = value2.Stat,
                        Display = value2.Display,
                        Amount = value2.Amount,
                        Amount64 = value2.Amount64,
                        HeroXP64 = value2.HeroXP64,
                        Percent = value2.Percent,
                        SpellId = value2.SpellId,
                        WealthRating = value2.WealthRating,
                        TreasureClass = value2.TreasureClass,
                        TreasureType = value2.TreasureType,
                        PScript = value2.PScript,
                        Sound = value2.Sound,
                        DestinationType = value2.DestinationType,
                        WeenieClassId = value2.WeenieClassId,
                        StackSize = value2.StackSize,
                        Palette = value2.Palette,
                        Shade = value2.Shade,
                        TryToBond = value2.TryToBond,
                        ObjCellId = value2.ObjCellId,
                        OriginX = value2.OriginX,
                        OriginY = value2.OriginY,
                        OriginZ = value2.OriginZ,
                        AnglesW = value2.AnglesW,
                        AnglesX = value2.AnglesX,
                        AnglesY = value2.AnglesY,
                        AnglesZ = value2.AnglesZ,
                    };

                    emote.BiotaPropertiesEmoteAction.Add(action);
                }

                result.BiotaPropertiesEmote.Add(emote);
            }


            foreach (BiotaPropertiesEnchantmentRegistry value in biota.BiotaPropertiesEnchantmentRegistry)
            {
                result.BiotaPropertiesEnchantmentRegistry.Add(new BiotaPropertiesEnchantmentRegistry
                {
                    //Id = 0,
                    ObjectId = guid,
                    EnchantmentCategory = value.EnchantmentCategory,
                    SpellId = value.SpellId,
                    LayerId = value.LayerId,
                    HasSpellSetId = value.HasSpellSetId,
                    SpellCategory = value.SpellCategory,
                    PowerLevel = value.PowerLevel,
                    StartTime = value.StartTime,
                    Duration = value.Duration,
                    CasterObjectId = value.CasterObjectId,//perhaps this needs foreign PK to be scrubbed out as well
                    DegradeModifier = value.DegradeModifier,
                    DegradeLimit = value.DegradeLimit,
                    LastTimeDegraded = value.LastTimeDegraded,
                    StatModType = value.StatModType,
                    StatModKey = value.StatModKey,
                    StatModValue = value.StatModValue,
                    SpellSetId = value.SpellSetId,
                });
            }


            foreach (BiotaPropertiesEventFilter value in biota.BiotaPropertiesEventFilter)
            {
                result.BiotaPropertiesEventFilter.Add(new BiotaPropertiesEventFilter
                {
                    Id = 0,
                    ObjectId = guid,
                    Event = value.Event,
                });
            }

            foreach (BiotaPropertiesFloat value in biota.BiotaPropertiesFloat)
            {
                result.BiotaPropertiesFloat.Add(new BiotaPropertiesFloat
                {
                    Id = 0,
                    ObjectId = guid,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (BiotaPropertiesGenerator value in biota.BiotaPropertiesGenerator)
            {
                result.BiotaPropertiesGenerator.Add(new BiotaPropertiesGenerator
                {
                    Id = 0,
                    ObjectId = guid,
                    Probability = value.Probability,
                    WeenieClassId = value.WeenieClassId,
                    Delay = value.Delay,
                    InitCreate = value.InitCreate,
                    MaxCreate = value.MaxCreate,
                    WhenCreate = value.WhenCreate,
                    WhereCreate = value.WhereCreate,
                    StackSize = value.StackSize,
                    PaletteId = value.PaletteId,
                    Shade = value.Shade,
                    ObjCellId = value.ObjCellId,
                    OriginX = value.OriginX,
                    OriginY = value.OriginY,
                    OriginZ = value.OriginZ,
                    AnglesW = value.AnglesW,
                    AnglesX = value.AnglesX,
                    AnglesY = value.AnglesY,
                    AnglesZ = value.AnglesZ,
                });
            }

            foreach (BiotaPropertiesIID value in biota.BiotaPropertiesIID)
            {
                result.BiotaPropertiesIID.Add(new BiotaPropertiesIID
                {
                    Id = 0,
                    ObjectId = guid,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (BiotaPropertiesInt value in biota.BiotaPropertiesInt)
            {
                result.BiotaPropertiesInt.Add(new BiotaPropertiesInt
                {
                    Id = 0,
                    ObjectId = guid,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (BiotaPropertiesInt64 value in biota.BiotaPropertiesInt64)
            {
                result.BiotaPropertiesInt64.Add(new BiotaPropertiesInt64
                {
                    Id = 0,
                    ObjectId = guid,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (BiotaPropertiesPalette value in biota.BiotaPropertiesPalette)
            {
                result.BiotaPropertiesPalette.Add(new BiotaPropertiesPalette
                {
                    Id = 0,
                    ObjectId = guid,
                    SubPaletteId = value.SubPaletteId,
                    Offset = value.Offset,
                    Length = value.Length,
                });
            }

            foreach (BiotaPropertiesPosition value in biota.BiotaPropertiesPosition)
            {
                result.BiotaPropertiesPosition.Add(new BiotaPropertiesPosition
                {
                    Id = 0,
                    ObjectId = guid,
                    PositionType = value.PositionType,
                    ObjCellId = value.ObjCellId,
                    OriginX = value.OriginX,
                    OriginY = value.OriginY,
                    OriginZ = value.OriginZ,
                    AnglesW = value.AnglesW,
                    AnglesX = value.AnglesX,
                    AnglesY = value.AnglesY,
                    AnglesZ = value.AnglesZ,
                });
            }

            foreach (BiotaPropertiesSkill value in biota.BiotaPropertiesSkill)
            {
                result.BiotaPropertiesSkill.Add(new BiotaPropertiesSkill
                {
                    Id = 0,
                    ObjectId = guid,
                    Type = value.Type,
                    LevelFromPP = value.LevelFromPP,
                    SAC = value.SAC,
                    PP = value.PP,
                    InitLevel = value.InitLevel,
                    ResistanceAtLastCheck = value.ResistanceAtLastCheck,
                    LastUsedTime = value.LastUsedTime,
                });
            }

            foreach (BiotaPropertiesSpellBook value in biota.BiotaPropertiesSpellBook)
            {
                result.BiotaPropertiesSpellBook.Add(new BiotaPropertiesSpellBook
                {
                    Id = 0,
                    ObjectId = guid,
                    Spell = value.Spell,
                    Probability = value.Probability,
                });
            }

            foreach (BiotaPropertiesString value in biota.BiotaPropertiesString)
            {
                result.BiotaPropertiesString.Add(new BiotaPropertiesString
                {
                    Id = 0,
                    ObjectId = guid,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (BiotaPropertiesTextureMap value in biota.BiotaPropertiesTextureMap)
            {
                result.BiotaPropertiesTextureMap.Add(new BiotaPropertiesTextureMap
                {
                    Id = 0,
                    ObjectId = guid,
                    Index = value.Index,
                    OldId = value.OldId,
                    NewId = value.NewId,
                    Order = value.Order,
                });
            }

            return result;
        }

        /// <summary>
        /// Check to see if the composition of the cookie is not good.
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns>If cookie is null returns false.  If cookie is not null and contains one or more invalid characters returns true.</returns>
        public static bool CookieContainsInvalidChars(string cookie)
        {
            if (cookie == null)
            {
                return false;
            }
            foreach (char c in cookie)
            {
                if (!CookieChars.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }

        private class CharacterSnapshot
        {
            public CharacterSnapshot() { }
            public Character Character { get; set; }
            public Biota Player { get; set; }
            public PossessedBiotas PossessedBiotas { get; set; }
        }
    }

    internal class InsecureWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri url)
        {
            HttpWebRequest _req = (HttpWebRequest)base.GetWebRequest(url);
            _req.ServerCertificateValidationCallback = (s, cert, chain, polErr) =>
            {
                return true; // chain-of-trust not implemented
            };
            return _req;
        }
    }
}
