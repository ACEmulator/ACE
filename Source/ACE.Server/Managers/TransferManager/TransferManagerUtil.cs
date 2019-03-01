using ACE.Database.Models.Shard;
using ACE.Server.Managers.TransferManager.Enums;
using ACE.Server.Managers.TransferManager.Responses;
using log4net;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace ACE.Server.Managers.TransferManager
{
    public static class TransferManagerUtil
    {
        public static string EnsureTransferPath(ILog log = null)
        {
            string u = Path.Combine(ServerManager.EnsureBasePath(log), "transfers");
            if (!Directory.Exists(u))
            {
                try
                {
                    Directory.CreateDirectory(u);
                    log?.Info($"Created transfers directory {u}");
                }
                catch (Exception ex) { log?.Fatal($"Failed to create transfers directory {u}", ex); }
            }
            return u;
        }
        /// <summary>
        /// Checks for the existence of a snapshot package file
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns>null if non-existent, or if existent the path to the transfer package file</returns>
        public static string GetTransferPackageFilePath(string cookie, ILog log = null)
        {
            if (StringContainsInvalidChars(TransferManagerConstants.CookieChars, cookie))
            {
                return null;
            }
            string filePath = Path.Combine(EnsureTransferPath(log), cookie + ".zip");
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
        public static void DeleteTransferPackageFile(string cookie, ILog log = null)
        {
            if (StringContainsInvalidChars(TransferManagerConstants.CookieChars, cookie))
            {
                return;
            }
            try
            {
                string filePath = Path.Combine(EnsureTransferPath(log), cookie + ".zip");
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                log?.Warn(ex);
            }
        }
        /// <summary>
        /// Check to see if the composition of the subject is not good based on a character white list.
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns>If subject is null returns false.  If subject is not null and contains one or more invalid characters returns true.</returns>
        public static bool StringContainsInvalidChars(string validChars, string subject)
        {
            if (subject == null)
            {
                return false;
            }
            foreach (char c in subject)
            {
                if (!validChars.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }
        public static ImportAndMigrateFailiureReason Verify(SignedMigrationCheckResponseModel resp, string myNonce, out string SignerThumbprint)
        {
            SignerThumbprint = null;
            X509Certificate2 signer = new X509Certificate2(resp.Signer);
            if (signer.Thumbprint != resp.Result.Config.MyThumbprint)
            {
                return ImportAndMigrateFailiureReason.SignatureMismatch;
            }
            SignerThumbprint = signer.Thumbprint;
            if (!CertificateManager.VerifySignedData(JsonConvert.SerializeObject(resp.Result, GetSerializationSettings()), resp.Signature, signer))
            {
                return ImportAndMigrateFailiureReason.SignatureInvalid;
            }
            if (myNonce != resp.Result.Nonce)
            {
                return ImportAndMigrateFailiureReason.NonceInvalid;
            }
            return ImportAndMigrateFailiureReason.None;
        }
        public static JsonSerializerSettings GetSerializationSettings()
        {
            return new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            };
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
    }
}
