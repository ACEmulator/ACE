using System;
using System.Collections.Generic;
using System.Text;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using ACE.Server.Factories;
using ACE.Server.Network;
namespace ACE.Server.Command.Handlers
{
    public static class DeveloperLootCommands
    {
        [CommandHandler("testlootgen", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Generates Loot for testing LootFactories.  Do testlootgen -info for examples.", "<number of items> <loot tier> <melee, missile, caster, armor, pet, aetheria (optional)>")]
        public static void TestLootGenerator(Session session, params string[] parameters)
        {
            // This generates loot items and displays the drop rates of LootFactory
            string logFile = "";
            string displayTable = "";
            bool logstats = false;
            // Switch for different options
            switch (parameters[0])
            {
                case "-info":
                    Console.WriteLine($"Usage: \n" +
                                    $"<number of items> <loot tier> <(optional)display table - melee, missile, caster, armor, pet, aetheria> \n" +
                                    $" Example: The following command will generate 1000 items in Tier 7 that shows the melee table\n" +
                                    $"testlootgen 1000 7 melee \n" +
                                    $" Example: The following command will generate 1000 items in Tier 6 that just shows a summary \n" +
                                    $"testlootgen 1000 6 \n");
                    return;
                default:
                    break;
            }

            if (Int32.TryParse(parameters[0], out int numberItemsGenerate))
            {
                ////Console.WriteLine("Number of items to generate " + numberItemsGenerate);
            }
            else
            {
                Console.WriteLine("Number of items is not an integer");
                return;
            }

            if (Int32.TryParse(parameters[1], out int itemsTier))
            {
                ////Console.WriteLine("tier is " + itemsTier);
            }
            else
            {
                Console.WriteLine("Tier is not an integer");
                return;
            }
            if (parameters.LongLength > 2)
                displayTable = parameters?[2].ToLower();
            switch (displayTable)
            {
                case "melee":
                    break;
                case "missile":
                    break;
                case "caster":
                    break;
                case "armor":
                    break;
                case "pet":
                    break;
                case "aetheria":
                    break;
                case "all":
                    break;
                case "-log":
                    logstats = true;                    
                    break;
                case "":
                    break;
                default:
                    Console.WriteLine("Invalid Table Option.  Available Tables to show are melee, missile, caster, armor, pet, aetheria or all.");
                    return;
            }
            if (parameters.LongLength > 3)
                logFile = parameters?[3].ToLower();
            switch (logFile)
            {
                case "":
                    break;
                case "-log":
                    logstats = true;
                    // Console.WriteLine("Logging is not currently working, Displaying results to screen.");
                    break;
                default:
                    Console.WriteLine("Invalid Option.  To log a file, use option -log");
                    return;
            }
            if (itemsTier > 0 && itemsTier < 9)
                Console.WriteLine(LootGenerationFactory_Test.TestLootGen(numberItemsGenerate, itemsTier, logstats, displayTable));
            else
            {
                Console.WriteLine($"Tier must be 1-8.  You entered tier {itemsTier}, which does not exist!");
                return;
            }
        }
        [CommandHandler("testlootgencorpse", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Generates Corpses for testing LootFactories", "<DID> <number corpses> <display table - melee, missile, caster, armor, pet, aetheria>")]
        public static void TestLootGeneratorCorpse(Session session, params string[] parameters)
        {
            // This generates loot items and displays the drop rates of LootFactory
            int monsterDID = 0;
            int numberItemsGenerate = 0;
            string logFile = "";
            string displayTable = "";

            bool logstats = false;

            // Switch for different options
            switch (parameters[0])
            {
                case "-info":
                    Console.WriteLine($"Usage: \n" +
                                    $"<DID> <number corpses> <(optional)display table - melee, missile, caster, armor, pet, aetheria> \n" +
                                    $" Example: The following command will generate 50 corpses generated from DeathTreasure DID 998 that shows the caster table\n" +
                                    $"testlootgencorpse 998 50 caster \n" +
                                    $" Example: The following command will generate 75 corpses generated from DeathTreasure DID 452 that just shows a summary \n" +
                                    $"testlootgencorpse 452 75 \n");
                    return;
                default:
                    break;
            }
            if (!int.TryParse(parameters[0], out monsterDID))
            {
                Console.WriteLine($" LootFactory Simulator \n ---------------------\n DID specified is not an integer \n");
                return;
            }
            if (parameters.Length > 1)
            {
                if (!int.TryParse(parameters[1], out numberItemsGenerate))
                {
                    Console.WriteLine($" LootFactory Simulator \n ---------------------\n Invalid Parameter - Must be a number \n");
                    return;
                }
            }
            else
            {
                Console.WriteLine($" LootFactory Simulator \n ---------------------\n Need to specify number of coprses\n");
                return;
            }
            if (parameters.LongLength > 2)
                displayTable = parameters?[2].ToLower();
            switch (displayTable)
            {
                case "melee":
                    break;
                case "missile":
                    break;
                case "caster":
                    break;
                case "armor":
                    break;
                case "pet":
                    break;
                case "aetheria":
                    break;
                case "all":
                    break;
                case "-log":
                    logstats = true;
                    break;
                case "":
                    break;
                default:
                    Console.WriteLine("Invalid Table Option.  Available Tables to show are melee, missile, caster, armor, pet, aetheria or all.");
                    return;
            }
            if (parameters.LongLength > 3)
                logFile = parameters?[3].ToLower();
            switch (logFile)
            {
                case "":
                    break;
                case "-log":
                    logstats = true;
                    // Console.WriteLine("Logging is not currently working, Displaying results to screen.");
                    break;
                default:
                    Console.WriteLine("Invalid Option.  To log a file, use option -log");
                    return;
            }
            Console.WriteLine(LootGenerationFactory_Test.TestLootGenMonster(Convert.ToUInt32(monsterDID), numberItemsGenerate, logstats, displayTable));
        }
    }
}
