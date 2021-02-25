using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Factories.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static class PlayerFactoryEx
    {
        private static readonly Random rand = new Random();

        /// <summary>
        /// Heritage: Gear Knight
        /// 10 for all attributes
        /// trained Creature/Item/Life/Mana Conversion. This will make sure the player starts off with foci
        /// </summary>
        private static readonly byte[] baseGearKnight =
        {
            0x01, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
            0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0xF0, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, 0x88, 0x2E, 0x44, 0x17, 0xA2, 0x0B,
            0xD1, 0x3F, 0xC7, 0xBF, 0xE3, 0xDF, 0xF1, 0xEF, 0xE8, 0x3F, 0xD4, 0x1E, 0x6A, 0x0F, 0xB5, 0x87, 0xDA, 0x3F,
            0xAD, 0x76, 0x56, 0x3B, 0xAB, 0x9D, 0xD5, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x0A, 0x00,
            0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x37, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00,
            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00,
            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00,
            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00,
            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x07, 0x00,
            0x4E, 0x6F, 0x20, 0x4E, 0x61, 0x6D, 0x65, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00
        };

        private static CharacterCreateInfo CreateCharacterCreateInfo(string name, uint strength, uint endurance, uint coordination, uint quickness, uint focus, uint self, bool randomizeHeritageAndApperance = true)
        {
            var characterCreateInfo = new CharacterCreateInfo();

            using (var memoryStream = new MemoryStream(baseGearKnight))
            using (var binaryReader = new BinaryReader(memoryStream))
                characterCreateInfo.Unpack(binaryReader);

            characterCreateInfo.Name = name;

            characterCreateInfo.StrengthAbility = strength;
            characterCreateInfo.EnduranceAbility = endurance;
            characterCreateInfo.CoordinationAbility = coordination;
            characterCreateInfo.QuicknessAbility = quickness;
            characterCreateInfo.FocusAbility = focus;
            characterCreateInfo.SelfAbility = self;

            if (randomizeHeritageAndApperance)
                RandomizeHeritage(characterCreateInfo);

            return characterCreateInfo;
        }

        private static void RandomizeHeritage(CharacterCreateInfo characterCreateInfo)
        {
            var heritage = (uint)rand.Next(1, 12);
            var heritageGroup = DatManager.PortalDat.CharGen.HeritageGroups[heritage];

            characterCreateInfo.Heritage = heritage;
            characterCreateInfo.Gender = (uint)heritageGroup.Genders.ElementAt(rand.Next(0, heritageGroup.Genders.Count)).Key;

            var sex = heritageGroup.Genders[(int)characterCreateInfo.Gender];

            characterCreateInfo.Apperance.HairColor = (uint)rand.Next(0, sex.HairColorList.Count);
            characterCreateInfo.Apperance.HairStyle = (uint)rand.Next(0, sex.HairStyleList.Count);

            characterCreateInfo.Apperance.Eyes = (uint)rand.Next(0, sex.EyeStripList.Count);
            characterCreateInfo.Apperance.EyeColor = (uint)rand.Next(0, sex.EyeColorList.Count);
            characterCreateInfo.Apperance.Nose = (uint)rand.Next(0, sex.NoseStripList.Count);
            characterCreateInfo.Apperance.Mouth = (uint)rand.Next(0, sex.MouthStripList.Count);

            // todo randomize skin
        }


        /// <summary>
        /// Creates a fully leveled/augmented 275 base character player
        /// </summary>
        private static Player Create275Base(CharacterCreateInfo characterCreateInfo, Weenie weenie, ObjectGuid guid, uint accountId)
        {
            PlayerFactory.Create(characterCreateInfo, weenie, guid, accountId, WeenieType.Creature, out var player);

            // Remove junk inventory
            player.TryDequipObject(player.EquippedObjects.FirstOrDefault(k => k.Value.Name.Contains("Leather Boots")).Key, out var wo, out _);
            if (wo != null)
                player.TryRemoveFromInventory(wo.Guid);

            player.TryRemoveFromInventory(player.Inventory.FirstOrDefault(k => k.Value.Name.Contains("Training Wand")).Key);
            player.TryRemoveFromInventory(player.Inventory.FirstOrDefault(k => k.Value.Name.Contains("Letter From Home")).Key);

            LevelUpPlayer(player);

            AddAllSpells(player);

            LoadDefaultSpellBars(player);

            return player;
        }

        private static void LevelUpPlayer(Player player)
        {
            player.AvailableExperience += 191226310247;
            player.TotalExperience += 191226310247;
            player.Level = 275;
            player.AvailableSkillCredits += 46;
            player.TotalSkillCredits += 46;

            // Playability Augs
            player.AugmentationExtraPackSlot = 1;
            player.AugmentationIncreasedCarryingCapacity = 5;
            player.AugmentationLessDeathItemLoss = 3;
            player.AugmentationSpellsRemainPastDeath = 1;
            player.AugmentationIncreasedSpellDuration = 5;
            player.AugmentationJackOfAllTrades = 1;

            // todo: Optionally add other augs
        }

        private static void LoadDefaultSpellBars(Player player)
        {
            // Recall Spells
            uint barNumber = 0;
            uint indexInBar = 0;
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2645, player.CharacterDatabaseLock); // Portal Recall
            player.Character.AddSpellToBar(barNumber, indexInBar++,   48, player.CharacterDatabaseLock); // Primary Portal Recall
            player.Character.AddSpellToBar(barNumber, indexInBar++,  157, player.CharacterDatabaseLock); // Summon Primary Portal I
            player.Character.AddSpellToBar(barNumber, indexInBar++,   47, player.CharacterDatabaseLock); // Primary Portal Tie
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2647, player.CharacterDatabaseLock); // Secondary Portal Recall
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2648, player.CharacterDatabaseLock); // Summon Secondary Portal I
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2646, player.CharacterDatabaseLock); // Secondary Portal Tie
            player.Character.AddSpellToBar(barNumber, indexInBar++, 1635, player.CharacterDatabaseLock); // Lifestone Recall
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2644, player.CharacterDatabaseLock); // Lifestone Tie

            player.Character.AddSpellToBar(barNumber, indexInBar++, 2041, player.CharacterDatabaseLock); // Aerlinthe Recall
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2931, player.CharacterDatabaseLock); // Recall Aphus Lassel
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2941, player.CharacterDatabaseLock); // Ulgrim's Recall
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3865, player.CharacterDatabaseLock); // Glenden Wood Recall
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4084, player.CharacterDatabaseLock); // Bur Recall
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4128, player.CharacterDatabaseLock); // Call of the Mhoire Forge
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4198, player.CharacterDatabaseLock); // Paradox-touched Olthoi
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4213, player.CharacterDatabaseLock); // Colosseum Recall
            player.Character.AddSpellToBar(barNumber, indexInBar++, 5330, player.CharacterDatabaseLock); // Gear Knight Invasion Area Camp Recall
            player.Character.AddSpellToBar(barNumber, indexInBar++, 5541, player.CharacterDatabaseLock); // Lost City of Neftet Recall
            player.Character.AddSpellToBar(barNumber, indexInBar++, 6321, player.CharacterDatabaseLock); // Viridian Rise Recall

            // Vitals
            barNumber++;
            indexInBar = 0;
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2073, player.CharacterDatabaseLock); // "Adja's Intervention","Restores 80-150 points of the caster's Health.
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2343, player.CharacterDatabaseLock); // "Rushed Recovery","Drains one-half of the caster's Stamina and gives 175% of that to his/her Health."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2083, player.CharacterDatabaseLock); // "Robustification","Restores 100-200 points of the caster's Stamina."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2345, player.CharacterDatabaseLock); // "Meditative Trance","Drains one-half of the caster's Stamina and gives 175% of that to his/her Mana."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3194, player.CharacterDatabaseLock); // "Eradicate Life Magic Self","Dispels 3-6 negative Life Magic enchantments of level 7 or lower from the caster."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 2072, player.CharacterDatabaseLock); // "Adja's Gift","Restores 80-150 points of the target's Health."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2082, player.CharacterDatabaseLock); // "Replenish","Restores 100-200 points of the target's Stamina."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2336, player.CharacterDatabaseLock); // "Gift of Essence","Drains one-quarter of the caster's Mana and gives 175% of that to the target."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3193, player.CharacterDatabaseLock); // "Eradicate Life Magic Other","Dispels 3-6 negative Life Magic enchantments of level 7 or lower from the target."

            // Buffs - Self
            barNumber++;
            indexInBar = 0;
            player.Character.AddSpellToBar(barNumber, indexInBar++,  562, player.CharacterDatabaseLock); // "Creature Enchantment Mastery Self VI","Increases the caster's Creature Enchantment skill by 35 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 1426, player.CharacterDatabaseLock); // "Focus Self VI","Increases the caster's Focus by 35 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 1450, player.CharacterDatabaseLock); // "Willpower Self VI","Increases the caster's Self by 35 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4325, player.CharacterDatabaseLock); // "Incantation of Strength Self","Increases the caster's Strength by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4299, player.CharacterDatabaseLock); // "Incantation of Endurance Self","Increases the caster's Endurance by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4297, player.CharacterDatabaseLock); // "Incantation of Coordination Self","Increases the caster's Coordination by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4319, player.CharacterDatabaseLock); // "Incantation of Quickness Self","Increases the caster's Quickness by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4305, player.CharacterDatabaseLock); // "Incantation of Focus Self","Increases the caster's Focus by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4329, player.CharacterDatabaseLock); // "Incantation of Willpower Self","Increases the caster's Self by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4496, player.CharacterDatabaseLock); // "Incantation of Regeneration Self","Increase caster's natural healing rate by 145%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4498, player.CharacterDatabaseLock); // "Incantation of Rejuvenation Self","Increases the rate at which the caster regains Stamina by 145%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4494, player.CharacterDatabaseLock); // "Incantation of Mana Renewal Self","Increases the caster's natural mana rate by 145%."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4530, player.CharacterDatabaseLock); // "Incantation of Creature Enchantment Mastery Self","Increases the caster's Creature Enchantment skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4582, player.CharacterDatabaseLock); // "Incantation of Life Magic Mastery Self","Increases the caster's Life Magic skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4564, player.CharacterDatabaseLock); // "Incantation of Item Enchantment Mastery Self","Increases the caster's Item Enchantment skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4602, player.CharacterDatabaseLock); // "Incantation of Mana Conversion Mastery Self","Increases the caster's Mana Conversion skill by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4510, player.CharacterDatabaseLock); // "Incantation of Arcane Enlightenment Self","Increases the caster's Arcane Lore skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4616, player.CharacterDatabaseLock); // "Incantation of Sprint Self","Increases the caster's Run skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4572, player.CharacterDatabaseLock); // "Incantation of Jumping Mastery Self","Increases the caster's Jump skill by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4548, player.CharacterDatabaseLock); // "Incantation of Fealty Self","Increases the caster's Loyalty skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4578, player.CharacterDatabaseLock); // "Incantation of Leadership Mastery Self","Increases the caster's Leadership skill by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4560, player.CharacterDatabaseLock); // "Incantation of Invulnerability Self","Increases the caster's Melee Defense skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4558, player.CharacterDatabaseLock); // "Incantation of Impregnability Self","Increases the caster's Missile Defense skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4596, player.CharacterDatabaseLock); // "Incantation of Magic Resistance Self","Increases the caster's Magic Defense skill by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4624, player.CharacterDatabaseLock); // "Incantation of Heavy Weapon Mastery Self","Increases the caster's Heavy Weapons skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4522, player.CharacterDatabaseLock); // "Incantation of Missile Weapon Mastery Self","Increases the caster's Missile Weapons skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4638, player.CharacterDatabaseLock); // "Incantation of War Magic Mastery Self","Increases the caster's War Magic skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 6123, player.CharacterDatabaseLock); // "Incantation of Summoning Mastery Self","Increases the caster's Summoning skill by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4556, player.CharacterDatabaseLock); // "Incantation of Healing Mastery Self","Increases the caster's Healing skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4552, player.CharacterDatabaseLock); // "Incantation of Fletching Mastery Self","Increases the caster's Fletching skill by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4468, player.CharacterDatabaseLock); // "Incantation of Fire Protection Self","Reduces damage the caster takes from Fire by 68%"
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4470, player.CharacterDatabaseLock); // "Incantation of Lightning Protection Self","Reduces damage the caster takes from Lightning by 68%"
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4462, player.CharacterDatabaseLock); // "Incantation of Blade Protection Self","Reduces damage the caster takes from Slashing by 68%"
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4472, player.CharacterDatabaseLock); // "Incantation of Piercing Protection Self","Reduces damage the caster takes from Piercing by 68%"
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4464, player.CharacterDatabaseLock); // "Incantation of Bludgeoning Protection Self","Reduces damage the caster takes from Bludgeoning by 68%"
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4466, player.CharacterDatabaseLock); // "Incantation of Cold Protection Self","Reduces damage the caster takes from Cold by 68%"
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4460, player.CharacterDatabaseLock); // "Incantation of Acid Protection Self","Reduces damage the caster takes from acid by 68%"

            // Buffs - Item
            barNumber++;
            indexInBar = 0;
            player.Character.AddSpellToBar(barNumber, indexInBar++, 5183, player.CharacterDatabaseLock); // "Aura of Incantation of Blood Drinker Self","Increases a weapon's damage value by 24 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4414, player.CharacterDatabaseLock); // "Aura of Incantation of Spirit Drinker Self","Increases the elemental damage bonus of an elemental magic caster by 8%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4417, player.CharacterDatabaseLock); // "Aura of Incantation of Swift Killer Self","Improves a weapon's speed by 80 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4405, player.CharacterDatabaseLock); // "Aura of Incantation of Heart Seeker Self","Increases a weapon's Attack Skill modifier by 20.0 percentage points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4400, player.CharacterDatabaseLock); // "Aura of Incantation of Defender Self","Increases the Melee Defense skill modifier of a weapon or magic caster by 20%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4418, player.CharacterDatabaseLock); // "Aura of Incantation of Hermetic Link Self","Increases a magic casting implement's mana conversion bonus by 80%."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4407, player.CharacterDatabaseLock); // "Incantation of Impenetrability","Improves a shield or piece of armor's armor value by 240 points. Target yourself to cast this spell on all of your equipped armor."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4401, player.CharacterDatabaseLock); // "Incantation of Flame Bane","Increases a shield or piece of armor's resistance to fire damage by 200%. Target yourself to cast this spell on all of your equipped armor."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4409, player.CharacterDatabaseLock); // "Incantation of Lightning Bane","Increases a shield or piece of armor's resistance to electric damage by 200%. Target yourself to cast this spell on all of your equipped armor."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4393, player.CharacterDatabaseLock); // "Incantation of Blade Bane","Increases a shield or piece of armor's resistance to slashing damage by 200%. Target yourself to cast this spell on all of your equipped armor."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4412, player.CharacterDatabaseLock); // "Incantation of Piercing Bane","Increases a shield or piece of armor's resistance to piercing damage by 200%. Target yourself to cast this spell on all of your equipped armor."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4397, player.CharacterDatabaseLock); // "Incantation of Bludgeon Bane","Increases a shield or piece of armor's resistance to bludgeoning damage by 200%. Target yourself to cast this spell on all of your equipped armor."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4403, player.CharacterDatabaseLock); // "Incantation of Frost Bane","Increases a shield or piece of armor's resistance to cold damage by 200%. Target yourself to cast this spell on all of your equipped armor."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4391, player.CharacterDatabaseLock); // "Incantation of Acid Bane","Increases a shield or piece of armor's resistance to acid damage by 200%. Target yourself to cast this spell on all of your equipped armor."

            // Buffs - Other
            barNumber++;
            indexInBar = 0;
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4324, player.CharacterDatabaseLock); // "Incantation of Strength Other","Increases the target's Strength by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4298, player.CharacterDatabaseLock); // "Incantation of Endurance Other","Increases the target's Endurance by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4296, player.CharacterDatabaseLock); // "Incantation of Coordination Other","Increases the target's Coordination by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4318, player.CharacterDatabaseLock); // "Incantation of Quickness Other","Increases the target's Quickness by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4304, player.CharacterDatabaseLock); // "Incantation of Focus Other","Increases the target's Focus by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4328, player.CharacterDatabaseLock); // "Incantation of Willpower Self","Increases the caster's Self by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4495, player.CharacterDatabaseLock); // "Incantation of Regeneration Other","Increase target's natural healing rate by 145%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4497, player.CharacterDatabaseLock); // "Incantation of Rejuvenation Other","Increases the rate at which the target regains Stamina by 145%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4493, player.CharacterDatabaseLock); // "Incantation of Mana Renewal Other","Increases the target's natural mana rate by 145%."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4529, player.CharacterDatabaseLock); // "Incantation of Creature Enchantment Mastery Other","Increases the target's Creature Enchantment skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4581, player.CharacterDatabaseLock); // "Incantation of Life Magic Mastery Other","Increases the target's Life Magic skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4563, player.CharacterDatabaseLock); // "Incantation of Item Enchantment Mastery Other","Increases the target's Item Enchantment skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4601, player.CharacterDatabaseLock); // "Incantation of Mana Conversion Mastery Other","Increases the target's Mana Conversion skill by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4509, player.CharacterDatabaseLock); // "Incantation of Arcane Enlightenment Other","Increases the target's Arcane Lore skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4615, player.CharacterDatabaseLock); // "Incantation of Sprint Other","Increases the target's Run skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4571, player.CharacterDatabaseLock); // "Incantation of Jumping Mastery Other","Increases the target's Jump skill by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4547, player.CharacterDatabaseLock); // "Incantation of Fealty Other","Increases the target's Loyalty skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4577, player.CharacterDatabaseLock); // "Incantation of Leadership Mastery Other","Increases the target's Leadership skill by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4559, player.CharacterDatabaseLock); // "Incantation of Invulnerability Other","Increases the target's Melee Defense skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4557, player.CharacterDatabaseLock); // "Incantation of Impregnability Other","Increases the target's Missile Defense skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4595, player.CharacterDatabaseLock); // "Incantation of Magic Resistance Other","Increases the target's Magic Defense skill by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4623, player.CharacterDatabaseLock); // "Incantation of Heavy Weapon Mastery Other","Increases the target's Heavy Weapons skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4521, player.CharacterDatabaseLock); // "Incantation of Missile Weapon Mastery Other","Increases the target's Missile Weapons skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4637, player.CharacterDatabaseLock); // "Incantation of War Magic Mastery Other","Increases the target's War Magic skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 6115, player.CharacterDatabaseLock); // "Incantation of Summoning Mastery Other","Increases the target's Summoning skill by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4555, player.CharacterDatabaseLock); // "Incantation of Healing Mastery Other","Increases the target's Healing skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4551, player.CharacterDatabaseLock); // "Incantation of Fletching Mastery Other","Increases the target's Fletching skill by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4467, player.CharacterDatabaseLock); // "Incantation of Fire Protection Other","Reduces damage the target takes from fire by 68%"
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4469, player.CharacterDatabaseLock); // "Incantation of Lightning Protection Other","Reduces damage the target takes from Lightning by 68%"
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4461, player.CharacterDatabaseLock); // "Incantation of Blade Protection Other","Reduces damage the target takes from Slashing by 68%"
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4471, player.CharacterDatabaseLock); // "Incantation of Piercing Protection Other","Reduces damage the target takes from Piercing by 68%"
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4463, player.CharacterDatabaseLock); // "Incantation of Bludgeoning Protection Other","Reduces damage the target takes from Bludgeoning by 68%"
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4465, player.CharacterDatabaseLock); // "Incantation of Cold Protection Other","Reduces damage the target takes from Cold by 68%"
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4459, player.CharacterDatabaseLock); // "Incantation of Acid Protection Other","Reduces damage the target takes from acid by 68%"

            player.Character.AddSpellToBar(barNumber, indexInBar++, 5997, player.CharacterDatabaseLock); // "Aura of Incantation of Blood Drinker Other","Increases a weapon's damage value by 24 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 6022, player.CharacterDatabaseLock); // "Aura of Incantation of Spirit Drinker Other","Increases the elemental damage bonus of an elemental magic caster by 8%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 6031, player.CharacterDatabaseLock); // "Aura of Incantation of Swift Killer Other","Improves a weapon's speed by 80 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 6014, player.CharacterDatabaseLock); // "Aura of Incantation of Heart Seeker Other","Increases a weapon's Attack Skill modifier by 20.0 percentage points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 6006, player.CharacterDatabaseLock); // "Aura of Incantation of Defender Other","Increases the Melee Defense skill modifier of a weapon or magic caster by 17%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 5989, player.CharacterDatabaseLock); // "Aura of Incantation of Hermetic Link Other","Increases a magic casting implement's mana conversion bonus by 80%."

            // Fellowship
            barNumber++;
            indexInBar = 0;
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3178, player.CharacterDatabaseLock); // "Superior Empowering the Conclave","Enhances the Strength of all Fellowship members by 40 points for 60 minutes."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3162, player.CharacterDatabaseLock); // "Superior Vivify the Conclave","Enhances the Endurance of all Fellowship members by 40 points for 60 minutes."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3158, player.CharacterDatabaseLock); // "Superior Alacrity of the Conclave","Enhances the Coordination of all Fellowship members by 40 points for 60 minutes."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3170, player.CharacterDatabaseLock); // "Superior Speed the Conclave","Enhances the Quickness of all Fellowship members by 40 points for 60 minutes."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3166, player.CharacterDatabaseLock); // "Superior Acumen of the Conclave","Enhances the Focus of all Fellowship members by 40 points for 60 minutes."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3174, player.CharacterDatabaseLock); // "Superior Volition of the Conclave","Enhances the Self of all Fellowship members by 40 points for 60 minutes."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 3477, player.CharacterDatabaseLock); // "Superior Soothing Wind","Enhances the blood flow and aids in knitting wounds closed. All fellowship member receive a 115% increase to their natural health recovery rate."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3481, player.CharacterDatabaseLock); // "Superior Golden Wind","Enhances the intake of air and utilization of energy. All fellowship member receive a 115% increase to their natural stamina recovery rate."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3473, player.CharacterDatabaseLock); // "Superior Endless Well","Enhances the understanding of the ebb and flow of mana. All fellowship members received a 115% increase to their nautral mana recovery rate."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 3387, player.CharacterDatabaseLock); // "Superior Conjurant Chant","Enhances the Creature Enchantment skill of all Fellowship members by 40 points for 60 minutes."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3395, player.CharacterDatabaseLock); // "Superior Vitaeic Chant","Enhances the Life Magic skill of all Fellowship members by 40 points for 60 minutes."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3391, player.CharacterDatabaseLock); // "Superior Artificant Chant","Enhances the Item Enchantment skill of all Fellowship members by 40 points for 60 minutes."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3399, player.CharacterDatabaseLock); // "Superior Conveyic Chant","Enhances the Mana Conversion skill of all Fellowship members by 40 points for 60 minutes."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 3355, player.CharacterDatabaseLock); // "Potent Guardian of the Clutch","Enhances the Melee Defense of all Fellowship members by 40 points for 60 minutes."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3351, player.CharacterDatabaseLock); // "Potent Warden of the Clutch","Enhances the Missile Defense of all Fellowship members by 40 points for 60 minutes."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3359, player.CharacterDatabaseLock); // "Potent Sanctifier of the Clutch","Enhances the Magic Defense of all Fellowship members by 40 points for 60 minutes."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 3403, player.CharacterDatabaseLock); // "Superior Hieromantic Chant","Enhances the War Magic skill of all Fellowship members by 40 points for 60 minutes."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 3339, player.CharacterDatabaseLock); // "Superior Inferno Ward","Reduces damage all fellowship members take from fire by 65%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3343, player.CharacterDatabaseLock); // "Superior Voltaic Ward","Reduces damage all fellowship members take from Lightning by 65%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3327, player.CharacterDatabaseLock); // "Superior Scythe Ward","Reduces damage all fellowship members take from Slashing by 65%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3347, player.CharacterDatabaseLock); // "Superior Lance Ward","Reduces damage all fellowship members take from Piercing by 65%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3331, player.CharacterDatabaseLock); // "Superior Flange Ward","Reduces damage all fellowship members from Bludgeoning by 65%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3335, player.CharacterDatabaseLock); // "Superior Frore Ward","Reduces damage all fellowship members take from Cold by 65%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 3323, player.CharacterDatabaseLock); // "Superior Corrosive Ward","Reduces damage all fellowship members take from acid by 65%."

            // Debuffs
            barNumber++;
            indexInBar = 0;
            player.Character.AddSpellToBar(barNumber, indexInBar++, 2074, player.CharacterDatabaseLock); // "Gossamer Flesh","Decreases the target's natural armor by 225 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4481, player.CharacterDatabaseLock); // "Incantation of Fire Vulnerability Other","Increases damage the target takes from Fire by 210%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4483, player.CharacterDatabaseLock); // "Incantation of Lightning Vulnerability Other","Increases damage the target takes from Lightning by 210%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4475, player.CharacterDatabaseLock); // "Incantation of Blade Vulnerability Other","Increases damage the target takes from Slashing by 210%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4485, player.CharacterDatabaseLock); // "Incantation of Piercing Vulnerability Other","Increases damage the target takes from Piercing by 210%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4477, player.CharacterDatabaseLock); // "Incantation of Bludgeoning Vulnerability Other","Increases damage the target takes from Bludgeoning by 210%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4479, player.CharacterDatabaseLock); // "Incantation of Cold Vulnerability Other","Increases damage the target takes from Cold by 210%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4473, player.CharacterDatabaseLock); // "Incantation of Acid Vulnerability Other","Increases damage the target takes from acid by 210%."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4633, player.CharacterDatabaseLock); // "Incantation of Vulnerability Other","Decrease the target's Melee Defense skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4597, player.CharacterDatabaseLock); // "Incantation of Magic Yield Other","Decreases the target's Magic Defense skill by 45 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4543, player.CharacterDatabaseLock); // "Incantation of Defenselessness Other","Decreases the target's Missile Defense skill by 45 points."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4402, player.CharacterDatabaseLock); // "Incantation of Flame Lure","Decreases a shield or piece of armor's resistance to fire damage by 200%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4410, player.CharacterDatabaseLock); // "Incantation of Lightning Lure","Decreases a shield or piece of armor's resistance to electric damage by 200%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4394, player.CharacterDatabaseLock); // "Incantation of Blade Lure","Decreases a shield or piece of armor's resistance to slashing damage by 200%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4413, player.CharacterDatabaseLock); // "Incantation of Piercing Lure","Decreases a shield or piece of armor's resistance to piercing damage by 200%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4398, player.CharacterDatabaseLock); // "Incantation of Bludgeon Lure","Decreases a shield or piece of armor's resistance to bludgeoning damage by 200%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4404, player.CharacterDatabaseLock); // "Incantation of Frost Lure","Decreases a shield or piece of armor's resistance to cold damage by 200%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4392, player.CharacterDatabaseLock); // "Incantation of Acid Lure","Decreases a shield or piece of armor's resistance to acid damage by 200%."

            player.Character.AddSpellToBar(barNumber, indexInBar++, 4396, player.CharacterDatabaseLock); // "Incantation of Blood Loather","Decreases a weapon's damage value by 24 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4419, player.CharacterDatabaseLock); // "Incantation of Turn Blade","Decreases a weapon's Attack Skill modifier by 20.0 percentage points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4411, player.CharacterDatabaseLock); // "Incantation of Lure Blade","Decreases the Melee Defense skill modifier of a weapon or magic caster by 20%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4408, player.CharacterDatabaseLock); // "Incantation of Leaden Weapon","Worsens a weapon's speed by 80 points."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4415, player.CharacterDatabaseLock); // "Incantation of Spirit Loather","Decreases the elemental damage bonus of an elemental magic caster by 8%."
            player.Character.AddSpellToBar(barNumber, indexInBar++, 4406, player.CharacterDatabaseLock); // "Incantation of Hermetic Void","Decreases a magic casting implement's mana conversion bonus by 80%."
        }

        private static void LoadSkillSpecificDefaultSpellBar(Player player)
        {
            if (player.Skills.ContainsKey(Skill.WarMagic) && player.Skills[Skill.WarMagic].AdvancementClass == SkillAdvancementClass.Specialized)
            {
                // War
                uint barNumber = 7;
                uint indexInBar = 0;

                player.Character.AddSpellToBar(barNumber, indexInBar++, 4423, player.CharacterDatabaseLock); // "Incantation of Flame Arc","Shoots a bolt of flame at the target.  The bolt does 142-204 points of fire damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4426, player.CharacterDatabaseLock); // "Incantation of Lightning Arc","Shoots a bolt of lighting at the target. The bolt does 142-204 points of electrical damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4422, player.CharacterDatabaseLock); // "Incantation of Blade Arc","Shoots a magical blade at the target. The bolt does 142-204 points of slashing damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4424, player.CharacterDatabaseLock); // "Incantation of Force Arc","Shoots a bolt of force at the target. The bolt does 142-204 points of piercing damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4427, player.CharacterDatabaseLock); // "Incantation of Shock Arc","Shoots a shock wave at the target. The wave does 142-204 points of bludgeoning damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4421, player.CharacterDatabaseLock); // "Incantation of Acid Arc","Shoots a stream of acid at the target. The stream does 142-204 points of acid damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4425, player.CharacterDatabaseLock); // "Incantation of Frost Arc","Shoots a bolt of cold at the target. The bolt does 142-204 points of cold damage to the first thing it hits."

                player.Character.AddSpellToBar(barNumber, indexInBar++, 4439, player.CharacterDatabaseLock); // "Incantation of Flame Bolt","Shoots a bolt of flame at the target.  The bolt does 142-204 points of fire damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4451, player.CharacterDatabaseLock); // "Incantation of Lightning Bolt","Shoots a bolt of lighting at the target. The bolt does 142-204 points of electrical damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4457, player.CharacterDatabaseLock); // "Incantation of Whirling Blade","Shoots a magical blade at the target. The bolt does 142-204 points of slashing damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4443, player.CharacterDatabaseLock); // "Incantation of Force Bolt","Shoots a bolt of force at the target. The bolt does 142-204 points of piercing damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4455, player.CharacterDatabaseLock); // "Incantation of Shock Wave","Shoots a shock wave at the target. The wave does 142-204 points of bludgeoning damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4447, player.CharacterDatabaseLock); // "Incantation of Frost Bolt","Shoots a bolt of cold at the target. The bolt does 142-204 points of cold damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4433, player.CharacterDatabaseLock); // "Incantation of Acid Stream","Shoots a stream of acid at the target. The stream does 142-204 points of acid damage to the first thing it hits."

                player.Character.AddSpellToBar(barNumber, indexInBar++, 4440, player.CharacterDatabaseLock); // "Incantation of Flame Streak","Sends a bolt of flame streaking towards the target.  The bolt does 47-94 points of fire damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4452, player.CharacterDatabaseLock); // "Incantation of Lightning Streak","Sends a bolt of lighting streaking towards the target. The bolt does 47-94 points of electrical damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4458, player.CharacterDatabaseLock); // "Incantation of Whirling Blade Streak","Sends a magical blade streaking towards the target. The bolt does 47-94 points of slashing damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4444, player.CharacterDatabaseLock); // "Incantation of Force Streak","Sends a bolt of force streaking towards the target. The bolt does 47-94 points of piercing damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4456, player.CharacterDatabaseLock); // "Incantation of Shock Wave Streak","Sends a shock wave streaking towards the target. The wave does 47-94 points of bludgeoning damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4432, player.CharacterDatabaseLock); // "Incantation of Acid Streak","Sends a stream of acid streaking towards the target. The stream does 47-94 points of acid damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 4448, player.CharacterDatabaseLock); // "Incantation of Frost Streak","Sends a bolt of cold streaking towards the target. The bolt does 47-94 points of cold damage to the first thing it hits."

                player.Character.AddSpellToBar(barNumber, indexInBar++, 2934, player.CharacterDatabaseLock); // "Tusker Fists","A hail of tusker fists pummels a clear path ahead of the caster."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 1785, player.CharacterDatabaseLock); // "Cassius' Ring of Fire","Shoots eight waves of flame outward from the caster. Each wave does 42-84 points of fire damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 1788, player.CharacterDatabaseLock); // "Eye of the Storm","Shoots eight waves of lightning outward from the caster. Each wave does 42-84 points of electric damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 1784, player.CharacterDatabaseLock); // "Horizon's Blades","Shoots eight blades outward from the caster. Each blade does 42-84 points of slashing damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 1786, player.CharacterDatabaseLock); // "Nuhmudira's Spines","Shoots eight waves of force outward from the caster. Each wave does 42-84 points of piercing damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 1789, player.CharacterDatabaseLock); // "Tectonic Rifts","Shoots eight shock waves outward from the caster. Each wave does 42-84 points of bludgeoning damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 1787, player.CharacterDatabaseLock); // "Halo of Frost","Shoots eight waves of frost outward from the caster. Each wave does 42-84 points of cold damage to the first thing it hits."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 1783, player.CharacterDatabaseLock); // "Searing Disc","Shoots eight waves of acid outward from the caster. Each wave does 42-84 points of acid damage to the first thing it hits.",

                player.Character.AddSpellToBar(barNumber, indexInBar++, 4428, player.CharacterDatabaseLock); // "Incantation of Martyr's Hecatomb","Drains one-quarter of the caster's health into a bolt of energy.  When struck by the bolt, the target's health is reduced by 200% of the amount drained."
                player.Character.AddSpellToBar(barNumber, indexInBar++, 3818, player.CharacterDatabaseLock); // "Curse of Raven Fury","Drains half of the caster�s health and projects a ring of vicious energy outwards. When struck, the target�s health is reduced by 200% of the amount drained from the caster."
            }
        }


        /// <summary>
        /// Creates a fully leveled 275 Heavy Weapons character player
        /// No augmentations are included
        /// </summary>
        public static Player Create275HeavyWeapons(Weenie weenie, ObjectGuid guid, uint accountId, string name)
        {
            var characterCreateInfo = CreateCharacterCreateInfo(name, 100, 10, 100, 100, 10, 10);

            var player = Create275Base(characterCreateInfo, weenie, guid, accountId);

            // Trained skills
            player.TrainSkill(Skill.HeavyWeapons, 6);
            player.TrainSkill(Skill.Healing, 6);
            player.TrainSkill(Skill.MeleeDefense, 10);
            player.TrainSkill(Skill.MissileDefense, 6);
            player.TrainSkill(Skill.Shield, 2);

            // Specialized skills
            player.SpecializeSkill(Skill.HeavyWeapons, 6);
            player.SpecializeSkill(Skill.Healing, 4);
            player.SpecializeSkill(Skill.MagicDefense, 12);
            player.SpecializeSkill(Skill.MeleeDefense, 10);
            player.SpecializeSkill(Skill.Shield, 2);

            // 0 remaining skill points.
            // If/When we add the 4 skill points in LevelUpPlayer, we can spend them here as well

            LoadSkillSpecificDefaultSpellBar(player);

            // todo aug endurance

            SpendAllXp(player);

            AddCommonInventory(player, RelicAlduressa);

            // Create a dummy treasure profile for passing in tier value
            var profile = new Database.Models.World.TreasureDeath
            {
                Tier = 7,
                LootQualityMod = 0
            };

            for (int i = 0; i < 12; i++)
            {
                var item = LootGenerationFactory.CreateMeleeWeapon(profile, true, MeleeWeaponSkill.HeavyWeapons);
                AddRend(item);
                player.TryAddToInventory(item);
            }

            return player;
        }

        /// <summary>
        /// Creates a fully leveled 275 Missile Weapons character player
        /// No augmentations are included
        /// </summary>
        public static Player Create275MissileWeapons(Weenie weenie, ObjectGuid guid, uint accountId, string name)
        {
            var characterCreateInfo = CreateCharacterCreateInfo(name, 10, 100, 100, 10, 10, 100);

            var player = Create275Base(characterCreateInfo, weenie, guid, accountId);

            // Trained skills
            player.TrainSkill(Skill.Healing, 6);
            player.TrainSkill(Skill.MeleeDefense, 10);
            player.TrainSkill(Skill.MissileDefense, 6);
            player.TrainSkill(Skill.MissileWeapons, 6);
            player.TrainSkill(Skill.Fletching, 4);

            // Specialized skills
            player.SpecializeSkill(Skill.MagicDefense, 12);
            player.SpecializeSkill(Skill.MeleeDefense, 10);
            player.SpecializeSkill(Skill.MissileWeapons, 6);
            player.SpecializeSkill(Skill.Fletching, 4);

            // 0 remaining skill points.
            // If/When we add the 4 skill points in LevelUpPlayer, we can spend them here as well

            LoadSkillSpecificDefaultSpellBar(player);

            // todo aug what attribute?

            SpendAllXp(player);

            AddCommonInventory(player, NobleRelic);

            // Create a dummy treasure profile for passing in tier value
            var profile = new Database.Models.World.TreasureDeath
            {
                Tier = 7,
                LootQualityMod = 0
            };

            for (int i = 0; i < 12; i++)
            {
                var item = LootGenerationFactory.CreateMissileWeapon(profile, true);
                AddRend(item);
                player.TryAddToInventory(item);
            }

            return player;
        }

        /// <summary>
        /// Creates a fully leveled 275 War Magic character player
        /// No augmentations are included
        /// </summary>
        public static Player Create275WarMagic(Weenie weenie, ObjectGuid guid, uint accountId, string name)
        {
            var characterCreateInfo = CreateCharacterCreateInfo(name, 10, 100, 10, 10, 100, 100);

            var player = Create275Base(characterCreateInfo, weenie, guid, accountId);

            // Trained skills
            player.TrainSkill(Skill.MeleeDefense, 10);
            player.TrainSkill(Skill.MissileDefense, 6);
            player.TrainSkill(Skill.Summoning, 8);
            player.TrainSkill(Skill.WarMagic, 16);

            // Specialized skills
            player.SpecializeSkill(Skill.LifeMagic, 8);
            player.SpecializeSkill(Skill.Summoning, 4);
            player.SpecializeSkill(Skill.WarMagic, 12);

            // 0 remaining skill points
            // If/When we add the 4 skill points in LevelUpPlayer, we can spend them here as well

            var foci = WorldObjectFactory.CreateNewWorldObject(15271); // Foci of Strife
            player.TryAddToInventory(foci);

            LoadSkillSpecificDefaultSpellBar(player);

            // todo aug what attribute?

            SpendAllXp(player);

            AddCommonInventory(player, AncientRelic);

            // Create a dummy treasure profile for passing in tier value
            var profile = new Database.Models.World.TreasureDeath
            {
                Tier = 7,
                LootQualityMod = 0
            };

            for (int i = 0; i < 12; i++)
            {
                var item = LootGenerationFactory.CreateCaster(profile, true, 1, true);
                AddRend(item);
                player.TryAddToInventory(item);
            }

            return player;
        }

        private static void SpendAllXp(Player player)
        {
            player.SpendAllXp(false);

            player.Health.Current = player.Health.MaxValue;
            player.Stamina.Current = player.Stamina.MaxValue;
            player.Mana.Current = player.Mana.MaxValue;
        }

        public static readonly HashSet<uint> CommonSpellComponents = new HashSet<uint> { 691, 689, 686, 688, 687, 690, 8897, 7299, 37155, 20631 };

        private static readonly HashSet<uint> NobleRelic = new HashSet<uint> { 33584, 33585, 33586, 33587, 33588 };
        private static readonly HashSet<uint> RelicAlduressa = new HashSet<uint> { 33574, 33575, 33576, 33577, 33578 };
        private static readonly HashSet<uint> AncientRelic = new HashSet<uint> { 33579, 33580, 33581, 33582, 33583 };

        private static void AddCommonInventory(Player player, params HashSet<uint>[] additionalGroups)
        {
            // MMD
            AddWeeniesToInventory(player, new List<uint> { 20630, 20630, 20630, 20630, 20630, 20630 });

            // Spell Components
            AddWeeniesToInventory(player, CommonSpellComponents);

            // Focusing Stone
            AddWeeniesToInventory(player, new HashSet<uint> { 8904 });

            AddWeeniesToInventory(player, new HashSet<uint> { 5893 }); // Hoary Robe
            AddWeeniesToInventory(player, new HashSet<uint> { 14594 }); // Helm of the Elements

            foreach (var group in additionalGroups)
                AddWeeniesToInventory(player, group);

            var orb = WorldObjectFactory.CreateNewWorldObject("Orb");
            orb.RemoveProperty(PropertyInt.PaletteTemplate);
            orb.RemoveProperty(PropertyFloat.Shade);
            orb.RemoveProperty(PropertyFloat.Shade2);
            orb.RemoveProperty(PropertyFloat.Shade3);
            orb.RemoveProperty(PropertyFloat.Shade4);
            orb.RemoveProperty(PropertyDataId.MutateFilter);
            orb.RemoveProperty(PropertyDataId.TsysMutationFilter);
            // biota_properties_int
            orb.UiEffects = UiEffects.Magical;
            orb.Bonded = BondedStatus.Bonded;
            orb.W_DamageType = DamageType.Slash;
            orb.TargetType = ItemType.Creature;
            orb.ItemSpellcraft = 325;
            orb.ItemCurMana = 1000;
            orb.ItemMaxMana = 1000;
            orb.ItemDifficulty = 280;
            orb.WieldRequirements = WieldRequirement.Skill;
            orb.WieldSkillType = 31;
            orb.WieldDifficulty = 355;
            // biota_properties_float
            orb.ManaRate = -0.033333;
            orb.WeaponDefense = 1.15;
            orb.ObjScale = 1.3f;
            orb.Translucency = 0.6f;
            orb.ManaConversionMod = 0.31;
            orb.ElementalDamageMod = 1.2;
            // biota_properties_d_i_d
            orb.Name = "Replica Drudge Scrying Orb";
            orb.LongDesc = "Same same, but different.";
            // biota_properties_d_i_d
            orb.SetupTableId = 33558259;
            orb.SoundTableId = 536870932;
            orb.PaletteBaseDID = 67111919;
            orb.IconId = 100674116;
            orb.PhysicsTableId = 872415275;
            orb.SpellDID = 2076;
            orb.IconUnderlayId = 100686604;
            // biota_properties_spell_book
            orb.Biota.GetOrAddKnownSpell(2076, orb.BiotaDatabaseLock, out _);
            orb.Biota.GetOrAddKnownSpell(2101, orb.BiotaDatabaseLock, out _);
            orb.Biota.GetOrAddKnownSpell(2242, orb.BiotaDatabaseLock, out _);
            orb.Biota.GetOrAddKnownSpell(2244, orb.BiotaDatabaseLock, out _);
            orb.Biota.GetOrAddKnownSpell(2507, orb.BiotaDatabaseLock, out _);
            orb.Biota.GetOrAddKnownSpell(2577, orb.BiotaDatabaseLock, out _);
            orb.Biota.GetOrAddKnownSpell(2581, orb.BiotaDatabaseLock, out _);
            player.TryAddToInventory(orb);

            // todo Buffing wand that has all defenses maxed
        }

        private static void AddWeeniesToInventory(Player player, IEnumerable<uint> weenieIds, ushort? stackSize = null)
        {
            foreach (uint weenieId in weenieIds)
            {
                var loot = WorldObjectFactory.CreateNewWorldObject(weenieId);

                if (loot == null) // weenie doesn't exist
                    continue;

                if (stackSize == null)
                    stackSize = loot.MaxStackSize;

                if (stackSize > 1)
                    loot.SetStackSize(stackSize);

                // Make sure the item is full of mana
                if (loot.ItemCurMana.HasValue)
                    loot.ItemCurMana = loot.ItemMaxMana;

                player.TryAddToInventory(loot);
            }
        }

        private static void AddAllSpells(Player player)
        {
            for (uint spellLevel = 1; spellLevel <= 8; spellLevel++)
            {
                player.LearnSpellsInBulk(MagicSchool.CreatureEnchantment, spellLevel, false);
                player.LearnSpellsInBulk(MagicSchool.ItemEnchantment, spellLevel, false);
                player.LearnSpellsInBulk(MagicSchool.LifeMagic, spellLevel, false);
                player.LearnSpellsInBulk(MagicSchool.VoidMagic, spellLevel, false);
                player.LearnSpellsInBulk(MagicSchool.WarMagic, spellLevel, false);
            }
        }

        private static void AddRend(WorldObject worldObject)
        {
            ImbuedEffectType imbuedEffectType = ImbuedEffectType.Undef;

            if (worldObject.W_DamageType == DamageType.Slash) imbuedEffectType = ImbuedEffectType.SlashRending;
            if (worldObject.W_DamageType == DamageType.Pierce) imbuedEffectType = ImbuedEffectType.PierceRending;
            if (worldObject.W_DamageType == DamageType.Bludgeon) imbuedEffectType = ImbuedEffectType.BludgeonRending;
            if (worldObject.W_DamageType == DamageType.Cold) imbuedEffectType = ImbuedEffectType.ColdRending;
            if (worldObject.W_DamageType == DamageType.Fire) imbuedEffectType = ImbuedEffectType.FireRending;
            if (worldObject.W_DamageType == DamageType.Acid) imbuedEffectType = ImbuedEffectType.AcidRending;
            if (worldObject.W_DamageType == DamageType.Electric) imbuedEffectType = ImbuedEffectType.ElectricRending;

            if (worldObject.W_DamageType == (DamageType.Slash | DamageType.Pierce)) imbuedEffectType = ImbuedEffectType.SlashRending;

            if (imbuedEffectType != ImbuedEffectType.Undef)
            {
                worldObject.SetProperty(PropertyInt.ImbuedEffect, (int)imbuedEffectType);
                worldObject.IconUnderlayId = RecipeManager.IconUnderlay[imbuedEffectType];
                worldObject.NumTimesTinkered++;
            }
        }


        public static void MakeSurePlayerHasFullStackForWeenies(Player player, HashSet<uint> weenieIds)
        {
            foreach (uint weenieId in weenieIds)
            {
                var amountFound = 0;
                var maxStackSize = 0;

                foreach (var item in player.GetAllPossessions())
                {
                    if (item.WeenieClassId == weenieId)
                    {
                        amountFound += item.StackSize ?? 1;
                        maxStackSize = item.MaxStackSize ?? 1;
                    }
                }

                if (amountFound > 0 && amountFound >= maxStackSize)
                    continue;

                var loot = WorldObjectFactory.CreateNewWorldObject(weenieId);

                if (loot == null) // weenie doesn't exist
                    continue;

                if (loot.MaxStackSize > 1)
                {
                    var amountToAdd = loot.MaxStackSize - amountFound;

                    loot.StackSize = amountToAdd;
                    loot.EncumbranceVal = (loot.StackUnitEncumbrance ?? 0) * (amountToAdd ?? 1);
                    loot.Value = (loot.StackUnitValue ?? 0) * (amountToAdd ?? 1);
                }

                player.TryAddToInventory(loot);
            }
        }
    }
}
