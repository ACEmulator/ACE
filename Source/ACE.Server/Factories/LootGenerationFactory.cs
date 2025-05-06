using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;
using ACE.Server.Factories.Tables;
using ACE.Server.Factories.Tables.Wcids;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Used for cumulative ServerPerformanceMonitor event recording
        //private static readonly ThreadLocal<Stopwatch> stopwatch = new ThreadLocal<Stopwatch>(() => new Stopwatch());

        static LootGenerationFactory()
        {
            InitRares();
            InitClothingColors();
        }

        public static List<WorldObject> CreateRandomLootObjects(TreasureDeath profile)
        {
            //stopwatch.Value.Restart();

            try
            {
                int numItems;
                WorldObject lootWorldObject;

                var loot = new List<WorldObject>();

                var itemChance = ThreadSafeRandom.Next(1, 100);
                if (itemChance <= profile.ItemChance)
                {
                    numItems = ThreadSafeRandom.Next(profile.ItemMinAmount, profile.ItemMaxAmount);

                    for (var i = 0; i < numItems; i++)
                    {
                        lootWorldObject = CreateRandomLootObjects(profile, TreasureItemCategory.Item);

                        if (lootWorldObject != null)
                            loot.Add(lootWorldObject);
                    }
                }

                itemChance = ThreadSafeRandom.Next(1, 100);
                if (itemChance <= profile.MagicItemChance)
                {
                    numItems = ThreadSafeRandom.Next(profile.MagicItemMinAmount, profile.MagicItemMaxAmount);

                    for (var i = 0; i < numItems; i++)
                    {
                        lootWorldObject = CreateRandomLootObjects(profile, TreasureItemCategory.MagicItem);

                        if (lootWorldObject != null)
                            loot.Add(lootWorldObject);
                    }
                }

                itemChance = ThreadSafeRandom.Next(1, 100);
                if (itemChance <= profile.MundaneItemChance)
                {
                    numItems = ThreadSafeRandom.Next(profile.MundaneItemMinAmount, profile.MundaneItemMaxAmount);

                    for (var i = 0; i < numItems; i++)
                    {
                        lootWorldObject = CreateRandomLootObjects(profile, TreasureItemCategory.MundaneItem);

                        if (lootWorldObject != null)
                            loot.Add(lootWorldObject);
                    }

                    // extra roll for mundane:
                    // https://asheron.fandom.com/wiki/Announcements_-_2010/04_-_Shedding_Skin :: May 5th, 2010 entry
                    // aetheria and coalesced mana were handled in here
                    lootWorldObject = TryRollMundaneAddon(profile);

                    if (lootWorldObject != null)
                        loot.Add(lootWorldObject);
                }

                return loot;
            }
            finally
            {
                //ServerPerformanceMonitor.AddToCumulativeEvent(ServerPerformanceMonitor.CumulativeEventHistoryType.LootGenerationFactory_CreateRandomLootObjects, stopwatch.Value.Elapsed.TotalSeconds);
            }
        }

        public static WorldObject CreateRandomLootObjects(TreasureDeath treasureDeath, TreasureItemCategory category, TreasureItemType treasureItemType = TreasureItemType.Undef)
        {
            var treasureRoll = RollWcid(treasureDeath, category, treasureItemType);

            if (treasureRoll == null) return null;

            var wo = CreateAndMutateWcid(treasureDeath, treasureRoll, category == TreasureItemCategory.MagicItem);

            return wo;
        }

        private static TreasureRoll RollWcid(TreasureDeath treasureDeath, TreasureItemCategory category, TreasureItemType treasureItemType = TreasureItemType.Undef)
        {
            if (treasureItemType == TreasureItemType.Undef)
                treasureItemType = RollItemType(treasureDeath, category);

            if (treasureItemType == TreasureItemType.Undef)
            {
                log.Error($"LootGenerationFactory.RollWcid({treasureDeath.TreasureType}, {category}): treasureItemType == Undef");
                return null;
            }

            var treasureRoll = new TreasureRoll(treasureItemType);

            // TODO: quality mod
            switch (treasureItemType)
            {
                case TreasureItemType.Pyreal:

                    treasureRoll.Wcid = WeenieClassName.coinstack;
                    break;

                case TreasureItemType.Gem:

                    var gemClass = GemClassChance.Roll(treasureDeath.Tier);
                    var gemResult = GemMaterialChance.Roll(gemClass);

                    treasureRoll.Wcid = gemResult.ClassName;
                    break;

                case TreasureItemType.Jewelry:

                    treasureRoll.Wcid = JewelryWcids.Roll(treasureDeath.Tier);
                    break;

                case TreasureItemType.ArtObject:

                    treasureRoll.Wcid = GenericWcids.Roll(treasureDeath.Tier);
                    break;

                case TreasureItemType.Weapon:

                    treasureRoll.WeaponType = WeaponTypeChance.Roll(treasureDeath.Tier);
                    treasureRoll.Wcid = WeaponWcids.Roll(treasureDeath, ref treasureRoll.WeaponType);
                    break;

                case TreasureItemType.Armor:

                    treasureRoll.ArmorType = ArmorTypeChance.Roll(treasureDeath.Tier);
                    treasureRoll.Wcid = ArmorWcids.Roll(treasureDeath, ref treasureRoll.ArmorType);
                    break;

                case TreasureItemType.Clothing:

                    treasureRoll.Wcid = ClothingWcids.Roll(treasureDeath);
                    break;

                case TreasureItemType.Scroll:

                    treasureRoll.Wcid = ScrollWcids.Roll();
                    break;

                case TreasureItemType.Caster:

                    // only called if TreasureItemType.Caster was specified directly
                    treasureRoll.WeaponType = TreasureWeaponType.Caster;
                    treasureRoll.Wcid = CasterWcids.Roll(treasureDeath.Tier);
                    break;

                case TreasureItemType.ManaStone:

                    treasureRoll.Wcid = ManaStoneWcids.Roll(treasureDeath);
                    break;

                case TreasureItemType.Consumable:

                    treasureRoll.Wcid = ConsumeWcids.Roll(treasureDeath);
                    break;

                case TreasureItemType.HealKit:

                    treasureRoll.Wcid = HealKitWcids.Roll(treasureDeath);
                    break;

                case TreasureItemType.Lockpick:

                    treasureRoll.Wcid = LockpickWcids.Roll(treasureDeath);
                    break;

                case TreasureItemType.SpellComponent:

                    treasureRoll.Wcid = SpellComponentWcids.Roll(treasureDeath);
                    break;

                case TreasureItemType.SocietyArmor:
                case TreasureItemType.SocietyBreastplate:
                case TreasureItemType.SocietyGauntlets:
                case TreasureItemType.SocietyGirth:
                case TreasureItemType.SocietyGreaves:
                case TreasureItemType.SocietyHelm:
                case TreasureItemType.SocietyPauldrons:
                case TreasureItemType.SocietyTassets:
                case TreasureItemType.SocietyVambraces:
                case TreasureItemType.SocietySollerets:

                    treasureRoll.ItemType = TreasureItemType.SocietyArmor;     // collapse for mutation
                    treasureRoll.ArmorType = TreasureArmorType.Society;

                    treasureRoll.Wcid = SocietyArmorWcids.Roll(treasureDeath, treasureItemType);
                    break;

                case TreasureItemType.Cloak:

                    treasureRoll.Wcid = CloakWcids.Roll();
                    break;

                case TreasureItemType.PetDevice:

                    treasureRoll.Wcid = PetDeviceWcids.Roll(treasureDeath);
                    break;

                case TreasureItemType.EncapsulatedSpirit:

                    treasureRoll.Wcid = WeenieClassName.ace49485_encapsulatedspirit;
                    break;
            }
            return treasureRoll;
        }

        /// <summary>
        /// Rolls for an overall item type, based on the *_Chances columns in the treasure_death profile
        /// </summary>
        private static TreasureItemType RollItemType(TreasureDeath treasureDeath, TreasureItemCategory category)
        {
            switch (category)
            {
                case TreasureItemCategory.Item:
                    return TreasureProfile_Item.Roll(treasureDeath.ItemTreasureTypeSelectionChances);

                case TreasureItemCategory.MagicItem:
                    return TreasureProfile_MagicItem.Roll(treasureDeath.MagicItemTreasureTypeSelectionChances);

                case TreasureItemCategory.MundaneItem:
                    return TreasureProfile_Mundane.Roll(treasureDeath.MundaneItemTypeSelectionChances);
            }
            return TreasureItemType.Undef;
        }

        private static WorldObject CreateAndMutateWcid(TreasureDeath treasureDeath, TreasureRoll treasureRoll, bool isMagical)
        {
            WorldObject wo = null;

            if (treasureRoll.ItemType != TreasureItemType.Scroll)
            {
                wo = WorldObjectFactory.CreateNewWorldObject((uint)treasureRoll.Wcid);

                if (wo == null)
                {
                    log.Error($"CreateAndMutateWcid({treasureDeath.TreasureType}, {(int)treasureRoll.Wcid} - {treasureRoll.Wcid}, {treasureRoll.GetItemType()}, {isMagical}) - failed to create item");
                    return null;
                }

                treasureRoll.BaseArmorLevel = wo.ArmorLevel ?? 0;
            }

            switch (treasureRoll.ItemType)
            {
                case TreasureItemType.Pyreal:
                    MutateCoins(wo, treasureDeath);
                    break;
                case TreasureItemType.Gem:
                    MutateGem(wo, treasureDeath, isMagical, treasureRoll);
                    break;
                case TreasureItemType.Jewelry:

                    if (!treasureRoll.HasArmorLevel(wo))
                        MutateJewelry(wo, treasureDeath, isMagical, treasureRoll);
                    else
                    {
                        // crowns, coronets, diadems, etc.
                        MutateArmor(wo, treasureDeath, isMagical, treasureRoll);
                    }
                    break;
                case TreasureItemType.ArtObject:
                    MutateDinnerware(wo, treasureDeath, isMagical, treasureRoll);
                    break;

                case TreasureItemType.Weapon:

                    switch (treasureRoll.WeaponType)
                    {
                        case TreasureWeaponType.Axe:
                        case TreasureWeaponType.Dagger:
                        case TreasureWeaponType.DaggerMS:
                        case TreasureWeaponType.Mace:
                        case TreasureWeaponType.MaceJitte:
                        case TreasureWeaponType.Spear:
                        case TreasureWeaponType.Staff:
                        case TreasureWeaponType.Sword:
                        case TreasureWeaponType.SwordMS:
                        case TreasureWeaponType.Unarmed:

                        case TreasureWeaponType.TwoHandedAxe:
                        case TreasureWeaponType.TwoHandedMace:
                        case TreasureWeaponType.TwoHandedSpear:
                        case TreasureWeaponType.TwoHandedSword:

                            MutateMeleeWeapon(wo, treasureDeath, isMagical, treasureRoll);
                            break;

                        case TreasureWeaponType.Caster:

                            MutateCaster(wo, treasureDeath, isMagical, treasureRoll);
                            break;

                        case TreasureWeaponType.Bow:
                        case TreasureWeaponType.Crossbow:
                        case TreasureWeaponType.Atlatl:

                            MutateMissileWeapon(wo, treasureDeath, isMagical, treasureRoll);
                            break;

                        default:
                            log.Error($"CreateAndMutateWcid({treasureDeath.TreasureType}, {(int)treasureRoll.Wcid} - {treasureRoll.Wcid}, {treasureRoll.GetItemType()}, {isMagical}) - unknown weapon type");
                            break;
                    }
                    break;

                case TreasureItemType.Caster:

                    // alternate path -- only called if TreasureItemType.Caster was specified directly
                    MutateCaster(wo, treasureDeath, isMagical, treasureRoll);
                    break;

                case TreasureItemType.Armor:
                case TreasureItemType.Clothing:
                case TreasureItemType.SocietyArmor:    // collapsed, after rolling for initial wcid

                    MutateArmor(wo, treasureDeath, isMagical, treasureRoll);
                    break;

                case TreasureItemType.Scroll:
                    wo = CreateRandomScroll(treasureDeath, treasureRoll);     // using original method
                    break;

                case TreasureItemType.Cloak:
                    MutateCloak(wo, treasureDeath, treasureRoll);
                    break;

                case TreasureItemType.PetDevice:
                    MutatePetDevice(wo, treasureDeath.Tier);
                    break;

                    // other mundane items (mana stones, food/drink, healing kits, lockpicks, and spell components/peas) don't get mutated
            }
            return wo;
        }

        private static WorldObject TryRollMundaneAddon(TreasureDeath profile)
        {
            // coalesced mana only dropped in tiers 1-4
            if (profile.Tier <= 4)
                return TryRollCoalescedMana(profile);

            // aetheria dropped in tiers 5+
            else
                return TryRollAetheria(profile);
        }

        private static WorldObject TryRollCoalescedMana(TreasureDeath profile)
        {
            // 2% chance in here, which turns out to be less per corpse w/ MundaneItemChance > 0,
            // when the outer MundaneItemChance roll is factored in

            // loot quality mod?
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            if (rng < 0.02f)
                return CreateCoalescedMana(profile);
            else
                return null;
        }

        private static WorldObject TryRollAetheria(TreasureDeath profile)
        {
            var aetheria_drop_rate = (float)PropertyManager.GetDouble("aetheria_drop_rate").Item;

            if (aetheria_drop_rate <= 0.0f)
                return null;

            var dropRateMod = 1.0f / aetheria_drop_rate;

            // 2% base chance in here, which turns out to be less per corpse w/ MundaneItemChance > 0,
            // when the outer MundaneItemChance roll is factored in

            // loot quality mod?
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f * dropRateMod);

            if (rng < 0.02f)
                return CreateAetheria(profile);
            else
                return null;
        }

        /// <summary>
        /// Returns an appropriate material type for the World Object based on its loot tier.
        /// </summary>
        private static MaterialType GetMaterialType(WorldObject wo, int tier)
        {
            if (wo.TsysMutationData == null)
            {
                log.Warn($"[LOOT] Missing PropertyInt.TsysMutationData on loot item {wo.WeenieClassId} - {wo.Name}");
                return GetDefaultMaterialType(wo);
            }

            int materialCode = (int)wo.TsysMutationData & 0xFF;

            // Enforce some bounds
            // Data only goes to Tier 6 at the moment... Just in case the loot gem goes above this first, we'll cap it here for now.
            tier = Math.Clamp(tier, 1, 6);

            var materialBase = DatabaseManager.World.GetCachedTreasureMaterialBase(materialCode, tier);

            if (materialBase == null)
                return GetDefaultMaterialType(wo);

            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            float probability = 0.0f;
            foreach (var m in materialBase)
            {
                probability += m.Probability;
                if (rng < probability)
                {
                    // Ivory is unique... It doesn't have a group
                    if (m.MaterialId == (uint)MaterialType.Ivory)
                        return (MaterialType)m.MaterialId;

                    var materialGroup = DatabaseManager.World.GetCachedTreasureMaterialGroup((int)m.MaterialId, tier);

                    if (materialGroup == null)
                        return GetDefaultMaterialType(wo);

                    var groupRng = ThreadSafeRandom.Next(0.0f, 1.0f);
                    float groupProbability = 0.0f;
                    foreach (var g in materialGroup)
                    {
                        groupProbability += g.Probability;
                        if (groupRng < groupProbability)
                            return (MaterialType)g.MaterialId;
                    }
                    break;
                }
            }
            return GetDefaultMaterialType(wo);
        }

        /// <summary>
        /// Gets a randomized default material type for when a weenie does not have TsysMutationData 
        /// </summary>
        private static MaterialType GetDefaultMaterialType(WorldObject wo)
        {
            if (wo == null)
                return MaterialType.Unknown;

            MaterialType material = MaterialType.Unknown;
            int defaultMaterialEntry = ThreadSafeRandom.Next(0, 4);

            WeenieType weenieType = wo.WeenieType;
            switch (weenieType)
            {
                case WeenieType.Caster:
                    material = (MaterialType)LootTables.DefaultMaterial[3][defaultMaterialEntry];
                    break;
                case WeenieType.Clothing:
                    if (wo.ItemType == ItemType.Armor)
                        material = (MaterialType)LootTables.DefaultMaterial[0][defaultMaterialEntry];
                    if (wo.ItemType == ItemType.Clothing)
                        material = (MaterialType)LootTables.DefaultMaterial[5][defaultMaterialEntry];
                    break;
                case WeenieType.MissileLauncher:
                case WeenieType.Missile:
                    material = (MaterialType)LootTables.DefaultMaterial[1][defaultMaterialEntry];
                    break;
                case WeenieType.MeleeWeapon:
                    material = (MaterialType)LootTables.DefaultMaterial[2][defaultMaterialEntry];
                    break;
                case WeenieType.Generic:
                    if (wo.ItemType == ItemType.Jewelry)
                        material = (MaterialType)LootTables.DefaultMaterial[3][defaultMaterialEntry];
                    if (wo.ItemType == ItemType.MissileWeapon)
                        material = (MaterialType)LootTables.DefaultMaterial[4][defaultMaterialEntry];
                    break;
                default:
                    material = MaterialType.Unknown;
                    break;
            }

            return material;
        }

        private static List<TreasureMaterialColor> clothingColors { get; } = new List<TreasureMaterialColor>();

        private static void InitClothingColors()
        {
            for (uint i = 1; i < 19; i++)
            {
                TreasureMaterialColor tmc = new TreasureMaterialColor
                {
                    PaletteTemplate = i,
                    Probability = 1
                };
                clothingColors.Add(tmc);
            }
        }

        /// <summary>
        /// Assign a random color (Int.PaletteTemplate and Float.Shade) to a World Object based on the material assigned to it.
        /// </summary>
        /// <returns>WorldObject with a random applicable PaletteTemplate and Shade applied, if available</returns>
        private static void MutateColor(WorldObject wo)
        {
            if (wo.MaterialType > 0 && wo.TsysMutationData != null && wo.ClothingBase != null)
            {
                byte colorCode = (byte)(wo.TsysMutationData.Value >> 16);

                // BYTE spellCode = (tsysMutationData >> 24) & 0xFF;
                // BYTE colorCode = (tsysMutationData >> 16) & 0xFF;
                // BYTE gemCode = (tsysMutationData >> 8) & 0xFF;
                // BYTE materialCode = (tsysMutationData >> 0) & 0xFF;

                List<TreasureMaterialColor> colors = DatabaseManager.World.GetCachedTreasureMaterialColors((int)wo.MaterialType, colorCode);

                if (colors == null)
                {
                    // legacy support for hardcoded colorCode 0 table
                    if (colorCode == 0 && (uint)wo.MaterialType > 0)
                    {
                        // This is a unique situation that typically applies to Under Clothes.
                        // If the Color Code is 0, they can be PaletteTemplate 1-18, assuming there is a MaterialType
                        // (gems have ColorCode of 0, but also no MaterialCode as they are defined by the weenie)

                        // this can be removed after all servers have upgraded to latest db
                        colors = clothingColors;
                    }
                    else
                        return;
                }

                // Load the clothingBase associated with the WorldObject
                DatLoader.FileTypes.ClothingTable clothingBase = DatLoader.DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.ClothingTable>((uint)wo.ClothingBase);

                // TODO : Probably better to use an intersect() function here. I defer to someone who knows how these work better than I - Optim
                // Compare the colors list and the clothingBase PaletteTemplates and remove any invalid items
                var colorsValid = new List<TreasureMaterialColor>();
                foreach (var e in colors)
                    if (clothingBase.ClothingSubPalEffects.ContainsKey(e.PaletteTemplate))
                        colorsValid.Add(e);
                colors = colorsValid;

                float totalProbability = colors?.Sum(i => i.Probability) ?? 0.0f;
                // If there's zero chance to get a random color, no point in continuing.
                if (totalProbability == 0) return;

                var rng = ThreadSafeRandom.Next(0.0f, totalProbability);

                uint paletteTemplate = 0;
                float probability = 0.0f;
                // Loop through the colors until we've reach our target value
                foreach (var color in colors)
                {
                    probability += color.Probability;
                    if (rng < probability)
                    {
                        paletteTemplate = color.PaletteTemplate;
                        break;
                    }
                }

                if (paletteTemplate > 0)
                {
                    var cloSubPal = clothingBase.ClothingSubPalEffects[paletteTemplate];
                    // Make sure this entry has a valid icon, otherwise there's likely something wrong with the ClothingBase value for this WorldObject (e.g. not supposed to be a loot item)
                    if (cloSubPal.Icon > 0)
                    {
                        // Assign the appropriate Icon and PaletteTemplate
                        wo.IconId = cloSubPal.Icon;
                        wo.PaletteTemplate = (int)paletteTemplate;

                        // Throw some shade, at random
                        wo.Shade = ThreadSafeRandom.Next(0.0f, 1.0f);

                        // Some debug info...
                        // log.Info($"Color success for {wo.MaterialType}({(int)wo.MaterialType}) - {wo.WeenieClassId} - {wo.Name}. PaletteTemplate {paletteTemplate} applied.");
                    }
                }
                else
                {
                    log.Warn($"[LOOT] Color looked failed for {wo.MaterialType} ({(int)wo.MaterialType}) - {wo.WeenieClassId} - {wo.Name}.");
                }
            }
        }

        private static MaterialType RollGemType(int tier)
        {
            // previous formula
            //return (MaterialType)ThreadSafeRandom.Next(10, 50);

            // the gem class value can be further utilized for determining the item's monetary value
            var gemClass = GemClassChance.Roll(tier);

            var gemResult = GemMaterialChance.Roll(gemClass);

            return gemResult.MaterialType;
        }

        private const float WeaponBulk = 0.50f;
        private const float ArmorBulk = 0.25f;

        private static bool MutateBurden(WorldObject wo, TreasureDeath treasureDeath, bool isWeapon)
        {
            // ensure item has burden
            if (wo.EncumbranceVal == null)
                return false;

            var qualityInterval = QualityChance.RollInterval(treasureDeath);

            // only continue if the initial roll to modify the quality succeeded
            if (qualityInterval == 0.0f)
                return false;

            // only continue if initial roll succeeded?
            var bulk = isWeapon ? WeaponBulk : ArmorBulk;
            bulk *= (float)(wo.BulkMod ?? 1.0f);

            var maxBurdenMod = 1.0f - bulk;

            var burdenMod = 1.0f - (qualityInterval * maxBurdenMod);

            // modify burden
            var prevBurden = wo.EncumbranceVal.Value;
            wo.EncumbranceVal = (int)Math.Round(prevBurden * burdenMod);

            if (wo.EncumbranceVal < 1)
                wo.EncumbranceVal = 1;

            //Console.WriteLine($"Modified burden from {prevBurden} to {wo.EncumbranceVal} for {wo.Name} ({wo.WeenieClassId})");

            return true;
        }

        private static void MutateValue(WorldObject wo, int tier, TreasureRoll roll)
        {
            if (wo.Value == null)
                wo.Value = 0;   // fixme: data

            //var weenieValue = wo.Value;

            if (!(wo is Gem))
            {
                if (wo.HasArmorLevel())
                    MutateValue_Armor(wo);

                MutateValue_Generic(wo, tier);
            }
            else
                MutateValue_Gem(wo);

            MutateValue_Spells(wo);

            /*Console.WriteLine($"Mutating value for {wo.Name} ({weenieValue:N0} -> {wo.Value:N0})");

            // compare with previous function
            var materialMod = LootTables.getMaterialValueModifier(wo);
            var gemMod = LootTables.getGemMaterialValueModifier(wo);

            var rngRange = itemValue_RandomRange[tier - 1];

            var minValue = (int)(rngRange.min * gemMod * materialMod * Math.Ceiling(tier / 2.0f));
            var maxValue = (int)(rngRange.max * gemMod * materialMod * Math.Ceiling(tier / 2.0f));

            Console.WriteLine($"Previous ACE range: {minValue:N0} - {maxValue:N0}");*/
        }

        // increase for a wider variance in item value ranges
        private const float valueFactor = 1.0f / 3.0f;

        private const float valueNonFactor = 1.0f - valueFactor;

        private static void MutateValue_Generic(WorldObject wo, int tier)
        {
            // confirmed from retail magloot logs, matches up relatively closely

            var rng = (float)ThreadSafeRandom.Next(0.7f, 1.25f);

            var workmanshipMod = WorkmanshipChance.GetModifier(wo.ItemWorkmanship);

            var materialMod = MaterialTable.GetValueMod(wo.MaterialType);
            var gemValue = GemMaterialChance.GemValue(wo.GemType);

            var tierMod = ItemValue_TierMod[Math.Clamp(tier, 1, 8) - 1];

            var newValue = (int)wo.Value * valueFactor + materialMod * tierMod + gemValue;

            newValue *= (workmanshipMod /* + qualityMod */ ) * rng;

            newValue += (int)wo.Value * valueNonFactor;

            int iValue = (int)Math.Ceiling(newValue);

            // only raise value?
            if (iValue > wo.Value)
                wo.Value = iValue;
        }

        private static void MutateValue_Spells(WorldObject wo)
        {
            if (wo.ItemMaxMana != null)
                wo.Value += wo.ItemMaxMana * 2;

            int spellLevelSum = 0;

            if (wo.SpellDID != null)
            {
                var spell = new Server.Entity.Spell(wo.SpellDID.Value);
                spellLevelSum += (int)spell.Level;
            }

            if (wo.Biota.PropertiesSpellBook != null)
            {
                foreach (var spellId in wo.Biota.PropertiesSpellBook.Keys)
                {
                    var spell = new Server.Entity.Spell(spellId);
                    spellLevelSum += (int)spell.Level;
                }
            }
            wo.Value += spellLevelSum * 10;
        }

        private static readonly List<int> ItemValue_TierMod = new List<int>()
        {
            25,     // T1
            50,     // T2
            100,    // T3
            250,    // T4
            500,    // T5
            1000,   // T6
            2000,   // T7
            3000,   // T8
        };

        /// <summary>
        /// The min/max amount of pyreals that can be rolled per tier, from magloot corpse logs
        /// </summary>
        private static readonly List<(int min, int max)> coinRanges = new List<(int, int)>()
        {
            (5,   50),   // T1
            (10,  200),  // T2
            (10,  500),  // T3
            (25,  1000), // T4
            (50,  5000), // T5
            (250, 5000), // T6
            (250, 5000), // T7
            (250, 5000), // T8
        };

        private static void MutateCoins(WorldObject wo, TreasureDeath profile)
        {
            var tierRange = coinRanges[profile.Tier - 1];

            // flat rng range, according to magloot corpse logs
            var rng = ThreadSafeRandom.Next(tierRange.min, tierRange.max);

            wo.SetStackSize(rng);
        }

        public static string GetLongDesc(WorldObject wo)
        {
            if (wo.SpellDID != null)
            {
                var longDesc = TryGetLongDesc(wo, (SpellId)wo.SpellDID);

                if (longDesc != null)
                    return longDesc;
            }

            if (wo.Biota.PropertiesSpellBook != null)
            {
                foreach (var spellId in wo.Biota.PropertiesSpellBook.Keys)
                {
                    var longDesc = TryGetLongDesc(wo, (SpellId)spellId);

                    if (longDesc != null)
                        return longDesc;
                }
            }
            return wo.Name;
        }

        private static string TryGetLongDesc(WorldObject wo, SpellId spellId)
        {
            var spellLevels = SpellLevelProgression.GetSpellLevels(spellId);

            if (spellLevels != null && CasterSlotSpells.descriptors.TryGetValue(spellLevels[0], out var descriptor))
                return $"{wo.Name} of {descriptor}";
            else
                return null;
        }

        private static void RollWieldLevelReq_T7_T8(WorldObject wo, TreasureDeath profile)
        {
            if (profile.Tier < 7)
                return;

            var wieldLevelReq = 150;

            if (profile.Tier == 8)
            {
                // t8 had a 90% chance for 180
                // loot quality mod?
                var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

                if (rng < 0.9f)
                    wieldLevelReq = 180;
            }

            wo.WieldRequirements = WieldRequirement.Level;
            wo.WieldDifficulty = wieldLevelReq;

            // as per retail pcaps, must be set to appear in client
            wo.WieldSkillType = 1;  
        }

        /// <summary>
        /// This is only called by /cirand, and isn't really part of standard lootgen
        /// </summary>
        public static List<WorldObject> CreateRandomObjectsOfType(WeenieType type, int count)
        {
            var weenies = DatabaseManager.World.GetRandomWeeniesOfType((int)type, count);

            var worldObjects = new List<WorldObject>();

            foreach (var weenie in weenies)
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(weenie.WeenieClassId);
                worldObjects.Add(wo);
            }

            return worldObjects;
        }

        /// <summary>
        /// This is only called by /testlootgen command
        /// The actual lootgen system doesn't use this.
        /// </summary>
        public static WorldObject CreateRandomLootObjects_Test(TreasureDeath profile, bool isMagical, LootBias lootBias = LootBias.UnBiased)
        {
            WorldObject wo = null;

            var treasureItemTypeChances = isMagical ? TreasureItemTypeChances.DefaultMagical : TreasureItemTypeChances.DefaultNonMagical;

            switch (lootBias)
            {
                case LootBias.Armor:
                    treasureItemTypeChances = TreasureItemTypeChances.Armor;
                    break;
                case LootBias.Weapons:
                    treasureItemTypeChances = TreasureItemTypeChances.Weapons;
                    break;
                case LootBias.Jewelry:
                    treasureItemTypeChances = TreasureItemTypeChances.Jewelry;
                    break;

                case LootBias.MagicEquipment:
                case LootBias.MixedEquipment:
                    treasureItemTypeChances = TreasureItemTypeChances.MixedMagicEquipment;
                    break;
            }

            var treasureItemType = treasureItemTypeChances.Roll();

            switch (treasureItemType)
            {
                case TreasureItemType.Gem:
                    wo = CreateGem(profile, isMagical);
                    break;

                case TreasureItemType.Armor:
                    wo = CreateArmor(profile, isMagical, true);
                    break;

                case TreasureItemType.Clothing:
                    wo = CreateArmor(profile, isMagical, false);
                    break;

                case TreasureItemType.Cloak:
                    wo = CreateCloak(profile);
                    break;

                case TreasureItemType.Weapon:
                    wo = CreateWeapon(profile, isMagical);
                    break;

                case TreasureItemType.Jewelry:
                    wo = CreateJewelry(profile, isMagical);
                    break;

                case TreasureItemType.ArtObject:
                    // Added Dinnerware at tail end of distribution, as
                    // they are mutable loot drops that don't belong with the non-mutable drops
                    // TODO: Will likely need some adjustment/fine tuning
                    wo = CreateDinnerware(profile, isMagical);
                    break;
            }
            return wo;
        }

        /// <summary>
        /// This is only called by the /lootgen command
        /// Even though this is not called by normal gameplay, it should still produce functionally identical results.
        /// </summary>
        public static bool MutateItem(WorldObject item, TreasureDeath profile, bool isMagical)
        {
            // should ideally be split up between getting the item type,
            // and getting the specific mutate function parameters
            // however, with the way the current loot tables are set up, this is not ideal...

            // this function does a bunch of o(n) lookups through the loot tables,
            // and is only used for the /lootgen dev command currently
            // if this needs to be used in high performance scenarios, the collections for the loot tables will
            // will need to be updated to support o(1) queries

            // update: most of the o(n) lookup issues have been fixed,
            // however this is still looking into more hashtables than necessary.
            // ideally there should only be 1 hashtable that gets the roll.ItemType,
            // and any other necessary info (armorType / weaponType)
            // then just call the existing mutation method

            var roll = new TreasureRoll();

            roll.Wcid = (WeenieClassName)item.WeenieClassId;
            roll.BaseArmorLevel = item.ArmorLevel ?? 0;

            if (roll.Wcid == WeenieClassName.coinstack)
            {
                roll.ItemType = TreasureItemType.Pyreal;
                MutateCoins(item, profile);
            }
            else if (GemMaterialChance.Contains(roll.Wcid))
            {
                roll.ItemType = TreasureItemType.Gem;
                MutateGem(item, profile, isMagical, roll);
            }
            else if (JewelryWcids.Contains(roll.Wcid))
            {
                roll.ItemType = TreasureItemType.Jewelry;

                if (!roll.HasArmorLevel(item))
                    MutateJewelry(item, profile, isMagical, roll);
                else
                {
                    // crowns, coronets, diadems, etc.
                    MutateArmor(item, profile, isMagical, roll);
                }
            }
            else if (GenericWcids.Contains(roll.Wcid))
            {
                roll.ItemType = TreasureItemType.ArtObject;
                MutateDinnerware(item, profile, isMagical, roll);
            }
            else if (HeavyWeaponWcids.TryGetValue(roll.Wcid, out var weaponType) ||
                LightWeaponWcids.TryGetValue(roll.Wcid, out weaponType) ||
                FinesseWeaponWcids.TryGetValue(roll.Wcid, out weaponType) ||
                TwoHandedWeaponWcids.TryGetValue(roll.Wcid, out weaponType))
            {
                roll.ItemType = TreasureItemType.Weapon;
                roll.WeaponType = weaponType;
                MutateMeleeWeapon(item, profile, isMagical, roll);
            }
            else if (BowWcids_Aluvian.TryGetValue(roll.Wcid, out weaponType) ||
                BowWcids_Gharundim.TryGetValue(roll.Wcid, out weaponType) ||
                BowWcids_Sho.TryGetValue(roll.Wcid, out weaponType) ||
                CrossbowWcids.TryGetValue(roll.Wcid, out weaponType) ||
                AtlatlWcids.TryGetValue(roll.Wcid, out weaponType))
            {
                roll.ItemType = TreasureItemType.Weapon;
                roll.WeaponType = weaponType;
                MutateMissileWeapon(item, profile, isMagical, roll);
            }
            else if (CasterWcids.Contains(roll.Wcid))
            {
                roll.ItemType = TreasureItemType.Weapon;
                roll.WeaponType = TreasureWeaponType.Caster;
                MutateCaster(item, profile, isMagical, roll);
            }
            else if (ArmorWcids.TryGetValue(roll.Wcid, out var armorType))
            {
                roll.ItemType = TreasureItemType.Armor;
                roll.ArmorType = armorType;
                MutateArmor(item, profile, isMagical, roll);
            }
            else if (SocietyArmorWcids.Contains(roll.Wcid))
            {
                roll.ItemType = TreasureItemType.SocietyArmor;     // collapsed for mutation
                roll.ArmorType = TreasureArmorType.Society;
                MutateArmor(item, profile, isMagical, roll);
            }
            else if (ClothingWcids.Contains(roll.Wcid))
            {
                roll.ItemType = TreasureItemType.Clothing;
                MutateArmor(item, profile, isMagical, roll);
            }
            // scrolls don't really get mutated, even though they are in the main mutation method still
            else if (CloakWcids.Contains(roll.Wcid))
            {
                roll.ItemType = TreasureItemType.Cloak;
                MutateCloak(item, profile, roll);
            }
            else if (PetDeviceWcids.Contains(roll.Wcid))
            {
                roll.ItemType = TreasureItemType.PetDevice;
                MutatePetDevice(item, profile.Tier);
            }
            else if (AetheriaWcids.Contains(roll.Wcid))
            {
                // mundane add-on
                MutateAetheria(item, profile);
            }
            // other mundane items (mana stones, food/drink, healing kits, lockpicks, and spell components/peas) don't get mutated
            // it should be safe to return false here, for the 1 caller that currently uses this method
            // since it's not this function's responsibility to determine if an item is a lootgen item,
            // and only returns true if the item has been mutated.
            else
                return false;

            return true;
        }
    }
}
