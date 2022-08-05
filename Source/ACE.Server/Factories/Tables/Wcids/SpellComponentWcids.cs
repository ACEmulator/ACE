using System.Collections.Generic;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class SpellComponentWcids
    {
        private static ChanceTable<WeenieClassName> T1_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.peascarablead,   1.00f ),
        };

        private static ChanceTable<WeenieClassName> T2_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.peascarablead,   0.50f ),
            ( WeenieClassName.peascarabiron,   0.50f ),
        };

        private static ChanceTable<WeenieClassName> T3_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.peascarablead,   0.25f ),
            ( WeenieClassName.peascarabiron,   0.50f ),
            ( WeenieClassName.peascarabcopper, 0.25f ),
        };

        private static ChanceTable<WeenieClassName> T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.peascarabiron,   0.25f ),
            ( WeenieClassName.peascarabcopper, 0.50f ),
            ( WeenieClassName.peascarabsilver, 0.25f ),
        };

        private static ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.peascarabcopper, 0.25f ),
            ( WeenieClassName.peascarabsilver, 0.50f ),
            ( WeenieClassName.peascarabgold,   0.25f ),
        };

        private static ChanceTable<WeenieClassName> T6_T8_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.peascarabsilver, 0.25f ),
            ( WeenieClassName.peascarabgold,   0.50f ),
            ( WeenieClassName.peascarabpyreal, 0.25f ),
        };

        private static readonly List<ChanceTable<WeenieClassName>> peaTiers = new List<ChanceTable<WeenieClassName>>()
        {
            T1_Chances,
            T2_Chances,
            T3_Chances,
            T4_Chances,
            T5_Chances,
            T6_T8_Chances,
            T6_T8_Chances,
            T6_T8_Chances,
        };

        // level 8 spell components have a chance of dropping in t7 / t8
        private static ChanceTable<bool> level8SpellComponentChance = new ChanceTable<bool>()
        {
            ( false, 0.6f ),
            ( true,  0.4f ),
        };

        public static WeenieClassName Roll(TreasureDeath profile)
        {
            // possible retail bug: peas not dropping in t6 / t7??
            if (profile.Tier >= 7)
            {
                // loot quality mod?
                // this could be helpful for mana forge chests -- did they drop peas?
                var level8SpellComponent = level8SpellComponentChance.Roll(profile.LootQualityMod);

                if (level8SpellComponent)
                    return Roll_Level8SpellComponent(profile);
            }

            var table = peaTiers[profile.Tier - 1];

            return table.Roll(profile.LootQualityMod);
        }

        private static ChanceTable<WeenieClassName> Quills = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.ace37363_quillofinfliction,    0.50f ),   // war/debuff (other)
            ( WeenieClassName.ace37364_quillofintrospection, 0.35f ),   // beneficial (self)
            ( WeenieClassName.ace37365_quillofbenevolence,   0.10f ),   // beneficial (other)
            ( WeenieClassName.ace37362_quillofextraction,    0.05f ),   // drain (other)
        };

        private static ChanceTable<WeenieClassName> Inks = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.ace37353_inkofformation,       0.30f ),   // self (can only be used with introspection quills)
            ( WeenieClassName.ace37360_inkofconveyance,      0.24f ),   // other (can only be used with benevolence, infliction, and extraction quills)
            ( WeenieClassName.ace37355_inkofobjectification, 0.10f ),   // item spells
            ( WeenieClassName.ace37354_inkofnullification,   0.06f ),   // nullify (can only be used with introspection or benevolence quills)
            // war spells
            ( WeenieClassName.ace37356_parabolicink,         0.06f ),   // arc
            ( WeenieClassName.ace37357_inkofpartition,       0.06f ),   // blast
            ( WeenieClassName.ace37358_inkofseparation,      0.06f ),   // volley
            ( WeenieClassName.ace37359_alacritousink,        0.06f ),   // streak
            ( WeenieClassName.ace37361_inkofdirection,       0.06f ),   // bolt
        };

        private static readonly List<WeenieClassName> Glyphs = new List<WeenieClassName>()
        {
            WeenieClassName.ace37343_glyphofalchemy,
            WeenieClassName.ace37344_glyphofarcanelore,
            WeenieClassName.ace37345_glyphofarmor,
            WeenieClassName.ace37346_glyphofarmortinkering,
            WeenieClassName.ace37347_glyphofbludgeoning,
            WeenieClassName.ace37349_glyphofcooking,
            WeenieClassName.ace37350_glyphofcoordination,
            WeenieClassName.ace37342_glyphofcorrosion,
            WeenieClassName.ace37351_glyphofcreatureenchantment,
            WeenieClassName.ace43379_glyphofdamage,
            WeenieClassName.ace37352_glyphofdeception,
            WeenieClassName.ace45370_glyphofdirtyfighting,
            WeenieClassName.ace45371_glyphofdualwield,
            WeenieClassName.ace37300_glyphofendurance,
            WeenieClassName.ace37373_glyphoffinesseweapons,
            WeenieClassName.ace37301_glyphofflame,
            WeenieClassName.ace37302_glyphoffletching,
            WeenieClassName.ace37303_glyphoffocus,
            WeenieClassName.ace37348_glyphoffrost,
            WeenieClassName.ace37304_glyphofhealing,
            WeenieClassName.ace37305_glyphofhealth,
            WeenieClassName.ace37369_glyphofheavyweapons,
            WeenieClassName.ace37309_glyphofitemenchantment,
            WeenieClassName.ace37310_glyphofitemtinkering,
            WeenieClassName.ace37311_glyphofjump,
            WeenieClassName.ace37312_glyphofleadership,
            WeenieClassName.ace37313_glyphoflifemagic,
            WeenieClassName.ace37339_glyphoflightweapons,
            WeenieClassName.ace37314_glyphoflightning,
            WeenieClassName.ace37315_glyphoflockpick,
            WeenieClassName.ace37316_glyphofloyalty,
            WeenieClassName.ace37317_glyphofmagicdefense,
            WeenieClassName.ace38760_glyphofmagicitemtinkering,
            WeenieClassName.ace37318_glyphofmana,
            WeenieClassName.ace37319_glyphofmanaconversion,
            WeenieClassName.ace37321_glyphofmanaregeneration,
            WeenieClassName.ace37323_glyphofmeleedefense,
            WeenieClassName.ace37324_glyphofmissiledefense,
            WeenieClassName.ace37338_glyphofmissileweapons,
            WeenieClassName.ace37325_glyphofmonsterappraisal,
            WeenieClassName.ace43387_glyphofnether,
            WeenieClassName.ace37326_glyphofpersonappraisal,
            WeenieClassName.ace37327_glyphofpiercing,
            WeenieClassName.ace37328_glyphofquickness,
            WeenieClassName.ace45372_glyphofrecklessness,
            WeenieClassName.ace37307_glyphofregeneration,
            WeenieClassName.ace37329_glyphofrun,
            WeenieClassName.ace37330_glyphofsalvaging,
            WeenieClassName.ace37331_glyphofself,
            WeenieClassName.ace45373_glyphofshield,
            WeenieClassName.ace37332_glyphofslashing,
            WeenieClassName.ace45374_glyphofsneakattack,
            WeenieClassName.ace37333_glyphofstamina,
            WeenieClassName.ace37336_glyphofstaminaregeneration,
            WeenieClassName.ace37337_glyphofstrength,
            WeenieClassName.ace49455_glyphofsummoning,
            WeenieClassName.ace41747_glyphoftwohandedcombat,
            WeenieClassName.ace43380_glyphofvoidmagic,
            WeenieClassName.ace37340_glyphofwarmagic,
            WeenieClassName.ace37341_glyphofweapontinkering,
        };

        private static WeenieClassName Roll_Level8SpellComponent(TreasureDeath profile)
        {
            // even chance between quill / ink / glyph
            var type = (Level8_SpellComponentType)ThreadSafeRandom.Next(1, 3);

            switch (type)
            {
                // quality mod?
                case Level8_SpellComponentType.Quill:
                    return Quills.Roll(profile.LootQualityMod);

                case Level8_SpellComponentType.Ink:
                    return Inks.Roll(profile.LootQualityMod);

                case Level8_SpellComponentType.Glyph:

                    // even chance for each glyph
                    var rng = ThreadSafeRandom.Next(0, Glyphs.Count - 1);
                    return Glyphs[rng];
            }

            return WeenieClassName.undef;
        }
    }
}
