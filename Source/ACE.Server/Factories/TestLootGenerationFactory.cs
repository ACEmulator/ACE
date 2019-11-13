using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.Factories
{
    class TestLootGenerationFactory
    {
        public string Cantrip { get; }

        static void CantripSpells(string[] args)
        {
          
            var epicCantrips = new Dictionary<string, int>
            {
                ["Epic Bludgeon Ward"] = 3955,
                ["Epic Piercing Ward"] = 3956,
                ["Epic Slashing Ward"] = 3957,
                ["Epic Coordination"] = 3963,
                ["Epic Focus"] = 3964,
                ["Epic Strength"] = 3965,
                ["Epic Quickness"] = 4019,
                ["Epic Deception Prowess"] = 4020,
                ["Epic Endurance"] = 4226,
                ["Epic Willpower"] = 4227,
                ["Epic Leadership"] = 4232,
                ["Epic Acid Bane"] = 4660,
                ["Epic Blood Thirst"] = 4661,
                ["Epic Bludgeoning Bane"] = 4662,
                ["Epic Defender"] = 4663,
                ["Epic Flame Bane"] = 4664,
                ["Epic Frost Bane"] = 4665,
                ["Epic Heart Thirst"] = 4666,
                ["Epic Impenetrability"] = 4667,
                ["Epic Piercing Bane"] = 4668,
                ["Epic Slashing Bane"] = 4669,
                ["Epic Spirit Thirst"] = 4670,
                ["Epic Storm Bane"] = 4671,
                ["Epic Swift Hunter"] = 4672,
                ["Epic Acid Ward"] = 4673,
                ["Epic Bludgeoning Ward"] = 4674,
                ["Epic Flame Ward"] = 4675,
                ["Epic Frost Ward"] = 4676,
                ["Epic Piercing Ward"] = 4677,
                ["Epic Slashing Ward"] = 4678,
                ["Epic Storm Ward"] = 4679,
                ["Epic Health Gain"] = 4680,
                ["Epic Mana Gain"] = 4681,
                ["Epic Stamina Gain"] = 4682,
                ["Epic Alchemical Prowess"] = 4683,
                ["Epic Arcane Prowess"] = 4684,
                ["Epic Armor Tinkering Expertise"] = 4685,
                ["Epic Light Weapon Aptitude"] = 4686,
                ["Epic Missile Weapon Aptitude"] = 4687,
                ["Epic Cooking Prowess"] = 4688,
                ["Epic Creature Enchantment Aptitude"] = 4689,
                ["Epic Missile Weapon Aptitude"] = 4690,
                ["Epic Finesse Weapon Aptitude"] = 4691,
                ["Epic Fealty"] = 4692,
                ["Epic Fletching Prowess"] = 4693,
                ["Epic Healing Prowess"] = 4694,
                ["Epic Impregnability"] = 4695,
                ["Epic Invulnerability"] = 4696,
                ["Epic Item Enchantment Aptitude"] = 4697,
                ["Epic Item Tinkering Expertise"] = 4698,
                ["Epic Jumping Prowess"] = 4699,
                ["Epic Life Magic Aptitude"] = 4700,
                ["Epic Lockpick Prowess"] = 4701,
                ["Epic Light Weapon Aptitude"] = 4702,
                ["Epic Magic Item Tinkering Expertise"] = 4703,
                ["Epic Magic Resistance"] = 4704,
                ["Epic Mana Conversion Prowess"] = 4705,
                ["Epic Monster Attunement"] = 4706,
                ["Epic Person Attunement"] = 4707,
                ["Epic Salvaging Aptitude"] = 4708,
                ["Epic Light Weapon Aptitude"] = 4709,
                ["Epic Sprint"] = 4710,
                ["Epic Light Weapon Aptitude"] = 4711,
                ["Epic Heavy Weapon Aptitude"] = 4712,
                ["Epic Missile Weapon Aptitude"] = 4713,
                ["Epic Light Weapon Aptitude"] = 4714,
                ["Epic War Magic Aptitude"] = 4715,
                ["Epic Armor"] = 4911,
                ["Epic Weapon Tinkering Expertise"] = 4912,
                ["Epic Item Tinkering Expertise"] = 5033,
                ["Epic Two Handed Combat Aptitude"] = 5034,
                ["Epic Void Magic Aptitude"] = 5429,
                ["Epic Dirty Fighting Prowess"] = 5893,
                ["Epic Dual Wield Aptitude"] = 5894,
                ["Epic Recklessness Prowess"] = 5895,
                ["Epic Shield Aptitude"] = 5896,
                ["Epic Sneak Attack Prowess"] = 5897,
                ["Epic Hermetic Link"] = 6086,
                ["Epic Summoning Prowess"] = 6124


            };
            epicCantrips.TryGetValue(args, out)
        }
    }
}
