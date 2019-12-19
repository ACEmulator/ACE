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
        [CommandHandler("testlootgen", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Generates Loot for testing LootFactories", "<number of items> <loot tier>")]
        public static void TestLootGenerator(Session session, params string[] parameters)
        {
            // This generates loot items and displays the drop rates of LootFactory

            // Switch for different options
            switch (parameters[0])
            {
                case "-info":
                    Console.WriteLine("This is for more info on how to use this command");
                    break;
                default:
                    break;
            }

            if (Int32.TryParse(parameters[0], out int numberItemsGenerate))
            {
                Console.WriteLine("Number of items to generate " + numberItemsGenerate);
            }
            else
            {
                Console.WriteLine("numbr of items is not an integer");
                return;
            }

            if (Int32.TryParse(parameters[1], out int itemsTier))
            {
                Console.WriteLine("tier is " + itemsTier);
            }
            else
            {
                Console.WriteLine("tier is not an integer");
                return;
            }

            Console.WriteLine(LootGenerationFactory_Test.TestLootGen(numberItemsGenerate, itemsTier));

            Console.WriteLine($"Loot Generation of {numberItemsGenerate} items, in tier {itemsTier} complete.");
        }
        [CommandHandler("testlootgencorpse", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Generates Corpses for testing LootFactories", "<DID> <number corpses>")]
        public static void TestLootGeneratorCorpse(Session session, params string[] parameters)
        {
            // This generates loot items and displays the drop rates of LootFactory
            int monsterDID = 0;
            int numberItemsGenerate = 0;
            // Switch for different options
            switch (parameters[0])
            {
                case "-info":
                    Console.WriteLine("This is for more info on how to use this command");
                    break;
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
            Console.WriteLine(LootGenerationFactory_Test.TestLootGenMonster(Convert.ToUInt32(monsterDID), numberItemsGenerate));
        }
    }
}
