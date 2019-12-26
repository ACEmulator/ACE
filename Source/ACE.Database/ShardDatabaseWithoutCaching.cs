using System;
using System.Linq;
using System.Threading;

using Microsoft.EntityFrameworkCore;

using log4net;

using ACE.Common.Extensions;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum.Properties;

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

        public override Biota GetBiota(uint id)
        {
            if (ObjectGuid.IsPlayer(id))
            {
                var context = new ShardDbContext();

                var biota = GetBiota(context, id);

                if (biota != null)
                    BiotaContexts.Add(biota, context);

                return biota;
            }

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetBiota(context, id);
            }
        }

        public override bool SaveBiota(Biota biota, ReaderWriterLockSlim rwLock)
        {
            if (BiotaContexts.TryGetValue(biota, out var cachedContext))
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
                            log.Debug($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

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
                        log.Error($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                        log.Error($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }

            if (ObjectGuid.IsPlayer(biota.Id))
            {
                var context = new ShardDbContext();

                BiotaContexts.Add(biota, context);

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
                            log.Debug($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

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
                        log.Error($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                        log.Error($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }

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
                            log.Debug($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

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
                        log.Error($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                        log.Error($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }
        }

        private void UpdateBiota(ShardDbContext context, Biota existingBiota, Biota biota)
        {
            // This pattern is described here: https://docs.microsoft.com/en-us/ef/core/saving/disconnected-entities
            // You'll notice though that we're not using the recommended: context.Entry(existingEntry).CurrentValues.SetValues(newEntry);
            // It is EXTREMLY slow. 4x or more slower. I suspect because it uses reflection to find the properties that the object contains
            // Manually setting the properties like we do below is the best case scenario for performance. However, it also has risks.
            // If we add columns to the schema and forget to add those changes here, changes to the biota may not propegate to the database.
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
                BiotaPropertiesAttribute existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesAttribute.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesAttribute.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.InitLevel = value.InitLevel;
                    existingValue.LevelFromCP = value.LevelFromCP;
                    existingValue.CPSpent = value.CPSpent;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesAttribute)
            {
                if (!biota.BiotaPropertiesAttribute.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesAttribute.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesAttribute2nd)
            {
                BiotaPropertiesAttribute2nd existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesAttribute2nd.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesAttribute2nd.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.InitLevel = value.InitLevel;
                    existingValue.LevelFromCP = value.LevelFromCP;
                    existingValue.CPSpent = value.CPSpent;
                    existingValue.CurrentLevel = value.CurrentLevel;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesAttribute2nd)
            {
                if (!biota.BiotaPropertiesAttribute2nd.Any(p => p.Id == value.Id))
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
                BiotaPropertiesBool existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesBool.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesBool.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesBool)
            {
                if (!biota.BiotaPropertiesBool.Any(p => p.Id == value.Id))
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
                BiotaPropertiesDID existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesDID.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesDID.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesDID)
            {
                if (!biota.BiotaPropertiesDID.Any(p => p.Id == value.Id))
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
                BiotaPropertiesEnchantmentRegistry existingValue = (value.ObjectId == 0 ? null : existingBiota.BiotaPropertiesEnchantmentRegistry.FirstOrDefault(r => r.ObjectId == value.ObjectId && r.SpellId == value.SpellId && r.LayerId == value.LayerId && r.CasterObjectId == value.CasterObjectId));

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
                if (!biota.BiotaPropertiesEnchantmentRegistry.Any(p => p.ObjectId == value.ObjectId && p.SpellId == value.SpellId && p.LayerId == value.LayerId && p.CasterObjectId == value.CasterObjectId))
                    context.BiotaPropertiesEnchantmentRegistry.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesEventFilter)
            {
                BiotaPropertiesEventFilter existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesEventFilter.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesEventFilter.Add(value);
                else
                {
                    existingValue.Event = value.Event;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesEventFilter)
            {
                if (!biota.BiotaPropertiesEventFilter.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesEventFilter.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesFloat)
            {
                BiotaPropertiesFloat existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesFloat.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesFloat.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesFloat)
            {
                if (!biota.BiotaPropertiesFloat.Any(p => p.Id == value.Id))
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
                BiotaPropertiesIID existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesIID.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesIID.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesIID)
            {
                if (!biota.BiotaPropertiesIID.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesIID.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesInt)
            {
                BiotaPropertiesInt existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesInt.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesInt.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesInt)
            {
                if (!biota.BiotaPropertiesInt.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesInt.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesInt64)
            {
                BiotaPropertiesInt64 existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesInt64.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesInt64.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesInt64)
            {
                if (!biota.BiotaPropertiesInt64.Any(p => p.Id == value.Id))
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
                BiotaPropertiesPosition existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesPosition.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesPosition.Add(value);
                else
                {
                    existingValue.PositionType = value.PositionType;
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
                if (!biota.BiotaPropertiesPosition.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesPosition.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesSkill)
            {
                BiotaPropertiesSkill existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesSkill.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesSkill.Add(value);
                else
                {
                    existingValue.Type = value.Type;
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
                if (!biota.BiotaPropertiesSkill.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesSkill.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesSpellBook)
            {
                BiotaPropertiesSpellBook existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesSpellBook.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesSpellBook.Add(value);
                else
                {
                    existingValue.Spell = value.Spell;
                    existingValue.Probability = value.Probability;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesSpellBook)
            {
                if (!biota.BiotaPropertiesSpellBook.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesSpellBook.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesString)
            {
                BiotaPropertiesString existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesString.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesString.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesString)
            {
                if (!biota.BiotaPropertiesString.Any(p => p.Id == value.Id))
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
                HousePermission existingValue = (value.Id == 0 ? null : existingBiota.HousePermission.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.HousePermission.Add(value);
                else
                {
                    existingValue.PlayerGuid = value.PlayerGuid;
                    existingValue.Storage = value.Storage;
                }
            }
            foreach (var value in existingBiota.HousePermission)
            {
                if (!biota.HousePermission.Any(p => p.Id == value.Id))
                    context.HousePermission.Remove(value);
            }
        }

        public override bool RemoveBiota(Biota biota, ReaderWriterLockSlim rwLock)
        {
            if (BiotaContexts.TryGetValue(biota, out var cachedContext))
            {
                BiotaContexts.Remove(biota);

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
                            log.Debug($"RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

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
                        log.Error($"RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                        log.Error($"RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
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
                        context.Biota.Remove(biota);

                        Exception firstException = null;
                        retry:

                        try
                        {
                            context.SaveChanges();

                            if (firstException != null)
                                log.Debug($"RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

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
                            log.Error($"RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                            log.Error($"RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
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
    }
}
