using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Factories;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// Dictionary for salvage bags/material types
        /// TODO: This list needs to go somewhere else
        /// </summary>
        static Dictionary<int, int> dict = new Dictionary<int, int>()
                                                            {
                                                                {1, 20983},
                                                                {2, 21067},
                                                                {3, 0},//cloth
                                                                {4, 20987},
                                                                {5, 20992},
                                                                {6, 21076},
                                                                {7, 20994},
                                                                {8, 20995},
                                                                {9, 0},//gem
                                                                {10, 21034},
                                                                {11, 21035},
                                                                {12, 21036},
                                                                {13, 21037},
                                                                {14, 21038},
                                                                {15, 21039},
                                                                {16, 21040},
                                                                {17, 21041},
                                                                {18, 21043},
                                                                {19, 21044},
                                                                {20, 21046},
                                                                {21, 21048},
                                                                {22, 21049},
                                                                {23, 21050},
                                                                {24, 21051},
                                                                {25, 21053},
                                                                {26, 21054},
                                                                {27, 21056},
                                                                {28, 21057},
                                                                {29, 21058},
                                                                {30, 21060},
                                                                {31, 21062},
                                                                {32, 21064},
                                                                {33, 21065},
                                                                {34, 21066},
                                                                {35, 21069},
                                                                {36, 21070},
                                                                {37, 21071},
                                                                {38, 21072},
                                                                {39, 21074},
                                                                {40, 21078},
                                                                {41, 21079},
                                                                {42, 21081},
                                                                {43, 21082},
                                                                {44, 21083},
                                                                {45, 21084},
                                                                {46, 21085},
                                                                {47, 21086},
                                                                {48, 21087},
                                                                {49, 21088},
                                                                {50, 21089},
                                                                {51, 21055},
                                                                {52, 21059},
                                                                {53, 20981},
                                                                {54, 21052},
                                                                {55, 20991},
                                                                {56, 0}, //metal
                                                                {57, 21042},
                                                                {58, 20982},
                                                                {59, 21045},
                                                                {60, 20984},
                                                                {61, 20986},
                                                                {62, 21068},
                                                                {63, 21077},
                                                                {64, 20993},
                                                                {65, 0}, //stone
                                                                {66, 20980},
                                                                {67, 20985},
                                                                {68, 21061},
                                                                {69, 21063},
                                                                {70, 21073},
                                                                {71, 21075},
                                                                {72, 0}, //wood
                                                                {73, 21047},
                                                                {74, 20988},
                                                                {75, 20989},
                                                                {76, 20990},
                                                                {77, 21080}
                                                            };

        /// <summary>
        /// This code handles salvaginng materials with the Ust.
        /// FORMULA FOR VALUE
        /// (skill) / 387 * (1.0 + (augmentations)
        /// WHERE
        /// skill = Salvaging Skill
        /// augmentations = 0.0 for no augmentations, 0.25 for one, 0.5 for two, 
        /// 0.75 for three, 1.0 for four
        /// </summary>
        public void HandleTinkeringTool(List<uint /*ObjectGuid*/> list)
        {
            
            //CreatureSkill skill = GetCreatureSkill(Skill.Salvaging);
            double salvageSkill = (double)Math.Max((uint)GetCreatureSkill(Skill.Salvaging).Current, Math.Max((uint)GetCreatureSkill(Skill.ArmorTinkering).Current, Math.Max((uint)GetCreatureSkill(Skill.MagicItemTinkering).Current, Math.Max((uint)GetCreatureSkill(Skill.WeaponTinkering).Current, (uint)GetCreatureSkill(Skill.ItemTinkering).Current))));
            double workAverage = 0;
            int materialType = 0;
            int amount = 0;
            int numItems = 0;
            int value = 0;
            if (GetCharacterOption(CharacterOption.SalvageMultipleMaterialsAtOnce))
            {
                int counter = 0;
                int objectCounter = 0;
                WorldObject[] salvageBags = new WorldObject[list.Count()];
                WorldObject[] items = new WorldObject[list.Count()];
                int[] materials = new int[list.Count()];
                foreach (var guid in list)
                {
                    bool inMaterialsList = false;
                    var item = GetInventoryItem(guid);
                    items[objectCounter] = item;
                    objectCounter++;
                    int materialsPlace = 0;
                    foreach (int a in materials)
                    {
                        if (item.GetProperty(PropertyInt.MaterialType) == a)
                        {
                            inMaterialsList = true;
                            break;
                        }
                        materialsPlace++;
                    }
                    if (!inMaterialsList)
                    {
                        numItems = 0;
                        value = 0;
                        amount = 0;
                        materialType = 0;
                        materials[counter] = item.GetProperty(PropertyInt.MaterialType) ?? 0;
                        materialType = item.GetProperty(PropertyInt.MaterialType) ?? 0;
                        int workmanship = item.GetProperty(PropertyInt.ItemWorkmanship) ?? 0;
                        int numItemsinThisItem = item.GetProperty(PropertyInt.NumItemsInMaterial) ?? 1;
                        if (numItemsinThisItem > 1)
                        {
                            value += item.GetProperty(PropertyInt.Value) ?? 0;
                        }
                        else
                        {
                            value += (int)((item.GetProperty(PropertyInt.Value) ?? 0) * (salvageSkill / 387.0));
                        }
                        if (value > 75000)
                        {
                            value = 75000;
                        }
                        numItems += numItemsinThisItem;
                        workAverage += (double)workmanship;
                        double multiplier = (salvageSkill / 225.0);
                        double multiplier2 = .6 > multiplier ? .6 : multiplier;
                        amount += (int)Math.Ceiling(workmanship * multiplier2);
                        TryConsumeFromInventoryWithNetworking(item, 1);
                        salvageBags[counter] = WorldObjectFactory.CreateNewWorldObject((uint)dict[materials[counter]]);
                        salvageBags[counter].SetProperty(PropertyInt.Structure, amount);
                        salvageBags[counter].SetProperty(PropertyInt.NumItemsInMaterial, numItems);
                        salvageBags[counter].SetProperty(PropertyInt.ItemWorkmanship, amount);
                        salvageBags[counter].SetProperty(PropertyString.Name, "Salvage (" + amount + ")");
                        salvageBags[counter].SetProperty(PropertyInt.Value, value);
                        counter++;
                    }
                    else
                    {
                        materialType = item.GetProperty(PropertyInt.MaterialType) ?? 0;
                        int workmanship = item.GetProperty(PropertyInt.ItemWorkmanship) ?? 0;
                        int numItemsinThisItem = item.GetProperty(PropertyInt.NumItemsInMaterial) ?? 1;
                        if (numItemsinThisItem > 1)
                        {
                            value += item.GetProperty(PropertyInt.Value) ?? 0;
                        }
                        else
                        {
                            value += (int)((item.GetProperty(PropertyInt.Value) ?? 0) * (salvageSkill / 387.0));
                        }
                        if (value > 75000)
                        {
                            value = 75000;
                        }
                        numItems += numItemsinThisItem;
                        workAverage += (double)workmanship;
                        double multiplier = (salvageSkill / 225.0);
                        double multiplier2 = .6 > multiplier ? .6 : multiplier;
                        amount += (int)Math.Ceiling(workmanship * multiplier2);
                        TryConsumeFromInventoryWithNetworking(item, 1);
                        salvageBags[materialsPlace].SetProperty(PropertyInt.Structure, amount);
                        salvageBags[materialsPlace].SetProperty(PropertyInt.NumItemsInMaterial, numItems);
                        salvageBags[materialsPlace].SetProperty(PropertyInt.ItemWorkmanship, amount);
                        salvageBags[materialsPlace].SetProperty(PropertyString.Name, "Salvage (" + amount + ")");
                        salvageBags[materialsPlace].SetProperty(PropertyInt.Value, value);
                    }

                }
                foreach (WorldObject wo2 in salvageBags)
                {
                    if (wo2 != null)
                    {
                        Console.WriteLine("The name of this bag is " + wo2.Name);
                        TryCreateInInventoryWithNetworking(wo2);
                    }
                }
            }
            else
            {

                foreach (var guid in list)
                {
                    var item = GetInventoryItem(guid);
                    materialType = item.GetProperty(PropertyInt.MaterialType) ?? 0;
                    int workmanship = item.GetProperty(PropertyInt.ItemWorkmanship) ?? 0;
                    int numItemsinThisItem = item.GetProperty(PropertyInt.NumItemsInMaterial) ?? 1;
                    if (numItemsinThisItem > 1)
                    {
                        value += item.GetProperty(PropertyInt.Value) ?? 0;
                    }
                    else
                    {
                        value += (int)((item.GetProperty(PropertyInt.Value) ?? 0) * (salvageSkill / 387.0));
                    }
                    if (value > 75000)
                    {
                        value = 75000;
                    }
                    numItems += numItemsinThisItem;
                    workAverage += (double)workmanship;
                    double multiplier = (salvageSkill / 225.0);
                    double multiplier2 = .6 > multiplier ? .6 : multiplier;
                    amount += (int)Math.Ceiling(workmanship * multiplier2);
                    TryConsumeFromInventoryWithNetworking(item, 1);
                }

                WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)dict[materialType]);
                wo.SetProperty(PropertyInt.Structure, amount);
                wo.SetProperty(PropertyInt.NumItemsInMaterial, numItems);
                wo.SetProperty(PropertyInt.ItemWorkmanship, amount);
                wo.SetProperty(PropertyString.Name, "Salvage (" + amount + ")");
                wo.SetProperty(PropertyInt.Value, value);
                TryCreateInInventoryWithNetworking(wo);
            }
        }
    }
}
