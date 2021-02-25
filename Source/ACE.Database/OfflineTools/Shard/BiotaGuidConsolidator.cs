using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using ACE.Database.Adapter;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.OfflineTools.Shard
{
    public static class BiotaGuidConsolidator
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly HashSet<WeenieType> ConsolidatableBasicWeenieTypes = new HashSet<WeenieType>
        {
            WeenieType.Generic,
            WeenieType.Clothing,
            WeenieType.MissileLauncher,
            WeenieType.Missile,
            WeenieType.Ammunition,
            WeenieType.MeleeWeapon,
            WeenieType.Book,
            WeenieType.Coin,
            WeenieType.Food,
            WeenieType.Key,
            WeenieType.Lockpick,
            WeenieType.Healer,
            WeenieType.LightSource,
            WeenieType.Allegiance,
            WeenieType.SpellComponent,
            WeenieType.Scroll,
            WeenieType.Caster,
            WeenieType.ManaStone,
            WeenieType.Gem,
            WeenieType.CraftTool,
            WeenieType.Stackable,
            WeenieType.Deed,
            WeenieType.SkillAlterationDevice,
            WeenieType.AttributeTransferDevice,
            WeenieType.Hooker,
            WeenieType.AugmentationDevice,
            WeenieType.PetDevice,
        };

        /// <summary>
        /// These weenie types require contained items to also be updated
        /// </summary>
        private static readonly HashSet<WeenieType> ConsolidatableContainerWeenieTypes = new HashSet<WeenieType>
        {
            WeenieType.Corpse,
            WeenieType.Container,
        };

        public static void ConsolidateBiotaGuids(uint startingGuid, out int numberOfBiotasConsolidated, out int numberOfErrors)
        {
            log.Info($"Consolidating biotas, starting at guid 0x{startingGuid:X8}...");

            Thread.Sleep(1000); // Give the logger type to flush to the client so that our output lines up in order

            Console.WriteLine("!!! Do not proceed unless you have backed up your shard database first !!!");
            Console.WriteLine("In the event of any failure, you may be asked to rollback your shard database.");
            Console.WriteLine("Press any key to proceed, or abort the process to quit.");
            Console.ReadLine();
            Console.WriteLine(".... hold on to your butts...");

            if (startingGuid < ObjectGuid.DynamicMin)
                throw new Exception($"startingGuid cannot be lower than ObjectGuid.DynamicMin (0x{ObjectGuid.DynamicMin:X8})");

            int numOfBiotasConsolidated = 0;
            int numOfErrors = 0;

            var shardDatabase = new ShardDatabase();

            var sequenceGaps = shardDatabase.GetSequenceGaps(ObjectGuid.DynamicMin, 10000000);
            var availableIDs = new LinkedList<(uint start, uint end)>(sequenceGaps);
            List<Biota> partialBiotas;

            using (var context = new ShardDbContext())
                partialBiotas = context.Biota.Where(r => r.Id >= startingGuid).OrderByDescending(r => r.Id).ToList();

            var idConversions = new ConcurrentDictionary<uint, uint>();

            // Process ConsolidatableBasicWeenieTypes first
            Parallel.ForEach(partialBiotas, partialBiota =>
            {
                if (numOfErrors > 0)
                    return;

                if (!ConsolidatableBasicWeenieTypes.Contains((WeenieType)partialBiota.WeenieType))
                    return;

                // Get the original biota
                var fullBiota = shardDatabase.GetBiota(partialBiota.Id, true);

                if (fullBiota == null)
                {
                    Interlocked.Increment(ref numOfErrors);
                    log.Warn($"Failed to get biota with id 0x{partialBiota.Id:X8} from the database. This shouldn't happen. It also shouldn't require a rollback.");
                    return;
                }

                // Get the next available id
                uint newId = 0;

                lock (availableIDs)
                {
                    if (availableIDs.First != null)
                    {
                        var id = availableIDs.First.Value.start;

                        if (availableIDs.First.Value.start == availableIDs.First.Value.end)
                            availableIDs.RemoveFirst();
                        else
                            availableIDs.First.Value = (availableIDs.First.Value.start + 1, availableIDs.First.Value.end);

                        newId = id;
                    }
                }

                if (newId == 0)
                {
                    Interlocked.Increment(ref numOfErrors);
                    log.Fatal("Failed to generate new id. No more id's available for consolidation. This shouldn't require a rollback.");
                    return;
                }

                idConversions[fullBiota.Id] = newId;

                // Copy our original biota into a new biota and set the new id
                var converted = BiotaConverter.ConvertToEntityBiota(fullBiota);
                converted.Id = newId;

                // Save the new biota
                if (!shardDatabase.SaveBiota(converted, new ReaderWriterLockSlim()))
                {
                    Interlocked.Increment(ref numOfErrors);
                    log.Fatal($"Failed to save new biota with id 0x{fullBiota.Id:X8} to the database. Please rollback your shard.");
                    return;
                }

                // Finally, remove the original biota
                if (!shardDatabase.RemoveBiota(fullBiota.Id))
                {
                    Interlocked.Increment(ref numOfErrors);
                    log.Fatal($"Failed to remove original biota with id 0x{fullBiota.Id:X8} from database. Please rollback your shard.");
                    return;
                }

                var tempNumOfBiotasConsolidated = Interlocked.Increment(ref numOfBiotasConsolidated);

                if ((tempNumOfBiotasConsolidated + numOfErrors) % 1000 == 0)
                    Console.WriteLine($"{tempNumOfBiotasConsolidated:N0} biotas successfully processed out of {partialBiotas.Count:N0}...");
            });


            // Process ConsolidatableContainerWeenieTypes second
            foreach (var partialBiota in partialBiotas)
            {
                if (numOfErrors > 0)
                    break;

                if (!ConsolidatableContainerWeenieTypes.Contains((WeenieType)partialBiota.WeenieType))
                    continue;

                // Get the original biota
                var fullBiota = shardDatabase.GetBiota(partialBiota.Id, true);

                if (fullBiota == null)
                {
                    Interlocked.Increment(ref numOfErrors);
                    log.Warn($"Failed to get biota with id 0x{partialBiota.Id:X8} from the database. This shouldn't happen. It also shouldn't require a rollback.");
                    break;
                }

                // Get the next available id
                uint newId = 0;

                lock (availableIDs)
                {
                    if (availableIDs.First != null)
                    {
                        var id = availableIDs.First.Value.start;

                        if (availableIDs.First.Value.start == availableIDs.First.Value.end)
                            availableIDs.RemoveFirst();
                        else
                            availableIDs.First.Value = (availableIDs.First.Value.start + 1, availableIDs.First.Value.end);

                        newId = id;
                    }
                }

                if (newId == 0)
                {
                    Interlocked.Increment(ref numOfErrors);
                    log.Fatal("Failed to generate new id. No more id's available for consolidation. This shouldn't require a rollback.");
                    break;
                }

                idConversions[fullBiota.Id] = newId;

                // Copy our original biota into a new biota and set the new id
                var converted = BiotaConverter.ConvertToEntityBiota(fullBiota);
                converted.Id = newId;

                // Save the new biota
                if (!shardDatabase.SaveBiota(converted, new ReaderWriterLockSlim()))
                {
                    Interlocked.Increment(ref numOfErrors);
                    log.Fatal($"Failed to save new biota with id 0x{fullBiota.Id:X8} to the database. Please rollback your shard.");
                    break;
                }

                // update contained items to point to the new container
                using (var context = new ShardDbContext())
                {
                    var ownedItems = context.BiotaPropertiesIID.Where(r => r.Type == (ushort)PropertyInstanceId.Owner && r.Value == fullBiota.Id);

                    foreach (var item in ownedItems)
                        item.Value = converted.Id;

                    var containedItems = context.BiotaPropertiesIID.Where(r => r.Type == (ushort)PropertyInstanceId.Container && r.Value == fullBiota.Id);

                    foreach (var item in containedItems)
                        item.Value = converted.Id;

                    context.SaveChanges();
                }

                // Finally, remove the original biota
                if (!shardDatabase.RemoveBiota(fullBiota.Id))
                {
                    Interlocked.Increment(ref numOfErrors);
                    log.Fatal($"Failed to remove original biota with id 0x{fullBiota.Id:X8} from database. Please rollback your shard.");
                    break;
                }

                var tempNumOfBiotasConsolidated = Interlocked.Increment(ref numOfBiotasConsolidated);

                if ((tempNumOfBiotasConsolidated + numOfErrors) % 1000 == 0)
                    Console.WriteLine($"{tempNumOfBiotasConsolidated:N0} biotas successfully processed out of {partialBiotas.Count:N0}...");
            }


            // Update enchantment tables for equipped items
            using (var context = new ShardDbContext())
            {
                var enchantments = context.BiotaPropertiesEnchantmentRegistry.Where(r => r.CasterObjectId >= startingGuid).ToList();

                // First, remove the enchantments from the database
                foreach (var enchantment in enchantments)
                {
                    if (idConversions.TryGetValue(enchantment.CasterObjectId, out var newId))
                        context.BiotaPropertiesEnchantmentRegistry.Remove(enchantment);
                }

                context.SaveChanges();

                // Second, re-id them and add them back
                foreach (var enchantment in enchantments)
                {
                    if (idConversions.TryGetValue(enchantment.CasterObjectId, out var newId))
                    {
                        enchantment.CasterObjectId = newId;

                        context.BiotaPropertiesEnchantmentRegistry.Add(enchantment);
                    }
                }

                var shortcuts = context.CharacterPropertiesShortcutBar.Where(r => r.ShortcutObjectId >= startingGuid).ToList();

                foreach (var shortcut in shortcuts)
                {
                    if (idConversions.TryGetValue(shortcut.ShortcutObjectId, out var newId))
                        shortcut.ShortcutObjectId = newId;
                }

                context.SaveChanges();
            }


            // Finished
            numberOfBiotasConsolidated = numOfBiotasConsolidated;
            numberOfErrors = numOfErrors;

            log.Info($"Consolidated {numberOfBiotasConsolidated:N0} biotas with {numberOfErrors:N0} errors out of {partialBiotas.Count:N0} total.");
        }
    }
}
