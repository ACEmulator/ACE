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
    }
}
