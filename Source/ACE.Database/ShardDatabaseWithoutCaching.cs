using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

using Microsoft.EntityFrameworkCore;

using log4net;

using ACE.Common.Extensions;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;

namespace ACE.Database
{
    /// <summary>
    /// This overloads GetBiota, SaveBiota and DeleteBiota to bypass the normal caching layer found in the base ShardDatabase.<para />
    /// You might want to use this if you have a very active server (500+ active players) and are having issues with memory consumption from Entity Framework entity tracking.<para />
    /// Player biotas are still tracked normally. Only non player biotas will bypass entity tracking.
    /// </summary>
    public class ShardDatabaseWithoutCaching : ShardDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ConditionalWeakTable<Models.Shard.Biota, ShardDbContext> PlayerBiotaContexts = new ConditionalWeakTable<Models.Shard.Biota, ShardDbContext>();

        public override Models.Shard.Biota GetBiota(uint id)
        {
            /*if (ObjectGuid.IsPlayer(id))
            {
                var context = new ShardDbContext();

                var biota = GetBiota(context, id);

                if (biota != null)
                    PlayerBiotaContexts.Add(biota, context);

                return biota;
            }*/

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetBiota(context, id);
            }
        }

        public override bool SaveBiota(Models.Shard.Biota biota, ReaderWriterLockSlim rwLock)
        {
            if (PlayerBiotaContexts.TryGetValue(biota, out var cachedContext))
            {
                rwLock.EnterReadLock();
                try
                {
                    SetBiotaPopulatedCollections(biota);

                    Exception firstException = null;
                    retry:

                    try
                    {
                        cachedContext.SaveChanges();

                        if (firstException != null)
                            log.Debug($"[DATABASE] SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (firstException == null)
                        {
                            firstException = ex;
                            goto retry;
                        }

                        // Character name might be in use or some other fault
                        log.Error($"[DATABASE] SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                        log.Error($"[DATABASE] SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }

            /*if (ObjectGuid.IsPlayer(biota.Id))
            {
                var context = new ShardDbContext();

                PlayerBiotaContexts.Add(biota, context);

                rwLock.EnterReadLock();
                try
                {
                    SetBiotaPopulatedCollections(biota);

                    context.Biota.Add(biota);

                    Exception firstException = null;
                    retry:

                    try
                    {
                        context.SaveChanges();

                        if (firstException != null)
                            log.Debug($"[DATABASE] SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (firstException == null)
                        {
                            firstException = ex;
                            goto retry;
                        }

                        // Character name might be in use or some other fault
                        log.Error($"[DATABASE] SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                        log.Error($"[DATABASE] SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }*/

            using (var context = new ShardDbContext())
            {
                var existingBiota = GetBiota(context, biota.Id);

                rwLock.EnterReadLock();
                try
                {
                    SetBiotaPopulatedCollections(biota);

                    if (existingBiota == null)
                        context.Biota.Add(biota);
                    else
                        UpdateBiota(context, existingBiota, biota);

                    Exception firstException = null;
                    retry:

                    try
                    {
                        context.SaveChanges();

                        if (firstException != null)
                            log.Debug($"[DATABASE] SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (firstException == null)
                        {
                            firstException = ex;
                            goto retry;
                        }

                        // Character name might be in use or some other fault
                        log.Error($"[DATABASE] SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                        log.Error($"[DATABASE] SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }
        }

        private void UpdateBiota(ShardDbContext context, Models.Shard.Biota existingBiota, Models.Shard.Biota biota)
        {
            // This pattern is described here: https://docs.microsoft.com/en-us/ef/core/saving/disconnected-entities
            // You'll notice though that we're not using the recommended: context.Entry(existingEntry).CurrentValues.SetValues(newEntry);
            // It is EXTREMLY slow. 4x or more slower. I suspect because it uses reflection to find the properties that the object contains
            // Manually setting the properties like we do below is the best case scenario for performance. However, it also has risks.
            // If we add columns to the schema and forget to add those changes here, changes to the biota may not propagate to the database.
            // Mag-nus 2018-08-18

            context.Entry(existingBiota).CurrentValues.SetValues(biota);

            foreach (var value in biota.BiotaPropertiesAnimPart)
            {
                BiotaPropertiesAnimPart existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesAnimPart.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesAnimPart.Add(value);
                else
                {
                    existingValue.Index = value.Index;
                    existingValue.AnimationId = value.AnimationId;
                    existingValue.Order = value.Order;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesAnimPart)
            {
                if (!biota.BiotaPropertiesAnimPart.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesAnimPart.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesAttribute)
            {
                BiotaPropertiesAttribute existingValue = existingBiota.BiotaPropertiesAttribute.FirstOrDefault(r => r.Type == value.Type);

                if (existingValue == null)
                    existingBiota.BiotaPropertiesAttribute.Add(value);
                else
                {
                    existingValue.InitLevel = value.InitLevel;
                    existingValue.LevelFromCP = value.LevelFromCP;
                    existingValue.CPSpent = value.CPSpent;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesAttribute)
            {
                if (!biota.BiotaPropertiesAttribute.Any(p => p.Type == value.Type))
                    context.BiotaPropertiesAttribute.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesAttribute2nd)
            {
                BiotaPropertiesAttribute2nd existingValue = existingBiota.BiotaPropertiesAttribute2nd.FirstOrDefault(r => r.Type == value.Type);

                if (existingValue == null)
                    existingBiota.BiotaPropertiesAttribute2nd.Add(value);
                else
                {
                    existingValue.InitLevel = value.InitLevel;
                    existingValue.LevelFromCP = value.LevelFromCP;
                    existingValue.CPSpent = value.CPSpent;
                    existingValue.CurrentLevel = value.CurrentLevel;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesAttribute2nd)
            {
                if (!biota.BiotaPropertiesAttribute2nd.Any(p => p.Type == value.Type))
                    context.BiotaPropertiesAttribute2nd.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesBodyPart)
            {
                BiotaPropertiesBodyPart existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesBodyPart.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesBodyPart.Add(value);
                else
                {
                    existingValue.Key = value.Key;
                    existingValue.DType = value.DType;
                    existingValue.DVal = value.DVal;
                    existingValue.DVar = value.DVar;
                    existingValue.BaseArmor = value.BaseArmor;
                    existingValue.ArmorVsSlash = value.ArmorVsSlash;
                    existingValue.ArmorVsPierce = value.ArmorVsPierce;
                    existingValue.ArmorVsBludgeon = value.ArmorVsBludgeon;
                    existingValue.ArmorVsCold = value.ArmorVsCold;
                    existingValue.ArmorVsFire = value.ArmorVsFire;
                    existingValue.ArmorVsAcid = value.ArmorVsAcid;
                    existingValue.ArmorVsElectric = value.ArmorVsElectric;
                    existingValue.ArmorVsNether = value.ArmorVsNether;
                    existingValue.BH = value.BH;
                    existingValue.HLF = value.HLF;
                    existingValue.MLF = value.MLF;
                    existingValue.LLF = value.LLF;
                    existingValue.HRF = value.HRF;
                    existingValue.MRF = value.MRF;
                    existingValue.LRF = value.LRF;
                    existingValue.HLB = value.HLB;
                    existingValue.MLB = value.MLB;
                    existingValue.LLB = value.LLB;
                    existingValue.HRB = value.HRB;
                    existingValue.MRB = value.MRB;
                    existingValue.LRB = value.LRB;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesBodyPart)
            {
                if (!biota.BiotaPropertiesBodyPart.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesBodyPart.Remove(value);
            }

            if (biota.BiotaPropertiesBook != null)
            {
                if (existingBiota.BiotaPropertiesBook == null)
                    existingBiota.BiotaPropertiesBook = biota.BiotaPropertiesBook;
                else
                {
                    existingBiota.BiotaPropertiesBook.MaxNumPages = biota.BiotaPropertiesBook.MaxNumPages;
                    existingBiota.BiotaPropertiesBook.MaxNumCharsPerPage = biota.BiotaPropertiesBook.MaxNumCharsPerPage;
                }
            }
            else
            {
                if (existingBiota.BiotaPropertiesBook != null)
                    context.BiotaPropertiesBook.Remove(existingBiota.BiotaPropertiesBook);
            }

            foreach (var value in biota.BiotaPropertiesBookPageData)
            {
                BiotaPropertiesBookPageData existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesBookPageData.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesBookPageData.Add(value);
                else
                {
                    existingValue.PageId = value.PageId;
                    existingValue.AuthorId = value.AuthorId;
                    existingValue.AuthorName = value.AuthorName;
                    existingValue.AuthorAccount = value.AuthorAccount;
                    existingValue.IgnoreAuthor = value.IgnoreAuthor;
                    existingValue.PageText = value.PageText;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesBookPageData)
            {
                if (!biota.BiotaPropertiesBookPageData.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesBookPageData.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesBool)
            {
                BiotaPropertiesBool existingValue = existingBiota.BiotaPropertiesBool.FirstOrDefault(r => r.Type == value.Type);

                if (existingValue == null)
                    existingBiota.BiotaPropertiesBool.Add(value);
                else
                    existingValue.Value = value.Value;
            }
            foreach (var value in existingBiota.BiotaPropertiesBool)
            {
                if (!biota.BiotaPropertiesBool.Any(p => p.Type == value.Type))
                    context.BiotaPropertiesBool.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesCreateList)
            {
                BiotaPropertiesCreateList existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesCreateList.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesCreateList.Add(value);
                else
                {
                    existingValue.DestinationType = value.DestinationType;
                    existingValue.WeenieClassId = value.WeenieClassId;
                    existingValue.StackSize = value.StackSize;
                    existingValue.Palette = value.Palette;
                    existingValue.Shade = value.Shade;
                    existingValue.TryToBond = value.TryToBond;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesCreateList)
            {
                if (!biota.BiotaPropertiesCreateList.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesCreateList.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesDID)
            {
                BiotaPropertiesDID existingValue = existingBiota.BiotaPropertiesDID.FirstOrDefault(r => r.Type == value.Type);

                if (existingValue == null)
                    existingBiota.BiotaPropertiesDID.Add(value);
                else
                    existingValue.Value = value.Value;
            }
            foreach (var value in existingBiota.BiotaPropertiesDID)
            {
                if (!biota.BiotaPropertiesDID.Any(p => p.Type == value.Type))
                    context.BiotaPropertiesDID.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesEmote)
            {
                BiotaPropertiesEmote existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesEmote.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesEmote.Add(value);
                else
                {
                    existingValue.Category = value.Category;
                    existingValue.Probability = value.Probability;
                    existingValue.WeenieClassId = value.WeenieClassId;
                    existingValue.Style = value.Style;
                    existingValue.Substyle = value.Substyle;
                    existingValue.Quest = value.Quest;
                    existingValue.VendorType = value.VendorType;
                    existingValue.MinHealth = value.MinHealth;
                    existingValue.MaxHealth = value.MaxHealth;

                    foreach (var value2 in value.BiotaPropertiesEmoteAction)
                    {
                        BiotaPropertiesEmoteAction existingValue2 = (value2.Id == 0 ? null : existingValue.BiotaPropertiesEmoteAction.FirstOrDefault(r => r.Id == value2.Id));

                        if (existingValue2 == null)
                            existingValue.BiotaPropertiesEmoteAction.Add(value2);
                        else
                        {
                            existingValue2.EmoteId = value2.EmoteId;
                            existingValue2.Order = value2.Order;
                            existingValue2.Type = value2.Type;
                            existingValue2.Delay = value2.Delay;
                            existingValue2.Extent = value2.Extent;
                            existingValue2.Motion = value2.Motion;
                            existingValue2.Message = value2.Message;
                            existingValue2.TestString = value2.TestString;
                            existingValue2.Min = value2.Min;
                            existingValue2.Max = value2.Max;
                            existingValue2.Min64 = value2.Min64;
                            existingValue2.Max64 = value2.Max64;
                            existingValue2.MinDbl = value2.MinDbl;
                            existingValue2.MaxDbl = value2.MaxDbl;
                            existingValue2.Stat = value2.Stat;
                            existingValue2.Display = value2.Display;
                            existingValue2.Amount = value2.Amount;
                            existingValue2.Amount64 = value2.Amount64;
                            existingValue2.HeroXP64 = value2.HeroXP64;
                            existingValue2.Percent = value2.Percent;
                            existingValue2.SpellId = value2.SpellId;
                            existingValue2.WealthRating = value2.WealthRating;
                            existingValue2.TreasureClass = value2.TreasureClass;
                            existingValue2.TreasureType = value2.TreasureType;
                            existingValue2.PScript = value2.PScript;
                            existingValue2.Sound = value2.Sound;
                            existingValue2.DestinationType = value2.DestinationType;
                            existingValue2.WeenieClassId = value2.WeenieClassId;
                            existingValue2.StackSize = value2.StackSize;
                            existingValue2.Palette = value2.Palette;
                            existingValue2.Shade = value2.Shade;
                            existingValue2.TryToBond = value2.TryToBond;
                            existingValue2.ObjCellId = value2.ObjCellId;
                            existingValue2.OriginX = value2.OriginX;
                            existingValue2.OriginY = value2.OriginY;
                            existingValue2.OriginZ = value2.OriginZ;
                            existingValue2.AnglesW = value2.AnglesW;
                            existingValue2.AnglesX = value2.AnglesX;
                            existingValue2.AnglesY = value2.AnglesY;
                            existingValue2.AnglesZ = value2.AnglesZ;
                        }
                    }
                    foreach (var value2 in value.BiotaPropertiesEmoteAction)
                    {
                        if (!existingValue.BiotaPropertiesEmoteAction.Any(p => p.Id == value2.Id))
                            context.BiotaPropertiesEmoteAction.Remove(value2);
                    }
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesEmote)
            {
                if (!biota.BiotaPropertiesEmote.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesEmote.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesEnchantmentRegistry)
            {
                BiotaPropertiesEnchantmentRegistry existingValue = (value.ObjectId == 0 ? null : existingBiota.BiotaPropertiesEnchantmentRegistry.FirstOrDefault(r => r.SpellId == value.SpellId && r.LayerId == value.LayerId && r.CasterObjectId == value.CasterObjectId));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesEnchantmentRegistry.Add(value);
                else
                {
                    existingValue.EnchantmentCategory = value.EnchantmentCategory;
                    existingValue.SpellId = value.SpellId;
                    existingValue.LayerId = value.LayerId;
                    existingValue.HasSpellSetId = value.HasSpellSetId;
                    existingValue.SpellCategory = value.SpellCategory;
                    existingValue.PowerLevel = value.PowerLevel;
                    existingValue.StartTime = value.StartTime;
                    existingValue.Duration = value.Duration;
                    existingValue.CasterObjectId = value.CasterObjectId;
                    existingValue.DegradeModifier = value.DegradeModifier;
                    existingValue.DegradeLimit = value.DegradeLimit;
                    existingValue.LastTimeDegraded = value.LastTimeDegraded;
                    existingValue.StatModType = value.StatModType;
                    existingValue.StatModKey = value.StatModKey;
                    existingValue.StatModValue = value.StatModValue;
                    existingValue.SpellSetId = value.SpellSetId;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesEnchantmentRegistry)
            {
                if (!biota.BiotaPropertiesEnchantmentRegistry.Any(p => p.SpellId == value.SpellId && p.LayerId == value.LayerId && p.CasterObjectId == value.CasterObjectId))
                    context.BiotaPropertiesEnchantmentRegistry.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesEventFilter)
            {
                BiotaPropertiesEventFilter existingValue = existingBiota.BiotaPropertiesEventFilter.FirstOrDefault(r => r.Event == value.Event);

                if (existingValue == null)
                    existingBiota.BiotaPropertiesEventFilter.Add(value);
            }
            foreach (var value in existingBiota.BiotaPropertiesEventFilter)
            {
                if (!biota.BiotaPropertiesEventFilter.Any(p => p.Event == value.Event))
                    context.BiotaPropertiesEventFilter.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesFloat)
            {
                BiotaPropertiesFloat existingValue = existingBiota.BiotaPropertiesFloat.FirstOrDefault(r => r.Type == value.Type);

                if (existingValue == null)
                    existingBiota.BiotaPropertiesFloat.Add(value);
                else
                    existingValue.Value = value.Value;
            }
            foreach (var value in existingBiota.BiotaPropertiesFloat)
            {
                if (!biota.BiotaPropertiesFloat.Any(p => p.Type == value.Type))
                    context.BiotaPropertiesFloat.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesGenerator)
            {
                BiotaPropertiesGenerator existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesGenerator.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesGenerator.Add(value);
                else
                {
                    existingValue.Probability = value.Probability;
                    existingValue.WeenieClassId = value.WeenieClassId;
                    existingValue.Delay = value.Delay;
                    existingValue.InitCreate = value.InitCreate;
                    existingValue.MaxCreate = value.MaxCreate;
                    existingValue.WhenCreate = value.WhenCreate;
                    existingValue.WhereCreate = value.WhereCreate;
                    existingValue.StackSize = value.StackSize;
                    existingValue.PaletteId = value.PaletteId;
                    existingValue.Shade = value.Shade;
                    existingValue.ObjCellId = value.ObjCellId;
                    existingValue.OriginX = value.OriginX;
                    existingValue.OriginY = value.OriginY;
                    existingValue.OriginZ = value.OriginZ;
                    existingValue.AnglesW = value.AnglesW;
                    existingValue.AnglesX = value.AnglesX;
                    existingValue.AnglesY = value.AnglesY;
                    existingValue.AnglesZ = value.AnglesZ;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesGenerator)
            {
                if (!biota.BiotaPropertiesGenerator.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesGenerator.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesIID)
            {
                BiotaPropertiesIID existingValue = existingBiota.BiotaPropertiesIID.FirstOrDefault(r => r.Type == value.Type);

                if (existingValue == null)
                    existingBiota.BiotaPropertiesIID.Add(value);
                else
                    existingValue.Value = value.Value;
            }
            foreach (var value in existingBiota.BiotaPropertiesIID)
            {
                if (!biota.BiotaPropertiesIID.Any(p => p.Type == value.Type))
                    context.BiotaPropertiesIID.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesInt)
            {
                BiotaPropertiesInt existingValue = existingBiota.BiotaPropertiesInt.FirstOrDefault(r => r.Type == value.Type);

                if (existingValue == null)
                    existingBiota.BiotaPropertiesInt.Add(value);
                else
                    existingValue.Value = value.Value;
            }
            foreach (var value in existingBiota.BiotaPropertiesInt)
            {
                if (!biota.BiotaPropertiesInt.Any(p => p.Type == value.Type))
                    context.BiotaPropertiesInt.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesInt64)
            {
                BiotaPropertiesInt64 existingValue = existingBiota.BiotaPropertiesInt64.FirstOrDefault(r => r.Type == value.Type);

                if (existingValue == null)
                    existingBiota.BiotaPropertiesInt64.Add(value);
                else
                    existingValue.Value = value.Value;
            }
            foreach (var value in existingBiota.BiotaPropertiesInt64)
            {
                if (!biota.BiotaPropertiesInt64.Any(p => p.Type == value.Type))
                    context.BiotaPropertiesInt64.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesPalette)
            {
                BiotaPropertiesPalette existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesPalette.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesPalette.Add(value);
                else
                {
                    existingValue.SubPaletteId = value.SubPaletteId;
                    existingValue.Offset = value.Offset;
                    existingValue.Length = value.Length;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesPalette)
            {
                if (!biota.BiotaPropertiesPalette.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesPalette.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesPosition)
            {
                BiotaPropertiesPosition existingValue = existingBiota.BiotaPropertiesPosition.FirstOrDefault(r => r.PositionType == value.PositionType);

                if (existingValue == null)
                    existingBiota.BiotaPropertiesPosition.Add(value);
                else
                {
                    existingValue.ObjCellId = value.ObjCellId;
                    existingValue.OriginX = value.OriginX;
                    existingValue.OriginY = value.OriginY;
                    existingValue.OriginZ = value.OriginZ;
                    existingValue.AnglesW = value.AnglesW;
                    existingValue.AnglesX = value.AnglesX;
                    existingValue.AnglesY = value.AnglesY;
                    existingValue.AnglesZ = value.AnglesZ;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesPosition)
            {
                if (!biota.BiotaPropertiesPosition.Any(p => p.PositionType == value.PositionType))
                    context.BiotaPropertiesPosition.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesSkill)
            {
                BiotaPropertiesSkill existingValue = existingBiota.BiotaPropertiesSkill.FirstOrDefault(r => r.Type == value.Type);

                if (existingValue == null)
                    existingBiota.BiotaPropertiesSkill.Add(value);
                else
                {
                    existingValue.LevelFromPP = value.LevelFromPP;
                    existingValue.SAC = value.SAC;
                    existingValue.PP = value.PP;
                    existingValue.InitLevel = value.InitLevel;
                    existingValue.ResistanceAtLastCheck = value.ResistanceAtLastCheck;
                    existingValue.LastUsedTime = value.LastUsedTime;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesSkill)
            {
                if (!biota.BiotaPropertiesSkill.Any(p => p.Type == value.Type))
                    context.BiotaPropertiesSkill.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesSpellBook)
            {
                BiotaPropertiesSpellBook existingValue = existingBiota.BiotaPropertiesSpellBook.FirstOrDefault(r => r.Spell == value.Spell);

                if (existingValue == null)
                    existingBiota.BiotaPropertiesSpellBook.Add(value);
                else
                    existingValue.Probability = value.Probability;
            }
            foreach (var value in existingBiota.BiotaPropertiesSpellBook)
            {
                if (!biota.BiotaPropertiesSpellBook.Any(p => p.Spell == value.Spell))
                    context.BiotaPropertiesSpellBook.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesString)
            {
                BiotaPropertiesString existingValue = existingBiota.BiotaPropertiesString.FirstOrDefault(r => r.Type == value.Type);

                if (existingValue == null)
                    existingBiota.BiotaPropertiesString.Add(value);
                else
                    existingValue.Value = value.Value;
            }
            foreach (var value in existingBiota.BiotaPropertiesString)
            {
                if (!biota.BiotaPropertiesString.Any(p => p.Type == value.Type))
                    context.BiotaPropertiesString.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesTextureMap)
            {
                BiotaPropertiesTextureMap existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesTextureMap.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesTextureMap.Add(value);
                else
                {
                    existingValue.Index = value.Index;
                    existingValue.OldId = value.OldId;
                    existingValue.NewId = value.NewId;
                    existingValue.Order = value.Order;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesTextureMap)
            {
                if (!biota.BiotaPropertiesTextureMap.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesTextureMap.Remove(value);
            }

            foreach (var value in biota.HousePermission)
            {
                Models.Shard.HousePermission existingValue = existingBiota.HousePermission.FirstOrDefault(r => r.HouseId == value.HouseId && r.PlayerGuid == value.PlayerGuid);

                if (existingValue == null)
                    existingBiota.HousePermission.Add(value);
                else
                    existingValue.Storage = value.Storage;
            }
            foreach (var value in existingBiota.HousePermission)
            {
                if (!biota.HousePermission.Any(p => p.HouseId == value.HouseId && p.PlayerGuid == value.PlayerGuid))
                    context.HousePermission.Remove(value);
            }
        }

        public override bool RemoveBiota(Models.Shard.Biota biota, ReaderWriterLockSlim rwLock)
        {
            if (PlayerBiotaContexts.TryGetValue(biota, out var cachedContext))
            {
                PlayerBiotaContexts.Remove(biota);

                rwLock.EnterReadLock();
                try
                {
                    cachedContext.Biota.Remove(biota);

                    Exception firstException = null;
                retry:

                    try
                    {
                        cachedContext.SaveChanges();

                        if (firstException != null)
                            log.Debug($"[DATABASE] RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (firstException == null)
                        {
                            firstException = ex;
                            goto retry;
                        }

                        // Character name might be in use or some other fault
                        log.Error($"[DATABASE] RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                        log.Error($"[DATABASE] RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();

                    cachedContext.Dispose();
                }
            }

            if (!ObjectGuid.IsPlayer(biota.Id))
            {
                using (var context = new ShardDbContext())
                {
                    var existingBiota = context.Biota
                        .AsNoTracking()
                        .FirstOrDefault(r => r.Id == biota.Id);

                    if (existingBiota == null)
                        return true;

                    rwLock.EnterWriteLock();
                    try
                    {
                        context.Biota.Remove(existingBiota);

                        Exception firstException = null;
                    retry:

                        try
                        {
                            context.SaveChanges();

                            if (firstException != null)
                                log.Debug($"[DATABASE] RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                            return true;
                        }
                        catch (Exception ex)
                        {
                            if (firstException == null)
                            {
                                firstException = ex;
                                goto retry;
                            }

                            // Character name might be in use or some other fault
                            log.Error($"[DATABASE] RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                            log.Error($"[DATABASE] RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
                            return false;
                        }
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
            }

            // If we got here, the biota didn't come from the database through this class.
            // Most likely, it doesn't exist in the database, so, no need to remove.
            return true;
        }

























        public override bool SaveBiota(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock)
        {
            using (var context = new ShardDbContext())
            {
                var existingBiota = GetBiota(context, biota.Id);

                rwLock.EnterReadLock();
                try
                {
                    if (existingBiota == null)
                    {
                        var dbBiota = ACE.Database.Adapter.BiotaConverter.ConvertFromEntityBiota(biota);

                        SetBiotaPopulatedCollections(dbBiota);

                        context.Biota.Add(dbBiota);
                    }
                    else
                    {
                        UpdateBiota(context, existingBiota, biota);

                        SetBiotaPopulatedCollections(existingBiota);
                    }

                    Exception firstException = null;
                retry:

                    try
                    {
                        context.SaveChanges();

                        if (firstException != null)
                            log.Debug($"[DATABASE] SaveBiota 0x{biota.Id:X8}:{biota.GetName()} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (firstException == null)
                        {
                            firstException = ex;
                            goto retry;
                        }

                        // Character name might be in use or some other fault
                        log.Error($"[DATABASE] SaveBiota 0x{biota.Id:X8}:{biota.GetName()} failed first attempt with exception: {firstException}");
                        log.Error($"[DATABASE] SaveBiota 0x{biota.Id:X8}:{biota.GetName()} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }
        }

        private void UpdateBiota(ShardDbContext context, Models.Shard.Biota existingBiota, ACE.Entity.Models.Biota biota)
        {
            // This pattern is described here: https://docs.microsoft.com/en-us/ef/core/saving/disconnected-entities
            // You'll notice though that we're not using the recommended: context.Entry(existingEntry).CurrentValues.SetValues(newEntry);
            // It is EXTREMLY slow. 4x or more slower. I suspect because it uses reflection to find the properties that the object contains
            // Manually setting the properties like we do below is the best case scenario for performance. However, it also has risks.
            // If we add columns to the schema and forget to add those changes here, changes to the biota may not propagate to the database.
            // Mag-nus 2018-08-18

            existingBiota.WeenieClassId = biota.WeenieClassId;
            existingBiota.WeenieType = (int)biota.WeenieType;

            if (biota.PropertiesBool != null)
            {
                foreach (var kvp in biota.PropertiesBool)
                    existingBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in existingBiota.BiotaPropertiesBool)
            {
                if (biota.PropertiesBool == null || !biota.PropertiesBool.ContainsKey((PropertyBool)value.Type))
                    context.BiotaPropertiesBool.Remove(value);
            }

            if (biota.PropertiesDID != null)
            {
                foreach (var kvp in biota.PropertiesDID)
                    existingBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in existingBiota.BiotaPropertiesDID)
            {
                if (biota.PropertiesDID == null || !biota.PropertiesDID.ContainsKey((PropertyDataId)value.Type))
                    context.BiotaPropertiesDID.Remove(value);
            }

            if (biota.PropertiesFloat != null)
            {
                foreach (var kvp in biota.PropertiesFloat)
                    existingBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in existingBiota.BiotaPropertiesFloat)
            {
                if (biota.PropertiesFloat == null || !biota.PropertiesFloat.ContainsKey((PropertyFloat)value.Type))
                    context.BiotaPropertiesFloat.Remove(value);
            }

            if (biota.PropertiesIID != null)
            {
                foreach (var kvp in biota.PropertiesIID)
                    existingBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in existingBiota.BiotaPropertiesIID)
            {
                if (biota.PropertiesIID == null || !biota.PropertiesIID.ContainsKey((PropertyInstanceId)value.Type))
                    context.BiotaPropertiesIID.Remove(value);
            }

            if (biota.PropertiesInt != null)
            {
                foreach (var kvp in biota.PropertiesInt)
                    existingBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in existingBiota.BiotaPropertiesInt)
            {
                if (biota.PropertiesInt == null || !biota.PropertiesInt.ContainsKey((PropertyInt)value.Type))
                    context.BiotaPropertiesInt.Remove(value);
            }

            if (biota.PropertiesInt64 != null)
            {
                foreach (var kvp in biota.PropertiesInt64)
                    existingBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in existingBiota.BiotaPropertiesInt64)
            {
                if (biota.PropertiesInt64 == null || !biota.PropertiesInt64.ContainsKey((PropertyInt64)value.Type))
                    context.BiotaPropertiesInt64.Remove(value);
            }

            if (biota.PropertiesString != null)
            {
                foreach (var kvp in biota.PropertiesString)
                    existingBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in existingBiota.BiotaPropertiesString)
            {
                if (biota.PropertiesString == null || !biota.PropertiesString.ContainsKey((PropertyString)value.Type))
                    context.BiotaPropertiesString.Remove(value);
            }


            if (biota.PropertiesPosition != null)
            {
                foreach (var kvp in biota.PropertiesPosition)
                {
                    BiotaPropertiesPosition existingValue = existingBiota.BiotaPropertiesPosition.FirstOrDefault(r => r.PositionType == (ushort)kvp.Key);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesPosition { ObjectId = biota.Id };

                        existingBiota.BiotaPropertiesPosition.Add(existingValue);
                    }

                    existingValue.PositionType = (ushort)kvp.Key;
                    existingValue.ObjCellId = kvp.Value.ObjCellId;
                    existingValue.OriginX = kvp.Value.PositionX;
                    existingValue.OriginY = kvp.Value.PositionY;
                    existingValue.OriginZ = kvp.Value.PositionZ;
                    existingValue.AnglesW = kvp.Value.RotationW;
                    existingValue.AnglesX = kvp.Value.RotationX;
                    existingValue.AnglesY = kvp.Value.RotationY;
                    existingValue.AnglesZ = kvp.Value.RotationZ;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesPosition)
            {
                if (biota.PropertiesPosition == null || !biota.PropertiesPosition.ContainsKey((PositionType)value.PositionType))
                    context.BiotaPropertiesPosition.Remove(value);
            }


            if (biota.PropertiesSpellBook != null)
            {
                foreach (var kvp in biota.PropertiesSpellBook)
                {
                    BiotaPropertiesSpellBook existingValue = existingBiota.BiotaPropertiesSpellBook.FirstOrDefault(r => r.Spell == (ushort)kvp.Key);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesSpellBook { ObjectId = biota.Id };

                        existingBiota.BiotaPropertiesSpellBook.Add(existingValue);
                    }

                    existingValue.Spell = kvp.Key;
                    existingValue.Probability = kvp.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesSpellBook)
            {
                if (biota.PropertiesSpellBook == null || !biota.PropertiesSpellBook.ContainsKey(value.Spell))
                    context.BiotaPropertiesSpellBook.Remove(value);
            }


            /*if (biota.PropertiesAnimPart != null)
            {
                foreach (var value in biota.PropertiesAnimPart) // todo switch to for int i
                {
                    BiotaPropertiesAnimPart existingValue = existingBiota.BiotaPropertiesAnimPart.FirstOrDefault(r => r.AnimationId == value.AnimationId); // todo animationid is not a unique key

                    if (existingValue == null)
                        existingBiota.BiotaPropertiesAnimPart.Add(value);
                    else
                    {
                        existingValue.Index = value.Index;
                        existingValue.AnimationId = value.AnimationId;
                        existingValue.Order = (byte)biota.PropertiesAnimPart.IndexOf(value);
                    }
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesAnimPart)
            {
                if (biota.PropertiesAnimPart == null || !biota.PropertiesAnimPart.Any(p => p.AnimationId == value.AnimationId))
                    context.BiotaPropertiesAnimPart.Remove(value);
            }*/

            /*if (biota.PropertiesPalette != null)
            {
                foreach (var value in biota.PropertiesPalette)
                {
                    BiotaPropertiesPalette existingValue = existingBiota.BiotaPropertiesPalette.FirstOrDefault(r => r.Id == value.Id);

                    if (existingValue == null)
                        existingBiota.BiotaPropertiesPalette.Add(value);
                    else
                    {
                        existingValue.SubPaletteId = value.SubPaletteId;
                        existingValue.Offset = value.Offset;
                        existingValue.Length = value.Length;
                    }
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesPalette)
            {
                if (biota.PropertiesPalette == null || !biota.PropertiesPalette.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesPalette.Remove(value);
            }*/

            if (biota.PropertiesTextureMap != null)
            {
                for (int i = 0; i < biota.PropertiesTextureMap.Count; i++)
                {
                    var value = biota.PropertiesTextureMap[i];

                    BiotaPropertiesTextureMap existingValue = existingBiota.BiotaPropertiesTextureMap.FirstOrDefault(r => r.Order == i);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesTextureMap { ObjectId = biota.Id };

                        existingBiota.BiotaPropertiesTextureMap.Add(existingValue);
                    }

                    existingValue.Index = value.PartIndex;
                    existingValue.OldId = value.OldTexture;
                    existingValue.NewId = value.NewTexture;
                    existingValue.Order = (byte)biota.PropertiesTextureMap.IndexOf(value);
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesTextureMap)
            {
                if (biota.PropertiesTextureMap == null || value.Order >= biota.PropertiesTextureMap.Count)
                    context.BiotaPropertiesTextureMap.Remove(value);
            }


            // Properties for all world objects that typically aren't modified over the original Biota

            /*if (biota.PropertiesCreateList != null)
            {
                foreach (var value in biota.PropertiesCreateList)
                {
                    BiotaPropertiesCreateList existingValue = existingBiota.BiotaPropertiesCreateList.FirstOrDefault(r => r.Id == value.Id);

                    if (existingValue == null)
                        existingBiota.BiotaPropertiesCreateList.Add(value);
                    else
                    {
                        existingValue.DestinationType = value.DestinationType;
                        existingValue.WeenieClassId = value.WeenieClassId;
                        existingValue.StackSize = value.StackSize;
                        existingValue.Palette = value.Palette;
                        existingValue.Shade = value.Shade;
                        existingValue.TryToBond = value.TryToBond;
                    }
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesCreateList)
            {
                if (biota.PropertiesCreateList == null || !biota.PropertiesCreateList.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesCreateList.Remove(value);
            }*/

            /*if (biota.PropertiesEmote != null)
            {
                foreach (var value in biota.PropertiesEmote)
                {
                    BiotaPropertiesEmote existingValue = existingBiota.BiotaPropertiesEmote.FirstOrDefault(r => r.Id == value.Id);

                    if (existingValue == null)
                        existingBiota.BiotaPropertiesEmote.Add(value);
                    else
                    {
                        existingValue.Category = value.Category;
                        existingValue.Probability = value.Probability;
                        existingValue.WeenieClassId = value.WeenieClassId;
                        existingValue.Style = value.Style;
                        existingValue.Substyle = value.Substyle;
                        existingValue.Quest = value.Quest;
                        existingValue.VendorType = value.VendorType;
                        existingValue.MinHealth = value.MinHealth;
                        existingValue.MaxHealth = value.MaxHealth;

                        foreach (var value2 in value.PropertiesEmoteAction)
                        {
                            BiotaPropertiesEmoteAction existingValue2 = existingValue.BiotaPropertiesEmoteAction.FirstOrDefault(r => r.Id == value2.Id);

                            if (existingValue2 == null)
                                existingValue.BiotaPropertiesEmoteAction.Add(value2);
                            else
                            {
                                //existingValue2.EmoteId = value2.EmoteId;
                                existingValue2.Order = (uint)value.PropertiesEmoteAction.IndexOf(value2);
                                existingValue2.Type = value2.Type;
                                existingValue2.Delay = value2.Delay;
                                existingValue2.Extent = value2.Extent;
                                existingValue2.Motion = value2.Motion;
                                existingValue2.Message = value2.Message;
                                existingValue2.TestString = value2.TestString;
                                existingValue2.Min = value2.Min;
                                existingValue2.Max = value2.Max;
                                existingValue2.Min64 = value2.Min64;
                                existingValue2.Max64 = value2.Max64;
                                existingValue2.MinDbl = value2.MinDbl;
                                existingValue2.MaxDbl = value2.MaxDbl;
                                existingValue2.Stat = value2.Stat;
                                existingValue2.Display = value2.Display;
                                existingValue2.Amount = value2.Amount;
                                existingValue2.Amount64 = value2.Amount64;
                                existingValue2.HeroXP64 = value2.HeroXP64;
                                existingValue2.Percent = value2.Percent;
                                existingValue2.SpellId = value2.SpellId;
                                existingValue2.WealthRating = value2.WealthRating;
                                existingValue2.TreasureClass = value2.TreasureClass;
                                existingValue2.TreasureType = value2.TreasureType;
                                existingValue2.PScript = value2.PScript;
                                existingValue2.Sound = value2.Sound;
                                existingValue2.DestinationType = value2.DestinationType;
                                existingValue2.WeenieClassId = value2.WeenieClassId;
                                existingValue2.StackSize = value2.StackSize;
                                existingValue2.Palette = value2.Palette;
                                existingValue2.Shade = value2.Shade;
                                existingValue2.TryToBond = value2.TryToBond;
                                existingValue2.ObjCellId = value2.ObjCellId;
                                existingValue2.OriginX = value2.OriginX;
                                existingValue2.OriginY = value2.OriginY;
                                existingValue2.OriginZ = value2.OriginZ;
                                existingValue2.AnglesW = value2.AnglesW;
                                existingValue2.AnglesX = value2.AnglesX;
                                existingValue2.AnglesY = value2.AnglesY;
                                existingValue2.AnglesZ = value2.AnglesZ;
                            }
                        }
                        foreach (var value2 in value.PropertiesEmoteAction)
                        {
                            if (!existingValue.BiotaPropertiesEmoteAction.Any(p => p.Id == value2.Id))
                                context.BiotaPropertiesEmoteAction.Remove(value2);
                        }
                    }
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesEmote)
            {
                if (biota.PropertiesEmote == null || !biota.PropertiesEmote.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesEmote.Remove(value);
            }*/

            if (biota.PropertiesEventFilter != null)
            {
                foreach (var value in biota.PropertiesEventFilter)
                {
                    BiotaPropertiesEventFilter existingValue = existingBiota.BiotaPropertiesEventFilter.FirstOrDefault(r => r.Event == value);

                    if (existingValue == null)
                    {
                        var entity = new BiotaPropertiesEventFilter { ObjectId = biota.Id, Event = value };

                        existingBiota.BiotaPropertiesEventFilter.Add(entity);
                    }
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesEventFilter)
            {
                if (biota.PropertiesEventFilter == null || !biota.PropertiesEventFilter.Any(p => p == value.Event))
                    context.BiotaPropertiesEventFilter.Remove(value);
            }

            /*if (biota.PropertiesGenerator != null)
            {
                foreach (var value in biota.PropertiesGenerator)
                {
                    BiotaPropertiesGenerator existingValue = existingBiota.BiotaPropertiesGenerator.FirstOrDefault(r => r.Id == value.Id);

                    if (existingValue == null)
                        existingBiota.BiotaPropertiesGenerator.Add(value);
                    else
                    {
                        existingValue.Probability = value.Probability;
                        existingValue.WeenieClassId = value.WeenieClassId;
                        existingValue.Delay = value.Delay;
                        existingValue.InitCreate = value.InitCreate;
                        existingValue.MaxCreate = value.MaxCreate;
                        existingValue.WhenCreate = value.WhenCreate;
                        existingValue.WhereCreate = value.WhereCreate;
                        existingValue.StackSize = value.StackSize;
                        existingValue.PaletteId = value.PaletteId;
                        existingValue.Shade = value.Shade;
                        existingValue.ObjCellId = value.ObjCellId;
                        existingValue.OriginX = value.OriginX;
                        existingValue.OriginY = value.OriginY;
                        existingValue.OriginZ = value.OriginZ;
                        existingValue.AnglesW = value.AnglesW;
                        existingValue.AnglesX = value.AnglesX;
                        existingValue.AnglesY = value.AnglesY;
                        existingValue.AnglesZ = value.AnglesZ;
                    }
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesGenerator)
            {
                if (biota.PropertiesGenerator == null || !biota.PropertiesGenerator.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesGenerator.Remove(value);
            }*/


            // Properties for creatures

            if (biota.PropertiesAttribute != null)
            {
                foreach (var kvp in biota.PropertiesAttribute)
                {
                    BiotaPropertiesAttribute existingValue = existingBiota.BiotaPropertiesAttribute.FirstOrDefault(r => r.Type == (ushort)kvp.Key);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesAttribute { ObjectId = biota.Id };

                        existingBiota.BiotaPropertiesAttribute.Add(existingValue);
                    }

                    existingValue.Type = (ushort)kvp.Key;
                    existingValue.InitLevel = kvp.Value.InitLevel;
                    existingValue.LevelFromCP = kvp.Value.LevelFromCP;
                    existingValue.CPSpent = kvp.Value.CPSpent;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesAttribute)
            {
                if (biota.PropertiesAttribute == null || !biota.PropertiesAttribute.ContainsKey((PropertyAttribute)value.Type))
                    context.BiotaPropertiesAttribute.Remove(value);
            }

            if (biota.PropertiesAttribute2nd != null)
            {
                foreach (var kvp in biota.PropertiesAttribute2nd)
                {
                    BiotaPropertiesAttribute2nd existingValue = existingBiota.BiotaPropertiesAttribute2nd.FirstOrDefault(r => r.Type == (ushort)kvp.Key);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesAttribute2nd { ObjectId = biota.Id };

                        existingBiota.BiotaPropertiesAttribute2nd.Add(existingValue);
                    }

                    existingValue.Type = (ushort)kvp.Key;
                    existingValue.InitLevel = kvp.Value.InitLevel;
                    existingValue.LevelFromCP = kvp.Value.LevelFromCP;
                    existingValue.CPSpent = kvp.Value.CPSpent;
                    existingValue.CurrentLevel = kvp.Value.CurrentLevel;
                }

            }
            foreach (var value in existingBiota.BiotaPropertiesAttribute2nd)
            {
                if (biota.PropertiesAttribute2nd == null || !biota.PropertiesAttribute2nd.ContainsKey((PropertyAttribute2nd)value.Type))
                    context.BiotaPropertiesAttribute2nd.Remove(value);
            }

            if (biota.PropertiesBodyPart != null)
            {
                foreach (var kvp in biota.PropertiesBodyPart)
                {
                    BiotaPropertiesBodyPart existingValue = existingBiota.BiotaPropertiesBodyPart.FirstOrDefault(r => r.Key == (uint)kvp.Key);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesBodyPart { ObjectId = biota.Id };

                        existingBiota.BiotaPropertiesBodyPart.Add(existingValue);
                    }

                    existingValue.Key = (ushort)kvp.Key;
                    existingValue.DType = (int)kvp.Value.DType;
                    existingValue.DVal = kvp.Value.DVal;
                    existingValue.DVar = kvp.Value.DVar;
                    existingValue.BaseArmor = kvp.Value.BaseArmor;
                    existingValue.ArmorVsSlash = kvp.Value.ArmorVsSlash;
                    existingValue.ArmorVsPierce = kvp.Value.ArmorVsPierce;
                    existingValue.ArmorVsBludgeon = kvp.Value.ArmorVsBludgeon;
                    existingValue.ArmorVsCold = kvp.Value.ArmorVsCold;
                    existingValue.ArmorVsFire = kvp.Value.ArmorVsFire;
                    existingValue.ArmorVsAcid = kvp.Value.ArmorVsAcid;
                    existingValue.ArmorVsElectric = kvp.Value.ArmorVsElectric;
                    existingValue.ArmorVsNether = kvp.Value.ArmorVsNether;
                    existingValue.BH = kvp.Value.BH;
                    existingValue.HLF = kvp.Value.HLF;
                    existingValue.MLF = kvp.Value.MLF;
                    existingValue.LLF = kvp.Value.LLF;
                    existingValue.HRF = kvp.Value.HRF;
                    existingValue.MRF = kvp.Value.MRF;
                    existingValue.LRF = kvp.Value.LRF;
                    existingValue.HLB = kvp.Value.HLB;
                    existingValue.MLB = kvp.Value.MLB;
                    existingValue.LLB = kvp.Value.LLB;
                    existingValue.HRB = kvp.Value.HRB;
                    existingValue.MRB = kvp.Value.MRB;
                    existingValue.LRB = kvp.Value.LRB;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesBodyPart)
            {
                if (biota.PropertiesBodyPart == null || !biota.PropertiesBodyPart.ContainsKey((CombatBodyPart)value.Key))
                    context.BiotaPropertiesBodyPart.Remove(value);
            }

            if (biota.PropertiesSkill != null)
            {
                foreach (var kvp in biota.PropertiesSkill)
                {
                    BiotaPropertiesSkill existingValue = existingBiota.BiotaPropertiesSkill.FirstOrDefault(r => r.Type == (ushort)kvp.Key);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesSkill { ObjectId = biota.Id };

                        existingBiota.BiotaPropertiesSkill.Add(existingValue);
                    }

                    existingValue.Type = (ushort)kvp.Key;
                    existingValue.LevelFromPP = kvp.Value.LevelFromPP;
                    existingValue.SAC = (uint)kvp.Value.SAC;
                    existingValue.PP = kvp.Value.PP;
                    existingValue.InitLevel = kvp.Value.InitLevel;
                    existingValue.ResistanceAtLastCheck = kvp.Value.ResistanceAtLastCheck;
                    existingValue.LastUsedTime = kvp.Value.LastUsedTime;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesSkill)
            {
                if (biota.PropertiesSkill == null || !biota.PropertiesSkill.ContainsKey((Skill)value.Type))
                    context.BiotaPropertiesSkill.Remove(value);
            }


            // Properties for books

            if (biota.PropertiesBook != null)
            {
                if (existingBiota.BiotaPropertiesBook == null)
                    existingBiota.BiotaPropertiesBook = new BiotaPropertiesBook { ObjectId = biota.Id, };

                existingBiota.BiotaPropertiesBook.MaxNumPages = biota.PropertiesBook.MaxNumPages;
                existingBiota.BiotaPropertiesBook.MaxNumCharsPerPage = biota.PropertiesBook.MaxNumCharsPerPage;
            }
            else
            {
                if (existingBiota.BiotaPropertiesBook != null)
                    context.BiotaPropertiesBook.Remove(existingBiota.BiotaPropertiesBook);
            }

            if (biota.PropertiesBookPageData != null)
            {
                for (int i = 0; i < biota.PropertiesBookPageData.Count; i++)
                {
                    var value = biota.PropertiesBookPageData[i];

                    BiotaPropertiesBookPageData existingValue = existingBiota.BiotaPropertiesBookPageData.FirstOrDefault(r => r.PageId == i);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesBookPageData { ObjectId = biota.Id };

                        existingBiota.BiotaPropertiesBookPageData.Add(existingValue);
                    }

                    existingValue.PageId = (uint)i;
                    existingValue.AuthorId = value.AuthorId;
                    existingValue.AuthorName = value.AuthorName;
                    existingValue.AuthorAccount = value.AuthorAccount;
                    existingValue.IgnoreAuthor = value.IgnoreAuthor;
                    existingValue.PageText = value.PageText;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesBookPageData)
            {
                if (biota.PropertiesBookPageData == null || value.PageId >= biota.PropertiesBookPageData.Count)
                    context.BiotaPropertiesBookPageData.Remove(value);
            }


            // Biota additions over Weenie

            if (biota.PropertiesAllegiance != null)
            {
                foreach (var value in biota.PropertiesAllegiance)
                {
                    BiotaPropertiesAllegiance existingValue = existingBiota.BiotaPropertiesAllegiance.FirstOrDefault(r => r.CharacterId == value.CharacterId);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesAllegiance { AllegianceId = biota.Id };

                        existingBiota.BiotaPropertiesAllegiance.Add(existingValue);
                    }

                    existingValue.CharacterId = value.CharacterId;
                    existingValue.Banned = value.Banned;
                    existingValue.ApprovedVassal = value.ApprovedVassal;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesAllegiance)
            {
                if (biota.PropertiesAllegiance == null || !biota.PropertiesAllegiance.Any(p => p.CharacterId == value.CharacterId))
                    context.BiotaPropertiesAllegiance.Remove(value);
            }

            if (biota.PropertiesEnchantmentRegistry != null)
            {
                foreach (var value in biota.PropertiesEnchantmentRegistry)
                {
                    BiotaPropertiesEnchantmentRegistry existingValue = existingBiota.BiotaPropertiesEnchantmentRegistry.FirstOrDefault(r => r.SpellId == value.SpellId && r.LayerId == value.LayerId && r.CasterObjectId == value.CasterObjectId);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesEnchantmentRegistry { ObjectId = biota.Id };

                        existingBiota.BiotaPropertiesEnchantmentRegistry.Add(existingValue);
                    }

                    existingValue.EnchantmentCategory = value.EnchantmentCategory;
                    existingValue.SpellId = value.SpellId;
                    existingValue.LayerId = value.LayerId;
                    existingValue.HasSpellSetId = value.HasSpellSetId;
                    existingValue.SpellCategory = (ushort)value.SpellCategory;
                    existingValue.PowerLevel = value.PowerLevel;
                    existingValue.StartTime = value.StartTime;
                    existingValue.Duration = value.Duration;
                    existingValue.CasterObjectId = value.CasterObjectId;
                    existingValue.DegradeModifier = value.DegradeModifier;
                    existingValue.DegradeLimit = value.DegradeLimit;
                    existingValue.LastTimeDegraded = value.LastTimeDegraded;
                    existingValue.StatModType = (uint)value.StatModType;
                    existingValue.StatModKey = value.StatModKey;
                    existingValue.StatModValue = value.StatModValue;
                    existingValue.SpellSetId = (uint)value.SpellSetId;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesEnchantmentRegistry)
            {
                if (biota.PropertiesEnchantmentRegistry == null || !biota.PropertiesEnchantmentRegistry.Any(p => p.SpellId == value.SpellId && p.LayerId == value.LayerId && p.CasterObjectId == value.CasterObjectId))
                    context.BiotaPropertiesEnchantmentRegistry.Remove(value);
            }

            /*if (biota.HousePermissions != null)
            {
                foreach (var value in biota.HousePermissions)
                {
                    Models.Shard.HousePermission existingValue = existingBiota.HousePermission.FirstOrDefault(r => r.HouseId == value.HouseId && r.PlayerGuid == value.PlayerGuid);

                    if (existingValue == null)
                        existingBiota.HousePermission.Add(value);
                    else
                        existingValue.Storage = value.Storage;
                }
            }
            foreach (var value in existingBiota.HousePermission)
            {
                if ((biota.HousePermissions == null || !biota.HousePermissions.Any(p => p.HouseId == value.HouseId && p.PlayerGuid == value.PlayerGuid))
                    context.HousePermission.Remove(value);
            }*/
        }

        public override bool RemoveBiota(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock)
        {
            using (var context = new ShardDbContext())
            {
                var existingBiota = context.Biota
                    .AsNoTracking()
                    .FirstOrDefault(r => r.Id == biota.Id);

                if (existingBiota == null)
                    return true;

                rwLock.EnterWriteLock();
                try
                {
                    context.Biota.Remove(existingBiota);

                    Exception firstException = null;
                    retry:

                    try
                    {
                        context.SaveChanges();

                        if (firstException != null)
                            log.Debug($"[DATABASE] RemoveBiota 0x{biota.Id:X8}:{biota.GetName()} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (firstException == null)
                        {
                            firstException = ex;
                            goto retry;
                        }

                        // Character name might be in use or some other fault
                        log.Error($"[DATABASE] RemoveBiota 0x{biota.Id:X8}:{biota.GetName()} failed first attempt with exception: {firstException}");
                        log.Error($"[DATABASE] RemoveBiota 0x{biota.Id:X8}:{biota.GetName()} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitWriteLock();
                }
            }
        }
    }
}
