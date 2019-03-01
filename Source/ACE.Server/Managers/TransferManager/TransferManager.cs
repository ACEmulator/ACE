using ACE.Common;
using ACE.Database;
using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Managers.TransferManager.Enums;
using ACE.Server.Managers.TransferManager.Responses;
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
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Server.Managers.TransferManager
{
    /// <summary>
    /// ServerManager handles character transfer and backup functionality.
    /// </summary>
    public static class TransferManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Initialize()
        {
            // TO-DO need active polling of stale temp files
            if (ConfigManager.Config.Transfer.KeepMigrationsForDays > -1)
            {
                DirectoryInfo di = new DirectoryInfo(TransferManagerUtil.EnsureTransferPath(log));
                FileInfo[] files = di.GetFiles();
                int fileCount = files.Count();
                if (fileCount > 0)
                {
                    string plural = (fileCount > 1 || fileCount == 0) ? "s" : "";
                    log.Info($"{fileCount} pending migration{plural}.");
                    int deletionCount = 0;
                    List<FileInfo> stales = files.Where(k => DateTime.Now - k.CreationTime > TimeSpan.FromDays(ConfigManager.Config.Transfer.KeepMigrationsForDays)).ToList();
                    stales.ForEach(file =>
                    {
                        File.Delete(file.FullName);
                        deletionCount++;
                    });
                    if (deletionCount > 0)
                    {
                        plural = (fileCount > 1 || fileCount == 0) ? "s" : "";
                        log.Debug($"{deletionCount} stale migration{plural} deleted.");
                    }
                }
            }
            int amfc = ConfigManager.Config.Transfer.AllowMigrationFrom.Count;
            log.Info($"Found {amfc} AllowMigrationFrom entr{((amfc > 1 || amfc == 0) ? "ies" : "y")} in Transfer configuration.");

            foreach (string trusted in ConfigManager.Config.Transfer.AllowMigrationFrom)
            {
                log.Debug($"AllowMigrationFrom Entry: {trusted}");
            }
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

            metadata.NewCharacterName = metadata.NewCharacterName.Trim();

            if (metadata.NewCharacterName.Length < GameConfiguration.CharacterNameMinimumLength || metadata.NewCharacterName.Length > GameConfiguration.CharacterNameMaximumLength)
            {
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.NameTooShortOrTooLong };
            }

            if (TransferManagerUtil.StringContainsInvalidChars(GameConfiguration.AllowedCharacterNameCharacters, metadata.NewCharacterName))
            {
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.NameContainsInvalidCharacters };
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
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.NameIsUnavailable };
            }
            // TO-DO: restricted and weenie name matching
            if (DatManager.PortalDat.TabooTable.ContainsBadWord(metadata.NewCharacterName))
            {
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.NameIsNaughty };
            }

            mre = new ManualResetEvent(false);
            bool slotCheck = false;
            DatabaseManager.Shard.GetCharacters(metadata.AccountId, false, new Action<List<Character>>((chars) =>
            {
                slotCheck = chars.Count + 1 <= GameConfiguration.SlotCount;
                mre.Set();
            }));
            mre.WaitOne();
            if (!slotCheck)
            {
                return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.NoCharacterSlotsAvailable };
            }

            string selectedMigrationSourceThumb = null;
            CharacterMigrationDownloadResponseModel snapshotPack = null;
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
                    // don't fail here, prevents inter-account same-server transfers and name changes
                    // return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.CookieAlreadyUsed };
                }

                // try to verify we trust the server before downloading package, since migrations cause source server to delete the original char upon download of the package
                string unverifiedThumbprintsSerialized = null;
                string nonce = ThreadSafeRandom.NextString(TransferManagerConstants.NonceChars, TransferManagerConstants.NonceLength);
                try
                {
                    using (InsecureWebClient iwc = new InsecureWebClient())
                    {
                        unverifiedThumbprintsSerialized = iwc.DownloadString(metadata.ImportUrl.ToString() + $"api/character/migrationCheck?Cookie={metadata.Cookie}&Nonce={nonce}");
                    }
                }
                catch
                {
                    return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.CannotContactSourceServer };
                }
                SignedMigrationCheckResponseModel signedMigrationCheckResult = null;
                try
                {
                    signedMigrationCheckResult = JsonConvert.DeserializeObject<SignedMigrationCheckResponseModel>(unverifiedThumbprintsSerialized, TransferManagerUtil.GetSerializationSettings());
                }
                catch
                {
                    return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.ProtocolError };
                }
                ImportAndMigrateFailiureReason SignedMigrationCheckResultValidationStatus = TransferManagerUtil.Verify(signedMigrationCheckResult, nonce, out selectedMigrationSourceThumb);
                if (SignedMigrationCheckResultValidationStatus != ImportAndMigrateFailiureReason.None)
                {
                    return new ImportAndMigrateResult() { FailReason = SignedMigrationCheckResultValidationStatus };
                }
                if (!signedMigrationCheckResult.Result.Ready)
                {
                    return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.MigrationCheckFailed };
                }
                if ((metadata.PackageType == PackageType.Migrate && !ConfigManager.Config.Transfer.AllowMigrationFrom.Any(k => k == signedMigrationCheckResult.Result.Config.MyThumbprint)) || (metadata.PackageType == PackageType.Backup && !ConfigManager.Config.Transfer.AllowImportFrom.Any(k => k == signedMigrationCheckResult.Result.Config.MyThumbprint)))
                {
                    return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.UnverifiedSourceServerNotAllowed };
                }
                if (metadata.PackageType == PackageType.Migrate && !signedMigrationCheckResult.Result.Config.AllowMigrate)
                {
                    return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.UnverifiedSourceServerDoesntAllowMigrate };
                }
                try
                {
                    using (InsecureWebClient iwc = new InsecureWebClient())
                    {
                        string TransferManagerCharacterMigrationDownloadResponseModelSerialized = iwc.DownloadString(metadata.ImportUrl.ToString() + $"api/character/migrationDownload?Cookie={metadata.Cookie}");
                        snapshotPack = JsonConvert.DeserializeObject<CharacterMigrationDownloadResponseModel>(TransferManagerCharacterMigrationDownloadResponseModelSerialized, TransferManagerUtil.GetSerializationSettings());
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
                if (!string.IsNullOrWhiteSpace(selectedMigrationSourceThumb) && verifiedSourceThumbprint != selectedMigrationSourceThumb)
                {
                    return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.VerifiedMigrationSourceThumbprintMismatch };
                }
                // verify that the signatures are valid
                foreach (FileInfo fil in diTmpDirPath.GetFiles("*.json"))
                {
                    if (!CertificateManager.VerifySignedFile(fil.FullName, signer))
                    {
                        Directory.Delete(diTmpDirPath.FullName, true);
                        return new ImportAndMigrateResult() { FailReason = ImportAndMigrateFailiureReason.CharacterPackageSignatureInvalid };
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
                    packInfo = JsonConvert.DeserializeObject<PackageMetadata>(File.ReadAllText(fil.FullName), TransferManagerUtil.GetSerializationSettings());
                }
                else
                {
                    snapshot.Add(JsonConvert.DeserializeObject<Biota>(File.ReadAllText(fil.FullName), TransferManagerUtil.GetSerializationSettings()));
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
            newCharBiota = TransferManagerUtil.SetGuidAndScrubPKs(newCharBiota, guid.Full);
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
                possessedBiotas.Add((TransferManagerUtil.SetGuidAndScrubPKs(possession, GuidManager.NewDynamicGuid().Full), new ReaderWriterLockSlim()));
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
                    Session session = WorldManager.Find(metadata.AccountId);
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
        public static MigrationReadyStatus CheckReadyStatusOfMigration(string cookie)
        {
            CharacterTransfer xfer = null;
            ManualResetEvent mre = new ManualResetEvent(false);
            DatabaseManager.Shard.GetCharacterTransfers((xfers) =>
            {
                xfer = xfers.FirstOrDefault(k => k.Cookie == cookie);
                mre.Set();
            });
            mre.WaitOne();
            if (xfer == null)
            {
                return MigrationReadyStatus.NonExistant;
            }
            if (xfer.TransferType != (uint)PackageType.Migrate)
            {
                return MigrationReadyStatus.NotMigration;
            }
            if (xfer.DownloadTime != null)
            {
                return MigrationReadyStatus.AlreadyDownloaded;
            }
            if (xfer.CancelTime != null)
            {
                return MigrationReadyStatus.AlreadyCancelled;
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
                return MigrationReadyStatus.CharNotFound;
            }
            if (!character.IsReadOnly)
            {
                return MigrationReadyStatus.InvalidCharState;
            }
            string packageFilePath = TransferManagerUtil.GetTransferPackageFilePath(cookie, log);
            if (!File.Exists(packageFilePath))
            {
                return MigrationReadyStatus.PackageFileMissing;
            }
            return MigrationReadyStatus.Ready;
        }
        public static MigrateCloseResult CloseMigration(PackageMetadata metadata, MigrationCloseType type)
        {
            MigrationReadyStatus readyStatus = CheckReadyStatusOfMigration(metadata.Cookie);
            if (readyStatus != MigrationReadyStatus.Ready)
            {
                return new MigrateCloseResult();
            }
            CharacterTransfer xfer = null;
            ManualResetEvent mre = new ManualResetEvent(false);
            DatabaseManager.Shard.GetCharacterTransfers((xfers) =>
            {
                xfer = xfers.FirstOrDefault(k => k.Cookie == metadata.Cookie);
                mre.Set();
            });
            mre.WaitOne();
            if (xfer.AccountId != metadata.AccountId)
            {
                return new MigrateCloseResult();
            }
            Character character = null;
            mre = new ManualResetEvent(false);
            DatabaseManager.Shard.GetCharacters(xfer.AccountId, false, new Action<List<Character>>(new Action<List<Character>>((chars) =>
            {
                character = chars.FirstOrDefault(k => k.Id == xfer.PackageSourceId);
                mre.Set();
            })));
            mre.WaitOne();
            string packageFilePath = TransferManagerUtil.GetTransferPackageFilePath(metadata.Cookie, log);
            if (type == MigrationCloseType.Cancel)
            {
                character.IsReadOnly = false;
                DatabaseManager.Shard.SaveCharacter(character, new ReaderWriterLockSlim(), null);
                xfer.CancelTime = (ulong)Time.GetUnixTime();
                DatabaseManager.Shard.SaveCharacterTransfer(xfer, new ReaderWriterLockSlim(), null);
                TransferManagerUtil.DeleteTransferPackageFile(metadata.Cookie, log);
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
                TransferManagerUtil.DeleteTransferPackageFile(metadata.Cookie, log);
                return res;
            }
            else
            {
                return new MigrateCloseResult();
            }
        }
        /// <summary>
        /// generate a snapshot package the specified character and freeze the character if it's a migration.
        /// </summary>
        /// <returns></returns>
        public static async Task<PackageMetadata> CreatePackage(PackageMetadata metadata)
        {
            metadata.Cookie = ThreadSafeRandom.NextString(TransferManagerConstants.CookieChars, TransferManagerConstants.CookieLength);

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
            string basePath = Path.Combine(TransferManagerUtil.EnsureTransferPath(log), metadata.Cookie);
            Directory.CreateDirectory(basePath);

            // serialize, save, and sign
            string playerPath = Path.Combine(basePath, snapshot.Player.Id + ".json");
            File.WriteAllText(playerPath, JsonConvert.SerializeObject(snapshot.Player, TransferManagerUtil.GetSerializationSettings()));
            CertificateManager.SignFile(playerPath);
            foreach (Biota biota in snapshot.PossessedBiotas.Inventory.Union(snapshot.PossessedBiotas.WieldedItems))
            {
                string biotaPath = Path.Combine(basePath, biota.Id + ".json");
                File.WriteAllText(biotaPath, JsonConvert.SerializeObject(biota, TransferManagerUtil.GetSerializationSettings()));
                CertificateManager.SignFile(biotaPath);
            }

            string metaPath = Path.Combine(basePath, "packinfo.json");
            File.WriteAllText(metaPath, JsonConvert.SerializeObject(metadata, TransferManagerUtil.GetSerializationSettings()));
            CertificateManager.SignFile(metaPath);

            CertificateManager.ExportCert(Path.Combine(basePath, "signer.crt"));

            // compress
            metadata.FilePath = Path.Combine(TransferManagerUtil.EnsureTransferPath(log), metadata.Cookie + ".zip");
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
    }
}
