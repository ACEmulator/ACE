using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// A lookup table for MaterialType => Salvage Bag WCIDs
        /// </summary>
        public static Dictionary<int, int> MaterialSalvage = new Dictionary<int, int>()
        {
            {1, 20983},     // Ceramic
            {2, 21067},     // Porcelain
            {3, 0},         // ======= Cloth =======
            {4, 20987},     // Linen
            {5, 20992},     // Satin
            {6, 21076},     // Silk
            {7, 20994},     // Velvet
            {8, 20995},     // Wool
            {9, 0},         // ======= Gems =======
            {10, 21034},    // Agate
            {11, 21035},    // Amber
            {12, 21036},    // Amethyst
            {13, 21037},    // Aquamarine
            {14, 21038},    // Azurite
            {15, 21039},    // Black Garnet
            {16, 21040},    // Black Opal
            {17, 21041},    // Bloodstone
            {18, 21043},    // Carnelian
            {19, 21044},    // Citrine
            {20, 21046},    // Diamond
            {21, 21048},    // Emerald
            {22, 21049},    // Fire Opal
            {23, 21050},    // Green Garnet
            {24, 21051},    // Green Jade
            {25, 21053},    // Hematite
            {26, 21054},    // Imperial Topaz
            {27, 21056},    // Jet
            {28, 21057},    // Lapis Lazuli
            {29, 21058},    // Lavender Jade
            {30, 21060},    // Malachite
            {31, 21062},    // Moonstone
            {32, 21064},    // Onyx
            {33, 21065},    // Opal
            {34, 21066},    // Peridot
            {35, 21069},    // Red Garnet
            {36, 21070},    // Red Jade
            {37, 21071},    // Rose Quartz
            {38, 21072},    // Ruby
            {39, 21074},    // Sapphire
            {40, 21078},    // Smokey Quartz
            {41, 21079},    // Sunstone
            {42, 21081},    // Tiger Eye
            {43, 21082},    // Tourmaline
            {44, 21083},    // Turquoise
            {45, 21084},    // White Jade
            {46, 21085},    // White Quartz
            {47, 21086},    // White Sapphire
            {48, 21087},    // Yellow Garnet
            {49, 21088},    // Yellow Topaz
            {50, 21089},    // Zircon
            {51, 21055},    // Ivory
            {52, 21059},    // Leather
            {53, 20981},    // Armoredillo Hide
            {54, 21052},    // Gromnie Hide
            {55, 20991},    // Reedshark Hide
            {56, 0},        // ======= Metal =======
            {57, 21042},    // Brass
            {58, 20982},    // Bronze
            {59, 21045},    // Copper
            {60, 20984},    // Gold
            {61, 20986},    // Iron
            {62, 21068},    // Pyreal
            {63, 21077},    // Silver
            {64, 20993},    // Steel
            {65, 0},        // ======= Stone =======
            {66, 20980},    // Alabaster
            {67, 20985},    // Granite
            {68, 21061},    // Marble
            {69, 21063},    // Obsidian
            {70, 21073},    // Sandstone
            {71, 21075},    // Serpentine
            {72, 0},        // ======= Wood =======
            {73, 21047},    // Ebony
            {74, 20988},    // Mahogany
            {75, 20989},    // Oak
            {76, 20990},    // Pine
            {77, 21080}     // Teak
        };

        /// <summary>
        /// Returns the skill with the largest current value (buffed)
        /// </summary>
        public CreatureSkill GetMaxSkill(List<Skill> skills)
        {
            CreatureSkill maxSkill = null;
            foreach (var skill in skills)
            {
                var creatureSkill = GetCreatureSkill(skill);
                if (maxSkill == null || creatureSkill.Current > maxSkill.Current)
                    maxSkill = creatureSkill;
            }
            return maxSkill;
        }

        public static List<Skill> TinkeringSkills = new List<Skill>()
        {
            Skill.ArmorTinkering,
            Skill.WeaponTinkering,
            Skill.ItemTinkering,
            Skill.MagicItemTinkering
        };

        public void HandleSalvaging(List<uint> salvageItems)
        {
            var salvageBags = new List<WorldObject>();
            var salvageResults = new SalvageResults();

            foreach (var itemGuid in salvageItems)
            {
                var item = GetInventoryItem(itemGuid);
                if (item == null)
                {
                    log.Warn($"{Name}.HandleSalvaging({itemGuid}): couldn't find inventory item");
                    continue;
                }

                if (item.MaterialType == null)
                {
                    log.Warn($"{Name}.HandleSalvaging({item.Name}): no material type");
                    continue;
                }

                if (item.Workmanship == null || (item.Retained ?? false)) continue;

                AddSalvage(salvageBags, item, salvageResults);

                // can any salvagable items be stacked?
                TryConsumeFromInventoryWithNetworking(item);
            }

            // add salvage bags
            foreach (var salvageBag in salvageBags)
                TryCreateInInventoryWithNetworking(salvageBag);

            // send network messages
            foreach (var kvp in salvageResults.GetMessages())
                Session.Network.EnqueueSend(new GameEventSalvageOperationsResult(Session, kvp.Key, kvp.Value));
        }

        public void AddSalvage(List<WorldObject> salvageBags, WorldObject item, SalvageResults salvageResults)
        {
            var materialType = (MaterialType)item.MaterialType;

            // determine the amount of salvage produced (structure)
            SalvageMessage message = null;
            var amountProduced = GetStructure(item, salvageResults, ref message);

            var remaining = amountProduced;

            while (remaining > 0)
            {
                // get the destination salvage bag

                // if there are no existing salvage bags for this material type,
                // of all of the salvage bags for this material type are full,
                // this will create a new salvage bag, and adds it to salvageBags

                var salvageBag = GetSalvageBag(materialType, salvageBags);

                var added = TryAddSalvage(salvageBag, item, remaining);
                remaining -= added;

                // increase value of salvage bag - salvage skill is a factor,
                // if bags aren't being combined here
                var valueFactor = (float)added / amountProduced;
                if (item.WeenieType != WeenieType.CraftTool)
                    valueFactor *= GetCreatureSkill(Skill.Salvaging).Current / 387.0f;  // TODO: take augs into account

                var addedValue = (int)Math.Round((item.Value ?? 0) * valueFactor);

                salvageBag.Value = Math.Min((salvageBag.Value ?? 0) + addedValue, 75000);

                // a bit different here, since ACE handles overages
                if (message != null)
                {
                    message.Workmanship += item.GetProperty(PropertyInt.ItemWorkmanship) ?? 0.0f;
                    message.NumItemsInMaterial++;
                }
            }
        }

        public int TryAddSalvage(WorldObject salvageBag, WorldObject item, int tryAmount)
        {
            var maxStructure = salvageBag.MaxStructure ?? 100;
            var structure = salvageBag.Structure ?? 0;

            var space = maxStructure - structure;

            var amount = Math.Min(tryAmount, space);

            salvageBag.Structure = (ushort)(structure + amount);

            // add workmanship
            var item_numItems = item.StackSize ?? 1;
            var workmanship_bag = salvageBag.GetProperty(PropertyInt.ItemWorkmanship) ?? 0;
            var workmanship_item = item.GetProperty(PropertyInt.ItemWorkmanship) ?? 0;
            salvageBag.SetProperty(PropertyInt.ItemWorkmanship, workmanship_bag + workmanship_item * item_numItems);

            // increment # of items that went into this salvage bag
            if (item.WeenieType == WeenieType.CraftTool)
            {
                item_numItems = item.NumItemsInMaterial ?? 1;

                // handle overflows when combining bags
                if (tryAmount > space)
                {
                    var scalar = (float)space / tryAmount;
                    var newItems = (int)Math.Ceiling(item_numItems * scalar);
                    scalar = (float)newItems / item_numItems;
                    item_numItems = newItems;

                    salvageBag.SetProperty(PropertyInt.ItemWorkmanship, salvageBag.GetProperty(PropertyInt.ItemWorkmanship).Value - (int)Math.Round(workmanship_item * (1.0 - scalar)));

                    // and for the next bag...
                    item.SetProperty(PropertyInt.ItemWorkmanship, item.GetProperty(PropertyInt.ItemWorkmanship).Value - (int)Math.Round(workmanship_item * scalar));
                    item.NumItemsInMaterial -= newItems;
                }
            }
            salvageBag.NumItemsInMaterial = (salvageBag.NumItemsInMaterial ?? 0) + item_numItems;

            salvageBag.Name = $"Salvage ({salvageBag.Structure})";

            return amount;
        }

        public int GetStructure(WorldObject salvageItem, SalvageResults salvageResults, ref SalvageMessage message)
        {
            // By default, salvaging uses either a tinkering skill, or your salvaging skill that would yield the greatest amount of material.
            // Tinkering skills can only yield at most the workmanship number in units of salvage.
            // The salvaging skill can produce more units than workmanship.

            // You can also significantly increase the amount of material returned by training the Ciandra's Fortune augmentation.
            // This augmentation can be trained 4 times, each time providing an additional 25% bonus to the amount of material returned.

            // is this a bag of salvage?
            // if so, return its existing structure
            if (salvageItem.WeenieType == WeenieType.CraftTool)
                return salvageItem.Structure.Value;

            var workmanship = salvageItem.Workmanship ?? 1.0f;
            var stackSize = salvageItem.StackSize ?? 1;

            // should this be getting the highest tinkering skill,
            // or the tinkering skill for the material?
            var salvageSkill = GetCreatureSkill(Skill.Salvaging).Current;
            var highestTinkeringSkill = GetMaxSkill(TinkeringSkills).Current;

            var salvageMultiplier = Math.Max(0.6f, salvageSkill / 225.0f);
            var tinkeringMultiplier = Math.Max(0.6f, highestTinkeringSkill / 225.0f);

            // take augs into account for salvaging only
            var augMod = 1.0f;
            if (AugmentationBonusSalvage > 0)
                augMod += AugmentationBonusSalvage * 0.25f;

            var fSalvageAmount = workmanship * salvageMultiplier * stackSize * augMod;
            var fTinkeringAmount = workmanship * tinkeringMultiplier * stackSize;

            var salvageAmount = fSalvageAmount.Round();
            var tinkeringAmount = Math.Min(fTinkeringAmount.Round(), (int)Math.Round(salvageItem.Workmanship ?? 1.0f));

            // choose the best one
            var addStructure = Math.Max(salvageAmount, tinkeringAmount);

            var skill = salvageAmount > tinkeringAmount ? Skill.Salvaging : GetMaxSkill(TinkeringSkills).Skill;

            message = salvageResults.GetMessage(salvageItem.MaterialType ?? ACE.Entity.Enum.MaterialType.Unknown, skill);
            message.Amount += (uint)addStructure;

            if (skill == Skill.Salvaging && augMod > 1.0f)
                message.AugBonus += (int)Math.Round(fSalvageAmount - fSalvageAmount / augMod);

            return addStructure;
        }

        public WorldObject GetSalvageBag(MaterialType materialType, List<WorldObject> salvageBags)
        {
            // first try finding the first non-filled salvage bag, for this material type
            var existing = salvageBags.FirstOrDefault(i => (i.GetProperty(PropertyInt.MaterialType) ?? 0) == (int)materialType && (i.Structure ?? 0) < (i.MaxStructure ?? 0));

            if (existing != null)
                return existing;

            // not found - create a new salvage bag
            var wcid = (uint)MaterialSalvage[(int)materialType];
            var salvageBag = WorldObjectFactory.CreateNewWorldObject(wcid);

            salvageBags.Add(salvageBag);

            return salvageBag;
        }
    }
}
