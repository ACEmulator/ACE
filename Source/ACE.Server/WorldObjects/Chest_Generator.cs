using ACE.Database;
using ACE.Server.Factories;
using System.Collections.Generic;

namespace ACE.Server.WorldObjects
{
    public partial class Chest
    {
        public void GenerateTreasure()
        {

            // Adding loot to chests
            // Eventually these case statements would be linked to individual treasure generators.
            // Each one should be a different profile, but currently it will be the complete appropriate tier profile.
            for (int i = 0; i < GeneratorProfiles.Count; i++)
            {
                int amount = ThreadSafeRandom.Next(2, 14);  //r.Next(2, 14);
                var generator = GeneratorProfiles[i];
                var wcid = generator.Biota.WeenieClassId;
                var deathTreasure = DatabaseManager.World.GetCachedDeathTreasure(wcid);
                if (deathTreasure != null)
                {
                    // TODO: get randomly generated death treasure from LootGenerationFactory
                    //Console.WriteLine($"{_generator.Name}.TreasureGenerator(): found death treasure {Biota.WeenieClassId}");
                    List<WorldObject> items = LootGenerationFactory.CreateRandomLootObjects(deathTreasure);
                    foreach(WorldObject wo in items)
                    {
                        TryAddToInventory(wo);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a loot tier 1-6 for a treasure DID
        /// If no treasure DID mapping defined, returns 0
        /// </summary>
        //public int GetLootTier(uint treasureDID)
        //{
        //    switch (treasureDID)
        //    {
        //        case 0:
        //        case 6:
        //        case 18:
        //        case 414:
        //        case 459:
        //        case 465:
        //            return 1;

        //        case 4:
        //        case 16:
        //        case 395:
        //        case 410:
        //        case 413:
        //        case 457:
        //        case 463:
        //            return 2;

        //        case 3:
        //        case 15:
        //        case 313:
        //        case 340:
        //        case 365:
        //        case 411:
        //        case 456:
        //        case 462:
        //            return 3;

        //        case 1:
        //        case 13:
        //        case 59:
        //        case 339:
        //        case 354:
        //        case 412:
        //        case 460:
        //            return 4;

        //        case 317:
        //        case 334:
        //        case 341:
        //            return 5;

        //        case 2:
        //        case 32:
        //        case 338:
        //        case 349:
        //        case 351:
        //        case 421:
        //        case 422:
        //        case 449:
        //            return 6;

        //        default:
        //            return 0;
        //    }
        //}

    }
}
