using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using log4net;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database
{
    public static class ShardDatabaseOfflineTools
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /*public static bool CanPurgeCharacter(ShardDbContext context, uint characterId)
        {
            var result = context.Character
                .FirstOrDefault(r => r.Id == characterId);

            if (result != null && (result.DeleteTime > 0 || result.IsDeleted))
                return true;

            return false;
        }

        public static bool CanPurgeCharacter(uint characterId)
        {
            using (var context = new ShardDbContext())
                return CanPurgeCharacter(context, characterId);
        }*/


        private static List<Biota> GetInventoryBiotas(ShardDbContext context, uint parentId, bool includedNestedItems)
        {
            var inventory = new List<Biota>();

            var results = context.BiotaPropertiesIID
                .Where(r => r.Type == (ushort)PropertyInstanceId.Container && r.Value == parentId)
                .Include(r => r.Object)
                .ToList();

            foreach (var result in results)
            {
                inventory.Add(result.Object);

                if (includedNestedItems && result.Object.WeenieType == (int)WeenieType.Container)
                {
                    var subItems = GetInventoryBiotas(context, result.ObjectId, false);

                    inventory.AddRange(subItems);
                }
            }

            return inventory;
        }

        private static List<uint> GetWieldedGuids(ShardDbContext context, uint parentId)
        {
            return context.BiotaPropertiesIID
                .Where(r => r.Type == (ushort)PropertyInstanceId.Wielder && r.Value == parentId)
                .AsNoTracking()
                .Select(r => r.ObjectId)
                .ToList();
        }


        public static void PurgeCharacter(ShardDbContext context, uint characterId, out int charactersPurged, out int playerBiotasPurged, out int possessionsPurged, string reason = null)
        {
            charactersPurged = 0;
            playerBiotasPurged = 0;
            possessionsPurged = 0;

            // First purge the inventory
            var inventoryBiotas = GetInventoryBiotas(context, characterId, true);

            foreach (var biota in inventoryBiotas)
            {
                context.Biota.Remove(biota);

                possessionsPurged++;
            }

            // Then the wielded items
            var wieldedGuids = GetWieldedGuids(context, characterId);

            foreach (var guid in wieldedGuids)
            {
                var stub = new Biota { Id = guid };
                context.Biota.Attach(stub);
                context.Biota.Remove(stub);

                possessionsPurged++;
            }

            // Second to last, the payer biota
            if (context.Biota.Any(r => r.Id == characterId))
            {
                var stub = new Biota { Id = characterId };
                context.Biota.Attach(stub);
                context.Biota.Remove(stub);

                playerBiotasPurged++;
            }

            // Lastly, the character record
            var character = context.Character.FirstOrDefault(r => r.Id == characterId);

            if (character != null)
            {
                context.Character.Remove(character);

                charactersPurged++;
            }

            var message = $"[DATABASE][PURGE] Character 0x{characterId:X8}";
            if (character != null)
               message += $":{character.Name}, deleted on {Common.Time.GetDateTimeFromTimestamp(character.DeleteTime).ToLocalTime()}";
            message += $", and {possessionsPurged} of their possessions has been purged.";
            if (!String.IsNullOrWhiteSpace(reason))
                message += $" Reason: {reason}.";
            log.Debug(message);

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Error($"[DATABASE][PURGE] PurgeCharacter 0x{characterId:X8} failed with exception: {ex}");
            }
        }

        public static void PurgeCharacter(uint characterId, out int charactersPurged, out int playerBiotasPurged, out int possessionsPurged, string reason = null)
        {
            using (var context = new ShardDbContext())
                PurgeCharacter(context, characterId, out charactersPurged, out playerBiotasPurged, out possessionsPurged, reason);
        }

        public static void PurgeCharactersInParallel(ShardDbContext context, int daysLimiter, out int charactersPurged, out int playerBiotasPurged, out int possessionsPurged)
        {
            var deleteLimit = Common.Time.GetUnixTime(DateTime.UtcNow.AddDays(-daysLimiter));

            var results = context.Character
                .Where(r => (r.DeleteTime > 0 && r.DeleteTime < (ulong)deleteLimit) || (r.IsDeleted && r.DeleteTime == 0))
                .AsNoTracking()
                .ToList();

            int charactersPurgedTotal = 0;
            int playerBiotasPurgedTotal = 0;
            int possessionsPurgedTotal = 0;

            Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
            {
                PurgeCharacter(result.Id, out var charactersPurgedResult, out var playerBiotasPurgedResult, out var possessionsPurgedResult);

                Interlocked.Add(ref charactersPurgedTotal, charactersPurgedResult);
                Interlocked.Add(ref playerBiotasPurgedTotal, playerBiotasPurgedResult);
                Interlocked.Add(ref possessionsPurgedTotal, possessionsPurgedResult);
            });

            charactersPurged = charactersPurgedTotal;
            playerBiotasPurged = playerBiotasPurgedTotal;
            possessionsPurged = possessionsPurgedTotal;
        }

        public static void PurgeCharactersInParallel(int daysLimiter, out int charactersPurged, out int playerBiotasPurged, out int possessionsPurged)
        {
            using (var context = new ShardDbContext())
                PurgeCharactersInParallel(context, daysLimiter, out charactersPurged, out playerBiotasPurged, out possessionsPurged);
        }


        public static void PurgePlayer(ShardDbContext context, uint playerId, out int charactersPurged, out int playerBiotasPurged, out int possessionsPurged, string reason = null)
        {
            charactersPurged = 0;
            playerBiotasPurged = 0;
            possessionsPurged = 0;

            // First purge the inventory
            var inventoryBiotas = GetInventoryBiotas(context, playerId, true);

            foreach (var biota in inventoryBiotas)
            {
                context.Biota.Remove(biota);

                possessionsPurged++;
            }

            // Then the wielded items
            var wieldedGuids = GetWieldedGuids(context, playerId);

            foreach (var guid in wieldedGuids)
            {
                var stub = new Biota { Id = guid };
                context.Biota.Attach(stub);
                context.Biota.Remove(stub);

                possessionsPurged++;
            }

            // Second to last, the character record
            if (context.Character.Any(r => r.Id == playerId))
            {
                var stub = new Character { Id = playerId };
                context.Character.Attach(stub);
                context.Character.Remove(stub);

                charactersPurged++;
            }

            // Lastly, the payer biota
            var player = context.Biota.Include(r => r.BiotaPropertiesString).FirstOrDefault(r => r.Id == playerId);

            if (player != null)
            {
                context.Biota.Remove(player);

                playerBiotasPurged++;
            }

            var message = $"[DATABASE][PURGE] Player 0x{playerId:X8}";
            if (player != null)
            {
                var name = player.GetProperty(PropertyString.Name);
                if (!String.IsNullOrWhiteSpace(name))
                    message += $":{name}";
            }
            message += $", and {possessionsPurged} of their possessions has been purged.";
            if (!String.IsNullOrWhiteSpace(reason))
                message += $" Reason: {reason}.";
            log.Debug(message);

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Error($"[DATABASE][PURGE] PurgePlayer 0x{playerId:X8} failed with exception: {ex}");
            }
        }

        public static void PurgePlayer(uint playerId, out int charactersPurged, out int playerBiotasPurged, out int possessionsPurged, string reason = null)
        {
            using (var context = new ShardDbContext())
                PurgePlayer(context, playerId, out charactersPurged, out playerBiotasPurged, out possessionsPurged, reason);
        }


        //public static readonly HashSet<WeenieType> NonPurgeableWeenieTypes = new HashSet<WeenieType>
        //{
        //    WeenieType.Allegiance,
        //};

        public static bool PurgeBiota(ShardDbContext context, uint id, string reason = null)
        {
            var biota = context.Biota
                .Include(r => r.BiotaPropertiesString)
                .FirstOrDefault(r => r.Id == id);

            if (biota != null)
            {
                var message = $"[DATABASE][PURGE] Biota 0x{id:X8}";
                var name = biota.GetProperty(PropertyString.Name);
                if (!String.IsNullOrWhiteSpace(name))
                    message += $":{name}";
                message += $", WeenieType: {(WeenieType)biota.WeenieType}";

                //if (NonPurgeableWeenieTypes.Contains((WeenieType)biota.WeenieType))
                //{
                //    message += ", has NOT been purged due to non-purgeable WeenieType.";
                //    if (!String.IsNullOrWhiteSpace(reason))
                //        message += $" Reason: {reason}.";
                //    log.Debug(message);

                //    return false;
                //}

                message += $", has been purged.";
                if (!String.IsNullOrWhiteSpace(reason))
                    message += $" Reason: {reason}.";
                log.Debug(message);

                context.Biota.Remove(biota);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    log.Error($"[DATABASE][PURGE] PurgeBiota 0x{id:X8} failed with exception: {ex}");
                }

                return true;
            }

            return false;
        }

        public static bool PurgeBiota(uint id, string reason = null)
        {
            using (var context = new ShardDbContext())
                return PurgeBiota(context, id, reason);
        }

        public static void PurgeOrphanedBiotasInParallel(ShardDbContext context, out int numberOfBiotasPurged)
        {
            int totalNumberOfBiotasPurged = 0;

            // Purge characters that do not have an associated biota
            {
                // select * from `character` left join biota on biota.id=`character`.id where biota.id is null;

                /* EF Core 2.2.6 method
                var query = from character in context.Character
                            join biota in context.Biota on character.Id equals biota.Id into combined
                            from b in combined.DefaultIfEmpty()
                            where b == null
                            select new
                            {
                                id = character.Id,
                            };

                // SELECT `character`.`id`
                // FROM `character` AS `character`
                // LEFT JOIN `biota` AS `biota` ON `character`.`id` = `biota`.`id`
                // WHERE `biota`.`id` IS NULL

                var results = query.ToList();
                */

                var playerBiotaIds = context.Biota.Select(r => r.Id).Where(id => id >= 0x50000000 && id <= 0x5FFFFFFF).ToList();
                var characterIds = context.Character.Select(r => r.Id).ToList();

                var results = characterIds.Except(playerBiotaIds);

                Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
                {
                    PurgeCharacter(result, out var charactersPurged, out var playerBiotasPurged, out var possessionPurged, "No Player biota counterpart found");

                    if (charactersPurged != 1)
                        log.Error("[DATABASE][PURGE] PurgeOrphanedBiotasInParallel failed to purge exactly 1 character. This should not happen!");

                    if (playerBiotasPurged != 0)
                        log.Error("[DATABASE][PURGE] PurgeOrphanedBiotasInParallel purged a player biota and character record. This should not happen!");

                    Interlocked.Add(ref totalNumberOfBiotasPurged, charactersPurged);
                    Interlocked.Add(ref totalNumberOfBiotasPurged, playerBiotasPurged);
                    Interlocked.Add(ref totalNumberOfBiotasPurged, possessionPurged);
                });
            }

            // Purge player biotas that do not have an associated character
            {
                // select * from biota left join `character` on character.id=biota.id where biota.id >= 0x50000000 and biota.id <= 0x5FFFFFFF and character.id is null;

                /* EF Core 2.2.6 method
                var query = from biota in context.Biota
                            join character in context.Character on biota.Id equals character.Id into combined
                            where biota.Id >= 0x50000000 && biota.Id <= 0x5FFFFFFF
                            from c in combined.DefaultIfEmpty()
                            where c == null
                            select new
                            {
                                id = biota.Id,
                            };

                // SELECT `biota`.`id` AS `id0`, `biota`.`populated_Collection_Flags`, `biota`.`weenie_Class_Id`, `biota`.`weenie_Type`, `character`.`id`, `character`.`account_Id`, `character`.`character_Options_1`, `character`.`character_Options_2`, `character`.`default_Hair_Texture`, `character`.`delete_Time`, `character`.`gameplay_Options`, `character`.`hair_Texture`, `character`.`is_Deleted`, `character`.`is_Plussed`, `character`.`last_Login_Timestamp`, `character`.`name`, `character`.`spellbook_Filters`, `character`.`total_Logins`
                // FROM `biota` AS `biota`
                // LEFT JOIN `character` AS `character` ON `biota`.`id` = `character`.`id`
                // WHERE ((`biota`.`id` >= 1342177280) AND (`biota`.`id` <= 1610612735)) AND `character`.`id` IS NULL
                // ORDER BY `id0`

                var results = query.ToList();
                */

                var playerBiotaIds = context.Biota.Select(r => r.Id).Where(id => id >= 0x50000000 && id <= 0x5FFFFFFF).ToList();
                var characterIds = context.Character.Select(r => r.Id).ToList();

                var results = playerBiotaIds.Except(characterIds);

                Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
                {
                    PurgePlayer(result, out var charactersPurged, out var playerBiotasPurged, out var possessionPurged, "No Character record counterpart found");

                    if (charactersPurged != 0)
                        log.Error("[DATABASE][PURGE] PurgeOrphanedBiotasInParallel purged a character record and a player biota. This should not happen!");

                    if (playerBiotasPurged != 1)
                        log.Error("[DATABASE][PURGE] PurgeOrphanedBiotasInParallel failed to purge exactly 1 player biota. This should not happen!");

                    Interlocked.Add(ref totalNumberOfBiotasPurged, charactersPurged);
                    Interlocked.Add(ref totalNumberOfBiotasPurged, playerBiotasPurged);
                    Interlocked.Add(ref totalNumberOfBiotasPurged, possessionPurged);
                });
            }

            // Purge contained items that belong to a parent container that no longer exists
            {
                // select * from biota_properties_i_i_d iid left join biota on biota.id=iid.`value` where iid.`type`=2 and biota.id is null;

                /* EF Core 2.2.6 method
                var query = from iid in context.BiotaPropertiesIID
                            join biota in context.Biota on iid.Value equals biota.Id into combined
                            where iid.Type == (ushort)PropertyInstanceId.Container
                            from b in combined.DefaultIfEmpty()
                            where b == null
                            select new
                            {
                                id = iid.ObjectId,
                            };

                // SELECT `iid`.`id`, `iid`.`object_Id` AS `id0`, `iid`.`type`, `iid`.`value`, `biota`.`id`, `biota`.`populated_Collection_Flags`, `biota`.`weenie_Class_Id`, `biota`.`weenie_Type`
                // FROM `biota_properties_i_i_d` AS `iid`
                // LEFT JOIN `biota` AS `biota` ON `iid`.`value` = `biota`.`id`
                // WHERE (`iid`.`type` = 2) AND `biota`.`id` IS NULL
                // ORDER BY `iid`.`value`

                var results = query.ToList();
                */

                var biotaIds = context.Biota.Select(r => r.Id).ToList();
                var iidRecords = context.BiotaPropertiesIID.Where(r => r.Type == (ushort)PropertyInstanceId.Container).ToList();

                var results = iidRecords.Where(r => !biotaIds.Contains(r.Value));

                Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
                {
                    if (PurgeBiota(result.ObjectId, "Parent container not found"))
                        Interlocked.Increment(ref totalNumberOfBiotasPurged);
                });

            }

            // Purge wielded items that belong to a parent container that no longer exists
            {
                // select * from biota_properties_i_i_d iid left join biota on biota.id=iid.`value` where iid.`type`=3 and biota.id is null;

                /* EF Core 2.2.6 method
                var query = from iid in context.BiotaPropertiesIID
                            join biota in context.Biota on iid.Value equals biota.Id into combined
                            where iid.Type == (ushort)PropertyInstanceId.Wielder
                            from b in combined.DefaultIfEmpty()
                            where b == null
                            select new
                            {
                                id = iid.ObjectId,
                            };

                // SELECT `iid`.`id`, `iid`.`object_Id` AS `id0`, `iid`.`type`, `iid`.`value`, `biota`.`id`, `biota`.`populated_Collection_Flags`, `biota`.`weenie_Class_Id`, `biota`.`weenie_Type`
                // FROM `biota_properties_i_i_d` AS `iid`
                // LEFT JOIN `biota` AS `biota` ON `iid`.`value` = `biota`.`id`
                // WHERE (`iid`.`type` = 3) AND `biota`.`id` IS NULL
                // ORDER BY `iid`.`value`

                var results = query.ToList();
                */

                var biotaIds = context.Biota.Select(r => r.Id).ToList();
                var iidRecords = context.BiotaPropertiesIID.Where(r => r.Type == (ushort)PropertyInstanceId.Wielder).ToList();

                var results = iidRecords.Where(r => !biotaIds.Contains(r.Value));

                Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
                {
                    if (PurgeBiota(result.ObjectId, "Parent wielder not found"))
                        Interlocked.Increment(ref totalNumberOfBiotasPurged);
                });

            }

            // Purge biotas that don't have a parent Container, Wielder or Location
            {
                var results = context.Biota
                    .Include(r => r.BiotaPropertiesIID)
                    .Include(r => r.BiotaPropertiesPosition)
                    .Where(r => r.WeenieType != (int)WeenieType.Allegiance)
                    .Where(r => r.BiotaPropertiesIID.All(y => y.Type != (ushort)PropertyInstanceId.Container && y.Type != (ushort)PropertyInstanceId.Wielder))
                    .Where(r => r.BiotaPropertiesPosition.All(y => y.PositionType != (ushort)PositionType.Location))
                    .AsNoTracking()
                    .ToList();

                // This is very time consuming
                Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
                {
                    if (PurgeBiota(result.Id, "No parent Container, parent Wielder, or Location"))
                        Interlocked.Increment(ref totalNumberOfBiotasPurged);
                });
            }

            // Purge allegiances biotas that have no valid monarch or are duplicates as a result of double init on first swear
            {
                var monarchs = context.Biota
                    .Include(r => r.BiotaPropertiesIID)
                    .Where(r => r.WeenieType == (int)WeenieType.Creature
                             || r.WeenieType == (int)WeenieType.Admin
                             || r.WeenieType == (int)WeenieType.Sentinel
                    )
                    .AsNoTracking()
                    .ToList()
                    .Select(r => r.GetProperty(PropertyInstanceId.Monarch) ?? 0)
                    .Distinct()
                    .ToList();

                //monarchs = monarchs.OrderBy(r => r).ToList();

                //if (monarchs.Contains(0))
                //    monarchs.Remove(0);

                var allegiances = context.Biota
                    .Include(r => r.BiotaPropertiesIID)
                    .Where(r => r.WeenieType == (int)WeenieType.Allegiance)
                    .AsNoTracking()
                    .ToList();

                //var results = new List<Biota>();
                //foreach (var allegiance in allegiances)
                //{
                //    if (results.Contains(allegiance))
                //        continue;

                //    var monarchIID = allegiance.BiotaPropertiesIID.FirstOrDefault(m => m.Type == (int)PropertyInstanceId.Monarch).Value;
                //    if (!monarchs.Contains(monarchIID))
                //        results.Add(allegiance);
                //    else
                //    {
                //        var x = results.Where(a => a.GetProperty(PropertyInstanceId.Monarch) == monarchIID).FirstOrDefault();
                //        if (x == null)
                //        {
                //            var y = allegiances.Where(a => a.GetProperty(PropertyInstanceId.Monarch) == monarchIID).SkipWhile(a => a.Id == allegiance.Id).ToList();
                //            results.AddRange(y);
                //        }
                //    }
                //}

                //Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
                //{
                //    if (PurgeBiota(result.Id, "Allegiance has no valid monarch or is an unused duplicate"))
                //        Interlocked.Increment(ref totalNumberOfBiotasPurged);
                //});

                var results = new List<Biota>();
                foreach (var allegiance in allegiances)
                {
                    if (results.Contains(allegiance))
                        continue;

                    var monarchIID = allegiance.BiotaPropertiesIID.FirstOrDefault(m => m.Type == (int)PropertyInstanceId.Monarch).Value;

                    if (!monarchs.Contains(monarchIID))
                        results.Add(allegiance);
                }

                Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
                {
                    if (PurgeBiota(result.Id, "Allegiance has no valid monarch"))
                        Interlocked.Increment(ref totalNumberOfBiotasPurged);
                });

                results = new List<Biota>();
                foreach (var allegiance in allegiances)
                {
                    if (results.Contains(allegiance))
                        continue;

                    var monarchIID = allegiance.BiotaPropertiesIID.FirstOrDefault(m => m.Type == (int)PropertyInstanceId.Monarch).Value;

                    //var allegianceToKeep = results.Where(a => a.GetProperty(PropertyInstanceId.Monarch) == monarchIID).FirstOrDefault();
                    //if (allegianceToKeep == null)
                    //{
                        var allegiancesToPurge = allegiances.Where(a => a.GetProperty(PropertyInstanceId.Monarch) == monarchIID).SkipWhile(a => a.Id == allegiance.Id).ToList();
                        results.AddRange(allegiancesToPurge);
                    //}
                }

                Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
                {
                    if (PurgeBiota(result.Id, "Allegiance is an unused duplicate"))
                        Interlocked.Increment(ref totalNumberOfBiotasPurged);
                });
            }

            numberOfBiotasPurged = totalNumberOfBiotasPurged;




            // WIP code, not ready for production

            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // NOTE: Fixing things like Allegiances, houses, etc.. where biotas aren't being pruned, but instead modified and integrity is being validated should be a different function
            // NOTE: Perhaps FixBiotasWithBrokenLinks()
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //var context = new ShardDbContext();

            //var totalNumBiotas = context.Biota.Count();

            ////var results = context.Biota
            ////    .Include(r => r.BiotaPropertiesAllegiance)
            ////    .Include(r => r.BiotaPropertiesIID)
            ////    //.Include(r => r.BiotaPropertiesInt)
            ////    .Include(r => r.BiotaPropertiesPosition)
            ////    .Include(r => r.BiotaPropertiesString)
            ////    .Take(10000)
            ////    .ToList();

            ////var results = context.Biota
            ////    .Include(r => r.BiotaPropertiesAllegiance)
            ////    .Include(r => r.BiotaPropertiesIID)
            ////    .Include(r => r.BiotaPropertiesInt)
            ////    .Include(r => r.BiotaPropertiesPosition)
            ////    .Include(r => r.BiotaPropertiesString)
            ////    .Take(10000)
            ////    .AsNoTracking()
            ////    .ToList();


            //for (var skip = 0; skip < totalNumBiotas; skip = skip + 1000) //skip = Math.Min(totalNumBiotas, skip + 1000))
            //{
            //    var results = context.Biota
            //        .Include(r => r.BiotaPropertiesAllegiance)
            //        .Include(r => r.BiotaPropertiesIID)
            //        //.Include(r => r.BiotaPropertiesInt)
            //        .Include(r => r.BiotaPropertiesPosition)
            //        .Include(r => r.BiotaPropertiesString)
            //        .Skip(skip)
            //        .Take(1000)
            //        .ToList();

            //    //if (results == null)
            //    //{
            //    //    return false;
            //    //}
            //    //else
            //    if (results != null)
            //    {

            //        foreach (var item in results)
            //        {
            //            var delete = false;
            //            var deleteReason = "";

            //            var nameString = item.BiotaPropertiesString.FirstOrDefault(i => i.Type == (int)PropertyString.Name);

            //            var creationInt = item.BiotaPropertiesInt.FirstOrDefault(i => i.Type == (int)PropertyInt.CreationTimestamp);

            //            //var creationTS = DateTime.MinValue;

            //            //if (creationInt != null)
            //            //{
            //            //    creationTS = Common.Time.GetDateTimeFromTimestamp(creationInt.Value);
            //            //}

            //            if (item.WeenieType == (int)WeenieType.Creature || item.WeenieType == (int)WeenieType.Admin || item.WeenieType == (int)WeenieType.Sentinel)
            //            {
            //                var character = context.Character
            //                    .AsNoTracking()
            //                    .FirstOrDefault(c => c.Id == item.Id);

            //                if (character == null)
            //                {
            //                    if (item.WeenieClassId == 1 || item.WeenieClassId == 4 || item.WeenieClassId == 3648)
            //                    {
            //                        delete = true;
            //                        deleteReason = "Player Biota did not have matching Character entry.";
            //                    }
            //                }
            //            }

            //            if (item.WeenieType == (int)WeenieType.House || item.WeenieType == (int)WeenieType.SlumLord || item.WeenieType == (int)WeenieType.HousePortal
            //                || item.WeenieType == (int)WeenieType.Hook || item.WeenieType == (int)WeenieType.Storage)
            //            {
            //                var houseOwnerIID = item.BiotaPropertiesIID.FirstOrDefault(i => i.Type == (int)PropertyInstanceId.HouseOwner);

            //                if (houseOwnerIID != null)
            //                {
            //                    var character = context.Character
            //                        .AsNoTracking()
            //                        .FirstOrDefault(c => c.Id == houseOwnerIID.Value && c.DeleteTime == 0 && !c.IsDeleted);

            //                    if (character == null)
            //                    {
            //                        item.BiotaPropertiesIID.Remove(houseOwnerIID);

            //                        if (item.WeenieType == (int)WeenieType.SlumLord)
            //                        {
            //                            if (nameString.Value.EndsWith("Apartment"))
            //                                nameString.Value = "Apartment";
            //                            else if (nameString.Value.EndsWith("Cottage"))
            //                                nameString.Value = "Cottage";
            //                            else if (nameString.Value.EndsWith("Villa"))
            //                                nameString.Value = "Villa";
            //                            else if (nameString.Value.EndsWith("Mansion"))
            //                                nameString.Value = "Mansion";
            //                        }

            //                        log.Info($"Biota for {nameString.Value} (0x{item.Id:X8}) has been altered. Reason: HouseOwnerIID (0x{houseOwnerIID.Value:X8}) was not found in database");

            //                        context.SaveChanges();

            //                        continue;
            //                    }
            //                }
            //            }

            //            if (item.WeenieType == (int)WeenieType.Allegiance)
            //            {
            //                var amonarchIID = item.BiotaPropertiesIID.FirstOrDefault(i => i.Type == (int)PropertyInstanceId.Monarch);

            //                if (amonarchIID != null)
            //                {
            //                    var character = context.Character
            //                        .AsNoTracking()
            //                        .FirstOrDefault(c => c.Id == amonarchIID.Value && c.DeleteTime == 0 && !c.IsDeleted);

            //                    if (character == null)
            //                    {
            //                        delete = true;
            //                        deleteReason = $"Allegiance Biota had a MonarchIID (0x{amonarchIID.Value:X8}) that was not found in database.";
            //                    }
            //                }
            //                else
            //                {
            //                    delete = true;
            //                    deleteReason = "Allegiance Biota did not have a MonarchIID entry.";
            //                }
            //            }


            //            var ownerIID = item.BiotaPropertiesIID.FirstOrDefault(i => i.Type == (int)PropertyInstanceId.Owner);
            //            var containerIID = item.BiotaPropertiesIID.FirstOrDefault(i => i.Type == (int)PropertyInstanceId.Container);
            //            var wielderIID = item.BiotaPropertiesIID.FirstOrDefault(i => i.Type == (int)PropertyInstanceId.Wielder);

            //            var locationPosition = item.BiotaPropertiesPosition.FirstOrDefault(i => i.PositionType == (int)PositionType.Location);

            //            if (item.WeenieType != (int)WeenieType.Allegiance && containerIID == null && wielderIID == null && locationPosition == null)
            //            {
            //                delete = true;
            //                deleteReason = "ContainerIID, WielderIID, and Location was null";
            //            }

            //            if (containerIID != null && results.Where(r => r.Id == containerIID.Value).Count() == 0)
            //            {
            //                var biota = context.Biota
            //                        .AsNoTracking()
            //                        .FirstOrDefault(b => b.Id == containerIID.Value);

            //                if (biota == null)
            //                {
            //                    delete = true;
            //                    deleteReason = $"ContainerIID (0x{containerIID.Value:X8}) was not found in database.";
            //                }
            //            }

            //            if (wielderIID != null)
            //            {
            //                var biota = context.Biota
            //                        .AsNoTracking()
            //                        .FirstOrDefault(b => b.Id == wielderIID.Value);

            //                if (biota == null)
            //                {
            //                    delete = true;
            //                    deleteReason = $"WielderIID (0x{wielderIID.Value:X8}) was not found in database.";
            //                }
            //            }

            //            if (ownerIID != null)
            //            {
            //                var biota = context.Biota
            //                        .AsNoTracking()
            //                        .FirstOrDefault(b => b.Id == ownerIID.Value);

            //                if (biota == null)
            //                {
            //                    delete = true;
            //                    deleteReason = $"OwnerIID (0x{ownerIID.Value:X8}) was not found in database.";
            //                }
            //            }

            //            if (!delete)
            //            {
            //                var monarchIID = item.BiotaPropertiesIID.FirstOrDefault(i => i.Type == (int)PropertyInstanceId.Monarch);
            //                if (monarchIID != null)
            //                {
            //                    var character = context.Character
            //                        .AsNoTracking()
            //                        .FirstOrDefault(c => c.Id == monarchIID.Value && c.DeleteTime == 0 && !c.IsDeleted);

            //                    if (character == null)
            //                    {
            //                        item.BiotaPropertiesIID.Remove(monarchIID);

            //                        log.Info($"Biota for {nameString.Value} (0x{item.Id:X8}) has been altered. Reason: MonarchIID (0x{monarchIID.Value:X8}) was not found in database");

            //                        context.SaveChanges();
            //                    }
            //                }

            //                var patronIID = item.BiotaPropertiesIID.FirstOrDefault(i => i.Type == (int)PropertyInstanceId.Patron);
            //                if (patronIID != null)
            //                {
            //                    var character = context.Character
            //                        .AsNoTracking()
            //                        .FirstOrDefault(c => c.Id == patronIID.Value && c.DeleteTime == 0 && !c.IsDeleted);

            //                    if (character == null)
            //                    {
            //                        item.BiotaPropertiesIID.Remove(patronIID);

            //                        log.Info($"Biota for {nameString.Value} (0x{item.Id:X8}) has been altered. Reason: PatronIID (0x{patronIID.Value:X8}) was not found in database");

            //                        context.SaveChanges();
            //                    }
            //                }

            //                var allegianceIID = item.BiotaPropertiesIID.FirstOrDefault(i => i.Type == (int)PropertyInstanceId.Allegiance);
            //                if (allegianceIID != null)
            //                {
            //                    var allegiance = context.Biota
            //                        .AsNoTracking()
            //                        .FirstOrDefault(c => c.Id == allegianceIID.Value);

            //                    if (allegiance == null)
            //                    {
            //                        item.BiotaPropertiesIID.Remove(allegianceIID);

            //                        log.Info($"Biota for {nameString.Value} (0x{item.Id:X8}) has been altered. Reason: AllegianceIID (0x{allegianceIID.Value:X8}) was not found in database");

            //                        context.SaveChanges();
            //                    }
            //                }
            //            }

            //            if (delete)
            //            {
            //                context.Remove(item);

            //                log.Info($"Biota for {nameString.Value} (0x{item.Id:X8}) - WCID: {item.WeenieClassId} | WeenieType: {(WeenieType)item.WeenieType} - has been purged. Reason: {deleteReason}");
            //                //log.Info($"Biota for {nameString.Value} (0x{item.Id:X8}) - WCID: {item.WeenieClassId} | WeenieType: {(WeenieType)item.WeenieType} | CreationTimestamp: {creationTS.ToLocalTime()} - has been purged. Reason: {deleteReason}");

            //                context.SaveChanges();

            //                numberOfBiotasPurged++;
            //            }
            //        }

            //    }

            //}

            //var charResults = context.Character
            //            //.Include(r => r.CharacterPropertiesContractRegistry)
            //            //.Include(r => r.CharacterPropertiesFillCompBook)
            //            .Include(r => r.CharacterPropertiesFriendList)
            //            // .Include(r => r.CharacterPropertiesQuestRegistry)
            //            .Include(r => r.CharacterPropertiesShortcutBar)
            //            //.Include(r => r.CharacterPropertiesSpellBar)
            //            .Include(r => r.CharacterPropertiesSquelch)
            //            //.Include(r => r.CharacterPropertiesTitleBook)
            //            .ToList();

            //foreach (var item in charResults)
            //{
            //    foreach (var friend in item.CharacterPropertiesFriendList.ToList())
            //    {
            //        var character = context.Character
            //            .AsNoTracking()
            //            .FirstOrDefault(c => c.Id == friend.FriendId && c.DeleteTime == 0 && !c.IsDeleted);

            //        if (character == null)
            //        {
            //            item.CharacterPropertiesFriendList.Remove(friend);

            //            log.Info($"Character for {item.Name} (0x{item.Id:X8}) has been altered. Reason: Friend (0x{friend.FriendId:X8}) was not found in database");

            //            context.SaveChanges();
            //        }
            //    }

            //    foreach (var shortcut in item.CharacterPropertiesShortcutBar.ToList())
            //    {
            //        var biota = context.Biota
            //            .AsNoTracking()
            //            .FirstOrDefault(b => b.Id == shortcut.ShortcutObjectId);

            //        if (biota == null)
            //        {
            //            item.CharacterPropertiesShortcutBar.Remove(shortcut);

            //            log.Info($"Character for {item.Name} (0x{item.Id:X8}) has been altered. Reason: Shortcut (0x{shortcut.ShortcutObjectId:X8}) was not found in database");

            //            context.SaveChanges();
            //        }
            //    }

            //    foreach (var squelch in item.CharacterPropertiesSquelch.ToList())
            //    {
            //        var character = context.Character
            //            .AsNoTracking()
            //            .FirstOrDefault(c => c.Id == squelch.SquelchCharacterId && c.DeleteTime == 0 && !c.IsDeleted);

            //        if (character != null)
            //            continue; // Skip Confirmed Character Biotas.
            //        else
            //        {
            //            item.CharacterPropertiesSquelch.Remove(squelch);

            //            log.Info($"Character for {item.Name} (0x{item.Id:X8}) has been altered. Reason: Squelched Character (0x{squelch.SquelchCharacterId:X8}) was not found in database");

            //            context.SaveChanges();
            //        }
            //    }
            //}

            //return true;
        }

        public static void PurgeOrphanedBiotasInParallel(out int numberOfBiotasPurged)
        {
            using (var context = new ShardDbContext())
                PurgeOrphanedBiotasInParallel(context, out numberOfBiotasPurged);
        }

        /// <summary>
        /// This is temporary and can be removed in the near future, 2020-04-05 Mag-nus
        /// </summary>
        public static void FixAnimPartAndTextureMapFromPR2731(out int numberOfRecordsFixed)
        {
            numberOfRecordsFixed = 0;

            using (var context = new ShardDbContext())
            {
                // BiotaPropertiesAnimPart
                var animPartNullRecords = context.BiotaPropertiesAnimPart.Where(r => r.Order == null).Select(r => r.ObjectId).ToList();

                var animPartRecordsToRemove = context.BiotaPropertiesAnimPart.Where(r => animPartNullRecords.Contains(r.ObjectId) && r.Order != null).ToList();

                numberOfRecordsFixed += animPartRecordsToRemove.Count;

                foreach (var recordToRemove in animPartRecordsToRemove)
                    context.BiotaPropertiesAnimPart.Remove(recordToRemove);

                // BiotaPropertiesTextureMap
                var textureMapNullRecords = context.BiotaPropertiesTextureMap.Where(r => r.Order == null).Select(r => r.ObjectId).ToList();

                var textureMapRecordsToRemove = context.BiotaPropertiesTextureMap.Where(r => textureMapNullRecords.Contains(r.ObjectId) && r.Order != null).ToList();

                numberOfRecordsFixed += textureMapRecordsToRemove.Count;

                foreach (var recordToRemove in textureMapRecordsToRemove)
                    context.BiotaPropertiesTextureMap.Remove(recordToRemove);

                // Save
                context.SaveChanges();
            }
        }

        /// <summary>
        /// This is temporary and can be removed in the near future, 2020-04-12 Ripley
        /// </summary>
        public static void CheckForPR2918Script()
        {
            log.Info($"Checking for 2020-04-11-00-Update-Character-SpellBars.sql patch");

            using (var context = new ShardDbContext())
            {
                var characterSpellBarsNotFixed = context.CharacterPropertiesSpellBar.Where(c => c.SpellBarNumber == 0).ToList();

                if (characterSpellBarsNotFixed.Count > 0)
                {
                    log.Warn("2020-04-11-00-Update-Character-SpellBars.sql patch not yet applied. Please apply this patch ASAP! Skipping FixSpellBarsPR2918 for now...");
                    log.Fatal("2020-04-11-00-Update-Character-SpellBars.sql patch not yet applied. You must apply this patch before proceeding further...");
                    Environment.Exit(1);
                    return;
                }
            }

            log.Info($"2020-04-11-00-Update-Character-SpellBars.sql patch has been successfully installed. Before opening world to players, make sure you've run fix-spell-bars command from console");
        }

        /// <summary>
        /// <para>unknown how this column keeps deleting, but it is likely a bug that is a result of auto (world only?) database updates that repros under currently unknown conditions</para>
        /// this checks for and attempts to correct shard database missing order column
        /// </summary>
        public static void CheckForBiotaPropertiesPaletteOrderColumnInShard()
        {
            //log.Info($"Checking for order column in biota_properties_palette table in shard database...");

            using (var context = new ShardDbContext())
            {
                try
                {
                    var result = context.BiotaPropertiesPalette.FirstOrDefault();
                }
                catch (MySql.Data.MySqlClient.MySqlException)
                {
                    log.Warn("order column in biota_properties_palette table in shard database is missing! Attempting to fix...");
                    try
                    {
                        context.Database.ExecuteSqlRaw("ALTER TABLE `biota_properties_palette` ADD COLUMN `order` TINYINT(3) UNSIGNED NULL DEFAULT NULL AFTER `length`;");

                        var result = context.BiotaPropertiesPalette.FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        log.Fatal($"Unable to restore order column in biota_properties_palette table in shard database due to following error: {ex.GetFullMessage()}");
                        Environment.Exit(1);
                        return;
                    }
                }
            }

            //log.Info($"Successfully verified order column in biota_properties_palette table in shard database!");
        }
    }
}
