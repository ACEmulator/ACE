using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using log4net;

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

        public static void PurgeCharacter(ShardDbContext context, uint characterId, out int charactersPurged, out int playerBiotasPurged, out int possessionsPurged)
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

            if (character != null)
                log.Debug($"[DATABASE][PURGE] Character 0x{characterId:X8}:{character.Name}, deleted on {Common.Time.GetDateTimeFromTimestamp(character.DeleteTime).ToLocalTime()}, and {possessionsPurged} of their possessions has been purged.");
            else
                log.Debug($"[DATABASE][PURGE] Character 0x{characterId:X8} and {possessionsPurged} of their possessions has been purged.");

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Character name might be in use or some other fault
                log.Error($"PurgeCharacter 0x{characterId:X8} failed with exception: {ex}");
            }
        }

        public static void PurgeCharacter(uint characterId, out int charactersPurged, out int playerBiotasPurged, out int possessionsPurged)
        {
            using (var context = new ShardDbContext())
                PurgeCharacter(context, characterId, out charactersPurged, out playerBiotasPurged, out possessionsPurged);
        }

        public static void PurgeCharacters(ShardDbContext context, int daysLimiter, out int charactersPurged, out int playerBiotasPurged, out int possessionsPurged)
        {
            var deleteLimit = Common.Time.GetUnixTime(DateTime.UtcNow.AddDays(-daysLimiter));

            var results = context.Character
                .Where(r => (r.DeleteTime > 0 && r.DeleteTime < deleteLimit) || (r.IsDeleted && r.DeleteTime == 0))
                .AsNoTracking()
                .ToList();

            int charactersPurgedTotal = 0;
            int playerBiotasPurgedTotal = 0;
            int possessionsPurgedTotal = 0;

            Parallel.ForEach(results, result =>
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

        public static void PurgeCharacters(int daysLimiter, out int charactersPurged, out int playerBiotasPurged, out int possessionsPurged)
        {
            using (var context = new ShardDbContext())
                PurgeCharacters(context, daysLimiter, out charactersPurged, out playerBiotasPurged, out possessionsPurged);
        }


        /*public static bool PurgeOrphanedBiotas(ShardDbContext context, out int numberOfBiotasPurged)
        {
            numberOfBiotasPurged = 0;

            // WIP code, not ready for production
            return false;

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

        public static bool PurgeOrphanedBiotas(out int numberOfBiotasPurged)
        {
            using (var context = new ShardDbContext())
                return PurgeOrphanedBiotas(context, out numberOfBiotasPurged);
        }*/
    }
}
