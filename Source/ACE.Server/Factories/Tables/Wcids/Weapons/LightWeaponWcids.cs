using System.Collections.Generic;

using ACE.Common;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class LightWeaponWcids
    {
        private static readonly List<WeenieClassName> Dolabras = new List<WeenieClassName>()
        {
            // light - axe
            WeenieClassName.axedolabra,
            WeenieClassName.axedolabraacid,
            WeenieClassName.axedolabraelectric,
            WeenieClassName.axedolabrafire,
            WeenieClassName.axedolabrafrost,
        };

        private static readonly List<WeenieClassName> HandAxes = new List<WeenieClassName>()
        {
            // light - axe
            WeenieClassName.axehand,
            WeenieClassName.axehandacid,
            WeenieClassName.axehandelectric,
            WeenieClassName.axehandfire,
            WeenieClassName.axehandfrost,
        };

        private static readonly List<WeenieClassName> Onos = new List<WeenieClassName>()
        {
            // light - axe
            WeenieClassName.ono,
            WeenieClassName.onoacid,
            WeenieClassName.onoelectric,
            WeenieClassName.onofire,
            WeenieClassName.onofrost,
        };

        private static readonly List<WeenieClassName> WarHammers = new List<WeenieClassName>()
        {
            // light - axe
            WeenieClassName.warhammer,
            WeenieClassName.warhammeracid,
            WeenieClassName.warhammerelectric,
            WeenieClassName.warhammerfire,
            WeenieClassName.warhammerfrost,
        };

        private static readonly List<WeenieClassName> Daggers = new List<WeenieClassName>()
        {
            // light - dagger
            WeenieClassName.dagger,
            WeenieClassName.daggeracid,
            WeenieClassName.daggerelectric,
            WeenieClassName.daggerfire,
            WeenieClassName.daggerfrost,
        };

        private static readonly List<WeenieClassName> Khanjars = new List<WeenieClassName>()
        {
            // light - dagger
            WeenieClassName.khanjar,
            WeenieClassName.khanjaracid,
            WeenieClassName.khanjarelectric,
            WeenieClassName.khanjarfire,
            WeenieClassName.khanjarfrost,
        };

        private static readonly List<WeenieClassName> Clubs = new List<WeenieClassName>()
        {
            // light - mace
            WeenieClassName.club,
            WeenieClassName.clubacid,
            WeenieClassName.clubelectric,
            WeenieClassName.clubfire,
            WeenieClassName.clubfrost,
        };

        private static readonly List<WeenieClassName> Kasrullahs = new List<WeenieClassName>()
        {
            // light - mace
            WeenieClassName.kasrullah,
            WeenieClassName.kasrullahacid,
            WeenieClassName.kasrullahelectric,
            WeenieClassName.kasrullahfire,
            WeenieClassName.kasrullahfrost,
        };

        private static readonly List<WeenieClassName> SpikedClubs = new List<WeenieClassName>()
        {
            // light - mace
            WeenieClassName.clubspiked,
            WeenieClassName.clubspikedacid,
            WeenieClassName.clubspikedelectric,
            WeenieClassName.clubspikedfire,
            WeenieClassName.clubspikedfrost,
        };

        private static readonly List<WeenieClassName> Spears = new List<WeenieClassName>()
        {
            // light - spear
            WeenieClassName.spear,
            WeenieClassName.spearacid,
            WeenieClassName.spearelectric,
            WeenieClassName.spearflame,
            WeenieClassName.spearfrost,
        };

        private static readonly List<WeenieClassName> Yaris = new List<WeenieClassName>()
        {
            // light - spear
            WeenieClassName.yari,
            WeenieClassName.yariacid,
            WeenieClassName.yarielectric,
            WeenieClassName.yarifire,
            WeenieClassName.yarifrost,
        };

        private static readonly List<WeenieClassName> QuarterStaffs = new List<WeenieClassName>()
        {
            // light - staff
            WeenieClassName.quarterstaffnew,
            WeenieClassName.quarterstaffacidnew,
            WeenieClassName.quarterstaffelectricnew,
            WeenieClassName.quarterstaffflamenew,
            WeenieClassName.quarterstafffrostnew,
        };

        private static readonly List<WeenieClassName> BroadSwords = new List<WeenieClassName>()
        {
            // light - sword
            WeenieClassName.swordbroad,
            WeenieClassName.swordbroadacid,
            WeenieClassName.swordbroadelectric,
            WeenieClassName.swordbroadfire,
            WeenieClassName.swordbroadfrost,
        };

        private static readonly List<WeenieClassName> DericostBlades = new List<WeenieClassName>()
        {
            // light - sword
            WeenieClassName.ace31759_dericostblade,
            WeenieClassName.ace31760_aciddericostblade,
            WeenieClassName.ace31761_lightningdericostblade,
            WeenieClassName.ace31762_flamingdericostblade,
            WeenieClassName.ace31758_frostdericostblade,
        };

        // epee?

        private static readonly List<WeenieClassName> Kaskaras = new List<WeenieClassName>()
        {
            // light - sword
            WeenieClassName.kaskara,
            WeenieClassName.kaskaraacid,
            WeenieClassName.kaskaraelectric,
            WeenieClassName.kaskarafire,
            WeenieClassName.kaskarafrost,
        };

        private static readonly List<WeenieClassName> Shamshirs = new List<WeenieClassName>()
        {
            // light - sword
            WeenieClassName.shamshir,
            WeenieClassName.shamshiracid,
            WeenieClassName.shamshirelectric,
            WeenieClassName.shamshirfire,
            WeenieClassName.shamshirfrost,
        };

        private static readonly List<WeenieClassName> Spadas = new List<WeenieClassName>()
        {
            // light - sword
            WeenieClassName.swordspada,
            WeenieClassName.swordspadaacid,
            WeenieClassName.swordspadaelectric,
            WeenieClassName.swordspadafire,
            WeenieClassName.swordspadafrost,
        };

        private static readonly List<WeenieClassName> Katars = new List<WeenieClassName>()
        {
            // light - unarmed
            WeenieClassName.katar,
            WeenieClassName.kataracid,
            WeenieClassName.katarelectric,
            WeenieClassName.katarfire,
            WeenieClassName.katarfrost,
        };

        private static readonly List<WeenieClassName> Knuckles = new List<WeenieClassName>()
        {
            // light - unarmed
            WeenieClassName.knuckles,
            WeenieClassName.knucklesacid,
            WeenieClassName.knuckleselectric,
            WeenieClassName.knucklesfire,
            WeenieClassName.knucklesfrost,
        };

        private static readonly List<List<WeenieClassName>> lightWeaponsTables = new List<List<WeenieClassName>>()
        {
            Dolabras,
            HandAxes,
            Onos,
            WarHammers,
            Daggers,
            Khanjars,
            Clubs,
            Kasrullahs,
            SpikedClubs,
            Spears,
            Yaris,
            QuarterStaffs,
            BroadSwords,
            DericostBlades,
            Kaskaras,
            Shamshirs,
            Spadas,
            Katars,
            Knuckles,
        };

        public static WeenieClassName Roll()
        {
            // even chance of selecting each weapon type
            var weaponType = ThreadSafeRandom.Next(0, lightWeaponsTables.Count - 1);

            var weaponTable = lightWeaponsTables[weaponType];

            // 50/50 chance of selecting elemental/non-elemental
            // could have been 40/60 in retail?
            var elemental = ThreadSafeRandom.NextBool();

            if (elemental)
            {
                var elementType = ThreadSafeRandom.Next(1, 4);
                return weaponTable[elementType];
            }
            else
                return weaponTable[0];
        }
    }
}
