using ACE.Common;
using ACE.Database;
using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Command;
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
        public class CharacterDownload
        {
            public bool Valid { get; set; } = false;
            public string FilePath { get; set; } = null;
            public Action UploadCompleted { get; set; } = null;
        }

        public const int CookieLength = 8;
        public const string CookieChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string CookieRegex = @"[0-9a-zA-Z]{8}";
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly JsonSerializerSettings serializationSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.None
        };
        public static CharacterDownload DownloadCharacter(string cookie)
        {
            string filePath = GetTransferPackageFilePath(cookie);
            CharacterDownload dl = new CharacterDownload();
            if (filePath != null)
            {
                dl.Valid = true;
                dl.UploadCompleted = new Action(() =>
                {
                    //TransferServerWrapper.ServeZipFile(filePath, req.NetworkStream);
                    //log.Info($"transfer {cookie} uploaded to {req.Client.Client.RemoteEndPoint}");
                    //DeleteTransferPackageFile(cookie);
                    // also delete original character (and log it out if neccessary) here?
                });
                dl.FilePath = filePath;
            }
            return dl;
        }

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
        }

        public static void Import(Session session, params string[] parameters)
        {
            Task.Run(() => ImportInner(session, parameters));
        }
        private static void ImportInner(Session session, params string[] parameters)
        {
            List<CommandParameterHelpers.ACECommandParameter> aceParams = new List<CommandParameterHelpers.ACECommandParameter>()
            {
                new CommandParameterHelpers.ACECommandParameter() {
                    Type = CommandParameterHelpers.ACECommandParameterType.PlayerName,
                    Required = true,
                    ErrorMessage = "You must supply a new character name for this import."
                },
                new CommandParameterHelpers.ACECommandParameter()
                {
                    Type = CommandParameterHelpers.ACECommandParameterType.Uri,
                    Required = true,
                    ErrorMessage = "You must supply the URL to the zip file download containing the character to import."
                }
            };
            if (!CommandParameterHelpers.ResolveACEParameters(session, parameters, aceParams))
            {
                return;
            }

            string charName = Import(session, aceParams[0].AsString, aceParams[1].AsUri);
            if (charName != null)
            {
                session.WorldBroadcast($"Character {charName} imported.");
            }
            else
            {
                session.WorldBroadcast($"Character import failed.");
            }
        }

        /// <summary>
        /// Checks for the existence of a transfer package file
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
        /// destroy an existent transfer package file
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

        /// <summary>
        /// Import a character.
        /// </summary>
        /// <param name="session">player's current session</param>
        /// <param name="charName">name for the imported character</param>
        /// <param name="importUrl">The source URL</param>
        /// <returns></returns>
        private static string Import(Session session, string charName, Uri importUrl)
        {
            bool NameIsGood = false;
            TaskCompletionSource<object> tsc = new TaskCompletionSource<object>();
            DatabaseManager.Shard.IsCharacterNameAvailable(charName, isAvailable =>
            {
                NameIsGood = isAvailable;
                tsc.SetResult(new object());
            });
            object o = tsc.Task.Result;
            if (!NameIsGood)
            {
                session.WorldBroadcast($"The character name {charName} is unavailable.");
                // TO-DO: prevent abuse (use to make list of taken names)
                // TO-DO: implement taboo-table lookup and other char name validation
                return null;
            }

            string tmpFilePath = Path.GetTempFileName();
            DirectoryInfo diTmpDirPath = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(tmpFilePath), Path.GetFileNameWithoutExtension(tmpFilePath)));
            string signerCertPath = Path.Combine(diTmpDirPath.FullName, "signer.crt");

            string sourceServerThumb = null;

            string getThumbUrl = importUrl.Scheme + "://" + importUrl.Authority + "/GetServerThumbsprint";

            using (InsecureWebClient client = new InsecureWebClient())
            {
                try { sourceServerThumb = client.DownloadString(getThumbUrl); } catch { }
            }
            // try to verify we trust the server before downloading package, since migrations cause source server to destroy the original char upon download of the package
            if (!string.IsNullOrWhiteSpace(sourceServerThumb))
            {
                if (!ConfigManager.Config.Transfer.TrustedServerCertThumbprints.Any(k => k == sourceServerThumb))
                {
                    session.WorldBroadcast($"The originating server isn't trusted.  Please contact the server operator and provide the thumbprint {sourceServerThumb} so they can add it to the trusted server cert thumbprints list.");
                    log.Info($"Untrusted transfer attempted.  Thumbprint: {sourceServerThumb}");
                    Directory.Delete(diTmpDirPath.FullName, true);
                    return null;
                }
            }

            using (InsecureWebClient client = new InsecureWebClient())
            {
                client.DownloadFile(importUrl, tmpFilePath);
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

            string signerThumbprint = string.Empty;

            using (X509Certificate2 signer = new X509Certificate2(signerCertPath))
            {
                signerThumbprint = signer.Thumbprint;
                // verify that the signer is in the trusted signers list
                if (!ConfigManager.Config.Transfer.TrustedServerCertThumbprints.Any(k => k == signerThumbprint))
                {
                    session.WorldBroadcast($"The originating server isn't trusted.  Please contact the server operator and provide the thumbprint {signerThumbprint} so they can add it to the trusted server cert thumbprints list.");
                    log.Info($"Untrusted transfer attempted.  Thumbprint: {signerThumbprint}");
                    Directory.Delete(diTmpDirPath.FullName, true);
                    return null;
                }
                // verify that the signatures are valid
                foreach (FileInfo fil in diTmpDirPath.GetFiles("*.json"))
                {
                    if (!CryptoManager.VerifySignature(fil.FullName, signer))
                    {
                        log.Info($"Transfer containing invalid signature attempted.  Thumbprint: {signerThumbprint}");
                        Directory.Delete(diTmpDirPath.FullName, true);
                        return null;
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
                log.Info($"packinfo.json not found.  Import cancelled.");
                Directory.Delete(diTmpDirPath.FullName, true);
                return null;
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
                log.Info($"Found more than one character, cancelling character import.");
                return null;
            }
            if (playerCollection.Count < 1)
            {
                Directory.Delete(diTmpDirPath.FullName, true);
                log.Info($"Could not find character, cancelling character import.");
                return null;
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
                log.Info($"Malformed character data, cancelling character import.");
                return null;
            }
            string FormerCharName = nameProp.First().Value;
            nameProp.First().Value = charName;

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
            Player newPlayer = new Player(weenie, guid, session.AccountId)
            {
                Location = session.Player.Location,
                Name = charName,
            };
            newPlayer.Character.Name = charName;

            // insert
            TaskCompletionSource<bool> resultTask = new TaskCompletionSource<bool>();
            DatabaseManager.Shard.AddCharacterInParallel(newCharBiota, newPlayer.BiotaDatabaseLock, possessedBiotas, newPlayer.Character, newPlayer.CharacterDatabaseLock, new Action<bool>((res2) =>
            {
                resultTask.SetResult(res2);
            }));
            object res = tsc.Task.Result;
            if (resultTask.Task.Result)
            {
                // update server
                PlayerManager.AddOfflinePlayer(DatabaseManager.Shard.GetBiota(guid.Full));
                DatabaseManager.Shard.GetCharacters(session.AccountId, false, new Action<List<Character>>((chars) =>
                {
                    session.Characters.Add(chars.First(k => k.Id == guid.Full));
                }));
                log.Info($"Character {charName} (formerly {FormerCharName}) imported from {importUrl} signer: {signerThumbprint}.");
            }
            else
            {
                log.Info($"Character {charName} import failiure from {importUrl} signer: {signerThumbprint}.");
                Directory.Delete(diTmpDirPath.FullName, true);
                return null;
            }

            // cleanup
            Directory.Delete(diTmpDirPath.FullName, true);

            // done, return character name
            return charName;
        }

        public enum PackageType
        {
            Move,
            Export
        }
        public class PackageMetadata
        {
            public PackageType PackageType { get; set; }
            public string Cookie { get; set; }
        }

        /// <summary>
        /// Package the currently logged in character.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static async Task<string> CreatePackage(Session session, PackageMetadata metadata)
        {
            string cookie = ThreadSafeRandom.NextString(CookieChars, CookieLength);
            uint accountId = session.AccountId;
            uint charId = session.Player.Guid.Full;

            // obtain character snapshot
            CharacterSnapshot snapshot = new CharacterSnapshot();
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

            // prepare scratch directory
            string basePath = Path.Combine(ServerManager.TransferPath, cookie);
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

            metadata.Cookie = cookie;
            string metaPath = Path.Combine(basePath, "packinfo.json");
            File.WriteAllText(metaPath, JsonConvert.SerializeObject(metadata, serializationSettings));
            CryptoManager.SignFile(metaPath);

            CryptoManager.ExportCert(Path.Combine(basePath, "signer.crt"));

            // compress
            string zipPath = Path.Combine(ServerManager.TransferPath, cookie + ".zip");
            ZipFile.CreateFromDirectory(basePath, zipPath);

            // cleanup
            Directory.Delete(basePath, true);

            return $"https://{ConfigManager.Config.Transfer.ExternalIPAddressOrDomainName}:{ConfigManager.Config.Server.Network.Port + 2}/DownloadCharacter?Cookie={cookie}";
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
