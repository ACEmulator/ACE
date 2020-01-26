using System;
using System.Runtime.CompilerServices;
using System.Threading;

using log4net;

using ACE.Common.Extensions;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum.Properties;

namespace ACE.Database
{
    public class ShardDatabaseWithCaching : ShardDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ConditionalWeakTable<Biota, ShardDbContext> BiotaContexts = new ConditionalWeakTable<Biota, ShardDbContext>();

        public override Biota GetBiota(uint id)
        {
            var context = new ShardDbContext();

            var biota = GetBiota(context, id);

            if (biota != null)
                BiotaContexts.Add(biota, context);

            return biota;
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

            // If we got here, the biota didn't come from the database through this class.
            // Most likely, it doesn't exist in the database, so, no need to remove.
            return true;
        }
    }
}
