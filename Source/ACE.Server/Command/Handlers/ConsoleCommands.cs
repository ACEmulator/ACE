using System;
using System.Collections.Generic;

using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using ACE.Server.Network;

// this is for testing making lootgen items
using ACE.Server.Factories;

using Newtonsoft.Json;
using System.IO;
using ACE.Database;

namespace ACE.Server.Command.Handlers
{
    public static class ConsoleCommands
    {
        [CommandHandler("cell-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Export contents of CELL DAT file.", "<export-directory-without-spaces>")]
        public static void ExportCellDatContents(Session session, params string[] parameters)
        {
            if (parameters?.Length != 1)
                Console.WriteLine("cell-export <export-directory-without-spaces>");

            string exportDir = parameters[0];

            Console.WriteLine($"Exporting cell.dat contents to {exportDir}.  This can take longer than an hour.");
            DatManager.CellDat.ExtractLandblockContents(exportDir);
            Console.WriteLine($"Export of cell.dat to {exportDir} complete.");
        }

        [CommandHandler("portal-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Export contents of PORTAL DAT file.", "<export-directory-without-spaces>")]
        public static void ExportPortalDatContents(Session session, params string[] parameters)
        {
            if (parameters?.Length != 1)
                Console.WriteLine("portal-export <export-directory-without-spaces>");

            string exportDir = parameters[0];

            Console.WriteLine($"Exporting portal.dat contents to {exportDir}.  This will take a while.");
            DatManager.PortalDat.ExtractCategorizedPortalContents(exportDir);
            Console.WriteLine($"Export of portal.dat to {exportDir} complete.");
        }

        [CommandHandler("highres-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Export contents of client_highres.dat file.", "<export-directory-without-spaces>")]
        public static void ExportHighresDatContents(Session session, params string[] parameters)
        {
            if (DatManager.HighResDat == null)
            {
                Console.WriteLine("client_highres.dat file was not loaded.");
                return;
            }
            if (parameters?.Length != 1)
                Console.WriteLine("highres-export <export-directory-without-spaces>");

            string exportDir = parameters[0];

            Console.WriteLine($"Exporting client_highres.dat contents to {exportDir}.  This will take a while.");
            DatManager.HighResDat.ExtractCategorizedPortalContents(exportDir);
            Console.WriteLine($"Export of client_highres.dat to {exportDir} complete.");
        }

        [CommandHandler("language-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Export contents of client_local_English.dat file.", "<export-directory-without-spaces>")]
        public static void ExportLanguageDatContents(Session session, params string[] parameters)
        {
            if (DatManager.LanguageDat == null)
            {
                Console.WriteLine("client_highres.dat file was not loaded.");
                return;
            }
            if (parameters?.Length != 1)
                Console.WriteLine("language-export <export-directory-without-spaces>");

            string exportDir = parameters[0];

            Console.WriteLine($"Exporting client_local_English.dat contents to {exportDir}.  This will take a while.");
            DatManager.LanguageDat.ExtractCategorizedPortalContents(exportDir);
            Console.WriteLine($"Export of client_local_English.dat to {exportDir} complete.");
        }

        /// <summary>
        /// Export all wav files to a specific directory.
        /// </summary>
        [CommandHandler("wave-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Export Wave Files")]
        public static void ExportWaveFiles(Session session, params string[] parameters)
        {
            if (parameters?.Length != 1)
            {
                Console.WriteLine("wave-export <export-directory-without-spaces>");
                return;
            }

            string exportDir = parameters[0];

            Console.WriteLine($"Exporting portal.dat WAV files to {exportDir}.  This may take a while.");
            foreach (KeyValuePair<uint, DatFile> entry in DatManager.PortalDat.AllFiles)
            {
                if (entry.Value.GetFileType(DatDatabaseType.Portal) == DatFileType.Wave)
                {
                    var wave = DatManager.PortalDat.ReadFromDat<Wave>(entry.Value.ObjectId);

                    wave.ExportWave(exportDir);
                }
            }
            Console.WriteLine($"Export to {exportDir} complete.");
        }

        /// <summary>
        /// Export all texture/image files to a specific directory.
        /// </summary>
        [CommandHandler("image-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Export Texture/Image Files")]
        public static void ExportImageFile(Session session, params string[] parameters)
        {
            string syntax = "image-export <export-directory-without-spaces> [id]";
            if (parameters?.Length < 1)
            {
                Console.WriteLine(syntax);
                return;
            }

            string exportDir = parameters[0];
            if(exportDir.Length == 0 || !System.IO.Directory.Exists(exportDir))
            {
                Console.WriteLine(syntax);
                return;
            }

            if (parameters.Length > 1)
            {
                uint imageId;
                if (parameters[1].StartsWith("0x"))
                {
                    string hex = parameters[1].Substring(2);
                    if(!uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out imageId))
                    {
                        Console.WriteLine(syntax);
                        return;
                    }
                }
                else
                if (!uint.TryParse(parameters[1], out imageId))
                {
                    Console.WriteLine(syntax);
                    return;
                }

                var image = DatManager.PortalDat.ReadFromDat<Texture>(imageId);
                image.ExportTexture(exportDir);

                Console.WriteLine($"Exported " + imageId.ToString("X8") + " to " + exportDir + ".");
            }
            else
            {
                int portalFiles = 0;
                int highresFiles = 0;
                Console.WriteLine($"Exporting client_portal.dat textures and images to {exportDir}.  This may take a while.");
                foreach (KeyValuePair<uint, DatFile> entry in DatManager.PortalDat.AllFiles)
                {
                    if (entry.Value.GetFileType(DatDatabaseType.Portal) == DatFileType.Texture)
                    {
                        var image = DatManager.PortalDat.ReadFromDat<Texture>(entry.Value.ObjectId);
                        image.ExportTexture(exportDir);
                        portalFiles++;
                    }
                }
                Console.WriteLine($"Exported {portalFiles} total files from client_portal.dat to {exportDir}.");

                if (DatManager.HighResDat != null)
                {
                    foreach (KeyValuePair<uint, DatFile> entry in DatManager.HighResDat.AllFiles)
                    {
                        if (entry.Value.GetFileType(DatDatabaseType.Portal) == DatFileType.Texture)
                        {
                            var image = DatManager.HighResDat.ReadFromDat<Texture>(entry.Value.ObjectId);
                            image.ExportTexture(exportDir);
                            highresFiles++;
                        }
                    }
                    Console.WriteLine($"Exported {highresFiles} total files from client_highres.dat to {exportDir}.");
                }
                int totalFiles = portalFiles + highresFiles;
                Console.WriteLine($"Exported {totalFiles} total files to {exportDir}.");
            }
        }
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

            Console.WriteLine($"Creating {numberItemsGenerate} items, that are in tier {itemsTier}");

            // Creating JSON Serializer using NewtonSoft
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            serializer.Formatting = Formatting.Indented;

            // Counters
            float armorCount = 0;
            float meleeWeaponCount = 0;
            float casterCount = 0;
            float missileWeaponCount = 0;
            float jewelryCount = 0;
            float gemCount = 0;
            float clothingCount = 0;
            float otherCount = 0;
            float nullCount = 0;

            // Loop depending on how many items you are creating
            //string fileName = null;
            for (int i = 0; i < numberItemsGenerate; i++)
            {
                var testItem = LootGenerationFactory.CreateRandomLootObjects(itemsTier, true);              
                if (testItem is null )
                {
                    nullCount++;
                    continue;
                }
                string itemType = testItem.ItemType.ToString();
                if (itemType == null)
                {
                    nullCount++;

                    continue;
                }
                                                          
                switch (itemType)
                {
                    case "Armor":
                        armorCount++;
                        break;
                    case "MeleeWeapon":
                        meleeWeaponCount++;
                        break;
                    case "Caster":
                        casterCount++;
                        break;
                    case "MissileWeapon":
                        missileWeaponCount++;
                        break;
                    case "Jewelry":
                        jewelryCount++;
                        break;
                    case "Gem":
                        gemCount++;
                        break;
                    case "Clothing":
                        clothingCount++;
                        break;
                    default:
                        otherCount++;

                        break;
                }

                if (testItem == null)
                {
                    Console.WriteLine("*Name is Null*");
                    continue;
                }
                else
                {

                }

            }
            float totalItemsGenerated = armorCount + meleeWeaponCount + casterCount + missileWeaponCount + jewelryCount + gemCount + clothingCount + otherCount;

            Console.WriteLine($" Armor={armorCount} \n " +
                                $"MeleeWeapon={meleeWeaponCount} \n " +
                                $"Caster={casterCount} \n " +
                                $"MissileWeapon={missileWeaponCount} \n " +
                                $"Jewelry={jewelryCount} \n " +
                                $"Gem={gemCount} \n " +
                                $"Clothing={clothingCount} \n " +
                                $"Other={otherCount} \n " +
                                $"NullCount={nullCount} \n " +
                                $"TotalGenerated={totalItemsGenerated}");
            Console.WriteLine();
            Console.WriteLine($" Drop Rates \n " +
                                $"Armor= {armorCount / totalItemsGenerated * 100}% \n " +
                                $"MeleeWeapon= {meleeWeaponCount / totalItemsGenerated * 100}% \n " +
                                $"Caster= {casterCount / totalItemsGenerated * 100}% \n " +
                                $"MissileWeapon= {missileWeaponCount / totalItemsGenerated * 100}% \n " +
                                $"Jewelry= {jewelryCount / totalItemsGenerated * 100}% \n " +
                                $"Gem= {gemCount / totalItemsGenerated * 100}% \n " +
                                $"Clothing= {clothingCount / totalItemsGenerated * 100}% \n " +
                                $"Other={otherCount / totalItemsGenerated * 100}% \n  ");


            Console.WriteLine($"Loot Generation of {numberItemsGenerate} items, in tier {itemsTier} complete.");
        }

    }
}
